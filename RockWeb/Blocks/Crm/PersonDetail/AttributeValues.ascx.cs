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
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Rock;
using Rock.Attribute;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Crm.PersonDetail
{
    /// <summary>
    /// User control for editing the value(s) of a set of attributes for person
    /// </summary>
    [DisplayName( "Attribute Values" )]
    [Category( "CRM > Person Detail" )]
    [Description( "Allows for editing the value(s) of a set of attributes for person." )]

    [AttributeCategoryField( "Category", "The Attribute Category to display attributes from", false, "Rock.Model.Person" )]
    public partial class AttributeValues : PersonBlock
    {

        /// <summary>
        /// Gets or sets a value indicating whether [edit mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [edit mode]; otherwise, <c>false</c>.
        /// </value>
        protected bool EditMode
        {
            get { return ViewState["EditMode"] as bool? ?? false; }
            set { ViewState["EditMode"] = value; }
        }

        /// <summary>
        /// Gets or sets the attribute list.
        /// </summary>
        /// <value>
        /// The attribute list.
        /// </value>
        protected List<int> AttributeList
        {
            get 
            { 
                List<int> attributeList = ViewState["AttributeList"] as List<int>;
                if (attributeList == null)
                {
                    attributeList = new List<int>();
                    ViewState["AttributeList"] = attributeList;
                }
                return attributeList;
            }
            set { ViewState["AttributeList"] = value; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
 	        base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string categoryGuid = GetAttributeValue( "Category" );
                Guid guid = Guid.Empty;
                if ( Guid.TryParse( categoryGuid, out guid ) )
                {
                    var category = CategoryCache.Read( guid );
                    if ( category != null )
                    {
                        if (!string.IsNullOrWhiteSpace(category.IconCssClass))
                        {
                            lCategoryName.Text = string.Format( "<i class='{0}'></i> {1}", category.IconCssClass, category.Name );
                        }
                        else
                        {
                            lCategoryName.Text = category.Name;
                        }

                        foreach ( var attribute in new AttributeService().GetByCategoryId( category.Id ) )
                        {
                            if ( attribute.IsAuthorized( "View", CurrentPerson ) )
                            {
                                AttributeList.Add( attribute.Id );
                            }
                        }

                    }
                }

                CreateControls( true );
            }
        }

        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );
            CreateControls( false );
        }

        private void CreateControls( bool setValues )
        {
            fsAttributes.Controls.Clear();

            if ( Person != null )
            {
                foreach ( int attributeId in AttributeList )
                {
                    var attribute = AttributeCache.Read( attributeId );
                    string attributeValue = Person.GetAttributeValue( attribute.Key );
                    string formattedValue = string.Empty;

                    if ( !EditMode || !attribute.IsAuthorized( "Edit", CurrentPerson ) )
                    {
                        formattedValue = attribute.FieldType.Field.FormatValue( fsAttributes, attributeValue, attribute.QualifierValues, false );
                        if ( !string.IsNullOrWhiteSpace( formattedValue ) )
                        {
                            fsAttributes.Controls.Add( new RockLiteral { Label = attribute.Name, Text = formattedValue } );
                        }
                    }
                    else
                    {
                        attribute.AddControl( fsAttributes.Controls, attributeValue, string.Empty, setValues, true );
                    }
                }
            }

            pnlActions.Visible = EditMode;
        }

        protected void lbEdit_Click( object sender, EventArgs e )
        {
            EditMode = true;
            CreateControls( true );
        }

        protected void btnSave_Click( object sender, EventArgs e )
        {
            if ( Page.IsValid )
            {
                int personEntityTypeId = EntityTypeCache.Read(typeof(Person)).Id;

                using (new Rock.Data.UnitOfWorkScope())
                {
                    Rock.Data.RockTransactionScope.WrapTransaction( () =>
                    {
                        var changes = new List<string>();
                        var historyService = new HistoryService();

                        foreach ( int attributeId in AttributeList )
                        {
                            var attribute = AttributeCache.Read( attributeId );

                            if ( Person != null && EditMode && attribute.IsAuthorized( "Edit", CurrentPerson ) )
                            {
                                Control attributeControl = fsAttributes.FindControl( string.Format( "attribute_field_{0}", attribute.Id ) );
                                if ( attributeControl != null )
                                {
                                    string originalValue = Person.GetAttributeValue( attribute.Key );
                                    string newValue = attribute.FieldType.Field.GetEditValue( attributeControl, attribute.QualifierValues );
                                    Rock.Attribute.Helper.SaveAttributeValue( Person, attribute, newValue, CurrentPersonId );

                                    // Check for changes to write to history
                                    if ( ( originalValue ?? string.Empty ).Trim() != ( newValue ?? string.Empty ).Trim() )
                                    {
                                        string formattedOriginalValue = string.Empty;
                                        if ( !string.IsNullOrWhiteSpace( originalValue ) )
                                        {
                                            formattedOriginalValue = attribute.FieldType.Field.FormatValue( null, originalValue, attribute.QualifierValues, false );
                                        }

                                        string formattedNewValue = string.Empty;
                                        if ( !string.IsNullOrWhiteSpace( originalValue ) )
                                        {
                                            formattedNewValue = attribute.FieldType.Field.FormatValue( null, newValue, attribute.QualifierValues, false );
                                        }

                                        History.EvaluateChange( changes, attribute.Name, formattedOriginalValue, formattedNewValue );
                                    }
                                }
                            }
                        }

                        if ( changes.Any() )
                        {
                            new HistoryService().SaveChanges( typeof( Person ), Rock.SystemGuid.Category.HISTORY_PERSON_DEMOGRAPHIC_CHANGES.AsGuid(),
                                Person.Id, changes, CurrentPersonId );
                        }
                    } );
                }

                EditMode = false;
                CreateControls( false );
            }
        }

        protected void btnCancel_Click( object sender, EventArgs e )
        {
            EditMode = false;
            CreateControls( false );
        }
    }
}
