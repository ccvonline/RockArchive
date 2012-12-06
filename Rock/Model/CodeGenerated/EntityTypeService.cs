//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// EntityType Service class
    /// </summary>
    public partial class EntityTypeService : Service<EntityType, EntityTypeDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTypeService"/> class
        /// </summary>
        public EntityTypeService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTypeService"/> class
        /// </summary>
        public EntityTypeService(IRepository<EntityType> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override EntityType CreateNew()
        {
            return new EntityType();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<EntityTypeDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<EntityTypeDto> QueryableDto( IQueryable<EntityType> items )
        {
            return items.Select( m => new EntityTypeDto()
                {
                    Name = m.Name,
                    AssemblyName = m.AssemblyName,
                    FriendlyName = m.FriendlyName,
                    IsEntity = m.IsEntity,
                    IsSecured = m.IsSecured,
                    Id = m.Id,
                    Guid = m.Guid,
                });
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( EntityType item, out string errorMessage )
        {
            errorMessage = string.Empty;
            RockContext context = new RockContext();
            context.Database.Connection.Open();

            using ( var cmdCheckRef = context.Database.Connection.CreateCommand() )
            {
                cmdCheckRef.CommandText = string.Format( "select count(*) from Attribute where EntityTypeId = {0} ", item.Id );
                var result = cmdCheckRef.ExecuteScalar();
                int? refCount = result as int?;
                if ( refCount > 0 )
                {
                    Type entityType = RockContext.GetEntityFromTableName( "Attribute" );
                    string friendlyName = entityType != null ? entityType.GetFriendlyTypeName() : "Attribute";

                    errorMessage = string.Format("This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, friendlyName);
                    return false;
                }
            }

            using ( var cmdCheckRef = context.Database.Connection.CreateCommand() )
            {
                cmdCheckRef.CommandText = string.Format( "select count(*) from Audit where EntityTypeId = {0} ", item.Id );
                var result = cmdCheckRef.ExecuteScalar();
                int? refCount = result as int?;
                if ( refCount > 0 )
                {
                    Type entityType = RockContext.GetEntityFromTableName( "Audit" );
                    string friendlyName = entityType != null ? entityType.GetFriendlyTypeName() : "Audit";

                    errorMessage = string.Format("This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, friendlyName);
                    return false;
                }
            }

            using ( var cmdCheckRef = context.Database.Connection.CreateCommand() )
            {
                cmdCheckRef.CommandText = string.Format( "select count(*) from Auth where EntityTypeId = {0} ", item.Id );
                var result = cmdCheckRef.ExecuteScalar();
                int? refCount = result as int?;
                if ( refCount > 0 )
                {
                    Type entityType = RockContext.GetEntityFromTableName( "Auth" );
                    string friendlyName = entityType != null ? entityType.GetFriendlyTypeName() : "Auth";

                    errorMessage = string.Format("This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, friendlyName);
                    return false;
                }
            }

            using ( var cmdCheckRef = context.Database.Connection.CreateCommand() )
            {
                cmdCheckRef.CommandText = string.Format( "select count(*) from Category where EntityTypeId = {0} ", item.Id );
                var result = cmdCheckRef.ExecuteScalar();
                int? refCount = result as int?;
                if ( refCount > 0 )
                {
                    Type entityType = RockContext.GetEntityFromTableName( "Category" );
                    string friendlyName = entityType != null ? entityType.GetFriendlyTypeName() : "Category";

                    errorMessage = string.Format("This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, friendlyName);
                    return false;
                }
            }

            using ( var cmdCheckRef = context.Database.Connection.CreateCommand() )
            {
                cmdCheckRef.CommandText = string.Format( "select count(*) from EntityChange where EntityTypeId = {0} ", item.Id );
                var result = cmdCheckRef.ExecuteScalar();
                int? refCount = result as int?;
                if ( refCount > 0 )
                {
                    Type entityType = RockContext.GetEntityFromTableName( "EntityChange" );
                    string friendlyName = entityType != null ? entityType.GetFriendlyTypeName() : "EntityChange";

                    errorMessage = string.Format("This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, friendlyName);
                    return false;
                }
            }

            using ( var cmdCheckRef = context.Database.Connection.CreateCommand() )
            {
                cmdCheckRef.CommandText = string.Format( "select count(*) from Tag where EntityTypeId = {0} ", item.Id );
                var result = cmdCheckRef.ExecuteScalar();
                int? refCount = result as int?;
                if ( refCount > 0 )
                {
                    Type entityType = RockContext.GetEntityFromTableName( "Tag" );
                    string friendlyName = entityType != null ? entityType.GetFriendlyTypeName() : "Tag";

                    errorMessage = string.Format("This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, friendlyName);
                    return false;
                }
            }

            using ( var cmdCheckRef = context.Database.Connection.CreateCommand() )
            {
                cmdCheckRef.CommandText = string.Format( "select count(*) from WorkflowTrigger where EntityTypeId = {0} ", item.Id );
                var result = cmdCheckRef.ExecuteScalar();
                int? refCount = result as int?;
                if ( refCount > 0 )
                {
                    Type entityType = RockContext.GetEntityFromTableName( "WorkflowTrigger" );
                    string friendlyName = entityType != null ? entityType.GetFriendlyTypeName() : "WorkflowTrigger";

                    errorMessage = string.Format("This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, friendlyName);
                    return false;
                }
            }

            return true;
        }
    }
}
