﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Services.NuGet;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PageProperties : RockBlock
    {
        #region Fields

        private PageCache _page;
        private readonly List<string> _tabs = new List<string> { "Basic Settings", "Display Settings", "Advanced Settings", "Import/Export"} ;

        /// <summary>
        /// Gets or sets the current tab.
        /// </summary>
        /// <value>
        /// The current tab.
        /// </value>
        protected string CurrentTab
        {
            get
            {
                object currentProperty = ViewState["CurrentTab"];
                return currentProperty != null ? currentProperty.ToString() : "Basic Settings";
            }

            set
            {
                ViewState["CurrentTab"] = value;
            }
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            try
            {
                int pageId = int.MinValue;
                if ( int.TryParse( PageParameter( "Page" ), out pageId ) )
                {
                    _page = Rock.Web.Cache.PageCache.Read( pageId );

                    DialogMasterPage masterPage = this.Page.Master as DialogMasterPage;
                    if ( masterPage != null )
                    {
                        masterPage.OnSave += new EventHandler<EventArgs>( masterPage_OnSave );
                        masterPage.SubTitle = string.Format( "Id: {0}", _page.Id );
                    }

                    if ( _page.IsAuthorized( "Administrate", CurrentPerson ) )
                    {
                        ddlMenuWhen.BindToEnum( typeof( DisplayInNavWhen ) );

                        phAttributes.Controls.Clear();
                        Rock.Attribute.Helper.AddEditControls( _page, phAttributes, !Page.IsPostBack );

                        var blockContexts = new List<EntityTypeCache>();
                        foreach ( var block in _page.Blocks )
                        {
                            var blockControl = TemplateControl.LoadControl( block.BlockType.Path ) as RockBlock;
                            if ( blockControl != null )
                            {
                                blockControl.SetBlock( block );
                                foreach ( var context in blockControl.ContextTypesRequired )
                                {
                                    if ( !blockContexts.Contains( context ) )
                                    {
                                        blockContexts.Add( context );
                                    }
                                }
                            }
                        }

                        phContextPanel.Visible = blockContexts.Count > 0;

                        int i = 0;
                        foreach ( EntityTypeCache context in blockContexts )
                        {
                            var tbContext = new RockTextBox();
                            tbContext.ID = string.Format( "context_{0}", i++ );
                            tbContext.Required = true;
                            tbContext.Label = context.FriendlyName + " Parameter Name";
                            tbContext.Help = "The page parameter name that contains the id of this context entity.";
                            if ( _page.PageContexts.ContainsKey( context.Name ) )
                            {
                                tbContext.Text = _page.PageContexts[context.Name];
                            }

                            phContext.Controls.Add( tbContext );
                        }
                    }
                    else
                    {
                        DisplayError( "You are not authorized to edit this page" );
                    }
                }
                else
                {
                    DisplayError( "Invalid Page Id value" );
                }
            }
            catch ( SystemException ex )
            {
                DisplayError( ex.Message );
            }

            base.OnInit( e );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            if ( _page == null )
            {
                int pageId = Convert.ToInt32( PageParameter( "Page" ) );
                _page = Rock.Web.Cache.PageCache.Read( pageId );
            }

            if ( !Page.IsPostBack && _page.IsAuthorized( "Administrate", CurrentPerson ) )
            {
                PageService pageService = new PageService();
                Rock.Model.Page page = pageService.Get( _page.Id );

                LoadSites();
                if ( _page.Layout != null )
                {
                    ddlSite.SelectedValue = _page.Layout.SiteId.ToString();
                    LoadLayouts( _page.Layout.Site );
                    ddlLayout.SelectedValue = _page.Layout.Id.ToString();
                }

                rptProperties.DataSource = _tabs;
                rptProperties.DataBind();

                tbPageName.Text = _page.Name;
                tbPageTitle.Text = _page.Title;
                ppParentPage.SetValue( pageService.Get( page.ParentPageId ?? 0 ) );
                imgIcon.BinaryFileId = page.IconFileId;
                tbIconCssClass.Text = _page.IconCssClass;

                cbPageTitle.Checked = _page.PageDisplayTitle;
                cbPageBreadCrumb.Checked = _page.PageDisplayBreadCrumb;
                cbPageIcon.Checked = _page.PageDisplayIcon;
                cbPageDescription.Checked = _page.PageDisplayDescription;

                ddlMenuWhen.SelectedValue = ( (int)_page.DisplayInNavWhen ).ToString();
                cbMenuDescription.Checked = _page.MenuDisplayDescription;
                cbMenuIcon.Checked = _page.MenuDisplayIcon;
                cbMenuChildPages.Checked = _page.MenuDisplayChildPages;

                cbBreadCrumbIcon.Checked = _page.BreadCrumbDisplayIcon;
                cbBreadCrumbName.Checked = _page.BreadCrumbDisplayName;

                cbRequiresEncryption.Checked = _page.RequiresEncryption;
                cbEnableViewState.Checked = _page.EnableViewState;
                cbIncludeAdminFooter.Checked = _page.IncludeAdminFooter;
                tbCacheDuration.Text = _page.OutputCacheDuration.ToString();
                tbDescription.Text = _page.Description;
                tbPageRoute.Text = string.Join( ",", page.PageRoutes.Select( route => route.Route ).ToArray() );

                // Add enctype attribute to page's <form> tag to allow file upload control to function
                Page.Form.Attributes.Add( "enctype", "multipart/form-data" );
            }

            base.OnLoad( e );

        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the lbProperty control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbProperty_Click( object sender, EventArgs e )
        {
            LinkButton lb = sender as LinkButton;
            if ( lb != null )
            {
                CurrentTab = lb.Text;

                rptProperties.DataSource = _tabs;
                rptProperties.DataBind();
            }

            ShowSelectedPane();
        }

        protected void ddlSite_SelectedIndexChanged( object sender, EventArgs e )
        {
            LoadLayouts( SiteCache.Read( ddlSite.SelectedValueAsInt().Value ) );
        }
        
        /// <summary>
        /// Handles the OnSave event of the masterPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void masterPage_OnSave( object sender, EventArgs e )
        {
            Page.Validate( BlockValidationGroup );
            if ( Page.IsValid )
            {
                using ( new UnitOfWorkScope() )
                {
                    var pageService = new PageService();
                    var routeService = new PageRouteService();
                    var contextService = new PageContextService();

                    var page = pageService.Get( _page.Id );

                    int parentPageId = ppParentPage.SelectedValueAsInt() ?? 0;
                    if ( page.ParentPageId != parentPageId )
                    {
                        if ( page.ParentPageId.HasValue )
                        {
                            PageCache.Flush( page.ParentPageId.Value );
                        }

                        if ( parentPageId != 0 )
                        {
                            PageCache.Flush( parentPageId );
                        }
                    }

                    page.Name = tbPageName.Text;
                    page.Title = tbPageTitle.Text;
                    if ( parentPageId != 0 )
                    {
                        page.ParentPageId = parentPageId;
                    }
                    else
                    {
                        page.ParentPageId = null;
                    }

                    page.LayoutId = ddlLayout.SelectedValueAsInt().Value;

                    int? orphanedIconFileId = null;

                    if ( page.IconFileId != imgIcon.BinaryFileId )
                    {
                        orphanedIconFileId = page.IconFileId;
                        page.IconFileId = imgIcon.BinaryFileId;
                    }
                    page.IconCssClass = tbIconCssClass.Text;

                    page.PageDisplayTitle = cbPageTitle.Checked;
                    page.PageDisplayBreadCrumb = cbPageBreadCrumb.Checked;
                    page.PageDisplayIcon = cbPageIcon.Checked;
                    page.PageDisplayDescription = cbPageDescription.Checked;

                    page.DisplayInNavWhen = (DisplayInNavWhen)Enum.Parse( typeof( DisplayInNavWhen ), ddlMenuWhen.SelectedValue );
                    page.MenuDisplayDescription = cbMenuDescription.Checked;
                    page.MenuDisplayIcon = cbMenuIcon.Checked;
                    page.MenuDisplayChildPages = cbMenuChildPages.Checked;

                    page.BreadCrumbDisplayName = cbBreadCrumbName.Checked;
                    page.BreadCrumbDisplayIcon = cbBreadCrumbIcon.Checked;

                    page.RequiresEncryption = cbRequiresEncryption.Checked;
                    page.EnableViewState = cbEnableViewState.Checked;
                    page.IncludeAdminFooter = cbIncludeAdminFooter.Checked;
                    page.OutputCacheDuration = int.Parse( tbCacheDuration.Text );
                    page.Description = tbDescription.Text;

                    // new or updated route
                    foreach ( var pageRoute in page.PageRoutes.ToList() )
                    {
                        var existingRoute = RouteTable.Routes.OfType<Route>().FirstOrDefault( a => a.RouteId() == pageRoute.Id );
                        if ( existingRoute != null )
                        {
                            RouteTable.Routes.Remove( existingRoute );
                        }

                        routeService.Delete( pageRoute, CurrentPersonId );
                    }

                    page.PageRoutes.Clear();

                    foreach ( string route in tbPageRoute.Text.SplitDelimitedValues() )
                    {
                        var pageRoute = new PageRoute();
                        pageRoute.Route = route;
                        pageRoute.Guid = Guid.NewGuid();
                        page.PageRoutes.Add( pageRoute );
                    }

                    foreach ( var pageContext in page.PageContexts.ToList() )
                    {
                        contextService.Delete( pageContext, CurrentPersonId );
                    }

                    page.PageContexts.Clear();
                    foreach ( var control in phContext.Controls )
                    {
                        if ( control is RockTextBox )
                        {
                            var tbContext = control as RockTextBox;
                            if ( !string.IsNullOrWhiteSpace( tbContext.Text ) )
                            {
                                var pageContext = new PageContext();
                                pageContext.Entity = tbContext.Label;
                                pageContext.IdParameter = tbContext.Text;
                                page.PageContexts.Add( pageContext );
                            }
                        }
                    }

                    if ( page.IsValid )
                    {
                        pageService.Save( page, CurrentPersonId );

                        foreach ( var pageRoute in new PageRouteService().GetByPageId( page.Id ) )
                        {
                            RouteTable.Routes.AddPageRoute( pageRoute );
                        }

                        Rock.Attribute.Helper.GetEditValues( phAttributes, _page );
                        _page.SaveAttributeValues( CurrentPersonId );

                        if ( orphanedIconFileId.HasValue)
                        {
                            BinaryFileService binaryFileService = new BinaryFileService();
                            var binaryFile = binaryFileService.Get( orphanedIconFileId.Value );
                            if ( binaryFile != null )
                            {
                                // marked the old images as IsTemporary so they will get cleaned up later
                                binaryFile.IsTemporary = true;
                                binaryFileService.Save( binaryFile, CurrentPersonId );
                            }
                        }

                        Rock.Web.Cache.PageCache.Flush( _page.Id );

                        string script = "if (typeof window.parent.Rock.controls.modal.close === 'function') window.parent.Rock.controls.modal.close('PAGE_UPDATED');";
                        ScriptManager.RegisterStartupScript( this.Page, this.GetType(), "close-modal", script, true );
                    }
                }
            }
        }

        protected void lbExport_Click( object sender, EventArgs e )
        {
            var pageService = new PageService();
            var page = pageService.Get( _page.Guid );
            var packageService = new PackageService();
            var pageName = page.Name.Replace( " ", "_" ) + ( ( cbExportChildren.Checked ) ? "_wChildPages" : "" );
            using ( var stream = packageService.ExportPage( page, cbExportChildren.Checked ) )
            {
                EnableViewState = false;
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader( "content-disposition", "attachment; filename=" + pageName + ".nupkg" );
                Response.Charset = "";
                Response.BinaryWrite( stream.ToArray() );
                Response.Flush();
                Response.End();
            }
        }

        protected void lbImport_Click( object sender, EventArgs e )
        {
            var extension = fuImport.FileName.Substring( fuImport.FileName.LastIndexOf( '.' ) );

            if ( fuImport.PostedFile == null && extension != ".nupkg"  )
            {
                var errors = new List<string> { "Please attach an export file when trying to import a package." };
                rptImportErrors.DataSource = errors;
                rptImportErrors.DataBind();
                rptImportErrors.Visible = true;
                pnlImportSuccess.Visible = false;
                return;
            }

            var packageService = new PackageService();
            bool importResult;

            using ( new UnitOfWorkScope() )
            {
                importResult = packageService.ImportPage( fuImport.FileBytes, fuImport.FileName, CurrentPerson.Id, _page.Id, _page.Layout.SiteId );
            }

            if ( !importResult )
            {
                rptImportErrors.DataSource = packageService.ErrorMessages;
                rptImportErrors.DataBind();
                rptImportErrors.Visible = true;
                pnlImportSuccess.Visible = false;
            }
            else
            {
                pnlImportSuccess.Visible = true;
                rptImportWarnings.Visible = false;
                rptImportErrors.Visible = false;

                if ( packageService.WarningMessages.Count > 0 )
                {
                    rptImportErrors.DataSource = packageService.WarningMessages;
                    rptImportErrors.DataBind();
                    rptImportWarnings.Visible = true;
                }
            }
        }

        #endregion

        #region Internal Methods

        private void LoadSites()
        {
            ddlSite.Items.Clear();
            foreach(Site site in new SiteService().Queryable().OrderBy(s => s.Name))
            {
                ddlSite.Items.Add( new ListItem( site.Name, site.Id.ToString() ) );
            }
        }

        private void LoadLayouts(SiteCache Site)
        {
            ddlLayout.Items.Clear();
            var layoutService = new LayoutService();
            layoutService.RegisterLayouts( Request.MapPath( "~" ), Site, CurrentPersonId );
            foreach ( var layout in layoutService.GetBySiteId( Site.Id ) )
            {
                ddlLayout.Items.Add( new ListItem( layout.Name, layout.Id.ToString() ) );
            }
        }

        /// <summary>
        /// Displays the error.
        /// </summary>
        /// <param name="message">The message.</param>
        private void DisplayError( string message )
        {
            pnlError.Controls.Clear();
            pnlError.Controls.Add( new LiteralControl( message ) );
            pnlError.Visible = true;

            phContent.Visible = false;
        }

        /// <summary>
        /// Gets the tab class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        protected string GetTabClass( object property )
        {
            if ( property.ToString() == CurrentTab )
            {
                return "active";
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Shows the selected pane.
        /// </summary>
        private void ShowSelectedPane()
        {
            if ( CurrentTab.Equals( "Basic Settings" ) )
            {
                pnlBasicProperty.Visible = true;
                pnlDisplaySettings.Visible = false;
                pnlAdvancedSettings.Visible = false;
                pnlImportExport.Visible = false;
                pnlBasicProperty.DataBind();
            }
            else if ( CurrentTab.Equals( "Display Settings" ) )
            {
                pnlBasicProperty.Visible = false;
                pnlDisplaySettings.Visible = true;
                pnlAdvancedSettings.Visible = false;
                pnlImportExport.Visible = false;
                pnlDisplaySettings.DataBind();
            }
            else if ( CurrentTab.Equals( "Advanced Settings" ) )
            {
                pnlBasicProperty.Visible = false;
                pnlDisplaySettings.Visible = false;
                pnlAdvancedSettings.Visible = true;
                pnlImportExport.Visible = false;
                pnlAdvancedSettings.DataBind();
            }
            else if ( CurrentTab.Equals( "Import/Export" ) )
            {
                pnlBasicProperty.Visible = false;
                pnlDisplaySettings.Visible = false;
                pnlAdvancedSettings.Visible = false;
                pnlImportExport.Visible = true;
                pnlImportExport.DataBind();
            }

            upPanel.DataBind();
        }

        #endregion

        protected void cvPageRoute_ServerValidate( object source, ServerValidateEventArgs args )
        {
            var errorMessages = new List<string>();

            foreach ( string route in tbPageRoute.Text.SplitDelimitedValues() )
            {
                var pageRoute = new PageRoute();
                pageRoute.Route = route;
                pageRoute.Guid = Guid.NewGuid();
                if ( !pageRoute.IsValid )
                {
                    errorMessages.Add( string.Format( "The '{0}' route is invalid: {1}", route,
                    pageRoute.ValidationResults.Select( r => r.ErrorMessage ).ToList().AsDelimited( "; " ) ) );
                }
            }

            cvPageRoute.ErrorMessage = errorMessages.AsDelimited( "<br/>" );

            args.IsValid = !errorMessages.Any();
        }
    }
}