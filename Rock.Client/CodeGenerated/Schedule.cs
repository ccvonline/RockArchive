//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
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


namespace Rock.Client
{
    /// <summary>
    /// Base client model for Schedule that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class ScheduleEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public int? CategoryId { get; set; }

        /// <summary />
        public int? CheckInEndOffsetMinutes { get; set; }

        /// <summary />
        public int? CheckInStartOffsetMinutes { get; set; }

        /// <summary />
        public string Description { get; set; }

        /// <summary />
        public DateTime? EffectiveEndDate { get; set; }

        /// <summary />
        public DateTime? EffectiveStartDate { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public string iCalendarContent { get; set; }

        /// <summary />
        public string Name { get; set; }

        /// <summary />
        public int /* DayOfWeek*/? WeeklyDayOfWeek { get; set; }

        /// <summary />
        public TimeSpan? WeeklyTimeOfDay { get; set; }

        /// <summary />
        public DateTime? CreatedDateTime { get; set; }

        /// <summary />
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary />
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary />
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source Schedule object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( Schedule source )
        {
            this.Id = source.Id;
            this.CategoryId = source.CategoryId;
            this.CheckInEndOffsetMinutes = source.CheckInEndOffsetMinutes;
            this.CheckInStartOffsetMinutes = source.CheckInStartOffsetMinutes;
            this.Description = source.Description;
            this.EffectiveEndDate = source.EffectiveEndDate;
            this.EffectiveStartDate = source.EffectiveStartDate;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.iCalendarContent = source.iCalendarContent;
            this.Name = source.Name;
            this.WeeklyDayOfWeek = source.WeeklyDayOfWeek;
            this.WeeklyTimeOfDay = source.WeeklyTimeOfDay;
            this.CreatedDateTime = source.CreatedDateTime;
            this.ModifiedDateTime = source.ModifiedDateTime;
            this.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            this.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for Schedule that includes all the fields that are available for GETs. Use this for GETs (use ScheduleEntity for POST/PUTs)
    /// </summary>
    public partial class Schedule : ScheduleEntity
    {
        /// <summary />
        public Category Category { get; set; }

        /// <summary />
        public string FriendlyScheduleText { get; set; }

    }
}
