﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Rock;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using Attribute = Rock.Model.Attribute;
using System.ComponentModel;

namespace RockWeb.Blocks.Cms
{
    /// <summary>
    /// 
    /// </summary>
    [DisplayName("Marketing Campaign - Ad Type Detail")]
    [Category("CMS")]
    [Description("Displays the details for an ad type.")]
    public partial class MarketingCampaignAdTypeDetail : RockBlock, IDetailBlock
    {
        #region Child Grid States

        /// <summary>
        /// Gets or sets the state of the attributes.
        /// </summary>
        /// <value>
        /// The state of the attributes.
        /// </value>
        private ViewStateList<Attribute> AttributesState
        {
            get
            {
                return ViewState["AttributesState"] as ViewStateList<Attribute>;
            }

            set
            {
                ViewState["AttributesState"] = value;
            }
        }

        #endregion

        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            gMarketingCampaignAdAttributeTypes.DataKeyNames = new string[] { "Guid" };
            gMarketingCampaignAdAttributeTypes.Actions.ShowAdd = true;
            gMarketingCampaignAdAttributeTypes.Actions.AddClick += gMarketingCampaignAdAttributeType_Add;
            gMarketingCampaignAdAttributeTypes.GridRebind += gMarketingCampaignAdAttributeType_GridRebind;
            gMarketingCampaignAdAttributeTypes.EmptyDataText = Server.HtmlEncode( None.Text );
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
                string itemId = PageParameter( "marketingCampaignAdTypeId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "marketingCampaignAdTypeId", int.Parse( itemId ) );
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
        }

        #endregion

        #region AttributeTypes Grid and Picker

        /// <summary>
        /// Handles the Add event of the gMarketingCampaignAdAttributeType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gMarketingCampaignAdAttributeType_Add( object sender, EventArgs e )
        {
            gMarketingCampaignAdAttributeType_ShowEdit( Guid.Empty );
        }

        /// <summary>
        /// Handles the Edit event of the gMarketingCampaignAdAttributeType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gMarketingCampaignAdAttributeType_Edit( object sender, RowEventArgs e )
        {
            Guid attributeGuid = (Guid)e.RowKeyValue;
            gMarketingCampaignAdAttributeType_ShowEdit( attributeGuid );
        }

        /// <summary>
        /// Gs the marketing campaign ad attribute type_ show edit.
        /// </summary>
        /// <param name="attributeId">The attribute id.</param>
        protected void gMarketingCampaignAdAttributeType_ShowEdit( Guid attributeGuid )
        {
            pnlDetails.Visible = false;
            pnlAdTypeAttribute.Visible = true;

            Attribute attribute;
            if ( attributeGuid.Equals( Guid.Empty ) )
            {
                attribute = new Attribute();
                attribute.FieldTypeId = FieldTypeCache.Read( Rock.SystemGuid.FieldType.TEXT ).Id;
                edtAdTypeAttributes.ActionTitle = ActionTitle.Add( "attribute for ad type " + tbName.Text );

            }
            else
            {
                attribute = AttributesState.First( a => a.Guid.Equals( attributeGuid ) );
                edtAdTypeAttributes.ActionTitle = ActionTitle.Edit( "attribute for ad type " + tbName.Text );
            }

            edtAdTypeAttributes.SetAttributeProperties( attribute, typeof( MarketingCampaignAd ) );
        }

