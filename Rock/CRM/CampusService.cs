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

namespace Rock.Crm
{
    /// <summary>
    /// Campus Service class
    /// </summary>
    public partial class CampusService : Service<Campus, CampusDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampusService"/> class
        /// </summary>
        public CampusService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CampusService"/> class
        /// </summary>
        public CampusService(IRepository<Campus> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override Campus CreateNew()
        {
            return new Campus();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<CampusDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public IQueryable<CampusDto> QueryableDto( IQueryable<Campus> items )
		{
			return items.Select( m => new CampusDto()
				{
					IsSystem = m.IsSystem,
					Name = m.Name,
					Id = m.Id,
					Guid = m.Guid,
				});
		}
	}
}
