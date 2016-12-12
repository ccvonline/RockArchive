//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
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
    /// AnalyticsDimFinancialAccount Service class
    /// </summary>
    public partial class AnalyticsDimFinancialAccountService : Service<AnalyticsDimFinancialAccount>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsDimFinancialAccountService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public AnalyticsDimFinancialAccountService(RockContext context) : base(context)
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
        public bool CanDelete( AnalyticsDimFinancialAccount item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class AnalyticsDimFinancialAccountExtensionMethods
    {
        /// <summary>
        /// Clones this AnalyticsDimFinancialAccount object to a new AnalyticsDimFinancialAccount object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static AnalyticsDimFinancialAccount Clone( this AnalyticsDimFinancialAccount source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as AnalyticsDimFinancialAccount;
            }
            else
            {
                var target = new AnalyticsDimFinancialAccount();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another AnalyticsDimFinancialAccount object to this AnalyticsDimFinancialAccount object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this AnalyticsDimFinancialAccount target, AnalyticsDimFinancialAccount source )
        {
            target.Id = source.Id;
            target.AccountId = source.AccountId;
            target.AccountType = source.AccountType;
            target.ActiveStatus = source.ActiveStatus;
            target.Description = source.Description;
            target.EndDate = source.EndDate;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.GlCode = source.GlCode;
            target.ImageBinaryFileId = source.ImageBinaryFileId;
            target.ImageUrl = source.ImageUrl;
            target.Name = source.Name;
            target.Order = source.Order;
            target.ParentAccountId = source.ParentAccountId;
            target.PublicDescription = source.PublicDescription;
            target.PublicName = source.PublicName;
            target.PublicStatus = source.PublicStatus;
            target.StartDate = source.StartDate;
            target.TaxStatus = source.TaxStatus;
            target.Url = source.Url;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}
