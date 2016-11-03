﻿// <copyright>
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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;
using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Represents the fact record for an Analytic Fact Financial Transaction in Rock.
    /// NOTE: AnalyticsFactFinancialTransaction is simply a de-normalized sql VIEW based on AnalyticsSourceFinancialTransaction
    /// </summary>
    [Table( "AnalyticsFactFinancialTransaction" )]
    [DataContract]
    public class AnalyticsFactFinancialTransaction : AnalyticsBaseFinancialTransaction<AnalyticsFactFinancialTransaction>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the type of the transaction.
        /// </summary>
        /// <value>
        /// The type of the transaction.
        /// </value>
        [DataMember]
        [DefinedValue]
        public string TransactionType { get; set; }

        /// <summary>
        /// Gets or sets the transaction source.
        /// </summary>
        /// <value>
        /// The transaction source.
        /// </value>
        [DataMember]
        public string TransactionSource { get; set; }

        /// <summary>
        /// Gets or sets the type of the schedule.
        /// </summary>
        /// <value>
        /// The type of the schedule.
        /// </value>
        [DataMember]
        public string ScheduleType { get; set; }

        /// <summary>
        /// Gets or sets the authorized person key.
        /// </summary>
        /// <value>
        /// The authorized person key.
        /// </value>
        [DataMember]
        public string AuthorizedPersonKey { get; set; }

        /// <summary>
        /// Gets or sets the authorized current person identifier.
        /// </summary>
        /// <value>
        /// The authorized current person identifier.
        /// </value>
        [DataMember]
        public int AuthorizedCurrentPersonId { get; set; }

        /// <summary>
        /// Gets or sets the processed by person key.
        /// </summary>
        /// <value>
        /// The processed by person key.
        /// </value>
        [DataMember]
        public string ProcessedByPersonKey { get; set; }

        /// <summary>
        /// Gets or sets the giving unit key.
        /// </summary>
        /// <value>
        /// The giving unit key.
        /// </value>
        [DataMember]
        public string GivingUnitKey { get; set; }

        /// <summary>
        /// Gets or sets the gateway identifier.
        /// </summary>
        /// <value>
        /// The gateway identifier.
        /// </value>
        [DataMember]
        public string FinancialGateway { get; set; }

        /// <summary>
        /// Gets or sets the entity type identifier.
        /// </summary>
        /// <value>
        /// The entity type identifier.
        /// </value>
        [DataMember]
        public string EntityTypeName { get; set; }

        /// <summary>
        /// Gets or sets the type of the currency.
        /// </summary>
        /// <value>
        /// The type of the currency.
        /// </value>
        [DataMember]
        public string CurrencyType { get; set; }

        /// <summary>
        /// Gets or sets the type of the credit card.
        /// </summary>
        /// <value>
        /// The type of the credit card.
        /// </value>
        [DataMember]
        public string CreditCardType { get; set; }

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// 
    /// </summary>
    public partial class AnalyticsFactFinancialTransactionConfiguration : EntityTypeConfiguration<AnalyticsFactFinancialTransaction>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsFactFinancialTransactionConfiguration"/> class.
        /// </summary>
        public AnalyticsFactFinancialTransactionConfiguration()
        {
            this.HasRequired( t => t.TransactionDate ).WithMany().HasForeignKey( t => t.TransactionDateKey ).WillCascadeOnDelete( false );
            this.HasRequired( t => t.Batch ).WithMany().HasForeignKey( t => t.BatchId ).WillCascadeOnDelete( false );
            this.HasRequired( t => t.SourceTypeValue ).WithMany().HasForeignKey( t => t.SourceTypeValueId ).WillCascadeOnDelete( false );
            this.HasRequired( t => t.TransactionTypeValue ).WithMany().HasForeignKey( t => t.TransactionTypeValueId ).WillCascadeOnDelete( false );
            this.HasRequired( t => t.CurrencyTypeValue ).WithMany().HasForeignKey( t => t.CurrencyTypeValueId ).WillCascadeOnDelete( false );
            this.HasRequired( t => t.CreditCardTypeValue ).WithMany().HasForeignKey( t => t.CreditCardTypeValueId ).WillCascadeOnDelete( false );
            this.HasRequired( t => t.Account ).WithMany().HasForeignKey( t => t.AccountId ).WillCascadeOnDelete( false );
        }
    }

    #endregion
}