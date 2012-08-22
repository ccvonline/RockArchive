//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;

using Rock.Data;

namespace Rock.Core
{
	/// <summary>
	/// Attribute Qualifier POCO Service class
	/// </summary>
    public partial class AttributeQualifierService : Service<AttributeQualifier, AttributeQualifierDTO>
    {
		/// <summary>
		/// Gets Attribute Qualifiers by Attribute Id
		/// </summary>
		/// <param name="attributeId">Attribute Id.</param>
		/// <returns>An enumerable list of AttributeQualifier objects.</returns>
	    public IEnumerable<AttributeQualifier> GetByAttributeId( int attributeId )
        {
            return Repository.Find( t => t.AttributeId == attributeId );
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        /// <returns></returns>
        public override AttributeQualifier CreateNew()
        {
            return new AttributeQualifier();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of related DTO objects.</returns>
        public override IQueryable<AttributeQualifierDTO> QueryableDTO()
        {
            return this.Queryable().Select( m => new AttributeQualifierDTO( m ) );
        }
    }
}
