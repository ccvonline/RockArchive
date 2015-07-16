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
    /// Represents a connection opportunity group campus
    /// </summary>
    [Table( "ConnectionOpportunityGroupCampus" )]
    [DataContract]
    public partial class ConnectionOpportunityGroupCampus : Model<ConnectionOpportunityGroupCampus>
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the connection opportunity identifier.
        /// </summary>
        /// <value>
        /// The connection opportunity identifier.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int ConnectionOpportunityId { get; set; }

        /// <summary>
        /// Gets or sets the campus identifier.
        /// </summary>
        /// <value>
        /// The campus identifier.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int CampusId { get; set; }

        /// <summary>
        /// Gets or sets the connector group identifier.
        /// </summary>
        /// <value>
        /// The connector group identifier.
        /// </value>
        [DataMember]
        public int? ConnectorGroupId { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the connection opportunity.
        /// </summary>
        /// <value>
        /// The connection opportunity.
        /// </value>
        [DataMember]
        public virtual ConnectionOpportunity ConnectionOpportunity { get; set; }

        /// <summary>
        /// Gets or sets the campus.
        /// </summary>
        /// <value>
        /// The campus.
        /// </value>
        [DataMember]
        public virtual Campus Campus { get; set; }

        /// <summary>
        /// Gets or sets the connector group.
        /// </summary>
        /// <value>
        /// The connector group.
        /// </value>
        [DataMember]
        public virtual Group ConnectorGroup { get; set; }

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// ConnectionOpportunityGroupCampus Configuration class.
    /// </summary>
    public partial class ConnectionOpportunityGroupCampusConfiguration : EntityTypeConfiguration<ConnectionOpportunityGroupCampus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionOpportunityGroupCampusConfiguration" /> class.
        /// </summary>
        public ConnectionOpportunityGroupCampusConfiguration()
        {
            this.HasRequired( p => p.ConnectionOpportunity ).WithMany( p => p.ConnectionOpportunityGroupCampuses ).HasForeignKey( p => p.ConnectionOpportunityId ).WillCascadeOnDelete( true );
            this.HasRequired( p => p.Campus ).WithMany().HasForeignKey( p => p.CampusId ).WillCascadeOnDelete( true );
            this.HasOptional( p => p.ConnectorGroup ).WithMany().HasForeignKey( p => p.ConnectorGroupId ).WillCascadeOnDelete( true );
        }
    }

    #endregion
}