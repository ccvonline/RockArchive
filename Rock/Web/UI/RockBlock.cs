﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Rock.Attribute;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;

namespace Rock.Web.UI
{
    /// <summary>
    /// RockBlock is the base abstract class that all Blocks should inherit from
    /// </summary>
    public abstract class RockBlock : UserControl
    {
        #region Public Properties

        /// <summary>
        /// The current page.  This value is read and cached by the RockRouteHandler
        /// and set by the layout's base class (Rock.Web.UI.RockPage) when loading the block instance
        /// </summary>
        public PageCache CurrentPage { get; set; }

        /// <summary>
        /// The current block.  This value is read and cached by the layout's 
        /// base class (Rock.Web.UI.RockPage) when loading the block instance
        /// </summary>
        public BlockCache CurrentBlock { get; set; }

        /// <summary>
        /// Gets the current page reference.
        /// </summary>
        /// <value>
        /// The current page reference.
        /// </value>
        public PageReference CurrentPageReference { get; set; }

        /// <summary>
        /// The personID of the currently logged in user.  If user is not logged in, returns null
        /// </summary>
        public int? CurrentPersonId
        {
            get { return ( (RockPage)this.Page ).CurrentPersonId; }
        }

        /// <summary>
        /// Returns the currently logged in user.  If user is not logged in, returns null
        /// </summary>
        public UserLogin CurrentUser
        {
            get { return ( (RockPage)this.Page ).CurrentUser; }
        }

        /// <summary>
        /// Returns the currently logged in person. If user is not logged in, returns null
        /// </summary>
        public Person CurrentPerson
        {
            get { return ( (RockPage)this.Page ).CurrentPerson; }
        }

        /// <summary>
        /// Relative path to the current theme and layout folder.  Useful for setting paths to
        /// theme resource files
        /// <example>
        /// Client Side: <c><![CDATA[<img src='<%= CurrentTheme %>/Images/avatar.gif' />]]> </c>
        /// Server Side: <c>myImg.ImageUrl = CurrentTheme + "/Images/avatar.gif";</c>
        /// </example>
        /// </summary>
        public string CurrentTheme
        {
            get { return ( (RockPage)this.Page ).CurrentTheme; }
        }

        /// <summary>
        /// Gets the bread crumbs that were created during the page's oninit.  A block
        /// can add additional breadcrumbs to this list to be rendered.  Crumb's added 
        /// this way will not be saved to the current page reference's collection of 
        /// breadcrumbs, so wil not be available when user navigates to another child
        /// page.  Because of this only last-level crumbs should be added this way.  To
        /// persist breadcrumbs in the session state, override the GetBreadCrumbs 
        /// method instead.
        /// </summary>
        /// <value>
        /// The bread crumbs.
        /// </value>
        public List<BreadCrumb> BreadCrumbs
        {
            get { return ( (RockPage)this.Page ).BreadCrumbs; }
        }

        /// <summary>
        /// Gets the app path.
        /// </summary>
        /// <value>
        /// The app path.
        /// </value>
        public string AppPath
        {
            get { return ( (RockPage)this.Page ).AppPath; }
        }

        /// <summary>
        /// Gets the root URL Path.
        /// </summary>
        public string RootPath
        {
            get
            {
                Uri uri = new Uri( HttpContext.Current.Request.Url.ToString() );
                return uri.Scheme + "://" + uri.GetComponents( UriComponents.HostAndPort, UriFormat.UriEscaped ) + Page.ResolveUrl( "~" );
            }
        }

        /// <summary>
        /// Gets a list of any context entities that the block requires.
        /// </summary>
        public virtual List<string> ContextTypesRequired
        {
            get
            {
                if ( _contextTypesRequired == null )
                {
                    _contextTypesRequired = new List<string>();

                    int properties = 0;
                    foreach ( var attribute in this.GetType().GetCustomAttributes( typeof( ContextAwareAttribute ), true ) )
                    {
                        var contextAttribute = (ContextAwareAttribute)attribute;
                        string contextType = string.Empty;

                        if ( String.IsNullOrEmpty( contextAttribute.EntityType ) )
                        {
                            // If the entity type was not specified in the attibute, look for a property that defines it
                            string propertyKeyName = string.Format( "ContextEntityType{0}", properties > 0 ? properties.ToString() : "" );
                            properties++;

                            if ( !String.IsNullOrEmpty( GetAttributeValue( propertyKeyName ) ) )
                            {
                                contextType = GetAttributeValue( propertyKeyName );
                            }
                        }
                        else
                        {
                            contextType = contextAttribute.EntityType;
                        }

                        if ( contextType != string.Empty && !_contextTypesRequired.Contains( contextType ) )
                        {
                            _contextTypesRequired.Add( contextType );
                        }
                    }
                }
                return _contextTypesRequired;
            }
        }
        private List<string> _contextTypesRequired;

