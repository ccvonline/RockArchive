//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
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


namespace Rock.Client
{
    /// <summary>
    /// Base client model for Location that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class LocationEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public string AssessorParcelId { get; set; }

        /// <summary />
        public string Barcode { get; set; }

        /// <summary />
        public string City { get; set; }

        /// <summary />
        public string Country { get; set; }

        /// <summary />
        public string County { get; set; }

        /// <summary />
        public int? FirmRoomThreshold { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public DateTime? GeocodeAttemptedDateTime { get; set; }

        /// <summary />
        public string GeocodeAttemptedResult { get; set; }

        /// <summary />
        public string GeocodeAttemptedServiceType { get; set; }

        /// <summary />
        public DateTime? GeocodedDateTime { get; set; }

        /// <summary />
        public object GeoFence { get; set; }

        /// <summary />
        public object GeoPoint { get; set; }

        /// <summary />
        public int? ImageId { get; set; }

        /// <summary />
        public bool IsActive { get; set; }

        /// <summary />
        public bool? IsGeoPointLocked { get; set; }

        /// <summary />
        public int? LocationTypeValueId { get; set; }

        /// <summary />
        public string Name { get; set; }

        /// <summary />
        public int? ParentLocationId { get; set; }

        /// <summary />
        public string PostalCode { get; set; }

        /// <summary />
        public int? PrinterDeviceId { get; set; }

        /// <summary />
        public int? SoftRoomThreshold { get; set; }

        /// <summary />
        public DateTime? StandardizeAttemptedDateTime { get; set; }

        /// <summary />
        public string StandardizeAttemptedResult { get; set; }

        /// <summary />
        public string StandardizeAttemptedServiceType { get; set; }

        /// <summary />
        public DateTime? StandardizedDateTime { get; set; }

        /// <summary />
        public string State { get; set; }

        /// <summary />
        public string Street1 { get; set; }

        /// <summary />
        public string Street2 { get; set; }

        /// <summary />
        public DateTime? CreatedDateTime { get; set; }

        /// <summary />
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary />
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary />
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source Location object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( Location source )
        {
            this.Id = source.Id;
            this.AssessorParcelId = source.AssessorParcelId;
            this.Barcode = source.Barcode;
            this.City = source.City;
            this.Country = source.Country;
            this.County = source.County;
            this.FirmRoomThreshold = source.FirmRoomThreshold;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.GeocodeAttemptedDateTime = source.GeocodeAttemptedDateTime;
            this.GeocodeAttemptedResult = source.GeocodeAttemptedResult;
            this.GeocodeAttemptedServiceType = source.GeocodeAttemptedServiceType;
            this.GeocodedDateTime = source.GeocodedDateTime;
            this.GeoFence = source.GeoFence;
            this.GeoPoint = source.GeoPoint;
            this.ImageId = source.ImageId;
            this.IsActive = source.IsActive;
            this.IsGeoPointLocked = source.IsGeoPointLocked;
            this.LocationTypeValueId = source.LocationTypeValueId;
            this.Name = source.Name;
            this.ParentLocationId = source.ParentLocationId;
            this.PostalCode = source.PostalCode;
            this.PrinterDeviceId = source.PrinterDeviceId;
            this.SoftRoomThreshold = source.SoftRoomThreshold;
            this.StandardizeAttemptedDateTime = source.StandardizeAttemptedDateTime;
            this.StandardizeAttemptedResult = source.StandardizeAttemptedResult;
            this.StandardizeAttemptedServiceType = source.StandardizeAttemptedServiceType;
            this.StandardizedDateTime = source.StandardizedDateTime;
            this.State = source.State;
            this.Street1 = source.Street1;
            this.Street2 = source.Street2;
            this.CreatedDateTime = source.CreatedDateTime;
            this.ModifiedDateTime = source.ModifiedDateTime;
            this.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            this.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for Location that includes all the fields that are available for GETs. Use this for GETs (use LocationEntity for POST/PUTs)
    /// </summary>
    public partial class Location : LocationEntity
    {
        /// <summary />
        public ICollection<Location> ChildLocations { get; set; }

        /// <summary />
        public double Distance { get; set; }

        /// <summary />
        public List<Double[]> GeoFenceCoordinates { get; set; }

        /// <summary />
        public BinaryFile Image { get; set; }

        /// <summary />
        public double? Latitude { get; set; }

        /// <summary />
        public DefinedValue LocationTypeValue { get; set; }

        /// <summary />
        public double? Longitude { get; set; }

        /// <summary />
        public Device PrinterDevice { get; set; }

    }
}
