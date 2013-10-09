//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

using Rock;
using Rock.Model;
using Rock.Data;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using Site = Rock.Model.Site;
using Rock.Constants;
using Rock.Web;

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SiteDetail : RockBlock, IDetailBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            btnDelete.Attributes["onclick"] = string.Format( "javascript: return confirmDelete(event, '{0}');", Rock.Model.Site.FriendlyTypeName );
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
                string itemId = PageParameter( "siteId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "siteId", int.Parse( itemId ) );
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
            if ( hfSiteId.Value.Equals( "0" ) )
            {
                // Cancelling on Add return to site list
                NavigateToParentPage();
            }
            else
            {
                // Cancelling on Edit.  Return to Details
                SiteService siteService = new SiteService();
                Site site = siteService.Get( int.Parse( hfSiteId.Value ) );
                ShowReadonlyDetails( site );
            }
        }

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnEdit_Click( object sender, EventArgs e )
        {
            SiteService siteService = new SiteService();
            Site site = siteService.Get( int.Parse( hfSiteId.Value ) );
            ShowEditDetails( site );
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnDelete_Click( object sender, EventArgs e )
        {
            RockTransactionScope.WrapTransaction( () =>
            {
                SiteService siteService = new SiteService();
                Site site = siteService.Get( int.Parse( hfSiteId.Value ) );
                if ( site != null )
                {
                    string errorMessage;
                    if ( !siteService.CanDelete( site, out errorMessage ) )
                    {
                        mdDeleteWarning.Show( errorMessage, ModalAlertType.Information );
                        return;
                    }

                    siteService.Delete( site, CurrentPersonId );
                    siteService.Save( site, CurrentPersonId );

                    SiteCache.Flush( site.Id );
                }
            } );

            NavigateToParentPage();

        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;
            fieldsetViewDetails.Visible = !editable;

            this.DimOtherBlocks( editable );
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            Site site;

            using ( new Rock.Data.UnitOfWorkScope() )
            {
                SiteService siteService = new SiteService();
                SiteDomainService siteDomainService = new SiteDomainService();
                bool newSite = false;

                int siteId = int.Parse( hfSiteId.Value );

                if ( siteId == 0 )
                {
                    newSite = true;
                    site = new Rock.Model.Site();
                    siteService.Add( site, CurrentPersonId );
                }
                else
                {
                    site = siteService.Get( siteId );
                }

                site.Name = tbSiteName.Text;
                site.Description = tbDescription.Text;
                site.Theme = ddlTheme.Text;
                site.DefaultPageId = ppDefaultPage.PageId ?? 0;
                site.DefaultPageRouteId = ppDefaultPage.PageRouteId;
                site.LoginPageReference = tbLoginPageReference.Text;

                var currentDomains = tbSiteDomains.Text.SplitDelimitedValues().ToList<string>();
                site.SiteDomains = site.SiteDomains ?? new List<SiteDomain>();

                // Remove any deleted domains
                foreach ( var domain in site.SiteDomains.Where( w => !currentDomains.Contains( w.Domain ) ).ToList() )
                {
                    site.SiteDomains.Remove( domain );
                    siteDomainService.Delete( domain, CurrentPersonId );
                }

                foreach ( string domain in currentDomains )
                {
                    SiteDomain sd = site.SiteDomains.Where( d => d.Domain == domain ).FirstOrDefault();
                    if ( sd == null )
                    {
                        sd = new SiteDomain();
                        sd.Domain = domain;
                        sd.Guid = Guid.NewGuid();
                        site.SiteDomains.Add( sd );
                    }
                }

                site.FaviconUrl = tbFaviconUrl.Text;
                site.AppleTouchIconUrl = tbAppleTouchIconUrl.Text;
                site.FacebookAppId = tbFacebookAppId.Text;
                site.FacebookAppSecret = tbFacebookAppSecret.Text;
                site.RegistrationPageReference = tbRegistrationPageReference.Text;
                site.ErrorPage = tbErrorPage.Text;

                if ( !site.IsValid )
                {
                    // Controls will render the error messages                    
                    return;
                }

                RockTransactionScope.WrapTransaction( () =>
                {
                    siteService.Save( site, CurrentPersonId );

                    if ( newSite )
                    {
                        Rock.Security.Authorization.CopyAuthorization( CurrentPage.Layout.Site, site, CurrentPersonId );
                    }
                } );

                SiteCache.Flush( site.Id );
            }

            var qryParams = new Dictionary<string, string>();
            qryParams["siteId"] = site.Id.ToString();

            NavigateToPage( this.CurrentPage.Guid, qryParams );
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            ddlTheme.Items.Clear();
            DirectoryInfo di = new DirectoryInfo( this.Page.Request.MapPath( this.CurrentTheme ) );
            foreach ( var themeDir in di.Parent.EnumerateDirectories().OrderBy( a => a.Name ) )
            {
                ddlTheme.Items.Add( new ListItem( themeDir.Name, themeDir.Name ) );
            }
        }

        /// <summary>
        /// Shows the edit.
        /// </summary>
        /// <param name="siteId">The site id.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            pnlDetails.Visible = false;

            if ( !itemKey.Equals( "siteId" ) )
            {
                return;
            }

            Site site = null;

            if ( !itemKeyValue.Equals( 0 ) )
            {
                site = new SiteService().Get( itemKeyValue );
            }
            else
            {
                site = new Site { Id = 0 };
                site.SiteDomains = new List<SiteDomain>();
                site.Theme = CurrentPage.Layout.Site.Theme;
            }

            if ( site == null )
            {
                return;
            }

            pnlDetails.Visible = true;
            hfSiteId.Value = site.Id.ToString();

            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( "Edit" ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Group.FriendlyTypeName );
            }

            if ( site.IsSystem )
            {
                nbEditModeMessage.Text = EditModeMessage.System( Group.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                btnDelete.Visible = false;
                ShowReadonlyDetails( site );
            }
            else
            {
                btnEdit.Visible = true;
                btnDelete.Visible = !site.IsSystem;
                if ( site.Id > 0 )
                {
                    ShowReadonlyDetails( site );
                }
                else
                {
                    ShowEditDetails( site );
                }
            }

        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="group">The group.</param>
        private void ShowEditDetails( Rock.Model.Site site )
        {
            if ( site.Id == 0 )
            {
                lReadOnlyTitle.Text = ActionTitle.Add(Rock.Model.Site.FriendlyTypeName).FormatAsHtmlTitle();
            }
            else
            {
                lReadOnlyTitle.Text = site.Name.FormatAsHtmlTitle();
            }

            SetEditMode( true );

            LoadDropDowns();

            tbSiteName.ReadOnly = site.IsSystem;
            tbSiteName.Text = site.Name;

            tbDescription.ReadOnly = site.IsSystem;
            tbDescription.Text = site.Description;

            ddlTheme.Enabled = !site.IsSystem;
            ddlTheme.SetValue( site.Theme );
            if ( site.DefaultPageRoute != null )
            {
                ppDefaultPage.SetValue( site.DefaultPageRoute );
            }
            else
            {
                ppDefaultPage.SetValue( site.DefaultPage );
            }

            tbLoginPageReference.Text = site.LoginPageReference;

            tbSiteDomains.Text = string.Join( "\n", site.SiteDomains.Select( dom => dom.Domain ).ToArray() );
            tbFaviconUrl.Text = site.FaviconUrl;
            tbAppleTouchIconUrl.Text = site.AppleTouchIconUrl;
            tbFacebookAppId.Text = site.FacebookAppId;
            tbFacebookAppSecret.Text = site.FacebookAppSecret;
            tbRegistrationPageReference.Text = site.RegistrationPageReference;
            tbErrorPage.Text = site.ErrorPage;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="group">The group.</param>
        private void ShowReadonlyDetails( Rock.Model.Site site )
        {
            SetEditMode( false );

            hfSiteId.SetValue( site.Id );
            lReadOnlyTitle.Text = site.Name.FormatAsHtmlTitle();

            lSiteDescription.Text = site.Description;

            DescriptionList descriptionList = new DescriptionList();
            descriptionList.Add( "Domain(s)", site.SiteDomains.Select( d=> d.Domain ).ToList().AsDelimited(", "));
            descriptionList.Add( "Theme", site.Theme );
            descriptionList.Add( "Default Page", site.DefaultPageRoute );
            lblMainDetails.Text = descriptionList.Html;
        }

        #endregion
    }
}