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
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Data access and service class for <see cref="Rock.Model.Location"/> entities.
    /// </summary>
    public partial class LocationService 
    {
       
        /// <summary>
        /// Returns the first <see cref="Rock.Model.Location"/> where the address matches the provided address.  If no address is found with the provided values, 
        /// the address will be standardized. If there is still not a match, the address will be saved as a new location.
        /// </summary>
        /// <param name="street1">A <see cref="System.String"/> representing the Address Line 1 to search by.</param>
        /// <param name="street2">A <see cref="System.String"/> representing the Address Line 2 to search by.</param>
        /// <param name="city">A <see cref="System.String"/> representing the City to search by.</param>
        /// <param name="state">A <see cref="System.String"/> representing the State to search by.</param>
        /// <param name="zip">A <see cref="System.String"/> representing the Zip/Postal code to search by</param>
        /// <returns>The first <see cref="Rock.Model.Location"/> where an address match is found, if no match is found a new <see cref="Rock.Model.Location"/> is created and returned.</returns>
        public Location Get( string street1, string street2, string city, string state, string zip )
        {
            // Make sure it's not an empty address
            if ( string.IsNullOrWhiteSpace( street1 ) &&
                string.IsNullOrWhiteSpace( street2 ) &&
                string.IsNullOrWhiteSpace( city ) &&
                string.IsNullOrWhiteSpace( state ) &&
                string.IsNullOrWhiteSpace( zip ) )
            {
                return null;
            }

            // First check if a location exists with the entered values
            Location existingLocation = Repository.FirstOrDefault( t =>
                ( t.Street1 == street1 || ( street1 == null && t.Street1 == null ) ) &&
                ( t.Street2 == street2 || ( street2 == null && t.Street2 == null ) ) &&
                ( t.City == city || ( city == null && t.City == null ) ) &&
                ( t.State == state || ( state == null && t.State == null ) ) &&
                ( t.Zip == zip || ( zip == null && t.Zip == null ) ) );
            if ( existingLocation != null )
            {
                return existingLocation;
            }

            // If existing location wasn't found with entered values, try standardizing the values, and 
            // search for an existing value again
            var newLocation = new Location
            {
                Street1 = street1,
                Street2 = street2,
                City = city,
                State = state,
                Zip = zip
            };

            Standardize( newLocation, null );

            existingLocation = Repository.FirstOrDefault( t =>
                ( t.Street1 == newLocation.Street1 || ( newLocation.Street1 == null && t.Street1 == null ) ) &&
                ( t.Street2 == newLocation.Street2 || ( newLocation.Street2 == null && t.Street2 == null ) ) &&
                ( t.City == newLocation.City || ( newLocation.City == null && t.City == null ) ) &&
                ( t.State == newLocation.State || ( newLocation.State == null && t.State == null ) ) &&
                ( t.Zip == newLocation.Zip || ( newLocation.Zip == null && t.Zip == null ) ) );

            if ( existingLocation != null )
            {
                return existingLocation;
            }

            // Open Question on if we Add/Save the location if Standardize failed

            // If still no existing location, geocode the new location and save it.
            Geocode( newLocation, null );

            Add( newLocation, null );
            Save( newLocation, null );

            // refetch it from the database to make sure we get a valid .Id
            return Get(newLocation.Guid);
        }

        /// <summary>
        /// Returns a <see cref="Rock.Model.Location"/> by GeoPoint. If a match is not found,
        /// a new Location will be added based on the Geopoint.
        /// </summary>
        /// <param name="point">A <see cref="System.Data.Entity.Spatial.DbGeography"/> object
        ///     representing the Geopoint for the location.</param>
        /// <returns>The first <see cref="Rock.Model.Location"/> that matches the specified GeoPoint.</returns>
        public Location GetByGeoPoint( DbGeography point )
        {
            // get the first address that has a GeoPoint the value
            var result = Queryable()
                .Where( a => 
                    a.GeoPoint != null &&
                    a.GeoPoint.SpatialEquals(point))
                .FirstOrDefault();
            
            if ( result == null )
            {
                // if the Location can't be found, save the new location to the database
                Location newLocation = new Location
                {
                    GeoPoint = point,
                    Guid = Guid.NewGuid()
                };

                Add( newLocation, null );
                Save( newLocation, null );
                return Get( newLocation.Guid );
            }

            return result;
        }

        /// <summary>
        /// Returns the first <see cref="Rock.Model.Location"/> with a GeoFence that matches
        /// the specified GeoFence.
        /// </summary>
        /// <param name="fence">A <see cref="System.Data.Entity.Spatial.DbGeography"/> object that 
        ///  represents the GeoFence of the location to retrieve.</param>
        /// <returns>The <see cref="Rock.Model.Location"/> for the specified GeoFence. </returns>
        public Location GetByGeoFence( DbGeography fence )
        {

            // get the first address that has a GeoPoint or GeoFence that matches the value
            var result = Queryable()
                .Where( a => 
                    a.GeoFence != null &&
                    a.GeoFence.SpatialEquals( fence ) )
                .FirstOrDefault();

            if ( result == null )
            {
                // if the Location can't be found, save the new location to the database
                Location newLocation = new Location
                {
                    GeoFence = fence,
                    Guid = Guid.NewGuid()
                };

                Add( newLocation, null );
                Save( newLocation, null );
                return Get( newLocation.Guid );
            }

            return result;

        }

        /// <summary>
        /// Performs an Address Standardization on the provided <see cref="Rock.Model.Location"/>.
        /// </summary>
        /// <param name="location">A <see cref="Rock.Model.Location"/> to standardize.</param>
        /// <param name="personId">An <see cref="System.Int32"/> that represents the Id of the <see cref="Rock.Model.Person"/> requesting the address standardization.</param>
        public void Standardize( Location location, int? personId )
        {
            Model.ServiceLogService logService = new Model.ServiceLogService();
            string inputLocation = location.ToString();

            // Try each of the standardization services that were found through MEF
            foreach ( KeyValuePair<int, Lazy<Rock.Address.StandardizeComponent, Rock.Extension.IComponentData>> service in Rock.Address.StandardizeContainer.Instance.Components )
                if ( !service.Value.Value.AttributeValues.ContainsKey( "Active" ) || bool.Parse( service.Value.Value.AttributeValues["Active"][0].Value ) )
                {
                    string result;
                    bool success = service.Value.Value.Standardize( location, out result );

                    // Log the results of the service
                    Model.ServiceLog log = new Model.ServiceLog();
                    log.LogDateTime = RockDateTime.Now;
                    log.Type = "Location Standardize";
                    log.Name = service.Value.Metadata.ComponentName;
                    log.Input = inputLocation;
                    log.Result = result;
                    log.Success = success;
                    logService.Add( log, personId );
                    logService.Save( log, personId );

                    // If successful, set the results and stop processing
                    if ( success )
                    {
                        location.StandardizeAttemptedServiceType = service.Value.Metadata.ComponentName;
                        location.StandardizeAttemptedResult = result;
                        location.StandardizedDateTime = RockDateTime.Now;
                        break;
                    }
                }

            location.StandardizeAttemptedDateTime = RockDateTime.Now;
        }

        /// <summary>
        /// Performs a geolocation on the provided Location.
        /// </summary>
        /// <param name="location">The <see cref="Rock.Model.Location"/> entity to geocode.</param>
        /// <param name="personId">An <see cref="System.Int32"/> representing the Id of the <see cref="Rock.Model.Person"/> requesting the geolocation.</param>
        public void Geocode( Location location, int? personId )
        {
            Model.ServiceLogService logService = new Model.ServiceLogService();
            string inputLocation = location.ToString();

            // Try each of the geocoding services that were found through MEF

            foreach ( KeyValuePair<int, Lazy<Rock.Address.GeocodeComponent, Rock.Extension.IComponentData>> service in Rock.Address.GeocodeContainer.Instance.Components )
                if ( !service.Value.Value.AttributeValues.ContainsKey( "Active" ) || bool.Parse( service.Value.Value.AttributeValues["Active"][0].Value ) )
                {
                    string result;
                    bool success = service.Value.Value.Geocode( location, out result );

                    // Log the results of the service
                    Model.ServiceLog log = new Model.ServiceLog();
                    log.LogDateTime = RockDateTime.Now;
                    log.Type = "Location Geocode";
                    log.Name = service.Value.Metadata.ComponentName;
                    log.Input = inputLocation;
                    log.Result = result;
                    log.Success = success;
                    logService.Add( log, personId );
                    logService.Save( log, personId );

                    // If successful, set the results and stop processing
                    if ( success )
                    {
                        location.GeocodeAttemptedServiceType = service.Value.Metadata.ComponentName;
                        location.GeocodeAttemptedResult = result;
                        location.GeocodedDateTime = RockDateTime.Now;
                        break;
                    }
                }

            location.GeocodeAttemptedDateTime = RockDateTime.Now;
        }

        /// <summary>
        /// Returns an enumerable collection of <see cref="Rock.Model.Location">Locations</see> that are descendants of a <see cref="Rock.Model.Location"/>
        /// </summary>
        /// <param name="parentLocationId">A <see cref="System.Int32"/> representing the Id of the <see cref="Rock.Model.Location"/></param>
        /// <returns>A collection of <see cref="Rock.Model.Location"/> entities that are descendants of the provided parent <see cref="Rock.Model.Location"/>.</returns>
        public IEnumerable<Location> GetAllDescendents( int parentLocationId )
        {
            return Repository.ExecuteQuery(
                @"
                with CTE as (
                select * from [Location] where [ParentLocationId]={0}
                union all
                select [a].* from [Location] [a]
                inner join CTE pcte on pcte.Id = [a].[ParentLocationId]
                )
                select * from CTE
                ", parentLocationId );
        }
    }
}
