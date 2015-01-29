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
using System;
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Schedule Service class
    /// </summary>
    public partial class ScheduleService : Service<Schedule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public ScheduleService(RockContext context) : base(context)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( Schedule item, out string errorMessage )
        {
            errorMessage = string.Empty;
 
            if ( new Service<Metric>( Context ).Queryable().Any( a => a.ScheduleId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Schedule.FriendlyTypeName, Metric.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class ScheduleExtensionMethods
    {
        /// <summary>
        /// Clones this Schedule object to a new Schedule object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static Schedule Clone( this Schedule source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as Schedule;
            }
            else
            {
                var target = new Schedule();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another Schedule object to this Schedule object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this Schedule target, Schedule source )
        {
            target.Id = source.Id;
            target.CategoryId = source.CategoryId;
            target.CheckInEndOffsetMinutes = source.CheckInEndOffsetMinutes;
            target.CheckInStartOffsetMinutes = source.CheckInStartOffsetMinutes;
            target.Description = source.Description;
            target.iCalendarContent = source.iCalendarContent;
            target.Name = source.Name;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}
