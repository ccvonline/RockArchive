//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;

using Rock.Data;

namespace Rock.Groups
{
	/// <summary>
	/// Group Type POCO Service class
	/// </summary>
    public partial class GroupTypeService : Service<GroupType, GroupTypeDTO>
    {
		/// <summary>
		/// Gets Group Types by Default Group Role Id
		/// </summary>
		/// <param name="defaultGroupRoleId">Default Group Role Id.</param>
		/// <returns>An enumerable list of GroupType objects.</returns>
	    public IEnumerable<GroupType> GetByDefaultGroupRoleId( int? defaultGroupRoleId )
        {
            return Repository.Find( t => ( t.DefaultGroupRoleId == defaultGroupRoleId || ( defaultGroupRoleId == null && t.DefaultGroupRoleId == null ) ) );
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        /// <returns></returns>
        public override GroupType CreateNew()
        {
            return new GroupType();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of related DTO objects.</returns>
        public override IQueryable<GroupTypeDTO> QueryableDTO()
        {
            return this.Queryable().Select( m => new GroupTypeDTO( m ) );
        }
    }
}
