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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using Rock.Model;
using Rock.Rest.Filters;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace Rock.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GroupsController : IHasCustomRoutes
    {
        /// <summary>
        /// Adds the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public void AddRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "GroupsGetChildren",
                routeTemplate: "api/Groups/GetChildren/{id}/{rootGroupId}/{limitToSecurityRoleGroups}/{groupTypeIds}",
                defaults: new
                {
                    controller = "Groups",
                    action = "GetChildren"
                } );

            routes.MapHttpRoute(
                name: "GroupsMapInfo",
                routeTemplate: "api/Groups/GetMapInfo/{Groupid}",
                defaults: new
                {
                    controller = "Groups",
                    action = "GetMapInfo"
                } );

            routes.MapHttpRoute(
                name: "GroupsChildMapInfo",
                routeTemplate: "api/Groups/GetMapInfo/{Groupid}/Children",
                defaults: new
                {
                    controller = "Groups",
                    action = "GetChildMapInfo"
                } );
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="rootGroupId">The root group id.</param>
        /// <param name="limitToSecurityRoleGroups">if set to <c>true</c> [limit to security role groups].</param>
        /// <param name="groupTypeIds">The group type ids.</param>
        /// <returns></returns>
        [Authenticate, Secured]
        public IQueryable<TreeViewItem> GetChildren( int id, int rootGroupId, bool limitToSecurityRoleGroups, string groupTypeIds )
        {
            var qry = ((GroupService)Service).GetNavigationChildren( id, rootGroupId, limitToSecurityRoleGroups, groupTypeIds );

            List<Group> groupList = new List<Group>();
            List<TreeViewItem> groupNameList = new List<TreeViewItem>();

            var person = GetPerson();

            foreach ( var group in qry.OrderBy( g => g.Name ) )
            {
                if ( group.IsAuthorized( Rock.Security.Authorization.VIEW, person ) )
                {
                    groupList.Add( group );
                    var treeViewItem = new TreeViewItem();
                    treeViewItem.Id = group.Id.ToString();
                    treeViewItem.Name = group.Name;

                    // if there a IconCssClass is assigned, use that as the Icon.
                    var groupType = Rock.Web.Cache.GroupTypeCache.Read( group.GroupTypeId );
                    if ( groupType != null )
                    {
                        treeViewItem.IconCssClass = groupType.IconCssClass;
                    }

                    groupNameList.Add( treeViewItem );
                }
            }

            // try to quickly figure out which items have Children
            List<int> resultIds = groupList.Select( a => a.Id ).ToList();

            var qryHasChildren = from x in Get().Select( a => a.ParentGroupId )
                                    where resultIds.Contains( x.Value )
                                    select x.Value;

            var qryHasChildrenList = qryHasChildren.ToList();

            foreach ( var g in groupNameList )
            {
                int groupId = int.Parse( g.Id );
                g.HasChildren = qryHasChildrenList.Any( a => a == groupId );
            }

            return groupNameList.AsQueryable();
        }

        /// <summary>
        /// Gets the map information.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Web.Http.HttpResponseException">
        /// </exception>
        [Authenticate, Secured]
        public IQueryable<MapItem> GetMapInfo( int groupId )
        {
            var group = ( (GroupService)Service ).Queryable("GroupLocations.Location")
                .Where(g => g.Id == groupId)
                .FirstOrDefault();

            if (group != null)
            {
                var person = GetPerson();

                if (group.IsAuthorized( Rock.Security.Authorization.VIEW, person ))
                {
                    var mapItems = new List<MapItem>();
                    foreach ( var location in group.GroupLocations
                        .Where( l => l.Location.GeoPoint != null || l.Location.GeoFence != null )
                        .Select( l => l.Location ) )
                    {
                        var mapItem = new MapItem( location );
                        mapItem.EntityTypeId = EntityTypeCache.Read( "Rock.Model.Group" ).Id;
                        mapItem.EntityId = group.Id;
                        mapItem.Name = group.Name;
                        if ( mapItem.Point != null || mapItem.PolygonPoints.Any() )
                        {
                            mapItems.Add( mapItem );
                        }
                    }

                    return mapItems.AsQueryable();
                }
                else
                {
                    throw new HttpResponseException( HttpStatusCode.Unauthorized );
                }
            }
            else
            {
                throw new HttpResponseException( HttpStatusCode.BadRequest );
            }
        }

        [Authenticate, Secured]
        public IQueryable<MapItem> GetChildMapInfo( int groupId )
        {
            var person = GetPerson();

            var mapItems = new List<MapItem>();

            foreach ( var group in ( (GroupService)Service ).Queryable( "GroupLocations.Location" )
                .Where( g => g.ParentGroupId == groupId ))
            {
                if ( group != null && group.IsAuthorized( Rock.Security.Authorization.VIEW, person ) )
                {
                    foreach ( var location in group.GroupLocations
                        .Where( l => l.Location.GeoPoint != null || l.Location.GeoFence != null )
                        .Select( l => l.Location ) )
                    {
                        var mapItem = new MapItem( location );
                        mapItem.EntityTypeId = EntityTypeCache.Read( "Rock.Model.Group" ).Id;
                        mapItem.EntityId = group.Id;
                        mapItem.Name = group.Name;
                        if ( mapItem.Point != null || mapItem.PolygonPoints.Any() )
                        {
                            mapItems.Add( mapItem );
                        }
                    }

                }
            }

            return mapItems.AsQueryable();
        }

    }
}

    
