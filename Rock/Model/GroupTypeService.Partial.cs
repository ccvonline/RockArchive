//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.Collections.Generic;
using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Data access/service class for <see cref="Rock.Model.GroupType"/> entity objects. This class extends <see cref="Rock.Data.Service"/>.
    /// </summary>
    public partial class GroupTypeService 
    {

        /// <summary>
        /// Returns an enumerable collection of <see cref="Rock.Model.GroupType"/> entities by the Id of their <see cref="Rock.Model.GroupRole."/>.
        /// </summary>
        /// <param name="defaultGroupRoleId">An <see cref="System.Int32"/> representing the Id of the <see cref="Rock.Model.GroupRole"/> to search by.</param>
        /// <returns>An enumerable collection of <see cref="Rock.Model.GroupType">GroupTypes</see> that use the provided <see cref="Rock.Model.GroupRole"/> as the 
        /// default GroupRole for their member Groups.</returns>
        public IEnumerable<GroupType> GetByDefaultGroupRoleId( int? defaultGroupRoleId )
        {
            return Repository.Find( t => ( t.DefaultGroupRoleId == defaultGroupRoleId || ( defaultGroupRoleId == null && t.DefaultGroupRoleId == null ) ) );
        }

        /// <summary>
        /// Verifies if the specified <see cref="Rock.MOdel.GroupType"/> can be deleted, and if so deletes it.
        /// </summary>
        /// <param name="item">The <see cref="Rock.Model.GroupType"/> to delete.</param>
        /// <param name="personId">A <see cref="System.Int32"/> representing the Id of the <see cref="Rock.Model.Person"/> who is attempting to delete the
        /// <see cref="Rock.Model.GroupType"/>.</param>
        /// <returns>A <see cref="System.Boolean"/> value that is <c>true</c> if the <see cref="Rock.Model.GroupType"/> was able to be successfully deleted, otherwise <c>false</c>.</returns>
        public override bool Delete( GroupType item, int? personId )
        {
            string message;
            if ( !CanDelete( item, out message ) )
            {
                return false;
            }

            item.ChildGroupTypes.Clear();
            this.Save( item, personId );

            return base.Delete( item, personId );
        }
    }
}
