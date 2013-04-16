﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
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
    /// Scheduled Transaction POCO class.
    /// </summary>
    [Table( "FinancialScheduledTransaction" )]
    [DataContract]
    public partial class FinancialScheduledTransaction : Model<FinancialScheduledTransaction>
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the authorized person id.
        /// </summary>
        /// <value>
        /// The authorized person id.
        /// </value>
        [DataMember]
        public int AuthorizedPersonId { get; set; }

        /// <summary>
        /// Gets or sets the transaction frequency value id.
        /// </summary>
        /// <value>
        /// The transaction frequency value id.
        /// </value>
        [DataMember]
        [DefinedValue( SystemGuid.DefinedType.FINANCIAL_TRANSACTION_FREQUENCY )]
        public int TransactionFrequencyValueId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [DataMember]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the number of payments.
        /// </summary>
        /// <value>
        /// The number of payments.
        /// </value>
        [DataMember]
        public int? NumberOfPayments { get; set; }

        [DataMember]
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
        private bool _isActive = true;

        /// <summary>
        /// Gets or sets the gateway id.
        /// </summary>
        /// <value>
        /// The gateway id.
        /// </value>
        [DataMember]
        public int? GatewayId { get; set; }

        /// <summary>
        /// Gets or sets the transaction code.
        /// </summary>
        /// <value>
        /// The transaction code.
        /// </value>
        [MaxLength( 50 )]
        [DataMember]
        public string TransactionCode { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the authorized person.
        /// </summary>
        /// <value>
        /// The authorized person.
        /// </value>
        public virtual Person AuthorizedPerson { get; set; }

        /// <summary>
        /// Gets or sets the gateway.
        /// </summary>
        /// <value>
        /// The gateway.
        /// </value>
        [DataMember]
        public virtual FinancialGateway Gateway { get; set; }

        /// <summary>
        /// Gets or sets the transaction frequency value.
        /// </summary>
        /// <value>
        /// The transaction frequency value.
        /// </value>
        [DataMember]
        public virtual DefinedValue TransactionFrequencyValue { get; set; }

        #endregion

        #region Public Methods

        #endregion

    }

    #region Entity Configuration


    /// <summary>
    /// Transaction Configuration class.
    /// </summary>
    public partial class FinancialScheduledTransactionConfiguration : EntityTypeConfiguration<FinancialScheduledTransaction>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialScheduledTransactionConfiguration"/> class.
        /// </summary>
        public FinancialScheduledTransactionConfiguration()
        {
            this.HasRequired( t => t.AuthorizedPerson ).WithMany().HasForeignKey( t => t.AuthorizedPersonId ).WillCascadeOnDelete( false );
            this.HasOptional( t => t.Gateway ).WithMany().HasForeignKey( t => t.GatewayId ).WillCascadeOnDelete( false );
            this.HasRequired( t => t.TransactionFrequencyValue ).WithMany().HasForeignKey( t => t.TransactionFrequencyValueId ).WillCascadeOnDelete( false );
        }
    }

    #endregion

}