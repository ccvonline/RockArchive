﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

using Rock.Web.Cache;
using Rock.Web.UI;

namespace Rock.Web
{
    /// <summary>
    /// Helper class to work with the PageReference field type
    /// </summary>
    public class PageReference
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// Gets the route id.
        /// </summary>
        public int RouteId { get; private set; }

        /// <summary>
        /// Gets the route parameters.
        /// </summary>
        /// <value>
        /// The route parameters.
        /// </value>
        public Dictionary<string, string> Parameters { get; private set; }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <value>
        /// The query string.
        /// </value>
        public NameValueCollection QueryString { get; private set; }

        /// <summary>
        /// Gets or sets the bread crumbs.
        /// </summary>
        /// <value>
        /// The bread crumbs.
        /// </value>
        public List<BreadCrumb> BreadCrumbs { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid
        {
            get
            {
                if ( PageId != 0 )
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Builds the URL.
        /// </summary>
        /// <returns></returns>
        public string BuildUrl()
        {
            string url = string.Empty;

            var parms = new Dictionary<string, string>();

            // Add any route parameters
            if (Parameters != null)
            {
                foreach(var route in Parameters)
                {
                    parms.Add(route.Key, route.Value);
                }
            }

            // merge parms from query string to the parms dictionary to get a single list of parms
            // skipping those parms that are already in the dictionary
            if ( QueryString != null )
            {
                foreach ( string key in QueryString.AllKeys )
                {
                    // check that the dictionary doesn't already have this key
                    if ( !parms.ContainsKey( key ) )
                        parms.Add( key, QueryString[key].ToString() );
                }
            }

            // load route URL 
            if ( RouteId != 0 )
            {
                url = BuildRouteURL( parms );
            }

            // build normal url if route url didn't process
            if ( url == string.Empty )
            {
                url = "page/" + PageId;

                // add parms to the url
                if ( parms != null )
                {
                    string delimitor = "?";
                    foreach ( KeyValuePair<string, string> parm in parms )
                    {
                        url += delimitor + parm.Key + "=" + HttpUtility.UrlEncode( parm.Value );
                        delimitor = "&";
                    }
                }
            }

            // add base path to url -- Fixed bug #84
            url = ( HttpContext.Current.Request.ApplicationPath == "/" ) ? "/" + url : HttpContext.Current.Request.ApplicationPath + "/" + url;

            return url;
        }

        private string BuildRouteURL( Dictionary<string, string> parms )
        {
            string routeUrl = string.Empty;

            foreach ( Route route in RouteTable.Routes )
            {
                if ( route.DataTokens != null && route.DataTokens["RouteId"].ToString() == RouteId.ToString() )
                {
                    routeUrl = route.Url;
                    break;
                }
            }

            // get dictionary of parms in the route
            Dictionary<string, string> routeParms = new Dictionary<string, string>();
            bool allRouteParmsProvided = true;

            var r = new Regex( @"{([A-Za-z0-9\-]+)}" );
            foreach ( Match match in r.Matches( routeUrl ) )
            {
                // add parm to dictionary
                routeParms.Add( match.Groups[1].Value, match.Value );

                // check that a value for that parm is available
                if ( parms == null || !parms.ContainsKey( match.Groups[1].Value ) )
                    allRouteParmsProvided = false;
            }

            // if we have a value for all route parms build route url
            if ( allRouteParmsProvided )
            {
                // merge route parm values
                foreach ( KeyValuePair<string, string> parm in routeParms )
                {
                    // merge field
                    routeUrl = routeUrl.Replace( parm.Value, parms[parm.Key] );

                    // remove parm from dictionary
                    parms.Remove( parm.Key );
                }

                // add remaining parms to the query string
                if ( parms != null )
                {
                    string delimitor = "?";
                    foreach ( KeyValuePair<string, string> parm in parms )
                    {
                        routeUrl += delimitor + parm.Key + "=" + HttpUtility.UrlEncode( parm.Value );
                        delimitor = "&";
                    }
                }

                return routeUrl;
            }
            else
                return string.Empty;
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="PageReference"/> class.
        /// </summary>
        public PageReference(){}

        /// <summary>
        /// Initializes a new instance of the <see cref="PageReference"/> class.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public PageReference( string reference )
        {
            string[] items = reference.Split( ',' );

            int pageId = 0;
            int routeId = 0;

            if ( items.Length == 2 )
            {
                int.TryParse( items[0], out pageId );
                int.TryParse( items[1], out routeId );
            }

            PageId = pageId;
            RouteId = routeId;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageReference" /> class.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        public PageReference( int pageId )
        {
            PageId = pageId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageReference"/> class.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="routeId">The route id.</param>
        public PageReference( int pageId, int routeId )
            : this( pageId )
        {
            RouteId = routeId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageReference" /> class.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="routeId">The route id.</param>
        /// <param name="parameters">The route parameters.</param>
        public PageReference( int pageId, int routeId, Dictionary<string, string> parameters )
            : this( pageId, routeId )
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageReference" /> class.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="routeId">The route id.</param>
        /// <param name="parameters">The route parameters.</param>
        /// <param name="queryString">The query string.</param>
        public PageReference( int pageId, int routeId, Dictionary<string, string> parameters, NameValueCollection queryString )
            : this(pageId, routeId, parameters)
        {
            QueryString = queryString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageReference" /> class.
        /// </summary>
        /// <param name="pageReference">The page reference.</param>
        public PageReference( PageReference pageReference )
            : this( pageReference.PageId, pageReference.RouteId, pageReference.Parameters, pageReference.QueryString )
        {
        }

        /// <summary>
        /// Gets the parent page references.
        /// </summary>
        /// <returns></returns>
        public static List<PageReference> GetParentPageReferences(RockPage rockPage, PageCache currentPage, PageReference currentPageReference)
        {
            // Get previous page references in nav history
            var pageReferenceHistory = HttpContext.Current.Session["RockPageReferenceHistory"] as List<PageReference>;
                        
            // Current page heirarchy references
            var pageReferences = new List<PageReference>();

            if (currentPage != null)
            {
                var parentPage = currentPage.ParentPage;
                if ( parentPage != null )
                {
                    var currentParentPages = parentPage.GetPageHierarchy();
                    if ( currentParentPages != null && currentParentPages.Count > 0 )
                    {
                        currentParentPages.Reverse();
                        foreach ( PageCache page in currentParentPages )
                        {
                            PageReference parentPageReference = null;
                            if ( pageReferenceHistory != null )
                            {
                                parentPageReference = pageReferenceHistory.Where( p => p.PageId == page.Id ).FirstOrDefault();
                            }

                            if ( parentPageReference == null )
                            {
                                parentPageReference = new PageReference( currentPageReference );
                                parentPageReference.PageId = page.Id;

                                parentPageReference.BreadCrumbs = new List<BreadCrumb>();

                                // TODO: This should eventually use new BreadCrumbDisplayTitle field
                                //if (  )
                                //{
                                    parentPageReference.BreadCrumbs.Add( new BreadCrumb( page.Name, parentPageReference.BuildUrl() ) );
                                //}

                                foreach ( var block in page.Blocks.Where( b=> b.BlockLocation == Model.BlockLocation.Page) )
                                {
                                    System.Web.UI.Control control = rockPage.TemplateControl.LoadControl(block.BlockType.Path);
                                    if ( control is RockBlock )
                                    {
                                        RockBlock rockBlock = control as RockBlock;
                                        rockBlock.CurrentPage = page;
                                        rockBlock.CurrentPageReference = parentPageReference;
                                        rockBlock.CurrentBlock = block;
                                        rockBlock.GetBreadCrumbs( parentPageReference ).ForEach( c => parentPageReference.BreadCrumbs.Add( c ) );
                                    }
                                    control = null;
                                }

                            }

                            pageReferences.Add( parentPageReference );
                        }
                    }
                }
            }

            return pageReferences;
        }

        /// <summary>
        /// Saves the history.
        /// </summary>
        /// <param name="pageReferences">The page references.</param>
        public static void SavePageReferences( List<PageReference> pageReferences)
        {
            HttpContext.Current.Session["RockPageReferenceHistory"] = pageReferences;
        }
    }
}