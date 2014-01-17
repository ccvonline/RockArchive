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
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Location Service class
    /// </summary>
    public partial class LocationService : Service<Location>
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
        /// <param name="repository">The repository.</param>
        public LocationService(IRepository<Location> repository) : base(repository)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public LocationService(RockContext context) : base(context)
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
        public bool CanDelete( Location item, out string errorMessage )
        {
            errorMessage = string.Empty;
 
            if ( new Service<Campus>().Queryable().Any( a => a.LocationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Location.FriendlyTypeName, Campus.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Device>().Queryable().Any( a => a.LocationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Location.FriendlyTypeName, Device.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Location>().Queryable().Any( a => a.ParentLocationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Location.FriendlyTypeName, Location.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class LocationExtensionMethods
    {
        /// <summary>
        /// Clones this Location object to a new Location object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static Location Clone( this Location source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as Location;
            }
            else
            {
                var target = new Location();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another Location object to this Location object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this Location target, Location source )
        {
            target.ParentLocationId = source.ParentLocationId;
            target.Name = source.Name;
            target.IsActive = source.IsActive;
            target.LocationTypeValueId = source.LocationTypeValueId;
            target.GeoPoint = source.GeoPoint;
            target.GeoFence = source.GeoFence;
            target.IsNamedLocation = source.IsNamedLocation;
            target.Street1 = source.Street1;
            target.Street2 = source.Street2;
            target.City = source.City;
            target.State = source.State;
            target.Country = source.Country;
            target.Zip = source.Zip;
            target.FullAddress = source.FullAddress;
            target.AssessorParcelId = source.AssessorParcelId;
            target.StandardizeAttemptedDateTime = source.StandardizeAttemptedDateTime;
            target.StandardizeAttemptedServiceType = source.StandardizeAttemptedServiceType;
            target.StandardizeAttemptedResult = source.StandardizeAttemptedResult;
            target.StandardizedDateTime = source.StandardizedDateTime;
            target.GeocodeAttemptedDateTime = source.GeocodeAttemptedDateTime;
            target.GeocodeAttemptedServiceType = source.GeocodeAttemptedServiceType;
            target.GeocodeAttemptedResult = source.GeocodeAttemptedResult;
            target.GeocodedDateTime = source.GeocodedDateTime;
            target.PrinterDeviceId = source.PrinterDeviceId;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
