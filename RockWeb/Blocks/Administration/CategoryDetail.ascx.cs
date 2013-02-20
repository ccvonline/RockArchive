//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI;

namespace RockWeb.Blocks.Administration
{
    [TextField( "EntityTypeName", "Category Entity Type", false )]
    public partial class CategoryDetail : RockBlock, IDetailBlock
    {
        #region Control Methods

        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
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
                string itemId = PageParameter( "categoryId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "categoryId", int.Parse( itemId ) );
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
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
            if ( hfCategoryId.Value.Equals( "0" ) )
            {
                // Cancelling on Add.  Return to Grid
                NavigateToParentPage();
            }
            else
            {
                // Cancelling on Edit.  Return to Details
                CategoryService service = new CategoryService();
                Category category = service.Get( hfCategoryId.ValueAsInt() );
                ShowReadonlyDetails( category );
            }
        }

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnEdit_Click( object sender, EventArgs e )
        {
            CategoryService service = new CategoryService();
            Category category = service.Get( hfCategoryId.ValueAsInt() );
            ShowEditDetails( category );
        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;
            fieldsetViewDetails.Visible = !editable;
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            Category category;
            CategoryService categoryService = new CategoryService();

            int categoryId = hfCategoryId.ValueAsInt();

            if ( categoryId == 0 )
            {
                category = new Category();
                category.IsSystem = false;
                category.Name = string.Empty;
                string entityTypeName = GetAttributeValue( "EntityTypeName" );
                if ( !string.IsNullOrWhiteSpace( entityTypeName ) )
                {
                    category.EntityTypeId = EntityTypeCache.GetId( entityTypeName ).Value;
                }
            }
            else
            {
                category = categoryService.Get( categoryId );
            }

            category.Name = tbName.Text;
            category.ParentCategoryId = ddlParentCategory.SelectedValueAsInt();
            category.IconCssClass = tbIconCssClass.Text;
            category.IconSmallFileId = imgIconSmall.ImageId;
            category.IconLargeFileId = imgIconLarge.ImageId;

            // check for duplicates within EntityTypeId, ParentId
            if ( categoryService.Queryable().Where( g => g.EntityTypeId == category.EntityTypeId && g.ParentCategoryId == category.ParentCategoryId ).Count( a => a.Name.Equals( category.Name, StringComparison.OrdinalIgnoreCase ) && !a.Id.Equals( category.Id ) ) > 0 )
            {
                tbName.ShowErrorMessage( WarningMessage.DuplicateFoundMessage( "name", Category.FriendlyTypeName ) );
                return;
            }

            if ( !Page.IsValid )
            {
                return;
            }

            if ( !category.IsValid )
            {
                // Controls will render the error messages                    
                return;
            }

            RockTransactionScope.WrapTransaction( () =>
            {
                if ( category.Id.Equals( 0 ) )
                {
                    categoryService.Add( category, CurrentPersonId );
                }

                categoryService.Save( category, CurrentPersonId );
            } );

            // reload group from db using a new context
            category = new CategoryService().Get( category.Id );
            ShowReadonlyDetails( category );
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            CategoryService service = new CategoryService();
            var qry = service.Queryable();
            List<Category> cats = qry.OrderBy( a => a.Name ).ToList();
            ddlParentCategory.Items.Clear();
            ddlParentCategory.Items.Add(None.ListItem);
            foreach ( var item in cats )
            {
                ddlParentCategory.Items.Add(new ListItem(item.Name, item.Id.ToString()));
            }
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            if ( !itemKey.Equals( "categoryId" ) )
            {
                pnlDetails.Visible = false;
                return;
            }

            pnlDetails.Visible = true;
            Category category = null;

            if ( !itemKeyValue.Equals( 0 ) )
            {
                category = new CategoryService().Get( itemKeyValue );
            }
            else
            {
                category = new Category { Id = 0, IsSystem = false};
            }

            hfCategoryId.Value = category.Id.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Category.FriendlyTypeName );
            }

            if ( category.IsSystem )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlySystem( Category.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                ShowReadonlyDetails( category );
            }
            else
            {
                btnEdit.Visible = true;
                if ( category.Id > 0 )
                {
                    ShowReadonlyDetails( category );
                }
                else
                {
                    ShowEditDetails( category );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="category">The category.</param>
        private void ShowEditDetails( Category category )
        {
            if ( category.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( Category.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( Category.FriendlyTypeName );
            }

            SetEditMode( true );
            LoadDropDowns();
            
            tbName.Text = category.Name;
            ddlParentCategory.SetValue( category.ParentCategoryId );
            lblEntityTypeName.Text = category.EntityType.Name;
            lblEntityTypeQualifierColumn.Visible = !string.IsNullOrWhiteSpace( category.EntityTypeQualifierColumn );
            lblEntityTypeQualifierColumn.Text = category.EntityTypeQualifierColumn;
            lblEntityTypeQualifierValue.Visible = !string.IsNullOrWhiteSpace( category.EntityTypeQualifierValue );
            lblEntityTypeQualifierValue.Text = category.EntityTypeQualifierValue;
            tbIconCssClass.Text = category.IconCssClass;
            imgIconSmall.ImageId = category.IconSmallFileId;
            imgIconLarge.ImageId = category.IconLargeFileId;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="category">The category.</param>
        private void ShowReadonlyDetails( Category category )
        {
            SetEditMode( false );

            string categoryIconHtml = string.Empty;
            if ( !string.IsNullOrWhiteSpace( category.IconCssClass ) )
            {
                categoryIconHtml = string.Format( "<i class='{0} icon-large' ></i>", category.IconCssClass );
            }
            else
            {
                var appPath = System.Web.VirtualPathUtility.ToAbsolute( "~" );
                string imageUrlFormat = "<img src='" + appPath + "Image.ashx?id={0}&width=50&height=50' />";
                if ( category.IconLargeFileId != null )
                {
                    categoryIconHtml = string.Format( imageUrlFormat, category.IconLargeFileId );
                }
                else if ( category.IconSmallFileId != null )
                {
                    categoryIconHtml = string.Format( imageUrlFormat, category.IconSmallFileId );
                }
            }

            hfCategoryId.SetValue( category.Id );
            lCategoryIconHtml.Text = categoryIconHtml;
            lReadOnlyTitle.Text = category.Name;

            string descriptionFormat = "<dt>{0}</dt><dd>{1}</dd>";
            lblMainDetails.Text = @"
<div>
    <dl>";

            lblMainDetails.Text += string.Format( descriptionFormat, "Entity Type", category.EntityType.Name );
            lblMainDetails.Text += @"
    </dl>
</div>
";
        }

        #endregion
    }
}