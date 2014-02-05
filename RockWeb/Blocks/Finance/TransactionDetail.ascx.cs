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
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using Rock.Web.Cache;
using System.Collections.Generic;

namespace RockWeb.Blocks.Finance
{
    [DisplayName( "Transaction Detail" )]
    [Category( "Finance" )]
    [Description( "Displays the details of the given transaction for editing." )]
    public partial class TransactionDetail : Rock.Web.UI.RockBlock, IDetailBlock
    {
        #region Fields
        private string contextTypeName = string.Empty;

        #endregion

        #region Control Methods
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                BindDropdowns();
                BindForm();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Shows the edit value.
        /// </summary>
        /// <param name="transactionId">The transactionId id.</param>
        /// <param name="setValues">if set to <c>true</c> [set values].</param>
        protected void ShowTransactionEditValue( int transactionId, string batchId )
        {
            hfIdTransValue.Value = transactionId.ToString();
            hfBatchId.Value = batchId;

            var transaction = new Rock.Model.FinancialTransactionService().Get( transactionId );

            if ( transaction != null )
            {
                lTitle.Text = "Edit Transaction".FormatAsHtmlTitle();

                hfIdTransValue.Value = transaction.Id.ToString();
                tbAmount.Text = transaction.Amount.ToString();
                hfBatchId.Value = transaction.BatchId.ToString();
                ddlCreditCardType.SetValue( transaction.CreditCardTypeValueId );
                ddlCurrencyType.SetValue( transaction.CurrencyTypeValueId );
                if ( transaction.GatewayEntityTypeId.HasValue )
                {
                    var gatewayEntity = Rock.Web.Cache.EntityTypeCache.Read( transaction.GatewayEntityTypeId.Value );
                    if ( gatewayEntity != null )
                    {
                        ddlPaymentGateway.SetValue( gatewayEntity.Guid.ToString() );
                    }
                }

                ddlSourceType.SetValue( transaction.SourceTypeValueId );
                ddlTransactionType.SetValue( transaction.TransactionTypeValueId );
                tbSummary.Text = transaction.Summary;
                tbTransactionCode.Text = transaction.TransactionCode;
                dtTransactionDateTime.SelectedDateTime = transaction.TransactionDateTime;
            }
            else
            {
                lTitle.Text = "Add Transaction".FormatAsHtmlTitle();
            }

            if ( ddlCurrencyType != null && ddlCurrencyType.SelectedItem.ToString() != "Credit Card" )
            { 
                ddlCreditCardType.Visible = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSaveFinancialTransaction control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSaveTransaction_Click( object sender, EventArgs e )
        {
            using ( new Rock.Data.UnitOfWorkScope() )
            {
                var financialTransactionService = new Rock.Model.FinancialTransactionService();
                Rock.Model.FinancialTransaction financialTransaction = null;
                int financialTransactionId = !string.IsNullOrEmpty( hfIdTransValue.Value ) ? int.Parse( hfIdTransValue.Value ) : 0;

                // null if not associated with a batch
                int? batchId = hfBatchId.Value.AsInteger();

                if ( financialTransactionId == 0 )
                {
                    financialTransaction = new Rock.Model.FinancialTransaction();
                    financialTransactionService.Add( financialTransaction, CurrentPersonAlias );
                    financialTransaction.BatchId = batchId;
                }
                else
                {
                    financialTransaction = financialTransactionService.Get( financialTransactionId );
                }

                financialTransaction.AuthorizedPersonId = CurrentPerson.Id;

                decimal amount = 0M;
                decimal.TryParse( tbAmount.Text.Replace( "$", string.Empty ), out amount );
                financialTransaction.Amount = amount;

                if ( ddlCurrencyType.SelectedItem.ToString() == "Credit Card" )
                {
                    financialTransaction.CreditCardTypeValueId = int.Parse( ddlCreditCardType.SelectedValue );
                }
                else
                {
                    financialTransaction.CreditCardTypeValueId = null;
                }

                financialTransaction.CurrencyTypeValueId = int.Parse( ddlCurrencyType.SelectedValue );
                if ( !string.IsNullOrEmpty( ddlPaymentGateway.SelectedValue ) )
                {
                    var gatewayEntity = Rock.Web.Cache.EntityTypeCache.Read( new Guid( ddlPaymentGateway.SelectedValue ) );
                    if ( gatewayEntity != null )
                    {
                        financialTransaction.GatewayEntityTypeId = gatewayEntity.Id;
                    }
                }

                financialTransaction.SourceTypeValueId = int.Parse( ddlSourceType.SelectedValue );
                financialTransaction.TransactionTypeValueId = int.Parse( ddlTransactionType.SelectedValue );

                financialTransaction.Summary = tbSummary.Text;
                financialTransaction.TransactionCode = tbTransactionCode.Text;
                financialTransaction.TransactionDateTime = dtTransactionDateTime.SelectedDateTime;

                financialTransactionService.Save( financialTransaction, CurrentPersonAlias );

                if ( batchId != null )
                {
                    Dictionary<string, string> qryString = new Dictionary<string, string>();
                    qryString["financialBatchid"] = hfBatchId.Value;
                    NavigateToParentPage( qryString );                    
                }
                else
                {
                    NavigateToParentPage();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCancelFinancialTransaction control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancelTransaction_Click( object sender, EventArgs e )
        {
            if ( !string.IsNullOrEmpty( hfBatchId.Value ) )
            {
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString["financialBatchid"] = hfBatchId.Value;
                NavigateToParentPage( qryString );
            }
            else
            {
                NavigateToParentPage();
            }   
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlCurrencyType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlCurrencyType_SelectedIndexChanged( object sender, EventArgs e )
        {
            // We don't want to show the Credit Card Type drop down if the type of currency isn't Credit Card.
            if ( ddlCurrencyType.SelectedItem.ToString() == "Credit Card" )
            {
                ddlCreditCardType.Visible = true;
            }
            else
            {
                ddlCreditCardType.Visible = false;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Binds the form.
        /// </summary>
        protected void BindForm()
        {
            try
            {
                string batchId = PageParameter( "financialBatchId" );
                string transactionId = PageParameter( "transactionId" );

                if ( !string.IsNullOrEmpty( transactionId ) )
                {
                    ShowTransactionEditValue( Convert.ToInt32( transactionId ), batchId );
                }
                else
                {
                    ShowTransactionEditValue( 0, batchId );
                }
            }
            catch ( Exception exp )
            {
                Response.Write( "The access request was unclear. Please fix the following: " + exp.Message );
                Response.End();
            }
        }

        /// <summary>
        /// Binds the dropdowns.
        /// </summary>
        protected void BindDropdowns()
        {
            BindDefinedTypeDropdown( ddlCurrencyType, new Guid( Rock.SystemGuid.DefinedType.FINANCIAL_CURRENCY_TYPE ), "Currency Type" );
            BindDefinedTypeDropdown( ddlCreditCardType, new Guid( Rock.SystemGuid.DefinedType.FINANCIAL_CREDIT_CARD_TYPE ), "Credit Card Type" );
            BindDefinedTypeDropdown( ddlSourceType, new Guid( Rock.SystemGuid.DefinedType.FINANCIAL_SOURCE_TYPE ), "Source" );
            BindDefinedTypeDropdown( ddlTransactionType, new Guid( Rock.SystemGuid.DefinedType.FINANCIAL_TRANSACTION_TYPE ), "Transaction Type" );
        }

        /// <summary>
        /// Binds the defined type dropdown.
        /// </summary>
        /// <param name="ListControl">The list control.</param>
        /// <param name="definedTypeGuid">The defined type GUID.</param>
        /// <param name="userPreferenceKey">The user preference key.</param>
        private void BindDefinedTypeDropdown( ListControl listControl, Guid definedTypeGuid, string userPreferenceKey )
        {
            listControl.BindToDefinedType( DefinedTypeCache.Read( definedTypeGuid ) );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            if ( !itemKey.Equals( "transactionId" ) && !!itemKey.Equals( "batchfk" ) )
            {
                return;
            }
        }

        /// <summary>
        /// Displays the error.
        /// </summary>
        /// <param name="message">The message.</param>
        private void DisplayError( string message )
        {
            valSummaryTop.Controls.Clear();
            valSummaryTop.Controls.Add( new LiteralControl( message ) );
            valSummaryTop.Visible = true;
        }

        #endregion
    }
}
