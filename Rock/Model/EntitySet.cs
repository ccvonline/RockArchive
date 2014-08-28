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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Entity Set POCO Entity.
    /// </summary>
    [Table( "EntitySet" )]
    [DataContract]
    public partial class EntitySet : Model<EntitySet>, IOrdered
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the parent entity set identifier.
        /// </summary>
        /// <value>
        /// The parent entity set identifier.
        /// </value>
        [DataMember]
        public int? ParentEntitySetId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [MaxLength( 100 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the entity type identifier.
        /// </summary>
        /// <value>
        /// The entity type identifier.
        /// </value>
        [DataMember]
        public int? EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the expire date time.
        /// </summary>
        /// <value>
        /// The expire date time.
        /// </value>
        [DataMember]
        public DateTime? ExpireDateTime { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        [DataMember]
        public int Order { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the parent entity set.
        /// </summary>
        /// <value>
        /// The parent entity set.
        /// </value>
        public virtual EntitySet ParentEntitySet { get; set; }

        /// <summary>
        /// Gets or sets the child entity sets.
        /// </summary>
        /// <value>
        /// The child entity sets.
        /// </value>
        [DataMember]
        public virtual ICollection<EntitySet> ChildEntitySets { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [DataMember]
        public virtual ICollection<EntitySetItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        [DataMember]
        public virtual Model.EntityType EntityType { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this EntitySet
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this EntitySet
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// EntitySet Configuration class.
    /// </summary>
    public partial class EntitySetConfiguration : EntityTypeConfiguration<EntitySet>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySetConfiguration"/> class.
        /// </summary>
        public EntitySetConfiguration()
        {
            this.HasOptional( s => s.ParentEntitySet ).WithMany( s => s.ChildEntitySets ).HasForeignKey( s => s.ParentEntitySetId).WillCascadeOnDelete( false );
            this.HasOptional( a => a.EntityType ).WithMany().HasForeignKey( a => a.EntityTypeId ).WillCascadeOnDelete( false );
        }
    }

    #endregion
}
