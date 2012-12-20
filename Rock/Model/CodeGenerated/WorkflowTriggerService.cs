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
    /// WorkflowTrigger Service class
    /// </summary>
    public partial class WorkflowTriggerService : Service<WorkflowTrigger>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowTriggerService"/> class
        /// </summary>
        public WorkflowTriggerService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowTriggerService"/> class
        /// </summary>
        public WorkflowTriggerService(IRepository<WorkflowTrigger> repository) : base(repository)
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
        public bool CanDelete( WorkflowTrigger item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static class WorkflowTriggerExtensionMethods
    {
        /// <summary>
        /// Perform a shallow copy of this WorkflowTrigger to another
        /// </summary>
        public static void ShallowCopy( this WorkflowTrigger source, WorkflowTrigger target )
        {
            target.IsSystem = source.IsSystem;
            target.EntityTypeId = source.EntityTypeId;
            target.EntityTypeQualifierColumn = source.EntityTypeQualifierColumn;
            target.EntityTypeQualifierValue = source.EntityTypeQualifierValue;
            target.WorkflowTypeId = source.WorkflowTypeId;
            target.WorkflowTriggerType = source.WorkflowTriggerType;
            target.WorkflowName = source.WorkflowName;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
