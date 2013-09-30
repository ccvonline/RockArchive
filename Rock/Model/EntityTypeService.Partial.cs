//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Rock;
using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// EntityType POCO Service class
    /// </summary>
    public partial class EntityTypeService
    {
        /// <summary>
        /// Gets EntityTypes by EntityName
        /// </summary>
        /// <param name="entityName">Entity.</param>
        /// <returns>An enumerable list of EntityType objects.</returns>
        public EntityType Get( string entityName )
        {
            return Repository.FirstOrDefault( t => t.Name == entityName );
        }

        /// <summary>
        /// Gets the specified type, and optionally creates new type if not found.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="createIfNotFound">if set to <c>true</c> [create if not found].</param>
        /// <param name="personId">The person id.</param>
        /// <returns></returns>
        public EntityType Get( Type type, bool createIfNotFound, int? personId )
        {
            var entityType = Get( type.FullName );
            if ( entityType != null )
                return entityType;

            if ( createIfNotFound )
            {
                entityType = new EntityType();
                entityType.Name = type.FullName;
                entityType.FriendlyName = type.Name.SplitCase();
                entityType.AssemblyName = type.AssemblyQualifiedName;

                this.Add( entityType, personId );
                this.Save( entityType, personId );

                return entityType;
            }

            return null;
        }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="createIfNotFound">if set to <c>true</c> [create if not found].</param>
        /// <param name="personId">The person id.</param>
        /// <returns></returns>
        public EntityType Get( string name, bool createIfNotFound, int? personId )
        {
            var entityType = Get( name );
            if ( entityType != null )
                return entityType;

            if ( createIfNotFound )
            {
                entityType = new EntityType();
                entityType.Name = name;

                this.Add( entityType, personId );
                this.Save( entityType, personId );

                return entityType;
            }

            return null;
        }

        /// <summary>
        /// Gets the entities that have the IsEntity flag set to true
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EntityType> GetEntities()
        {
            return Repository.AsQueryable()
                .Where( e => e.IsEntity );
        }

        /// <summary>
        /// Returns the entities as a grouped collection of listitems with the 
        /// "CommonGets the entity list items.
        /// </summary>
        /// <returns></returns>
        public List<System.Web.UI.WebControls.ListItem> GetEntityListItems()
        {
            var items = new List<System.Web.UI.WebControls.ListItem>();

            var entities = GetEntities().OrderBy( e => e.FriendlyName ).ToList();

            foreach ( var entity in entities.Where( t => t.IsCommon ) )
            {
                var li = new System.Web.UI.WebControls.ListItem( entity.FriendlyName, entity.Id.ToString() );
                li.Attributes.Add( "optiongroup", "Common" );
                items.Add( li );
            }

            foreach ( var entity in entities )
            {
                var li = new System.Web.UI.WebControls.ListItem( entity.FriendlyName, entity.Id.ToString() );
                li.Attributes.Add( "optiongroup", "All Entities" );
                items.Add( li );
            }

            return items;
        }

        /// <summary>
        /// Gets a list of ISecured entities (all models) that have not yet been registered and adds them
        /// as an entity type.
        /// </summary>
        /// <param name="physWebAppPath">the physical path of the web application</param>
        public void RegisterEntityTypes( string physWebAppPath )
        {
            var entityTypes = new Dictionary<string, EntityType>();

            foreach ( var type in Rock.Reflection.FindTypes( typeof( Rock.Data.IEntity ) ) )
            {
                var entityType = new EntityType();
                entityType.Name = type.Key;
                entityType.FriendlyName = type.Value.Name.SplitCase();
                entityType.AssemblyName = type.Value.AssemblyQualifiedName;
                entityType.IsEntity = true;
                entityType.IsSecured = false;
                entityTypes.Add( type.Key, entityType );
            }

            foreach ( var type in Rock.Reflection.FindTypes( typeof( Rock.Security.ISecured ) ) )
            {
                if ( entityTypes.ContainsKey( type.Key ) )
                {
                    entityTypes[type.Key].IsSecured = true;
                }
                else
                {
                    var entityType = new EntityType();
                    entityType.Name = type.Key;
                    entityType.FriendlyName = type.Value.Name.SplitCase();
                    entityType.AssemblyName = type.Value.AssemblyQualifiedName;
                    entityType.IsEntity = false;
                    entityType.IsSecured = true;
                    entityTypes.Add( type.Key, entityType );
                }
            }

            // Find any existing EntityTypes marked as an entity or secured that are no longer an entity or secured
            foreach ( var oldEntityType in Repository.AsQueryable()
                .Where( e => !entityTypes.Keys.Contains( e.Name ) && ( e.IsEntity || e.IsSecured ) )
                .ToList() )
            {
                oldEntityType.IsSecured = false;
                oldEntityType.IsEntity = false;
                oldEntityType.AssemblyName = null;
                Save( oldEntityType, null );
            }

            // Update any existing entities
            foreach ( var existingEntityType in Repository.AsQueryable()
                .Where( e => entityTypes.Keys.Contains( e.Name ) )
                .ToList() )
            {
                var entityType = entityTypes[existingEntityType.Name];

                if ( existingEntityType.IsEntity != entityType.IsEntity ||
                    existingEntityType.IsSecured != entityType.IsSecured ||
                    existingEntityType.FriendlyName != ( existingEntityType.FriendlyName ?? entityType.FriendlyName ) ||
                    existingEntityType.AssemblyName != entityType.AssemblyName )
                {
                    existingEntityType.IsEntity = entityType.IsEntity;
                    existingEntityType.IsSecured = entityType.IsSecured;
                    existingEntityType.FriendlyName = existingEntityType.FriendlyName ?? entityType.FriendlyName;
                    existingEntityType.AssemblyName = entityType.AssemblyName;
                    Save( existingEntityType, null );
                }
                entityTypes.Remove( entityType.Name );
            }

            // Add the newly discovered entities 
            foreach ( var entityTypeInfo in entityTypes )
            {
                // Don't add the EntityType entity as it will probably have been automatically 
                // added by the audit on a previous save in this method.
                if ( entityTypeInfo.Value.Name != "Rock.Model.EntityType" )
                {
                    this.Add( entityTypeInfo.Value, null );
                    this.Save( entityTypeInfo.Value, null );
                }
            }
        }
    }
}
