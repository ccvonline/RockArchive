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
    /// GroupRole Service class
    /// </summary>
    public partial class GroupRoleService : Service<GroupRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupRoleService"/> class
        /// </summary>
        public GroupRoleService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupRoleService"/> class
        /// </summary>
        public GroupRoleService(IRepository<GroupRole> repository) : base(repository)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( GroupRole item, out string errorMessage )
        {
            errorMessage = string.Empty;
 
            if ( new Service<GroupMember>().Queryable().Any( a => a.GroupRoleId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", GroupRole.FriendlyTypeName, GroupMember.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<GroupType>().Queryable().Any( a => a.DefaultGroupRoleId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", GroupRole.FriendlyTypeName, GroupType.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static class GroupRoleExtensionMethods
    {
        /// <summary>
        /// Copies all the entity properties from another GroupRole entity
        /// </summary>
        public static void CopyPropertiesFrom( this GroupRole target, GroupRole source )
        {
            target.IsSystem = source.IsSystem;
            target.GroupTypeId = source.GroupTypeId;
            target.Name = source.Name;
            target.Description = source.Description;
            target.SortOrder = source.SortOrder;
            target.MaxCount = source.MaxCount;
            target.MinCount = source.MinCount;
            target.IsLeader = source.IsLeader;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
