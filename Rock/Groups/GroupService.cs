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

namespace Rock.Groups
    
    /// <summary>
    /// Group Service class
    /// </summary>
    public partial class GroupService : Service<Group, GroupDto>
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupService"/> class
        /// </summary>
        public GroupService()
            : base()
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupService"/> class
        /// </summary>
        public GroupService(IRepository<Group> repository) : base(repository)
            
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override Group CreateNew()
            
            return new Group();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<GroupDto> QueryableDto( )
            
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<GroupDto> QueryableDto( IQueryable<Group> items )
            
            return items.Select( m => new GroupDto()
                    
                    IsSystem = m.IsSystem,
                    ParentGroupId = m.ParentGroupId,
                    GroupTypeId = m.GroupTypeId,
                    CampusId = m.CampusId,
                    Name = m.Name,
                    Description = m.Description,
                    IsSecurityRole = m.IsSecurityRole,
                    Id = m.Id,
                    Guid = m.Guid,
                });
        }
    }
}
