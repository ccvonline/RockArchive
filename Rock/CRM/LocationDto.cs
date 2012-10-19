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
using System.Collections.Generic;
using System.Dynamic;

using Rock.Data;

namespace Rock.Crm
{
    /// <summary>
    /// Data Transfer Object for Location object
    /// </summary>
    public partial class LocationDto : IDto
    {

#pragma warning disable 1591
        public string Raw { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ParcelId { get; set; }
        public DateTime? StandardizeAttempt { get; set; }
        public string StandardizeService { get; set; }
        public string StandardizeResult { get; set; }
        public DateTime? StandardizeDate { get; set; }
        public DateTime? GeocodeAttempt { get; set; }
        public string GeocodeService { get; set; }
        public string GeocodeResult { get; set; }
        public DateTime? GeocodeDate { get; set; }
        public int Id { get; set; }
        public Guid Guid { get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public LocationDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="location"></param>
        public LocationDto ( Location location )
        {
            CopyFromModel( location );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add( "Raw", this.Raw );
            dictionary.Add( "Street1", this.Street1 );
            dictionary.Add( "Street2", this.Street2 );
            dictionary.Add( "City", this.City );
            dictionary.Add( "State", this.State );
            dictionary.Add( "Country", this.Country );
            dictionary.Add( "Zip", this.Zip );
            dictionary.Add( "Latitude", this.Latitude );
            dictionary.Add( "Longitude", this.Longitude );
            dictionary.Add( "ParcelId", this.ParcelId );
            dictionary.Add( "StandardizeAttempt", this.StandardizeAttempt );
            dictionary.Add( "StandardizeService", this.StandardizeService );
            dictionary.Add( "StandardizeResult", this.StandardizeResult );
            dictionary.Add( "StandardizeDate", this.StandardizeDate );
            dictionary.Add( "GeocodeAttempt", this.GeocodeAttempt );
            dictionary.Add( "GeocodeService", this.GeocodeService );
            dictionary.Add( "GeocodeResult", this.GeocodeResult );
            dictionary.Add( "GeocodeDate", this.GeocodeDate );
            dictionary.Add( "Id", this.Id );
            dictionary.Add( "Guid", this.Guid );
            return dictionary;
        }

        /// <summary>
        /// Creates a dynamic object.
        /// </summary>
        /// <returns></returns>
        public virtual dynamic ToDynamic()
        {
            dynamic expando = new ExpandoObject();
            expando.Raw = this.Raw;
            expando.Street1 = this.Street1;
            expando.Street2 = this.Street2;
            expando.City = this.City;
            expando.State = this.State;
            expando.Country = this.Country;
            expando.Zip = this.Zip;
            expando.Latitude = this.Latitude;
            expando.Longitude = this.Longitude;
            expando.ParcelId = this.ParcelId;
            expando.StandardizeAttempt = this.StandardizeAttempt;
            expando.StandardizeService = this.StandardizeService;
            expando.StandardizeResult = this.StandardizeResult;
            expando.StandardizeDate = this.StandardizeDate;
            expando.GeocodeAttempt = this.GeocodeAttempt;
            expando.GeocodeService = this.GeocodeService;
            expando.GeocodeResult = this.GeocodeResult;
            expando.GeocodeDate = this.GeocodeDate;
            expando.Id = this.Id;
            expando.Guid = this.Guid;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyFromModel( IEntity model )
        {
            if ( model is Location )
            {
                var location = (Location)model;
                this.Raw = location.Raw;
                this.Street1 = location.Street1;
                this.Street2 = location.Street2;
                this.City = location.City;
                this.State = location.State;
                this.Country = location.Country;
                this.Zip = location.Zip;
                this.Latitude = location.Latitude;
                this.Longitude = location.Longitude;
                this.ParcelId = location.ParcelId;
                this.StandardizeAttempt = location.StandardizeAttempt;
                this.StandardizeService = location.StandardizeService;
                this.StandardizeResult = location.StandardizeResult;
                this.StandardizeDate = location.StandardizeDate;
                this.GeocodeAttempt = location.GeocodeAttempt;
                this.GeocodeService = location.GeocodeService;
                this.GeocodeResult = location.GeocodeResult;
                this.GeocodeDate = location.GeocodeDate;
                this.Id = location.Id;
                this.Guid = location.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is Location )
            {
                var location = (Location)model;
                location.Raw = this.Raw;
                location.Street1 = this.Street1;
                location.Street2 = this.Street2;
                location.City = this.City;
                location.State = this.State;
                location.Country = this.Country;
                location.Zip = this.Zip;
                location.Latitude = this.Latitude;
                location.Longitude = this.Longitude;
                location.ParcelId = this.ParcelId;
                location.StandardizeAttempt = this.StandardizeAttempt;
                location.StandardizeService = this.StandardizeService;
                location.StandardizeResult = this.StandardizeResult;
                location.StandardizeDate = this.StandardizeDate;
                location.GeocodeAttempt = this.GeocodeAttempt;
                location.GeocodeService = this.GeocodeService;
                location.GeocodeResult = this.GeocodeResult;
                location.GeocodeDate = this.GeocodeDate;
                location.Id = this.Id;
                location.Guid = this.Guid;
            }
        }
    }
}
