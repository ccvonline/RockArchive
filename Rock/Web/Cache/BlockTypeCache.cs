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

using Rock.Data;
using Rock.Model;

namespace Rock.Web.Cache
{
    /// <summary>
    /// Information about a block type that is required by the rendering engine.
    /// This information will be cached by the engine
    /// </summary>
    [Serializable]
    public class BlockTypeCache : CachedModel<BlockType>
    {
        #region Constructors

        private BlockTypeCache()
        {
        }

        private BlockTypeCache( BlockType blockType )
        {
            CopyFromModel( blockType );
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
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

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
        /// Gets a value indicating whether the <see cref="Rock.Attribute.TextFieldAttribute" /> attributes have been
        /// verified for the block type.  If not, Rock will create and/or update the attributes associated with the block.
        /// </summary>
        /// <value>
        /// <c>true</c> if attributes have already been verified; otherwise, <c>false</c>.
        /// </value>
        public bool IsInstancePropertiesVerified { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether [checked security actions].
        /// </summary>
        /// <value>
        /// <c>true</c> if [checked security actions]; otherwise, <c>false</c>.
        /// </value>
        public bool CheckedSecurityActions { get; set; }

        /// <summary>
        /// Gets or sets the security actions that were defined by a SecurityActionAttribute on the block type
        /// </summary>
        /// <value>
        /// The security actions.
        /// </value>
        public Dictionary<string, string> SecurityActions { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Copies from model.
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( Data.IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is BlockType )
            {
                var blockType = (BlockType)model;
                this.IsSystem = blockType.IsSystem;
                this.Path = blockType.Path;
                this.Name = blockType.Name;
                this.Description = blockType.Description;

                this.IsInstancePropertiesVerified = false;
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
            return this.Name;
        }

        #endregion

        #region Static Methods

        private static string CacheKey( int id )
        {
            return string.Format( "Rock:BlockType:{0}", id );
        }

        /// <summary>
        /// Returns Block Type object from cache.  If block does not already exist in cache, it
        /// will be read and added to cache
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        public static BlockTypeCache Read( int id, RockContext rockContext = null )
        {
            string cacheKey = BlockTypeCache.CacheKey( id );
            ObjectCache cache = MemoryCache.Default;
            BlockTypeCache blockType = cache[cacheKey] as BlockTypeCache;

            if ( blockType == null )
            {
                rockContext = rockContext ?? new RockContext();
                var blockTypeService = new BlockTypeService( rockContext );
                var blockTypeModel = blockTypeService.Get( id );
                if ( blockTypeModel != null )
                {
                    blockTypeModel.LoadAttributes( rockContext );
                    blockType = new BlockTypeCache( blockTypeModel );

                    var cachePolicy = new CacheItemPolicy();
                    AddChangeMonitor( cachePolicy, blockType.Path );
                    cache.Set( cacheKey, blockType, cachePolicy );

                    var guidCachePolicy = new CacheItemPolicy();
                    AddChangeMonitor( guidCachePolicy, blockType.Path );
                    cache.Set( blockType.Guid.ToString(), blockType.Id, guidCachePolicy );
                }
            }

            return blockType;
        }

        /// <summary>
        /// Reads the specified GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        public static BlockTypeCache Read( Guid guid, RockContext rockContext = null )
        {
            ObjectCache cache = MemoryCache.Default;
            object cacheObj = cache[guid.ToString()];

            BlockTypeCache blockType = null;
            if ( cacheObj != null )
            {
                blockType = Read( (int)cacheObj );
            }

            if ( blockType == null )
            {
                rockContext = rockContext ?? new RockContext();
                var blockTypeService = new BlockTypeService( rockContext );
                var blockTypeModel = blockTypeService.Get( guid );
                if ( blockTypeModel != null )
                {
                    blockTypeModel.LoadAttributes( rockContext );
                    blockType = new BlockTypeCache( blockTypeModel );

                    var cachePolicy = new CacheItemPolicy();
                    AddChangeMonitor( cachePolicy, blockType.Path );
                    cache.Set( BlockTypeCache.CacheKey( blockType.Id ), blockType, cachePolicy );

                    var guidCachePolicy = new CacheItemPolicy();
                    AddChangeMonitor( guidCachePolicy, blockType.Path );
                    cache.Set( blockType.Guid.ToString(), blockType.Id, guidCachePolicy );
                }
            }

            return blockType;
        }

        /// <summary>
        /// Reads the specified block type model.
        /// </summary>
        /// <param name="blockTypeModel">The block type model.</param>
        /// <returns></returns>
        public static BlockTypeCache Read( BlockType blockTypeModel )
        {
            string cacheKey = BlockTypeCache.CacheKey( blockTypeModel.Id );
            ObjectCache cache = MemoryCache.Default;
            BlockTypeCache blockType = cache[cacheKey] as BlockTypeCache;

            if ( blockType != null )
            {
                blockType.CopyFromModel( blockTypeModel );
            }
            else
            {
                blockType = new BlockTypeCache( blockTypeModel );

                var cachePolicy = new CacheItemPolicy();
                AddChangeMonitor( cachePolicy, blockType.Path );
                cache.Set( cacheKey, blockType, cachePolicy );

                var guidCachePolicy = new CacheItemPolicy();
                AddChangeMonitor( guidCachePolicy, blockType.Path );
                cache.Set( blockType.Guid.ToString(), blockType.Id, guidCachePolicy );
            }

            return blockType;
        }

        private static void AddChangeMonitor( CacheItemPolicy cacheItemPolicy, string filePath )
        {
            // Block Type cache expiration monitors the actual block on the file system so that it is flushed from 
            // memory anytime the file contents change.  This is to force the cmsPage object to revalidate any
            // BlockPropery attributes that may have been added or modified
            string physicalPath = System.Web.HttpContext.Current.Request.MapPath( filePath );
            List<string> filePaths = new List<string>();
            filePaths.Add( physicalPath );
            filePaths.Add( physicalPath + ".cs" );

            var fileinfo = new System.IO.FileInfo( physicalPath );

            // TODO:  There is a bug in the the .NET 4.5 System.Runtime.Caching framework that causes an 
            // ArgumentOutOfRange exception when running in a positive UTC timezone.  The bug is caused by 
            // initializing a DateTimeOffset variable to DateTime.MinValue that when ajdusted for timzone 
            // ends up being lower then the minimum allowed range.  For now, HostFileChangeMonitoring will
            // only be done in negative timezone offsets.  After we upgrade to .NET 4.5.1 we will need to 
            // see if bug has been fixed.
            //if (TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).Ticks <= 0)
            //{
            if ( fileinfo.Exists )
            {
                cacheItemPolicy.ChangeMonitors.Add( new HostFileChangeMonitor( filePaths ) );
            }
            //}
        }

        /// <summary>
        /// Removes block type from cache
        /// </summary>
        /// <param name="id"></param>
        public static void Flush( int id )
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove( BlockTypeCache.CacheKey( id ) );
        }

        #endregion
    }
}