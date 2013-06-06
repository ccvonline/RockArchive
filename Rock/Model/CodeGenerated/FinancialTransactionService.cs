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
    /// FinancialTransaction Service class
    /// </summary>
    public partial class FinancialTransactionService : Service<FinancialTransaction>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialTransactionService"/> class
        /// </summary>
        public FinancialTransactionService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialTransactionService"/> class
        /// </summary>
        public FinancialTransactionService(IRepository<FinancialTransaction> repository) : base(repository)
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
        public bool CanDelete( FinancialTransaction item, out string errorMessage )
        {
            errorMessage = string.Empty;
 
            if ( new Service<FinancialTransactionImage>().Queryable().Any( a => a.TransactionId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", FinancialTransaction.FriendlyTypeName, FinancialTransactionImage.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<FinancialTransactionRefund>().Queryable().Any( a => a.Id == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", FinancialTransaction.FriendlyTypeName, FinancialTransactionRefund.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class FinancialTransactionExtensionMethods
    {
        /// <summary>
        /// Clones this FinancialTransaction object to a new FinancialTransaction object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static FinancialTransaction Clone( this FinancialTransaction source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as FinancialTransaction;
            }
            else
            {
                var target = new FinancialTransaction();
                target.AuthorizedPersonId = source.AuthorizedPersonId;
                target.BatchId = source.BatchId;
                target.GatewayId = source.GatewayId;
                target.TransactionDateTime = source.TransactionDateTime;
                target.Amount = source.Amount;
                target.TransactionCode = source.TransactionCode;
                target.Summary = source.Summary;
                target.TransactionTypeValueId = source.TransactionTypeValueId;
                target.CurrencyTypeValueId = source.CurrencyTypeValueId;
                target.CreditCardTypeValueId = source.CreditCardTypeValueId;
                target.SourceTypeValueId = source.SourceTypeValueId;
                target.CheckMicrEncrypted = source.CheckMicrEncrypted;
                target.Id = source.Id;
                target.Guid = source.Guid;

            
                return target;
            }
        }
    }
}
