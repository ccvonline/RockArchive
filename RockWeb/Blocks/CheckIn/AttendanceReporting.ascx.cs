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
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using Rock.Web.UI;

namespace RockWeb.Blocks.CheckIn
{
    /// <summary>
    /// Shows a graph of attendance statistics which can be configured for specific groups, date range, etc.
    /// </summary>
    [DisplayName( "Attendance Reporting" )]
    [Category( "Check-in" )]
    [Description( "Shows a graph of attendance statistics which can be configured for specific groups, date range, etc." )]

    [DefinedValueField( Rock.SystemGuid.DefinedType.CHART_STYLES, "Chart Style" )]
    [LinkedPage( "Detail Page", "Select the page to navigate to when the chart is clicked" )]
    [GroupTypeField( "Group Type Template", groupTypePurposeValueGuid: Rock.SystemGuid.DefinedValue.GROUPTYPE_PURPOSE_CHECKIN_TEMPLATE )]
    public partial class AttendanceReporting : RockBlock
    {
        #region Base Control Methods

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

            gAttendance.GridRebind += gAttendance_GridRebind;
        }

        /// <summary>
        /// Handles the GridRebind event of the gAttendance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void gAttendance_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
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
                LoadDropDowns();
                LoadSettings();
                LoadChart();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the detail page unique identifier.
        /// </summary>
        /// <value>
        /// The detail page unique identifier.
        /// </value>
        public Guid? DetailPageGuid
        {
            get
            {
                return ( GetAttributeValue( "DetailPage" ) ?? string.Empty ).AsGuidOrNull();
            }
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            LoadChart();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        public void LoadDropDowns()
        {
            var rockContext = new RockContext();

            cpCampuses.Campuses = new CampusService( rockContext ).Queryable().OrderBy( a => a.Name ).ToList();

            var groupTypeTemplateGuid = this.GetAttributeValue( "GroupTypeTemplate" ).AsGuidOrNull();
            nbGroupTypeWarning.Visible = !groupTypeTemplateGuid.HasValue;
            if ( groupTypeTemplateGuid.HasValue )
            {
                var groupTypes = new GroupTypeService( rockContext ).GetAllAssociatedDescendents( groupTypeTemplateGuid.Value );
                rptGroupTypes.DataSource = groupTypes.ToList();
                rptGroupTypes.DataBind();
            }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            foreach ( ListItem item in cpCampuses.Items )
            {
                item.Selected = true;
            }
        }

        /// <summary>
        /// Loads the chart.
        /// </summary>
        public void LoadChart()
        {
            lcAttendance.ShowTooltip = true;
            if ( this.DetailPageGuid.HasValue )
            {
                lcAttendance.ChartClick += lcAttendance_ChartClick;
            }

            lcAttendance.Options.SetChartStyle( this.GetAttributeValue( "ChartStyle" ).AsGuidOrNull() );

            var dataSourceUrl = "~/api/Attendances/GetChartData";
            var dataSourceParams = new Dictionary<string, object>();
            var dateRange = SlidingDateRangePicker.CalculateDateRangeFromDelimitedValues( drpSlidingDateRange.DelimitedValues );

            if ( dateRange.Start.HasValue )
            {
                dataSourceParams.AddOrReplace( "startDate", dateRange.Start.Value.ToString( "o" ) );
            }

            if ( dateRange.End.HasValue )
            {
                dataSourceParams.AddOrReplace( "endDate", dateRange.End.Value.ToString( "o" ) );
            }

            dataSourceParams.AddOrReplace( "groupBy", hfGroupBy.Value.AsInteger() );
            dataSourceParams.AddOrReplace( "graphBy", hfGraphBy.Value.AsInteger() );

            dataSourceParams.AddOrReplace( "campusIds", cpCampuses.SelectedCampusIds.AsDelimited( "," ) );

            dataSourceUrl += "?" + dataSourceParams.Select( s => string.Format( "{0}={1}", s.Key, s.Value ) ).ToList().AsDelimited( "&" );

            lcAttendance.DataSourceUrl = this.ResolveUrl( dataSourceUrl );

            if ( pnlGrid.Visible )
            {
                BindGrid();
            }
        }

        /// <summary>
        /// Lcs the attendance_ chart click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void lcAttendance_ChartClick( object sender, FlotChart.ChartClickArgs e )
        {
            if ( this.DetailPageGuid.HasValue )
            {
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString.Add( "YValue", e.YValue.ToString() );
                qryString.Add( "DateTimeValue", e.DateTimeValue.ToString( "o" ) );
                NavigateToPage( this.DetailPageGuid.Value, qryString );
            }
        }

        /// <summary>
        /// Handles the Click event of the lShowGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lShowGrid_Click( object sender, EventArgs e )
        {
            if ( pnlGrid.Visible )
            {
                pnlGrid.Visible = false;
            }
            else
            {
                pnlGrid.Visible = true;
                BindGrid();
            }
        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            var dateRange = SlidingDateRangePicker.CalculateDateRangeFromDelimitedValues( drpSlidingDateRange.DelimitedValues );

            string groupTypeIds = null;
            string campusIds = null;

            SortProperty sortProperty = gAttendance.SortProperty;

            var chartData = new AttendanceService( new RockContext() ).GetChartData(
                hfGroupBy.Value.ConvertToEnumOrNull<AttendanceGroupBy>() ?? AttendanceGroupBy.Week,
                hfGraphBy.Value.ConvertToEnumOrNull<AttendanceGraphBy>() ?? AttendanceGraphBy.Total,
                dateRange.Start,
                dateRange.End,
                groupTypeIds,
                campusIds );

            if ( sortProperty != null )
            {
                gAttendance.DataSource = chartData.AsQueryable().Sort( sortProperty ).ToList();
            }
            else
            {
                gAttendance.DataSource = chartData.OrderBy( a => a.DateTimeStamp ).ToList();
            }

            gAttendance.DataBind();
        }

        /// <summary>
        /// Handles the ItemDataBound event of the rptGroupTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void rptGroupTypes_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            if ( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
            {
                var groupType = e.Item.DataItem as GroupType;
                var cblGroups = e.Item.FindControl( "cblGroups" ) as RockCheckBoxList;
                cblGroups.Items.Clear();
                foreach ( var group in groupType.Groups.OrderBy( a => a.Order ).ThenBy( a => a.Name).Select( a => new { a.Id, a.Name } ).ToList() )
                {
                    cblGroups.Items.Add( new ListItem( group.Name, group.Id.ToString() ) );
                }
                
                cblGroups.Visible = cblGroups.Items.Count > 1;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnApply control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnApply_Click( object sender, EventArgs e )
        {
            LoadChart();
        }

        #endregion

    }
}