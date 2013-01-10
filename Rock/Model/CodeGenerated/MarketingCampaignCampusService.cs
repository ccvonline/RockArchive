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
    /// MarketingCampaignCampus Service class
    /// </summary>
    public partial class MarketingCampaignCampusService : Service<MarketingCampaignCampus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketingCampaignCampusService"/> class
        /// </summary>
        public MarketingCampaignCampusService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketingCampaignCampusService"/> class
        /// </summary>
        public MarketingCampaignCampusService(IRepository<MarketingCampaignCampus> repository) : base(repository)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( MarketingCampaignCampus item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static class MarketingCampaignCampusExtensionMethods
    {
        /// <summary>
        /// Copies all the entity properties from another MarketingCampaignCampus entity
        /// </summary>
        public static void CopyPropertiesFrom( this MarketingCampaignCampus target, MarketingCampaignCampus source )
        {
            target.MarketingCampaignId = source.MarketingCampaignId;
            target.CampusId = source.CampusId;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