        /// <summary>
        /// Gets a dictionary of the current context entities.  The key is the type of context, and the value is the entity object
        /// </summary>
        /// <value>
        /// The context entities.
        /// </value>
        private Dictionary<string, Rock.Data.IEntity> ContextEntities { get; set; }

        /// <summary>
        /// Returns the ContextEntity of the Type specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ContextEntity<T>() where T : Rock.Data.IEntity
        {
            string entityTypeName = typeof( T ).FullName;
            if ( ContextEntities.ContainsKey( entityTypeName ) )
            {
                var entity = ContextEntities[entityTypeName];
                return (T)entity;
            }
            else
            {
                return default( T );
            }
        }

        /// <summary>
        /// Return the ContextEntity for blocks that are designed to have at most one ContextEntity
        /// </summary>
        /// <returns></returns>
        public Rock.Data.IEntity ContextEntity()
        {
            if ( ContextEntities.Count() == 1 )
            {
                return ContextEntities.First().Value;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RockBlock" /> class.
        /// </summary>
        public RockBlock()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RockBlock" /> class.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <param name="currentBlock">The current block.</param>
        /// <param name="currentPageReference">The current page reference.</param>
        public RockBlock( PageCache currentPage, BlockCache currentBlock, PageReference currentPageReference )
        {
            CurrentPage = CurrentPage;
            CurrentBlock = currentBlock;
            CurrentPageReference = currentPageReference;
        }

        #endregion

        #region Protected Caching Methods

        /// <summary>
        /// Adds an object to the default MemoryCache.
        /// </summary>
        /// <param name="value">Object to cache</param>
        protected virtual void AddCacheItem( object value )
        {
            CacheItemPolicy cacheItemPolicy = null;
            AddCacheItem( string.Empty, value, cacheItemPolicy );
        }

        /// <summary>
        /// Adds an object to the default MemoryCache.
        /// </summary>
        /// <param name="key">Key to differentiate items from same block instance</param>
        /// <param name="value">Object to cache</param>
        protected virtual void AddCacheItem( string key, object value )
        {
            CacheItemPolicy cacheItemPolicy = null;
            AddCacheItem( key, value, cacheItemPolicy );
        }

        /// <summary>
        /// Adds an object to the default MemoryCache.
        /// </summary>
        /// <param name="key">Key to differentiate items from same block instance</param>
        /// <param name="value">Object to cache</param>
        /// <param name="seconds">The Number of seconds to cache object for</param>
        protected virtual void AddCacheItem( string key, object value, int seconds )
        {
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds( seconds );
            AddCacheItem( key, value, cacheItemPolicy );
        }

        /// <summary>
        /// Adds an object to the default MemoryCache.
        /// </summary>
        /// <param name="key">Key to differentiate items from same block instance</param>
        /// <param name="value">Object to cache</param>
        /// <param name="cacheItemPolicy">Optional CacheItemPolicy, defaults to null</param>
        protected virtual void AddCacheItem( string key, object value, CacheItemPolicy cacheItemPolicy )
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Set( ItemCacheKey( key ), value, cacheItemPolicy );
        }

        /// <summary>
        /// Retrieve an object from the cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual object GetCacheItem( string key = "" )
        {
            ObjectCache cache = MemoryCache.Default;
            return cache[ItemCacheKey( key )];
        }

        /// <summary>
        /// Flush an object from the cache
        /// </summary>
        /// <param name="key"></param>
        protected virtual void FlushCacheItem( string key = "" )
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove( ItemCacheKey( key ) );
        }

