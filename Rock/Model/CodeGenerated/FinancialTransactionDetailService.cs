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
    /// FinancialTransactionDetail Service class
    /// </summary>
    public partial class FinancialTransactionDetailService : Service<FinancialTransactionDetail>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialTransactionDetailService"/> class
        /// </summary>
        public FinancialTransactionDetailService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialTransactionDetailService"/> class
        /// </summary>
        public FinancialTransactionDetailService(IRepository<FinancialTransactionDetail> repository) : base(repository)
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
        public bool CanDelete( FinancialTransactionDetail item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static class FinancialTransactionDetailExtensionMethods
    {
        /// <summary>
        /// Perform a shallow copy of this FinancialTransactionDetail to another
        /// </summary>
        public static void ShallowCopy( this FinancialTransactionDetail source, FinancialTransactionDetail target )
        {
            target.TransactionId = source.TransactionId;
            target.Entity = source.Entity;
            target.EntityId = source.EntityId;
            target.Amount = source.Amount;
            target.Summary = source.Summary;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
