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
using Rock.Reporting.Dashboard;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Reporting.Dashboard
{
    /// <summary>
    /// 
    /// </summary>
    [DisplayName( "Pie Chart" )]
    [Category( "Reporting > Dashboard" )]
    [Description( "Pie Chart Dashboard Widget" )]

    [DefinedValueField( Rock.SystemGuid.DefinedType.CHART_STYLES, "Chart Style", Order = 3 )]
    [EntityField( "Series Partition", "Select the series partition entity (Campus, Group, etc) to be used to limit the metric values for the selected metrics.", "Either select a specific {0} or leave {0} blank to get it from the page context.", Key="Entity", Order = 4 )]
    [MetricCategoriesField( "Metrics", "Select the metrics to include in the pie chart.  Each Metric will be a section of the pie.", false, "", "", 5, "MetricCategories" )]
    [CustomRadioListField( "Metric Value Type", "Select which metric value type to display in the chart", "Goal,Measure", false, "Measure", Order = 6 )]
    [SlidingDateRangeField( "Date Range", Key = "SlidingDateRange", DefaultValue = "1||4||", Order = 7 )]
    [LinkedPage( "Detail Page", "Select the page to navigate to when the chart is clicked", Order = 8 )]
    public partial class PieChartDashboardWidget : DashboardWidget
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            this.BlockUpdated += Block_BlockUpdated;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            LoadChart();
        }

        /// <summary>
        /// Handles the BlockUpdated event of the Block control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            LoadChart();
        }

        /// <summary>
        /// Loads the chart.
        /// </summary>
        public void LoadChart()
        {
            pcChart.Title = this.Title;
            pcChart.Subtitle = this.Subtitle;
            pcChart.ShowTooltip = true;
            pcChart.Options.SetChartStyle( this.GetAttributeValue( "ChartStyle" ).AsGuidOrNull() );

            var metricIdList = this.GetMetricIds();

            string restApiUrl = string.Format( "~/api/MetricValues/GetSummary?metricIdList={0}", metricIdList.AsDelimited( "," ) );

            var dateRange = SlidingDateRangePicker.CalculateDateRangeFromDelimitedValues( this.GetAttributeValue( "SlidingDateRange" ) ?? string.Empty );
            if ( dateRange != null )
            {
                if ( dateRange.Start.HasValue )
                {
                    restApiUrl += string.Format( "&startDate={0}", dateRange.Start.Value.ToString( "o" ) );
                }

                if ( dateRange.End.HasValue )
                {
                    restApiUrl += string.Format( "&endDate={0}", dateRange.End.Value.ToString( "o" ) );
                }
            }

            var metricValueType = this.GetAttributeValue( "MetricValueTypes" ).ConvertToEnumOrNull<MetricValueType>() ?? Rock.Model.MetricValueType.Measure;

            restApiUrl += string.Format( "&metricValueType={0}", metricValueType );

            string[] entityValues = ( GetAttributeValue( "Entity" ) ?? string.Empty ).Split( '|' );
            if ( entityValues.Length == 2 )
            {
                var entityType = EntityTypeCache.Read( entityValues[0].AsGuid() );
                if ( entityType != null )
                {
                    restApiUrl += string.Format( "?entityTypeId={0}", entityType.Id );
                    int? entityId = entityValues[1].AsIntegerOrNull();
                    if ( entityId.HasValue )
                    {
                        restApiUrl += string.Format( "&entityId={0}", entityId );
                    }
                }
            }
            else
            {
                if ( this.ContextEntity() != null )
                {
                    var entityType = EntityTypeCache.Read( this.ContextEntity().GetType(), false );
                    if ( entityType != null )
                    {
                        restApiUrl += string.Format( "?entityTypeId={0}&entityId={1}", entityType.Id, this.ContextEntity().Id );
                    }
                }
            }

            pcChart.DataSourceUrl = restApiUrl;
            
            //// pcChart.PieOptions.tilt = 0.5;
            //// pcChart.ChartHeight =  
            
            pcChart.PieOptions.label = new Rock.Reporting.Dashboard.Flot.PieLabel { show = true };
            pcChart.PieOptions.label.formatter = @"
function labelFormatter(label, series) {
	return ""<div style='font-size:8pt; text-align:center; padding:2px; '>"" + label + ""<br/>"" + Math.round(series.percent) + ""%</div>"";
}
".Trim();
            pcChart.Legend.show = false;
            
            nbMetricWarning.Visible = !metricIdList.Any();
        }

        /// <summary>
        /// Gets the metrics.
        /// </summary>
        /// <returns></returns>
        /// 
        /// <value>
        /// The metrics.
        /// </value>
        public List<int> GetMetricIds()
        {
            var metricCategoryGuids = ( GetAttributeValue( "MetricCategories" ) ?? string.Empty ).Split( ',' ).Select( a => a.AsGuidOrNull() ?? Guid.Empty ).ToList();
            return new MetricCategoryService( new Rock.Data.RockContext() ).GetByGuids( metricCategoryGuids ).Select( a => a.MetricId ).ToList();
        }
    }
}