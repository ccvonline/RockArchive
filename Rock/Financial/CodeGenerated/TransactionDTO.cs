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
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Financial
{
    /// <summary>
    /// Data Transfer Object for Transaction object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class TransactionDto : IDto
    {
        /// <summary />
        [DataMember]
        public string Description { get; set; }

        /// <summary />
        [DataMember]
        public DateTime? TransactionDate { get; set; }

        /// <summary />
        [DataMember]
        public string Entity { get; set; }

        /// <summary />
        [DataMember]
        public int? EntityId { get; set; }

        /// <summary />
        [DataMember]
        public int? BatchId { get; set; }

        /// <summary />
        [DataMember]
        public int? CurrencyTypeId { get; set; }

        /// <summary />
        [DataMember]
        public int? CreditCardTypeId { get; set; }

        /// <summary />
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary />
        [DataMember]
        public int? RefundTransactionId { get; set; }

        /// <summary />
        [DataMember]
        public int? TransactionImageId { get; set; }

        /// <summary />
        [DataMember]
        public string TransactionCode { get; set; }

        /// <summary />
        [DataMember]
        public int? GatewayId { get; set; }

        /// <summary />
        [DataMember]
        public int? SourceTypeId { get; set; }

        /// <summary />
        [DataMember]
        public string Summary { get; set; }

        /// <summary />
        [DataMember]
        public int Id { get; set; }

        /// <summary />
        [DataMember]
        public Guid Guid { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public TransactionDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="transaction"></param>
        public TransactionDto ( Transaction transaction )
        {
            CopyFromModel( transaction );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add( "Description", this.Description );
            dictionary.Add( "TransactionDate", this.TransactionDate );
            dictionary.Add( "Entity", this.Entity );
            dictionary.Add( "EntityId", this.EntityId );
            dictionary.Add( "BatchId", this.BatchId );
            dictionary.Add( "CurrencyTypeId", this.CurrencyTypeId );
            dictionary.Add( "CreditCardTypeId", this.CreditCardTypeId );
            dictionary.Add( "Amount", this.Amount );
            dictionary.Add( "RefundTransactionId", this.RefundTransactionId );
            dictionary.Add( "TransactionImageId", this.TransactionImageId );
            dictionary.Add( "TransactionCode", this.TransactionCode );
            dictionary.Add( "GatewayId", this.GatewayId );
            dictionary.Add( "SourceTypeId", this.SourceTypeId );
            dictionary.Add( "Summary", this.Summary );
            dictionary.Add( "Id", this.Id );
            dictionary.Add( "Guid", this.Guid );
            return dictionary;
        }

        /// <summary>
        /// Creates a dynamic object.
        /// </summary>
        /// <returns></returns>
        public virtual dynamic ToDynamic()
        {
            dynamic expando = new ExpandoObject();
            expando.Description = this.Description;
            expando.TransactionDate = this.TransactionDate;
            expando.Entity = this.Entity;
            expando.EntityId = this.EntityId;
            expando.BatchId = this.BatchId;
            expando.CurrencyTypeId = this.CurrencyTypeId;
            expando.CreditCardTypeId = this.CreditCardTypeId;
            expando.Amount = this.Amount;
            expando.RefundTransactionId = this.RefundTransactionId;
            expando.TransactionImageId = this.TransactionImageId;
            expando.TransactionCode = this.TransactionCode;
            expando.GatewayId = this.GatewayId;
            expando.SourceTypeId = this.SourceTypeId;
            expando.Summary = this.Summary;
            expando.Id = this.Id;
            expando.Guid = this.Guid;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyFromModel( IEntity model )
        {
            if ( model is Transaction )
            {
                var transaction = (Transaction)model;
                this.Description = transaction.Description;
                this.TransactionDate = transaction.TransactionDate;
                this.Entity = transaction.Entity;
                this.EntityId = transaction.EntityId;
                this.BatchId = transaction.BatchId;
                this.CurrencyTypeId = transaction.CurrencyTypeId;
                this.CreditCardTypeId = transaction.CreditCardTypeId;
                this.Amount = transaction.Amount;
                this.RefundTransactionId = transaction.RefundTransactionId;
                this.TransactionImageId = transaction.TransactionImageId;
                this.TransactionCode = transaction.TransactionCode;
                this.GatewayId = transaction.GatewayId;
                this.SourceTypeId = transaction.SourceTypeId;
                this.Summary = transaction.Summary;
                this.Id = transaction.Id;
                this.Guid = transaction.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is Transaction )
            {
                var transaction = (Transaction)model;
                transaction.Description = this.Description;
                transaction.TransactionDate = this.TransactionDate;
                transaction.Entity = this.Entity;
                transaction.EntityId = this.EntityId;
                transaction.BatchId = this.BatchId;
                transaction.CurrencyTypeId = this.CurrencyTypeId;
                transaction.CreditCardTypeId = this.CreditCardTypeId;
                transaction.Amount = this.Amount;
                transaction.RefundTransactionId = this.RefundTransactionId;
                transaction.TransactionImageId = this.TransactionImageId;
                transaction.TransactionCode = this.TransactionCode;
                transaction.GatewayId = this.GatewayId;
                transaction.SourceTypeId = this.SourceTypeId;
                transaction.Summary = this.Summary;
                transaction.Id = this.Id;
                transaction.Guid = this.Guid;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class TransactionDtoExtension
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Transaction ToModel( this TransactionDto value )
        {
            Transaction result = new Transaction();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<Transaction> ToModel( this List<TransactionDto> value )
        {
            List<Transaction> result = new List<Transaction>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<TransactionDto> ToDto( this List<Transaction> value )
        {
            List<TransactionDto> result = new List<TransactionDto>();
            value.ForEach( a => result.Add( new TransactionDto( a ) ) );
            return result;
        }
    }
}