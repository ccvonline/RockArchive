﻿// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using Rock.Web.UI;
using Rock.Security;

namespace RockWeb.Blocks.Finance
{
    /// <summary>
    /// 
    /// </summary>
    [DisplayName( "Check Matching Detail" )]
    [Category( "Finance" )]
    [Description( "Used to match checks to an individual and allocate the check amount to financial account(s)." )]

    [AccountsField( "Accounts", "Select the funds that check amounts can be allocated to.  Leave blank to show all accounts" )]
    public partial class CheckMatchingDetail : RockBlock, IDetailBlock
    {
        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                hfBackNextHistory.Value = string.Empty;
                LoadDropDowns();
                ShowDetail( PageParameter( "FinancialBatchId" ).AsInteger() );
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        public void LoadDropDowns()
        {
            var rockContext = new RockContext();
            var accountGuidList = GetAttributeValue( "Accounts" ).SplitDelimitedValues().Select( a => a.AsGuid() );

            // TODO personal accounts filter

            var accountQry = new FinancialAccountService( rockContext ).Queryable();

            if ( accountGuidList.Any() )
            {
                accountQry = accountQry.Where( a => accountGuidList.Contains( a.Guid ) );
            }

            rptAccounts.DataSource = accountQry.OrderBy( a => a.Order ).ThenBy( a => a.Name ).ToList();
            rptAccounts.DataBind();
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="financialBatchId">The financial batch identifier.</param>
        public void ShowDetail( int financialBatchId )
        {
            hfFinancialBatchId.Value = financialBatchId.ToString();
            hfTransactionId.Value = string.Empty;

            NavigateToTransaction( Direction.Next);
        }

        /// <summary>
        /// 
        /// </summary>
        private enum Direction
        {
            Prev,
            Next
        }

        /// <summary>
        /// Navigates to the next (or previous) transaction to edit
        /// </summary>
        private void NavigateToTransaction( Direction direction)
        {
            int? transactionId = hfTransactionId.Value.AsInteger();
            
            if (direction == Direction.Prev)
            {
                var historyList = hfBackNextHistory.Value.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ).Select( a => a.AsInteger() ).Where( a => a > 0 ).ToList();
                
                int position = historyList.IndexOf( transactionId ?? 0 );
                if ( position > 0 )
                {
                    transactionId = historyList[position - 1];
                }
                else
                {
                    transactionId = null;
                }

                hfTransactionId.Value = transactionId.ToString();
            }
            else
            {
                // TODO
                var historyList = hfBackNextHistory.Value.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ).Select( a => a.AsInteger() ).Where( a => a > 0 ).ToList();

                // figure out which transaction to navigate to (or set to NULL if we need to look for the next one in the database)
                int position = historyList.IndexOf( transactionId.Value );
                if ( position < 0 )
                {
                    historyList.Add( transactionId.Value );
                    transactionId = null;
                }
                else
                {
                    if ( position < historyList.Count )
                    {
                        transactionId = historyList[position + 1];
                    }
                    else
                    {
                        transactionId = null;
                    }
                }

                hfBackNextHistory.Value = historyList.AsDelimited( "," );
            }

            // TODO fix up navigation logic, etc....

            
            int financialBatchId = hfFinancialBatchId.Value.AsInteger();
            var rockContext = new RockContext();
            var financialPersonBankAccountService = new FinancialPersonBankAccountService( rockContext );
            var financialTransactionService = new FinancialTransactionService( rockContext );
            var qryTransactionsToMatch = financialTransactionService.Queryable()
                .Where( a => a.AuthorizedPersonId == null );

            // if a specific transactionId was specified load that one. Otherwise, if a batch is specified, get the first unmatched transaction in that batch
            if ( transactionId.HasValue )
            {
                qryTransactionsToMatch = qryTransactionsToMatch.Where( a => a.Id == transactionId );
            }
            else if ( financialBatchId != 0 )
            {
                qryTransactionsToMatch = qryTransactionsToMatch.Where( a => a.BatchId == financialBatchId );
            }

