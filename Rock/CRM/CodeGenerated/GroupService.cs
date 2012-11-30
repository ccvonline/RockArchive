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
    /// Group Service class
    /// </summary>
    public partial class GroupService : Service<Group, GroupDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupService"/> class
        /// </summary>
        public GroupService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupService"/> class
        /// </summary>
        public GroupService(IRepository<Group> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override Group CreateNew()
        {
            return new Group();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<GroupDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<GroupDto> QueryableDto( IQueryable<Group> items )
        {
            return items.Select( m => new GroupDto()
                {
                    IsSystem = m.IsSystem,
                    ParentGroupId = m.ParentGroupId,
                    GroupTypeId = m.GroupTypeId,
                    CampusId = m.CampusId,
                    Name = m.Name,
                    Description = m.Description,
                    IsSecurityRole = m.IsSecurityRole,
                    IsActive = m.IsActive,
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
        public bool CanDelete( Group item, out string errorMessage )
        {
            errorMessage = string.Empty;
            RockContext context = new RockContext();
            context.Database.Connection.Open();

            using ( var cmdCheckRef = context.Database.Connection.CreateCommand() )
            {
                cmdCheckRef.CommandText = string.Format( "select count(*) from Group where ParentGroupId = {0} ", item.Id );
                var result = cmdCheckRef.ExecuteScalar();
                int? refCount = result as int?;
                if ( refCount > 0 )
                {
                    Type entityType = RockContext.GetEntityFromTableName( "Group" );
                    string friendlyName = entityType != null ? entityType.GetFriendlyTypeName() : "Group";

                    errorMessage = string.Format("This {0} is assigned to a {1}.", Group.FriendlyTypeName, friendlyName);
                    return false;
                }
            }

            using ( var cmdCheckRef = context.Database.Connection.CreateCommand() )
            {
                cmdCheckRef.CommandText = string.Format( "select count(*) from MarketingCampaign where EventGroupId = {0} ", item.Id );
                var result = cmdCheckRef.ExecuteScalar();
                int? refCount = result as int?;
                if ( refCount > 0 )
                {
                    Type entityType = RockContext.GetEntityFromTableName( "MarketingCampaign" );
                    string friendlyName = entityType != null ? entityType.GetFriendlyTypeName() : "MarketingCampaign";

                    errorMessage = string.Format("This {0} is assigned to a {1}.", Group.FriendlyTypeName, friendlyName);
                    return false;
                }
            }

            return true;
        }
    }
}
