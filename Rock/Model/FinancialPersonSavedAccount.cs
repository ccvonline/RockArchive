﻿// <copyright>
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Represents an bank or debit/credit card that a <see cref="Rock.Model.Person"/> has saved to Rock for future reuse. Please
    /// note that account number is not actually stored here. The reference/profile number is stored here as well as a masked 
    /// version of the account number.
    /// </summary>
    [Table( "FinancialPersonSavedAccount" )]
    [DataContract]
    public partial class FinancialPersonSavedAccount : Model<FinancialPersonSavedAccount>
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the person alias identifier.
        /// </summary>
        /// <value>
        /// The person alias identifier.
        /// </value>
        [DataMember]
        public int PersonAliasId { get; set; }
       
        /// <summary>
        /// Gets or sets a reference identifier needed by the payment provider to initiate a future transaction
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the reference identifier to initiate a future transaction.
        /// </value>
        [DataMember]
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the saved account. This property is required.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the name of the account.
        /// </value>
        [Required]
        [MaxLength( 50 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a masked version of the account number. This is a value with "*" and a partial account number (usually the last 4 digits).
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the masked account number.
        /// </value>
        [MaxLength( 100 )]
        [DataMember]
        public string MaskedAccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the transaction code for the transaction.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the transaction code of the transaction.
        /// </value>
        [MaxLength( 50 )]
        [DataMember]
        public string TransactionCode { get; set; }

        /// <summary>
        /// Gets or sets EntityTypeId of the <see cref="Rock.Model.EntityType"/> for the financial gateway (service) that processed this transaction.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32" /> representing the EntityTypeId of the <see cref="Rock.Model.EntityType"/> for the financial gateway (service) that processed this transaction.
        /// </value>
        [DataMember]
        public int? GatewayEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the DefinedValueId of the currency type <see cref="Rock.Model.DefinedValue"/> indicating the currency that the
        /// transaction was made in.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32" /> representing the DefinedValueId of the CurrencyType <see cref="Rock.Model.DefinedValue" /> for this transaction.
        /// </value>
        [DataMember]
        [DefinedValue( SystemGuid.DefinedType.FINANCIAL_CURRENCY_TYPE )]
        public int? CurrencyTypeValueId { get; set; }

        /// <summary>
        /// Gets or sets the DefinedValueId of the credit card type <see cref="Rock.Model.DefinedValue"/> indicating the credit card brand/type that was used 
        /// to make this transaction. This value will be null for transactions that were not made by credit card.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the DefinedValueId of the credit card type <see cref="Rock.Model.DefinedValue"/> that was used to make this transaction.
        /// This value value will be null for transactions that were not made by credit card.
        /// </value>
        [DataMember]
        [DefinedValue( SystemGuid.DefinedType.FINANCIAL_CREDIT_CARD_TYPE )]
        public int? CreditCardTypeValueId { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the person alias.
        /// </summary>
        /// <value>
        /// The person alias.
        /// </value>
        public virtual PersonAlias PersonAlias { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.EntityType"/> of the Payment Gateway service that was used to process this transaction.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.EntityType"/> of the payment gateway service that was used.  If this was not an electronic transaction, this value will be null.
        /// </value>
        [DataMember]
        public virtual EntityType GatewayEntityType { get; set; }

        /// <summary>
        /// Gets or sets the currency type <see cref="Rock.Model.DefinedValue"/> indicating the type of currency that was used for this 
        /// transaction.
        /// </summary>
        /// <value>
        /// A <see cref="Rock.Model.DefinedValue"/> indicating the type of currency that was used for the transaction.
        /// </value>
        [DataMember]
        public virtual DefinedValue CurrencyTypeValue { get; set; }

        /// <summary>
        /// Gets or sets the credit card type <see cref="Rock.Model.DefinedValue"/> indicating the type of credit card that was used for this transaction.
        /// If this was not a credit card based transaction, this value will be null.
        /// </summary>
        /// <value>
        /// A <see cref="Rock.Model.DefinedValue" /> indicating the type of credit card that was used for this transaction. This value is null
        /// for transactions that were not made by credit card.
        /// </value>
        [DataMember]
        public virtual DefinedValue CreditCardTypeValue { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this FinancialPersonSavedAccount.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this FinancialPersonSavedAccount.
        /// </returns>
        public override string ToString()
        {
            return this.MaskedAccountNumber.ToStringSafe();
        }

        #endregion

    }

    #region Entity Configuration


    /// <summary>
    /// FinancialPersonSavedAccount Configuration class.
    /// </summary>
    public partial class FinancialPersonSavedAccountConfiguration : EntityTypeConfiguration<FinancialPersonSavedAccount>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialPersonSavedAccountConfiguration"/> class.
        /// </summary>
        public FinancialPersonSavedAccountConfiguration()
        {
            this.HasRequired( t => t.PersonAlias ).WithMany().HasForeignKey( t => t.PersonAliasId ).WillCascadeOnDelete( true );
            this.HasOptional( t => t.GatewayEntityType ).WithMany().HasForeignKey( t => t.GatewayEntityTypeId ).WillCascadeOnDelete( false );
            this.HasOptional( t => t.CurrencyTypeValue ).WithMany().HasForeignKey( t => t.CurrencyTypeValueId ).WillCascadeOnDelete( false );
            this.HasOptional( t => t.CreditCardTypeValue ).WithMany().HasForeignKey( t => t.CreditCardTypeValueId ).WillCascadeOnDelete( false );
        }
    }

    #endregion

}