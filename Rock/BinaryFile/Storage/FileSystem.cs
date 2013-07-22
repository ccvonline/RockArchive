﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Web;
using Rock.Attribute;
using Rock.Model;

namespace Rock.BinaryFile.Storage
{
    [Description( "File System-driven document storage" )]
    [Export( typeof( StorageComponent ) )]
    [ExportMetadata( "ComponentName", "FileSystem" )]
    [TextField( "Path Root", "Root path where the files will be stored on disk." )]
    [TextField( "Folder Name", "Optional Folder name to place files on disk.", false )]
    public class FileSystem : StorageComponent
    {
        public string PathRoot
        {
            get { return GetAttributeValue( "PathRoot" ); }
        }

        public string FolderName
        {
            get { return GetAttributeValue( "FolderName" ); }
        }

        /// <summary>
        /// Saves the files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="personId"></param>
        public override void SaveFiles( IEnumerable<Model.BinaryFile> files, int? personId )
        {
            var fileService = new BinaryFileService();
            
            foreach ( var file in files )
            {
                if ( file.Data == null )
                {
                    throw new ArgumentException( "File Data must not be null." );
                }

                var url = GetUrl( file );
                var physicalPath = GetPhysicalPath( url );
                File.WriteAllBytes( physicalPath, file.Data.Content );

                // Set Data to null after successful OS save so the the binary data is not 
                // written into the database.
                file.Data = null;
                file.Url = FolderName;
                fileService.Save( file, personId );
            }
        }

        /// <summary>
        /// Removes the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="personId"></param>
        public override void RemoveFile( Model.BinaryFile file, int? personId )
        {
            var fileService = new BinaryFileService();
            File.Delete( HttpContext.Current.Server.MapPath( file.Url ) );
            fileService.Delete( file, personId );
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        public override string GetUrl( Model.BinaryFile file, int? height = null, int? width = null )
        {
            if ( string.IsNullOrWhiteSpace( file.FileName ) )
            {
                return null;
            }

            var urlBuilder = new StringBuilder();

            urlBuilder.Append( PathRoot );

            if ( !PathRoot.EndsWith( "/" ) )
            {
                urlBuilder.Append( "/" );
            }

            if ( !string.IsNullOrWhiteSpace( FolderName ) )
            {
                urlBuilder.Append( FolderName );

                if ( !FolderName.EndsWith( "/" ) )
                {
                    urlBuilder.Append( "/" );
                }
            }

            urlBuilder.Append( file.FileName );
            return urlBuilder.ToString();
        }

        private string GetPhysicalPath( string path )
        {
            if ( path.StartsWith( "C:" ) || path.StartsWith( "\\\\" ) )
            {
                return path;
            }
            
            if ( path.StartsWith( "http:" ) || path.StartsWith( "https:" ) )
            {
                return HttpContext.Current.Server.MapPath( path );
            }

            return Path.Combine( AppDomain.CurrentDomain.BaseDirectory, path );
        }
    }
}
