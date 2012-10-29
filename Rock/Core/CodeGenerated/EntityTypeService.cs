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

namespace Rock.Core
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
                    FriendlyName = m.FriendlyName,
                    IsModel = m.IsModel,
                    Id = m.Id,
                    Guid = m.Guid,
                });
        }
    }
}
