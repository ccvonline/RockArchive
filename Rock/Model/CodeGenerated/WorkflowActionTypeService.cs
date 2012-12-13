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
    /// WorkflowActionType Service class
    /// </summary>
    public partial class WorkflowActionTypeService : Service<WorkflowActionType, WorkflowActionTypeDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowActionTypeService"/> class
        /// </summary>
        public WorkflowActionTypeService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowActionTypeService"/> class
        /// </summary>
        public WorkflowActionTypeService(IRepository<WorkflowActionType> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override WorkflowActionType CreateNew()
        {
            return new WorkflowActionType();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<WorkflowActionTypeDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<WorkflowActionTypeDto> QueryableDto( IQueryable<WorkflowActionType> items )
        {
            return items.Select( m => new WorkflowActionTypeDto()
                {
                    ActivityTypeId = m.ActivityTypeId,
                    Name = m.Name,
                    Order = m.Order,
                    EntityTypeId = m.EntityTypeId,
                    IsActionCompletedOnSuccess = m.IsActionCompletedOnSuccess,
                    IsActivityCompletedOnSuccess = m.IsActivityCompletedOnSuccess,
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
        public bool CanDelete( WorkflowActionType item, out string errorMessage )
        {
            errorMessage = string.Empty;
 
            if ( new Service<WorkflowAction>().Queryable().Any( a => a.ActionTypeId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", WorkflowActionType.FriendlyTypeName, WorkflowAction.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }
}
