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
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;

namespace RockWeb.Blocks.Finance
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Kiosk Transaction Entry" )]
    [Category( "Finance" )]
    [Description( "Block used to process giving from a kiosk." )]

    #region Block Attributes
    [ComponentField( "Rock.Financial.GatewayContainer, Rock", "Credit Card Gateway", "The payment gateway to use for Credit Card transactions", true, "", "", 0, "CCGateway" )]
    [DefinedValueField( Rock.SystemGuid.DefinedType.FINANCIAL_SOURCE_TYPE, "Source", "The Financial Source Type to use when creating transactions", false, false,
        Rock.SystemGuid.DefinedValue.FINANCIAL_SOURCE_TYPE_KIOSK, "", 1 )]
    [AccountsField( "Accounts", "Accounts to allow giving. This list will be filtered by campus context when displayed.", true, "", "", 1 )]
    [TextField( "Batch Name Prefix", "The prefix to add to the financial batch.", true, "Kiosk Giving", "", 2 )]
    [LinkedPage( "Homepage", "Homepage of the kiosk.", true, "", "", 2 )]
    [PersonField("Anonymous Person", "Person in the database to assign anonymous giving to.", true, "", "", 3)]
    [DefinedValueField( Rock.SystemGuid.DefinedType.PERSON_CONNECTION_STATUS, "Connection Status", "The connection status to use when creating a new individual.", true, false, Rock.SystemGuid.DefinedValue.PERSON_CONNECTION_STATUS_PARTICIPANT, "", 4 )]
    [IntegerField( "Minimum Phone Number Length", "Minimum length for phone number searches (defaults to 4).", false, 4,"", 5 )]
    [IntegerField( "Maximum Phone Number Length", "Maximum length for phone number searches (defaults to 10).", false, 10, "", 6 )]
    [TextField( "Search Regex", "Regular Expression to run the search input through before searching. Useful for stripping off characters.", false, "", "", 7 )]
    #endregion

    public partial class KioskTransactionEntry : Rock.Web.UI.RockBlock
    {
        #region Fields

        // used for private variables
        private List<GivingUnit> GivingUnits
        {
            get { return (List<GivingUnit>)ViewState["GivingUnits"]; }
            set { ViewState["GivingUnits"] = value; }
        }

        private GivingUnit SelectedGivingUnit
        {
            get { return (GivingUnit)ViewState["SelectedGivingUnit"]; }
            set { ViewState["SelectedGivingUnit"] = value; }
        }

        private Dictionary<int, decimal> Amounts
        {
            get { return ViewState["Amounts"] as Dictionary<int, decimal>; }
            set { ViewState["Amounts"] = value; }
        }
        #endregion

        #region Properties

        // used for public / protected properties

        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

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

            RockPage.AddScriptLink( "~/Scripts/iscroll.js" );
            RockPage.AddScriptLink( "~/Scripts/Kiosk/kiosk-core.js" );
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
                // set max length of phone
                int maxLength = int.Parse( GetAttributeValue( "MaximumPhoneNumberLength" ) );
                tbPhone.MaxLength = maxLength;
            }
            else
            {
                if ( pnlGivingUnitSelect.Visible )
                {
                    BuildGivingUnitControls();
                }
            }
        }

        #endregion

        #region Events
                
        //
        // Search Events
        //

        protected void lbSearchNext_Click( object sender, EventArgs e )
        {
            ShowGivingUnitSelectPanel();
        }

        protected void lbSearchCancel_Click( object sender, EventArgs e )
        {
            GoHome();
        }

        //
        // Giving Unit Select Events
        //

        // called when a giving unit is selected
        void unitName_Click( object sender, EventArgs e )
        {
            LinkButton lb = (LinkButton)sender;
            this.SelectedGivingUnit = new GivingUnit( lb.CommandArgument );

            ShowAccountPanel();
        }

        protected void lbGivingUnitSelectCancel_Click( object sender, EventArgs e )
        {
            GoHome();
        }

        protected void lbGivingUnitSelectBack_Click( object sender, EventArgs e )
        {
            HidePanels();
            pnlSearch.Visible = true;
        }

        //
        // Account Entry Events
        //

        protected void lbAccountEntryBack_Click( object sender, EventArgs e )
        {
            HidePanels();
            pnlGivingUnitSelect.Visible = true;
        }
        protected void lbAccountEntryCancel_Click( object sender, EventArgs e )
        {
            GoHome();
        }

        // used to show the new registration page
        protected void lbRegisterFamily_Click( object sender, EventArgs e )
        {
            HidePanels();
            pnlRegister.Visible = true;
        }

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        #endregion

        #region Methods

        // show giving unit select panel
        private void ShowGivingUnitSelectPanel()
        {
            int minLength = int.Parse( GetAttributeValue( "MinimumPhoneNumberLength" ) );
            int maxLength = int.Parse( GetAttributeValue( "MaximumPhoneNumberLength" ) );

            if ( tbPhone.Text.Length >= minLength && tbPhone.Text.Length <= maxLength )
            {
                // run regex expression on input if provided
                string searchInput = tbPhone.Text;
                if ( !string.IsNullOrWhiteSpace( GetAttributeValue( "SearchRegex" ) ) )
                {
                    Regex regex = new Regex( GetAttributeValue( "SearchRegex" ) );
                    Match match = regex.Match( searchInput );
                    if ( match.Success )
                    {
                        if ( match.Groups.Count == 2 )
                        {
                            searchInput = match.Groups[1].ToString();
                        }
                    }
                }

                var searchResults = new List<GivingUnit>();

                RockContext rockContext = new RockContext();
                PersonService personService = new PersonService( rockContext );
                var people = personService.GetByPhonePartial( searchInput, false, true );

                foreach ( var person in people.ToList() )
                {
                    if ( person.GivingGroupId == null )
                    {
                        // giving as an individuals
                        searchResults.Add( new GivingUnit(person.PrimaryAliasId.Value, person.LastName, person.FirstName));
                    }
                    else
                    {
                        var givingGroupMembers = person.GivingGroup.Members
                                                    .Where( g => g.Person.GivingGroupId == g.GroupId )
                                                    .OrderBy( g => g.GroupRole.Order )
                                                    .ThenBy( g => g.Person.Gender )
                                                    .ThenBy( g => g.Person.Age );

                        if ( givingGroupMembers.ToList().Count == 1 )
                        {
                            // only one person in the giving group display as an individual
                            searchResults.Add( new GivingUnit( person.PrimaryAliasId.Value, person.LastName, person.FirstName ) );
                        }
                        else
                        {
                            // display as a family
                            string firstNameList = string.Join( ", ", givingGroupMembers.Select( g => g.Person.NickName ) ).ReplaceLastOccurrence(",", " &");
                            int headOfHousePersonAliasId = givingGroupMembers.Select( g => g.Person.PrimaryAliasId.Value ).FirstOrDefault();
                            string lastName = givingGroupMembers.Select( g => g.Person.LastName ).FirstOrDefault();
                            searchResults.Add( new GivingUnit( headOfHousePersonAliasId, person.LastName, firstNameList ) );
                        }
                    }
                }

                this.GivingUnits = searchResults;

                BuildGivingUnitControls();

                HidePanels();
                pnlGivingUnitSelect.Visible = true;
            }
            else
            {
                if ( tbPhone.Text.Length < minLength )
                {
                    nbSearch.Text = String.Format( "Please enter at least {0} numbers of your phone.", minLength.ToString() );
                }
                else
                {
                    nbSearch.Text = String.Format( "Please enter no more than {0} numbers to search on.", maxLength.ToString() );
                }
            }
        }

        // show accounts panel 
        private void ShowAccountPanel()
        {
            lblGivingAs.Text = String.Format( "Giving as {0} {1}", this.SelectedGivingUnit.FirstNames, this.SelectedGivingUnit.LastName );
            
            // get accounts
            BuildAccountControls();

            // show panels
            HidePanels();
            pnlAccountEntry.Visible = true;


        }

        // displays accounts
        private void BuildAccountControls()
        {
            // get list of selected accounts filtered by the current campus
            RockContext rockContext = new RockContext();
            FinancialAccountService accountService = new FinancialAccountService( rockContext );
            
            Guid[] selectedAccounts = GetAttributeValue( "Accounts" ).Split( ',' ).Select( s => Guid.Parse( s ) ).ToArray(); ;
            int campusContext = 1;

            var accounts = accountService.Queryable()
                            .Where( a => selectedAccounts.Contains( a.Guid ) && (a.CampusId.Value == campusContext || a.CampusId == null) )
                            .ToList();

            if ( accounts.Count > 0 )
            {
                bool firstAccount = true;
                
                foreach ( var account in accounts )
                {
                    HtmlGenericControl formGroup = new HtmlGenericControl( "div" );
                    formGroup.AddCssClass( "form-group" );
                    phAccounts.Controls.Add( formGroup );

                    RockTextBox tb = new RockTextBox();
                    tb.PrependText = "<i class='fa fa-dollar'></i>";
                    tb.ID = "tbAccount_" + account.Id;
                    tb.Attributes.Add( "name", tb.ID );
                    //tb.Attributes.Add( "placeholder", "$0.00" );
                    tb.Attributes.Add( "type", "number" );

                    if ( firstAccount )
                    {
                        tb.CssClass = "active";
                        firstAccount = false;
                    }

                    Label label = new Label();
                    label.AssociatedControlID = tb.ID;
                    label.ID = "labelFund_" + account.Id;
                    label.Text = account.PublicName;

                    formGroup.Controls.Add( label );
                    formGroup.Controls.Add( tb );
                }
            }
        }

        // displays giving units
        private void BuildGivingUnitControls()
        {           
            // display results
            if ( this.GivingUnits.Count > 0 )
            {

                foreach ( var unit in this.GivingUnits )
                {
                    LinkButton lb = new LinkButton();
                    lb.ID = "lbUnit_" + unit.PersonAliasId.ToString();
                    lb.CssClass = "btn btn-primary btn-kioskselect";
                    phGivingUnits.Controls.Add( lb );
                    lb.CommandArgument = unit.CommandArg;
                    lb.Click += new EventHandler( unitName_Click );
                    lb.Text = string.Format("{0} <small>{1}</small>", unit.LastName, unit.FirstNames);
                }
            }
            else
            {
                phGivingUnits.Controls.Add( new LiteralControl(
                    "<div class='alert alert-danger'>There were not any families found with the phone number you entered. You can add your family using the 'Register Your Family' button below.</div>" ) );
            }
        }

        // hides all panels
        private void HidePanels()
        {
            pnlSearch.Visible = false;
            pnlGivingUnitSelect.Visible = false;
            pnlRegister.Visible = false;
            pnlAccountEntry.Visible = false;
            pnlSwipe.Visible = false;
            pnlReceipt.Visible = false;

            // clear out specific notification blocks that are used for validation
            nbSearch.Text = string.Empty;
        }

        // redirects to the homepage
        private void GoHome()
        {
            NavigateToLinkedPage( "Homepage" );
        }

        #endregion
}

    [Serializable]
    class GivingUnit
    {
        public int PersonAliasId { get; set; }
        public string LastName { get; set; }
        public string FirstNames { get; set; }

        public string CommandArg
        {
            get { return string.Format( "{0}|{1}|{2}", PersonAliasId, LastName, FirstNames ); }
        }

        public GivingUnit( int personAliasId, string lastName, string firstNames )
        {
            PersonAliasId = personAliasId;
            LastName = lastName;
            FirstNames = firstNames;
        }

        public GivingUnit( string commandArg )
        {
            string[] parts = commandArg.Split( '|' );

            PersonAliasId = Convert.ToInt32( parts[0] );
            LastName = parts[1];
            FirstNames = parts[2];
        }
    }
}