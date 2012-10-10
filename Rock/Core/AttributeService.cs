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
    /// Attribute Service class
    /// </summary>
    public partial class AttributeService : Service<Attribute, AttributeDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeService"/> class
        /// </summary>
        public AttributeService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeService"/> class
        /// </summary>
        public AttributeService(IRepository<Attribute> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override Attribute CreateNew()
        {
            return new Attribute();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<AttributeDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<AttributeDto> QueryableDto( IQueryable<Attribute> items )
        {
            return items.Select( m => new AttributeDto()
                {
                    IsSystem = m.IsSystem,
                    FieldTypeId = m.FieldTypeId,
                    Entity = m.Entity,
                    EntityQualifierColumn = m.EntityQualifierColumn,
                    EntityQualifierValue = m.EntityQualifierValue,
                    Key = m.Key,
                    Name = m.Name,
                    Category = m.Category,
                    Description = m.Description,
                    Order = m.Order,
                    IsGridColumn = m.IsGridColumn,
                    DefaultValue = m.DefaultValue,
                    IsMultiValue = m.IsMultiValue,
                    IsRequired = m.IsRequired,
                    Id = m.Id,
                    Guid = m.Guid,
                });
        }
    }
}
