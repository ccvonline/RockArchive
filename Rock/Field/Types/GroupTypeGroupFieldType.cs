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
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI.Controls;

namespace Rock.Field.Types
{
    /// <summary>
    /// Field Type to select a single (or null) group filtered by a selected group type
    /// Stored as "GroupType.Guid|Group.Guid"
    /// </summary>
    public class GroupTypeGroupFieldType : FieldType
    {
        /// <summary>
        /// Returns the field's current value(s)
        /// </summary>
        /// <param name="parentControl">The parent control.</param>
        /// <param name="value">Information about the value</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="condensed">Flag indicating if the value should be condensed (i.e. for use in a grid column)</param>
        /// <returns></returns>
        public override string FormatValue( Control parentControl, string value, Dictionary<string, ConfigurationValue> configurationValues, bool condensed )
        {
            string formattedValue = string.Empty;

            Guid? groupTypeGuid = null;
            Guid? groupGuid = null;

            string[] parts = ( value ?? string.Empty ).Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries );
            if ( parts.Length > 0 )
            {
                groupTypeGuid = parts[0].AsGuidOrNull();
                if ( parts.Length > 1 )
                {
                    groupGuid = parts[1].AsGuidOrNull();
                }
            }

            var rockContext = new RockContext();
            if ( groupGuid.HasValue )
            {
                var group = new GroupService( rockContext ).Get( groupGuid.Value );
                if ( group != null )
                {
                    formattedValue = "Group: " + group.Name;
                }
            }
            else if ( groupTypeGuid.HasValue )
            {
                var groupType = new GroupTypeService( rockContext ).Get( groupTypeGuid.Value );
                if ( groupType != null )
                {
                    formattedValue = "Group type: " + groupType.Name;
                }
            }

            return base.FormatValue( parentControl, formattedValue, null, condensed );
        }

        /// <summary>
        /// Creates the control(s) necessary for prompting user for a new value
        /// </summary>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="id"></param>
        /// <returns>
        /// The control
        /// </returns>
        public override System.Web.UI.Control EditControl( Dictionary<string, ConfigurationValue> configurationValues, string id )
        {
            GroupTypeGroupPicker editControl = new GroupTypeGroupPicker { ID = id };
            editControl.GroupControlLabel = "Root Group";
            return editControl;
        }

        /// <summary>
        /// Reads new values entered by the user for the field ( as Guid )
        /// </summary>
        /// <param name="control">Parent control that controls were added to in the CreateEditControl() method</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <returns></returns>
        public override string GetEditValue( System.Web.UI.Control control, Dictionary<string, ConfigurationValue> configurationValues )
        {
            GroupTypeGroupPicker groupTypeGroupPicker = control as GroupTypeGroupPicker;
            if ( groupTypeGroupPicker != null )
            {
                var rockContext = new RockContext();

                Guid? groupTypeGuid = null;
                Guid? groupGuid = null;
                if ( groupTypeGroupPicker.GroupTypeId.HasValue )
                {
                    var groupType = new GroupTypeService( rockContext ).Get( groupTypeGroupPicker.GroupTypeId.Value );
                    if ( groupType != null )
                    {
                        groupTypeGuid = groupType.Guid;
                    }
                }

                if ( groupTypeGroupPicker.GroupId.HasValue )
                {
                    var group = new GroupService( rockContext ).Get( groupTypeGroupPicker.GroupId.Value );
                    if ( group != null )
                    {
                        groupGuid = group.Guid;
                    }
                }

                return string.Format( "{0}|{1}", groupTypeGuid, groupGuid );

            }

            return null;
        }

        /// <summary>
        /// Sets the value. ( as Guid )
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="value">The value.</param>
        public override void SetEditValue( System.Web.UI.Control control, Dictionary<string, ConfigurationValue> configurationValues, string value )
        {
            GroupTypeGroupPicker groupTypeGroupPicker = control as GroupTypeGroupPicker;
            if ( groupTypeGroupPicker != null )
            {
                // initialize in case the value isn't set
                groupTypeGroupPicker.GroupTypeId = null;
                groupTypeGroupPicker.GroupId = null;

                string[] parts = ( value ?? string.Empty ).Split( '|' );
                var rockContext = new RockContext();
                if ( parts.Length >= 1 )
                {
                    var groupType = new GroupTypeService( rockContext ).Get( parts[0].AsGuid() );
                    if ( groupType != null )
                    {
                        groupTypeGroupPicker.GroupTypeId = groupType.Id;
                    }

                    if ( parts.Length >= 2 )
                    {
                        var group = new GroupService( rockContext ).Get( parts[1].AsGuid() );
                        if ( group != null )
                        {
                            groupTypeGroupPicker.GroupId = group.Id;
                        }
                    }
                }
            }
        }
    }
}