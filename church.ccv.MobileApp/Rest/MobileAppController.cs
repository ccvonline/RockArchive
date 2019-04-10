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
using church.ccv.MobileApp;
using church.ccv.MobileApp.Models;
using System.Web.Http;
using System.Web.Routing;
using Rock.Rest.Filters;
using Rock.Security;
using Rock.Data;
using System.Collections.Generic;
using System.Linq;

namespace church.ccv.MobileApp.Rest
{
    public class MobileAppController : Rock.Rest.ApiControllerBase
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route( "api/MobileApp/LaunchData" )]
        [Authenticate, Secured]
        public HttpResponseMessage GetLaunchData(  )
        {
            LaunchData launchData = Launch.GetLaunchData( );

            StringContent restContent = new StringContent( JsonConvert.SerializeObject( launchData ), Encoding.UTF8, "application/json" );
            return new HttpResponseMessage()
            {
                Content = restContent
            };
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route( "api/MobileApp/PersonData" )]
        [Authenticate, Secured]
        public HttpResponseMessage GetPersonData( string userID )
        {
            HttpStatusCode statusCode;
            PersonData personData = Util.GetPersonData( userID, out statusCode );

            StringContent restContent = new StringContent( JsonConvert.SerializeObject( personData ), Encoding.UTF8, "application/json" );
            return new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = restContent
            };
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/MobileApp/GroupsByLocation")]
        [Authenticate, Secured]
        public HttpResponseMessage GetGroupsByLocation( int groupTypeId, int locationId, int skip, int top, bool publicOnly )
        {
            List<GroupResult> groups = GroupFinder.GetGroupsByLocation( groupTypeId, locationId, skip, top, publicOnly );

            HttpResponseMessage response = null;

            if (groups != null )
            {
                StringContent restContent = new StringContent(JsonConvert.SerializeObject( groups ), Encoding.UTF8, "application/json");

                response = new HttpResponseMessage()
                {
                    Content = restContent
                };
            }
            else
            {
                response = new HttpResponseMessage( HttpStatusCode.NotFound );
            }

            return response;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/MobileApp/GroupInfo")]
        [Authenticate, Secured]
        public HttpResponseMessage GetGroupInfo( int groupId )
        {
            GroupInfo groupInfo = null;
            bool result = GroupFinder.GetGroupInfo(groupId, out groupInfo);

            HttpResponseMessage response = null;

            if (result)
            {
                StringContent restContent = new StringContent(JsonConvert.SerializeObject(groupInfo), Encoding.UTF8, "application/json");

                response = new HttpResponseMessage()
                {
                    Content = restContent
                };
            }
            else
            {
                response = new HttpResponseMessage( HttpStatusCode.NotFound );
            }

            return response;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route( "api/MobileApp/Login" )]
        [Authenticate, Secured]
        public HttpResponseMessage Login( [FromBody]LoginParameters loginParameters )
        {
            // default to an internal error
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            HttpContent httpContent = null;

            do
            {
                RockContext rockContext = new RockContext( );

                // require login parameters
                if( loginParameters == null ) break;

                // verify their user login
                var userLoginService = new UserLoginService( rockContext );
                var userLogin = userLoginService.GetByUserName( loginParameters.Username );

                if ( userLogin == null || userLogin.EntityType == null ) { statusCode = HttpStatusCode.Unauthorized; break; }


                // verify their password
                var component = AuthenticationContainer.GetComponent(userLogin.EntityType.Name);
                if ( component == null || component.IsActive == false ) { statusCode = HttpStatusCode.Unauthorized; break; }
                
                if ( component.Authenticate( userLogin, loginParameters.Password ) == false ) { statusCode = HttpStatusCode.Unauthorized; break; }
                
                
                // ensure there's a person associated with this login.
                if ( userLogin.PersonId.HasValue == false ) { statusCode = HttpStatusCode.Unauthorized; break; }
                
                // if this account hasn't been confirmed yet, do not let them login. This prevents someone from
                // trying to do a password reset on a stolen email address.
                if( userLogin.IsConfirmed == false ) { statusCode = HttpStatusCode.Unauthorized; break; }
                
                // all good! build and return the response
                statusCode = HttpStatusCode.OK;
            }
            while( false );
            
            // build and return the response
            HttpResponseMessage response = new HttpResponseMessage( ) { StatusCode = statusCode, Content = httpContent };
            return response;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route( "api/MobileApp/RegisterNewUser" )]
        [Authenticate, Secured]
        public HttpResponseMessage RegisterNewUser( [FromBody]RegAccountData regAccountData )
        {
            // default to an internal error
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            HttpContent httpContent = null;

            do
            {
                // require reg parameters
                if( regAccountData == null ) break;

                RockContext rockContext = new RockContext( );
                UserLoginService userLoginService = new UserLoginService( rockContext );

                // first, if the user name already exists, simply stop and tell them the username already exists.
                UserLogin userLogin = userLoginService.GetByUserName( regAccountData.Username );
                if( userLogin != null ) { statusCode = HttpStatusCode.Unauthorized; break; }



                // since the username doesn't exist, make sure this person doesn't already exist. If they do, we need to deny
                // this registration so we don't end up with duplicates
                PersonService personService = new PersonService( rockContext );
                IEnumerable<Person> personList = personService.GetByMatch( regAccountData.FirstName, regAccountData.LastName, regAccountData.Email );
                if( personList.Count( ) > 0 )
                {
                    // to help the user, find out if the person they're trying to register as 
                    // has a username attached or not.
                    bool personHasUsername = false;
                    foreach( var person in personList )
                    {
                        var loginList = userLoginService.GetByPersonId( person.Id );
                        if ( loginList.Count( ) > 0 )
                        {
                            personHasUsername = true;
                            break;
                        }
                    }

                    // there IS a username attached, so return NotAcceptable (just arbitrary for the mobile app)
                    if( personHasUsername == true )
                    {
                        statusCode = HttpStatusCode.NotAcceptable;
                    }
                    else
                    {
                        // otherwise use Conflict, meaning the person exists but there are no usernames
                        statusCode = HttpStatusCode.Conflict;
                    }

                    break;
                }

                
                // we know we can create the person. So first, begin tracking who made these changes, and then
                // create the person with their login
                System.Web.HttpContext.Current.Items.Add( "CurrentPerson", GetPerson() );
                statusCode = Util.RegisterNewPerson( regAccountData );
                if( statusCode != HttpStatusCode.Created ) break;
                
                
                // all good! build and return the response
                statusCode = HttpStatusCode.OK;
            }
            while( false );
            
            // build and return the response
            HttpResponseMessage response = new HttpResponseMessage( ) { StatusCode = statusCode, Content = httpContent };
            return response;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route( "api/MobileApp/GroupRegistration" )]
        [Authenticate, Secured]
        public HttpResponseMessage GroupRegistration( [FromBody] GroupRegModel regModel )
        {
            bool success = GroupFinder.RegisterPersonInGroup( regModel );
            
            return new HttpResponseMessage( success == true ? HttpStatusCode.OK : HttpStatusCode.NotFound );
        }

        [HttpPost]
        [System.Web.Http.Route( "api/Auth/FacebookLogin" )]
        [Authenticate, Secured]
        public void FacebookLogin( [FromBody]Rock.Security.ExternalAuthentication.Facebook.FacebookUser facebookUser )
        {
            string userName = Rock.Security.ExternalAuthentication.Facebook.GetFacebookUserName( facebookUser );
            if ( !string.IsNullOrWhiteSpace( userName ) )
            {
                Rock.Security.Authorization.SetAuthCookie( userName, false, false );
            }
            else
            {
                throw new HttpResponseException( HttpStatusCode.Unauthorized );
            }
        }

        // Inherit StringWriter so we can set the encoding, which is protected
        public sealed class StringWriterWithEncoding : StringWriter
        {
            private readonly Encoding encoding;

            public StringWriterWithEncoding (Encoding encoding)
            {
                this.encoding = encoding;
            }

            public override Encoding Encoding
            {
                get { return encoding; }
            }
        }
    }
}
