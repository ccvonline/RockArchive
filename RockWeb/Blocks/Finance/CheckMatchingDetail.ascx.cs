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
using Rock.Web.UI;
using Rock.Security;
using Rock.Web;

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
                ShowDetail( PageParameter( "BatchId" ).AsInteger() );
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Unload" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains event data.</param>
        protected override void OnUnload( EventArgs e )
        {
            base.OnUnload( e );

            if ( !Page.IsPostBack )
            {
                MarkTransactionAsNotProcessedByCurrentUser( hfTransactionId.Value.AsInteger() );
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
        /// <param name="batchId">The financial batch identifier.</param>
        public void ShowDetail( int batchId )
        {
            hfBatchId.Value = batchId.ToString();
            hfTransactionId.Value = string.Empty;

            NavigateToTransaction( Direction.Next );
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
        private void NavigateToTransaction( Direction direction )
        {
            nbSaveError.Visible = false;
            int? fromTransactionId = hfTransactionId.Value.AsIntegerOrNull();
            int? toTransactionId = null;
            List<int> historyList = hfBackNextHistory.Value.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ).Select( a => a.AsInteger() ).Where( a => a > 0 ).ToList();
            int position = hfHistoryPosition.Value.AsIntegerOrNull() ?? -1;

            if ( direction == Direction.Prev )
            {
                position--;
            }
            else
            {
                position++;
            }

            if ( historyList.Count > position )
            {
                if ( position >= 0 )
                {
                    toTransactionId = historyList[position];
                }
                else
                {
                    // if we trying to go previous when we are already at the start of the list, wrap around to the last item in the list
                    toTransactionId = historyList.Last();
                    position = historyList.Count - 1;
                }
            }

            hfHistoryPosition.Value = position.ToString();

            // TODO fix up edit/display logic, etc....

            int batchId = hfBatchId.Value.AsInteger();
            var rockContext = new RockContext();
            var financialPersonBankAccountService = new FinancialPersonBankAccountService( rockContext );
            var financialTransactionService = new FinancialTransactionService( rockContext );
            var qryTransactionsToMatch = financialTransactionService.Queryable()
                .Where( a => a.AuthorizedPersonId == null && a.ProcessedByPersonAliasId == null);

            // if a specific transactionId was specified load that one. Otherwise, if a batch is specified, get the first unmatched transaction in that batch
            if ( toTransactionId.HasValue )
            {
                qryTransactionsToMatch = qryTransactionsToMatch.Where( a => a.Id == toTransactionId );
            }
            else if ( batchId != 0 )
            {
                qryTransactionsToMatch = qryTransactionsToMatch.Where( a => a.BatchId == batchId );
            }

            if ( historyList.Any() && !toTransactionId.HasValue )
            {
                // since we are looking for a transaction we haven't viewed or matched yet, look for the next one in the database that we haven't seen yet
                qryTransactionsToMatch = qryTransactionsToMatch.Where( a => !historyList.Contains( a.Id ) );
            }

            qryTransactionsToMatch = qryTransactionsToMatch.OrderBy( a => a.CreatedDateTime ).ThenBy( a => a.Id );

            // TODO add logic for ProcessedBy...
            FinancialTransaction transactionToMatch = qryTransactionsToMatch.FirstOrDefault();
            if ( transactionToMatch == null )
            {
                // TODO if no matches are left remove the limit of ProcessedBy and present a warning if there are InProcess ones that need to be finished
                var qryRemainingTransactionsToMatch = financialTransactionService.Queryable().Where( a => a.AuthorizedPersonId == null );
                if ( batchId != 0 )
                {
                    qryRemainingTransactionsToMatch = qryRemainingTransactionsToMatch.Where( a => a.BatchId == batchId );
                }

                transactionToMatch = qryRemainingTransactionsToMatch.Where( a => a.Id > fromTransactionId ).FirstOrDefault() ?? qryRemainingTransactionsToMatch.FirstOrDefault();
                if ( transactionToMatch != null )
                {
                    historyList.Add( transactionToMatch.Id );
                    position = historyList.LastIndexOf( transactionToMatch.Id );
                    hfHistoryPosition.Value = position.ToString();
                }
            }
            else
            {
                if ( !toTransactionId.HasValue )
                {
                    historyList.Add( transactionToMatch.Id );
                }
            }

            nbNoUnmatchedTransactionsRemaining.Visible = transactionToMatch == null;
            pnlEdit.Visible = transactionToMatch != null;
            nbIsInProcess.Visible = false;
            if ( transactionToMatch != null )
            {
                if ( transactionToMatch.ProcessedByPersonAlias != null )
                {
                    nbIsInProcess.Visible = true;
                    if ( transactionToMatch.AuthorizedPersonId.HasValue )
                    {
                        nbIsInProcess.Text = string.Format( "Warning. This check was matched by {0} at {1} ({2})", transactionToMatch.ProcessedByPersonAlias, transactionToMatch.ProcessedDateTime.ToString(), transactionToMatch.ProcessedDateTime.ToRelativeDateString() );
                    }
                    else
                    {
                        nbIsInProcess.Text = string.Format( "Warning. This check is getting processed by {0} as of {1} ({2})", transactionToMatch.ProcessedByPersonAlias, transactionToMatch.ProcessedDateTime.ToString(), transactionToMatch.ProcessedDateTime.ToRelativeDateString() );
                    }
                }

                // Unless somebody else is processing it, immediately mark the transaction as getting processed by the current person so that other potentional check matching sessions will know that it is currently getting looked at
                if ( !transactionToMatch.ProcessedByPersonAliasId.HasValue )
                {
                    transactionToMatch.ProcessedByPersonAlias = null;
                    transactionToMatch.ProcessedByPersonAliasId = this.CurrentPersonAlias.Id;
                    transactionToMatch.ProcessedDateTime = RockDateTime.Now;
                    rockContext.SaveChanges();
                }

                var descriptionList = new DescriptionList();
                descriptionList
                    .Add( "Date", transactionToMatch.TransactionDateTime )
                    .Add( "Id", transactionToMatch.Id );

                lTransactionInfo.Text = descriptionList.Html;
                hfTransactionId.Value = transactionToMatch.Id.ToString();
                int frontImageTypeId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.TRANSACTION_IMAGE_TYPE_CHECK_FRONT.AsGuid() ).Id;
                int backImageTypeId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.TRANSACTION_IMAGE_TYPE_CHECK_BACK.AsGuid() ).Id;
                var frontImage = transactionToMatch.Images.Where( a => a.TransactionImageTypeValueId == frontImageTypeId ).FirstOrDefault();
                var backImage = transactionToMatch.Images.Where( a => a.TransactionImageTypeValueId == backImageTypeId ).FirstOrDefault();

                string checkMicrHashed = null;

                if ( !string.IsNullOrWhiteSpace( transactionToMatch.CheckMicrEncrypted ) )
                {
                    try
                    {
                        var checkMicrClearText = Encryption.DecryptString( transactionToMatch.CheckMicrEncrypted );
                        var parts = checkMicrClearText.Split( '_' );
                        if ( parts.Length >= 2 )
                        {
                            checkMicrHashed = FinancialPersonBankAccount.EncodeAccountNumber( parts[0], parts[1] );
                        }
                    }
                    catch
                    {
                        // intentionally ignore exception when decripting CheckMicrEncrypted since we'll be checking for null below
                    }
                }

                nbNoMicrWarning.Visible = string.IsNullOrWhiteSpace( checkMicrHashed );

                hfCheckMicrHashed.Value = checkMicrHashed;

                if ( !string.IsNullOrWhiteSpace( checkMicrHashed ) )
                {
                    var matchedPersons = financialPersonBankAccountService.Queryable().Where( a => a.AccountNumberSecured == checkMicrHashed ).Select( a => a.Person );
                    ddlIndividual.Items.Clear();
                    foreach ( var person in matchedPersons.OrderBy( a => a.LastName ).ThenBy( a => a.NickName ) )
                    {
                        ddlIndividual.Items.Add( new ListItem( person.FullNameReversed, person.Id.ToString() ) );
                    }
                }
                else
                {
                    // TODO warn about missing MICR
                }

                if ( ddlIndividual.Items.Count == 1 )
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

                if ( transactionToMatch.AuthorizedPersonId.HasValue )
                {
                    ddlIndividual.SelectedValue = transactionToMatch.AuthorizedPersonId.ToString();
                }

                ppSelectNew.PersonId = null;
                if ( transactionToMatch.TransactionDetails.Any() )
                {
                    cbAmount.Text = transactionToMatch.TotalAmount.ToString();
                }
                else
                {
                    cbAmount.Text = string.Empty;
                }

                // update accountboxes
                foreach ( var accountBox in rptAccounts.ControlsOfTypeRecursive<CurrencyBox>() )
                {
                    accountBox.Text = string.Empty;
                }

                foreach ( var detail in transactionToMatch.TransactionDetails )
                {
                    var accountBox = rptAccounts.ControlsOfTypeRecursive<CurrencyBox>().Where( a => a.Attributes["data-account-id"].AsInteger() == detail.AccountId ).FirstOrDefault();
                    if ( accountBox != null )
                    {
                        accountBox.Text = detail.Amount.ToString();
                    }
                }

                cbRemaining.Text = "0.00";

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

            hfBackNextHistory.Value = historyList.AsDelimited( "," );

            // DEBUG bookmarks
            lBookmarkDebug.Text = string.Empty;

            for ( int i = 0; i < historyList.Count; i++ )
            {
                var item = historyList[i];
                if ( i == position )
                {
                    lBookmarkDebug.Text += "|<u>" + item.ToString() + "</u>";
                }
                else
                {
                    lBookmarkDebug.Text += "|" + item.ToString();
                }
            }

            lBookmarkDebug.Text = lBookmarkDebug.Text.TrimStart( new char[] { '|' } );
        }

        #endregion

        #region Events

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
        /// Marks the transaction as not processed by the current user
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        private void MarkTransactionAsNotProcessedByCurrentUser( int transactionId )
        {
            var rockContext = new RockContext();
            var financialTransactionService = new FinancialTransactionService( rockContext );
            var financialTransaction = financialTransactionService.Get( transactionId );

            if ( financialTransaction != null && financialTransaction.ProcessedByPersonAliasId == this.CurrentPersonAlias.Id && financialTransaction.AuthorizedPersonId == null )
            {
                // if the current user marked this as processed, and it wasn't matched, clear out the processedby fields.  Otherwise, assume the other person is still editing it
                financialTransaction.ProcessedByPersonAliasId = null;
                financialTransaction.ProcessedDateTime = null;
                rockContext.SaveChanges();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnPrevious control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnPrevious_Click( object sender, EventArgs e )
        {
            // if the transaction was not matched, clear out the ProcessedBy fields since we didn't match the check and are moving on to process another transaction
            MarkTransactionAsNotProcessedByCurrentUser( hfTransactionId.Value.AsInteger() );

            NavigateToTransaction( Direction.Prev );
        }

        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnNext_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            var financialTransactionService = new FinancialTransactionService( rockContext );
            var financialTransactionDetailService = new FinancialTransactionDetailService( rockContext );
            var financialPersonBankAccountService = new FinancialPersonBankAccountService( rockContext );
            var financialTransaction = financialTransactionService.Get( hfTransactionId.Value.AsInteger() );

            // set the AuthorizedPersonId (the person who wrote the check) to the if the SelectNew person (if selected) or person selected in the drop down (if there is somebody selected)
            int? authorizedPersonId = ppSelectNew.PersonId ?? ddlIndividual.SelectedValue.AsIntegerOrNull();

            var accountNumberSecured = hfCheckMicrHashed.Value;

            // if the transaction is matched to somebody, attempt to save it
            if ( financialTransaction != null && authorizedPersonId.HasValue )
            {
                if ( string.IsNullOrWhiteSpace( accountNumberSecured ) )
                {
                    // should be showing already, but just in case
                    nbNoMicrWarning.Visible = true;
                    return;
                }

                if ( cbAmount.Text.AsDecimalOrNull() == null )
                {
                    nbSaveError.Text = "Total Amount is required.";
                    nbSaveError.Visible = true;
                    return;
                }

                if ( cbRemaining.Text.AsDecimalOrNull() != 0 )
                {
                    nbSaveError.Text = "Total amount must be fully allocated to accounts.";
                    nbSaveError.Visible = true;
                    return;
                }

                var financialPersonBankAccount = financialPersonBankAccountService.Queryable().Where( a => a.AccountNumberSecured == accountNumberSecured && a.PersonId == authorizedPersonId ).FirstOrDefault();
                if ( financialPersonBankAccount == null )
                {
                    financialPersonBankAccount = new FinancialPersonBankAccount();
                    financialPersonBankAccount.PersonId = authorizedPersonId.Value;
                    financialPersonBankAccount.AccountNumberSecured = accountNumberSecured;
                    financialPersonBankAccountService.Add( financialPersonBankAccount );
                }

                financialTransaction.AuthorizedPersonId = authorizedPersonId;

                // just in case this transaction is getting re-edited either by the same user, or somebody else, clean out any existing TransactionDetail records
                foreach ( var detail in financialTransaction.TransactionDetails )
                {
                    financialTransactionDetailService.Delete( detail );
                }

                foreach ( var accountBox in rptAccounts.ControlsOfTypeRecursive<CurrencyBox>() )
                {
                    var amount = accountBox.Text.AsDecimalOrNull();

                    if ( amount.HasValue && amount.Value >= 0 )
                    {
                        var financialTransactionDetail = new FinancialTransactionDetail();
                        financialTransactionDetail.TransactionId = financialTransaction.Id;
                        financialTransactionDetail.AccountId = accountBox.Attributes["data-account-id"].AsInteger();
                        financialTransactionDetail.Amount = amount.Value;
                        financialTransactionDetailService.Add( financialTransactionDetail );
                    }
                }

                financialTransaction.ProcessedByPersonAliasId = this.CurrentPersonAlias.Id;
                financialTransaction.ProcessedDateTime = RockDateTime.Now;

                rockContext.SaveChanges();
            }
            else
            {
                // if the transaction was not matched, clear out the ProcessedBy fields since we didn't match the check and are moving on to process another transaction
                MarkTransactionAsNotProcessedByCurrentUser( hfTransactionId.Value.AsInteger() );
            }

            NavigateToTransaction( Direction.Next );
        }

        #endregion
    }
}