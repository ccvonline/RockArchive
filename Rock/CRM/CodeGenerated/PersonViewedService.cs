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
    /// PersonViewed Service class
    /// </summary>
    public partial class PersonViewedService : Service<PersonViewed, PersonViewedDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonViewedService"/> class
        /// </summary>
        public PersonViewedService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonViewedService"/> class
        /// </summary>
        public PersonViewedService(IRepository<PersonViewed> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override PersonViewed CreateNew()
        {
            return new PersonViewed();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<PersonViewedDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<PersonViewedDto> QueryableDto( IQueryable<PersonViewed> items )
        {
            return items.Select( m => new PersonViewedDto()
                {
                    ViewerPersonId = m.ViewerPersonId,
                    TargetPersonId = m.TargetPersonId,
                    ViewDateTime = m.ViewDateTime,
                    IpAddress = m.IpAddress,
                    Source = m.Source,
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
        public bool CanDelete( PersonViewed item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }
}
