//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Workflow Service class
    /// </summary>
    public partial class WorkflowService : Service<Workflow>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public WorkflowService(RockContext context) : base(context)
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
        public bool CanDelete( Workflow item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class WorkflowExtensionMethods
    {
        /// <summary>
        /// Clones this Workflow object to a new Workflow object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static Workflow Clone( this Workflow source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as Workflow;
            }
            else
            {
                var target = new Workflow();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another Workflow object to this Workflow object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this Workflow target, Workflow source )
        {
            target.WorkflowTypeId = source.WorkflowTypeId;
            target.Name = source.Name;
            target.Description = source.Description;
            target.Status = source.Status;
            target.IsProcessing = source.IsProcessing;
            target.ActivatedDateTime = source.ActivatedDateTime;
            target.LastProcessedDateTime = source.LastProcessedDateTime;
            target.CompletedDateTime = source.CompletedDateTime;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Id = source.Id;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}
