//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// MarketingCampaignAd Service class
    /// </summary>
    public partial class MarketingCampaignAdService : Service<MarketingCampaignAd, MarketingCampaignAdDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketingCampaignAdService"/> class
        /// </summary>
        public MarketingCampaignAdService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketingCampaignAdService"/> class
        /// </summary>
        public MarketingCampaignAdService(IRepository<MarketingCampaignAd> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override MarketingCampaignAd CreateNew()
        {
            return new MarketingCampaignAd();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<MarketingCampaignAdDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<MarketingCampaignAdDto> QueryableDto( IQueryable<MarketingCampaignAd> items )
        {
            return items.Select( m => new MarketingCampaignAdDto()
                {
                    MarketingCampaignId = m.MarketingCampaignId,
                    MarketingCampaignAdTypeId = m.MarketingCampaignAdTypeId,
                    Priority = m.Priority,
                    MarketingCampaignAdStatus = m.MarketingCampaignAdStatus,
                    MarketingCampaignStatusPersonId = m.MarketingCampaignStatusPersonId,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    Url = m.Url,
                    Id = m.Id,
                    Guid = m.Guid,
                });
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( MarketingCampaignAd item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }
}
