﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using Microsoft.Web.XmlTransform;
using NuGet;
using Rock.Model;

namespace Rock.Services.NuGet
{
    /// <summary>
    /// 
    /// </summary>
    public class WebProjectSystem : PhysicalFileSystem, IProjectSystem, IFileSystem
    {
        private static readonly string _transformFilePrefix = ".rock.xdt";

        /// <summary>
        /// 
        /// </summary>
        private bool _isBindingRedirectSupported = false;

        /// <summary>
        /// Gets a value indicating whether this instance is binding redirect supported.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is binding redirect supported; otherwise, <c>false</c>.
        /// </value>
        public bool IsBindingRedirectSupported { get { return _isBindingRedirectSupported; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebProjectSystem" /> class.
        /// </summary>
        /// <param name="siteRoot">The site root.</param>
        public WebProjectSystem( string siteRoot ) : base( siteRoot ) {    }

        /// <summary>
        /// Adds the framework reference.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void AddFrameworkReference( string name )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the reference.
        /// </summary>
        /// <param name="referencePath">The reference path.</param>
        /// <param name="stream">The stream.</param>
        public void AddReference( string referencePath, Stream stream )
        {
            string fileName = Path.GetFileName( referencePath );
            string fullPath = this.GetFullPath( GetReferencePath( fileName ) );
            this.AddFile( fullPath, stream );
        }

        /// <summary>
        /// Gets the reference path.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        protected virtual string GetReferencePath( string name )
        {
            return Path.Combine( "bin", name );
        }

        /// <summary>
        /// Determines whether [is supported file] [the specified path].
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if [is supported file] [the specified path]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSupportedFile( string path )
        {
            return ( !path.StartsWith( "tools", StringComparison.OrdinalIgnoreCase ) && !Path.GetFileName( path ).Equals( "app.config", StringComparison.OrdinalIgnoreCase ) );
        }

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        public string ProjectName
        {
            get { return Root; }
        }

        /// <summary>
        /// References the exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool ReferenceExists( string name )
        {
            string referencePath = GetReferencePath( name );
            return FileExists( referencePath );
        }

        /// <summary>
        /// Removes the reference.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveReference( string name )
        {
            DeleteFile( GetReferencePath( name ) );
            if ( !this.GetFiles( "bin", false ).Any<string>() )
            {
                DeleteDirectory( "bin" );
            }
        }

        /// <summary>
        /// Resolves the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public string ResolvePath( string path )
        {
            return path;
        }

        /// <summary>
        /// Gets the target framework.
        /// </summary>
        /// <value>
        /// The target framework.
        /// </value>
        public FrameworkName TargetFramework
        {
            get { return VersionUtility.DefaultTargetFramework; }
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public dynamic GetPropertyValue( string propertyName )
        {
            if ( ( propertyName != null ) && propertyName.Equals( "RootNamespace", StringComparison.OrdinalIgnoreCase ) )
            {
                return string.Empty;
            }
            return null;
        }

        /// <summary>
        /// Workaround until we get to NuGet 2.7 - Always add the file to the filesystem.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stream"></param>
        public override void AddFile( string path, Stream stream )
        {
            base.AddFile( path, stream );

            if ( path.Equals( Path.Combine( "App_Data", "deletefile.lst" ) ) )
            {
                ProcessFilesToDelete( path );
            }
            else if ( path.EndsWith( _transformFilePrefix ) )
            {
                ProcessXmlDocumentTransformation( path );
            }
        }

        /// <summary>
        /// Workaround until we get to NuGet 2.7 - Always treat the file as though it does not exist so that it will be replaced.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override bool FileExists( string path )
        {
            return false;
        }

        /// <summary>
        /// Deletes each file listed in the App_Data/deletefile.lst and then deletes that file.
        /// </summary>
        private void ProcessFilesToDelete( string deleteListFile )
        {
            // Turn relative path to virtual path
            deleteListFile = System.Web.HttpContext.Current.Server.MapPath( Path.Combine( "~", deleteListFile ) );

            using ( StreamReader file = new StreamReader( deleteListFile ) )
            {
                string filenameLine;
                while ( ( filenameLine = file.ReadLine() ) != null )
                {
                    if ( !string.IsNullOrWhiteSpace( filenameLine ) )
                    {
                        if ( filenameLine.StartsWith( @"RockWeb\" ) )
                        {
                            filenameLine = filenameLine.Substring( 8 );
                        }

                        string physicalFile = System.Web.HttpContext.Current.Server.MapPath( Path.Combine( "~", filenameLine ) );
                        
                        if ( File.Exists( physicalFile ) )
                        {
                            // TODO guard against things like file is temporarily locked, wait then try delete, etc.
                            File.Delete( physicalFile );
                        }
                    }
                }
                file.Close();
            }
            File.Delete( deleteListFile );
        }

        /// <summary>
        /// Transforms the file for the corresponding XDT file.
        /// </summary>
        /// <param name="transformListFile">A .rock.xdt transform file.</param>
        /// <returns>true if the transformation was successful; false otherwise.</returns>
        private bool ProcessXmlDocumentTransformation( string transformFile )
        {
            bool isSuccess = true;

            string sourceFile = transformFile.Remove( transformFile.Length - _transformFilePrefix.Length );

            transformFile = System.Web.HttpContext.Current.Server.MapPath( Path.Combine( "~", transformFile ) );
            sourceFile = System.Web.HttpContext.Current.Server.MapPath( Path.Combine( "~", sourceFile ) );

            if ( !File.Exists( sourceFile ) )
            {
                ExceptionLogService.LogException( new FileNotFoundException( string.Format( "Source transform file ({0}) does not exist.", sourceFile ) ), System.Web.HttpContext.Current );
                return false;
            }

            // This really shouldn't happen since it was theoretically‎ just added before
            // we were called.
            if ( !File.Exists( transformFile ) )
            {
                ExceptionLogService.LogException( new FileNotFoundException( string.Format( "Transform file ({0}) does not exist.", transformFile ) ), System.Web.HttpContext.Current );
                return false;
            }

            string destFile = sourceFile;

            using ( XmlTransformableDocument document = new XmlTransformableDocument() )
            {
                document.PreserveWhitespace = true;
                document.Load( sourceFile );

                using ( XmlTransformation transform = new XmlTransformation( transformFile ) )
                {
                    isSuccess = transform.Apply( document );
                    document.Save( destFile );
                }
            }

            if ( isSuccess )
            {
                File.Delete( transformFile );
            }

            return isSuccess;
        }

    }
}
