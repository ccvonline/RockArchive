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
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Web;
using Rock.Model;

namespace Rock.Storage.Provider
{
    /// <summary>
    /// Storage provider for saving binary files to Rock database
    /// </summary>
    [Description( "Database-driven file storage" )]
    [Export( typeof( ProviderComponent ) )]
    [ExportMetadata( "ComponentName", "Database" )]
    public class Database : ProviderComponent
    {
        /// <summary>
        /// Removes the file from the external storage medium associated with the provider.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="context">The context.</param>
        public override void RemoveFile( BinaryFile file, HttpContext context )
        {
            // Database storage just stores everything in the BinaryFile table, so there is no external file data to delete
        }

        /// <summary>
        /// Saves the file to the external storage medium associated with the provider.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="context">The context.</param>
        public override void SaveFile( BinaryFile file, HttpContext context )
        {
            // Database storage just stores everything in the BinaryFile table, so there is no external file data to save, but we do need to set the Url
            file.Url = GenerateUrl( file );
        }

        /// <summary>
        /// Gets the file bytes from the external storage medium associated with the provider.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override byte[] GetFileContent( BinaryFile file, HttpContext context )
        {
            if (file.Data != null)
            {
                return file.Data.Content;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Generate a URL for the file based on the rules of the StorageProvider
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public override string GenerateUrl( BinaryFile file)
        {
            string urlPath;

            switch ( file.MimeType.ToLower() )
            {
                case "image/jpeg":
                case "image/gif":
                case "image/png":
                case "image/bmp":
                    urlPath = "~/GetImage.ashx";
                    break;
                default:
                    urlPath = "~/GetFile.ashx";
                    break;
            }

            return string.Format( "{0}?guid={1}", urlPath, file.Guid );
        }
    }
}
