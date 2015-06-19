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
    /// Represents a connection activity type
    /// </summary>
    [Table( "ConnectionActivityType" )]
    [DataContract]
    public partial class ConnectionActivityType : Model<ConnectionActivityType>
    {

        #region Entity Properties

        [Required]
        [MaxLength( 50 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        [DataMember]
        public int? ConnectionTypeId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        #endregion

        #region Virtual Properties

        [DataMember]
        public virtual ConnectionType ConnectionType { get; set; }

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// ConnectionActivityType Configuration class.
    /// </summary>
    public partial class ConnectionActivityTypeConfiguration : EntityTypeConfiguration<ConnectionActivityType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionActivityTypeConfiguration" /> class.
        /// </summary>
        public ConnectionActivityTypeConfiguration()
        {
            this.HasOptional( p => p.ConnectionType ).WithMany( p => p.ConnectionActivityTypes ).HasForeignKey( p => p.ConnectionTypeId ).WillCascadeOnDelete( true );
        }
    }

    #endregion
}