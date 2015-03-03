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
using System.Linq.Expressions;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Model;
using Rock.Reporting;
using Rock.Web.UI.Controls;

namespace Rock.Field.Types
{
    /// <summary>
    /// Field used to save and display a date value
    /// </summary>
    [Serializable]
    public class DateFieldType : FieldType
    {

        #region Configuration

        /// <summary>
        /// Returns a list of the configuration keys
        /// </summary>
        /// <returns></returns>
        public override List<string> ConfigurationKeys()
        {
            var keys = base.ConfigurationKeys();
            keys.Add( "format" );
            keys.Add( "displayDiff" );
            keys.Add( "displayCurrentOption" );
            return keys;
        }

        /// <summary>
        /// Creates the HTML controls required to configure this type of field
        /// </summary>
        /// <returns></returns>
        public override System.Collections.Generic.List<System.Web.UI.Control> ConfigurationControls()
        {
            var controls = base.ConfigurationControls();

            var textbox = new RockTextBox();
            controls.Add( textbox );
            textbox.Label = "Date Format";
            textbox.Help = "The format string to use for date (default is system short date).";

            var cbDisplayDiff = new RockCheckBox();
            controls.Add( cbDisplayDiff );
            cbDisplayDiff.Label = "Display as Elapsed Time";
            cbDisplayDiff.Text = "Yes";
            cbDisplayDiff.Help = "Display value as an elapsed time.";

            var cbDisplayCurrent = new RockCheckBox();
            controls.Add( cbDisplayCurrent );
            cbDisplayCurrent.AutoPostBack = true;
            cbDisplayCurrent.CheckedChanged += OnQualifierUpdated;
            cbDisplayCurrent.Label = "Display Current Option";
            cbDisplayCurrent.Text = "Yes";
            cbDisplayCurrent.Help = "Include option to specify value as the current date.";

            return controls;
        }

        /// <summary>
        /// Sets the configuration value.
        /// </summary>
        /// <param name="controls">The controls.</param>
        /// <param name="configurationValues">The configuration values.</param>
        public override void SetConfigurationValues( List<Control> controls, Dictionary<string, ConfigurationValue> configurationValues )
        {
            base.SetConfigurationValues( controls, configurationValues );

            if ( controls != null )
            {
                if ( controls.Count > 0 && controls[0] != null && controls[0] is TextBox &&
                    configurationValues.ContainsKey( "format" ) )
                {
                    ( (TextBox)controls[0] ).Text = configurationValues["format"].Value ?? string.Empty;
                }

                if ( controls.Count > 1 && controls[1] != null && controls[1] is CheckBox &&
                    configurationValues.ContainsKey( "displayDiff" ) )
                {
                    ( (CheckBox)controls[1] ).Checked = configurationValues["displayDiff"].Value.AsBoolean( false );
                }

                if ( controls.Count > 2 && controls[2] != null && controls[2] is CheckBox &&
                    configurationValues.ContainsKey( "displayCurrentOption" ) )
                {
                    ( (CheckBox)controls[2] ).Checked = configurationValues["displayCurrentOption"].Value.AsBoolean( false );
                }
            }
        }

        /// <summary>
        /// Gets the configuration value.
        /// </summary>
        /// <param name="controls">The controls.</param>
        /// <returns></returns>
        public override Dictionary<string, ConfigurationValue> ConfigurationValues( List<Control> controls )
        {
            var values = base.ConfigurationValues( controls );
            values.Add( "format", new ConfigurationValue( "Date Format", "The format string to use for date (default is system short date).", "" ) );
            values.Add( "displayDiff", new ConfigurationValue( "Display as Elapsed Time", "Display value as an elapsed time.", "False" ) );
            values.Add( "displayCurrentOption", new ConfigurationValue( "Display Current Option", "Include option to specify value as the current date.", "False" ) );

            if ( controls != null )
            {
                if ( controls.Count > 0 && controls[0] != null && controls[0] is TextBox )
                {
                    values["format"].Value = ( (TextBox)controls[0] ).Text;
                }

                if ( controls.Count > 1 && controls[1] != null && controls[1] is CheckBox )
                {
                    values["displayDiff"].Value = ( (CheckBox)controls[1] ).Checked.ToString();
                }

                if ( controls.Count > 2 && controls[2] != null && controls[2] is CheckBox )
                {
                    values["displayCurrentOption"].Value = ( (CheckBox)controls[2] ).Checked.ToString();
                }
            }

            return values;
        }

