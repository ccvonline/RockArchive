﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;
using Rock.Crm;
using Rock.Data;

namespace Rock.Financial
{
    /// <summary>
    /// Payment Gateway POCO class.
    /// </summary>
    [Table("financialGateway")]
    public partial class Gateway : Model<Gateway>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the API URL.
        /// </summary>
        /// <value>
        /// The API URL.
        /// </value>
        [DataMember]
        [MaxLength(100)]
        public string ApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        [DataMember]
        [MaxLength(100)]
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the API secret.
        /// </summary>
        /// <value>
        /// The API secret.
        /// </value>
        [DataMember]
        [MaxLength(100)]
        public string ApiSecret { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>
        /// The transactions.
        /// </value>
        public virtual ICollection<Transaction> Transactions { get; set; }

        /// <summary>
        /// Gets the auth entity.
        /// </summary>
        [NotMapped]
        public string DeprecatedEntityTypeName { get { return "Financial.Gateway"; } }

        /// <summary>
        /// Static Method to return an object based on the id
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static Gateway Read( int id )
        {
            return Read<Gateway>( id );
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }

    /// <summary>
    /// Payment Gateway Configuration class.
    /// </summary>
    public partial class GatewayConfiguration : EntityTypeConfiguration<Gateway>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GatewayConfiguration"/> class.
        /// </summary>
        public GatewayConfiguration()
        {
        }
    }
}