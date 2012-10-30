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
    /// Tag Service class
    /// </summary>
    public partial class TagService : Service<Tag, TagDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagService"/> class
        /// </summary>
        public TagService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagService"/> class
        /// </summary>
        public TagService(IRepository<Tag> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override Tag CreateNew()
        {
            return new Tag();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<TagDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<TagDto> QueryableDto( IQueryable<Tag> items )
        {
            return items.Select( m => new TagDto()
                {
                    IsSystem = m.IsSystem,
                    EntityTypeId = m.EntityTypeId,
                    EntityQualifierColumn = m.EntityQualifierColumn,
                    EntityQualifierValue = m.EntityQualifierValue,
                    Name = m.Name,
                    Order = m.Order,
                    OwnerId = m.OwnerId,
                    Id = m.Id,
                    Guid = m.Guid,
                });
        }
    }
}
