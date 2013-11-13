﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Attribute;
using Rock.Model;
using Rock.Web.UI;

namespace RockWeb.Blocks.Utility
{
    /// <summary>
    /// User controls for managing defined values
    /// </summary>    
    [DefinedTypeField( "Defined Type", "The Defined Type to display values for." )]
    [TextField( "Attribute Key", "The attribute key on the Defined Type that is used to store whether item has been completed (should be a boolean field type)." )]
    [BooleanField( "Hide Checked Items", "Should checked items be hidden?", false )]
    public partial class DefinedTypeCheckList : RockBlock
    {
        private string attributeKey = string.Empty;
        private bool hideCheckedItems = false;

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            attributeKey = GetAttributeValue( "AttributeKey" );
            if ( !bool.TryParse( GetAttributeValue( "HideCheckedItems" ), out hideCheckedItems ) )
            {
                hideCheckedItems = false;
            }

            this.AttributesUpdated += DefinedTypeCheckList_AttributesUpdated;
            rptrValues.ItemDataBound += rptrValues_ItemDataBound;
            BindList();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( Page.IsPostBack )
            {
                var definedValueService = new DefinedValueService();

                foreach ( RepeaterItem item in rptrValues.Items )
                {
                    var hfValue = item.FindControl( "hfValue" ) as HiddenField;
                    var cbValue = item.FindControl( "cbValue" ) as CheckBox;

                    if ( hfValue != null && cbValue != null )
                    {
                        var value = definedValueService.Get( hfValue.ValueAsInt() );
                        if ( value != null )
                        {
                            Helper.LoadAttributes( value );
                            value.SetAttributeValue( attributeKey, cbValue.Checked.ToString() );
                            Helper.SaveAttributeValues( value, CurrentPersonId );
                        }
                    }
                }

                BindList();
            }
        }

        /// <summary>
        /// Handles the ItemDataBound event of the rptrValues control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void rptrValues_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            if ( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
            {
                var definedValue = e.Item.DataItem as DefinedValue;
                var pnlValue = e.Item.FindControl( "pnlValue" ) as Panel;
                var cbValue = e.Item.FindControl( "cbValue" ) as CheckBox;
                if ( definedValue != null && pnlValue != null && cbValue != null )
                {
                    Helper.LoadAttributes( definedValue );

                    cbValue.Text = string.Format( "{0}<br/><small>{1}</small>", definedValue.Name, definedValue.Description );

                    bool selected = false;
                    if ( !bool.TryParse( definedValue.GetAttributeValue( attributeKey ), out selected ) )
                    {
                        selected = false;
                    }
                    cbValue.Checked = selected;
                    pnlValue.CssClass = selected ? "text-muted" : "";
                    pnlValue.Visible = !hideCheckedItems || !selected;
                }
            }
        }

        /// <summary>
        /// Handles the AttributesUpdated event of the DefinedTypeCheckList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void DefinedTypeCheckList_AttributesUpdated( object sender, EventArgs e )
        {
            BindList();
        }

        private void BindList()
        {
            Guid guid = Guid.Empty;
            if ( Guid.TryParse( GetAttributeValue( "DefinedType" ), out guid ) )
            {
                var definedType = new DefinedTypeService().Get( guid );
                rptrValues.DataSource = definedType.DefinedValues;
                rptrValues.DataBind();
            }
        }

    }
}