﻿// <copyright>
// Copyright by the Spark Development Network
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
using System.Web.UI.WebControls;
using church.ccv.Utility.Groups;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.church_ccv.Groups
{
    public class ChildWithParents_Lava : PersonService.ChildWithParents, Rock.Lava.ILiquidizable
    {
        public bool RegisteredForCamp { get; set; }

        /// <summary>
        /// To the liquid.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ToLiquid()
        {
            return this;
        }

        /// <summary>
        /// Gets the available keys (for debuging info).
        /// </summary>
        /// <value>
        /// The available keys.
        /// </value>
        [Rock.Data.LavaIgnore]
        public List<string> AvailableKeys
        {
            get
            {
                var availableKeys = new List<string> { "Child", "Parents", "RegisteredForCamp" };
                if ( this.Child != null )
                {
                    availableKeys.AddRange( this.Child.AvailableKeys );
                }

                if ( this.Parents != null )
                {
                    foreach ( Person parent in this.Parents )
                    {
                        availableKeys.AddRange( parent.AvailableKeys );
                    }
                }

                return availableKeys;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [Rock.Data.LavaIgnore]
        public object this[object key]
        {
           get
            {
               switch( key.ToStringSafe() )
               {
                   case "RegisteredForCamp": return RegisteredForCamp;
                   case "Child": return Child;
                   case "Parents": return Parents;
               }

                return null;
            }
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool ContainsKey( object key )
        {
            var additionalKeys = new List<string> { "Child", "Parents", "RegisteredForCamp" };
            if ( additionalKeys.Contains( key.ToStringSafe() ) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "NG Group Detail Lava" )]
    [Category( "CCV > Groups" )]
    [Description( "Presents the details of a NG group using Lava" )]
    [LinkedPage( "Parent Page", "The page to link to to view the parents of kids in the roster.", true, "", "" )]
    [TextField( "Camp Group Ids", "Comma delimited list of Ids for this year's camp. If set, a badge is displayed for each person registered for camp.", false)]
    public partial class NGGroupDetailLava : ToolboxGroupDetailLava
    {
        public override UpdatePanel MainPanel { get { return upnlContent; } }
        public override PlaceHolder AttributesPlaceholder { get { return phAttributes; } }
        public override Literal MainViewContent { get { return lContent; } }
        public override Literal DebugContent { get { return lDebug; } }
               
        public override Panel GroupEdit { get { return pnlGroupEdit; } }
        public override Panel GroupView { get { return pnlGroupView; } }
               
        public override Panel Schedule { get { return pnlSchedule; } }
        public override DayOfWeekPicker DayOfWeekPicker { get { return dowWeekly; } }
        public override TimePicker MeetingTime { get { return timeWeekly; } }
               
        public override Literal GroupName { get { return tbName; } }

        const int NG_GroupRole_Member = 132;
    
        protected override void FinalizePresentView( Dictionary<string, object> mergeFields, bool enableDebug )
        {
            string template = GetAttributeValue( "LavaTemplate" );

            // add our custom "parents" page, which will show the parents for the kids in the NG group.
            Dictionary<string, object> linkedPages = (Dictionary<string, object>)mergeFields["LinkedPages"];
            linkedPages.Add( "ParentPage", LinkedPageUrl("ParentPage", null ) );


            // prepare the roster, which contains kids and their parents
            List<ChildWithParents_Lava> rosterMembers = GetChildAndParents( );
                        
            mergeFields.Add( "RosterMembers", rosterMembers );

            // show debug info
            if (enableDebug && IsUserAuthorized(Authorization.EDIT))
            {
                string postbackCommands = @"<h5>Available Postback Commands</h5>
                                            <ul>
                                                <li><strong>EditGroup:</strong> Shows a panel for modifing group info. Expects a group id. <code>{{ Group.Id | Postback:'EditGroup' }}</code></li>
                                                <li><strong>AddGroupMember:</strong> Shows a panel for adding group info. Does not require input. <code>{{ '' | Postback:'AddGroupMember' }}</code></li>
                                                <li><strong>SendCommunication:</strong> Sends a communication to all group members on behalf of the Current User. This will redirect them to the communication page where they can author their email. <code>{{ '' | Postback:'SendCommunication' }}</code></li>
                                            </ul>";

                DebugContent.Visible = true;
                DebugContent.Text = mergeFields.lavaDebugInfo(null, string.Empty, postbackCommands);
            }
            
            // now set the main HTML, including lava merge fields.
            MainViewContent.Text = template.ResolveMergeFields( mergeFields ).ResolveClientIds( MainPanel.ClientID );
        }

        List<ChildWithParents_Lava> GetChildAndParents( )
        {
            List<ChildWithParents_Lava> childWithParentsList = new List<ChildWithParents_Lava>( );

            RockContext rockContext = new RockContext();
            GroupMemberService groupMemberService = new GroupMemberService( rockContext );
            GroupService groupService = new GroupService( rockContext );
            
            // get the next-gen group
            Group group = groupService.Get( _groupId );

            // build a list of valid camp group IDs
            string campGroupIdsAttrib = GetAttributeValue( "CampGroupIds" );
            List<int> campGroupIds = new List<int>( );

            if( string.IsNullOrWhiteSpace( campGroupIdsAttrib ) == false )
            {
                string[] groupIdsString = campGroupIdsAttrib.Split( ',' );
                
                foreach( string groupIdStr in groupIdsString )
                {
                    // make sure it actually converts to an int
                    int groupId = 0;
                    int.TryParse( groupIdStr, out groupId );

                    if( groupId > 0 )
                    {
                        campGroupIds.Add( groupId );
                    }
                }
            }
            

            foreach( GroupMember member in group.Members )
            {
                // first, get all the groups this person is in as a child.
                var groupsAsChild = groupMemberService.Queryable( ).Where( gm => gm.PersonId == member.Person.Id && 
                                                                           gm.GroupRole.Guid == new Guid( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_CHILD ) )
                                                                   .Select( gm => gm.Group.Id );

                // now, get the adults of these groups
                var parentsInGroup = groupMemberService.Queryable( ).Where( gm => gm.GroupRole.Guid == new Guid( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT ) && 
                                                                            groupsAsChild.Contains( gm.GroupId ) )
                                                                    .Select( gm => gm.Person ).ToList( );

                // now create a ChildWithParents object to store this
                ChildWithParents_Lava childWithParents = new ChildWithParents_Lava( );
                childWithParents.Parents = parentsInGroup;
                childWithParents.Child = member.Person;

                // if there are valid camp groups
                if( campGroupIds.Count > 0 )
                {
                    // see if they're in any of them. We don't care which ones specifically.
                    int numCampGroups = groupMemberService.Queryable( ).Where( gm => gm.PersonId == member.PersonId && campGroupIds.Contains( gm.GroupId ) ).Count( );

                    childWithParents.RegisteredForCamp = numCampGroups > 0 ? true : false;
                }

                childWithParentsList.Add( childWithParents );
            }

            return childWithParentsList;
        }
        
        protected override void HandlePageAction( string action, string parameters )
        {
            switch ( action )
            {
                case "AddGroupMember":    DisplayAddGroupMember( ); break;
                case "EditGroup":         DisplayEditGroup( );      break;

                case "SendCommunication":
                {
                    List<int?> personAliasIds = new List<int?>( );
                        
                    if( parameters == "kids" )
                    {
                        personAliasIds = new GroupMemberService( new RockContext( ) ).Queryable()
                                        .Where( m => m.GroupId == _groupId && m.GroupMemberStatus != GroupMemberStatus.Inactive )
                                        .ToList()
                                        .Select( m => m.Person.PrimaryAliasId )
                                        .ToList();
                    }
                    else if( parameters == "parents" )
                    {
                        // re-obtain the list of parents
                        List<ChildWithParents_Lava> rosterMembers = GetChildAndParents( );

                        // now take each 'family', and put each parent's primary alias id into our list
                        foreach( ChildWithParents_Lava childWithParent in rosterMembers )
                        {
                            foreach( Person parent in childWithParent.Parents )
                            {
                                personAliasIds.Add( parent.PrimaryAliasId );
                            }
                        }

                        // take only unique items (in case there're siblings. Their parents would be in the list twice, once for each kid)
                        personAliasIds = personAliasIds.Distinct( ).ToList( );
                    }

                    SendCommunication( personAliasIds );
                    break;
                }
            }
        }
        
        protected void DisplayAddGroupMember( )
        {
            // reset any error message
            nbGroupMemberErrorMessage.Title = string.Empty;
            nbGroupMemberErrorMessage.Text = string.Empty;

            pnlGroupEdit.Visible = false;
            pnlGroupView.Visible = false;
            pnlEditGroupMember.Visible = true;
            
            // populate the member statuses, but remove InActive
            rblStatus.BindToEnum<GroupMemberStatus>();
            var inactiveItem = rblStatus.Items.FindByValue( ( (int)GroupMemberStatus.Inactive ).ToString() );
            if ( inactiveItem != null )
            {
                rblStatus.Items.Remove( inactiveItem );
            }

            // set default values
            ppGroupMemberPerson.SetValue( null );
            rblStatus.SetValue( (int)GroupMemberStatus.Active );
        }
        
        /// <summary>
        /// Handles the Click event of the btnSaveGroupMember control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSaveGroupMember_Click( object sender, EventArgs e )
        {
            if( Page.IsValid && ppGroupMemberPerson.SelectedValue.HasValue )
            {
                var selectedStatus = rblStatus.SelectedValueAsEnumOrNull<GroupMemberStatus>();

                bool result = AddGroupMember( ppGroupMemberPerson.SelectedValue.Value, _groupId, NG_GroupRole_Member, selectedStatus.Value );

                if ( result )
                {
                    pnlEditGroupMember.Visible = false;
                    pnlGroupView.Visible = true;

                    DisplayViewGroup();
                }
                else
                {
                    // if so, don't add and show error message
                    var person = new PersonService( new RockContext( ) ).Get( ( int ) ppGroupMemberPerson.PersonId );

                    nbGroupMemberErrorMessage.Title = "Person Already In Group";
                    nbGroupMemberErrorMessage.Text = string.Format(
                        "{0} already belongs to this group and cannot be added again.", person.FullName );
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCancelGroupMember control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancelGroupMember_Click( object sender, EventArgs e )
        {
            pnlEditGroupMember.Visible = false;
            pnlGroupView.Visible = true;

            var sm = ScriptManager.GetCurrent( Page );
            sm.AddHistoryPoint( "Action", "ViewGroup" );
        }
    }
}