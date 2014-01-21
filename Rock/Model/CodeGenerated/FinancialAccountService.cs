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
    /// FinancialAccount Service class
    /// </summary>
    public partial class FinancialAccountService : Service<FinancialAccount>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialAccountService"/> class
        /// </summary>
        public FinancialAccountService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialAccountService"/> class
        /// </summary>
        /// <param name="repository">The repository.</param>
        public FinancialAccountService(IRepository<FinancialAccount> repository) : base(repository)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialAccountService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public FinancialAccountService(RockContext context) : base(context)
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
        public bool CanDelete( FinancialAccount item, out string errorMessage )
        {
            errorMessage = string.Empty;
 
            if ( new Service<FinancialAccount>().Queryable().Any( a => a.ParentAccountId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", FinancialAccount.FriendlyTypeName, FinancialAccount.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<FinancialPledge>().Queryable().Any( a => a.AccountId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", FinancialAccount.FriendlyTypeName, FinancialPledge.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<FinancialScheduledTransactionDetail>().Queryable().Any( a => a.AccountId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", FinancialAccount.FriendlyTypeName, FinancialScheduledTransactionDetail.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<FinancialTransactionDetail>().Queryable().Any( a => a.AccountId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", FinancialAccount.FriendlyTypeName, FinancialTransactionDetail.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class FinancialAccountExtensionMethods
    {
        /// <summary>
        /// Clones this FinancialAccount object to a new FinancialAccount object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static FinancialAccount Clone( this FinancialAccount source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as FinancialAccount;
            }
            else
            {
                var target = new FinancialAccount();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another FinancialAccount object to this FinancialAccount object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this FinancialAccount target, FinancialAccount source )
        {
            target.ParentAccountId = source.ParentAccountId;
            target.CampusId = source.CampusId;
            target.Name = source.Name;
            target.PublicName = source.PublicName;
            target.Description = source.Description;
            target.IsTaxDeductible = source.IsTaxDeductible;
            target.GlCode = source.GlCode;
            target.Order = source.Order;
            target.IsActive = source.IsActive;
            target.StartDate = source.StartDate;
            target.EndDate = source.EndDate;
            target.AccountTypeValueId = source.AccountTypeValueId;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
