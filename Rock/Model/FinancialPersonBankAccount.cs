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
    /// Represents a relationship between a person and a bank account in Rock. A person can be related to multiple bank accounts 
    /// but a bank account can only be related to an individual person in Rock.
    /// </summary>
    [Table( "FinancialPersonBankAccount" )]
    [DataContract]
    public partial class FinancialPersonBankAccount : Model<FinancialPersonBankAccount>
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the PersonId of the <see cref="Rock.Model.Person"/> who owns the account.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the PersonId of the <see cref="Rock.Model.Person"/> who owns the account.
        /// </value>
        [DataMember]
        public int PersonId { get; set; }


        /// <summary>
        /// Gets or sets hash of the Checking Account AccountNumber.  Stored as a SHA1 hash (always 40 chars) so that it can be matched without being known
        /// Enables a match of a Check Account to Person ( or Persons if multiple persons share a checking account) can be made
        /// </summary>
        /// <value>
        /// AccountNumberSecured.
        /// </value>
        [Required]
        [MaxLength( 40 )]
        public string AccountNumberSecured { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.Person"/> who owns the account.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.Person"/> who owns the account.
        /// </value>
        public virtual Person Person { get; set; }

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
            return this.AccountNumberSecured.ToStringSafe();
        }

        #endregion

    }

    #region Entity Configuration


    /// <summary>
    /// FinancialPersonBankAccount Configuration class.
    /// </summary>
    public partial class FinancialPersonBankAccountConfiguration : EntityTypeConfiguration<FinancialPersonBankAccount>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialPersonBankAccountConfiguration"/> class.
        /// </summary>
        public FinancialPersonBankAccountConfiguration()
        {
            this.HasRequired( b => b.Person ).WithMany().HasForeignKey( b => b.PersonId ).WillCascadeOnDelete( true );
        }
    }

    #endregion

}