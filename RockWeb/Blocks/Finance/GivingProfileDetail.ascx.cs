﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
using Rock.Financial;
using Rock.Model;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using Rock.Web.Cache;

using System.Collections.Generic;

namespace RockWeb.Blocks.Finance
{
    #region Block Attributes

    /// <summary>
    /// Edit an existing scheduled transaction.
    /// </summary>
    [DisplayName( "Giving Profile Detail" )]
    [Category( "Financial" )]
    [Description( "Edit an existing scheduled transaction." )]

    [ComponentField( "Rock.Financial.GatewayContainer, Rock", "Credit Card Gateway", "The payment gateway to use for Credit Card transactions", false, "", "", 0, "CCGateway" )]
    [ComponentField( "Rock.Financial.GatewayContainer, Rock", "ACH Card Gateway", "The payment gateway to use for ACH (bank account) transactions", false, "", "", 1, "ACHGateway" )]

    [AccountsField( "Accounts", "The accounts to display.  By default all active accounts with a Public Name will be displayed", false, "", "", 6 )]
    [BooleanField( "Additional Accounts", "Display option for selecting additional accounts", "Don't display option",
        "Should users be allowed to select additional accounts?  If so, any active account with a Public Name value will be available", true, "", 7 )]
    [TextField( "Add Account Text", "The button text to display for adding an additional account", false, "Add Another Account", "", 8 )]

    [BooleanField( "Impersonation", "Allow (only use on an internal page used by staff)", "Don't Allow",
        "Should the current user be able to view and edit other people's transactions?  IMPORTANT: This should only be enabled on an internal page that is secured to trusted users", false, "", 10 )]

