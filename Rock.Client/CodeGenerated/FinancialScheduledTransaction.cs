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
using System.Collections.Generic;


namespace Rock.Client
{
    /// <summary>
    /// Base client model for FinancialScheduledTransaction that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class FinancialScheduledTransactionEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public int AuthorizedPersonAliasId { get; set; }

        /// <summary />
        public DateTime? CardReminderDate { get; set; }

        /// <summary />
        public int? CreditCardTypeValueId { get; set; }

        /// <summary />
        public int? CurrencyTypeValueId { get; set; }

        /// <summary />
        public DateTime? EndDate { get; set; }

        /// <summary />
        public int? FinancialGatewayId { get; set; }

        /// <summary />
        public string GatewayScheduleId { get; set; }

        /// <summary />
        public bool IsActive { get; set; }

        /// <summary />
        public DateTime? LastRemindedDate { get; set; }

        /// <summary />
        public DateTime? LastStatusUpdateDateTime { get; set; }

        /// <summary />
        public DateTime? NextPaymentDate { get; set; }

        /// <summary />
        public int? NumberOfPayments { get; set; }

        /// <summary />
        public DateTime StartDate { get; set; }

        /// <summary />
        public string TransactionCode { get; set; }

        /// <summary />
        public int TransactionFrequencyValueId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public string ForeignId { get; set; }

    }

    /// <summary>
    /// Client model for FinancialScheduledTransaction that includes all the fields that are available for GETs. Use this for GETs (use FinancialScheduledTransactionEntity for POST/PUTs)
    /// </summary>
    public partial class FinancialScheduledTransaction : FinancialScheduledTransactionEntity
    {
        /// <summary />
        public DefinedValue CreditCardTypeValue { get; set; }

        /// <summary />
        public DefinedValue CurrencyTypeValue { get; set; }

        /// <summary />
        public FinancialGateway FinancialGateway { get; set; }

        /// <summary />
        public ICollection<FinancialScheduledTransactionDetail> ScheduledTransactionDetails { get; set; }

        /// <summary />
        public DefinedValue TransactionFrequencyValue { get; set; }

        /// <summary />
        public ICollection<FinancialTransaction> Transactions { get; set; }

        /// <summary />
        public DateTime? CreatedDateTime { get; set; }

        /// <summary />
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary />
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary />
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary>
        /// NOTE: Attributes are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.Attribute> Attributes { get; set; }

        /// <summary>
        /// NOTE: AttributeValues are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.AttributeValue> AttributeValues { get; set; }

        /// <summary>
        /// Copies the base properties from a source FinancialScheduledTransaction object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( FinancialScheduledTransaction source )
        {
            this.Id = source.Id;
            this.AuthorizedPersonAliasId = source.AuthorizedPersonAliasId;
            this.CardReminderDate = source.CardReminderDate;
            this.CreditCardTypeValueId = source.CreditCardTypeValueId;
            this.CurrencyTypeValueId = source.CurrencyTypeValueId;
            this.EndDate = source.EndDate;
            this.FinancialGatewayId = source.FinancialGatewayId;
            this.GatewayScheduleId = source.GatewayScheduleId;
            this.IsActive = source.IsActive;
            this.LastRemindedDate = source.LastRemindedDate;
            this.LastStatusUpdateDateTime = source.LastStatusUpdateDateTime;
            this.NextPaymentDate = source.NextPaymentDate;
            this.NumberOfPayments = source.NumberOfPayments;
            this.StartDate = source.StartDate;
            this.TransactionCode = source.TransactionCode;
            this.TransactionFrequencyValueId = source.TransactionFrequencyValueId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }
}
