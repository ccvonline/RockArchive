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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Represents a Type of <see cref="Rock.Model.MarketingCampaignAd"/>.
    /// </summary>
    [Table( "MarketingCampaignAdType" )]
    [DataContract]
    public partial class MarketingCampaignAdType : Model<MarketingCampaignAdType>
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets a flag indicating if this MarketingCampaignAdType is part of the Rock core system/framework. 
        /// </summary>
        /// <value>
        ///   A <see cref="System.Boolean"/> flag that is <c>true</c> if this MarketingCAmpaignAdType is part of the Rock core system/framework; otherwise <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the name of the MarketingCampaignAdType. This property is required.
        /// </summary>
        /// <value>
        /// A <see cref="System.String" /> representing the name of the MarketingCampaignAdType.
        /// </value>
        [Required]
        [MaxLength( 100 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="DateRangeTypeEnum"/> enumeration that represents the type of date range that this DateRangeTypeEnum supports.
        /// </summary>
        /// <value>
        /// A <see cref="Rock.Model.DateRangeTypeEnum"/> that represents the type of DateRangeTypeEnum is supported. When <c>DateRangeTypeEnum.SingleDate</c> a single date 
        /// will be supported; when <c>DateRangeTypeEnum.DateRange</c> a date range will be supported.
        /// </value>
        [DataMember]
        public DateRangeTypeEnum DateRangeType { get; set; }

        #endregion

        #region Virtual Properties

        #endregion

        #region Methods

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

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// 
    /// </summary>
    public partial class MarketingCampaignAdTypeConfiguration : EntityTypeConfiguration<MarketingCampaignAdType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketingCampaignAdTypeConfiguration" /> class.
        /// </summary>
        public MarketingCampaignAdTypeConfiguration()
        {
        }
    }

    #endregion

    #region Enumerations


    #endregion

}