    [CodeEditorField( "Confirmation Header", "The text (HTML) to display at the top of the confirmation section.", CodeEditorMode.Html, CodeEditorTheme.Rock, 400, true, @"
<p>
Please confirm the information below. Once you have confirmed that the information is accurate click the 'Finish' button to complete your transaction. 
</p>
", "Text Options", 13 )]

    [CodeEditorField( "Confirmation Footer", "The text (HTML) to display at the bottom of the confirmation section.", CodeEditorMode.Html, CodeEditorTheme.Rock, 400, true, @"
<div class='alert alert-info'>
By clicking the 'finish' button below I agree to allow {{ OrganizationName }} to debit the amount above from my account. I acknowledge that I may 
update the transaction information at any time by returning to this website. Please call the Finance Office if you have any additional questions. 
</div>
", "Text Options", 14 )]

    [CodeEditorField( "Success Header", "The text (HTML) to display at the top of the success section.", CodeEditorMode.Html, CodeEditorTheme.Rock, 400, true, @"
<p>
Thank you for your generous contribution.  Your support is helping {{ OrganizationName }} actively 
achieve our mission.  We are so grateful for your commitment. 
</p>
", "Text Options", 15 )]

    [CodeEditorField( "Success Footer", "The text (HTML) to display at the bottom of the success section.", CodeEditorMode.Html, CodeEditorTheme.Rock, 400, true, @"
", "Text Options", 16 )]

    #endregion

    public partial class GivingProfileDetail : Rock.Web.UI.RockBlock
    {

        #region Fields

        private GatewayComponent _ccGateway;
        private GatewayComponent _achGateway;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the target person.
        /// </summary>
        protected Person TargetPerson { get; private set; }

        /// <summary>
        /// Gets or sets the accounts that are available for user to add to the list.
        /// </summary>
        protected List<AccountItem> AvailableAccounts
        {
            get
            {
                var accounts = ViewState["AvailableAccounts"] as List<AccountItem>;
                if ( accounts == null )
                {
                    accounts = new List<AccountItem>();
                }
                return accounts;
            }
            set
            {
                ViewState["AvailableAccounts"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the accounts that are currently displayed to the user
        /// </summary>
        protected List<AccountItem> SelectedAccounts
        {
            get
            {
                var accounts = ViewState["SelectedAccounts"] as List<AccountItem>;
                if ( accounts == null )
                {
                    accounts = new List<AccountItem>();
                }
                return accounts;
            }
            set
            {
                ViewState["SelectedAccounts"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the payment scheduled transaction Id.
        /// </summary>
        protected int? ScheduledTransactionId
        {
            get { return ViewState["ScheduledTransactionId"] as int?; }
            set { ViewState["ScheduledTransactionId"] = value; }
        }

        /// <summary>
        /// Gets or sets the payment transaction code.
        /// </summary>
        protected string TransactionCode
        {
            get { return ViewState["TransactionCode"] as string ?? string.Empty; }
            set { ViewState["TransactionCode"] = value; }
        }

        /// <summary>
        /// Gets or sets the payment schedule id.
        /// </summary>
        protected string ScheduleId
        {
            get { return ViewState["ScheduleId"] as string ?? string.Empty; }
            set { ViewState["ScheduleId"] = value; }
        }

        #endregion

        #region overridden control methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // If impersonation is allowed, and a valid person key was used, set the target to that person
            bool allowImpersonation = false;
            if ( bool.TryParse( GetAttributeValue( "Impersonation" ), out allowImpersonation ) && allowImpersonation )
            {
                string personKey = PageParameter( "Person" );
                if ( !string.IsNullOrWhiteSpace( personKey ) )
                {
                    TargetPerson = new PersonService().GetByUrlEncodedKey( personKey );
                }
            }
            if ( TargetPerson == null )
            {
                TargetPerson = CurrentPerson;
            }

            // Verify that transaction id is valid for selected person
            if ( !Page.IsPostBack && TargetPerson != null )
            {
                GetAccounts();

                int txnId = int.MinValue;
                if (int.TryParse(PageParameter( "Txn" ), out txnId))
                {
                    var scheduledTransaction = new FinancialScheduledTransactionService().Queryable("ScheduledTransactionDetails")
                        .Where( t =>
                            t.Id == txnId && 
                            ( t.AuthorizedPersonId == TargetPerson.Id || t.AuthorizedPerson.GivingGroupId == TargetPerson.GivingGroupId ) )
                        .FirstOrDefault();

                    if ( scheduledTransaction != null )
                    {
                        ScheduledTransactionId = txnId;
                        foreach( var txnDetail in scheduledTransaction.ScheduledTransactionDetails)
                        {
                            var availableAccount = AvailableAccounts.Where( a => a.Id == txnDetail.AccountId).FirstOrDefault();
                            if ( availableAccount != null )
                            {
                                var accountItem = new AccountItem( availableAccount.Id, availableAccount.Order, availableAccount.Name, availableAccount.CampusId );
                                accountItem.Amount = txnDetail.Amount;
                                SelectedAccounts.Add( accountItem );
                            }
                        }
                    }
                }
            }

            // Don't bother with anything else, if we don't have a valid person and transaction id
            if ( TargetPerson != null && ScheduledTransactionId.HasValue )
            {
                // Enable payment options based on the configured gateways
                bool ccEnabled = false;
                bool achEnabled = false;
                var supportedFrequencies = new List<DefinedValueCache>();

                string ccGatewayGuid = GetAttributeValue( "CCGateway" );
                if ( !string.IsNullOrWhiteSpace( ccGatewayGuid ) )
                {
                    _ccGateway = GatewayContainer.GetComponent( ccGatewayGuid );
                    if ( _ccGateway != null )
                    {
                        ccEnabled = true;
                        txtCardFirstName.Visible = _ccGateway.SplitNameOnCard;
                        txtCardLastName.Visible = _ccGateway.SplitNameOnCard;
                        txtCardName.Visible = !_ccGateway.SplitNameOnCard;
                        mypExpiration.MinimumYear = DateTime.Now.Year;
                    }
                }

                string achGatewayGuid = GetAttributeValue( "ACHGateway" );
                if ( !string.IsNullOrWhiteSpace( achGatewayGuid ) )
                {
                    _achGateway = GatewayContainer.GetComponent( achGatewayGuid );
                    achEnabled = _achGateway != null;
                }

                hfCurrentPage.Value = "1";
                RockPage page = Page as RockPage;
                if ( page != null )
                {
                    page.PageNavigate += page_PageNavigate;
                }

                hfPaymentTab.Value = "None";

                if ( ccEnabled || achEnabled )
                {
                    if ( ccEnabled )
                    {
                        supportedFrequencies = _ccGateway.SupportedPaymentSchedules;
                        divCCPaymentInfo.AddCssClass( "tab-pane" );
                        divCCPaymentInfo.Visible = ccEnabled;
                    }

                    if ( achEnabled )
                    {
                        supportedFrequencies = _achGateway.SupportedPaymentSchedules;
                        divACHPaymentInfo.AddCssClass( "tab-pane" );
                        divACHPaymentInfo.Visible = achEnabled;
                    }

                    if ( ccEnabled && achEnabled )
                    {
                        // If CC and ACH gateways are different, only allow frequencies supported by both payment gateways (if different)
                        if ( _ccGateway.TypeId != _achGateway.TypeId )
                        {
                            supportedFrequencies = _ccGateway.SupportedPaymentSchedules
                                .Where( c =>
                                    _achGateway.SupportedPaymentSchedules
                                        .Select( a => a.Id )
                                        .Contains( c.Id ) )
                                .ToList();
                        }
                    }

                    if ( supportedFrequencies.Any() )
                    {
                        var oneTimeFrequency = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.TRANSACTION_FREQUENCY_ONE_TIME );
                        divRepeatingPayments.Visible = true;

                        btnFrequency.DataSource = supportedFrequencies;
                        btnFrequency.DataBind();

                        // If gateway didn't specifically support one-time, add it anyway for immediate gifts
                        if ( !supportedFrequencies.Where( f => f.Id == oneTimeFrequency.Id ).Any() )
                        {
                            btnFrequency.Items.Insert( 0, new ListItem( oneTimeFrequency.Name, oneTimeFrequency.Id.ToString() ) );
                        }
                        btnFrequency.SelectedValue = oneTimeFrequency.Id.ToString();
                        dtpStartDate.SelectedDate = DateTime.Today;
                    }

                    // Display Options
                    btnAddAccount.Title = GetAttributeValue( "AddAccountText" );

                    BindSavedAccounts();

                    if ( rblSavedCC.Items.Count > 0 )
                    {
                        rblSavedCC.Items[0].Selected = true;
                        rblSavedCC.Visible = true;
                        divNewCard.Style[HtmlTextWriterStyle.Display] = "none";
                    }
                    else
                    {
                        rblSavedCC.Visible = false;
                        divNewCard.Style[HtmlTextWriterStyle.Display] = "block";
                    }

                    if ( rblSavedAch.Items.Count > 0 )
                    {
                        rblSavedAch.Items[0].Selected = true;
                        rblSavedAch.Visible = true;
                        divNewBank.Style[HtmlTextWriterStyle.Display] = "none";
                    }
                    else
                    {
                        rblSavedAch.Visible = false;
                        divNewCard.Style[HtmlTextWriterStyle.Display] = "block";
                    }

                    RegisterScript();

                    // Resolve the text field merge fields
                    var configValues = new Dictionary<string, object>();
                    Rock.Web.Cache.GlobalAttributesCache.Read().AttributeValues
                        .Where( v => v.Key.StartsWith( "Organization", StringComparison.CurrentCultureIgnoreCase ) )
                        .ToList()
                        .ForEach( v => configValues.Add( v.Key, v.Value.Value ) );
                    phConfirmationHeader.Controls.Add( new LiteralControl( GetAttributeValue( "ConfirmationHeader" ).ResolveMergeFields( configValues ) ) );
                    phConfirmationFooter.Controls.Add( new LiteralControl( GetAttributeValue( "ConfirmationFooter" ).ResolveMergeFields( configValues ) ) );
                    phSuccessHeader.Controls.Add( new LiteralControl( GetAttributeValue( "SuccessHeader" ).ResolveMergeFields( configValues ) ) );
                    phSuccessFooter.Controls.Add( new LiteralControl( GetAttributeValue( "SuccessFooter" ).ResolveMergeFields( configValues ) ) );

                    // Temp values for testing...
                    //txtCreditCard.Text = "5105105105105100";
                    //txtCVV.Text = "023";

                    //txtBankName.Text = "Test Bank";
                    //txtRoutingNumber.Text = "111111118";
                    //txtAccountNumber.Text = "1111111111";
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            // Hide the error box on every postback
            nbMessage.Visible = false;
            pnlDupWarning.Visible = false;

            if ( TargetPerson != null && ScheduledTransactionId.HasValue )
            {
                if ( _ccGateway != null || _achGateway != null )
                {

                    // Save amounts from controls to the viewstate list
                    foreach ( RepeaterItem item in rptAccountList.Items )
                    {
                        var accountAmount = item.FindControl( "txtAccountAmount" ) as RockTextBox;
                        if ( accountAmount != null )
                        {
                            if ( SelectedAccounts.Count > item.ItemIndex )
                            {
                                decimal amount = decimal.MinValue;
                                if ( decimal.TryParse( accountAmount.Text, out amount ) )
                                {
                                    SelectedAccounts[item.ItemIndex].Amount = amount;
                                }
                            }
                        }
                    }

                    // Update the total amount
                    lblTotalAmount.Text = SelectedAccounts.Sum( f => f.Amount ).ToString( "F2" );

                    // Set the frequency date label based on if 'One Time' is selected or not
                    if ( btnFrequency.Items.Count > 0 )
                    {
                        dtpStartDate.Label = btnFrequency.Items[0].Selected ? "When" : "First Gift";
                    }

                    liNone.RemoveCssClass( "active" );
                    liCreditCard.RemoveCssClass( "active" );
                    liACH.RemoveCssClass( "active" );
                    divNonePaymentInfo.RemoveCssClass( "active" );
                    divCCPaymentInfo.RemoveCssClass( "active" );
                    divACHPaymentInfo.RemoveCssClass( "active" );

                    switch ( hfPaymentTab.Value )
                    {
                        case "ACH":
                            {
                                liACH.AddCssClass( "active" );
                                divACHPaymentInfo.AddCssClass( "active" );
                                break;
                            }
                        case "CreditCard":
                            {
                                liCreditCard.AddCssClass( "active" );
                                divCCPaymentInfo.AddCssClass( "active" );
                                break;
                            }
                        default:
                            {
                                liNone.AddCssClass( "active" );
                                divNonePaymentInfo.AddCssClass( "active" );
                                break;
                            }
                    }

                    // Show or Hide the Credit card entry panel based on if a saved account exists and it's selected or not.
                    divNewCard.Style[HtmlTextWriterStyle.Display] = ( rblSavedCC.Items.Count == 0 || rblSavedCC.Items[rblSavedCC.Items.Count - 1].Selected ) ? "block" : "none";

                    if ( !Page.IsPostBack )
                    {
                        SetPage( 1 );

                        // Get the list of accounts that can be used
                        BindAccounts();
                    }
                }
                else
                {
                    SetPage( 0 );
                    ShowMessage( NotificationBoxType.Danger, "Configuration Error", "Please check the configuration of this block and make sure a valid Credit Card and/or ACH Finacial Gateway has been selected." );
                }
            }
            else
            {
                SetPage( 0 );
                ShowMessage( NotificationBoxType.Danger, "Invalid Transaction", "The transaction you've selected either does not exist or is not valid." );
            }

        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the SelectionChanged event of the btnAddAccount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnAddAccount_SelectionChanged( object sender, EventArgs e )
        {
            var selected = AvailableAccounts.Where( a => a.Id == ( btnAddAccount.SelectedValueAsId() ?? 0 ) ).ToList();
            AvailableAccounts = AvailableAccounts.Except( selected ).ToList();
            SelectedAccounts.AddRange( selected );

            BindAccounts();
        }

        /// <summary>
        /// Handles the PageNavigate event of the page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="HistoryEventArgs"/> instance containing the event data.</param>
        protected void page_PageNavigate( object sender, HistoryEventArgs e )
        {
            int pageId = e.State["GivingDetail"].AsInteger() ?? 0;
            if ( pageId > 0 )
            {
                SetPage( pageId );
            }
        }

        /// <summary>
        /// Handles the Click event of the btnPrev control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnPrev_Click( object sender, EventArgs e )
        {
            // Previous should only be enabled on the confirmation page (2)

            switch ( hfCurrentPage.Value.AsInteger() ?? 0 )
            {
                case 2:
                    SetPage( 1 );
                    break;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnNext_Click( object sender, EventArgs e )
        {
            string errorMessage = string.Empty;

            switch ( hfCurrentPage.Value.AsInteger() ?? 0 )
            {
                case 1:

                    if ( ProcessPaymentInfo( out errorMessage ) )
                    {
                        this.AddHistory( "GivingDetail", "1", null );
                        SetPage( 2 );
                    }
                    else
                    {
                        ShowMessage( NotificationBoxType.Danger, "Oops!", errorMessage );
                    }

                    break;

                case 2:

                    if ( ProcessConfirmation( out errorMessage ) )
                    {
                        this.AddHistory( "GivingDetail", "2", null );
                        SetPage( 3 );
                    }
                    else
                    {
                        ShowMessage( NotificationBoxType.Danger, "Payment Error", errorMessage );
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnConfirm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnConfirm_Click( object sender, EventArgs e )
        {
            TransactionCode = string.Empty;

            string errorMessage = string.Empty;
            if ( ProcessConfirmation( out errorMessage ) )
            {
                SetPage( 3 );
            }
            else
            {
                ShowMessage( NotificationBoxType.Danger, "Payment Error", errorMessage );
            }
        }

        #endregion

        #region Private Methods

        #region Methods for the Payment Info Page (panel)

        /// <summary>
        /// Gets the accounts.
        /// </summary>
        private void GetAccounts()
        {
            var selectedGuids = GetAttributeValues( "Accounts" ).Select( Guid.Parse ).ToList();
            bool showAll = !selectedGuids.Any();

            bool additionalAccounts = true;
            if ( !bool.TryParse( GetAttributeValue( "AdditionalAccounts" ), out additionalAccounts ) )
            {
                additionalAccounts = true;
            }

            SelectedAccounts = new List<AccountItem>();
            AvailableAccounts = new List<AccountItem>();

            // Enumerate through all active accounts that have a public name
            foreach ( var account in new FinancialAccountService().Queryable()
                .Where( f =>
                    f.IsActive &&
                    f.PublicName != null &&
                    f.PublicName.Trim() != "" &&
                    ( f.StartDate == null || f.StartDate <= DateTime.Today ) &&
                    ( f.EndDate == null || f.EndDate >= DateTime.Today ) )
                .OrderBy( f => f.Order ) )
            {
                var accountItem = new AccountItem( account.Id, account.Order, account.Name, account.CampusId );
                if ( showAll )
                {
                    SelectedAccounts.Add( accountItem );
                }
                else
                {
                    if ( selectedGuids.Contains( account.Guid ) )
                    {
                        SelectedAccounts.Add( accountItem );
                    }
                    else
                    {
                        if ( additionalAccounts )
                        {
                            AvailableAccounts.Add( accountItem );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Binds the accounts.
        /// </summary>
        private void BindAccounts()
        {
            rptAccountList.DataSource = SelectedAccounts.OrderBy( a => a.Order ).ToList();
            rptAccountList.DataBind();

            btnAddAccount.Visible = AvailableAccounts.Any();
            btnAddAccount.DataSource = AvailableAccounts;
            btnAddAccount.DataBind();
        }

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <param name="create">if set to <c>true</c> [create].</param>
        /// <returns></returns>
        private Person GetPerson( bool create )
        {
            Person person = null;

            int personId = ViewState["PersonId"] as int? ?? 0;
            if ( personId == 0 && TargetPerson != null )
            {
                person = TargetPerson;
            }
            else
            {
                using ( new UnitOfWorkScope() )
                {
                    var personService = new PersonService();

                    if ( personId != 0 )
                    {
                        person = personService.Get( personId );
                    }
                }
            }

            return person;
        }

        /// <summary>
        /// Binds the saved accounts.
        /// </summary>
        private void BindSavedAccounts()
        {
            rblSavedCC.Items.Clear();

            if ( TargetPerson != null )
            {
                // Get the saved accounts for the currently logged in user
                var savedAccounts = new FinancialPersonSavedAccountService()
                    .GetByPersonId( TargetPerson.Id );

                if ( _ccGateway != null )
                {
                    var ccCurrencyType = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.CURRENCY_TYPE_CREDIT_CARD ) );

                    rblSavedCC.DataSource = savedAccounts
                        .Where( a =>
                            a.FinancialTransaction.GatewayEntityTypeId == _ccGateway.TypeId &&
                            a.FinancialTransaction.CurrencyTypeValueId == ccCurrencyType.Id )
                        .OrderBy( a => a.Name )
                        .Select( a => new
                        {
                            Id = a.Id,
                            Name = "Use " + a.Name + " (" + a.MaskedAccountNumber + ")"
                        } ).ToList();
                    rblSavedCC.DataBind();
                    if ( rblSavedCC.Items.Count > 0 )
                    {
                        rblSavedCC.Items.Add( new ListItem( "Use a different card", "0" ) );
                    }
                }

                if ( _achGateway != null )
                {
                    var achCurrencyType = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.CURRENCY_TYPE_ACH ) );

                    rblSavedAch.DataSource = savedAccounts
                        .Where( a =>
                            a.FinancialTransaction.GatewayEntityTypeId == _achGateway.TypeId &&
                            a.FinancialTransaction.CurrencyTypeValueId == achCurrencyType.Id )
                        .OrderBy( a => a.Name )
                        .Select( a => new
                        {
                            Id = a.Id,
                            Name = "Use " + a.Name + " (" + a.MaskedAccountNumber + ")"
                        } ).ToList();
                    rblSavedAch.DataBind();
                    if ( rblSavedAch.Items.Count > 0 )
                    {
                        rblSavedAch.Items.Add( new ListItem( "Use a different bank account", "0" ) );
                    }
                }
            }
        }

        /// <summary>
        /// Processes the payment information.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        private bool ProcessPaymentInfo( out string errorMessage )
        {
            errorMessage = string.Empty;

            var errorMessages = new List<string>();

            // Validate that an amount was entered
            if ( SelectedAccounts.Sum( a => a.Amount ) <= 0 )
            {
                errorMessages.Add( "Make sure you've entered an amount for at least one account" );
            }

            // Validate that no negative amounts were entered
            if ( SelectedAccounts.Any( a => a.Amount < 0 ) )
            {
                errorMessages.Add( "Make sure the amount you've entered for each account is a positive amount" );
            }

            // Get the payment schedule
            PaymentSchedule schedule = GetSchedule();

            if ( schedule != null )
            {
                // Make sure a repeating payment starts in the future
                if ( schedule.StartDate <= DateTime.Today )
                {
                    errorMessages.Add( "When scheduling a repeating payment, make sure the First Gift date is in the future (after today)" );
                }
            }

            if ( hfPaymentTab.Value == "ACH" )
            {
                // Validate ach options
                if ( rblSavedAch.Items.Count > 0 && ( rblSavedAch.SelectedValueAsInt() ?? 0 ) > 0 )
                {
                    // TODO: Find saved account
                }
                else
                {
                    if ( string.IsNullOrWhiteSpace( txtBankName.Text ) )
                    {
                        errorMessages.Add( "Make sure to enter a bank name" );
                    }

                    if ( string.IsNullOrWhiteSpace( txtRoutingNumber.Text ) )
                    {
                        errorMessages.Add( "Make sure to enter a valid routing number" );
                    }

                    if ( string.IsNullOrWhiteSpace( txtAccountNumber.Text ) )
                    {
                        errorMessages.Add( "Make sure to enter a valid account number" );
                    }
                }
            }
            else if ( hfPaymentTab.Value == "CC" )
            {
                // validate cc options
                if ( rblSavedCC.Items.Count > 0 && ( rblSavedCC.SelectedValueAsInt() ?? 0 ) > 0 )
                {
                    // TODO: Find saved card
                }
                else
                {
                    if ( _ccGateway.SplitNameOnCard )
                    {
                        if ( string.IsNullOrWhiteSpace( txtCardFirstName.Text ) || string.IsNullOrWhiteSpace( txtCardLastName.Text ) )
                        {
                            errorMessages.Add( "Make sure to enter a valid first and last name as it appears on your credit card" );
                        }
                    }
                    else
                    {
                        if ( string.IsNullOrWhiteSpace( txtCardName.Text ) )
                        {
                            errorMessages.Add( "Make sure to enter a valid name as it appears on your credit card" );
                        }
                    }

                    if ( string.IsNullOrWhiteSpace( txtCreditCard.Text ) )
                    {
                        errorMessages.Add( "Make sure to enter a valid credit card number" );
                    }

                    var currentMonth = DateTime.Today;
                    currentMonth = new DateTime( currentMonth.Year, currentMonth.Month, 1 );
                    if ( !mypExpiration.SelectedDate.HasValue || mypExpiration.SelectedDate.Value.CompareTo( currentMonth ) < 0 )
                    {
                        errorMessages.Add( "Make sure to enter a valid credit card expiration date" );
                    }

                    if ( string.IsNullOrWhiteSpace( txtCVV.Text ) )
                    {
                        errorMessages.Add( "Make sure to enter a valid credit card security code" );
                    }
                }
            }

            if ( errorMessages.Any() )
            {
                errorMessage = errorMessages.AsDelimited( "<br/>" );
                return false;
            }

            PaymentInfo paymentInfo = GetPaymentInfo();

            tdName.Description = paymentInfo.FullName;
            tdPhone.Description = paymentInfo.Phone;
            tdEmail.Description = paymentInfo.Email;
            tdAddress.Description = string.Format( "{0} {1}, {2} {3}",
                paymentInfo.Street, paymentInfo.City, paymentInfo.State, paymentInfo.Zip );

            rptAccountListConfirmation.DataSource = SelectedAccounts.Where( a => a.Amount != 0 );
            rptAccountListConfirmation.DataBind();

            tdTotal.Description = paymentInfo.Amount.ToString( "C" );

            tdPaymentMethod.Description = paymentInfo.CurrencyTypeValue.Description;
            tdAccountNumber.Description = paymentInfo.MaskedNumber;
            tdWhen.Description = schedule != null ? schedule.ToString() : "Today";

            return true;
        }

        /// <summary>
        /// Gets the payment information.
        /// </summary>
        /// <returns></returns>
        private PaymentInfo GetPaymentInfo()
        {
            PaymentInfo paymentInfo = null;
            if ( hfPaymentTab.Value == "ACH" )
            {
                if ( rblSavedAch.Items.Count > 0 && ( rblSavedAch.SelectedValueAsId() ?? 0 ) > 0 )
                {
                    paymentInfo = GetReferenceInfo( rblSavedAch.SelectedValueAsId().Value );
                }
                else
                {
                    paymentInfo = GetACHInfo();
                }
            }
            else if ( hfPaymentTab.Value == "CC" )
            {
                if ( rblSavedCC.Items.Count > 0 && ( rblSavedCC.SelectedValueAsId() ?? 0 ) > 0 )
                {
                    paymentInfo = GetReferenceInfo( rblSavedCC.SelectedValueAsId().Value );
                }
                else
                {
                    paymentInfo = GetCCInfo();
                }
            }

            paymentInfo.Amount = SelectedAccounts.Sum( a => a.Amount );
            //paymentInfo.Email = txtEmail.Text;
            //paymentInfo.Phone = txtPhone.Text;
            //paymentInfo.Street = txtStreet.Text;
            //paymentInfo.City = txtCity.Text;
            //paymentInfo.State = ddlState.SelectedValue;
            //paymentInfo.Zip = txtZip.Text;

            return paymentInfo;
        }

        /// <summary>
        /// Gets the credit card information.
        /// </summary>
        /// <returns></returns>
        private CreditCardPaymentInfo GetCCInfo()
        {
            var cc = new CreditCardPaymentInfo( txtCreditCard.Text, txtCVV.Text, mypExpiration.SelectedDate.Value );
            cc.NameOnCard = _ccGateway.SplitNameOnCard ? txtCardFirstName.Text : txtCardName.Text;
            cc.LastNameOnCard = txtCardLastName.Text;
            cc.BillingStreet = txtBillingStreet.Text;
            cc.BillingCity = txtBillingCity.Text;
            cc.BillingState = ddlBillingState.SelectedValue;
            cc.BillingZip = txtBillingZip.Text;

            return cc;
        }

        /// <summary>
        /// Gets the ACH information.
        /// </summary>
        /// <returns></returns>
        private ACHPaymentInfo GetACHInfo()
        {
            var ach = new ACHPaymentInfo( txtAccountNumber.Text, txtRoutingNumber.Text, rblAccountType.SelectedValue == "Savings" ? BankAccountType.Savings : BankAccountType.Checking );
            ach.BankName = txtBankName.Text;
            return ach;
        }

        /// <summary>
        /// Gets the reference information.
        /// </summary>
        /// <param name="savedAccountId">The saved account unique identifier.</param>
        /// <returns></returns>
        private ReferencePaymentInfo GetReferenceInfo( int savedAccountId )
        {
            using ( new UnitOfWorkScope() )
            {
                var savedAccount = new FinancialPersonSavedAccountService().Get( savedAccountId );
                if ( savedAccount != null )
                {
                    var reference = new ReferencePaymentInfo();
                    reference.TransactionCode = savedAccount.FinancialTransaction.TransactionCode;
                    reference.ReferenceNumber = savedAccount.ReferenceNumber;
                    reference.MaskedAccountNumber = savedAccount.MaskedAccountNumber;
                    reference.InitialCurrencyTypeValue = DefinedValueCache.Read( savedAccount.FinancialTransaction.CurrencyTypeValue );
                    if ( reference.InitialCurrencyTypeValue.Guid.Equals( new Guid( Rock.SystemGuid.DefinedValue.CURRENCY_TYPE_CREDIT_CARD ) ) )
                    {
                        reference.InitialCreditCardTypeValue = DefinedValueCache.Read( savedAccount.FinancialTransaction.CreditCardTypeValue );
                    }
                    return reference;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the payment schedule.
        /// </summary>
        /// <returns></returns>
        private PaymentSchedule GetSchedule()
        {
            // If a one-time gift was selected for today's date, then treat as a onetime immediate transaction (not scheduled)
            int oneTimeFrequencyId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.TRANSACTION_FREQUENCY_ONE_TIME ).Id;
            if ( btnFrequency.SelectedValue == oneTimeFrequencyId.ToString() && dtpStartDate.SelectedDate == DateTime.Today )
            {
                // one-time immediate payment
                return null;
            }

            var schedule = new PaymentSchedule();
            schedule.TransactionFrequencyValue = DefinedValueCache.Read( btnFrequency.SelectedValueAsId().Value );
            if ( dtpStartDate.SelectedDate.HasValue && dtpStartDate.SelectedDate > DateTime.Today )
            {
                schedule.StartDate = dtpStartDate.SelectedDate.Value;
            }
            else
            {
                schedule.StartDate = DateTime.MinValue;
            }

            return schedule;
        }

        #endregion

        #region Methods for the confirmation Page (panel)

        /// <summary>
        /// Processes the confirmation.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        private bool ProcessConfirmation( out string errorMessage )
        {
            if ( string.IsNullOrWhiteSpace( TransactionCode ) )
            {
                GatewayComponent gateway = hfPaymentTab.Value == "ACH" ? _achGateway : _ccGateway;
                if ( gateway == null )
                {
                    errorMessage = "There was a problem creating the payment gateway information";
                    return false;
                }

                Person person = GetPerson( true );
                if ( person == null )
                {
                    errorMessage = "There was a problem creating the person information";
                    return false;
                }

                PaymentInfo paymentInfo = GetPaymentInfo();
                if ( paymentInfo == null )
                {
                    errorMessage = "There was a problem creating the payment information";
                    return false;
                }
                else
                {
                    paymentInfo.FirstName = person.FirstName;
                    paymentInfo.LastName = person.LastName;
                }

                PaymentSchedule schedule = GetSchedule();
                if ( schedule != null )
                {
                    schedule.PersonId = person.Id;

                    var scheduledTransaction = gateway.AddScheduledPayment( schedule, paymentInfo, out errorMessage );
                    if ( scheduledTransaction != null )
                    {
                        scheduledTransaction.TransactionFrequencyValueId = schedule.TransactionFrequencyValue.Id;
                        scheduledTransaction.AuthorizedPersonId = person.Id;

                        foreach ( var account in SelectedAccounts.Where( a => a.Amount > 0 ) )
                        {
                            var transactionDetail = new FinancialScheduledTransactionDetail();
                            transactionDetail.Amount = account.Amount;
                            transactionDetail.AccountId = account.Id;
                            scheduledTransaction.ScheduledTransactionDetails.Add( transactionDetail );
                        }

                        var transactionService = new FinancialScheduledTransactionService();
                        transactionService.Add( scheduledTransaction, CurrentPersonId );
                        transactionService.Save( scheduledTransaction, CurrentPersonId );

                        ScheduleId = scheduledTransaction.GatewayScheduleId;
                        TransactionCode = scheduledTransaction.TransactionCode;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                tdScheduleId.Description = ScheduleId;

                return true;
            }
            else
            {
                pnlDupWarning.Visible = true;
                errorMessage = string.Empty;
                return false;
            }
        }

        #endregion

        #region Methods used globally

        /// <summary>
        /// Sets the page.
        /// </summary>
        /// <param name="page">The page.</param>
        private void SetPage( int page )
        {
            // Page 1 = Payment Info
            // Page 2 = Confirmation
            // Page 3 = Success
            // Page 0 = Only message box is displayed

            pnlPaymentInfo.Visible = page == 1;
            pnlConfirmation.Visible = page == 2;
            pnlSuccess.Visible = page == 3;
            divActions.Visible = page > 0;

            btnPrev.Visible = page == 2;
            btnNext.Visible = page < 3;
            btnNext.Text = page > 1 ? "Finish" : "Next";

            hfCurrentPage.Value = page.ToString();
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        private void ShowMessage( NotificationBoxType type, string title, string text )
        {
            if ( !string.IsNullOrWhiteSpace( text ) )
            {
                nbMessage.Text = text;
                nbMessage.Title = title;
                nbMessage.NotificationBoxType = type;
                nbMessage.Visible = true;
            }
        }

        /// <summary>
        /// Registers the startup script.
        /// </summary>
        private void RegisterScript()
        {
            RockPage.AddScriptLink( ResolveUrl( "~/Scripts/jquery.creditCardTypeDetector.js" ) );

            int oneTimeFrequencyId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.TRANSACTION_FREQUENCY_ONE_TIME ).Id;

            string script = string.Format( @"
    Sys.Application.add_load(function () {{

        // As amounts are entered, validate that they are numeric and recalc total
        $('.account-amount').on('change', function() {{
            var totalAmt = Number(0);   
                 
            $('.account-amount .form-control').each(function (index) {{
                var itemValue = $(this).val();
                if (itemValue != null && itemValue != '') {{
                    if (isNaN(itemValue)) {{
                        $(this).parents('div.input-group').addClass('has-error');
                    }}
                    else {{
                        $(this).parents('div.input-group').removeClass('has-error');
                        var num = Number(itemValue);
                        $(this).val(num.toFixed(2));
                        totalAmt = totalAmt + num;
                    }}
                }}
                else {{
                    $(this).parents('div.input-group').removeClass('has-error');
                }}
            }});
            $('.total-amount').html('$ ' + totalAmt.toFixed(2));
            return false;
        }});

        // Set the date prompt based on the frequency value entered
        $('#ButtonDropDown_btnFrequency .dropdown-menu a').click( function () {{
            var $when = $(this).parents('div.form-group:first').next(); 
            if ($(this).attr('data-id') == '{2}') {{
                $when.find('label:first').html('When');
            }} else {{
                $when.find('label:first').html('First Gift');

                // Set date to tomorrow if it is equal or less than today's date
                var $dateInput = $when.find('input');
                var dt = new Date(Date.parse($dateInput.val()));
                var curr = new Date();
                if ( (dt-curr) <= 0 ) {{ 
                    curr.setDate(curr.getDate() + 1);
                    var dd = curr.getDate();
                    var mm = curr.getMonth()+1;
                    var yy = curr.getFullYear();
                    $dateInput.val(mm+'/'+dd+'/'+yy);
                    $dateInput.data('datePicker').value(mm+'/'+dd+'/'+yy);
                }}
            }};
            
        }});

        // Save the state of the selected payment type pill to a hidden field so that state can 
        // be preserved through postback
        $('a[data-toggle=""pill""]').on('shown.bs.tab', function (e) {{
            var tabHref = $(e.target).attr(""href"");
            if (tabHref == '#{0}') {{
                $('#{1}').val('CreditCard');
            }} else {{
                $('#{1}').val('ACH');
            }}
        }});

        // Detect credit card type
        $('.credit-card').creditCardTypeDetector({{ 'credit_card_logos': '.card-logos' }});

        // Toggle credit card display if saved card option is available
        $('div.radio-content').prev('.form-group').find('input:radio').unbind('click').on('click', function () {{
            var $content = $(this).parents('div.form-group:first').next('.radio-content')
            var radioDisplay = $content.css('display');            
            if ($(this).val() == 0 && radioDisplay == 'none') {{
                $content.slideToggle();
            }}
            else if ($(this).val() != 0 && radioDisplay != 'none') {{
                $content.slideToggle();
            }}
        }});      

        // Hide or show a div based on selection of checkbox
        $('input:checkbox.toggle-input').unbind('click').on('click', function () {{
            $(this).parents('.checkbox').next('.toggle-content').slideToggle();
        }});

        // Disable the submit button as soon as it's clicked to prevent double-clicking
        $('a[id$=""btnNext""]').click(function() {{
			$(this).addClass('disabled');
			$(this).unbind('click');
			$(this).click(function () {{
				return false;
			}});
        }});
 
    }});

", divCCPaymentInfo.ClientID, hfPaymentTab.ClientID, oneTimeFrequencyId );
            ScriptManager.RegisterStartupScript( upPayment, this.GetType(), "giving-profile", script, true );
        }

        #endregion

        #region Helper Classes

        /// <summary>
        /// Lightweight object for each contribution item 
        /// </summary>
        [Serializable]
        protected class AccountItem
        {
            public int Id { get; set; }
            public int Order { get; set; }
            public string Name { get; set; }
            public int? CampusId { get; set; }
            public decimal Amount { get; set; }

            public string AmountFormatted
            {
                get
                {
                    return Amount > 0 ? Amount.ToString( "F2" ) : string.Empty;
                }

            }

            public AccountItem( int id, int order, string name, int? campusId )
            {
                Id = id;
                Order = order;
                Name = name;
                CampusId = campusId;
            }
        }

        #endregion

        #endregion

    }

}
