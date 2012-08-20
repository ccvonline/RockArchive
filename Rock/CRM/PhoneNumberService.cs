//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.Collections.Generic;
using System.Linq;

using Rock.Data;

namespace Rock.CRM
{
	/// <summary>
	/// Phone Number POCO Service class
	/// </summary>
    public partial class PhoneNumberService : Service<PhoneNumber, DTO.PhoneNumber>
    {
		/// <summary>
		/// Gets Phone Numbers by Person Id
		/// </summary>
		/// <param name="personId">Person Id.</param>
		/// <returns>An enumerable list of PhoneNumber objects.</returns>
	    public IEnumerable<PhoneNumber> GetByPersonId( int personId )
        {
            return Repository.Find( t => t.PersonId == personId );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of related DTO objects.</returns>
        public override IQueryable<DTO.PhoneNumber> QueryableDTO()
        {
            throw new System.NotImplementedException();
        }
    }
}
