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
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock.Web.UI.Controls;

namespace Rock.Field.Types
{
    /// <summary>
    /// Field Type used to display a list of options as checkboxes.  Value is saved as a | delimited list
    /// </summary>
    [Serializable]
    public class SelectMultiFieldType : FieldType
    {
        /// <summary>
        /// Returns a list of the configuration keys
        /// </summary>
        /// <returns></returns>
        public override List<string> ConfigurationKeys()
        {
            List<string> configKeys = new List<string>();
            configKeys.Add( "values" );
            return configKeys;
        }

        /// <summary>
        /// Creates the HTML controls required to configure this type of field
        /// </summary>
        /// <returns></returns>
        public override List<Control> ConfigurationControls()
        {
            List<Control> controls = new List<Control>();

            var tb = new RockTextBox();
            controls.Add( tb );
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Rows = 3;
            tb.AutoPostBack = true;
            tb.TextChanged += OnQualifierUpdated;
            tb.Label = "Values";
            tb.Help = "The source of the values to display in a list.  Format is either 'value1,value2,value3,...', 'value1:text1,value2:text2,value3:text3,...', or a SQL Select statement that returns result set with a 'Value' and 'Text' column.";
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
            configurationValues.Add( "values", new ConfigurationValue( "Values", 
                "The source of the values to display in a list.  Format is either 'value1,value2,value3,...', 'value1:text1,value2:text2,value3:text3,...', or a SQL Select statement that returns result set with a 'Value' and 'Text' column.", "" ) );

            if ( controls != null && controls.Count == 1 )
            {
                if ( controls[0] != null && controls[0] is TextBox )
                    configurationValues["values"].Value = ( ( TextBox )controls[0] ).Text;
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
                controls[0] != null && controls[0] is TextBox && configurationValues.ContainsKey("values"))
                    ( ( TextBox )controls[0] ).Text = configurationValues["values"].Value;
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
            if ( configurationValues != null && configurationValues.ContainsKey( "values" ) )
            {
                var editControl = new RockCheckBoxList { ID = id };
                editControl.RepeatDirection = RepeatDirection.Horizontal;

                string listSource = configurationValues["values"].Value;

                if ( listSource.ToUpper().Contains( "SELECT" ) && listSource.ToUpper().Contains( "FROM" ) )
                {
                    var tableValues = new List<string>();
                    DataTable dataTable = new Rock.Data.Service().GetDataTable( listSource, CommandType.Text, null );
                    if ( dataTable != null && dataTable.Columns.Contains( "Value" ) && dataTable.Columns.Contains( "Text" ) )
                    {
                        foreach ( DataRow row in dataTable.Rows )
                        {
                            editControl.Items.Add( new ListItem( row["text"].ToString(), row["value"].ToString() ) );
                        }
                    }
                }

                else
                {
                    foreach ( string keyvalue in listSource.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ) )
                    {
                        var keyValueArray = keyvalue.Split( new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries );
                        if ( keyValueArray.Length > 0 )
                        {
                            ListItem li = new ListItem();
                            li.Value = keyValueArray[0];
                            li.Text = keyValueArray.Length > 1 ? keyValueArray[1] : keyValueArray[0];
                            editControl.Items.Add( li );
                        }
                    }
                }

                if ( editControl.Items.Count > 0 )
                {
                    return editControl;
                }
            }

            return null;
        }

        /// <summary>
        /// Reads new values entered by the user for the field
        /// </summary>
        /// <param name="control">Parent control that controls were added to in the CreateEditControl() method</param>
        /// <param name="configurationValues"></param>
        /// <returns></returns>
        public override string GetEditValue( Control control, Dictionary<string, ConfigurationValue> configurationValues )
        {
            List<string> values = new List<string>();

            if ( control != null && control is CheckBoxList )
            {
                CheckBoxList cbl = ( CheckBoxList )control;
                foreach ( ListItem li in cbl.Items )
                    if ( li.Selected )
                        values.Add( li.Value );
                return values.AsDelimited<string>( "," );
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
            if ( value != null )
            {
                List<string> values = new List<string>();
                values.AddRange( value.Split( ',' ) );

                if ( control != null && control is CheckBoxList )
                {
                    CheckBoxList cbl = (CheckBoxList)control;
                    foreach ( ListItem li in cbl.Items )
                        li.Selected = values.Contains( li.Value );
                }
            }
        }
    }
}