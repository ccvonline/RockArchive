//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
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


namespace Rock.Client
{
    /// <summary>
    /// Simple Client Model for Location
    /// </summary>
    public partial class Location
    {
        /// <summary />
        public int? ParentLocationId { get; set; }

        /// <summary />
        public string Name { get; set; }

        /// <summary />
        public bool IsActive { get; set; }

        /// <summary />
        public int? LocationTypeValueId { get; set; }

        /// <summary />
        public object GeoPoint { get; set; }

        /// <summary />
        public object GeoFence { get; set; }

        /// <summary />
        public string Street1 { get; set; }

        /// <summary />
        public string Street2 { get; set; }

        /// <summary />
        public string City { get; set; }

        /// <summary />
        public string State { get; set; }

        /// <summary />
        public string Country { get; set; }

        /// <summary />
        public string PostalCode { get; set; }

        /// <summary />
        public string AssessorParcelId { get; set; }

        /// <summary />
        public DateTime? StandardizeAttemptedDateTime { get; set; }

        /// <summary />
        public string StandardizeAttemptedServiceType { get; set; }

        /// <summary />
        public string StandardizeAttemptedResult { get; set; }

        /// <summary />
        public DateTime? StandardizedDateTime { get; set; }

        /// <summary />
        public DateTime? GeocodeAttemptedDateTime { get; set; }

        /// <summary />
        public string GeocodeAttemptedServiceType { get; set; }

        /// <summary />
        public string GeocodeAttemptedResult { get; set; }

        /// <summary />
        public DateTime? GeocodedDateTime { get; set; }

        /// <summary />
        public bool? IsGeoPointLocked { get; set; }

        /// <summary />
        public int? PrinterDeviceId { get; set; }

        /// <summary />
        public int? ImageId { get; set; }

        /// <summary />
        public bool IsNamedLocation { get; set; }

        /// <summary />
        public DefinedValue LocationTypeValue { get; set; }

        /// <summary />
        public ICollection<Location> ChildLocations { get; set; }

        /// <summary />
        public Device PrinterDevice { get; set; }

        /// <summary />
        public BinaryFile Image { get; set; }

        /// <summary />
        public DateTime? CreatedDateTime { get; set; }

        /// <summary />
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary />
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary />
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public string ForeignId { get; set; }

    }
}
