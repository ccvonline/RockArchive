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
    /// Represents a connection status
    /// </summary>
    [Table( "ConnectionStatus" )]
    [DataContract]
    public partial class ConnectionStatus : Model<ConnectionStatus>
    {

        #region Entity Properties

        [Required]
        [MaxLength( 50 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        [Required]
        [DataMember( IsRequired = true )]
        public string Description { get; set; }

        public int? ConnectionTypeId { get; set; }

        [DataMember]
        public bool IsCritical { get; set; }

        [DataMember]
        public bool IsDefault { get; set; }

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
    /// ConnectionStatus Configuration class.
    /// </summary>
    public partial class ConnectionStatusConfiguration : EntityTypeConfiguration<ConnectionStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStatusConfiguration" /> class.
        /// </summary>
        public ConnectionStatusConfiguration()
        {
            this.HasOptional( p => p.ConnectionType ).WithMany( p => p.ConnectionStatuses ).HasForeignKey( p => p.ConnectionTypeId ).WillCascadeOnDelete( false );
        }
    }

    #endregion
}