            qryTransactionsToMatch = qryTransactionsToMatch.OrderBy( a => a.CreatedDateTime ).ThenBy( a => a.Id );

            // TODO add logic for ProcessedBy...
            FinancialTransaction firstTransactionToMatch = qryTransactionsToMatch.FirstOrDefault();
            if ( firstTransactionToMatch == null )
            {
                // TODO if no matches are left remove the limit of ProcessedBy and present a warning if there are InProcess ones that need to be finished
            }

            nbNoUnmatchedTransactionsRemaining.Visible = firstTransactionToMatch == null;
            pnlEdit.Visible = firstTransactionToMatch != null;
            if ( firstTransactionToMatch != null )
            {
                var transactionToMatch = firstTransactionToMatch;
                hfTransactionId.Value = transactionToMatch.Id.ToString();
                int frontImageTypeId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.TRANSACTION_IMAGE_TYPE_CHECK_FRONT.AsGuid() ).Id;
                int backImageTypeId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.TRANSACTION_IMAGE_TYPE_CHECK_BACK.AsGuid() ).Id;
                var frontImage = transactionToMatch.Images.Where( a => a.TransactionImageTypeValueId == frontImageTypeId ).FirstOrDefault();
                var backImage = transactionToMatch.Images.Where( a => a.TransactionImageTypeValueId == backImageTypeId ).FirstOrDefault();

                string checkMicrHashed = null;
                var checkMicrClearText = Encryption.DecryptString( transactionToMatch.CheckMicrEncrypted );
                var parts = checkMicrClearText.Split( '_' );
                if ( parts.Length >= 2 )
                {
                    checkMicrHashed = FinancialPersonBankAccount.EncodeAccountNumber( parts[0], parts[1] );
                }

                hfCheckMicrHashed.Value = checkMicrHashed;

                var matchedPersons = financialPersonBankAccountService.Queryable().Where( a => a.AccountNumberSecured == checkMicrHashed ).Select( a => a.Person );
                ddlIndividual.Items.Clear();
                foreach ( var person in matchedPersons.OrderBy( a => a.LastName ).ThenBy( a => a.NickName ) )
                {
                    ddlIndividual.Items.Add( new ListItem( person.FullNameReversed, person.Id.ToString() ) );
                }

                if ( ddlIndividual.Items.Count == 0 )
                {
                    ddlIndividual.SelectedIndex = 0;
                }
                else
                {
                    ddlIndividual.SelectedIndex = -1;
                }

                string frontCheckUrl = string.Empty;
                string backCheckUrl = string.Empty;

                if ( frontImage != null )
                {
                    frontCheckUrl = string.Format( "~/GetImage.ashx?id={0}", frontImage.BinaryFileId.ToString() );
                }

                if ( backImage != null )
                {
                    backCheckUrl = string.Format( "~/GetImage.ashx?id={0}", backImage.BinaryFileId.ToString() );
                }

                imgCheck.Visible = !string.IsNullOrEmpty( frontCheckUrl ) || !string.IsNullOrEmpty( backCheckUrl ); ;
                imgCheckOtherSideThumbnail.Visible = imgCheck.Visible;
                nbNoCheckImageWarning.Visible = !imgCheck.Visible;
                imgCheck.ImageUrl = frontCheckUrl;
                imgCheckOtherSideThumbnail.ImageUrl = backCheckUrl;
            }
            else
            {
                hfTransactionId.Value = string.Empty;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            // TODO
        }

        protected void mdAccountsPersonalFilter_SaveClick( object sender, EventArgs e )
        {
            // TODO
            mdAccountsPersonalFilter.Hide();
        }

        protected void btnFilter_Click( object sender, EventArgs e )
        {
            // TODO
            mdAccountsPersonalFilter.Show();
        }

        /// <summary>
        /// Handles the Click event of the btnPrevious control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnPrevious_Click( object sender, EventArgs e )
        {
            NavigateToTransaction( Direction.Prev );

            
        }

        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnNext_Click( object sender, EventArgs e )
        {
            NavigateToTransaction( Direction.Next );
        }

        protected void rptAccounts_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            // TODO
        }

        #endregion
    }
}