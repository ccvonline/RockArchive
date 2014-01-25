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
using System.Net;
using System.Web.Http;

using Rock.Model;
using Rock.Rest.Filters;

namespace Rock.Rest.Controllers
{
    /// <summary>
    /// Blocks REST API
    /// </summary>
    public partial class BlocksController : IHasCustomRoutes 
    {
        /// <summary>
        /// Add Custom route needed for block move
        /// </summary>
        /// <param name="routes"></param>
        public void AddRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "BlockMove",
                routeTemplate: "api/blocks/move/{id}",
                defaults: new
                {
                    controller = "blocks",
                    action = "move"
                } );
        }

        /// <summary>
        /// Moves a block from one zone to another
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        [HttpPut]
        [Authenticate]
        public void Move( int id, Block block )
        {
            var user = CurrentUser();
            if ( user != null )
            {
                var service = new BlockService();
                block.Id = id;
                Block model;
                if ( !service.TryGet( id, out model ) )
                    throw new HttpResponseException( HttpStatusCode.NotFound );

                if ( !model.IsAuthorized( "Edit", user.Person ) )
                    throw new HttpResponseException( HttpStatusCode.Unauthorized );

                if ( model.LayoutId.HasValue && model.LayoutId != block.LayoutId )
                    Rock.Web.Cache.PageCache.FlushLayoutBlocks( model.LayoutId.Value );

                if ( block.LayoutId.HasValue )
                    Rock.Web.Cache.PageCache.FlushLayoutBlocks( block.LayoutId.Value );
                else
                {
                    var page = Rock.Web.Cache.PageCache.Read( block.PageId.Value );
                    page.FlushBlocks();
                }

                model.Zone = block.Zone;
                model.PageId = block.PageId;
                model.LayoutId = block.LayoutId;

                if ( model.IsValid )
                {
                    model.Order = service.GetMaxOrder( model );
                    service.Save( model, user.Person.PrimaryAlias );
                }
                else
                    throw new HttpResponseException( HttpStatusCode.BadRequest );
            }
            else
                throw new HttpResponseException( HttpStatusCode.Unauthorized );
        }
    }
}
