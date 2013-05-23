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
    /// FinancialPledge Service class
    /// </summary>
    public partial class FinancialPledgeService : Service<FinancialPledge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialPledgeService"/> class
        /// </summary>
        public FinancialPledgeService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialPledgeService"/> class
        /// </summary>
        public FinancialPledgeService(IRepository<FinancialPledge> repository) : base(repository)
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
        public bool CanDelete( FinancialPledge item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class FinancialPledgeExtensionMethods
    {
        /// <summary>
        /// Clones this FinancialPledge object to a new FinancialPledge object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static FinancialPledge Clone( this FinancialPledge source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as FinancialPledge;
            }
            else
            {
                var target = new FinancialPledge();
                target.PersonId = source.PersonId;
                target.AccountId = source.AccountId;
                target.TotalAmount = source.TotalAmount;
                target.PledgeFrequencyValueId = source.PledgeFrequencyValueId;
                target.StartDate = source.StartDate;
                target.EndDate = source.EndDate;
                target.Id = source.Id;
                target.Guid = source.Guid;

            
                return target;
            }
        }
    }
}