        #endregion

        #region Formatting

        /// <summary>
        /// Formats date display
        /// </summary>
        /// <param name="parentControl">The parent control.</param>
        /// <param name="value">Information about the value</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="condensed">Flag indicating if the value should be condensed (i.e. for use in a grid column)</param>
        /// <returns></returns>
        public override string FormatValue( System.Web.UI.Control parentControl, string value, Dictionary<string, ConfigurationValue> configurationValues, bool condensed )
        {
            if ( string.IsNullOrWhiteSpace( value ) )
            {
                return string.Empty;
            }

            if ( value.StartsWith( "CURRENT", StringComparison.OrdinalIgnoreCase ) )
            {
                DateTime currentDate = RockDateTime.Today;

                var valueParts = value.Split( ':' );
                if ( valueParts.Length > 1 )
                {
                    int? days = valueParts[1].AsIntegerOrNull();
                    if ( days.HasValue && days.Value != 0 )
                    {
                        if ( days > 0 )
                        {
                            return string.Format( "Current Date plus {0} days", days.Value );
                        }
                        else
                        {
                            return string.Format( "Current Date minus {0} days", -days.Value );
                        }
                    }
                }

                return "Current Date";
            }
            else
            {
                string formattedValue = string.Empty;

                DateTime? dateValue = value.AsDateTime();
                if ( dateValue.HasValue )
                {
                    formattedValue = dateValue.Value.ToShortDateString();

                    if ( configurationValues != null &&
                        configurationValues.ContainsKey( "format" ) &&
                        !String.IsNullOrWhiteSpace( configurationValues["format"].Value ) )
                    {
                        try
                        {
                            formattedValue = dateValue.Value.ToString( configurationValues["format"].Value );
                        }
                        catch
                        {
                            formattedValue = dateValue.Value.ToShortDateString();
                        }
                    }

                    if ( !condensed )
                    {
                        if ( configurationValues != null &&
                            configurationValues.ContainsKey( "displayDiff" ) )
                        {
                            bool displayDiff = false;
                            if ( bool.TryParse( configurationValues["displayDiff"].Value, out displayDiff ) && displayDiff )
                                formattedValue += " (" + dateValue.ToElapsedString( true, false ) + ")";
                        }
                    }
                }

                return formattedValue;
            }

        }

        #endregion

        #region Edit Control

        /// <summary>
        /// Creates the control(s) necessary for prompting user for a new value
        /// </summary>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="id"></param>
        /// <returns>
        /// The control
        /// </returns>
        public override Control EditControl( Dictionary<string, ConfigurationValue> configurationValues, string id )
        {
            var datePicker = new DatePicker { ID = id }; 
            datePicker.DisplayCurrentOption = configurationValues != null &&
                configurationValues.ContainsKey( "displayCurrentOption" ) &&
                configurationValues["displayCurrentOption"].Value.AsBoolean();
            return datePicker;
        }

