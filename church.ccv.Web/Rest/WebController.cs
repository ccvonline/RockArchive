﻿// <copyright>
// Copyright by the Spark Development Network
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
using System.IO;
using System.Net.Http;
using System.Text;
using Rock.Model;
using Newtonsoft.Json;
using System.Net;
using System.Web.Http;
using Rock.Security;
using Rock.Data;
using System.Collections.Generic;
using System.Linq;
using Rock.Rest.Filters;
using RestSharp;

namespace church.ccv.Web.Rest
{
    public class WebController : Rock.Rest.ApiControllerBase
    {
        public enum LoginResponse
        {
            Invalid,
            LockedOut,
            Confirm,
            Success
        }

        /// <summary>
        /// This should only be called by Rock Blocks.
        /// See "RockWeb\Plugins\church_ccv\Core\Login.ascx" for an example.
        /// </summary>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route( "api/Web/Login" )]
        //[Authenticate, Secured]
        public HttpResponseMessage Login( string username, string password, bool persist )
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            StringContent restContent = null;
            
            LoginResponse loginResponse = LoginResponse.Invalid;

            RockContext rockContext = new RockContext( );
            var userLoginService = new UserLoginService(rockContext);

            var userLogin = userLoginService.GetByUserName( username );
            if ( userLogin != null && userLogin.EntityType != null)
            {
                var component = AuthenticationContainer.GetComponent(userLogin.EntityType.Name);
                if (component != null && component.IsActive && !component.RequiresRemoteAuthentication)
                {
                    // see if the credentials are valid
                    if ( component.Authenticate( userLogin, password ) )
                    {
                        // if the account isn't locked or needing confirmation
                        if ( ( userLogin.IsConfirmed ?? true ) && !(userLogin.IsLockedOut ?? false ) )
                        {
                            // then proceed to the final step, validating them with PMG2's site
                            if ( TryPMG2Login( username, password ) )
                            {
                                // generate their cookie
                                UserLoginService.UpdateLastLogin( username );
                                Rock.Security.Authorization.SetAuthCookie( username, persist, false );

                                // no issues!
                                loginResponse = LoginResponse.Success;
                            }
                        }
                        else
                        {
                            if ( userLogin.IsLockedOut ?? false )
                            {
                                loginResponse = LoginResponse.LockedOut;
                            }
                            else
                            {
                                loginResponse = LoginResponse.Confirm;
                            }
                        }
                    }
                }
            }

            // build and return the response
            restContent = new StringContent( loginResponse.ToString( ), Encoding.UTF8, "text/plain");
            HttpResponseMessage response = new HttpResponseMessage( ) { StatusCode = statusCode, Content = restContent };

            return response;
        }

        protected bool TryPMG2Login( string username, string password )
        {
            // contact PMG2's site and attempt to login with the same credentials
            var restClient = new RestClient(
                string.Format( "https://apistaging.ccv.church/auth?user[username]={0}&user[password]={1}", username, password ) );

            var restRequest = new RestRequest( Method.POST );
            var restResponse = restClient.Execute( restRequest );

            if ( restResponse.StatusCode == HttpStatusCode.Created )
            {
                return true;
            }

            return false;
        }
    }
}
