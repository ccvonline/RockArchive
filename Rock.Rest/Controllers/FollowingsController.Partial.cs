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
using System.Linq;
using System.Web.Http;

using Rock.Rest.Filters;

namespace Rock.Rest.Controllers
{
    /// <summary>
    /// Followings REST API
    /// </summary>
    public partial class FollowingsController : IHasCustomRoutes
    {
        /// <summary>
        /// Adds the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public void AddRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "FollowingsDelete",
                routeTemplate: "api/Followings/{EntityTypeId}/{EntityId}/{PersonId}",
                defaults: new
                {
                    controller = "Followings",
                } );
        }

        /// <summary>
        /// Deletes the following record(s).
        /// </summary>
        /// <param name="entityTypeId">The entity type identifier.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="personId">The person identifier.</param>
        [Authenticate, Secured]
        public virtual void Delete( int entityTypeId, int entityId, int personId )
        {
            SetProxyCreation( true );

            foreach ( var following in Service.Queryable()
                .Where( f =>
                    f.EntityTypeId == entityTypeId &&
                    f.EntityId == entityId &&
                    f.PersonAlias.PersonId == personId ) )
            {
                CheckCanEdit( following );
                Service.Delete( following );
            }

            Service.Context.SaveChanges();
        }

    }
}
