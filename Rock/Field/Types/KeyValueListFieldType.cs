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
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock.Web.UI.Controls;
using Rock.Data;

namespace Rock.Field.Types
{
    /// <summary>
    /// Field used to save and display a key/value list
    /// </summary>
    [Serializable]
    public class KeyValueListFieldType : FieldType
    {
        /// <summary>
        /// Returns the field's current value(s)
        /// </summary>
        /// <param name="parentControl">The parent control.</param>
        /// <param name="value">Information about the value</param>
        /// <param name="configurationValues"></param>
        /// <param name="condensed">Flag indicating if the value should be condensed (i.e. for use in a grid column)</param>
        /// <returns></returns>
        public override string FormatValue( Control parentControl, string value, Dictionary<string, ConfigurationValue> configurationValues, bool condensed )
        {
            string formattedValue = string.Empty;

            string[] KeyValues = value.Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries );
            if ( KeyValues.Length == 1 )
            {
                formattedValue = "1 Key Value Pair";
            }
            else
            {
                formattedValue = string.Format( "{0:N0} Key Value Pairs", KeyValues.Length );
            }

            return base.FormatValue( parentControl, formattedValue, null, condensed );
        }

        /// <summary>
        /// Returns a list of the configuration keys
        /// </summary>
        /// <returns></returns>
        public override List<string> ConfigurationKeys()
        {
            var configKeys = base.ConfigurationKeys();
            configKeys.Add( "definedtype" );
            return configKeys;
        }

        /// <summary>
        /// Creates the HTML controls required to configure this type of field
        /// </summary>
        /// <returns></returns>
        public override List<Control> ConfigurationControls()
        {
            var controls = base.ConfigurationControls();

            var ddl = new RockDropDownList();
            controls.Add( ddl );
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += OnQualifierUpdated;
            ddl.DataTextField = "Name";
            ddl.DataValueField = "Id";
            ddl.DataSource = new Rock.Model.DefinedTypeService( new RockContext() ).Queryable().OrderBy( d => d.Order ).ToList();
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(string.Empty, string.Empty));
            ddl.Label = "Defined Type";
            ddl.Help = "Optional Defined Type to select values from, otherwise values will be free-form text fields.";
            return controls;
        }

        /// <summary>
        /// Gets the configuration value.
        /// </summary>
        /// <param name="controls">The controls.</param>
        /// <returns></returns>
        public override Dictionary<string, ConfigurationValue> ConfigurationValues( List<Control> controls )
        {
            Dictionary<string, ConfigurationValue> configurationValues = new Dictionary<string, ConfigurationValue>();
            configurationValues.Add( "definedtype", new ConfigurationValue( "Defined Type", "Optional Defined Type to select values from, otherwise values will be free-form text fields", "" ) );

            if ( controls != null && controls.Count == 1 &&
                controls[0] != null && controls[0] is DropDownList )
            {
                configurationValues["definedtype"].Value = ( (DropDownList)controls[0] ).SelectedValue;
            }

            return configurationValues;
        }

        /// <summary>
        /// Sets the configuration value.
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="configurationValues"></param>
        public override void SetConfigurationValues( List<Control> controls, Dictionary<string, ConfigurationValue> configurationValues )
        {
            if ( controls != null && controls.Count == 1 && configurationValues != null &&
                controls[0] != null && controls[0] is DropDownList && configurationValues.ContainsKey( "definedtype" ) )
            {
                ( (DropDownList)controls[0] ).SelectedValue = configurationValues["definedtype"].Value;
            }
        }

        /// <summary>
        /// Creates the control(s) neccessary for prompting user for a new value
        /// </summary>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="id"></param>
        /// <returns>
        /// The control
        /// </returns>
        public override Control EditControl( Dictionary<string, ConfigurationValue> configurationValues, string id )
        {
            var control = new KeyValueList { ID = id }; 

            if ( configurationValues != null && configurationValues.ContainsKey( "definedtype" ) )
            {
                int definedTypeId = 0;
                if ( Int32.TryParse( configurationValues["definedtype"].Value, out definedTypeId ) )
                {
                    control.DefinedTypeId = definedTypeId;
                }
            }

            return control;
        }

        /// <summary>
        /// Reads new values entered by the user for the field
        /// </summary>
        /// <param name="control">Parent control that controls were added to in the CreateEditControl() method</param>
        /// <param name="configurationValues"></param>
        /// <returns></returns>
        public override string GetEditValue( Control control, Dictionary<string, ConfigurationValue> configurationValues )
        {
            if ( control != null && control is KeyValueList )
            {
                return ( (KeyValueList)control ).Value;
            }
            return null;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="configurationValues"></param>
        /// <param name="value">The value.</param>
        public override void SetEditValue( Control control, Dictionary<string, ConfigurationValue> configurationValues, string value )
        {
            if ( value!= null && control != null && control is KeyValueList )
            {
                ( (KeyValueList)control ).Value = value;
            }
        }
    }
}