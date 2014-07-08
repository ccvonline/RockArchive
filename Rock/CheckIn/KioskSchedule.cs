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
using System.Runtime.Serialization;

using Rock.Model;

namespace Rock.CheckIn
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class KioskSchedule
    {
        /// <summary>
        /// Gets or sets the schedule.
        /// </summary>
        /// <value>
        /// The schedule.
        /// </value>
        [DataMember]
        public Schedule Schedule { get; set; }

        /// <summary>
        /// Gets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        [DataMember]
        public DateTime? StartTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KioskSchedule" /> class.
        /// </summary>
        public KioskSchedule()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KioskSchedule" /> class.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        public KioskSchedule( Schedule schedule )
            : base()
        {
            Schedule = schedule.Clone( false );

            var calEvent = Schedule.GetCalenderEvent();
            if ( calEvent != null && calEvent.DTStart != null )
            {
                StartTime = calEvent.DTStart.Value;
            }

        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Schedule.ToString();
        }
    }
}