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
	/// Entity Change Service class
	/// </summary>
    public partial class EntityChangeService : Service<EntityChange, EntityChangeDTO>
    {
		/// <summary>
		/// Gets Entity Changes by Change Set
		/// </summary>
		/// <param name="changeSet">Change Set.</param>
		/// <returns>An enumerable list of EntityChange objects.</returns>
	    public IEnumerable<EntityChange> GetByChangeSet( Guid changeSet )
        {
            return Repository.Find( t => t.ChangeSet == changeSet );
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        /// <returns></returns>
        public override EntityChange CreateNew()
        {
            return new EntityChange();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of related DTO objects.</returns>
        public override IQueryable<EntityChangeDTO> QueryableDTO()
        {
            return this.Queryable().Select( m => new EntityChangeDTO( m ) );
        }
    }
}
