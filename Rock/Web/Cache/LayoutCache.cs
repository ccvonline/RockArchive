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
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web;
using Rock.Model;
using Rock.Security;

namespace Rock.Web.Cache
{
    /// <summary>
    /// Information about a Layout that is required by the rendering engine.
    /// This information will be cached by the engine
    /// </summary>
    [Serializable]
    public class LayoutCache : CachedModel<Layout>
    {
        #region Constructors

        private LayoutCache()
        {
        }

        private LayoutCache( Layout Layout )
        {
            CopyFromModel( Layout );
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is system.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is system; otherwise, <c>false</c>.
        /// </value>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        /// <value>
        /// The site id.
        /// </value>
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets the site.
        /// </summary>
        /// <value>
        /// The site.
        /// </value>
        public SiteCache Site
        {
            get
            {
                return SiteCache.Read( SiteId );
            }
        }

        /// <summary>
        /// Gets the parent authority.
        /// </summary>
        /// <value>
        /// The parent authority.
        /// </value>
        public override ISecured ParentAuthority
        {
            get
            {
                return this.Site;
            }
        }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Copies from model.
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( Data.IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is Layout )
            {
                var Layout = (Layout)model;
                this.IsSystem = Layout.IsSystem;
                this.SiteId = Layout.SiteId;
                this.FileName = Layout.FileName;
                this.Name = Layout.Name;
                this.Description = Layout.Description;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion

        #region Static Methods

        private static string CacheKey( int id )
        {
            return string.Format( "Rock:Layout:{0}", id );
        }

        /// <summary>
        /// Returns Layout object from cache.  If Layout does not already exist in cache, it
        /// will be read and added to cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static LayoutCache Read( int id )
        {
            string cacheKey = LayoutCache.CacheKey( id );

            ObjectCache cache = MemoryCache.Default;
            LayoutCache Layout = cache[cacheKey] as LayoutCache;

            if ( Layout != null )
            {
                return Layout;
            }
            else
            {
                var LayoutService = new LayoutService();
                var LayoutModel = LayoutService.Get( id );
                if ( LayoutModel != null )
                {
                    LayoutModel.LoadAttributes();
                    Layout = new LayoutCache( LayoutModel );

                    var cachePolicy = new CacheItemPolicy();
                    cache.Set( cacheKey, Layout, cachePolicy );
                    cache.Set( Layout.Guid.ToString(), Layout.Id, cachePolicy );

                    return Layout;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Reads the specified GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        public static LayoutCache Read( Guid guid )
        {
            ObjectCache cache = MemoryCache.Default;
            object cacheObj = cache[guid.ToString()];

            if ( cacheObj != null )
            {
                return Read( (int)cacheObj );
            }
            else
            {
                var LayoutService = new LayoutService();
                var LayoutModel = LayoutService.Get( guid );
                if ( LayoutModel != null )
                {
                    LayoutModel.LoadAttributes();
                    var Layout = new LayoutCache( LayoutModel );

                    var cachePolicy = new CacheItemPolicy();
                    cache.Set( LayoutCache.CacheKey( Layout.Id ), Layout, cachePolicy );
                    cache.Set( Layout.Guid.ToString(), Layout.Id, cachePolicy );

                    return Layout;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Adds Layout model to cache, and returns cached object
        /// </summary>
        /// <param name="LayoutModel"></param>
        /// <returns></returns>
        public static LayoutCache Read( Layout LayoutModel )
        {
            string cacheKey = LayoutCache.CacheKey( LayoutModel.Id );

            ObjectCache cache = MemoryCache.Default;
            LayoutCache Layout = cache[cacheKey] as LayoutCache;

            if ( Layout != null )
            {
                return Layout;
            }
            else
            {
                Layout = new LayoutCache( LayoutModel );

                var cachePolicy = new CacheItemPolicy();
                cache.Set( cacheKey, Layout, cachePolicy );
                cache.Set( Layout.Guid.ToString(), Layout.Id, cachePolicy );

                return Layout;
            }
        }

        /// <summary>
        /// Removes Layout from cache
        /// </summary>
        /// <param name="id"></param>
        public static void Flush( int id )
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove( LayoutCache.CacheKey( id ) );
        }

        #endregion

    }
}