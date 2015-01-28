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
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using DotLiquid;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Field.Types;
using Rock.Model;
using Rock.Security;
using Rock.Web;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Groups
{
    /// <summary>
    /// Block for people to find a group that matches their search parameters.
    /// </summary>
    [DisplayName( "Group Finder" )]
    [Category( "Groups" )]
    [Description( "Block for people to find a group that matches their search parameters." )]

    // Block Properties
    [LinkedPage( "Detail Page", "The page to navigate to for group details.", false, "", "", 0 )]
    [LinkedPage( "Register Page", "The page to navigate to when registering for a group.", false, "", "", 1 )]

    // Custom Settings
    [GroupTypeField( "Group Type", "The group type to limit selection to.", true, "", "CustomSetting" )]
    [AttributeField( Rock.SystemGuid.EntityType.GROUP, "Attribute Filters", "The group attributes to display as filters.", false, true, "", "CustomSetting" )]
    [AttributeField( Rock.SystemGuid.EntityType.GROUP, "Attribute Columns", "The group attributes to display as a column in the results.", false, true, "", "CustomSetting" )]
    [BooleanField( "Show Count", "Should the group member count be displayed in list of groups?", false, "CustomSetting" )]
    [BooleanField( "Show Age", "Should the group member count be displayed in list of groups?", false, "CustomSetting" )]
    [BooleanField( "Show Proximity", "Should an address input be displayed and the distance displayed to each group?", false, "CustomSetting" )]
    [GroupTypeField( "Geofenced Group Type", "The group type that should be used as a geofence limit when finding groups.", false, "", "CustomSetting" )]

    [BooleanField( "Show Map", "Should a map of the groups be displayed?", false, "CustomSetting" )]
    [BooleanField( "Show Fence", "Should geofence boundary be displayed on the map (Requires a Geofence Group Type)?", false, "CustomSetting" )]
    [IntegerField( "Map Height", "Height of the map in pixels (default value is 600px)", false, 600, "CustomSetting" )]
    [DefinedValueField( Rock.SystemGuid.DefinedType.MAP_STYLES, "Map Style", "The map theme that should be used for styling the map.", true, false, Rock.SystemGuid.DefinedValue.MAP_STYLE_GOOGLE, "CustomSetting" )]
    [ValueListField( "Polygon Colors", "List of colors to use when displaying multiple fences (normally will only have one fence).", false, "#f37833,#446f7a,#afd074,#649dac,#f8eba2,#92d0df,#eaf7fc", "#ffffff", null, null, "CustomSetting" )]
    [CodeEditorField( "Map Info", "Lava template for the info window. To suppress the window provide a blank template.", CodeEditorMode.Liquid, CodeEditorTheme.Rock, 200, false, @"
<div style='width:250px'>

    <div class='clearfix'>
        <h4 class='pull-left' style='margin-top: 0;'>{{ Group.Name }}</h4> 
    </div>
    
    <div class='clearfix'>
		{% if Location.Address && Location.Address != '' %}
			<strong>{{ Location.Type }}</strong>
			<br>{{ Location.Address }}
		{% endif %}
    </div>
    
	<br>
	<a class='btn btn-xs btn-action' href='~/Page/999?Group={{ Group.Id }}'>View {{ Group.GroupType.GroupTerm }}</a>
	<a class='btn btn-xs btn-action' href='~/Page/888?Group={{ Group.Id }}'>Register</a>

</div>
", "CustomSetting" )]

    public partial class GroupFinder : RockBlockCustomSettings
    {

        #region Properties

        /// <summary>
        /// Gets the settings tool tip.
        /// </summary>
        /// <value>
        /// The settings tool tip.
        /// </value>
        public override string SettingsToolTip
        {
            get
            {
                return "Edit Settings";
            }
        }

        /// <summary>
        /// Gets or sets the attribute filters.
        /// </summary>
        /// <value>
        /// The attribute filters.
        /// </value>
        public List<AttributeCache> AttributeFilters { get; set; }

        /// <summary>
        /// Gets or sets the _ attribute columns.
        /// </summary>
        /// <value>
        /// The _ attribute columns.
        /// </value>
        public List<AttributeCache> AttributeColumns { get; set; }

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Restores the view-state information from a previous user control request that was saved by the <see cref="M:System.Web.UI.UserControl.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object" /> that represents the user control state to be restored.</param>
        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );

            AttributeFilters = ViewState["AttributeFilters"] as List<AttributeCache>;
            AttributeColumns = ViewState["AttributeColumns"] as List<AttributeCache>;

            BuildDynamicControls();

        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            gGroups.DataKeyNames = new string[] { "Id" };
            gGroups.Actions.ShowAdd = false;
            gGroups.GridRebind += gGroups_GridRebind;
            gGroups.ShowActionRow = false;
            gGroups.AllowPaging = false;

            this.BlockUpdated += Block_Updated;
            this.AddConfigurationUpdateTrigger( upnlContent );

            this.LoadGoogleMapsApi();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            nbNotice.Visible = false;

            if ( !Page.IsPostBack )
            {
                BindAttributes();
                BuildDynamicControls();
                ShowView();
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
            ViewState["AttributeFilters"] = AttributeFilters;
            ViewState["AttributeColumns"] = AttributeColumns;

            return base.SaveViewState();
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the BlockUpdated event of the ContentDynamic control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void Block_Updated( object sender, EventArgs e )
        {
            ShowView();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the gtpGroupType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gtpGroupType_SelectedIndexChanged( object sender, EventArgs e )
        {
            BindGroupAttributeList();
        }

        /// <summary>
        /// Handles the Click event of the lbSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbSave_Click( object sender, EventArgs e )
        {
            if ( !Page.IsValid )
            {
                return;
            }

            SetAttributeValue( "GroupType", GetGroupTypeGuid( gtpGroupType.SelectedGroupTypeId ) );
            SetAttributeValue( "GeofencedGroupType", GetGroupTypeGuid( gtpGeofenceGroupType.SelectedGroupTypeId ) );
            SetAttributeValue( "ShowProximity", cbProximity.Checked.ToString() );
            SetAttributeValue( "AttributeFilters", cblAttributes.Items.Cast<ListItem>().Where( i => i.Selected ).Select( i => i.Value ).ToList().AsDelimited( "," ) );
            SetAttributeValue( "ShowCount", cbShowCount.Checked.ToString() );
            SetAttributeValue( "ShowAge", cbShowAge.Checked.ToString() );
            SetAttributeValue( "AttributeColumns", cblGridAttributes.Items.Cast<ListItem>().Where( i => i.Selected ).Select( i => i.Value ).ToList().AsDelimited( "," ) );
            SetAttributeValue( "ShowMap", cbShowMap.Checked.ToString() );
            SetAttributeValue( "MapHeight", nbMapHeight.Text );
            SetAttributeValue( "MapStyle", ddlMapStyle.SelectedValue );
            SetAttributeValue( "ShowFence", cbShowFence.Checked.ToString() );
            SetAttributeValue( "PolygonColors", vlPolygonColors.Value );
            SetAttributeValue( "MapInfo", ceMapInfo.Text );
            SaveAttributeValues();

            mdEdit.Hide();
            pnlEditModal.Visible = false;
            upnlContent.Update();

            BindAttributes();
            BuildDynamicControls();
            ShowView();
        }

        /// <summary>
        /// Handles the Click event of the btnSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSearch_Click( object sender, EventArgs e )
        {
            ShowResults();
        }

        /// <summary>
        /// Handles the RowSelected event of the gGroups control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gGroups_RowSelected( object sender, RowEventArgs e )
        {
            NavigateToLinkedPage( "DetailPage", "Group", e.RowKeyId );
        }

        /// <summary>
        /// Handles the Click event of the registerColumn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        void registerColumn_Click( object sender, RowEventArgs e )
        {
            NavigateToLinkedPage( "RegisterPage", "Group", e.RowKeyId );
        }

        /// <summary>
        /// Handles the GridRebind event of the gGroups control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void gGroups_GridRebind( object sender, EventArgs e )
        {
            ShowResults();
        }


        #endregion

        #region Internal Methods

        /// <summary>
        /// Shows the settings.
        /// </summary>
        protected override void ShowSettings()
        {
            pnlEditModal.Visible = true;
            upnlContent.Update();
            mdEdit.Show();

            var rockContext = new RockContext();
            var groupTypes = new GroupTypeService( rockContext )
                .Queryable().AsNoTracking().ToList();

            BindGroupType( gtpGroupType, groupTypes, "GroupType" );
            BindGroupType( gtpGeofenceGroupType, groupTypes, "GeofencedGroupType" );

            BindGroupAttributeList();
            foreach ( string attr in GetAttributeValue( "AttributeFilters" ).SplitDelimitedValues() )
            {
                var li = cblAttributes.Items.FindByValue( attr );
                if ( li != null )
                {
                    li.Selected = true;
                }
            }


            cbShowCount.Checked = GetAttributeValue( "ShowCount" ).AsBoolean();
            cbShowAge.Checked = GetAttributeValue( "ShowAge" ).AsBoolean();
            cbProximity.Checked = GetAttributeValue( "ShowProximity" ).AsBoolean();

            foreach ( string attr in GetAttributeValue( "AttributeColumns" ).SplitDelimitedValues() )
            {
                var li = cblGridAttributes.Items.FindByValue( attr );
                if ( li != null )
                {
                    li.Selected = true;
                }
            }

            ddlMapStyle.BindToDefinedType( DefinedTypeCache.Read( Rock.SystemGuid.DefinedType.MAP_STYLES.AsGuid() ) );

            cbShowMap.Checked = GetAttributeValue( "ShowMap" ).AsBoolean();
            nbMapHeight.Text = GetAttributeValue( "MapHeight" );
            ddlMapStyle.SetValue( GetAttributeValue( "MapStyle" ) );
            cbShowFence.Checked = GetAttributeValue( "ShowFence" ).AsBoolean();
            vlPolygonColors.Value = GetAttributeValue( "PolygonColors" );
            ceMapInfo.Text = GetAttributeValue( "MapInfo" );

            upnlContent.Update();
        }

        /// <summary>
        /// Binds the group attribute list.
        /// </summary>
        private void BindGroupAttributeList()
        {
            // Rebuild the checkbox list settings for both the filter and display in grid attribute lists
            var group = new Group();
            group.GroupTypeId = gtpGroupType.SelectedGroupTypeId ?? 0;
            group.LoadAttributes();

            cblAttributes.Items.Clear();
            cblGridAttributes.Items.Clear();
            foreach ( var attribute in group.Attributes )
            {
                cblAttributes.Items.Add( new ListItem( attribute.Value.Name, attribute.Value.Guid.ToString() ) );
                cblGridAttributes.Items.Add( new ListItem( attribute.Value.Name, attribute.Value.Guid.ToString() ) );
            }

            cblAttributes.Visible = cblAttributes.Items.Count > 0;
            cblGridAttributes.Visible = cblAttributes.Items.Count > 0;
        }

        /// <summary>
        /// Shows the view.
        /// </summary>
        private void ShowView()
        {
            // If the groups should be limited by geofence, or the distance should be displayed,
            // then we need to capture the person's address
            Guid? fenceTypeGuid = GetAttributeValue( "GeofencedGroupType" ).AsGuidOrNull();
            if ( fenceTypeGuid.HasValue || GetAttributeValue( "ShowProximity" ).AsBoolean() )
            {
                acAddress.Visible = true;
                if ( CurrentPerson != null )
                {
                    var homeLocation = CurrentPerson.GetHomeLocation();
                    if ( homeLocation != null )
                    {
                        acAddress.Street1 = homeLocation.Street1;
                        acAddress.Street2 = homeLocation.Street2;
                        acAddress.City = homeLocation.City;
                        acAddress.State = homeLocation.State;
                        acAddress.PostalCode = homeLocation.PostalCode;
                        acAddress.Country = homeLocation.Country;
                    }
                }

                btnSearch.Visible = true;
            }
            else
            {
                acAddress.Visible = false;

                // Check to see if there's any attribute filters
                if ( AttributeFilters.Any() )
                {
                    btnSearch.Visible = true;
                }
                else
                {
                    // Hide the search button and show the results immediately since there is 
                    // no filter criteria to be entered
                    btnSearch.Visible = false;
                    pnlResults.Visible = true;
                }
            }

            // If we've already displayed results, then re-display them
            if ( pnlResults.Visible )
            {
                ShowResults();
            }
        }

        /// <summary>
        /// Adds the attribute filters.
        /// </summary>
        private void BindAttributes()
        {
            // Parse the attribute filters 
            AttributeFilters = new List<AttributeCache>();
            foreach ( string attr in GetAttributeValue( "AttributeFilters" ).SplitDelimitedValues() )
            {
                Guid? attributeGuid = attr.AsGuidOrNull();
                if ( attributeGuid.HasValue )
                {
                    var attribute = AttributeCache.Read( attributeGuid.Value );
                    if ( attribute != null )
                    {
                        AttributeFilters.Add( attribute );
                    }
                }
            }

            // Parse the attribute filters 
            AttributeColumns = new List<AttributeCache>();
            foreach ( string attr in GetAttributeValue( "AttributeColumns" ).SplitDelimitedValues() )
            {
                Guid? attributeGuid = attr.AsGuidOrNull();
                if ( attributeGuid.HasValue )
                {
                    var attribute = AttributeCache.Read( attributeGuid.Value );
                    if ( attribute != null )
                    {
                        AttributeColumns.Add( attribute );
                    }
                }
            }
        }

        /// <summary>
        /// Builds the dynamic controls.
        /// </summary>
        private void BuildDynamicControls()
        {
            // Clear attribute filter controls and recreate
            phAttributeFilters.Controls.Clear();
            if ( AttributeFilters != null )
            {
                foreach ( var attribute in AttributeFilters )
                {
                    var control = attribute.FieldType.Field.FilterControl( attribute.QualifierValues, "filter_" + attribute.Id.ToString() );
                    if ( control is IRockControl )
                    {
                        var rockControl = (IRockControl)control;
                        rockControl.Label = attribute.Name;
                        rockControl.Help = attribute.Description;
                        phAttributeFilters.Controls.Add( control );
                    }
                    else
                    {
                        var wrapper = new RockControlWrapper();
                        wrapper.ID = control.ID + "_wrapper";
                        wrapper.Label = attribute.Name;
                        wrapper.Controls.Add( control );
                        phAttributeFilters.Controls.Add( wrapper );
                    }
                }
            }

            // Build attribute columns
            foreach ( var column in gGroups.Columns.OfType<AttributeField>().ToList() )
            {
                gGroups.Columns.Remove( column );
            }
            if ( AttributeColumns != null )
            {
                foreach ( var attribute in AttributeColumns )
                {
                    string dataFieldExpression = attribute.Key;
                    bool columnExists = gGroups.Columns.OfType<AttributeField>().FirstOrDefault( a => a.DataField.Equals( dataFieldExpression ) ) != null;
                    if ( !columnExists )
                    {
                        AttributeField boundField = new AttributeField();
                        boundField.DataField = dataFieldExpression;
                        boundField.HeaderText = attribute.Name;
                        boundField.SortExpression = string.Empty;

                        var attributeCache = AttributeCache.Read( attribute.Id );
                        if ( attributeCache != null )
                        {
                            boundField.ItemStyle.HorizontalAlign = attributeCache.FieldType.Field.AlignValue;
                        }

                        gGroups.Columns.Add( boundField );
                    }
                }
            }

            // Add Register Column
            foreach ( var column in gGroups.Columns.OfType<EditField>().ToList() )
            {
                gGroups.Columns.Remove( column );
            }

            var registerPage = new PageReference( GetAttributeValue( "RegisterPage" ) );
            if ( registerPage.PageId > 0 )
            {
                var registerColumn = new EditField();
                registerColumn.HeaderText = "Register";
                registerColumn.Click += registerColumn_Click;
                gGroups.Columns.Add( registerColumn );
            }

        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void ShowResults()
        {
            // Get the group types that we're interested in
            Guid? groupTypeGuid = GetAttributeValue( "GroupType" ).AsGuidOrNull();
            if ( !groupTypeGuid.HasValue )
            {
                ShowError( "A valid Group Type is required." );
                return;
            }

            gGroups.Columns[2].Visible = GetAttributeValue( "ShowCount" ).AsBoolean();
            gGroups.Columns[3].Visible = GetAttributeValue( "ShowAge" ).AsBoolean();

            bool showProximity = GetAttributeValue( "ShowProximity" ).AsBoolean();
            gGroups.Columns[4].Visible = showProximity;  // Distance

            // Get query of groups of the selected group type
            var rockContext = new RockContext();
            var groupQry = new GroupService( rockContext )
                .Queryable( "GroupLocations.Location" ).AsNoTracking()
                .Where( g => g.GroupType.Guid.Equals( groupTypeGuid.Value ) );

            // Filter query by any configured attribute filters
            if ( AttributeFilters != null && AttributeFilters.Any() )
            {
                var attributeValueService = new AttributeValueService( rockContext );
                var parameterExpression = attributeValueService.ParameterExpression;

                foreach ( var attribute in AttributeFilters )
                {
                    var filterControl = phAttributeFilters.FindControl( "filter_" + attribute.Id.ToString() );
                    if ( filterControl != null )
                    {
                        var filterValues = attribute.FieldType.Field.GetFilterValues( filterControl, attribute.QualifierValues );
                        var expression = attribute.FieldType.Field.FilterExpression( attributeValueService, parameterExpression, "Value", filterValues );
                        if ( expression != null )
                        {
                            var attributeValues = attributeValueService
                                .Queryable().AsNoTracking()
                                .Where( v => v.Attribute.Id == attribute.Id );

                            attributeValues = attributeValues.Where( parameterExpression, expression, null );

                            groupQry = groupQry.Where( w => attributeValues.Select( v => v.EntityId ).Contains( w.Id ) );
                        }
                    }
                }
            }

            // Run query to get list of matching groups
            List<Group> groups = null;
            SortProperty sortProperty = gGroups.SortProperty;
            if ( sortProperty != null )
            {
                groups = groupQry.Sort( sortProperty ).ToList();
            }
            else
            {
                groups = groupQry.OrderBy( g => g.Name ).ToList();
            }

            int? fenceGroupTypeId = GetGroupTypeId( GetAttributeValue( "GeofencedGroupType" ).AsGuidOrNull() );
            bool showMap = GetAttributeValue( "ShowMap" ).AsBoolean();
            bool showFences = showMap && GetAttributeValue( "ShowFence" ).AsBoolean();

            var distances = new Dictionary<int, double>();

            // If we care where these groups are located...
            if ( fenceGroupTypeId.HasValue || showMap || showProximity )
            {
                // Get the location for the address entered
                Location personLocation = null;
                if ( fenceGroupTypeId.HasValue || showProximity )
                {
                    personLocation = new LocationService( rockContext )
                        .Get( acAddress.Street1, acAddress.Street2, acAddress.City,
                            acAddress.State, acAddress.PostalCode, acAddress.Country );
                }

                // If showing a map, and person's location was found, save a mapitem for this location
                FinderMapItem personMapItem = null;
                if ( showMap && personLocation != null && personLocation.GeoPoint != null )
                {
                    var infoWindow = string.Format( @"
<div style='width:250px'>
    <div class='clearfix'>
		<strong>Your Location</strong>
        <br/>{0}
    </div>
</div>
", personLocation.FormattedHtmlAddress );

                    personMapItem = new FinderMapItem( personLocation );
                    personMapItem.Name = "Your Location";
                    personMapItem.InfoWindow = HttpUtility.HtmlEncode( infoWindow.Replace( Environment.NewLine, string.Empty ).Replace( "\n", string.Empty ).Replace( "\t", string.Empty ) );
                }

                // Get the locations, and optionally calculate the distance for each of the groups
                var groupLocations = new List<GroupLocation>();
                foreach ( var group in groups )
                {
                    foreach ( var groupLocation in group.GroupLocations
                        .Where( gl => gl.Location.GeoPoint != null ) )
                    {
                        groupLocations.Add( groupLocation );

                        if ( showProximity && personLocation != null && personLocation.GeoPoint != null )
                        {
                            double meters = groupLocation.Location.GeoPoint.Distance( personLocation.GeoPoint ) ?? 0.0D;
                            double miles = meters / 1609.344;

                            // If this group already has a distance calculated, see if this location is closer and if so, use it instead
                            if ( distances.ContainsKey( group.Id ) )
                            {
                                if ( distances[group.Id] < miles )
                                {
                                    distances[group.Id] = miles;
                                }
                            }
                            else
                            {
                                distances.Add( group.Id, miles );
                            }
                        }
                    }
                }

                // If groups should be limited by a geofence
                var fenceMapItems = new List<MapItem>();
                if ( fenceGroupTypeId.HasValue )
                {
                    var fences = new List<GroupLocation>();
                    if ( personLocation != null && personLocation.GeoPoint != null )
                    {
                        fences = new GroupLocationService( rockContext )
                            .Queryable( "Group,Location" ).AsNoTracking()
                            .Where( gl =>
                                gl.Group.GroupTypeId == fenceGroupTypeId &&
                                gl.Location.GeoFence != null &&
                                personLocation.GeoPoint.Intersects( gl.Location.GeoFence ) )
                            .ToList();
                    }

                    // Limit the group locations to only those locations inside one of the fences
                    groupLocations = groupLocations
                        .Where( gl =>
                            fences.Any( f => gl.Location.GeoPoint.Intersects( f.Location.GeoFence ) ) )
                        .ToList();

                    // Limit the groups to the those that still contain a valid location
                    groups = groups
                        .Where( g =>
                            groupLocations.Any( gl => gl.GroupId == g.Id ) )
                        .ToList();

                    // If the map and fences should be displayed, create a map item for each fence
                    if ( showMap && showFences )
                    {
                        foreach ( var fence in fences )
                        {
                            var mapItem = new FinderMapItem( fence.Location );
                            mapItem.EntityTypeId = EntityTypeCache.Read( "Rock.Model.Group" ).Id;
                            mapItem.EntityId = fence.GroupId;
                            mapItem.Name = fence.Group.Name;
                            fenceMapItems.Add( mapItem );
                        }
                    }
                }

                // If a map is to be shown
                if ( showMap && groups.Any() )
                {

                    Template template = Template.Parse( GetAttributeValue( "MapInfo" ) );
                    string detailPageValue = GetAttributeValue("DetailPage");
                    string registerPageValue = GetAttributeValue("RegisterPage");

                    // Add mapitems for all the remaining valid group locations
                    var groupMapItems = new List<MapItem>();
                    foreach ( var gl in groupLocations )
                    {
                        var group = groups.Where( g => g.Id == gl.GroupId ).FirstOrDefault();
                        if ( group != null )
                        {
                            // Resolve info window lava template
                            var linkedPageParams = new Dictionary<string, string> {{ "GroupId", group.Id.ToString() }};
                            var mergeFields = new Dictionary<string, object>();
                            mergeFields.Add( "Group", gl.Group );
                            mergeFields.Add( "Location", gl.Location );
                            mergeFields.Add( "DetailPageLink", new PageReference( detailPageValue, linkedPageParams ).BuildUrl() );
                            mergeFields.Add( "RegisterPageLink", new PageReference( registerPageValue, linkedPageParams ).BuildUrl() );
                            string infoWindow = template.Render( Hash.FromDictionary( mergeFields ) );

                            // Add a map item for group
                            var mapItem = new FinderMapItem( gl.Location );
                            mapItem.EntityTypeId = EntityTypeCache.Read( "Rock.Model.Group" ).Id;
                            mapItem.EntityId = group.Id;
                            mapItem.Name = group.Name;
                            mapItem.InfoWindow = HttpUtility.HtmlEncode( infoWindow.Replace( Environment.NewLine, string.Empty ).Replace( "\n", string.Empty ).Replace( "\t", string.Empty ) );
                            groupMapItems.Add( mapItem );
                        }
                    }

                    // Show the map
                    Map( personMapItem, fenceMapItems, groupMapItems );
                    pnlMap.Visible = true;
                }
                else
                {
                    pnlMap.Visible = false;
                }
            }
            else
            {
                pnlMap.Visible = false;
            }

            // Save the groups into the grid's object list since it is not being bound to actual group objects
            gGroups.ObjectList = new Dictionary<string, object>();
            groups.ForEach( g => gGroups.ObjectList.Add( g.Id.ToString(), g ) );

            // Bind the grid
            gGroups.DataSource = groups.Select( g => new
            {
                Id = g.Id,
                Name = g.Name,
                GroupTypeName = g.GroupType.Name,
                GroupOrder = g.Order,
                GroupTypeOrder = g.GroupType.Order,
                Description = g.Description,
                IsSystem = g.IsSystem,
                IsActive = g.IsActive,
                GroupRole = string.Empty,
                DateAdded = DateTime.MinValue,
                MemberCount = g.Members.Count(),
                AverageAge = g.Members.Select( m => m.Person ).Average( p => p.Age ),
                Distance = distances.Where( d => d.Key == g.Id )
                    .Select( d => d.Value ).FirstOrDefault()
            } ).ToList();
            gGroups.DataBind();

            // Show the results
            pnlResults.Visible = true;

        }

        /// <summary>
        /// Binds the type of the group.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="groupTypes">The group types.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        private void BindGroupType( GroupTypePicker control, List<GroupType> groupTypes, string attributeName )
        {
            control.GroupTypes = groupTypes;

            Guid? groupTypeGuid = GetAttributeValue( attributeName ).AsGuidOrNull();
            if ( groupTypeGuid.HasValue )
            {
                var groupType = groupTypes.FirstOrDefault( g => g.Guid.Equals( groupTypeGuid.Value ) );
                if ( groupType != null )
                {
                    control.SelectedGroupTypeId = groupType.Id;
                }
            }
        }

        private int? GetGroupTypeId( Guid? groupTypeGuid )
        {
            if ( groupTypeGuid.HasValue )
            {
                var groupType = GroupTypeCache.Read( groupTypeGuid.Value );
                if ( groupType != null )
                {
                    return groupType.Id;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the group type unique identifier.
        /// </summary>
        /// <param name="groupTypeId">The group type identifier.</param>
        /// <returns></returns>
        private string GetGroupTypeGuid( int? groupTypeId )
        {
            if ( groupTypeId.HasValue )
            {
                var groupType = GroupTypeCache.Read( groupTypeId.Value );
                if ( groupType != null )
                {
                    return groupType.Guid.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Maps the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="fences">The fences.</param>
        /// <param name="groups">The groups.</param>
        private void Map( MapItem location, List<MapItem> fences, List<MapItem> groups )
        {
            pnlMap.Visible = true;

            string mapStylingFormat = @"
                        <style>
                            #map_wrapper {{
                                height: {0}px;
                            }}

                            #map_canvas {{
                                width: 100%;
                                height: 100%;
                                border-radius: 8px;
                            }}
                        </style>";
            lMapStyling.Text = string.Format( mapStylingFormat, GetAttributeValue( "MapHeight" ) );

            // add styling to map
            string styleCode = "null";
            var markerColors = new List<string>();

            DefinedValueCache dvcMapStyle = DefinedValueCache.Read( GetAttributeValue( "MapStyle" ).AsInteger() );
            if ( dvcMapStyle != null )
            {
                styleCode = dvcMapStyle.GetAttributeValue( "DynamicMapStyle" );
                markerColors = dvcMapStyle.GetAttributeValue( "Colors" )
                    .Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries )
                    .ToList();
                markerColors.ForEach( c => c = c.Replace( "#", string.Empty ) );
            }
            if ( !markerColors.Any() )
            {
                markerColors.Add( "FE7569" );
            }

            string locationColor = markerColors[0].Replace( "#", string.Empty );
            var polygonColorList = GetAttributeValue( "PolygonColors" ).Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ).ToList();
            string polygonColors = "\"" + polygonColorList.AsDelimited( "\", \"" ) + "\"";
            string groupColor = ( markerColors.Count > 1 ? markerColors[1] : markerColors[0] ).Replace( "#", string.Empty );

            string latitude = "39.8282";
            string longitude = "-98.5795";
            string zoom = "4";
            var orgLocation = GlobalAttributesCache.Read().OrganizationLocation;
            if ( orgLocation != null && orgLocation.GeoPoint != null )
            {
                latitude = orgLocation.GeoPoint.Latitude.ToString();
                longitude = orgLocation.GeoPoint.Longitude.ToString();
                zoom = "12";
            }

            // write script to page
            string mapScriptFormat = @"

        var locationData = {0};
        var fenceData = {1};
        var groupData = {2}; 

        var allMarkers = [];

        var map;
        var bounds = new google.maps.LatLngBounds();
        var infoWindow = new google.maps.InfoWindow();

        var mapStyle = {3};

        var pinShadow = new google.maps.MarkerImage('//chart.apis.google.com/chart?chst=d_map_pin_shadow',
            new google.maps.Size(40, 37),
            new google.maps.Point(0, 0),
            new google.maps.Point(12, 35));

        var polygonColorIndex = 0;
        var polygonColors = [{5}];

        var min = .999999;
        var max = 1.000001;

        initializeMap();

        function initializeMap() {{

            debugger;

            // Set default map options
            var mapOptions = {{
                 mapTypeId: 'roadmap'
                ,styles: mapStyle
                ,center: new google.maps.LatLng({7}, {8})
                ,zoom: {9}
            }};

            // Display a map on the page
            map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
            map.setTilt(45);

            if ( locationData != null )
            {{
                var items = addMapItem(0, locationData, '{4}');
                for (var j = 0; j < items.length; j++) {{
                    items[j].setMap(map);
                }}
            }}

            if ( fenceData != null ) {{
                for (var i = 0; i < fenceData.length; i++) {{
                    var items = addMapItem(i, fenceData[i] );
                    for (var j = 0; j < items.length; j++) {{
                        items[j].setMap(map);
                    }}
                }}
            }}

            if ( groupData != null ) {{
                for (var i = 0; i < groupData.length; i++) {{
                    var items = addMapItem(i, groupData[i], '{6}');
                    for (var j = 0; j < items.length; j++) {{
                        items[j].setMap(map);
                    }}
                }}
            }}

            // adjust any markers that may overlap
            adjustOverlappedMarkers();

            if (!bounds.isEmpty()) {{
                map.fitBounds(bounds);
            }}

        }}

        function addMapItem( i, mapItem, color ) {{

            var items = [];

            if (mapItem.Point) {{ 

                var position = new google.maps.LatLng(mapItem.Point.Latitude, mapItem.Point.Longitude);
                bounds.extend(position);

                if (!color) {{
                    color = 'FE7569'
                }}

                var pinImage = new google.maps.MarkerImage('http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|' + color,
                    new google.maps.Size(21, 34),
                    new google.maps.Point(0,0),
                    new google.maps.Point(10, 34));

                marker = new google.maps.Marker({{
                    position: position,
                    map: map,
                    title: htmlDecode(mapItem.Name),
                    icon: pinImage,
                    shadow: pinShadow
                }});
    
                items.push(marker);
                allMarkers.push(marker);

                if ( mapItem.InfoWindow != null ) {{ 
                    google.maps.event.addListener(marker, 'click', (function (marker, i) {{
                        return function () {{
                            infoWindow.setContent( $('<div/>').html(mapItem.InfoWindow).text() );
                            infoWindow.open(map, marker);
                        }}
                    }})(marker, i));
                }}

                if ( mapItem.EntityId && mapItem.EntityId > 0 ) {{ 
                    google.maps.event.addListener(marker, 'mouseover', (function (marker, i) {{
                        return function () {{
                            $(""tr[datakey='"" + mapItem.EntityId + ""']"").addClass('alert alert-danger');
                        }}
                    }})(marker, i));

                    google.maps.event.addListener(marker, 'mouseout', (function (marker, i) {{
                        return function () {{
                            $(""tr[datakey='"" + mapItem.EntityId + ""']"").removeClass('alert alert-danger');
                        }}
                    }})(marker, i));

                }}

            }}

            if (typeof mapItem.PolygonPoints !== 'undefined' && mapItem.PolygonPoints.length > 0) {{

                var polygon;
                var polygonPoints = [];

                $.each(mapItem.PolygonPoints, function(j, point) {{
                    var position = new google.maps.LatLng(point.Latitude, point.Longitude);
                    bounds.extend(position);
                    polygonPoints.push(position);
                }});

                var polygonColor = getNextPolygonColor();

                polygon = new google.maps.Polygon({{
                    paths: polygonPoints,
                    map: map,
                    strokeColor: polygonColor,
                    fillColor: polygonColor
                }});

                items.push(polygon);

                // Get Center
                var polyBounds = new google.maps.LatLngBounds();
                for ( j = 0; j < polygonPoints.length; j++) {{
                    polyBounds.extend(polygonPoints[j]);
                }}

                if ( mapItem.InfoWindow != null ) {{ 
                    google.maps.event.addListener(polygon, 'click', (function (polygon, i) {{
                        return function () {{
                            infoWindow.setContent( mapItem.InfoWindow );
                            infoWindow.setPosition(polyBounds.getCenter());
                            infoWindow.open(map);
                        }}
                    }})(polygon, i));
                }}
            }}

            return items;

        }}
        
        function setAllMap(markers, map) {{
            for (var i = 0; i < markers.length; i++) {{
                markers[i].setMap(map);
            }}
        }}

        function htmlDecode(input) {{
            var e = document.createElement('div');
            e.innerHTML = input;
            return e.childNodes.length === 0 ? """" : e.childNodes[0].nodeValue;
        }}

        function getNextPolygonColor() {{
            var color = 'FE7569';
            if ( polygonColors.length > polygonColorIndex ) {{
                color = polygonColors[polygonColorIndex];
                polygonColorIndex++;
            }} else {{
                color = polygonColors[0];
                polygonColorIndex = 1;
            }}
            return color;
        }}

        function adjustOverlappedMarkers() {{
            
            if (allMarkers.length > 1) {{
                for(i=0; i < allMarkers.length-1; i++) {{
                    var marker1 = allMarkers[i];
                    var pos1 = marker1.getPosition();
                    for(j=i+1; j < allMarkers.length; j++) {{
                        var marker2 = allMarkers[j];
                        var pos2 = marker2.getPosition();
                        if (pos1.equals(pos2)) {{
                            var newLat = pos1.lat() * (Math.random() * (max - min) + min);
                            var newLng = pos1.lng() * (Math.random() * (max - min) + min);
                            marker1.setPosition( new google.maps.LatLng(newLat,newLng) );
                        }}
                    }}
                }}
            }}

        }}
";

            var locationJson = location != null ?
                string.Format( "JSON.parse('{0}')", location.ToJson().Replace( Environment.NewLine, "" ).Replace( "\x0A", "" ) ) : "null";

            var fencesJson = fences != null && fences.Any() ?
                string.Format( "JSON.parse('{0}')", fences.ToJson().Replace( Environment.NewLine, "" ).Replace( "\x0A", "" ) ) : "null";

            var groupsJson = groups != null && groups.Any() ?
                string.Format( "JSON.parse('{0}')", groups.ToJson().Replace( Environment.NewLine, "" ).Replace( "\x0A", "" ) ) : "null";

            string mapScript = string.Format( mapScriptFormat,
                locationJson,       // 0
                fencesJson,         // 1
                groupsJson,         // 2
                styleCode,          // 3
                locationColor,      // 4
                polygonColors,      // 5
                groupColor,         // 6
                latitude,           // 7
                longitude,          // 8
                zoom );             // 9

            ScriptManager.RegisterStartupScript( pnlMap, pnlMap.GetType(), "group-finder-map-script", mapScript, true );

        }

        private void ShowError( string message )
        {
            nbNotice.Heading = "Error";
            nbNotice.NotificationBoxType = NotificationBoxType.Danger;
            ShowMessage( message );
        }

        private void ShowWarning( string message )
        {
            nbNotice.Heading = "Warning";
            nbNotice.NotificationBoxType = NotificationBoxType.Warning;
            ShowMessage( message );
        }

        private void ShowMessage( string message )
        {
            nbNotice.Text = string.Format( "<p>{0}</p>", message );
            nbNotice.Visible = true;
        }

        #endregion

        /// <summary>
        /// A map item class specific to group finder
        /// </summary>
        class FinderMapItem : MapItem
        {
            /// <summary>
            /// Gets or sets the information window.
            /// </summary>
            /// <value>
            /// The information window.
            /// </value>
            public string InfoWindow { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="FinderMapItem"/> class.
            /// </summary>
            /// <param name="location">The location.</param>
            public FinderMapItem( Location location )
                : base( location )
            {

            }
        }
    }
}