﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using Rock.Attribute;
using Rock.Extension;
using Rock.Web.Cache;

namespace Rock.BinaryFile
{
    /// <summary>
    /// Base class for BinaryFile storage components
    /// </summary>
    [ComponentField( "Rock.BinaryFile.StorageContainer, Rock" )]
    public abstract class StorageComponent : Component
    {
        /// <summary>
        /// Saves the files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="personId"></param>
        public abstract void SaveFiles( IEnumerable<Model.BinaryFile> files, int? personId );

        /// <summary>
        /// Removes the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="personId"></param>
        public abstract void RemoveFile( Model.BinaryFile file, int? personId );

        public abstract string GetUrl( Model.BinaryFile file, int? height = null, int? width = null );

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public EntityTypeCache EntityType
        {
            get { return EntityTypeCache.Read( this.GetType() ); }
        }

        /// <summary>
        /// Gets the storage.
        /// </summary>
        /// <value>
        /// The storage.
        /// </value>
        public StorageComponent Storage
        {
            get
            {
                Guid entityTypeGuid = Guid.Empty;

                if ( Guid.TryParse( GetAttributeValue( "StorageContainer" ), out entityTypeGuid ) )
                {
                    foreach ( var serviceEntry in StorageContainer.Instance.Components )
                    {
                        var component = serviceEntry.Value.Value;
                        var entityType = EntityTypeCache.Read( component.GetType() );

                        if ( entityType != null && entityType.Guid.Equals( entityTypeGuid ) )
                        {
                            return component;
                        }
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageComponent"/> class.
        /// </summary>
        public StorageComponent()
        {
            this.LoadAttributes();
        }
    }
}
