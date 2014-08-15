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
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization;

using Rock.Data;
using Rock.Model;
using Rock.Security;

namespace Rock.Web.Cache
{
    /// <summary>
    /// Information about a RestAction that is required by the rendering engine.
    /// This information will be cached by the engine
    /// </summary>
    [Serializable]
    [DataContract]
    public class RestActionCache : CachedModel<RestAction>
    {
        #region Constructors

        private RestActionCache()
        {
        }

        private RestActionCache( RestAction model )
        {
            CopyFromModel( model );
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the defined type id.
        /// </summary>
        /// <value>
        /// The defined type id.
        /// </value>
        [DataMember]
        public int ControllerId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string ApiId { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        /// <value>
        /// The type of the field.
        /// </value>
        public RestControllerCache RestController
        {
            get { return RestControllerCache.Read( ControllerId ); }
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
                return RestController;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( Data.IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is RestAction )
            {
                var RestAction = (RestAction)model;
                this.ControllerId = RestAction.ControllerId;
                this.Method = RestAction.Method;
                this.ApiId = RestAction.ApiId;
                this.Path = RestAction.Path;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Path;
        }

        #endregion

        #region Static Methods

        private static string CacheKey( int id )
        {
            return string.Format( "Rock:RestAction:{0}", id );
        }

        /// <summary>
        /// Returns RestAction object from cache.  If RestAction does not already exist in cache, it
        /// will be read and added to cache
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        public static RestActionCache Read( int id, RockContext rockContext = null )
        {
            string cacheKey = RestActionCache.CacheKey( id );
            ObjectCache cache = MemoryCache.Default;
            RestActionCache RestAction = cache[cacheKey] as RestActionCache;

            if ( RestAction == null )
            {
                rockContext = rockContext ?? new RockContext();
                var RestActionService = new RestActionService( rockContext );
                var RestActionModel = RestActionService.Get( id );
                if ( RestActionModel != null )
                {
                    RestAction = new RestActionCache( RestActionModel );

                    var cachePolicy = new CacheItemPolicy();
                    cache.Set( cacheKey, RestAction, cachePolicy );
                    cache.Set( RestAction.Guid.ToString(), RestAction.Id, cachePolicy );
                }
            }

            return RestAction;
        }

        /// <summary>
        /// Reads the specified GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        public static RestActionCache Read( Guid guid, RockContext rockContext = null )
        {
            ObjectCache cache = MemoryCache.Default;
            object cacheObj = cache[guid.ToString()];

            RestActionCache restAction = null;
            if ( cacheObj != null )
            {
                restAction = Read( (int)cacheObj );
            }

            if ( restAction == null )
            {
                rockContext = rockContext ?? new RockContext();
                var RestActionService = new RestActionService( rockContext );
                var RestActionModel = RestActionService.Get( guid );
                if ( RestActionModel != null )
                {
                    restAction = new RestActionCache( RestActionModel );

                    var cachePolicy = new CacheItemPolicy();
                    cache.Set( RestActionCache.CacheKey( restAction.Id ), restAction, cachePolicy );
                    cache.Set( restAction.Guid.ToString(), restAction.Id, cachePolicy );
                }
            }

            return restAction;
        }

        /// <summary>
        /// Reads the specified API identifier.
        /// </summary>
        /// <param name="apiId">The API identifier.</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        public static RestActionCache Read( string apiId, RockContext rockContext = null )
        {
            ObjectCache cache = MemoryCache.Default;
            object cacheObj = cache[apiId];

            RestActionCache restAction = null;
            if ( cacheObj != null )
            {
                restAction = Read( (int)cacheObj );
            }
            else
            {
                rockContext = rockContext ?? new RockContext();
                var RestActionService = new RestActionService( rockContext );
                var RestActionModel = RestActionService.Queryable()
                    .Where( a => a.ApiId == apiId )
                    .FirstOrDefault();
                if ( RestActionModel != null )
                {
                    restAction = new RestActionCache( RestActionModel );

                    var cachePolicy = new CacheItemPolicy();
                    cache.Set( RestActionCache.CacheKey( restAction.Id ), restAction, cachePolicy );
                    cache.Set( apiId, restAction.Id, cachePolicy );
                }
            }

            return restAction;
        }

        /// <summary>
        /// Reads the specified defined value model.
        /// </summary>
        /// <param name="RestActionModel">The defined value model.</param>
        /// <returns></returns>
        public static RestActionCache Read( RestAction RestActionModel )
        {
            string cacheKey = RestActionCache.CacheKey( RestActionModel.Id );
            ObjectCache cache = MemoryCache.Default;
            RestActionCache restAction = cache[cacheKey] as RestActionCache;

            if ( restAction != null )
            {
                restAction.CopyFromModel( RestActionModel );
            }
            else
            {
                restAction = new RestActionCache( RestActionModel );
                var cachePolicy = new CacheItemPolicy();
                cache.Set( cacheKey, restAction, cachePolicy );
                cache.Set( restAction.Guid.ToString(), restAction.Id, cachePolicy );
            }

            return restAction;
        }

        /// <summary>
        /// Removes RestAction from cache
        /// </summary>
        /// <param name="id"></param>
        public static void Flush( int id )
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove( RestActionCache.CacheKey( id ) );
        }

        /// <summary>
        /// Gets the name of the defined value given an id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static string GetName( int? id )
        {
            if ( id.HasValue )
            {
                var RestAction = Read( id.Value );
                if ( RestAction != null )
                {
                    return RestAction.Method;
                }
            }

            return null;
        }

        #endregion

    }
}