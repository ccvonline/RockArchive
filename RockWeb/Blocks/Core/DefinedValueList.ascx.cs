﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Rock;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using Attribute = Rock.Model.Attribute;

namespace RockWeb.Blocks.Core
{
    /// <summary>
    /// User controls for managing defined values
    /// </summary>
    [DisplayName( "Defined Value List" )]
    [Category( "Core" )]
    [Description( "Block for viewing values for a defined type." )]
    public partial class DefinedValueList : RockBlock, ISecondaryBlock
    {        
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            gDefinedValues.DataKeyNames = new string[] { "id" };
            gDefinedValues.Actions.ShowAdd = true;
            gDefinedValues.Actions.AddClick += gDefinedValues_Add;
            gDefinedValues.GridRebind += gDefinedValues_GridRebind;
            gDefinedValues.GridReorder += gDefinedValues_GridReorder;

            bool canAddEditDelete = IsUserAuthorized( "Edit" );
            gDefinedValues.Actions.ShowAdd = canAddEditDelete;
            gDefinedValues.IsDeleteEnabled = canAddEditDelete;

            modalValue.SaveClick += btnSaveValue_Click;
            modalValue.OnCancelScript = string.Format( "$('#{0}').val('');", hfDefinedValueId.ClientID );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                string itemId = PageParameter( "definedTypeId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "definedTypeId", (int)itemId.AsInteger( false ) );
                }
                else
                {
                    pnlList.Visible = false;
                }
            }
            else
            {
                if ( !string.IsNullOrWhiteSpace( hfDefinedValueId.Value ) )
                {
                    ShowAttributeValueEdit( hfDefinedValueId.ValueAsInt(), false );
                }
            }
        }

        #endregion

        #region Events
                
        /// <summary>
        /// Handles the Add event of the gDefinedValues control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gDefinedValues_Add( object sender, EventArgs e )
        {
            gDefinedValues_ShowEdit( 0 );
        }

        /// <summary>
        /// Handles the Edit event of the gDefinedValues control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gDefinedValues_Edit( object sender, RowEventArgs e )
        {            
            gDefinedValues_ShowEdit( (int)e.RowKeyValue );
        }

        /// <summary>
        /// Handles the Delete event of the gDefinedValues control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gDefinedValues_Delete( object sender, RowEventArgs e )
        {
            var valueService = new DefinedValueService();

            DefinedValue value = valueService.Get( (int)e.RowKeyValue );

            DefinedTypeCache.Flush(value.DefinedTypeId);
            DefinedValueCache.Flush(value.Id);

            if ( value != null )
            {
                valueService.Delete( value, CurrentPersonId );
                valueService.Save( value, CurrentPersonId );
            }

            BindDefinedValuesGrid();
        }

        /// <summary>
        /// Handles the Click event of the btnSaveDefinedValue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSaveValue_Click( object sender, EventArgs e )
        {
            DefinedValue definedValue;
            DefinedValueService definedValueService = new DefinedValueService();

            int definedValueId = hfDefinedValueId.ValueAsInt();

            if ( definedValueId.Equals( 0 ) )
            {
                int definedTypeId = hfDefinedTypeId.ValueAsInt();
                definedValue = new DefinedValue { Id = 0 };
                definedValue.DefinedTypeId = definedTypeId;
                definedValue.IsSystem = false;

                var orders = definedValueService.Queryable()
                    .Where( d => d.DefinedTypeId == definedTypeId )
                    .Select( d => d.Order)
                    .ToList();
                
                definedValue.Order = orders.Any() ? orders.Max() + 1 : 0;
            }
            else
            {
                definedValue = definedValueService.Get( definedValueId );
            }

            definedValue.Name = tbValueName.Text;
            definedValue.Description = tbValueDescription.Text;
            definedValue.LoadAttributes();
            Rock.Attribute.Helper.GetEditValues( phDefinedValueAttributes, definedValue );

            if ( !Page.IsValid )
            {
                return;
            }

            if ( !definedValue.IsValid )
            {
                // Controls will render the error messages                    
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                if ( definedValue.Id.Equals( 0 ) )
                {
                    definedValueService.Add( definedValue, CurrentPersonId );
                }

                definedValueService.Save( definedValue, CurrentPersonId );
                Rock.Attribute.Helper.SaveAttributeValues( definedValue, CurrentPersonId );

                Rock.Web.Cache.DefinedTypeCache.Flush( definedValue.DefinedTypeId );
                Rock.Web.Cache.DefinedValueCache.Flush( definedValue.Id );
            } );
                        
            BindDefinedValuesGrid();

            hfDefinedValueId.Value = string.Empty;
            modalValue.Hide();
        }
               
        #endregion

        #region Internal Methods

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            if ( !itemKey.Equals( "definedTypeId" ) )
            {
                return;
            }

            pnlList.Visible = true;
            DefinedType definedType = null;

            if ( !itemKeyValue.Equals( 0 ) )
            {
                definedType = new DefinedTypeService().Get( itemKeyValue );
            }
            else
            {
                definedType = new DefinedType { Id = 0 };
            }

            hfDefinedTypeId.SetValue( definedType.Id );
            BindDefinedValuesGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gDefinedValues control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gDefinedValues_GridRebind( object sender, EventArgs e )
        {
            BindDefinedValuesGrid();
        }

        /// <summary>
        /// Handles the GridReorder event of the gDefinedValues control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridReorderEventArgs"/> instance containing the event data.</param>
        private void gDefinedValues_GridReorder( object sender, GridReorderEventArgs e )
        {
            int definedTypeId = hfDefinedTypeId.ValueAsInt();
            DefinedTypeCache.Flush( definedTypeId );

            using ( new UnitOfWorkScope() )
            {
                var definedValueService = new DefinedValueService();               
                var definedValues = definedValueService.Queryable().Where( a => a.DefinedTypeId == definedTypeId ).OrderBy( a => a.Order );
                definedValueService.Reorder( definedValues.ToList(), e.OldIndex, e.NewIndex, CurrentPersonId );
                BindDefinedValuesGrid();
            }
        }

        /// <summary>
        /// Binds the defined values grid.
        /// </summary>
        protected void BindDefinedValuesGrid()
        {
            AttributeService attributeService = new AttributeService();

            int definedTypeId = hfDefinedTypeId.ValueAsInt();
            
            // add attributes with IsGridColumn to grid
            string qualifierValue = hfDefinedTypeId.Value;
            var qryDefinedTypeAttributes = attributeService.GetByEntityTypeId( new DefinedValue().TypeId ).AsQueryable()
                .Where( a => a.EntityTypeQualifierColumn.Equals( "DefinedTypeId", StringComparison.OrdinalIgnoreCase )
                && a.EntityTypeQualifierValue.Equals( qualifierValue ) );

            qryDefinedTypeAttributes = qryDefinedTypeAttributes.Where( a => a.IsGridColumn );

            List<Attribute> gridItems = qryDefinedTypeAttributes.ToList();

            foreach ( var item in gDefinedValues.Columns.OfType<AttributeField>().ToList() )
            {
                gDefinedValues.Columns.Remove( item );
            }

            foreach ( var item in gridItems.OrderBy( a => a.Order ).ThenBy( a => a.Name ) )
            {
                string dataFieldExpression = item.Key;
                bool columnExists = gDefinedValues.Columns.OfType<AttributeField>().FirstOrDefault( a => a.DataField.Equals( dataFieldExpression ) ) != null;
                if ( !columnExists )
                {
                    AttributeField boundField = new AttributeField();
                    boundField.DataField = dataFieldExpression;
                    boundField.HeaderText = item.Name;
                    boundField.SortExpression = string.Empty;
                    int insertPos = gDefinedValues.Columns.IndexOf( gDefinedValues.Columns.OfType<DeleteField>().First());
                    gDefinedValues.Columns.Insert(insertPos, boundField );
                }
            }

            var queryable = new DefinedValueService().Queryable().Where( a => a.DefinedTypeId == definedTypeId ).OrderBy( a => a.Order );
            var result = queryable.ToList();

            gDefinedValues.DataSource = result;
            gDefinedValues.DataBind();
        }

        /// <summary>
        /// Shows the edit value.
        /// </summary>
        /// <param name="valueId">The value id.</param>
        protected void gDefinedValues_ShowEdit( int valueId )
        {
            ShowAttributeValueEdit( valueId, true );
        }

        private void ShowAttributeValueEdit(int valueId, bool setValues)
        {
            var definedType = DefinedTypeCache.Read( hfDefinedTypeId.ValueAsInt() );
            DefinedValue definedValue;
            if ( !valueId.Equals( 0 ) )
            {
                definedValue = new DefinedValueService().Get( valueId );
                if ( definedType != null )
                {
                    lActionTitleDefinedValue.Text = ActionTitle.Edit( "defined value for " + definedType.Name );
                }
            }
            else
            {
                definedValue = new DefinedValue { Id = 0 };
                definedValue.DefinedTypeId = hfDefinedTypeId.ValueAsInt();
                if ( definedType != null )
                {
                    lActionTitleDefinedValue.Text = ActionTitle.Add( "defined value for " + definedType.Name );
                }
            }

            if ( setValues )
            {
                hfDefinedValueId.SetValue( definedValue.Id );
                tbValueName.Text = definedValue.Name;
                tbValueDescription.Text = definedValue.Description;
            }

            definedValue.LoadAttributes();
            phDefinedValueAttributes.Controls.Clear();
            Rock.Attribute.Helper.AddEditControls( definedValue, phDefinedValueAttributes, setValues );

            SetValidationGroup( phDefinedValueAttributes.Controls, modalValue.ValidationGroup );

            modalValue.Show();
        }

        #endregion

        #region ISecondaryBlock

        /// <summary>
        /// Sets the visible.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        public void SetVisible( bool visible )
        {
            pnlContent.Visible = visible;
        }

        #endregion
    }
}