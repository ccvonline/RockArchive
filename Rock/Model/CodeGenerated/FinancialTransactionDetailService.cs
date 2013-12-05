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
        /// <param name="repository">The repository.</param>
        public FinancialTransactionDetailService(IRepository<FinancialTransactionDetail> repository) : base(repository)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialTransactionDetailService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public FinancialTransactionDetailService(RockContext context) : base(context)
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
    public static partial class FinancialTransactionDetailExtensionMethods
    {
        /// <summary>
        /// Clones this FinancialTransactionDetail object to a new FinancialTransactionDetail object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static FinancialTransactionDetail Clone( this FinancialTransactionDetail source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as FinancialTransactionDetail;
            }
            else
            {
                var target = new FinancialTransactionDetail();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another FinancialTransactionDetail object to this FinancialTransactionDetail object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this FinancialTransactionDetail target, FinancialTransactionDetail source )
        {
            target.TransactionId = source.TransactionId;
            target.AccountId = source.AccountId;
            target.Amount = source.Amount;
            target.Summary = source.Summary;
            target.EntityTypeId = source.EntityTypeId;
            target.EntityId = source.EntityId;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
