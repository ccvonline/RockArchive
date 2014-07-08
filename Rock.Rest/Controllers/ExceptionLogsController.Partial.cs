//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
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

using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Rock.Model;
using Rock;
using System.Linq;
using System.Data.Entity;
using Rock.Data;
using System.Collections.Generic;

namespace Rock.Rest.Controllers
{
    /// <summary>
    /// ExceptionLogs REST API
    /// </summary>
    public partial class ExceptionLogsController : IHasCustomRoutes
    {
        public void AddRoutes( RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "GetChartData",
                routeTemplate: "api/ExceptionLogs/GetChartData",
                defaults: new
                {
                    controller = "ExceptionLogs",
                    action = "GetChartData"
                } );
        }

        /// <summary>
        /// Gets the exceptions grouped by date.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IChartData> GetChartData()
        {
            var exceptionList = this.Get().Where( x => x.HasInnerException == false && x.CreatedDateTime != null )
            .GroupBy( x => DbFunctions.TruncateTime( x.CreatedDateTime.Value ) )
            .Select( eg => new
            {
                DateValue = eg.Key.Value,
                ExceptionCount = eg.Count(),
                UniqueExceptionCount = eg.Select( y => y.ExceptionType ).Distinct().Count()
            } )
            .OrderBy( eg => eg.DateValue ).ToList();

            var allCountsQry = exceptionList.Select( c => new ExceptionChartData
            {
                DateTimeStamp = c.DateValue.ToJavascriptMilliseconds(),
                YValue = c.ExceptionCount,
                SeriesId = "Total Exceptions"
            } );

            var uniqueCountsQry = exceptionList.Select( c => new ExceptionChartData
            {
                DateTimeStamp = c.DateValue.ToJavascriptMilliseconds(),
                YValue = c.UniqueExceptionCount,
                SeriesId = "Unique Exceptions"
            } );

            var result = allCountsQry.Union( uniqueCountsQry );
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public class ExceptionChartData : IChartData
        {
            public long DateTimeStamp { get; set; }
            public decimal? YValue { get; set; }
            public string SeriesId { get; set; }
        }
    }
}
