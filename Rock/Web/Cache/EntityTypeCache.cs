﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Rock.Web.Cache
{
    /// <summary>
    /// Information about a entityType that is required by the rendering engine.
    /// This information will be cached by the engine
    /// </summary>
    [Serializable]
    public class EntityTypeCache : Rock.Core.EntityTypeDto
    {
        private EntityTypeCache() : base() { }
        private EntityTypeCache( Rock.Core.EntityType model ) : base( model ) { }

        private static Dictionary<string, int> entityTypes = new Dictionary<string, int>();

        #region Static Methods

        private static string CacheKey( int id )
        {
            return string.Format( "Rock:EntityType:{0}", id );
        }

        public static EntityTypeCache Read( string name )
        {
            if ( entityTypes.ContainsKey( name ) )
                return Read( entityTypes[name] );

            var entityTypeService = new Rock.Core.EntityTypeService();
            var entityTypeModel = entityTypeService.Get( name, true, null );
            return Read( entityTypeModel );
        }

        /// <summary>
        /// Returns EntityType object from cache.  If entityType does not already exist in cache, it
        /// will be read and added to cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static EntityTypeCache Read( int id )
        {
            string cacheKey = EntityTypeCache.CacheKey( id );

            ObjectCache cache = MemoryCache.Default;
            EntityTypeCache entityType = cache[cacheKey] as EntityTypeCache;

            if ( entityType != null )
                return entityType;
            else
            {
                Rock.Core.EntityTypeService entityTypeService = new Rock.Core.EntityTypeService();
                Rock.Core.EntityType entityTypeModel = entityTypeService.Get( id );
                if ( entityTypeModel != null )
                {
                    entityType = CopyModel( entityTypeModel );

                    cache.Set( cacheKey, entityType, new CacheItemPolicy() );

                    return entityType;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Reads the specified field type model.
        /// </summary>
        /// <param name="entityTypeModel">The field type model.</param>
        /// <returns></returns>
        public static EntityTypeCache Read( Rock.Core.EntityType entityTypeModel )
        {
            string cacheKey = EntityTypeCache.CacheKey( entityTypeModel.Id );

            ObjectCache cache = MemoryCache.Default;
            EntityTypeCache entityType = cache[cacheKey] as EntityTypeCache;

            if ( entityType != null )
                return entityType;
            else
            {
                entityType = EntityTypeCache.CopyModel( entityTypeModel );
                cache.Set( cacheKey, entityType, new CacheItemPolicy() );

                return entityType;
            }
        }

        /// <summary>
        /// Copies the model.
        /// </summary>
        /// <param name="entityTypeModel">The field type model.</param>
        /// <returns></returns>
        public static EntityTypeCache CopyModel( Rock.Core.EntityType entityTypeModel )
        {
            EntityTypeCache entityType = new EntityTypeCache( entityTypeModel );

            // update static dictionary object with name/id combination
            if ( entityTypes.ContainsKey( entityType.Name ) )
            {
                entityTypes[entityType.Name] = entityType.Id;
            }
            else
            {
                entityTypes.Add( entityType.Name, entityType.Id );
            }

            return entityType;
        }

        /// <summary>
        /// Removes entityType from cache
        /// </summary>
        /// <param name="id"></param>
        public static void Flush( int id )
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove( EntityTypeCache.CacheKey( id ) );
        }

        #endregion
    }
}