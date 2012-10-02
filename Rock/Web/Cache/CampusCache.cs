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
	/// Information about a campus that is required by the rendering engine.
	/// This information will be cached by the engine
	/// </summary>
    [Serializable]
    public class CampusCache : Rock.Crm.CampusDto
    {
		private CampusCache() : base() { }
		private CampusCache( Rock.Crm.Campus model ) : base( model) { }

        #region Static Methods

        /// <summary>
        /// Gets the cache key for the selected campu id.
        /// </summary>
        /// <param name="id">The campus id.</param>
        /// <returns></returns>
        public static string CacheKey( int id )
        {
            return string.Format( "Rock:Campus:{0}", id );
        }

        /// <summary>
        /// Adds Campus model to cache, and returns cached object
        /// </summary>
        /// <param name="campusModel"></param>
        /// <returns></returns>
        public static CampusCache Read( Rock.Crm.Campus campusModel )
        {
            string cacheKey = CampusCache.CacheKey( campusModel.Id );

            ObjectCache cache = MemoryCache.Default;
            CampusCache campus = cache[cacheKey] as CampusCache;

			if ( campus != null )
				return campus;
			else
			{
				campus = CampusCache.CopyModel( campusModel );
				cache.Set( cacheKey, campus, new CacheItemPolicy() );

				return campus;
			}
        }

        /// <summary>
        /// Returns Campus object from cache.  If campus does not already exist in cache, it
        /// will be read and added to cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CampusCache Read( int id )
        {
            string cacheKey = CampusCache.CacheKey( id );

            ObjectCache cache = MemoryCache.Default;
            CampusCache campus = cache[cacheKey] as CampusCache;

            if ( campus != null )
                return campus;
            else
            {
                Rock.Crm.CampusService campusService = new Rock.Crm.CampusService();
                Rock.Crm.Campus campusModel = campusService.Get( id );
                if ( campusModel != null )
                {
                    Rock.Attribute.Helper.LoadAttributes( campusModel );

                    campus = CampusCache.CopyModel( campusModel );
 
                    cache.Set( cacheKey, campus, new CacheItemPolicy() );

                    return campus;
                }
                else
                    return null;

            }
        }

        // Copies the Model object to the Cached object
        private static CampusCache CopyModel( Rock.Crm.Campus campusModel )
        {
			// Creates new object by copying properties of model
            var campus = new Rock.Web.Cache.CampusCache(campusModel);
            return campus;
        }

        /// <summary>
        /// Removes campus from cache
        /// </summary>
        /// <param name="id"></param>
        public static void Flush( int id )
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove( CampusCache.CacheKey( id ) );
		}

		#endregion
	}
}