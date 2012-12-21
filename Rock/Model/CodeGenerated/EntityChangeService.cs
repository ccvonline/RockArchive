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
    /// EntityChange Service class
    /// </summary>
    public partial class EntityChangeService : Service<EntityChange>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityChangeService"/> class
        /// </summary>
        public EntityChangeService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityChangeService"/> class
        /// </summary>
        public EntityChangeService(IRepository<EntityChange> repository) : base(repository)
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
        public bool CanDelete( EntityChange item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static class EntityChangeExtensionMethods
    {
        /// <summary>
        /// Perform a shallow copy of this EntityChange to another
        /// </summary>
        public static void ShallowCopy( this EntityChange source, EntityChange target )
        {
            target.ChangeSet = source.ChangeSet;
            target.ChangeType = source.ChangeType;
            target.EntityTypeId = source.EntityTypeId;
            target.EntityId = source.EntityId;
            target.Property = source.Property;
            target.OriginalValue = source.OriginalValue;
            target.CurrentValue = source.CurrentValue;
            target.CreatedDateTime = source.CreatedDateTime;
            target.CreatedByPersonId = source.CreatedByPersonId;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
