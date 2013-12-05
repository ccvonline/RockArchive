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
    /// FinancialPersonBankAccount Service class
    /// </summary>
    public partial class FinancialPersonBankAccountService : Service<FinancialPersonBankAccount>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialPersonBankAccountService"/> class
        /// </summary>
        public FinancialPersonBankAccountService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialPersonBankAccountService"/> class
        /// </summary>
        /// <param name="repository">The repository.</param>
        public FinancialPersonBankAccountService(IRepository<FinancialPersonBankAccount> repository) : base(repository)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialPersonBankAccountService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public FinancialPersonBankAccountService(RockContext context) : base(context)
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
        public bool CanDelete( FinancialPersonBankAccount item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class FinancialPersonBankAccountExtensionMethods
    {
        /// <summary>
        /// Clones this FinancialPersonBankAccount object to a new FinancialPersonBankAccount object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static FinancialPersonBankAccount Clone( this FinancialPersonBankAccount source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as FinancialPersonBankAccount;
            }
            else
            {
                var target = new FinancialPersonBankAccount();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another FinancialPersonBankAccount object to this FinancialPersonBankAccount object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this FinancialPersonBankAccount target, FinancialPersonBankAccount source )
        {
            target.PersonId = source.PersonId;
            target.AccountNumberSecured = source.AccountNumberSecured;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
