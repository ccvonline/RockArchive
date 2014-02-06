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
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;

using Rock.Model;

namespace Rock.PersonProfile.Badge
{
    // TODO: Update to return actual data

    /// <summary>
    /// eraAttendance Badge
    /// </summary>
    [Description( "eraAttendance Badge" )]
    [Export( typeof( BadgeComponent ) )]
    [ExportMetadata( "ComponentName", "eraAttendance" )]
    public class eraAttendanceAttendance : IconBadge
    {

        /// <summary>
        /// Gets the tool tip text.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public override string GetToolTipText( Person person )
        {
            return "Family has attended 14 times in the last 16 weeks";
        }

        /// <summary>
        /// Gets the icon path.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public override string GetIconPath( Person person )
        {
            return Path.Combine( System.Web.VirtualPathUtility.ToAbsolute( "~" ), "Assets/Mockup/attendence-16.jpg" );
        }
    }
}
