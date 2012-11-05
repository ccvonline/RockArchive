//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Rock.Data;

namespace Rock.Crm
{
    /// <summary>
    /// Location POCO Entity.
    /// </summary>
    [Table( "crmLocation" )]
    public partial class Location : Model<Location>
    {
        /// <summary>
        /// Gets or sets the Raw.
        /// </summary>
        /// <value>
        /// Raw.
        /// </value>
        [MaxLength( 400 )]
        public string Raw { get; set; }
        
        /// <summary>
        /// Gets or sets the Street 1.
        /// </summary>
        /// <value>
        /// Street 1.
        /// </value>
        [MaxLength( 100 )]
        public string Street1 { get; set; }
        
        /// <summary>
        /// Gets or sets the Street 2.
        /// </summary>
        /// <value>
        /// Street 2.
        /// </value>
        [MaxLength( 100 )]
        public string Street2 { get; set; }
        
        /// <summary>
        /// Gets or sets the City.
        /// </summary>
        /// <value>
        /// City.
        /// </value>
        [MaxLength( 50 )]
        public string City { get; set; }
        
        /// <summary>
        /// Gets or sets the State.
        /// </summary>
        /// <value>
        /// State.
        /// </value>
        [MaxLength( 50 )]
        public string State { get; set; }
        
        /// <summary>
        /// Gets or sets the Country.
        /// </summary>
        /// <value>
        /// Country.
        /// </value>
        [MaxLength( 50 )]
        public string Country { get; set; }
        
        /// <summary>
        /// Gets or sets the Zip.
        /// </summary>
        /// <value>
        /// Zip.
        /// </value>
        [MaxLength( 10 )]
        public string Zip { get; set; }
        
        /// <summary>
        /// Gets or sets the Latitude.
        /// </summary>
        /// <value>
        /// Latitude.
        /// </value>
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the Longitude.
        /// </summary>
        /// <value>
        /// Longitude.
        /// </value>
        public double? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the Parcel Id.
        /// </summary>
        /// <value>
        /// Parcel Id.
        /// </value>
        [MaxLength( 50 )]
        public string ParcelId { get; set; }

        /// <summary>
        /// Gets or sets the Standardize Attempt.
        /// </summary>
        /// <value>
        /// Standardize Attempt.
        /// </value>
        public DateTime? StandardizeAttempt { get; set; }
        
        /// <summary>
        /// Gets or sets the Standardize Service.
        /// </summary>
        /// <value>
        /// Standardize Service.
        /// </value>
        [MaxLength( 50 )]
        public string StandardizeService { get; set; }
        
        /// <summary>
        /// Gets or sets the Standardize Result.
        /// </summary>
        /// <value>
        /// .
        /// </value>
        [MaxLength( 50 )]
        public string StandardizeResult { get; set; }
        
        /// <summary>
        /// Gets or sets the Standardize Date.
        /// </summary>
        /// <value>
        /// Standardize Date.
        /// </value>
        public DateTime? StandardizeDate { get; set; }
        
        /// <summary>
        /// Gets or sets the Geocode Attempt.
        /// </summary>
        /// <value>
        /// Geocode Attempt.
        /// </value>
        public DateTime? GeocodeAttempt { get; set; }
        
        /// <summary>
        /// Gets or sets the Geocode Service.
        /// </summary>
        /// <value>
        /// Geocode Service.
        /// </value>
        [MaxLength( 50 )]
        public string GeocodeService { get; set; }
        
        /// <summary>
        /// Gets or sets the Geocode Result.
        /// </summary>
        /// <value>
        /// .
        /// </value>
        [MaxLength( 50 )]
        public string GeocodeResult { get; set; }
        
        /// <summary>
        /// Gets or sets the Geocode Date.
        /// </summary>
        /// <value>
        /// Geocode Date.
        /// </value>
        public DateTime? GeocodeDate { get; set; }
        
        /// <summary>
        /// Static Method to return an object based on the id
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static Location Read( int id )
        {
            return Read<Location>( id );
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format( "{0} {1} {2}, {3} {4}",
                this.Street1, this.Street2, this.City, this.State, this.Zip );
        }
    }

    /// <summary>
    /// Location Configuration class.
    /// </summary>
    public partial class LocationConfiguration : EntityTypeConfiguration<Location>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationConfiguration"/> class.
        /// </summary>
        public LocationConfiguration()
        {
        }
    }
}
