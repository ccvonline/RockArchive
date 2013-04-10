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
    public partial class EntityTypeService : Service<EntityType>
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
 
            if ( new Service<Attribute>().Queryable().Any( a => a.EntityTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, Attribute.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Audit>().Queryable().Any( a => a.EntityTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, Audit.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Auth>().Queryable().Any( a => a.EntityTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, Auth.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Category>().Queryable().Any( a => a.EntityTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, Category.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<DataView>().Queryable().Any( a => a.EntityTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, DataView.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<DataViewFilter>().Queryable().Any( a => a.EntityTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, DataViewFilter.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<FinancialTransaction>().Queryable().Any( a => a.EntityTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, FinancialTransaction.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<NoteType>().Queryable().Any( a => a.EntityTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, NoteType.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Tag>().Queryable().Any( a => a.EntityTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, Tag.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<WorkflowTrigger>().Queryable().Any( a => a.EntityTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", EntityType.FriendlyTypeName, WorkflowTrigger.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static class EntityTypeExtensionMethods
    {
        /// <summary>
        /// Clones this EntityType object to a new EntityType object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static EntityType Clone( this EntityType source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as EntityType;
            }
            else
            {
                var target = new EntityType();
                target.Name = source.Name;
                target.AssemblyName = source.AssemblyName;
                target.FriendlyName = source.FriendlyName;
                target.IsEntity = source.IsEntity;
                target.IsSecured = source.IsSecured;
                target.Id = source.Id;
                target.Guid = source.Guid;

            
                return target;
            }
        }
    }
}