        /// <summary>
        /// Items the cache key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string ItemCacheKey( string key )
        {
            return string.Format( "Rock:Page:{0}:RockBlock:{1}:ItemCache:{2}",
                this.CurrentPage.Id, CurrentBlock.Id, key );
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            // Get the context types defined through configuration or block properties
            var requiredContext = ContextTypesRequired;

            // Check to see if a context type was specified in a query, form, or page route parameter
            string param = PageParameter( "ContextEntityType" );
            if ( !String.IsNullOrWhiteSpace( param ) )
            {
                requiredContext.Add( param );
            }

            // Get the current context for each required context type
            ContextEntities = new Dictionary<string, Data.IEntity>();
            foreach ( var contextEntityType in requiredContext )
            {
                if ( !String.IsNullOrWhiteSpace( contextEntityType ) )
                {
                    Data.IEntity contextEntity = CurrentPage.GetCurrentContext( contextEntityType );
                    if ( contextEntity != null )
                    {
                        ContextEntities.Add( contextEntityType, contextEntity );
                    }
                }
            }

            base.OnInit( e );
        }

        /// <summary>
        /// When a control renders it's content to the page, this method will also check to see if 
        /// the block instance of this control has been configured for output caching, and if so, 
        /// the contents will also be rendered to a string variable that will gets cached in the 
        /// default MemoryCache for use next time by the Rock.Web.UI.RockPage.OnInit() method when rendering the 
        /// control content.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render( HtmlTextWriter writer )
        {
            if ( CurrentBlock.OutputCacheDuration > 0 )
            {
                string blockCacheKey = string.Format( "Rock:BlockOutput:{0}", CurrentBlock.Id );
                StringBuilder sbOutput = new StringBuilder();
                StringWriter swOutput = new StringWriter( sbOutput );
                HtmlTextWriter twOutput = new HtmlTextWriter( swOutput );

                base.Render( twOutput );

                CacheItemPolicy cacheDuration = new CacheItemPolicy();
                cacheDuration.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds( CurrentBlock.OutputCacheDuration );

                ObjectCache cache = MemoryCache.Default;
                cache.Set( blockCacheKey, sbOutput.ToString(), cacheDuration );
            }

            base.Render( writer );
        }

        ///// <summary>
        ///// When an unhandled error occurs in a module, a notification box will be displayed.
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnError( EventArgs e )
        //{
        //    DisplayNotification( "Exception", NotificationBoxType.Error,
        //        HttpContext.Current.Server.GetLastError().Message );

        //    base.OnError( e );
        //}
        #endregion

        #region Public Methods

        ///// <summary>
        ///// Clear all child controls and add a notification box with error or warning message
        ///// </summary>
        ///// <param name="title"></param>
        ///// <param name="type"></param>
        ///// <param name="message"></param>
        //public void DisplayNotification( string title, NotificationBoxType type, string message )
        //{
        //    NotificationBox notification = new NotificationBox();
        //    notification.Title = title;
        //    notification.NotificationBoxType = type;
        //    notification.Text = message;
        //    this.Controls.Add( notification );
        //}

        ///// <summary>
        ///// Clear all child controls and add a notification box with an error message
        ///// </summary>
        ///// <param name="message">The message.</param>
        //public void DisplayError( string message )
        //{
        //    DisplayNotification( "Error", NotificationBoxType.Error, message );
        //}

        ///// <summary>
        ///// Clear all child controls and add a notification box with a warning message
        ///// </summary>
        ///// <param name="message">The message.</param>
        //public void DisplayWarning( string message )
        //{
        //    DisplayNotification( "Warning", NotificationBoxType.Warning, message );
        //}

        /// <summary>
        /// Saves the attribute values.
        /// </summary>
        /// <param name="personId">The person id.</param>
        public void SaveAttributeValues( int? personId )
        {
            if ( CurrentBlock != null )
            {
                CurrentBlock.SaveAttributeValues( personId );
            }
        }

        /// <summary>
        /// Returns the current block value for the selected attribute
        /// If the attribute doesn't exist a null value is returned  
        /// </summary>
        /// <param name="key">the block attribute key</param>
        /// <returns>the stored value as a string or null if none exists</returns>
        public string GetAttributeValue( string key )
        {
            if ( CurrentBlock != null )
            {
                return CurrentBlock.GetAttributeValue( key );
            }
            return null;
        }

