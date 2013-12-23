//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using Rock;
using Rock.Attribute;
using Rock.Model;
using Rock.Web.UI;

namespace RockWeb.Blocks.Groups
{
    /// <summary>
    /// 
    /// </summary>
    [TextField( "Treeview Title", "Group Tree View", false )]
    [GroupTypesField( "Group Types", "Select group types to show in this block.  Leave all unchecked to show all group types.", false )]
    [GroupField( "Group", "Select the root group to show in this block.", false )]
    [BooleanField( "Limit to Security Role Groups" )]
    [LinkedPage("Detail Page")]
    public partial class GroupTreeView : RockBlock
    {
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
            string groupId = PageParameter( "groupId" );

            if ( !string.IsNullOrWhiteSpace( groupId ) )
            {
                hfInitialGroupId.Value = groupId;
                hfSelectedGroupId.Value = groupId;
                Group group = ( new GroupService() ).Get( int.Parse( groupId ) );

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
}
}