        /// <summary>
        /// Reads new values entered by the user for the field
        /// </summary>
        /// <param name="control">Parent control that controls were added to in the CreateEditControl() method</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <returns></returns>
        public override string GetEditValue( Control control, Dictionary<string, ConfigurationValue> configurationValues )
        {
            var dp = control as DatePicker;
            if ( dp != null )
            {
                if ( dp.DisplayCurrentOption && dp.IsCurrentDateOffset )
                {
                    return string.Format( "CURRENT:{0}", dp.CurrentDateOffsetDays );
                }
                else if ( dp.SelectedDate.HasValue )
                {
                    return dp.SelectedDate.Value.ToString( "o" );
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="value">The value.</param>
        public override void SetEditValue( Control control, Dictionary<string, ConfigurationValue> configurationValues, string value )
        {
            var dp = control as DatePicker;
            if ( dp != null )
            {
                if ( dp.DisplayCurrentOption && value != null && value.StartsWith( "CURRENT", StringComparison.OrdinalIgnoreCase ) )
                {
                    dp.IsCurrentDateOffset = true;
                    var valueParts = value.Split( ':' );
                    if ( valueParts.Length > 1 )
                    {
                        dp.CurrentDateOffsetDays = valueParts[1].AsInteger();
                    }
                }
                else
                {
                    var dt = value.AsDateTime();
                    if ( dt.HasValue )
                    {
                        dp.SelectedDate = dt;
                    }
                }
            }
        }

        #endregion

        #region Filter Control

        /// <summary>
        /// Gets the type of the filter comparison.
        /// </summary>
        /// <value>
        /// The type of the filter comparison.
        /// </value>
        public override ComparisonType FilterComparisonType
        {
            get { return ComparisonHelper.DateFilterComparisonTypes; }
        }

        /// <summary>
        /// Gets the filter value control.
        /// </summary>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="required">if set to <c>true</c> [required].</param>
        /// <returns></returns>
        public override Control FilterValueControl( Dictionary<string, ConfigurationValue> configurationValues, string id, bool required )
        {
            var datePicker = new DatePicker();
            datePicker.ID = string.Format( "{0}_dtPicker", id );
            datePicker.AddCssClass( "js-filter-control" );
            datePicker.DisplayCurrentOption = true;
            return datePicker;
        }
        
        /// <summary>
        /// Gets the filter format script.
        /// </summary>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        /// <remarks>
        /// This script must set a javascript variable named 'result' to a friendly string indicating value of filter controls
        /// a '$selectedContent' should be used to limit script to currently selected filter fields
        /// </remarks>
        public override string GetFilterFormatScript( Dictionary<string, ConfigurationValue> configurationValues, string title )
        {
            string titleJs = System.Web.HttpUtility.JavaScriptStringEncode( title );

            var format = @"
var useCurrentDateOffset = $('.js-current-date-checkbox', $selectedContent).is(':checked');
var dateValue = '';
if (useCurrentDateOffset) {{
    var daysOffset = $('.js-current-date-offset', $selectedContent).val();
    if (daysOffset > 0) {{
        dateValue = 'Current Date plus ' + daysOffset + ' days'; 
    }}
    else if (daysOffset < 0) {{
        dateValue = 'Current Date minus ' + -daysOffset + ' days'; 
    }}
    else {{
        dateValue = 'Current Date';
    }}
}}
else {{
   dateValue = ( $('input', $selectedContent).filter(':visible').length ?  (' ' +  $('input', $selectedContent).filter(':visible').val()  + ' ') : '' );
}}
result = '{0} ' + $('select', $selectedContent).find(':selected').text() + ' \'' + dateValue + '\''";

            return string.Format( format, titleJs );

        }

        /// <summary>
        /// Gets a filter expression for an entity property value.
        /// </summary>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="filterValues">The filter values.</param>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns></returns>
        public override Expression PropertyFilterExpression( Dictionary<string, ConfigurationValue> configurationValues, List<string> filterValues, Expression parameterExpression, string propertyName, Type propertyType )
        {
            if ( filterValues.Count >= 2 )
            {
                filterValues[1] = ParseRelativeValue( filterValues[1] );
            }

            return base.PropertyFilterExpression( configurationValues, filterValues, parameterExpression, propertyName, propertyType );
        }

        /// <summary>
        /// Geta a filter expression for an attribute value.
        /// </summary>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="filterValues">The filter values.</param>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <returns></returns>
        public override Expression AttributeFilterExpression( Dictionary<string, ConfigurationValue> configurationValues, List<string> filterValues, ParameterExpression parameterExpression )
        {
            if ( filterValues.Count >= 2 )
            {
                string comparisonValue = filterValues[0];
                if ( comparisonValue != "0" )
                {
                    filterValues[1] = ParseRelativeValue( filterValues[1] );
                    DateTime date = filterValues[1].AsDateTime() ?? DateTime.MinValue;

                    ComparisonType comparisonType = comparisonValue.ConvertToEnum<ComparisonType>( ComparisonType.EqualTo );
                    MemberExpression propertyExpression = Expression.Property( parameterExpression, "ValueAsDateTime" );
                    ConstantExpression constantExpression = Expression.Constant( date, typeof( DateTime ) );

                    return ComparisonHelper.ComparisonExpression( comparisonType, propertyExpression, constantExpression );
                }
            }

            return null;
        }

        /// <summary>
        /// Checks to see if value is for 'current' date and if so, adjusts the date value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual string ParseRelativeValue( string value )
        {
            if ( value.StartsWith( "CURRENT", StringComparison.OrdinalIgnoreCase ) )
            {
                DateTime currentDate = RockDateTime.Today;

                var valueParts = value.Split( ':' );

                if ( valueParts.Length > 1 )
                {
                    currentDate = currentDate.AddDays( valueParts[1].AsInteger() );
                }

                return currentDate.ToString( "o" );
            }

            return value;
        }

        #endregion

    }
}