﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Core
{
    /// <summary>
    /// Block for managing categories for an specific entity type.
    /// </summary>
    [DisplayName( "Categories" )]
    [Category( "Core" )]
    [Description( "Block for managing categories for a specific, configured entity type." )]

    [EntityTypeField("Entity Type", "The entity type to manage categories for.")]
    public partial class Categories : RockBlock
    {
        #region Fields

        int _entityTypeId = 0;
        int? _parentCategoryId = null;
        bool _canConfigure = false;

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            _canConfigure = RockPage.IsAuthorized( "Administrate", CurrentPerson );
            if ( _canConfigure )
            {
                Guid entityTypeGuid = Guid.Empty;
                if ( Guid.TryParse( GetAttributeValue( "EntityType" ), out entityTypeGuid ) )
                {
                    _entityTypeId = Rock.Web.Cache.EntityTypeCache.Read( entityTypeGuid ).Id;
                    catpParentCategory.EntityTypeId = _entityTypeId;

                    int parentCategoryId = int.MinValue;
                    if (int.TryParse(PageParameter("CategoryId"), out parentCategoryId))
                    {
                        _parentCategoryId = parentCategoryId;
                    }

                    gCategories.DataKeyNames = new string[] { "id" };
                    gCategories.Actions.ShowAdd = true;

                    gCategories.Actions.AddClick += gCategories_Add;
                    gCategories.GridReorder += gCategories_GridReorder;
                    gCategories.GridRebind += gCategories_GridRebind;

                    mdDetails.SaveClick += mdDetails_SaveClick;
                    mdDetails.OnCancelScript = string.Format( "$('#{0}').val('');", hfIdValue.ClientID );
                }
                else
                {
                    pnlList.Visible = false;
                    nbMessage.Text = "Block has not been configured for a valid Enity Type.";
                    nbMessage.Visible = true;
                }
            }
            else
            {
                pnlList.Visible = false;
                nbMessage.Text = "You are not authorized to configure this page.";
                nbMessage.Visible = true;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                if ( _canConfigure )
                {
                    BindGrid();
                }
            }
            else
            {
                if ( !string.IsNullOrWhiteSpace( hfIdValue.Value ) )
                {
                    mdDetails.Show();
                }
            }


            base.OnLoad( e );
        }

        public override List<BreadCrumb> GetBreadCrumbs( PageReference pageReference )
        {
            Guid entityTypeGuid = Guid.Empty;
            Guid.TryParse( GetAttributeValue( "EntityType" ), out entityTypeGuid );
            var entityType = EntityTypeCache.Read( entityTypeGuid );
            if ( entityType == null )
            {
                return base.GetBreadCrumbs( pageReference );
            }

            var breadCrumbs = new List<BreadCrumb>();


            int parentCategoryId = int.MinValue;
            if ( int.TryParse( PageParameter( "CategoryId" ), out parentCategoryId ) )
            {
                var category = CategoryCache.Read( parentCategoryId );
                while ( category != null )
                {
                    var parms = new Dictionary<string, string>();
                    parms.Add( "CategoryId", category.Id.ToString() );
                    breadCrumbs.Add( new BreadCrumb( category.Name, new PageReference( pageReference.PageId, 0, parms ) ) );

                    category = category.ParentCategory;
                }
            }

            breadCrumbs.Add( new BreadCrumb( entityType.FriendlyName + " Categories", new PageReference( pageReference.PageId ) ) );

            breadCrumbs.Reverse();

            return breadCrumbs;
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Edit event of the gCategories control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gCategories_Select( object sender, RowEventArgs e )
        {
            var parms = new Dictionary<string, string>();
            parms.Add( "CategoryId", gCategories.DataKeys[e.RowIndex]["id"].ToString() );
            Response.Redirect( new PageReference( CurrentPageReference.PageId, 0, parms ).BuildUrl(), false );
        }

        protected void gCategories_Edit( object sender, RowEventArgs e )
        {
            ShowEdit( (int)gCategories.DataKeys[e.RowIndex]["id"] );
        }
        
        /// <summary>
        /// Handles the Delete event of the gCategories control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gCategories_Delete( object sender, RowEventArgs e )
        {
            var service = new CategoryService();

            var category = service.Get( (int)gCategories.DataKeys[e.RowIndex]["id"] );
            if ( category != null )
            {
                string errorMessage = string.Empty;
                if ( service.CanDelete( category, out errorMessage ) )
                {

                    service.Delete( category, CurrentPersonId );
                    service.Save( category, CurrentPersonId );
                }
                else
                {
                    nbMessage.Text = errorMessage;
                    nbMessage.Visible = true;
                }
            }

            BindGrid();
        }

        /// <summary>
        /// Handles the Add event of the gCategories control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gCategories_Add( object sender, EventArgs e )
        {
            ShowEdit( 0 );
        }

        /// <summary>
        /// Handles the GridRebind event of the gCategories control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gCategories_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        void gCategories_GridReorder( object sender, GridReorderEventArgs e )
        {
            using ( new UnitOfWorkScope() )
            {
                var categories = GetCategories();
                if ( categories != null )
                {
                    new CategoryService().Reorder( categories.ToList(), e.OldIndex, e.NewIndex, CurrentPersonId );
                }

                BindGrid();
            }
        }
        
        /// <summary>
        /// Handles the SaveClick event of the modalDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void mdDetails_SaveClick( object sender, EventArgs e )
        {
            int categoryId = 0;
            if ( hfIdValue.Value != string.Empty && !int.TryParse( hfIdValue.Value, out categoryId ) )
            {
                categoryId = 0;
            }

            var service = new CategoryService();
            Category category = null;

            if ( categoryId != 0 )
            {
                category = service.Get( categoryId );
            }

            if ( category == null )
            {
                category = new Category();
                category.EntityTypeId = _entityTypeId;
                var lastCategory = GetUnorderedCategories()
                    .OrderByDescending( c => c.Order ).FirstOrDefault();
                category.Order = lastCategory != null ? lastCategory.Order + 1 : 0;

                service.Add( category, CurrentPersonId );
            }

            category.Name = tbName.Text;
            category.Description = tbDescription.Text;
            category.ParentCategoryId = catpParentCategory.SelectedValueAsInt();
            category.IconCssClass = tbIconCssClass.Text;

            List<int> orphanedBinaryFileIdList = new List<int>();
           
            if ( category.IsValid )
            {
                RockTransactionScope.WrapTransaction( () =>
                {
                    service.Save( category, CurrentPersonId );

                    BinaryFileService binaryFileService = new BinaryFileService();
                    foreach ( int binaryFileId in orphanedBinaryFileIdList )
                    {
                        var binaryFile = binaryFileService.Get( binaryFileId );
                        if ( binaryFile != null )
                        {
                            // marked the old images as IsTemporary so they will get cleaned up later
                            binaryFile.IsTemporary = true;
                            binaryFileService.Save( binaryFile, CurrentPersonId );
                        }
                    }

                } );

                hfIdValue.Value = string.Empty;
                mdDetails.Hide();

                BindGrid();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            gCategories.DataSource = GetCategories()
                .Select( c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IconCssClass = c.IconCssClass,
                    ChildCount = c.ChildCategories.Count()
                } ).ToList();

            gCategories.DataBind();
        }

        private IQueryable<Category> GetCategories()
        {
            return GetUnorderedCategories()
                .OrderBy( a => a.Order )
                .ThenBy( a => a.Name );
        }

        private IQueryable<Category> GetUnorderedCategories()
        {
            var queryable = new CategoryService().Queryable().Where( c => c.EntityTypeId == _entityTypeId );
            if (_parentCategoryId.HasValue)
            {
                queryable = queryable.Where( c => c.ParentCategoryId == _parentCategoryId );
            }
            else
            {
                queryable = queryable.Where( c => c.ParentCategoryId == null );
            }

            return queryable;
        }


        /// <summary>
        /// Shows the edit.
        /// </summary>
        /// <param name="attributeId">The attribute id.</param>
        protected void ShowEdit( int categoryId )
        {
            Category category = null;
            if ( categoryId > 0 )
            {
                category = new CategoryService().Get( categoryId );
            }

            if ( category != null )
            {
                tbName.Text = category.Name;
                tbDescription.Text = category.Description;
                catpParentCategory.SetValue( category.ParentCategoryId );
                tbIconCssClass.Text = category.IconCssClass;
            }
            else
            {
                tbName.Text = string.Empty;
                tbDescription.Text = string.Empty;
                catpParentCategory.SetValue( _parentCategoryId );
                tbIconCssClass.Text = string.Empty;
            }

            hfIdValue.Value = categoryId.ToString();
            mdDetails.Show();
        }

        #endregion

    }
}