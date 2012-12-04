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
    /// Location Service class
    /// </summary>
    public partial class LocationService : Service<Location, LocationDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationService"/> class
        /// </summary>
        public LocationService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationService"/> class
        /// </summary>
        public LocationService(IRepository<Location> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override Location CreateNew()
        {
            return new Location();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<LocationDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<LocationDto> QueryableDto( IQueryable<Location> items )
        {
            return items.Select( m => new LocationDto()
                {
                    Raw = m.Raw,
                    Street1 = m.Street1,
                    Street2 = m.Street2,
                    City = m.City,
                    State = m.State,
                    Country = m.Country,
                    Zip = m.Zip,
                    Latitude = m.Latitude,
                    Longitude = m.Longitude,
                    ParcelId = m.ParcelId,
                    StandardizeAttempt = m.StandardizeAttempt,
                    StandardizeService = m.StandardizeService,
                    StandardizeResult = m.StandardizeResult,
                    StandardizeDate = m.StandardizeDate,
                    GeocodeAttempt = m.GeocodeAttempt,
                    GeocodeService = m.GeocodeService,
                    GeocodeResult = m.GeocodeResult,
                    GeocodeDate = m.GeocodeDate,
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
        public bool CanDelete( Location item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }
}
