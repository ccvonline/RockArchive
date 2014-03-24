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
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using Attribute = Rock.Model.Attribute;

namespace RockWeb.Blocks.Groups
{
    [DisplayName( "Group Detail" )]
    [Category( "Groups" )]
    [Description( "Displays the details of the given group." )]
    [GroupTypesField( "Group Types", "Select group types to show in this block.  Leave all unchecked to show all group types.", false, "", "", 0 )]
    [BooleanField( "Show Edit", "", true, "", 1 )]
    [BooleanField( "Limit to Security Role Groups", "", false, "", 2 )]
    [BooleanField( "Limit to Group Types that are shown in navigation", "", false, "", 3, "LimitToShowInNavigationGroupTypes" )]
    [CodeEditorField( "Map HTML", "The HTML to use for displaying group location maps. Liquid syntax is used to render data from the following data structure: points[type, latitude, longitude], polygons[type, polygon_wkt, google_encoded_polygon]", CodeEditorMode.Liquid, CodeEditorTheme.Rock, 300, false, @"
    {% for point in points %}
        <div class='group-location-map'>
            <h4>{{ point.type }}</h4>
            <img src='http://maps.googleapis.com/maps/api/staticmap?sensor=false&size=350x200&zoom=13&format=png&style=feature:all|saturation:0|hue:0xe7ecf0&style=feature:road|saturation:-70&style=feature:transit|visibility:off&style=feature:poi|visibility:off&style=feature:water|visibility:simplified|saturation:-60&markers=color:0x779cb1|{{ point.latitude }},{{ point.longitude }}&visual_refresh=true'/>
        </div>
    {% endfor %}
    {% for polygon in polygons %}
        <div class='group-location-map'>
            <h4>{{ polygon.type }}</h4>
            <img src='http://maps.googleapis.com/maps/api/staticmap?sensor=false&size=350x200&format=png&style=feature:all|saturation:0|hue:0xe7ecf0&style=feature:road|saturation:-70&style=feature:transit|visibility:off&style=feature:poi|visibility:off&style=feature:water|visibility:simplified|saturation:-60&visual_refresh=true&path=fillcolor:0x779cb155|color:0xFFFFFF00|enc:{{ polygon.google_encoded_polygon }}'/>
        </div>
    {% endfor %}
", "", 4 )]
    public partial class GroupDetail : RockBlock, IDetailBlock
    {
        #region Constants

        private const string MEMBER_LOCATION_TAB_TITLE = "Member Location";
        private const string OTHER_LOCATION_TAB_TITLE = "Other Location";

        #endregion

        #region Fields

        private readonly List<string> _tabs = new List<string> { MEMBER_LOCATION_TAB_TITLE, OTHER_LOCATION_TAB_TITLE };

        private string LocationTypeTab
        {
            get
            {
                object currentProperty = ViewState["LocationTypeTab"];
                return currentProperty != null ? currentProperty.ToString() : MEMBER_LOCATION_TAB_TITLE;
            }

            set
            {
                ViewState["LocationTypeTab"] = value;
            }
        }

        #endregion

        #region Child Grid Dictionarys

        /// <summary>
        /// Gets or sets the state of the location.
        /// </summary>
        /// <value>
        /// The state of the location.
        /// </value>
        private ViewStateList<GroupLocation> GroupLocationsState
        {
            get
            {
                return ViewState["GroupLocationsState"] as ViewStateList<GroupLocation>;
            }

            set
            {
                ViewState["GroupLocationsState"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the group member inherited attributes.
        /// </summary>
        /// <value>
        /// The group member inherited attributes.
        /// </value>
        private List<InheritedAttribute> GroupMemberAttributesInheritedState
        {
            get
            {
                return ViewState["GroupMemberAttributesInheritedState"] as List<InheritedAttribute>;
            }
            set
            {
                ViewState["GroupMemberAttributesInheritedState"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the state of the group member attributes.
        /// </summary>
        /// <value>
        /// The state of the group member attributes.
        /// </value>
        private ViewStateList<Attribute> GroupMemberAttributesState
        {
            get
            {
                return ViewState["GroupMemberAttributesState"] as ViewStateList<Attribute>;
            }

            set
            {
                ViewState["GroupMemberAttributesState"] = value;
            }
        }

        private bool AllowMultipleLocations
        {
            get { return ViewState["AllowMultipleLocations"] as bool? ?? false; }
            set { ViewState["AllowMultipleLocations"] = value; }
        }

        #endregion

        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            gLocations.DataKeyNames = new string[] { "Guid" };
            gLocations.Actions.AddClick += gLocations_Add;
            gLocations.GridRebind += gLocations_GridRebind;

            gGroupMemberAttributesInherited.Actions.ShowAdd = false;
            gGroupMemberAttributesInherited.EmptyDataText = Server.HtmlEncode( None.Text );
            gGroupMemberAttributesInherited.GridRebind += gGroupMemberAttributesInherited_GridRebind;

            gGroupMemberAttributes.DataKeyNames = new string[] { "Guid" };
            gGroupMemberAttributes.Actions.ShowAdd = true;
            gGroupMemberAttributes.Actions.AddClick += gGroupMemberAttributes_Add;
            gGroupMemberAttributes.EmptyDataText = Server.HtmlEncode( None.Text );
            gGroupMemberAttributes.GridRebind += gGroupMemberAttributes_GridRebind;
            gGroupMemberAttributes.GridReorder += gGroupMemberAttributes_GridReorder;

            btnDelete.Attributes["onclick"] = string.Format( "javascript: return Rock.dialogs.confirmDelete(event, '{0}');", Group.FriendlyTypeName );
            btnSecurity.EntityTypeId = EntityTypeCache.Read( typeof( Rock.Model.Group ) ).Id;
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
                string itemId = PageParameter( "groupId" );
                string parentGroupId = PageParameter( "parentGroupId" );

                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    if ( string.IsNullOrWhiteSpace( parentGroupId ) )
                    {
                        ShowDetail( "groupId", int.Parse( itemId ) );
                    }
                    else
                    {
                        ShowDetail( "groupId", int.Parse( itemId ), int.Parse( parentGroupId ) );
                    }
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
            else
            {
                ShowDialog();
            }

            // Rebuild the attribute controls on postback based on group type
            if ( pnlDetails.Visible )
            {
                var group = new Group { GroupTypeId = ddlGroupType.SelectedValueAsInt() ?? 0 };
                if ( group.GroupTypeId > 0 )
                {
                    ShowGroupTypeEditDetails( GroupTypeCache.Read(group.GroupTypeId), group, false );
                }
            }
        }

        /// <summary>
        /// Saves any user control view-state changes that have occurred since the last page postback.
        /// </summary>
        /// <returns>
        /// Returns the user control's current view state. If there is no view state associated with the control, it returns null.
        /// </returns>
        protected override object SaveViewState()
        {
            if ( GroupLocationsState != null)
            {
                GroupLocationsState.SaveViewState();
            }

            if ( GroupMemberAttributesState != null )
            {
                GroupMemberAttributesState.SaveViewState();
            }

            return base.SaveViewState();
        }

        /// <summary>
        /// Returns breadcrumbs specific to the block that should be added to navigation
        /// based on the current page reference.  This function is called during the page's
        /// oninit to load any initial breadcrumbs.
        /// </summary>
        /// <param name="pageReference">The <see cref="Rock.Web.PageReference" />.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{BreadCrumb}" /> of block related <see cref="Rock.Web.UI.BreadCrumb">BreadCrumbs</see>.
        /// </returns>
        public override List<BreadCrumb> GetBreadCrumbs( PageReference pageReference )
        {
            var breadCrumbs = new List<BreadCrumb>();

            int? groupId = PageParameter( pageReference, "groupId" ).AsInteger();
            if ( groupId != null )
            {
                Group group = new GroupService().Get( groupId.Value );
                if ( group != null )
                {
                    breadCrumbs.Add( new BreadCrumb( group.Name, pageReference ) );
                }
                else
                {
                    breadCrumbs.Add( new BreadCrumb( "New Group", pageReference ) );
                }
            }
            else
            {
                // don't show a breadcrumb if we don't have a pageparam to work with
            }

            return breadCrumbs;
        }

        #endregion

        #region Edit Events

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnEdit_Click( object sender, EventArgs e )
        {
            ShowEditDetails( GetGroup( hfGroupId.Value.AsInteger() ?? 0 ) );
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnDelete_Click( object sender, EventArgs e )
        {
            int? parentGroupId = null;

            // NOTE: Very similar code in GroupList.gGroups_Delete
            RockTransactionScope.WrapTransaction( () =>
            {
                GroupService groupService = new GroupService();
                AuthService authService = new AuthService();
                Group group = groupService.Get( int.Parse( hfGroupId.Value ) );

                if ( group != null )
                {
                    if ( !group.IsAuthorized( Authorization.EDIT, this.CurrentPerson ) )
                    {
                        mdDeleteWarning.Show( "You are not authorized to delete this group.", ModalAlertType.Information );
                        return;
                    }

                    parentGroupId = group.ParentGroupId;
                    string errorMessage;
                    if ( !groupService.CanDelete( group, out errorMessage ) )
                    {
                        mdDeleteWarning.Show( errorMessage, ModalAlertType.Information );
                        return;
                    }

                    bool isSecurityRoleGroup = group.IsSecurityRole || group.GroupType.Guid.Equals( Rock.SystemGuid.GroupType.GROUPTYPE_SECURITY_ROLE.AsGuid() );
                    if ( isSecurityRoleGroup )
                    {
                        Rock.Security.Role.Flush( group.Id );
                        foreach ( var auth in authService.Queryable().Where( a => a.GroupId == group.Id ).ToList() )
                        {
                            authService.Delete( auth, CurrentPersonAlias );
                            authService.Save( auth, CurrentPersonAlias );
                        }
                    }

                    groupService.Delete( group, CurrentPersonAlias );
                    groupService.Save( group, CurrentPersonAlias );

                    if ( isSecurityRoleGroup )
                    {
                        Rock.Security.Authorization.Flush();
                    }
                }

                // reload page, selecting the deleted group's parent
                var qryParams = new Dictionary<string, string>();
                if ( parentGroupId != null )
                {
                    qryParams["groupId"] = parentGroupId.ToString();
                }

                NavigateToPage( RockPage.Guid, qryParams );
            } );
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            Group group;
            bool wasSecurityRole = false;

            using ( new UnitOfWorkScope() )
            {
                GroupService groupService = new GroupService();
                GroupLocationService groupLocationService = new GroupLocationService();
                AttributeService attributeService = new AttributeService();
                AttributeQualifierService attributeQualifierService = new AttributeQualifierService();
                CategoryService categoryService = new CategoryService();

                if ( ( ddlGroupType.SelectedValueAsInt() ?? 0 ) == 0 )
                {
                    ddlGroupType.ShowErrorMessage( Rock.Constants.WarningMessage.CannotBeBlank( GroupType.FriendlyTypeName ) );
                    return;
                }

                int groupId = int.Parse( hfGroupId.Value );

                if ( groupId == 0 )
                {
                    group = new Group();
                    group.IsSystem = false;
                    group.Name = string.Empty;
                }
                else
                {
                    group = groupService.Get( groupId );
                    wasSecurityRole = group.IsSecurityRole;

                    var selectedLocations = GroupLocationsState.Select( l => l.Guid );
                    foreach( var groupLocation in group.GroupLocations.Where( l => !selectedLocations.Contains( l.Guid)).ToList() )
                    {
                        group.GroupLocations.Remove( groupLocation );
                        groupLocationService.Delete( groupLocation, CurrentPersonAlias );
                    }
                }

                foreach ( var groupLocationState in GroupLocationsState )
                {
                    GroupLocation groupLocation = group.GroupLocations.Where( l => l.Guid == groupLocationState.Guid).FirstOrDefault();
                    if (groupLocation == null)
                    {
                        groupLocation = new GroupLocation();
                        group.GroupLocations.Add( groupLocation );
                    }
                    else
                    {
                        groupLocationState.Id = groupLocation.Id;
                        groupLocationState.Guid = groupLocation.Guid;
                    }

                    groupLocation.CopyPropertiesFrom( groupLocationState );
                }

                group.Name = tbName.Text;
                group.Description = tbDescription.Text;
                group.CampusId = ddlCampus.SelectedValue.Equals( None.IdValue ) ? (int?)null : int.Parse( ddlCampus.SelectedValue );
                group.GroupTypeId = int.Parse( ddlGroupType.SelectedValue );
                group.ParentGroupId = gpParentGroup.SelectedValue.Equals( None.IdValue ) ? (int?)null : int.Parse( gpParentGroup.SelectedValue );
                group.IsSecurityRole = cbIsSecurityRole.Checked;
                group.IsActive = cbIsActive.Checked;

                if ( group.ParentGroupId == group.Id )
                {
                    gpParentGroup.ShowErrorMessage( "Group cannot be a Parent Group of itself." );
                    return;
                }

                group.LoadAttributes();

                Rock.Attribute.Helper.GetEditValues( phGroupAttributes, group );

                if ( !Page.IsValid )
                {
                    return;
                }

                if ( !group.IsValid )
                {
                    // Controls will render the error messages                    
                    return;
                }

                RockTransactionScope.WrapTransaction( () =>
                {
                    if ( group.Id.Equals( 0 ) )
                    {
                        groupService.Add( group, CurrentPersonAlias );
                    }

                    groupService.Save( group, CurrentPersonAlias );
                    group.SaveAttributeValues( CurrentPersonAlias );

                    /* Take care of Group Member Attributes */
                    var entityTypeId = EntityTypeCache.Read( typeof( GroupMember ) ).Id;
                    string qualifierColumn = "GroupId";
                    string qualifierValue = group.Id.ToString();

                    // Get the existing attributes for this entity type and qualifier value
                    var attributes = attributeService.Get( entityTypeId, qualifierColumn, qualifierValue );

                    // Delete any of those attributes that were removed in the UI
                    var selectedAttributeGuids = GroupMemberAttributesState.Select( a => a.Guid );
                    foreach ( var attr in attributes.Where( a => !selectedAttributeGuids.Contains( a.Guid ) ) )
                    {
                        Rock.Web.Cache.AttributeCache.Flush( attr.Id );

                        attributeService.Delete( attr, CurrentPersonAlias );
                        attributeService.Save( attr, CurrentPersonAlias );
                    }

                    // Update the Attributes that were assigned in the UI
                    foreach ( var attributeState in GroupMemberAttributesState )
                    {
                        Rock.Attribute.Helper.SaveAttributeEdits( attributeState, attributeService, attributeQualifierService, categoryService,
                            entityTypeId, qualifierColumn, qualifierValue, CurrentPersonAlias );
                    }
                } );

            }

            if ( group != null && wasSecurityRole )
            {
                if ( !group.IsSecurityRole )
                {
                    // if this group was a SecurityRole, but no longer is, flush
                    Rock.Security.Role.Flush( group.Id );
                    Rock.Security.Authorization.Flush();
                }
            }
            else
            {
                if ( group.IsSecurityRole )
                {
                    // new security role, flush
                    Rock.Security.Authorization.Flush();
                }
            }

            var qryParams = new Dictionary<string, string>();
            qryParams["groupId"] = group.Id.ToString();

            NavigateToPage( RockPage.Guid, qryParams );
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            if ( hfGroupId.Value.Equals( "0" ) )
            {
                if ( RockPage.Layout.FileName.Equals( "TwoColumnLeft" ) )
                {
                    // Cancelling on Add.  Return to tree view with parent category selected
                    var qryParams = new Dictionary<string, string>();

                    string parentGroupId = PageParameter( "parentGroupId" );
                    if ( !string.IsNullOrWhiteSpace( parentGroupId ) )
                    {
                        qryParams["groupId"] = parentGroupId;
                    }

                    NavigateToPage( RockPage.Guid, qryParams );
                }
                else
                {
                    // Cancelling on Add.  Return to Grid
                    NavigateToParentPage();
                }
            }
            else
            {
                // Cancelling on Edit.  Return to Details
                ShowReadonlyDetails( GetGroup( hfGroupId.Value.AsInteger() ?? 0 ) );
            }
        }

        #endregion

        #region Control Events

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlParentGroup control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlParentGroup_SelectedIndexChanged( object sender, EventArgs e )
        {
            GroupTypeService groupTypeService = new GroupTypeService();
            var groupTypeQry = groupTypeService.Queryable();

            // limit GroupType selection to what Block Attributes allow
            List<Guid> groupTypeGuids = GetAttributeValue( "GroupTypes" ).SplitDelimitedValues().Select( a => Guid.Parse( a ) ).ToList();
            if ( groupTypeGuids.Count > 0 )
            {
                groupTypeQry = groupTypeQry.Where( a => groupTypeGuids.Contains( a.Guid ) );
            }

            // next, limit GroupType to ChildGroupTypes that the ParentGroup allows
            int? parentGroupId = gpParentGroup.SelectedValueAsInt();
            if ( ( parentGroupId ?? 0 ) != 0 )
            {
                Group parentGroup = new GroupService().Queryable( "GroupType" ).Where( g => g.Id == parentGroupId.Value ).FirstOrDefault();
                List<int> allowedChildGroupTypeIds = parentGroup.GroupType.ChildGroupTypes.Select( a => a.Id ).ToList();
                groupTypeQry = groupTypeQry.Where( a => allowedChildGroupTypeIds.Contains( a.Id ) );
            }

            // limit to GroupTypes where ShowInNavigation=True depending on block setting
            if (GetAttributeValue("LimitToShowInNavigationGroupTypes").AsBoolean())
            {
                groupTypeQry = groupTypeQry.Where( a => a.ShowInNavigation );
            }

            List<GroupType> groupTypes = groupTypeQry.OrderBy( a => a.Name ).ToList();
            if ( groupTypes.Count() > 1 )
            {
                // add a empty option so they are forced to choose
                groupTypes.Insert( 0, new GroupType { Id = 0, Name = string.Empty } );
            }

            // If the currently selected GroupType isn't an option anymore, set selected GroupType to null
            int? selectedGroupTypeId = ddlGroupType.SelectedValueAsInt();
            if ( ddlGroupType.SelectedValue != null )
            {
                
                if ( !groupTypes.Any( a => a.Id.Equals( selectedGroupTypeId ?? 0 ) ) )
                {
                    selectedGroupTypeId = null;
                }
            }

            ddlGroupType.DataSource = groupTypes;
            ddlGroupType.DataBind();

            if ( selectedGroupTypeId.HasValue )
            {
                ddlGroupType.SelectedValue = selectedGroupTypeId.ToString();
            }
            else
            {
                ddlGroupType.SelectedValue = null;
            }
        }

        /// <summary>
        /// Handles the Click event of the lbProperty control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbLocationType_Click( object sender, EventArgs e )
        {
            LinkButton lb = sender as LinkButton;
            if ( lb != null )
            {
                LocationTypeTab = lb.Text;

                rptLocationTypes.DataSource = _tabs;
                rptLocationTypes.DataBind();
            }

            ShowSelectedPane();
        }        
        
        #endregion

        #region Internal Methods

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            ShowDetail( itemKey, itemKeyValue, null );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The group id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? parentGroupId )
        {
            pnlDetails.Visible = false;

            if ( !itemKey.Equals( "groupId" ) )
            {
                return;
            }

            bool editAllowed = true; 

            Group group = null;

            if ( !itemKeyValue.Equals( 0 ) )
            {
                group = GetGroup( itemKeyValue );
                if ( group != null )
                {
                    editAllowed = group.IsAuthorized( Authorization.EDIT, CurrentPerson );
                }
            }
            else
            {
                group = new Group { Id = 0, IsActive = true, ParentGroupId = parentGroupId };
                wpGeneral.Expanded = true;
            }

            if ( group == null )
            {
                return;
            }

            pnlDetails.Visible = true;
            hfGroupId.Value = group.Id.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !editAllowed || !IsUserAuthorized( Authorization.EDIT ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Group.FriendlyTypeName );
            }

            if ( group.IsSystem )
            {
                nbEditModeMessage.Text = EditModeMessage.System( Group.FriendlyTypeName );
            }

            var roleLimitWarnings = new StringBuilder();

            if ( group.GroupType != null && group.GroupType.Roles != null && group.GroupType.Roles.Any() )
            {
                foreach ( var role in group.GroupType.Roles )
                {
                    int curCount = 0;
                    if ( group.Members != null )
                    {
                        curCount = group.Members
                            .Where( m => m.GroupRoleId == role.Id && m.GroupMemberStatus == GroupMemberStatus.Active )
                            .Count();
                    }

                    if ( role.MinCount.HasValue && role.MinCount.Value > curCount )
                    {
                        roleLimitWarnings.AppendFormat( "The <strong>{1}</strong> role is currently below its minimum requirement of {2:N0} active {3}.<br/>",
                            role.Name.Pluralize(), role.Name, role.MinCount, role.MinCount == 1 ? group.GroupType.GroupMemberTerm : group.GroupType.GroupMemberTerm.Pluralize() );
                    }
                    if ( role.MaxCount.HasValue && role.MaxCount.Value < curCount )
                    {
                        roleLimitWarnings.AppendFormat( "The <strong>{1}</strong> role is currently above its maximum limit of {2:N0} active {3}.<br/>",
                            role.Name.Pluralize(), role.Name, role.MaxCount, role.MaxCount == 1 ? group.GroupType.GroupMemberTerm : group.GroupType.GroupMemberTerm.Pluralize() );
                    }
                }
            }

            nbRoleLimitWarning.Text = roleLimitWarnings.ToString();
            nbRoleLimitWarning.Visible = roleLimitWarnings.Length > 0;

            if ( readOnly )
            {
                btnEdit.Visible = false;
                btnDelete.Visible = false;
                ShowReadonlyDetails( group );
            }
            else
            {
                btnEdit.Visible = true;
                btnDelete.Visible = !group.IsSystem;
                if ( group.Id > 0 )
                {
                    ShowReadonlyDetails( group );
                }
                else
                {
                    ShowEditDetails( group );
                }
            }

        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="group">The group.</param>
        private void ShowEditDetails( Group group )
        {
            if ( group.Id == 0 )
            {
                lReadOnlyTitle.Text = ActionTitle.Add( Group.FriendlyTypeName ).FormatAsHtmlTitle();
                hlInactive.Visible = false;
            }
            else
            {
                lReadOnlyTitle.Text = group.Name.FormatAsHtmlTitle();
            }

            SetEditMode( true );

            tbName.Text = group.Name;
            tbDescription.Text = group.Description;
            cbIsSecurityRole.Checked = group.IsSecurityRole;
            cbIsActive.Checked = group.IsActive;

            using ( new UnitOfWorkScope() )
            {
                var groupService = new GroupService();
                var attributeService = new AttributeService();

                LoadDropDowns();

                gpParentGroup.SetValue( group.ParentGroup ?? groupService.Get( group.ParentGroupId ?? 0 ) );

                // GroupType depends on Selected ParentGroup
                ddlParentGroup_SelectedIndexChanged( null, null );
                gpParentGroup.Label = "Parent Group";

                if ( group.Id == 0 && ddlGroupType.Items.Count > 1 )
                {
                    if ( GetAttributeValue( "LimittoSecurityRoleGroups" ).FromTrueFalse() )
                    {
                        // default GroupType for new Group to "Security Roles"  if LimittoSecurityRoleGroups
                        var securityRoleGroupType = GroupTypeCache.GetSecurityRoleGroupType();
                        if ( securityRoleGroupType != null )
                        {
                            ddlGroupType.SetValue( securityRoleGroupType.Id );
                        }
                        else
                        {
                            ddlGroupType.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        // if this is a new group (and not "LimitToSecurityRoleGroups", and there is more than one choice for GroupType, default to no selection so they are forced to choose (vs unintentionallly choosing the default one)
                        ddlGroupType.SelectedIndex = 0;
                    }
                }
                else
                {
                    ddlGroupType.SetValue( group.GroupTypeId );
                }

                ddlCampus.SetValue( group.CampusId );

                GroupLocationsState = new ViewStateList<GroupLocation>();
                foreach ( var groupLocation in group.GroupLocations )
                {
                    var groupLocationState = new GroupLocation();
                    groupLocationState.CopyPropertiesFrom( groupLocation );
                    if ( groupLocation.Location != null )
                    {
                        groupLocationState.Location = new Location();
                        groupLocationState.Location.CopyPropertiesFrom( groupLocation.Location );
                    }
                    if ( groupLocation.GroupLocationTypeValue != null)
                    {
                        groupLocationState.GroupLocationTypeValue = new DefinedValue();
                        groupLocationState.GroupLocationTypeValue.CopyPropertiesFrom( groupLocation.GroupLocationTypeValue );
                    }

                    GroupLocationsState.Add( groupLocationState );
                }

                ShowGroupTypeEditDetails( GroupTypeCache.Read(group.GroupTypeId), group, true);

                // if this block's attribute limit group to SecurityRoleGroups, don't let them edit the SecurityRole checkbox value
                if ( GetAttributeValue( "LimittoSecurityRoleGroups" ).FromTrueFalse() )
                {
                    cbIsSecurityRole.Enabled = false;
                    cbIsSecurityRole.Checked = true;
                }

                string qualifierValue = group.Id.ToString();
                GroupMemberAttributesState = new ViewStateList<Attribute>();
                GroupMemberAttributesState.AddAll( attributeService.GetByEntityTypeId( new GroupMember().TypeId ).AsQueryable()
                        .Where( a =>
                            a.EntityTypeQualifierColumn.Equals( "GroupId", StringComparison.OrdinalIgnoreCase ) &&
                            a.EntityTypeQualifierValue.Equals( qualifierValue ) )
                        .OrderBy( a => a.Order )
                        .ThenBy( a => a.Name )
                        .ToList() );
                BindGroupMemberAttributesGrid();

                BindInheritedAttributes( group.GroupTypeId, attributeService );
            }
        }

        private void ShowGroupTypeEditDetails(GroupTypeCache groupType, Group group, bool setValues)
        {
            if ( group != null )
            {
                // Save value to viewstate for use later when binding location grid
                AllowMultipleLocations = groupType != null && groupType.AllowMultipleLocations;

                if ( groupType != null && groupType.LocationSelectionMode != GroupLocationPickerMode.None )
                {
                    wpLocations.Visible = true;
                    wpLocations.Title = AllowMultipleLocations ? "Locations" : "Location";
                    BindLocationsGrid();
                }
                else
                {
                    wpLocations.Visible = false;
                }

                phGroupAttributes.Controls.Clear();
                group.LoadAttributes();

                if ( group.Attributes != null && group.Attributes.Any() )
                {
                    wpGroupAttributes.Visible = true;
                    Rock.Attribute.Helper.AddEditControls( group, phGroupAttributes, setValues, "GroupDetail" );
                }
                else
                {
                    wpGroupAttributes.Visible = false;
                }
            }

        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="group">The group.</param>
        private void ShowReadonlyDetails( Group group )
        {
            SetEditMode( false );

            string groupIconHtml = !string.IsNullOrWhiteSpace( group.GroupType.IconCssClass ) ?
                groupIconHtml = string.Format( "<i class='{0}' ></i>", group.GroupType.IconCssClass ) : "";

            hfGroupId.SetValue( group.Id );
            lGroupIconHtml.Text = groupIconHtml;
            lReadOnlyTitle.Text = group.Name.FormatAsHtmlTitle();

            hlInactive.Visible = !group.IsActive;
            hlType.Text = group.GroupType.Name;

            lGroupDescription.Text = group.Description;

            DescriptionList descriptionList = new DescriptionList();

            if ( group.ParentGroup != null )
            {
                descriptionList.Add( "Parent Group", group.ParentGroup.Name );
            }

            if ( group.Campus != null )
            {
                hlCampus.Visible = true;
                hlCampus.Text = group.Campus.Name;
            }
            else
            {
                hlCampus.Visible = false;
            }


            lblMainDetails.Text = descriptionList.Html;

            var attributes = new List<Rock.Web.Cache.AttributeCache>();

            // Get the attributes inherited from group type
            GroupType groupType = new GroupTypeService().Get( group.GroupTypeId );
            groupType.LoadAttributes();
            attributes = groupType.Attributes.Select( a => a.Value ).OrderBy( a => a.Order ).ThenBy( a => a.Name ).ToList();

            // Combine with the group attributes
            group.LoadAttributes();
            attributes.AddRange( group.Attributes.Select( a => a.Value ).OrderBy( a => a.Order ).ThenBy( a => a.Name ) );

            // display attribute values
            var attributeCategories = Helper.GetAttributeCategories( attributes );
            Rock.Attribute.Helper.AddDisplayControls( group, attributeCategories, phAttributes );

            // Get all the group locations and group all those that have a geo-location into either points or polygons
            var points = new List<GroupLocation>();
            var polygons = new List<GroupLocation>();
            foreach ( GroupLocation groupLocation in group.GroupLocations )
            {
                if ( groupLocation.Location != null )
                {
                    if ( groupLocation.Location.GeoPoint != null )
                    {
                        points.Add( groupLocation );
                    }
                    else if ( groupLocation.Location.GeoFence != null )
                    {
                        polygons.Add( groupLocation );
                    }
                }
            }

            var dict = new Dictionary<string, object>();
            if ( points.Any() )
            {
                var pointsList = new List<Dictionary<string, object>>();
                foreach ( var groupLocation in points)
                {
                    var pointsDict = new Dictionary<string, object>();
                    if ( groupLocation.GroupLocationTypeValue != null )
                    {
                        pointsDict.Add( "type", groupLocation.GroupLocationTypeValue.Name );
                    }
                    pointsDict.Add( "latitude", groupLocation.Location.GeoPoint.Latitude );
                    pointsDict.Add( "longitude", groupLocation.Location.GeoPoint.Longitude );
                    pointsList.Add( pointsDict );
                }
                dict.Add( "points", pointsList );
            }

            if ( polygons.Any() )
            {
                var polygonsList = new List<Dictionary<string, object>>();
                foreach ( var groupLocation in polygons )
                {
                    var polygonDict = new Dictionary<string, object>();
                    if ( groupLocation.GroupLocationTypeValue != null )
                    {
                        polygonDict.Add( "type", groupLocation.GroupLocationTypeValue.Name );
                    }
                    polygonDict.Add( "polygon_wkt", groupLocation.Location.GeoFence.AsText());
                    polygonDict.Add( "google_encoded_polygon", groupLocation.Location.EncodeGooglePolygon() );
                    polygonsList.Add( polygonDict );
                }
                dict.Add( "polygons", polygonsList );
            }

            phMaps.Controls.Clear();
            phMaps.Controls.Add( new LiteralControl( GetAttributeValue( "MapHTML" ).ResolveMergeFields( dict ) ) );

            btnSecurity.Visible = group.IsAuthorized( Authorization.ADMINISTRATE, CurrentPerson );
            btnSecurity.Title = group.Name;
            btnSecurity.EntityId = group.Id;

        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;
            fieldsetViewDetails.Visible = !editable;

            this.HideSecondaryBlocks( editable );
        }

        private Group GetGroup( int groupId )
        {
            string key = string.Format( "Group:{0}", groupId );
            Group group = RockPage.GetSharedItem( key ) as Group;
            if ( group == null )
            {
                group = new GroupService().Queryable( "GroupType" )
                    .Where( g => g.Id == groupId )
                    .FirstOrDefault();
                RockPage.SaveSharedItem( key, group );
            }

            return group;
        }

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            CampusService campusService = new CampusService();
            List<Campus> campuses = campusService.Queryable().OrderBy( a => a.Name ).ToList();
            campuses.Insert( 0, new Campus { Id = None.Id, Name = None.Text } );
            ddlCampus.DataSource = campuses;
            ddlCampus.DataBind();
        }

        private void ShowDialog( string dialog, bool setValues = false )
        {
            hfActiveDialog.Value = dialog.ToUpper().Trim();
            ShowDialog( setValues );
        }

        private void ShowDialog( bool setValues = false )
        {
            switch ( hfActiveDialog.Value )
            {
                case "LOCATIONS":
                    dlgLocations.Show();
                    break;
                case "GROUPMEMBERATTRIBUTES":
                    dlgGroupMemberAttribute.Show();
                    break;
            }
        }

        private void HideDialog()
        {
            switch ( hfActiveDialog.Value )
            {
                case "LOCATIONS":
                    dlgLocations.Hide();
                    break;
                case "GROUPMEMBERATTRIBUTES":
                    dlgGroupMemberAttribute.Hide();
                    break;
            }

            hfActiveDialog.Value = string.Empty;
        }

        /// <summary>
        /// Gets the tab class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        protected string GetTabClass( object property )
        {
            if ( property.ToString() == LocationTypeTab )
            {
                return "active";
            }

            return string.Empty;
        }

        private void ShowSelectedPane()
        {
            if ( LocationTypeTab.Equals( MEMBER_LOCATION_TAB_TITLE ) )
            {
                pnlMemberSelect.Visible = true;
                pnlLocationSelect.Visible = false;
            }
            else if ( LocationTypeTab.Equals( OTHER_LOCATION_TAB_TITLE ) )
            {
                pnlMemberSelect.Visible = false;
                pnlLocationSelect.Visible = true;
            }
        }
        
        /// <summary>
        /// Gets the number of active members in a group role.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="roleId">The role id.</param>
        /// <returns></returns>
        private int GetGroupRoleMemberCount( int groupId, int roleId )
        {
            return new GroupMemberService().Queryable()
                        .Where( m => m.GroupId == groupId )
                        .Where( m => m.GroupRoleId == roleId )
                        .Where( m => m.GroupMemberStatus == GroupMemberStatus.Active )
                        .Count();
        }

        private void BindInheritedAttributes( int? inheritedGroupTypeId, AttributeService attributeService )
        {
            GroupMemberAttributesInheritedState = new List<InheritedAttribute>();

            while ( inheritedGroupTypeId.HasValue )
            {
                var inheritedGroupType = GroupTypeCache.Read( inheritedGroupTypeId.Value );
                if ( inheritedGroupType != null )
                {
                    string qualifierValue = inheritedGroupType.Id.ToString();

                    foreach ( var attribute in attributeService.GetByEntityTypeId( new GroupMember().TypeId ).AsQueryable()
                        .Where( a =>
                            a.EntityTypeQualifierColumn.Equals( "GroupTypeId", StringComparison.OrdinalIgnoreCase ) &&
                            a.EntityTypeQualifierValue.Equals( qualifierValue ) )
                        .OrderBy( a => a.Order )
                        .ThenBy( a => a.Name )
                        .ToList() )
                    {
                        GroupMemberAttributesInheritedState.Add( new InheritedAttribute( attribute.Name, attribute.Description,
                            Page.ResolveUrl( "~/GroupType/" + attribute.EntityTypeQualifierValue ), inheritedGroupType.Name ) );
                    }

                    inheritedGroupTypeId = inheritedGroupType.InheritedGroupTypeId;
                }
                else
                {
                    inheritedGroupTypeId = null;
                }
            }

            BindGroupMemberAttributesInheritedGrid();
        }

        private void SetAttributeListOrder( ViewStateList<Attribute> attributeList )
        {
            int order = 0;
            attributeList.OrderBy( a => a.Order ).ThenBy( a => a.Name ).ToList().ForEach( a => a.Order = order++ );
        }

        public virtual void ReorderAttributeList( ViewStateList<Attribute> attributeList, int oldIndex, int newIndex )
        {
            var movedItem = attributeList.Where( a => a.Order == oldIndex ).FirstOrDefault();
            if ( movedItem != null )
            {
                foreach ( var otherItem in attributeList.Where( a => a.Order != oldIndex && a.Order >= newIndex ) )
                {
                    otherItem.Order = otherItem.Order + 1;
                }
                movedItem.Order = newIndex;
            }
        }

        #endregion

        #region Location Grid and Picker

        /// <summary>
        /// Handles the Add event of the gLocations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gLocations_Add( object sender, EventArgs e )
        {
            gLocations_ShowEdit( Guid.Empty );
        }

        /// <summary>
        /// Handles the Edit event of the gLocations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gLocations_Edit( object sender, RowEventArgs e )
        {
            Guid locationGuid = (Guid)e.RowKeyValue;
            gLocations_ShowEdit( locationGuid );
        }

        /// <summary>
        /// Gs the locations_ show edit.
        /// </summary>
        /// <param name="locationGuid">The location unique identifier.</param>
        protected void gLocations_ShowEdit( Guid locationGuid )
        {
            using ( new UnitOfWorkScope() )
            {
                ddlMember.Items.Clear();

                int? groupTypeId = ddlGroupType.SelectedValueAsId();
                if ( groupTypeId.HasValue )
                {
                    var groupType = GroupTypeCache.Read( groupTypeId.Value );
                    if ( groupType != null )
                    {
                        GroupLocationPickerMode groupTypeModes = groupType.LocationSelectionMode;
                        if ( groupTypeModes != GroupLocationPickerMode.None )
                        {
                            // Set the location picker modes allowed based on the group type's allowed modes
                            LocationPickerMode modes = LocationPickerMode.None;
                            if ( ( groupTypeModes & GroupLocationPickerMode.Named ) == GroupLocationPickerMode.Named )
                            {
                                modes = modes | LocationPickerMode.Named;
                            }
                            if ( ( groupTypeModes & GroupLocationPickerMode.Address ) == GroupLocationPickerMode.Address )
                            {
                                modes = modes | LocationPickerMode.Address;
                            }
                            if ( ( groupTypeModes & GroupLocationPickerMode.Point ) == GroupLocationPickerMode.Point )
                            {
                                modes = modes | LocationPickerMode.Point;
                            }
                            if ( ( groupTypeModes & GroupLocationPickerMode.Polygon ) == GroupLocationPickerMode.Polygon )
                            {
                                modes = modes | LocationPickerMode.Polygon;
                            }

                            bool displayMemberTab = ( groupTypeModes & GroupLocationPickerMode.GroupMember ) == GroupLocationPickerMode.GroupMember;
                            bool displayOtherTab = modes != LocationPickerMode.None;

                            ulNav.Visible = displayOtherTab && displayMemberTab;
                            pnlMemberSelect.Visible = displayMemberTab;
                            pnlLocationSelect.Visible = displayOtherTab && !displayMemberTab;

                            if ( displayMemberTab )
                            {
                                int groupId = hfGroupId.ValueAsInt();
                                if ( groupId != 0 )
                                {
                                    var personService = new PersonService();
                                    Guid previousLocationType = Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_PREVIOUS.AsGuid();

                                    foreach ( GroupMember member in new GroupMemberService().GetByGroupId( groupId ) )
                                    {
                                        foreach ( Group family in personService.GetFamilies( member.PersonId ) )
                                        {
                                            foreach ( GroupLocation familyGroupLocation in family.GroupLocations
                                                .Where( l => l.IsMappedLocation && !l.GroupLocationTypeValue.Guid.Equals( previousLocationType ) ) )
                                            {
                                                ListItem li = new ListItem(
                                                    string.Format( "{0} {1} ({2})", member.Person.FullName, familyGroupLocation.GroupLocationTypeValue.Name, familyGroupLocation.Location.ToString() ),
                                                    string.Format( "{0}|{1}", familyGroupLocation.Location.Id, member.PersonId ) );

                                                ddlMember.Items.Add( li );
                                            }
                                        }
                                    }

                                }
                            }

                            if ( displayOtherTab )
                            {
                                locpGroupLocation.AllowedPickerModes = modes;
                            }

                            ddlLocationType.DataSource = groupType.LocationTypeValues.ToList();
                            ddlLocationType.DataBind();

                            var groupLocation = GroupLocationsState.FirstOrDefault( l => l.Guid.Equals( locationGuid ) );
                            if ( groupLocation != null && groupLocation.Location != null )
                            {
                                if ( displayOtherTab )
                                {
                                    locpGroupLocation.CurrentPickerMode = locpGroupLocation.GetBestPickerModeForLocation( groupLocation.Location );
                                    if ( groupLocation.Location != null )
                                    {
                                        locpGroupLocation.Location = new LocationService().Get( groupLocation.Location.Id );
                                    }
                                }

                                if ( displayMemberTab && ddlMember.Items.Count > 0 && groupLocation.GroupMemberPersonId.HasValue )
                                {
                                    ddlMember.SetValue( string.Format( "{0}|{1}", groupLocation.LocationId, groupLocation.GroupMemberPersonId ) );
                                    LocationTypeTab = MEMBER_LOCATION_TAB_TITLE;
                                }
                                else if ( displayOtherTab )
                                {
                                    LocationTypeTab = OTHER_LOCATION_TAB_TITLE;
                                }

                                ddlLocationType.SetValue( groupLocation.GroupLocationTypeValueId );

                                hfAddLocationGroupGuid.Value = locationGuid.ToString();
                            }
                            else
                            {
                                hfAddLocationGroupGuid.Value = string.Empty;
                                LocationTypeTab = ( displayMemberTab && ddlMember.Items.Count > 0 ) ? MEMBER_LOCATION_TAB_TITLE : OTHER_LOCATION_TAB_TITLE;
                            }


                            rptLocationTypes.DataSource = _tabs;
                            rptLocationTypes.DataBind();
                            ShowSelectedPane();

                            ShowDialog( "Locations", true );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Delete event of the gLocations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gLocations_Delete( object sender, RowEventArgs e )
        {
            Guid rowGuid = (Guid)e.RowKeyValue;
            GroupLocationsState.RemoveEntity( rowGuid );

            BindLocationsGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gLocations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gLocations_GridRebind( object sender, EventArgs e )
        {
            BindLocationsGrid();
        }

        /// <summary>
        /// Handles the SaveClick event of the dlgLocations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void dlgLocations_SaveClick( object sender, EventArgs e )
        {
            Location location = null;
            int? memberPersonId = null;

            if ( LocationTypeTab.Equals( MEMBER_LOCATION_TAB_TITLE ) )
            {
                if ( ddlMember.SelectedValue != null )
                {
                    var ids = ddlMember.SelectedValue.Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries );
                    if ( ids.Length == 2 )
                    {
                        var dbLocation = new LocationService().Get( int.Parse( ids[0] ) );
                        if (dbLocation != null)
                        {
                            location = new Location();
                            location.CopyPropertiesFrom(dbLocation);
                        }
                        memberPersonId = int.Parse( ids[1] );
                    }
                }
            }
            else
            {
                if (locpGroupLocation.Location != null)
                {
                    location = new Location();
                    location.CopyPropertiesFrom( locpGroupLocation.Location );
                }
            }

            if (location != null)
            { 
                GroupLocation groupLocation = null;
                
                Guid guid = Guid.Empty;
                if (Guid.TryParse(hfAddLocationGroupGuid.Value, out guid))
                {
                    groupLocation = GroupLocationsState.FirstOrDefault( l => l.Guid.Equals(guid));
                }

                if (groupLocation == null)
                {
                    groupLocation = new GroupLocation();
                    GroupLocationsState.Add(groupLocation);
                }

                groupLocation.GroupMemberPersonId = memberPersonId;
                groupLocation.Location = location;
                groupLocation.LocationId = groupLocation.Location.Id;
                groupLocation.GroupLocationTypeValueId = ddlLocationType.SelectedValueAsId();

                if ( groupLocation.GroupLocationTypeValueId.HasValue )
                {
                    groupLocation.GroupLocationTypeValue = new DefinedValue();
                    var definedValue = new DefinedValueService().Get( groupLocation.GroupLocationTypeValueId.Value );
                    if ( definedValue != null )
                    {
                        groupLocation.GroupLocationTypeValue.CopyPropertiesFrom( definedValue );
                    }
                }
            }

            BindLocationsGrid();

            HideDialog();
        }

        /// <summary>
        /// Binds the locations grid.
        /// </summary>
        private void BindLocationsGrid()
        {
            gLocations.Actions.ShowAdd = AllowMultipleLocations || !GroupLocationsState.Any();

            gLocations.DataSource = GroupLocationsState.ToList();
            gLocations.DataBind();
        }

        #endregion

        #region GroupMemberAttributes Grid and Picker

        /// <summary>
        /// Handles the Add event of the gGroupMemberAttributes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gGroupMemberAttributes_Add( object sender, EventArgs e )
        {
            gGroupMemberAttributes_ShowEdit( Guid.Empty );
        }

        /// <summary>
        /// Handles the Edit event of the gGroupMemberAttributes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gGroupMemberAttributes_Edit( object sender, RowEventArgs e )
        {
            Guid attributeGuid = (Guid)e.RowKeyValue;
            gGroupMemberAttributes_ShowEdit( attributeGuid );
        }

        /// <summary>
        /// Gs the group member attributes_ show edit.
        /// </summary>
        /// <param name="attributeGuid">The attribute GUID.</param>
        protected void gGroupMemberAttributes_ShowEdit( Guid attributeGuid )
        {
            Attribute attribute;
            if ( attributeGuid.Equals( Guid.Empty ) )
            {
                attribute = new Attribute();
                attribute.FieldTypeId = FieldTypeCache.Read( Rock.SystemGuid.FieldType.TEXT ).Id;
                edtGroupMemberAttributes.ActionTitle = ActionTitle.Add( "attribute for group members of " + tbName.Text );
            }
            else
            {
                attribute = GroupMemberAttributesState.First( a => a.Guid.Equals( attributeGuid ) );
                edtGroupMemberAttributes.ActionTitle = ActionTitle.Edit( "attribute for group members of " + tbName.Text );
            }

            edtGroupMemberAttributes.SetAttributeProperties( attribute, typeof( GroupMember ) );

            ShowDialog( "GroupMemberAttributes", true );
        }

        /// <summary>
        /// Handles the GridReorder event of the gGroupMemberAttributes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridReorderEventArgs"/> instance containing the event data.</param>
        protected void gGroupMemberAttributes_GridReorder( object sender, GridReorderEventArgs e )
        {
            ReorderAttributeList( GroupMemberAttributesState, e.OldIndex, e.NewIndex );
            BindGroupMemberAttributesGrid();
        }

        /// <summary>
        /// Handles the Delete event of the gGroupMemberAttributes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gGroupMemberAttributes_Delete( object sender, RowEventArgs e )
        {
            Guid attributeGuid = (Guid)e.RowKeyValue;
            GroupMemberAttributesState.RemoveEntity( attributeGuid );

            BindGroupMemberAttributesGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gGroupMemberAttributesInherited control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gGroupMemberAttributesInherited_GridRebind( object sender, EventArgs e )
        {
            BindGroupMemberAttributesInheritedGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gGroupMemberAttributes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gGroupMemberAttributes_GridRebind( object sender, EventArgs e )
        {
            BindGroupMemberAttributesGrid();
        }

        /// <summary>
        /// Handles the SaveClick event of the dlgGroupMemberAttribute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void dlgGroupMemberAttribute_SaveClick( object sender, EventArgs e )
        {
            Rock.Model.Attribute attribute = new Rock.Model.Attribute();
            edtGroupMemberAttributes.GetAttributeProperties( attribute );

            // Controls will show warnings
            if ( !attribute.IsValid )
            {
                return;
            }

            GroupMemberAttributesState.RemoveEntity( attribute.Guid );
            attribute.Order = GroupMemberAttributesState.Any() ? GroupMemberAttributesState.Max( a => a.Order ) + 1 : 0;
            GroupMemberAttributesState.Add( attribute );

            BindGroupMemberAttributesGrid();

            HideDialog();
        }

        /// <summary>
        /// Binds the group member attributes inherited grid.
        /// </summary>
        private void BindGroupMemberAttributesInheritedGrid()
        {
            gGroupMemberAttributesInherited.AddCssClass( "inherited-attribute-grid" );
            gGroupMemberAttributesInherited.DataSource = GroupMemberAttributesInheritedState;
            gGroupMemberAttributesInherited.DataBind();
            rcGroupMemberAttributesInherited.Visible = GroupMemberAttributesInheritedState.Any();
        }

        /// <summary>
        /// Binds the group member attributes grid.
        /// </summary>
        private void BindGroupMemberAttributesGrid()
        {
            gGroupMemberAttributes.AddCssClass( "attribute-grid" );
            SetAttributeListOrder( GroupMemberAttributesState );
            gGroupMemberAttributes.DataSource = GroupMemberAttributesState.OrderBy( a => a.Name ).ToList();
            gGroupMemberAttributes.DataBind();
        }

        #endregion

}
}