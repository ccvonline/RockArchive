﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table( "MarketingCampaignAd")]
    public partial class MarketingCampaignAd : Model<MarketingCampaignAd>
    {
        /// <summary>
        /// Gets or sets the marketing campaign id.
        /// </summary>
        /// <value>
        /// The marketing campaign id.
        /// </value>
        public int MarketingCampaignId { get; set; }

        /// <summary>
        /// Gets or sets the marketing campaign ad type id.
        /// </summary>
        /// <value>
        /// The marketing campaign ad type id.
        /// </value>
        public int MarketingCampaignAdTypeId { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the marketing campaign ad status.
        /// </summary>
        /// <value>
        /// The marketing campaign ad status.
        /// </value>
        public MarketingCampaignAdStatus MarketingCampaignAdStatus { get; set; }

        /// <summary>
        /// Gets or sets the marketing campaign status person id.
        /// </summary>
        /// <value>
        /// The marketing campaign status person id.
        /// </value>
        public int? MarketingCampaignStatusPersonId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [MaxLength(2000)]
        public string Url { get; set; }

        /// <summary>
        /// Gets the dto.
        /// </summary>
        /// <returns></returns>
        public override IDto Dto
        {
            get { return this.ToDto(); }
        }

        /// <summary>
        /// Reads the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static MarketingCampaignAd Read( int id )
        {
            return Read<MarketingCampaignAd>( id );
        }

        /// <summary>
        /// Gets or sets the marketing campaign.
        /// </summary>
        /// <value>
        /// The marketing campaign.
        /// </value>
        public virtual MarketingCampaign MarketingCampaign { get; set; }

        /// <summary>
        /// Gets or sets the type of the marketing campaign ad.
        /// </summary>
        /// <value>
        /// The type of the marketing campaign ad.
        /// </value>
        public virtual MarketingCampaignAdType MarketingCampaignAdType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class MarketingCampaignAdConfiguration : EntityTypeConfiguration<MarketingCampaignAd>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketingCampaignAdConfiguration" /> class.
        /// </summary>
        public MarketingCampaignAdConfiguration()
        {
            this.HasRequired( p => p.MarketingCampaign ).WithMany( a => a.MarketingCampaignAds ).HasForeignKey( p => p.MarketingCampaignId ).WillCascadeOnDelete( true );
            this.HasRequired( p => p.MarketingCampaignAdType ).WithMany().HasForeignKey( p => p.MarketingCampaignAdTypeId ).WillCascadeOnDelete( false );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MarketingCampaignAdStatus : byte
    {
        /// <summary>
        /// 
        /// </summary>
        PendingApproval = 1,

        /// <summary>
        /// 
        /// </summary>
        Approved = 2,

        /// <summary>
        /// 
        /// </summary>
        Denied = 3
    }
}