        /// <summary>
        /// Returns the current block values for the selected attribute.
        /// If the attribute doesn't exist an empty list is returned.
        /// </summary>
        /// <param name="key">the block attribute key</param>
        /// <returns>a list of strings or an empty list if none exists</string></returns>
        public List<string> GetAttributeValues( string key )
        {
            if ( CurrentBlock != null )
            {
                return CurrentBlock.GetAttributeValues( key );
            }

            return new List<string>();
        }

        /// <summary>
        /// Sets the attribute value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetAttributeValue( string key, string value )
        {
            if ( CurrentBlock != null )
            {
                CurrentBlock.SetAttributeValue( key, value );
            }
        }

        /// <summary>
        /// Adds an update trigger for when the block instance properties are updated.
        /// </summary>
        /// <param name="updatePanel">The update panel.</param>
        public void AddAttributeUpdateTrigger( UpdatePanel updatePanel )
        {
            if ( CurrentBlock.IsAuthorized( "Administrate", CurrentPerson ) )
            {
                AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
                trigger.ControlID = string.Format( "blck-cnfg-trggr-{0}", CurrentBlock.Id );
                trigger.EventName = "Click";
                updatePanel.Triggers.Add( trigger );
            }
        }

        /// <summary>
        /// Evaluates if the CurrentPerson is authorized to perform the requested action 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool IsUserAuthorized( string action )
        {
            return CurrentBlock.IsAuthorized( action, CurrentPerson );
        }

        /// <summary>
        /// Checks the page's RouteData values and then the query string for a
        /// parameter matching the specified name, and if found returns the string
        /// value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string PageParameter( string name )
        {
            return ( (RockPage)this.Page ).PageParameter( name );
        }

        /// <summary>
        /// Pages the parameter.
        /// </summary>
        /// <param name="pageReference">The page reference.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string PageParameter( PageReference pageReference, string name )
        {
            return ( (RockPage)this.Page ).PageParameter( pageReference, name );
        }

        /// <summary>
        /// Gets the page route and query string parameters
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> PageParameters()
        {
            return ( (RockPage)this.Page ).PageParameters();
        }

        /// <summary>
        /// Navigates to a linked page attribute.
        /// </summary>
        /// <param name="attributeKey">The attribute key.</param>
        /// <param name="queryParams">The query params.</param>
        public void NavigateToLinkedPage( string attributeKey, Dictionary<string, string> queryParams = null )
        {
            Guid pageGuid = Guid.Empty;
            if ( Guid.TryParse( GetAttributeValue( attributeKey ), out pageGuid ) )
            {
                NavigateToPage( pageGuid, queryParams );
            }
        }

        /// <summary>
        /// Navigates to parent page.
        /// </summary>
        public void NavigateToParentPage( Dictionary<string, string> queryString = null )
        {
            NavigateToPage( this.CurrentPage.ParentPage.Guid, queryString );
        }

        /// <summary>
        /// Shows the detail page.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        /// <param name="itemParentKey">The item parent key.</param>
        /// <param name="itemParentValue">The item parent value.</param>
        public void NavigateToDetailPage( string itemKey, int itemKeyValue, string itemParentKey = null, int? itemParentValue = null )
        {
            string pageGuid = GetAttributeValue( DetailPageAttribute.Key );

            if ( !string.IsNullOrWhiteSpace( pageGuid ) )
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.Add( itemKey, itemKeyValue.ToString() );
                if ( !string.IsNullOrWhiteSpace( itemParentKey ) )
                {
                    queryString.Add( itemParentKey, ( itemParentValue ?? 0 ).ToString() );
                }

                NavigateToPage( new Guid( pageGuid ), queryString );
            }
        }

