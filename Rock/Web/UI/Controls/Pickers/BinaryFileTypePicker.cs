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
using System.Linq;
using System.Web.UI.WebControls;

using Rock.Model;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class BinaryFileTypePicker : RockDropDownList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryFileTypePicker" /> class.
        /// </summary>
        public BinaryFileTypePicker()
        {
            this.Items.Clear();
            this.DataTextField = "Name";
            this.DataValueField = "Id";
            this.DataSource = new BinaryFileTypeService().Queryable().OrderBy( f => f.Name ).ToList();
            this.DataBind();
        }

        /// <summary>
        /// Selects the value as int.
        /// </summary>
        /// <returns></returns>
        public int? SelectedValueAsInt( bool NoneAsNull = true )
        {
            if ( NoneAsNull )
            {
                if ( this.SelectedValue.Equals( Rock.Constants.None.Id.ToString() ) )
                {
                    return null;
                }
            }

            if ( string.IsNullOrWhiteSpace( this.SelectedValue ) )
            {
                return null;
            }
            else
            {
                return int.Parse( this.SelectedValue );
            }
        }

    }
}