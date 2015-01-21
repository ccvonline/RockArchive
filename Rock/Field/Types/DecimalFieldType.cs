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
using System.Web.UI;
using Rock.Model;
using Rock.Reporting;
using Rock.Web.UI.Controls;

namespace Rock.Field.Types
{
    /// <summary>
    /// Field used to save and display a decimal numeric value
    /// </summary>
    [Serializable]
    public class DecimalFieldType : FieldType
    {

        /// <summary>
        /// Gets the align value that should be used when displaying value
        /// </summary>
        public override System.Web.UI.WebControls.HorizontalAlign AlignValue
        {
            get
            {
                return System.Web.UI.WebControls.HorizontalAlign.Right;
            }
        }

        /// <summary>
        /// Tests the value to ensure that it is a valid value.  If not, message will indicate why
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="required">if set to <c>true</c> [required].</param>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid( string value, bool required, out string message )
        {
            if ( !string.IsNullOrWhiteSpace( value ) )
            {
                decimal result;
                if ( !decimal.TryParse( value, out result ) )
                {
                    message = "The input provided is not a valid decimal value.";
                    return true;
                }
            }

            return base.IsValid( value, required, out message );
        }

        /// <summary>
        /// Gets the type of the filter comparison.
        /// </summary>
        /// <value>
        /// The type of the filter comparison.
        /// </value>
        public override ComparisonType FilterComparisonType
        {
            get { return ComparisonHelper.NumericFilterComparisonTypes; }
        }

        /// <summary>
        /// Gets the filter value control.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public override Control FilterValueControl( string id )
        {
            var numberBox = new NumberBox();
            numberBox.ID = string.Format( "{0}_numberBox", id );
            numberBox.AddCssClass( "js-filter-control" );
            numberBox.NumberType = System.Web.UI.WebControls.ValidationDataType.Double;
            return numberBox;
        }

        /// <summary>
        /// Creates the control(s) necessary for prompting user for a new value
        /// </summary>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="id"></param>
        /// <returns>
        /// The control
        /// </returns>
        public override System.Web.UI.Control EditControl( System.Collections.Generic.Dictionary<string, ConfigurationValue> configurationValues, string id )
        {
            var numberBox = new NumberBox { ID = id }; 
            numberBox.NumberType = System.Web.UI.WebControls.ValidationDataType.Double;
            return numberBox;
        }
    }
}