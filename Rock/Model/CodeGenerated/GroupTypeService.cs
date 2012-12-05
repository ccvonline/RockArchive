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
    /// GroupType Service class
    /// </summary>
    public partial class GroupTypeService : Service<GroupType, GroupTypeDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupTypeService"/> class
        /// </summary>
        public GroupTypeService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupTypeService"/> class
        /// </summary>
        public GroupTypeService(IRepository<GroupType> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override GroupType CreateNew()
        {
            return new GroupType();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<GroupTypeDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<GroupTypeDto> QueryableDto( IQueryable<GroupType> items )
        {
            return items.Select( m => new GroupTypeDto()
                {
                    IsSystem = m.IsSystem,
                    Name = m.Name,
                    Description = m.Description,
                    GroupTerm = m.GroupTerm,
                    GroupMemberTerm = m.GroupMemberTerm,
                    DefaultGroupRoleId = m.DefaultGroupRoleId,
                    AllowMultipleLocations = m.AllowMultipleLocations,
                    SmallIconFileId = m.SmallIconFileId,
                    LargeIconFileId = m.LargeIconFileId,
                    TakesAttendance = m.TakesAttendance,
                    AttendanceRule = m.AttendanceRule,
                    AttendancePrintTo = m.AttendancePrintTo,
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
        public bool CanDelete( GroupType item, out string errorMessage )
        {
            errorMessage = string.Empty;
            RockContext context = new RockContext();
            context.Database.Connection.Open();

            using ( var cmdCheckRef = context.Database.Connection.CreateCommand() )
            {
                cmdCheckRef.CommandText = string.Format( "select count(*) from Group where GroupTypeId = {0} ", item.Id );
                var result = cmdCheckRef.ExecuteScalar();
                int? refCount = result as int?;
                if ( refCount > 0 )
                {
                    Type entityType = RockContext.GetEntityFromTableName( "Group" );
                    string friendlyName = entityType != null ? entityType.GetFriendlyTypeName() : "Group";

                    errorMessage = string.Format("This {0} is assigned to a {1}.", GroupType.FriendlyTypeName, friendlyName);
                    return false;
                }
            }

            return true;
        }
    }
}