        /// <summary>
        /// Handles the Delete event of the gMarketingCampaignAdAttributeType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected void gMarketingCampaignAdAttributeType_Delete( object sender, RowEventArgs e )
        {
            Guid attributeGuid = (Guid)e.RowKeyValue;
            AttributesState.RemoveEntity( attributeGuid );

            BindMarketingCampaignAdAttributeTypeGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gMarketingCampaignAdAttributeType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gMarketingCampaignAdAttributeType_GridRebind( object sender, EventArgs e )
        {
            BindMarketingCampaignAdAttributeTypeGrid();
        }

        /// <summary>
        /// Handles the Click event of the btnSaveAttribute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSaveAttribute_Click( object sender, EventArgs e )
        {
            Rock.Model.Attribute attribute = new Rock.Model.Attribute();
            edtAdTypeAttributes.GetAttributeProperties( attribute );

            // Controls will show warnings
            if ( !attribute.IsValid )
            {
                return;
            }

            AttributesState.RemoveEntity( attribute.Guid );
            AttributesState.Add( attribute );

            pnlDetails.Visible = true;
            pnlAdTypeAttribute.Visible = false;

            BindMarketingCampaignAdAttributeTypeGrid();
        }

        /// <summary>
        /// Handles the Click event of the btnCancelAttribute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancelAttribute_Click( object sender, EventArgs e )
        {
            pnlDetails.Visible = true;
            pnlAdTypeAttribute.Visible = false;
        }

        /// <summary>
        /// Binds the marketing campaign ad attribute type grid.
        /// </summary>
        private void BindMarketingCampaignAdAttributeTypeGrid()
        {
            gMarketingCampaignAdAttributeTypes.DataSource = AttributesState.OrderBy( a => a.Name ).ToList();
            gMarketingCampaignAdAttributeTypes.DataBind();
        }

        #endregion

        #region Edit Events

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            NavigateToParentPage();
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            using ( new UnitOfWorkScope() )
            {
                MarketingCampaignAdType marketingCampaignAdType;
                MarketingCampaignAdTypeService marketingCampaignAdTypeService = new MarketingCampaignAdTypeService();

                int marketingCampaignAdTypeId = int.Parse( hfMarketingCampaignAdTypeId.Value );

                if ( marketingCampaignAdTypeId == 0 )
                {
                    marketingCampaignAdType = new MarketingCampaignAdType();
                    marketingCampaignAdTypeService.Add( marketingCampaignAdType, CurrentPersonId );
                }
                else
                {
                    marketingCampaignAdType = marketingCampaignAdTypeService.Get( marketingCampaignAdTypeId );
                }

                marketingCampaignAdType.Name = tbName.Text;
                marketingCampaignAdType.DateRangeType = (DateRangeTypeEnum)int.Parse( ddlDateRangeType.SelectedValue );

                if ( !marketingCampaignAdType.IsValid )
                {
                    // Controls will render the error messages                    
                    return;
                }

                RockTransactionScope.WrapTransaction( () =>
                {
                    AttributeService attributeService = new AttributeService();
                    AttributeQualifierService attributeQualifierService = new AttributeQualifierService();
                    CategoryService categoryService = new CategoryService();

                    marketingCampaignAdTypeService.Save( marketingCampaignAdType, CurrentPersonId );

                    // get it back to make sure we have a good Id for it for the Attributes
                    marketingCampaignAdType = marketingCampaignAdTypeService.Get( marketingCampaignAdType.Guid );

                    var entityTypeId = EntityTypeCache.Read(typeof(MarketingCampaignAd)).Id;
                    string qualifierColumn = "MarketingCampaignAdTypeId";
                    string qualifierValue = marketingCampaignAdType.Id.ToString();

                    // Get the existing attributes for this entity type and qualifier value
                    var attributes = attributeService.Get( entityTypeId, qualifierColumn, qualifierValue );

                    // Delete any of those attributes that were removed in the UI
                    var selectedAttributeGuids = AttributesState.Select( a => a.Guid );
                    foreach ( var attr in attributes.Where( a => !selectedAttributeGuids.Contains( a.Guid ) ) )
                    {
                        Rock.Web.Cache.AttributeCache.Flush( attr.Id );

                        attributeService.Delete( attr, CurrentPersonId );
                        attributeService.Save( attr, CurrentPersonId );
                    }

                    // Update the Attributes that were assigned in the UI
                    foreach ( var attributeState in AttributesState )
                    {
                        Rock.Attribute.Helper.SaveAttributeEdits( attributeState, attributeService, attributeQualifierService, categoryService,
                            entityTypeId, qualifierColumn, qualifierValue, CurrentPersonId );
                    }
                } );
            }

            NavigateToParentPage();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            ddlDateRangeType.BindToEnum( typeof( DateRangeTypeEnum ) );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            if ( !itemKey.Equals( "marketingCampaignAdTypeId" ) )
            {
                return;
            }

            pnlDetails.Visible = true;
            MarketingCampaignAdType marketingCampaignAdType = null;

            if ( !itemKeyValue.Equals( 0 ) )
            {
                marketingCampaignAdType = new MarketingCampaignAdTypeService().Get( itemKeyValue );
                lActionTitle.Text = ActionTitle.Edit( MarketingCampaignAdType.FriendlyTypeName ).FormatAsHtmlTitle();
            }
            else
            {
                marketingCampaignAdType = new MarketingCampaignAdType { Id = 0 };
                lActionTitle.Text = ActionTitle.Add( MarketingCampaignAdType.FriendlyTypeName ).FormatAsHtmlTitle();
            }

            LoadDropDowns();

            // load data into UI controls
            AttributesState = new ViewStateList<Attribute>();

            hfMarketingCampaignAdTypeId.Value = marketingCampaignAdType.Id.ToString();
            tbName.Text = marketingCampaignAdType.Name;
            ddlDateRangeType.SetValue( (int)marketingCampaignAdType.DateRangeType );

            AttributeService attributeService = new AttributeService();

            string qualifierValue = marketingCampaignAdType.Id.ToString();
            var qry = attributeService.GetByEntityTypeId( new MarketingCampaignAd().TypeId ).AsQueryable()
                .Where( a => a.EntityTypeQualifierColumn.Equals( "MarketingCampaignAdTypeId", StringComparison.OrdinalIgnoreCase )
                && a.EntityTypeQualifierValue.Equals( qualifierValue ) );

            AttributesState.AddAll( qry.ToList() );
            BindMarketingCampaignAdAttributeTypeGrid();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( MarketingCampaignAdType.FriendlyTypeName );
            }

            if ( marketingCampaignAdType.IsSystem )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlySystem( MarketingCampaignAdType.FriendlyTypeName );
            }

            if ( readOnly )
            {
                lActionTitle.Text = ActionTitle.View( MarketingCampaignAdType.FriendlyTypeName );
                btnCancel.Text = "Close";
            }

            tbName.ReadOnly = readOnly;
            ddlDateRangeType.Enabled = !readOnly;
            gMarketingCampaignAdAttributeTypes.Enabled = !readOnly;

            btnSave.Visible = !readOnly;
        }

        #endregion

    }
}