//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright by the Spark Development Network
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
    /// ConnectionOpportunityCampus Service class
    /// </summary>
    public partial class ConnectionOpportunityCampusService : Service<ConnectionOpportunityCampus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionOpportunityCampusService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public ConnectionOpportunityCampusService(RockContext context) : base(context)
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
        public bool CanDelete( ConnectionOpportunityCampus item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class ConnectionOpportunityCampusExtensionMethods
    {
        /// <summary>
        /// Clones this ConnectionOpportunityCampus object to a new ConnectionOpportunityCampus object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static ConnectionOpportunityCampus Clone( this ConnectionOpportunityCampus source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as ConnectionOpportunityCampus;
            }
            else
            {
                var target = new ConnectionOpportunityCampus();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another ConnectionOpportunityCampus object to this ConnectionOpportunityCampus object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this ConnectionOpportunityCampus target, ConnectionOpportunityCampus source )
        {
            target.Id = source.Id;
            target.CampusId = source.CampusId;
            target.ConnectionOpportunityId = source.ConnectionOpportunityId;
            target.DefaultConnectorPersonAliasId = source.DefaultConnectorPersonAliasId;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}
