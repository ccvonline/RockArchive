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
    /// Information about a entityType that is required by the rendering engine.
    /// This information will be cached by the engine
    /// </summary>
    [Serializable]
    public class EntityTypeCache
    {
        #region Static Fields

        // Locking object
        private static readonly Object obj = new object();

        private static Dictionary<string, int> entityTypes = new Dictionary<string, int>();

        #endregion

        #region Constructors

        private EntityTypeCache()
        {
        }

        private EntityTypeCache( EntityType model )
        {
            CopyFromModel( model );
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>
        /// The GUID.
        /// </value>
        public virtual Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        /// <value>
        /// The name of the assembly.
        /// </value>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is entity.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is entity; otherwise, <c>false</c>.
        /// </value>
        public bool IsEntity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is secured.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is secured; otherwise, <c>false</c>.
        /// </value>
        public bool IsSecured { get; set; }

        /// <summary>
        /// Gets or sets the single value field type identifier.
        /// </summary>
        /// <value>
        /// The single value field type identifier.
        /// </value>
        private int? SingleValueFieldTypeId { get; set; }

        /// <summary>
        /// Gets the type of the single value field.
        /// </summary>
        /// <value>
        /// The type of the single value field.
        /// </value>
        public FieldTypeCache SingleValueFieldType
        {
            get
            {
                return FieldTypeCache.Read( this.SingleValueFieldTypeId ?? 0 );
            }
        }

        /// <summary>
        /// Gets or sets the multi value field type identifier.
        /// </summary>
        /// <value>
        /// The multi value field type identifier.
        /// </value>
        private int? MultiValueFieldTypeId { get; set; }

        /// <summary>
        /// Gets the type of the multi value field.
        /// </summary>
        /// <value>
        /// The type of the multi value field.
        /// </value>
        public FieldTypeCache MultiValueFieldType
        {
            get
            {
                return FieldTypeCache.Read( this.MultiValueFieldTypeId ?? 0 );
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <returns></returns>
        public Type GetEntityType()
        {
            if ( !string.IsNullOrWhiteSpace( this.AssemblyName ) )
            {
                return Type.GetType( this.AssemblyName );
            }
            return null;
        }

        /// <summary>
        /// Copies from model.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        public void CopyFromModel( EntityType entityType )
        {
            this.Id = entityType.Id;
            this.Guid = entityType.Guid;
            this.Name = entityType.Name;
            this.AssemblyName = entityType.AssemblyName;
            this.FriendlyName = entityType.FriendlyName;
            this.IsEntity = entityType.IsEntity;
            this.IsSecured = entityType.IsSecured;
            this.SingleValueFieldTypeId = entityType.SingleValueFieldTypeId;
            this.MultiValueFieldTypeId = entityType.MultiValueFieldTypeId;

            lock ( obj )
            {
                // update static dictionary object with name/id combination
                if ( entityTypes.ContainsKey( entityType.Name ) )
                {
                    entityTypes[entityType.Name] = entityType.Id;
                }
                else
                {
                    entityTypes.Add( entityType.Name, entityType.Id );
                }
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
            return string.Format( "Rock:EntityType:{0}", id );
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static int? GetId( Type type )
        {
            return Read( type ).Id;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static int? GetId( string name )
        {
            if ( String.IsNullOrEmpty( name ) )
            {
                return null;
            }

            return Read( name ).Id;
        }

        /// <summary>
        /// Reads the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="createIfNotFound">if set to <c>true</c> [create if not found].</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        public static EntityTypeCache Read( Type type, bool createIfNotFound = true, RockContext rockContext = null )
        {
            int? entityTypeId = null;

            if ( type.Namespace == "System.Data.Entity.DynamicProxies" )
            {
                type = type.BaseType;
            }

            lock ( obj )
            {
                if ( entityTypes.ContainsKey( type.FullName ) )
                {
                    entityTypeId = entityTypes[type.FullName];
                }
            }

            if ( entityTypeId.HasValue )
            {
                return Read( entityTypeId.Value );
            }

            var entityTypeService = new EntityTypeService( rockContext ?? new RockContext() );
            var entityTypeModel = entityTypeService.Get( type, createIfNotFound, null );
            return Read( entityTypeModel );
        }

        /// <summary>
        /// Returns EntityType object from cache.  If entityBlockType does not already exist in cache, it
        /// will be read and added to cache
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static EntityTypeCache Read( string name )
        {
            return Read( name, true );
        }

        /// <summary>
        /// Reads the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="createNew">if set to <c>true</c> [create new].</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        public static EntityTypeCache Read( string name, bool createNew, RockContext rockContext = null )
        {
            int? entityTypeId = null;

            lock ( obj )
            {
                if ( entityTypes.ContainsKey( name ) )
                {
                    entityTypeId = entityTypes[name];
                }
            }

            if ( entityTypeId.HasValue )
            {
                return Read( entityTypeId.Value );
            }

            var entityTypeService = new EntityTypeService( rockContext ?? new RockContext() );
            var entityTypeModel = entityTypeService.Get( name, createNew );
            if ( entityTypeModel != null )
            {
                return Read( entityTypeModel );
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns EntityType object from cache.  If entityBlockType does not already exist in cache, it
        /// will be read and added to cache
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        public static EntityTypeCache Read( int id, RockContext rockContext = null )
        {
            string cacheKey = EntityTypeCache.CacheKey( id );

            ObjectCache cache = MemoryCache.Default;
            EntityTypeCache entityType = cache[cacheKey] as EntityTypeCache;

            if ( entityType != null )
            {
                return entityType;
            }
            else
            {
                var entityTypeService = new EntityTypeService( rockContext ?? new RockContext() );
                var entityTypeModel = entityTypeService.Get( id );
                if ( entityTypeModel != null )
                {
                    entityType = new EntityTypeCache( entityTypeModel );

                    var cachePolicy = new CacheItemPolicy();
                    cache.Set( cacheKey, entityType, cachePolicy );
                    cache.Set( entityType.Guid.ToString(), entityType.Id, cachePolicy );

                    return entityType;
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
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        public static EntityTypeCache Read( Guid guid, RockContext rockContext = null )
        {
            ObjectCache cache = MemoryCache.Default;
            object cacheObj = cache[guid.ToString()];

            if ( cacheObj != null )
            {
                return Read( (int)cacheObj );
            }
            else
            {
                var entityTypeService = new EntityTypeService( rockContext ?? new RockContext() );
                var entityTypeModel = entityTypeService.Get( guid );
                if ( entityTypeModel != null )
                {
                    var entityType = new EntityTypeCache( entityTypeModel );

                    var cachePolicy = new CacheItemPolicy();
                    cache.Set( EntityTypeCache.CacheKey( entityType.Id ), entityType, cachePolicy );
                    cache.Set( entityType.Guid.ToString(), entityType.Id, cachePolicy );

                    return entityType;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Reads the specified field type model.
        /// </summary>
        /// <param name="entityTypeModel">The field type model.</param>
        /// <returns></returns>
        public static EntityTypeCache Read( EntityType entityTypeModel )
        {
            string cacheKey = EntityTypeCache.CacheKey( entityTypeModel.Id );

            ObjectCache cache = MemoryCache.Default;
            EntityTypeCache entityType = cache[cacheKey] as EntityTypeCache;

            if ( entityType != null )
            {
                entityType.CopyFromModel( entityTypeModel );
                return entityType;
            }
            else
            {
                entityType = new EntityTypeCache( entityTypeModel );

                var cachePolicy = new CacheItemPolicy();
                cache.Set( cacheKey, entityType, cachePolicy );
                cache.Set( entityType.Guid.ToString(), entityType.Id, cachePolicy );

                return entityType;
            }
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