﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Scheduled Transaction Detail POCO class.
    /// </summary>
    [Table( "FinancialScheduledTransactionDetail" )]
    [DataContract]
    public partial class FinancialScheduledTransactionDetail : Model<FinancialScheduledTransactionDetail>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the scheduled transaction id.
        /// </summary>
        /// <value>
        /// The scheduled transaction id.
        /// </value>
        [DataMember]
        public int ScheduledTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the account id.
        /// </summary>
        /// <value>
        /// The account id.
        /// </value>
        [DataMember]
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        [MaxLength( 500 )]
        [DataMember]
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        [DataMember]
        public int? EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        /// <value>
        /// The entity id.
        /// </value>
        [DataMember]
        public int? EntityId { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the scheduled transaction.
        /// </summary>
        /// <value>
        /// The scheduled transaction.
        /// </value>
        [DataMember]
        public virtual FinancialScheduledTransaction ScheduledTransaction { get; set; }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        [DataMember]
        public virtual FinancialAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        [DataMember]
        public virtual EntityType EntityType { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Amount.ToString();
        }

        #endregion

    }

    #region Entity Configuration

    /// <summary>
    /// TransactionDetail Configuration class
    /// </summary>
    public partial class FinancialScheduledTransactionDetailConfiguration : EntityTypeConfiguration<FinancialScheduledTransactionDetail>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialScheduledTransactionDetailConfiguration"/> class.
        /// </summary>
        public FinancialScheduledTransactionDetailConfiguration()
        {
            this.HasRequired( d => d.ScheduledTransaction ).WithMany( t => t.ScheduledTransactionDetails ).HasForeignKey( d => d.ScheduledTransactionId ).WillCascadeOnDelete( true );
            this.HasRequired( d => d.Account ).WithMany().HasForeignKey( d => d.AccountId ).WillCascadeOnDelete( false );
            this.HasOptional( d => d.EntityType ).WithMany().HasForeignKey( d => d.EntityTypeId ).WillCascadeOnDelete( false );
        }
    }

    #endregion

}