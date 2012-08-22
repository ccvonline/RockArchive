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
	/// Member POCO Service class
	/// </summary>
    public partial class MemberService : Service<Member, MemberDTO>
    {
		/// <summary>
		/// Gets Members by Group Id
		/// </summary>
		/// <param name="groupId">Group Id.</param>
		/// <returns>An enumerable list of Member objects.</returns>
	    public IEnumerable<Member> GetByGroupId( int groupId )
        {
            return Repository.Find( t => t.GroupId == groupId );
        }
		
		/// <summary>
		/// Gets Member by Group Id And Person Id And Group Role Id
		/// </summary>
		/// <param name="groupId">Group Id.</param>
		/// <param name="personId">Person Id.</param>
		/// <param name="groupRoleId">Group Role Id.</param>
		/// <returns>Member object.</returns>
	    public Member GetByGroupIdAndPersonIdAndGroupRoleId( int groupId, int personId, int groupRoleId )
        {
            return Repository.FirstOrDefault( t => t.GroupId == groupId && t.PersonId == personId && t.GroupRoleId == groupRoleId );
        }
		
		/// <summary>
		/// Gets Members by Group Role Id
		/// </summary>
		/// <param name="groupRoleId">Group Role Id.</param>
		/// <returns>An enumerable list of Member objects.</returns>
	    public IEnumerable<Member> GetByGroupRoleId( int groupRoleId )
        {
            return Repository.Find( t => t.GroupRoleId == groupRoleId );
        }
		
		/// <summary>
		/// Gets Members by Person Id
		/// </summary>
		/// <param name="personId">Person Id.</param>
		/// <returns>An enumerable list of Member objects.</returns>
	    public IEnumerable<Member> GetByPersonId( int personId )
        {
            return Repository.Find( t => t.PersonId == personId );
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        /// <returns></returns>
        public override Member CreateNew()
        {
            return new Member();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of related DTO objects.</returns>
        public override IQueryable<MemberDTO> QueryableDTO()
        {
            return this.Queryable().Select( m => new MemberDTO( m ) );
        }
    }
}
