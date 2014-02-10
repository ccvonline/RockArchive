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
using System.Linq;
using System.Net;
using System.Web.Http;

using Rock.Model;
using Rock.Rest.Filters;

namespace Rock.Rest.Controllers
{
    /// <summary>
    /// TaggedItems REST API
    /// </summary>
    public partial class TagsController : IHasCustomRoutes
    {
        /// <summary>
        /// Add Custom route for flushing cached attributes
        /// </summary>
        /// <param name="routes"></param>
        public void AddRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "TagNamesAvail",
                routeTemplate: "api/tags/availablenames/{entityTypeId}/{ownerid}/{name}/{entityguid}/{entityqualifier}/{entityqualifiervalue}",
                defaults: new
                {
                    controller = "tags",
                    action = "availablenames",
                    entityqualifier = RouteParameter.Optional,
                    entityqualifiervalue = RouteParameter.Optional
                } );

            routes.MapHttpRoute(
                name: "TagsByEntity",
                routeTemplate: "api/tags/{entityTypeId}/{ownerid}/{name}/{entityqualifier}/{entityqualifiervalue}",
                defaults: new
                {
                    controller = "tags",
                    entityqualifier = RouteParameter.Optional,
                    entityqualifiervalue = RouteParameter.Optional
                } );
        }

        [Authenticate, Secured]
        [HttpGet]
        public Tag Get( int entityTypeId, int ownerId, string name )
        {
            return Get( entityTypeId, ownerId, name, string.Empty, string.Empty );
        }

        [Authenticate, Secured]
        [HttpGet]
        public Tag Get( int entityTypeId, int ownerId, string name, string entityQualifier )
        {
            return Get( entityTypeId, ownerId, name, entityQualifier, string.Empty );
        }

        [Authenticate, Secured]
        [HttpGet]
        public Tag Get( int entityTypeId, int ownerId, string name, string entityQualifier, string entityQualifierValue )
        {
            var service = new TagService();
            var tag = service.Get( entityTypeId, entityQualifier, entityQualifierValue, ownerId ).FirstOrDefault(t => t.Name == name);

            if ( tag != null )
                return tag;
            else
                throw new HttpResponseException( HttpStatusCode.NotFound );
        }

        [Authenticate, Secured]
        [HttpGet]
        public IQueryable<Tag> AvailableNames( int entityTypeId, int ownerId, Guid entityGuid, string name )
        {
            return AvailableNames( entityTypeId, ownerId, entityGuid, name, string.Empty, string.Empty );
        }

        [Authenticate, Secured]
        [HttpGet]
        public IQueryable<Tag> AvailableNames( int entityTypeId, int ownerId, Guid entityGuid, string name, string entityQualifier )
        {
            return AvailableNames( entityTypeId, ownerId, entityGuid, name, entityQualifier, string.Empty );
        }

        [Authenticate, Secured]
        [HttpGet]
        public IQueryable<Tag> AvailableNames( int entityTypeId, int ownerId, Guid entityGuid, string name, string entityQualifier, string entityQualifierValue )
        {
            var service = new TagService();
            return service.Get( entityTypeId, entityQualifier, entityQualifierValue, ownerId )
                .Where( t => 
                    t.Name.StartsWith(name) && 
                    !t.TaggedItems.Any( i => i.EntityGuid == entityGuid ))
                .OrderBy( t => t.Name );
        }

    }
}
