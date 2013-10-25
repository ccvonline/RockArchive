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
    /// DefinedValue Service class
    /// </summary>
    public partial class DefinedValueService : Service<DefinedValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinedValueService"/> class
        /// </summary>
        public DefinedValueService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinedValueService"/> class
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefinedValueService(IRepository<DefinedValue> repository) : base(repository)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinedValueService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public DefinedValueService(RockContext context) : base(context)
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
        public bool CanDelete( DefinedValue item, out string errorMessage )
        {
            errorMessage = string.Empty;
            
            // ignoring Attendance,QualifierValueId 
            
            // ignoring Attendance,SearchTypeValueId 
 
            if ( new Service<Device>().Queryable().Any( a => a.DeviceTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Device.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<FinancialAccount>().Queryable().Any( a => a.AccountTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialAccount.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<FinancialPledge>().Queryable().Any( a => a.PledgeFrequencyValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialPledge.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<FinancialScheduledTransaction>().Queryable().Any( a => a.TransactionFrequencyValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialScheduledTransaction.FriendlyTypeName );
                return false;
            }  
            
            // ignoring FinancialTransaction,CreditCardTypeValueId 
            
            // ignoring FinancialTransaction,CurrencyTypeValueId 
            
            // ignoring FinancialTransaction,SourceTypeValueId 
            
            // ignoring FinancialTransaction,TransactionTypeValueId 
 
            if ( new Service<FinancialTransactionImage>().Queryable().Any( a => a.TransactionImageTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialTransactionImage.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<FinancialTransactionRefund>().Queryable().Any( a => a.RefundReasonValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, FinancialTransactionRefund.FriendlyTypeName );
                return false;
            }  
            
            // ignoring GroupLocation,GroupLocationTypeValueId 
 
            if ( new Service<GroupType>().Queryable().Any( a => a.GroupTypePurposeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, GroupType.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Location>().Queryable().Any( a => a.LocationTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Location.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Metric>().Queryable().Any( a => a.CollectionFrequencyValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Metric.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Note>().Queryable().Any( a => a.SourceTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, Note.FriendlyTypeName );
                return false;
            }  
            
            // ignoring Person,MaritalStatusValueId 
            
            // ignoring Person,PersonStatusValueId 
            
            // ignoring Person,RecordStatusReasonValueId 
            
            // ignoring Person,RecordStatusValueId 
            
            // ignoring Person,RecordTypeValueId 
            
            // ignoring Person,SuffixValueId 
            
            // ignoring Person,TitleValueId 
 
            if ( new Service<PhoneNumber>().Queryable().Any( a => a.NumberTypeValueId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", DefinedValue.FriendlyTypeName, PhoneNumber.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class DefinedValueExtensionMethods
    {
        /// <summary>
        /// Clones this DefinedValue object to a new DefinedValue object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static DefinedValue Clone( this DefinedValue source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as DefinedValue;
            }
            else
            {
                var target = new DefinedValue();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another DefinedValue object to this DefinedValue object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this DefinedValue target, DefinedValue source )
        {
            target.IsSystem = source.IsSystem;
            target.DefinedTypeId = source.DefinedTypeId;
            target.Order = source.Order;
            target.Name = source.Name;
            target.Description = source.Description;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
