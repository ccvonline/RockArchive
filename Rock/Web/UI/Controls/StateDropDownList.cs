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
using System.Linq;
using Rock.Web.Cache;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class StateDropDownList : RockDropDownList
    {
        private bool _rebindRequired = false;

        /// <summary>
        /// Display an abbreviated state name
        /// </summary>
        public bool UseAbbreviation
        {
            get { return ViewState["UseAbbreviation"] as bool? ?? false; }
            set 
            {
                _rebindRequired = ( ViewState["UseAbbreviation"] as bool? ?? false ) != value;
                ViewState["UseAbbreviation"] = value; 
            }
        }

        /// <summary>
        /// Handles the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            this.DataValueField = "Id";
            this.DataTextField = UseAbbreviation ? "Id" : "Value";

            _rebindRequired = false;
            var definedType = DefinedTypeCache.Read( new Guid( SystemGuid.DefinedType.LOCATION_ADDRESS_STATE ) );
            this.DataSource = definedType.DefinedValues.OrderBy( v => v.Order ).Select( v => new { Id = v.Name, Value = v.Description } );
            this.DataBind();

            if ( !this.Page.IsPostBack )
            {
                string orgLocGuid = GlobalAttributesCache.Read().GetValue( "OrganizationAddress" );
                if ( !string.IsNullOrWhiteSpace( orgLocGuid ) )
                {
                    Guid locGuid = Guid.Empty;
                    if ( Guid.TryParse( orgLocGuid, out locGuid ) )
                    {
                        var location = new Rock.Model.LocationService().Get( locGuid );
                        if ( location != null )
                        {
                            var li = this.Items.FindByValue( location.State );
                            if ( li != null )
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( System.Web.UI.HtmlTextWriter writer )
        {
            if ( _rebindRequired )
            {
                string value = this.SelectedValue;

                this.DataTextField = UseAbbreviation ? "Id" : "Value";
                this.DataBind();

                var li = this.Items.FindByValue( value );
                if ( li != null )
                {
                    li.Selected = true;
                }
            }

            base.RenderControl( writer );
        }

    }
}
