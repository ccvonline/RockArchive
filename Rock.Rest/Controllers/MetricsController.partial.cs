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
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Http;
using Newtonsoft.Json;
using Rock.Data;
using Rock.Model;

namespace Rock.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MetricsController : IHasCustomRoutes
    {
        /// <summary>
        /// Adds the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public void AddRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "MetricsGetHtmlByBlockId",
                routeTemplate: "api/Metrics/GetHtmlForBlock/{blockId}",
                defaults: new
                {
                    controller = "Metrics",
                    action = "GetHtmlForBlock"
                } );
        }

        /// <summary>
        /// Gets the HTML for block.
        /// </summary>
        /// <param name="blockId">The block identifier.</param>
        /// <param name="entityTypeId">The entity type identifier.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        public string GetHtmlForBlock( int blockId, int? entityTypeId = null, int? entityId = null )
        {
            Block block = new BlockService( new RockContext() ).Get( blockId );
            if ( block != null )
            {
                block.LoadAttributes();

                string displayText = block.GetAttributeValue( "DisplayText" );

                List<Guid> metricCategoryGuids = block.GetAttributeValue( "MetricCategories" ).Split( ',' ).Select( a => a.AsGuid() ).ToList();

                RockContext rockContext = new RockContext();
                MetricCategoryService metricCategoryService = new MetricCategoryService( rockContext );
                var metricCategories = metricCategoryService.GetByGuids( metricCategoryGuids );
                List<object> metricsData = new List<object>();

                if ( metricCategories.Count() == 0 )
                {
                    return @"<div class='alert alert-warning'> 
								Please select a metric in the block settings.
							</div>";
                }
                
                MetricValueService metricValueService = new MetricValueService( rockContext );
                
                DateTime firstDayOfYear = new DateTime( RockDateTime.Now.Year, 1, 1 );
                DateTime currentDateTime = RockDateTime.Now;
                DateTime firstDayOfNextYear = new DateTime( RockDateTime.Now.Year + 1, 1, 1 );

                foreach ( var metric in metricCategories.Select(a => a.Metric).Distinct() )
                {
                    var metricYTDData = JsonConvert.DeserializeObject( metric.ToJson(), typeof( MetricYTDData ) ) as MetricYTDData;
                    var qryMeasureValues = metricValueService.Queryable()
                        .Where( a => a.MetricId == metricYTDData.Id )
                        .Where( a => a.MetricValueDateTime >= firstDayOfYear && a.MetricValueDateTime < currentDateTime )
                        .Where( a => a.MetricValueType == MetricValueType.Measure );

                    // if an entityTypeId/EntityId filter was specified, and the entityTypeId is the same as the metrics.EntityTypeId, filter the values to the specified entityId
                    if ( entityTypeId.HasValue && metric.EntityTypeId == entityTypeId )
                    {
                        if ( entityId.HasValue )
                        {
                            qryMeasureValues = qryMeasureValues.Where( a => a.EntityId == entityId );
                        }
                    }

                    var lastMetricValue = qryMeasureValues.OrderByDescending( a => a.MetricValueDateTime ).FirstOrDefault();
                    if ( lastMetricValue != null )
                    {
                        metricYTDData.LastValue = lastMetricValue.YValue;
                        metricYTDData.LastValueDate = lastMetricValue.MetricValueDateTime.HasValue ? lastMetricValue.MetricValueDateTime.Value.Date : DateTime.MinValue;
                    }

                    metricYTDData.CumulativeValue = qryMeasureValues.Sum( a => a.YValue );

                    // figure out goal as of current date time by figuring out the slope of the goal
                    var qryGoalValuesCurrentYear = metricValueService.Queryable()
                        .Where( a => a.MetricId == metricYTDData.Id )
                        .Where( a => a.MetricValueDateTime >= firstDayOfYear && a.MetricValueDateTime < firstDayOfNextYear )
                        .Where( a => a.MetricValueType == MetricValueType.Goal );

                    // if an entityTypeId/EntityId filter was specified, and the entityTypeId is the same as the metrics.EntityTypeId, filter the values to the specified entityId
                    if ( entityTypeId.HasValue && metric.EntityTypeId == entityTypeId )
                    {
                        if ( entityId.HasValue )
                        {
                            qryGoalValuesCurrentYear = qryGoalValuesCurrentYear.Where( a => a.EntityId == entityId );
                        }
                    }

                    MetricValue goalLineStartPoint = qryGoalValuesCurrentYear.Where( a => a.MetricValueDateTime <= currentDateTime ).OrderByDescending( a => a.MetricValueDateTime ).FirstOrDefault();
                    MetricValue goalLineEndPoint = qryGoalValuesCurrentYear.Where( a => a.MetricValueDateTime >= currentDateTime ).FirstOrDefault();
                    if ( goalLineStartPoint != null && goalLineEndPoint != null )
                    {
                        var changeInX = goalLineEndPoint.DateTimeStamp - goalLineStartPoint.DateTimeStamp;
                        var changeInY = goalLineEndPoint.YValue - goalLineStartPoint.YValue;
                        if ( changeInX != 0 )
                        {
                            var slope = changeInY / changeInX;
                            var goalValue = ( ( slope * ( currentDateTime.ToJavascriptMilliseconds() - goalLineStartPoint.DateTimeStamp ) ) + goalLineStartPoint.YValue ).Value;
                            metricYTDData.GoalValue = Math.Round( goalValue, 2 );
                        }
                    }
                    else
                    {
                        var singleGoal = goalLineStartPoint ?? goalLineEndPoint;
                        metricYTDData.GoalValue = singleGoal != null ? singleGoal.YValue : (decimal?)null;
                    }

                    metricsData.Add( metricYTDData.ToLiquid() );
                }

                Dictionary<string, object> mergeValues = new Dictionary<string, object>();
                mergeValues.Add( "Metrics", metricsData );

                string resultHtml = displayText.ResolveMergeFields( mergeValues );

                // show liquid help
                if ( block.GetAttributeValue( "EnableDebug" ).AsBoolean() )
                {
                    string debugInfo = string.Format(
                        @"<small><a data-toggle='collapse' data-parent='#accordion' href='#liquid-metric-debug'><i class='fa fa-eye'></i></a></small>
                            <pre id='liquid-metric-debug' class='collapse well liquid-metric-debug'>
                                {0}
                            </pre>",
                        mergeValues.LiquidHelpText() );

                    resultHtml += debugInfo;
                }

                return resultHtml;
            }

            return string.Format(
                @"<div class='alert alert-danger'> 
                    unable to find block_id: {1}
                </div>",
                blockId );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MetricYTDData : Metric
    {
        /// <summary>
        /// Gets or sets the last value.
        /// </summary>
        /// <value>
        /// The last value.
        /// </value>
        [DataMember]
        public decimal? LastValue { get; set; }

        /// <summary>
        /// Gets or sets the last value date.
        /// </summary>
        /// <value>
        /// The last value date.
        /// </value>
        [DataMember]
        public DateTime LastValueDate { get; set; }

        /// <summary>
        /// Gets or sets the cumulative value.
        /// </summary>
        /// <value>
        /// The cumulative value.
        /// </value>
        [DataMember]
        public decimal? CumulativeValue { get; set; }

        /// <summary>
        /// Gets or sets the goal value.
        /// </summary>
        /// <value>
        /// The goal value.
        /// </value>
        [DataMember]
        public decimal? GoalValue { get; set; }
    }
}
