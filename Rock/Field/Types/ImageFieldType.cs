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
using System.IO;
using System.Web.UI;
using Rock.Data;
using Rock.Model;

namespace Rock.Field.Types
{
    /// <summary>
    /// Field used to save and display an image value
    /// </summary>
    [Serializable]
    public class ImageFieldType : BinaryFileFieldType
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
            if ( !string.IsNullOrWhiteSpace( value ) )
            {
                var imagePath = System.Web.VirtualPathUtility.ToAbsolute( "~/GetImage.ashx" );

                // create querystring parms
                string queryParms = string.Empty;
                if ( condensed )
                {
                    queryParms = "&width=100"; // for grids hardcode to 100px wide
                }
                else
                {
                    // determine image size parameters
                    // width
                    if ( configurationValues != null &&
                        configurationValues.ContainsKey( "width" ) &&
                        !String.IsNullOrWhiteSpace( configurationValues["width"].Value ) )
                    {
                        queryParms = "&width=" + configurationValues["width"].Value;
                    }

                    // height
                    if ( configurationValues != null &&
                        configurationValues.ContainsKey( "height" ) &&
                        !String.IsNullOrWhiteSpace( configurationValues["height"].Value ) )
                    {
                        queryParms += "&height=" + configurationValues["height"].Value;
                    }
                }

                string imageUrlFormat = "<img src='" + imagePath + "?id={0}{1}' />";
                return string.Format( imageUrlFormat, value, queryParms );
            }
            else
            {
                return string.Empty;
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
            var control = new Web.UI.Controls.ImageUploader { ID = id };

            if ( configurationValues != null && configurationValues.ContainsKey( "binaryFileType" ) )
            {
                int? binaryFileTypeId = configurationValues["binaryFileType"].Value.AsInteger();
                if ( binaryFileTypeId.HasValue )
                {
                    var binaryFileType = new BinaryFileTypeService( new RockContext() ).Get( binaryFileTypeId.Value );

                    if ( binaryFileType != null )
                    {
                        control.BinaryFileTypeGuid = binaryFileType.Guid;
                    }
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
            if ( control != null && control is Rock.Web.UI.Controls.ImageUploader )
            {
                int? imageId = ( (Rock.Web.UI.Controls.ImageUploader)control ).BinaryFileId;
                return imageId.ToString();
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
            int? idvalue = value.AsInteger();
            if ( control != null && control is Rock.Web.UI.Controls.ImageUploader )
            {
                ( control as Rock.Web.UI.Controls.ImageUploader ).BinaryFileId = idvalue;
            }
        }
    }
}