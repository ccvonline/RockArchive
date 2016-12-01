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
using System.Data.Entity;
using System.Linq;
using System.Web.UI;

using com.minecartstudio.ExchangeContactSync.Transactions;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.com_mineCartStudio.ExchangeContactSync
{
    /// <summary>
    /// Block that syncs selected people to an exchange server.
    /// </summary>
    [DisplayName( "Sync Contacts" )]
    [Category( "Mine Cart Studio > Exchange Contact Sync" )]
    [Description( "Block that syncs selected people to an exchange server." )]

    [TextField("Admin Username", "The exchange admin's username.", true, "", "", 0)]
    [TextField("Admin Password", "The exchange admin's password.", true, "", "", 1, "AdminPassword", true )]
    [TextField( "Admin Domain", "The exchange admin's domain.", false, "", "", 2 )]
    [CustomDropdownListField("Exchange Version", "Exchange Server version", "0^2007 SP1,1^2010,2^2010 SP1,3^2010 SP2,4^2013,5^2013 SP1",true,"3","",3 )]
    [TextField( "Auto Discover URL", "The Exchange Auto Discover URL (If left blank, plugin will attempt to find it based on admin's username (email address).", false, "", "", 4 )]
    [SecurityRoleField( "Eligible People", "The security role that contains individuals who are allowed to sync contacts.", true, Rock.SystemGuid.Group.GROUP_STAFF_MEMBERS, "", 5 )]
    [BooleanField("Enable Trace", "Should tracing be enabled for the Exchange Web Service (helpful to debug connection issues).", false, "", 6)]
    [BooleanField("Sync Business Phone Number", "Should business phone numbers be synced?", true, "", 7, "SyncBusinessPhone")]
    public partial class SyncContacts : Rock.Web.UI.RockBlock
    {

        #region Fields

        private string _keyPrefix = string.Empty;

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:Init" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            _keyPrefix = string.Format( "MineCartStudio.SyncContacts.{0}.", this.BlockId );
        }

        /// <summary>
        /// Raises the <see cref="E:Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            nbNotice.Visible = false;
            pnlEntry.Visible = true;

            if ( !Page.IsPostBack )
            {
                Guid groupGuid = GetAttributeValue( "EligiblePeople" ).AsGuid();

                var rockContext = new RockContext();

                // Verify person is eligible
                var person = new GroupMemberService( rockContext )
                    .Queryable().AsNoTracking()
                    .Where( m =>
                        m.Group != null &&
                        m.Group.Guid.Equals( groupGuid ) &&
                        m.GroupMemberStatus == GroupMemberStatus.Active &&
                        m.PersonId == ( CurrentPersonId ?? 0 ) )
                    .Select( m => m.Person )
                    .FirstOrDefault();

                if ( person == null )
                {
                    nbNotice.Heading = "No Access";
                    nbNotice.Text = "<p>Sorry, you do not currently belong to the group that is allowed to sync Rock with their Exchange contact list.</p>"; 
                    nbNotice.NotificationBoxType = NotificationBoxType.Warning;
                    nbNotice.Visible = true;

                    pnlEntry.Visible = false;
                }
                else
                {
                    if ( string.IsNullOrWhiteSpace( person.Email ) )
                    {
                        nbNotice.Heading = "Missing Email Address";
                        nbNotice.Text = "<p>You do not currently have an email address on your profile record. Your contacts will not sync until a valid email address is added.</p>";
                        nbNotice.NotificationBoxType = NotificationBoxType.Warning;
                        nbNotice.Visible = true;
                    }

                    cbFollowing.Checked = GetUserPreference( _keyPrefix + "IncludeFollowing" ).AsBoolean();
                    cbStaff.Checked = GetUserPreference( _keyPrefix + "IncludeStaff" ).AsBoolean();

                    var groupIds = new List<int>();
                    GetUserPreference( _keyPrefix + "Groups" ).SplitDelimitedValues().ToList().ForEach( v => groupIds.Add( v.AsInteger() ) );
                    gpGroup.SetValues( new GroupService( new RockContext() ).Queryable().AsNoTracking().Where( g => groupIds.Contains( g.Id ) ) );
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the lbSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbSave_Click( object sender, EventArgs e )
        {
            bool includeFollowing = cbFollowing.Checked;
            bool includeStaff = cbStaff.Checked;
            List<int> groupIds = gpGroup.SelectedValuesAsInt().ToList();

            SetUserPreference( _keyPrefix + "IncludeFollowing", includeFollowing.ToString() );
            SetUserPreference( _keyPrefix + "IncludeStaff", includeStaff.ToString() );
            SetUserPreference( _keyPrefix + "Groups", groupIds.AsDelimited( "|" ) );

            var transaction = new SyncContactsTransaction( GetExchangeUserData(),
                CurrentPersonId ?? 0, includeFollowing, includeStaff, groupIds );
            Rock.Transactions.RockQueue.TransactionQueue.Enqueue( transaction );

            nbNotice.Heading = "Settings Succesfully Saved";
            nbNotice.Text = "<p>Your preferences have been saved, and a request to add these people to your Exchange contact list has been queued. Your contacts will be updated in the next few minutes.</p>";
            nbNotice.NotificationBoxType = NotificationBoxType.Success;
            nbNotice.Visible = true;

            pnlEntry.Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the lbRemove control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbRemove_Click( object sender, EventArgs e )
        {
            SetUserPreference( _keyPrefix + "IncludeFollowing", "false" );
            SetUserPreference( _keyPrefix + "IncludeStaff", "false" );
            SetUserPreference( _keyPrefix + "Groups", string.Empty );
            
            cbFollowing.Checked = false;
            cbStaff.Checked = false;
            gpGroup.SetValue( null );

            var transaction = new RemoveContactsTransaction( GetExchangeUserData(), CurrentPersonId ?? 0 );
            Rock.Transactions.RockQueue.TransactionQueue.Enqueue( transaction );

            nbNotice.Heading = "Contacts Removed";
            nbNotice.Text = "<p>Your preferences have been cleared, and a request to remove all previously added people from your Exchange contact list has been queued. Your contacts will be updated in the next few minutes.</p>";
            nbNotice.NotificationBoxType = NotificationBoxType.Success;
            nbNotice.Visible = true;

            pnlEntry.Visible = false;
        }

        protected void lbTest_Click( object sender, EventArgs e )
        {
            nbNotice.Heading = "Settings Successfully Saved";
            nbNotice.Text = "<p>Your preferences have been saved, and a request to add these people to your Exchange contact list has been queued. Your contacts will be updated in the next few minutes.</p>";
            nbNotice.NotificationBoxType = NotificationBoxType.Success;
            nbNotice.Visible = true;

            string message = string.Empty;
            if ( com.minecartstudio.ExchangeContactSync.ExchangeContact.TestConnection( GetExchangeUserData(), CurrentPerson, out message ) )
            {
                nbNotice.Heading = "Successful Connection";
                nbNotice.Text = "<p>Congratulations, we were able to successfully connect to your Exchange server and we have authority to update your contacts.</p>";
                nbNotice.NotificationBoxType = NotificationBoxType.Success;
                nbNotice.Visible = true;
            }
            else
            {
                nbNotice.Heading = "Problem Connecting";
                nbNotice.Text = message;
                nbNotice.NotificationBoxType = NotificationBoxType.Danger;
                nbNotice.Visible = true;
            }

        }

        #endregion

        #region Methods

        private com.minecartstudio.ExchangeContactSync.UserData GetExchangeUserData()
        {
            string adminUsername = GetAttributeValue( "AdminUsername" );
            string adminPassword = GetAttributeValue( "AdminPassword" );
            string adminDomain = GetAttributeValue( "AdminDomain" );
            string exchangeVersion = GetAttributeValue( "ExchangeVersion" );
            string autoDiscoverUrl = GetAttributeValue( "AutoDiscoverURL" );
            bool enableTrace = GetAttributeValue( "EnableTrace" ).AsBoolean();
            bool syncBusinessPhone = GetAttributeValue( "SyncBusinessPhone" ).AsBoolean();

            return new com.minecartstudio.ExchangeContactSync.UserData( adminUsername, adminPassword, adminDomain, exchangeVersion, autoDiscoverUrl, enableTrace, syncBusinessPhone );
        }

        #endregion
    }

}