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
using System.Linq;

using Rock;
using Rock.Attribute;
using Rock.Model;
using Rock.Web.UI;

namespace RockWeb.Blocks.Groups
{
    /// <summary>
    /// Navigation Tree for groups
    /// </summary>
    [DisplayName( "Group Tree View" )]
    [Category( "Groups" )]
    [Description( "Creates a navigation tree for groups of the configured group type(s)." )]

    [TextField( "Treeview Title", "Group Tree View", false )]
    [GroupTypesField( "Group Types", "Select group types to show in this block.  Leave all unchecked to show all group types.", false )]
    [GroupField( "Group", "Select the root group to show in this block.", false )]
    [BooleanField( "Limit to Security Role Groups" )]
    [LinkedPage("Detail Page")]
    public partial class GroupTreeView : RockBlock
    {

        #region Fields

        private string _groupId = string.Empty;

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            _groupId = PageParameter( "groupId" );
            hfPageRouteTemplate.Value = ( this.RockPage.RouteData.Route as System.Web.Routing.Route ).Url;

            if ( string.IsNullOrWhiteSpace( _groupId ) )
            {
                // If no group was selected, try to find the first group and redirect
                // back to current page with that group selected
                var group = FindFirstGroup();
                {
                    if ( group != null )
                    {
                        _groupId = group.Id.ToString();
                        string redirectUrl = string.Empty;

                        // redirect so that the group treeview has the first node selected right away and group detail shows the group
                        if ( hfPageRouteTemplate.Value.IndexOf("{groupId}", StringComparison.OrdinalIgnoreCase) >= 0 )
                        {
                            redirectUrl = "~/" + hfPageRouteTemplate.Value.ReplaceCaseInsensitive( "{groupId}", _groupId.ToString() );
                        }
                        else
                        {
                            redirectUrl = this.Request.Url + "?groupId=" + _groupId.ToString();
                        }

                        this.Response.Redirect( redirectUrl, false );
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            hfLimitToSecurityRoleGroups.Value = GetAttributeValue( "LimittoSecurityRoleGroups" );
            hfRootGroupId.Value = GetAttributeValue( "Group" );

            // limit GroupType selection to what Block Attributes allow
            List<Guid> groupTypeGuids = GetAttributeValue( "GroupTypes" ).SplitDelimitedValues().Select( Guid.Parse ).ToList();

            string groupTypeIds = "0";
            if ( groupTypeGuids.Any() )
            {
                groupTypeIds = new GroupTypeService().Queryable().Where( a => groupTypeGuids.Contains( a.Guid ) ).Select( a => a.Id ).ToList().AsDelimited( "," );
                groupTypeIds = string.IsNullOrWhiteSpace( groupTypeIds ) ? "0" : groupTypeIds;
            }
            hfGroupTypes.Value = groupTypeIds;

            if ( !string.IsNullOrWhiteSpace( _groupId ) )
            {
                hfInitialGroupId.Value = _groupId;
                hfSelectedGroupId.Value = _groupId;
                Group group = ( new GroupService() ).Get( int.Parse( _groupId ) );

                if ( group != null )
                {
                    // show the Add button if the selected Group's GroupType can have children
                    lbAddGroupChild.Enabled = group.GroupType.ChildGroupTypes.Count > 0;
                }
                else
                {
                    // hide the Add Button when adding a new Group
                    lbAddGroupChild.Enabled = false;
                }

                // get the parents of the selected item so we can tell the treeview to expand those
                List<string> parentIdList = new List<string>();
                while ( group != null )
                {
                    group = group.ParentGroup;
                    if ( group != null )
                    {
                        parentIdList.Insert( 0, group.Id.ToString() );
                    }
                }

                // also get any additional expanded nodes that were sent in the Post
                string postedExpandedIds = this.Request.Params["expandedIds"];
                if ( !string.IsNullOrWhiteSpace( postedExpandedIds ) )
                {
                    var postedExpandedIdList = postedExpandedIds.Split( ',' ).ToList();
                    foreach ( var id in postedExpandedIdList )
                    {
                        if ( !parentIdList.Contains( id ) )
                        {
                            parentIdList.Add( id );
                        }
                    }
                }

                hfInitialGroupParentIds.Value = parentIdList.AsDelimited( "," );
            }
            else
            {
                // let the Add button be visible if there is nothing selected
                lbAddGroupChild.Enabled = true;
            }

            // disable add child group if no group is selected
            int selectedGroupId = hfSelectedGroupId.ValueAsInt();

            if (selectedGroupId == 0)
            {
                lbAddGroupChild.Enabled = false;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the lbAddGroup control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbAddGroupRoot_Click(object sender, EventArgs e)
        {
            NavigateToLinkedPage("DetailPage", "groupId", 0, "parentGroupId", 0);
        }

        protected void lbAddGroupChild_Click(object sender, EventArgs e)
        {
            int groupId = hfSelectedGroupId.ValueAsInt();
            NavigateToLinkedPage("DetailPage", "groupId", 0, "parentGroupId", groupId);
        }

        #endregion

        #region Methods

        private Group FindFirstGroup()
        {
            var groupService = new GroupService();
            var qry = groupService.GetNavigationChildren( 0, hfRootGroupId.ValueAsInt(), hfLimitToSecurityRoleGroups.Value.AsBoolean(), hfGroupTypes.Value );

            foreach ( var group in qry )
            {
                if ( group.IsAuthorized( "View", CurrentPerson ) )
                {
                    return group;
                }
            }

            return null;
        }

        #endregion
    }
}