        /// <summary>
        /// Navigates to page.
        /// </summary>
        /// <param name="pageGuid">The page GUID.</param>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        /// <param name="additionalParams">The additional params.</param>
        public void NavigateToPage( Guid pageGuid, Dictionary<string, string> queryString )
        {
            var pageCache = PageCache.Read( pageGuid );
            if ( pageCache != null )
            {
                int routeId = 0;
                {
                    routeId = pageCache.PageRoutes.FirstOrDefault().Key;
                }

                var pageReference = new PageReference( pageCache.Id, routeId, queryString, null );
                string pageUrl = pageReference.BuildUrl();
                Response.Redirect( pageUrl, false );
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        /// <summary>
        /// Dims the other blocks.
        /// </summary>
        /// <param name="dimmed">if set to <c>true</c> [dimmed].</param>
        public void DimOtherBlocks( bool dimmed )
        {
            this.RockPage().DimOtherBlocks( this, dimmed );
        }

        #region User Preferences

        /// <summary>
        /// Gets the user preference value for the current user for a given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetUserPreference( string key )
        {
            return ( (RockPage)this.Page ).GetUserPreference( key );
        }

        /// <summary>
        /// Gets the preferences for the current user that start with a given key.
        /// </summary>
        /// <param name="keyPrefix">The key prefix.</param>
        /// <returns></returns>
        public Dictionary<string, string> GetUserPreferences( string keyPrefix )
        {
            return ( (RockPage)this.Page ).GetUserPreferences( keyPrefix );
        }

        /// <summary>
        /// Sets a preference for the current user for a given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetUserPreference( string key, string value )
        {
            ( (RockPage)this.Page ).SetUserPreference( key, value );
        }

        #endregion

        /// <summary>
        /// Adds icons to the configuration area of a block instance.  Can be overridden to
        /// add additionsl icons
        /// </summary>
        /// <param name="canConfig"></param>
        /// <param name="canEdit"></param>
        /// <returns></returns>
        public virtual List<Control> GetAdministrateControls( bool canConfig, bool canEdit )
        {
            List<Control> configControls = new List<Control>();

            if ( canConfig || canEdit )
            {
                // Attributes
                CompiledTemplateBuilder upContent = new CompiledTemplateBuilder(
                    delegate( Control content )
                    {
                        Button trigger = new Button();
                        trigger.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        trigger.ID = string.Format( "blck-cnfg-trggr-{0}", CurrentBlock.Id.ToString() );
                        trigger.Click += trigger_Click;
                        content.Controls.Add( trigger );

                        HiddenField triggerData = new HiddenField();
                        triggerData.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        triggerData.ID = string.Format( "blck-cnfg-trggr-data-{0}", CurrentBlock.Id.ToString() );
                        content.Controls.Add( triggerData );
                    }
                );

                UpdatePanel upTrigger = new UpdatePanel();
                upTrigger.ContentTemplate = upContent;
                configControls.Add( upTrigger );
                upTrigger.Attributes.Add( "style", "display:none" );

                // Icon to display block properties
                HtmlGenericControl aAttributes = new HtmlGenericControl( "a" );
                aAttributes.ID = "aBlockProperties";
                aAttributes.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                aAttributes.Attributes.Add( "class", "properties" );
                aAttributes.Attributes.Add( "height", "500px" );
                aAttributes.Attributes.Add( "href", "javascript: showModalPopup($(this), '" + ResolveUrl( string.Format( "~/BlockProperties/{0}?t=Block Properties", CurrentBlock.Id ) ) + "')" );
                aAttributes.Attributes.Add( "title", "Block Properties" );
                //aAttributes.Attributes.Add( "instance-id", BlockInstance.Id.ToString() );
                configControls.Add( aAttributes );
                HtmlGenericControl iAttributes = new HtmlGenericControl( "i" );
                aAttributes.Controls.Add( iAttributes );
                iAttributes.Attributes.Add( "class", "icon-cog" );
            }

            if ( canConfig )
            {
                // Security
                HtmlGenericControl aSecureBlock = new HtmlGenericControl( "a" );
                aSecureBlock.ID = "aSecureBlock";
                aSecureBlock.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                aSecureBlock.Attributes.Add( "class", "security" );
                aSecureBlock.Attributes.Add( "height", "500px" );
                aSecureBlock.Attributes.Add( "href", "javascript: showModalPopup($(this), '" + ResolveUrl( string.Format( "~/Secure/{0}/{1}?t=Block Security&pb=&sb=Done",
                    EntityTypeCache.Read( typeof( Block ) ).Id, CurrentBlock.Id ) ) + "')" );
                aSecureBlock.Attributes.Add( "title", "Block Security" );
                configControls.Add( aSecureBlock );
                HtmlGenericControl iSecureBlock = new HtmlGenericControl( "i" );
                aSecureBlock.Controls.Add( iSecureBlock );
                iSecureBlock.Attributes.Add( "class", "icon-lock" );

                // Move
                HtmlGenericControl aMoveBlock = new HtmlGenericControl( "a" );
                aMoveBlock.Attributes.Add( "class", "block-move block-move" );
                aMoveBlock.Attributes.Add( "href", CurrentBlock.Id.ToString() );
                aMoveBlock.Attributes.Add( "zone", CurrentBlock.Zone );
                aMoveBlock.Attributes.Add( "zoneloc", CurrentBlock.BlockLocation.ToString() );
                aMoveBlock.Attributes.Add( "title", "Move Block" );
                configControls.Add( aMoveBlock );
                HtmlGenericControl iMoveBlock = new HtmlGenericControl( "i" );
                aMoveBlock.Controls.Add( iMoveBlock );
                iMoveBlock.Attributes.Add( "class", "icon-external-link" );

                // Delete
                HtmlGenericControl aDeleteBlock = new HtmlGenericControl( "a" );
                aDeleteBlock.Attributes.Add( "class", "delete block-delete" );
                aDeleteBlock.Attributes.Add( "href", CurrentBlock.Id.ToString() );
                aDeleteBlock.Attributes.Add( "title", "Delete Block" );
                configControls.Add( aDeleteBlock );
                HtmlGenericControl iDeleteBlock = new HtmlGenericControl( "i" );
                aDeleteBlock.Controls.Add( iDeleteBlock );
                iDeleteBlock.Attributes.Add( "class", "icon-remove-circle" );
            }

            return configControls;
        }

        /// <summary>
        /// Returns breadcrumbs specific to the block that should be added to navigation
        /// based on the current page reference.  This function is called during the page's
        /// oninit to load any initial breadcrumbs
        /// </summary>
        /// <param name="pageReference">The page reference.</param>
        /// <returns></returns>
        public virtual List<BreadCrumb> GetBreadCrumbs( PageReference pageReference )
        {
            return new List<BreadCrumb>();
        }

        /// <summary>
        /// Contents the updated.
        /// </summary>
        protected virtual void ContentUpdated()
        {
            CurrentPage.BlockContentUpdated( this );
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates and or updates any attributes defined for the block
        /// </summary>
        internal void CreateAttributes()
        {
            int? blockEntityTypeId = EntityTypeCache.Read( typeof( Block ) ).Id;

            using ( new Rock.Data.UnitOfWorkScope() )
            {
                if ( Rock.Attribute.Helper.UpdateAttributes( this.GetType(), blockEntityTypeId, "BlockTypeId",
                    this.CurrentBlock.BlockTypeId.ToString(), CurrentPersonId ) )
                {
                    this.CurrentBlock.ReloadAttributeValues();
                }
            }
        }

        /// <summary>
        /// Reads the additional actions.
        /// </summary>
        internal List<string> GetAdditionalActions()
        {
            var additionalActions = new List<string>();

            object[] customAttributes = this.GetType().GetCustomAttributes( typeof( AdditionalActionsAttribute ), true );
            if ( customAttributes.Length > 0 )
            {
                foreach ( string action in ( (AdditionalActionsAttribute)customAttributes[0] ).AdditionalActions )
                {
                    additionalActions.Add( action );
                }
            }

            return additionalActions;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the trigger control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void trigger_Click( object sender, EventArgs e )
        {
            if ( AttributesUpdated != null )
            {
                AttributesUpdated( sender, e );
            }
        }

        /// <summary>
        /// Handles the BlockAttributesUpdated event of the CmsBlock control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Rock.Web.UI.BlockAttributesUpdatedEventArgs"/> instance containing the event data.</param>
        void CmsBlock_BlockAttributesUpdated( object sender, BlockAttributesUpdatedEventArgs e )
        {
            if ( e.BlockID == CurrentBlock.Id && AttributesUpdated != null )
                AttributesUpdated( sender, e );
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the block instance properties are updated.
        /// </summary>
        public event EventHandler<EventArgs> AttributesUpdated;

        #endregion

    }
}
