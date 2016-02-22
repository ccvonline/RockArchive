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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Rock.Data;
using Rock.Model;
using Rock.Rest.Filters;
using Rock.Utility;

namespace Rock.Rest.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public partial class BinaryFilesController
    {
        /// <summary>
        /// Uploads a file and stores it as a binary file
        /// </summary>
        /// <param name="binaryFileTypeId"></param>
        /// <returns></returns>
        [Authenticate, Secured]
        [HttpPost]
        [System.Web.Http.Route( "api/BinaryFiles/Upload" )]
        public HttpResponseMessage Upload( Guid binaryFileTypeGuid )
        {
            try
            {
                var rockContext = new RockContext();
                var binaryFileType = new BinaryFileTypeService( rockContext ).Get( binaryFileTypeGuid );

                if ( binaryFileType == null )
                {
                    GenerateResponse( HttpStatusCode.BadRequest, "Invalid binary file type guid" );
                }

                return Upload( binaryFileType.Id );
            }
            catch ( HttpResponseException exception )
            {
                return exception.Response;
            }
            catch
            {
                return new HttpResponseMessage( HttpStatusCode.InternalServerError )
                {
                    Content = new StringContent( "Unhandled exception" )
                };
            }
        }

        /// <summary>
        /// Uploads a file and stores it as a binary file
        /// </summary>
        /// <param name="binaryFileTypeId"></param>
        /// <returns></returns>
        [Authenticate, Secured]
        [HttpPost]
        [System.Web.Http.Route( "api/BinaryFiles/Upload" )]
        public HttpResponseMessage Upload( int binaryFileTypeId )
        {
            try
            {
                var rockContext = new RockContext();
                var context = HttpContext.Current;
                var files = context.Request.Files;
                var uploadedFile = files.AllKeys.Select( fk => files[fk] ).FirstOrDefault();
                var binaryFileType = new BinaryFileTypeService( rockContext ).Get( binaryFileTypeId );

                if ( uploadedFile == null )
                {
                    GenerateResponse( HttpStatusCode.BadRequest, "No file was sent" );
                }

                if ( binaryFileType == null )
                {
                    GenerateResponse( HttpStatusCode.InternalServerError, "There is no person image file type" );
                }

                var currentUser = UserLoginService.GetCurrentUser();
                var currentPerson = currentUser != null ? currentUser.Person : null;

                if ( !binaryFileType.IsAuthorized( Rock.Security.Authorization.EDIT, currentPerson ) )
                {
                    GenerateResponse( HttpStatusCode.Unauthorized, "Not authorized to upload this type of file" );
                }

                var binaryFileService = new BinaryFileService( rockContext );
                var binaryFile = new BinaryFile();

                if ( uploadedFile != null )
                {
                    binaryFileService.Add( binaryFile );

                    binaryFile.IsTemporary = false;
                    binaryFile.BinaryFileTypeId = binaryFileType.Id;
                    binaryFile.MimeType = uploadedFile.ContentType;
                    binaryFile.FileName = Path.GetFileName( uploadedFile.FileName );
                    binaryFile.ContentStream = ImageUtilities.GetFileContentStream( uploadedFile, true, true );
                }

                rockContext.SaveChanges();

                return new HttpResponseMessage( HttpStatusCode.Created )
                {
                    Content = new StringContent( binaryFile.Id.ToString() )
                };
            }
            catch ( HttpResponseException exception )
            {
                return exception.Response;
            }
            catch ( InvalidDataException idException )
            {
                return new HttpResponseMessage( HttpStatusCode.BadRequest )
                {
                    Content = new StringContent( idException.Message )
                };
            }
            catch
            {
                return new HttpResponseMessage( HttpStatusCode.InternalServerError )
                {
                    Content = new StringContent( "Unhandled exception" )
                };
            }
        }

        /// <summary>
        /// Generates the response.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        private void GenerateResponse( HttpStatusCode code, string message = null )
        {
            var response = new HttpResponseMessage( code );

            if ( !string.IsNullOrWhiteSpace( message ) )
            {
                response.Content = new StringContent( message );
            }

            throw new HttpResponseException( response );
        }
    }
}