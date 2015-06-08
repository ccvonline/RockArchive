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
    /// Represents a connection opportunity group
    /// </summary>
    [Table( "ConnectionOpportunityGroup" )]
    [DataContract]
    public partial class ConnectionOpportunityGroup : Model<ConnectionOpportunityGroup>
    {

        #region Entity Properties

        [Required]
        [DataMember]
        public int? ConnectionOpportunityId { get; set; }

        [Required]
        [DataMember]
        public int? GroupId { get; set; }

        #endregion

        #region Virtual Properties

        [DataMember]
        public virtual ConnectionOpportunity ConnectionOpportunity { get; set; }

        [DataMember]
        public virtual Group Group { get; set; }

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// ConnectionOpportunityGroup Configuration class.
    /// </summary>
    public partial class ConnectionOpportunityGroupConfiguration : EntityTypeConfiguration<ConnectionOpportunityGroup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionOpportunityGroupConfiguration" /> class.
        /// </summary>
        public ConnectionOpportunityGroupConfiguration()
        {
            this.HasRequired( p => p.ConnectionOpportunity ).WithMany().HasForeignKey( p => p.ConnectionOpportunityId ).WillCascadeOnDelete( false );
            this.HasRequired( p => p.Group ).WithMany().HasForeignKey( p => p.GroupId ).WillCascadeOnDelete( false );
        }
    }

    #endregion
}