﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;
using Rock.Crm;
using Rock.Data;

namespace Rock.Financial
    
    /// <summary>
    /// TransactionDetail POCO class.
    /// </summary>
    [Table("financialTransactionDetail")]
    public partial class TransactionDetail : Model<TransactionDetail>
        
        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        /// <value>
        /// The transaction id.
        /// </value>
        [DataMember]
        public int? TransactionId      get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        [DataMember]
        [MaxLength(50)]
        public string Entity      get; set; }

        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        /// <value>
        /// The entity id.
        /// </value>
        [DataMember]
        public string EntityId      get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        [DataMember]
        public decimal Amount      get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        [DataMember]
        [MaxLength(500)]
        public string Summary      get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>
        /// The transaction.
        /// </value>
        public virtual Transaction Transaction      get; set; }

		/// <summary>
		/// Static Method to return an object based on the id
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public static TransactionDetail Read( int id )
		    
			return Read<TransactionDetail>( id );
		}

        /// <summary>
        /// Gets the auth entity.
        /// </summary>
        public override string EntityTypeName      get      return "Financial.TransactionDetail"; } }

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		    
			return this.Amount.ToString();
		}
    }

    /// <summary>
    /// TransactionDetail Configuration class
    /// </summary>
    public partial class TransactionDetailConfiguration : EntityTypeConfiguration<TransactionDetail>
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionDetailConfiguration"/> class.
        /// </summary>
        public TransactionDetailConfiguration()
            
            this.HasOptional(d => d.Transaction).WithMany(t => t.TransactionDetails).HasForeignKey(t => t.TransactionId).WillCascadeOnDelete(false);
        }
    }
}