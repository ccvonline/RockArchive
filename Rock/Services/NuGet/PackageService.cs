﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DotLiquid.Exceptions;
using NuGet;
using Rock.Data;
using Rock.Model;

namespace Rock.Services.NuGet
{
    /// <summary>
    /// Facade class to provide intaraction with NuGet internals
    /// </summary>
    public class PackageService
    {
        /// <summary>
        /// Exports the page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="isRecursive">if set to <c>true</c> [should export children].</param>
        /// <returns>a <see cref="T:System.IO.MemoryStream"/> of the exported page package.</returns>
        public MemoryStream ExportPage( Page page, bool isRecursive )
        {
            // Create a temp directory to hold package contents in staging area
            string packageId = Guid.NewGuid().ToString();
            var packageDirectory = CreatePackageDirectory( page.Name, packageId );
            var webRootPath = HttpContext.Current.Server.MapPath( "~" );

            // Create a manifest for this page export...
            Manifest manifest = new Manifest();
            manifest.Metadata = new ManifestMetadata();
            manifest.Metadata.Authors = "PageExport";
            manifest.Metadata.Version = "0.0";
            manifest.Metadata.Id = packageId;
            manifest.Metadata.Description = ( string.IsNullOrEmpty( page.Description ) ) ? "a page export" : page.Description;
            manifest.Files = new List<ManifestFile>();

            // Generate the JSON from model data and write it to disk, then add it to the manifest
            var json = GetJson( page, isRecursive );
            using ( var file = new StreamWriter( Path.Combine( packageDirectory.FullName, "export.json" ) ) )
            {
                file.Write( json );
            }
            AddToManifest( manifest, packageDirectory.FullName, webRootPath, "export.json", SearchOption.TopDirectoryOnly );

            // * Find out from `page` which Blocks need to be packaged up (recursively if ExportChildren box is checked)
            // * Grab asset folders for each Block's code file uniquely (Scripts, Assets, CSS folders)
            // * Add them to the package manifest
            var blockTypes = new Dictionary<Guid, BlockType>();
            var uniqueDirectories = new Dictionary<string, string>();
            FindUniqueBlockTypesAndDirectories( page, isRecursive, blockTypes, uniqueDirectories );

            foreach ( var blockType in blockTypes.Values )
            {
                var sourcePath = HttpContext.Current.Server.MapPath( blockType.Path );
                var directory = sourcePath.Substring( 0, sourcePath.LastIndexOf( Path.DirectorySeparatorChar ) );
                var fileName = Path.GetFileNameWithoutExtension( sourcePath );
                var pattern = string.Format( "{0}.*", fileName );
                AddToManifest( manifest, directory, webRootPath, pattern, SearchOption.TopDirectoryOnly );
            }

            // Second pass through blocks. Determine which folders (by convention) need to be added
            // to the package manifest.
            foreach ( var dir in uniqueDirectories.Keys )
            {
                var sourcePath = HttpContext.Current.Server.MapPath( dir );
                // Are there any folders present named "CSS", "Scripts" or "Assets"?
                AddToManifest( manifest, Path.Combine( sourcePath, "CSS" ), webRootPath );
                AddToManifest( manifest, Path.Combine( sourcePath, "Scripts" ), webRootPath );
                AddToManifest( manifest, Path.Combine( sourcePath, "Assets" ), webRootPath );
            }

            // Save the manifest as "pageexport.nuspec" in the temp folder
            string basePath = packageDirectory.FullName;
            string manifestPath = Path.Combine( basePath, "pageexport.nuspec" );
            using ( Stream fileStream = File.Create( manifestPath ) )
            {
                manifest.Save( fileStream );
            }

            // Create a NuGet package from the manifest and 'save' it to a memory stream for the consumer...
            // BTW - don't use anything older than nuget 2.1 due to Manifest bug (http://nuget.codeplex.com/workitem/2813)
            // which will cause the PackageBuilder constructor to fail due to <Files> vs <files> being in the manifest.
            PackageBuilder packageBuilder = new PackageBuilder( manifestPath, basePath, NullPropertyProvider.Instance, false );

            var outputStream = new MemoryStream();
            packageBuilder.Save( outputStream );

            // Clean up staging area
            packageDirectory.Delete( recursive: true );
            return outputStream;
        }

        /// <summary>
        /// Imports the page.
        /// </summary>
        /// <param name="uploadedPackage">Byte array of the uploaded package</param>
        /// <param name="fileName">File name of uploaded package</param>
        public void ImportPage( byte[] uploadedPackage, string fileName )
        {
            // Write .nupkg file to the PackageStaging folder...
            var path = Path.Combine( HttpContext.Current.Server.MapPath( "~/App_Data/PackageStaging" ), fileName );
            using ( var file = new FileStream( path, FileMode.Create ) )
            {
                file.Write( uploadedPackage, 0, uploadedPackage.Length );
            }

            var package = new ZipPackage( path );
            var packageFiles = package.GetFiles().ToList();
            var exportFile = packageFiles.FirstOrDefault( f => f.Path.Contains( "export.json" ) );
            
            Page page = null;

            if ( exportFile != null )
            {
                string json;

                using ( var stream = exportFile.GetStream() )
                {
                    json = stream.ReadToEnd();
                }

                page = Page.FromJson( json );
            }

            if ( page != null )
            {
                var installedBlockTypes = new BlockTypeService().Queryable();
                var newBlockTypes = FindNewBlockTypes( page, installedBlockTypes );
                ValidateImportData( page, newBlockTypes );
                ExpandFiles( package, packageFiles, path );   
            }

            // Validate package...
            // * Does it have any executable .dll files? Should those go to the bin folder, or into a plugins directory to be loaded via MEF?
            // * Does it have code or asset files that need to go on the file system?
            // * Does it have an export.json file? Should that be a requirement?

            // If export.json is present, deserialize data
            // * Are there any new BlockTypes to register? If so, save them first.
            // * Scrub out any `Id` and `Guid` fields that came over from export
            // * Save page data via PageService

            // Clean up PackageStaging folder.
            // Once data is saved, do we want to save the .nuspec file for later?
        }

        /// <summary>
        /// Creates a unique directory for temporarily holding the page export package.
        /// </summary>
        /// <param name="pageName">Name of the page.</param>
        /// <param name="packageId">The unique package id.</param>
        /// <returns>a <see cref="T:System.IO.DirectoryInfo"/> of the new directory.</returns>
        private DirectoryInfo CreatePackageDirectory( string pageName, string packageId )
        {
            var name = Regex.Replace( pageName, @"[^A-Za-z0-9]", string.Empty );
            var rootPath = HttpContext.Current.Server.MapPath( "~/App_Data" );
            var packageName = string.Format( "{0}_{1}", name, packageId );
            return Directory.CreateDirectory( Path.Combine( rootPath, "PackageStaging", packageName ) );
        }

        /// <summary>
        /// Gets the json for this page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="isRecursive">if set to <c>true</c> [is recursive].</param>
        /// <returns></returns>
        private string GetJson( Page page, bool isRecursive )
        {
            string json;

            if ( isRecursive )
            {
                json = page.ToJson();
            }
            else
            {
                // make a deep copy, remove it's child pages, and ToJson that...
                var pageCopy = page.Clone() as Page ?? new Page();
                pageCopy.Pages = new List<Page>();
                json = pageCopy.ToJson();
            }

            return json;
        }

        /// <summary>
        /// Finds the unique block types and directories for the given page and adds them to the <paramref name="blockTypes"/> and <paramref name="directories"/> dictionaries.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="isRecursive">if set to <c>true</c> [child pages are recursively searched too].</param>
        /// <param name="blockTypes">a Dictionary of BlockTypes.</param>
        /// <param name="directories">a Dictionary of directory names.</param>
        private void FindUniqueBlockTypesAndDirectories( Page page, bool isRecursive, Dictionary<Guid, BlockType> blockTypes, Dictionary<string, string> directories )
        {
            foreach ( var block in page.Blocks )
            {
                var blockType = block.BlockType;

                if ( !blockTypes.ContainsKey( blockType.Guid ) )
                {
                    blockTypes.Add( blockType.Guid, blockType );

                    var path = blockType.Path;
                    var directory = path.Substring( 0, path.LastIndexOf( '/' ) );

                    if ( !directories.ContainsKey( directory ) )
                    {
                        directories.Add( directory, directory );
                    }
                }
            }

            if ( isRecursive )
            {
                foreach ( var child in page.Pages )
                {
                    FindUniqueBlockTypesAndDirectories( child, true, blockTypes, directories );
                }
            }
        }

        /// <summary>
        /// Add the given directories files (matching the given file filter and search options)
        /// to the manifest.
        /// </summary>
        /// <param name="manifest">A NuGet Manifest</param>
        /// <param name="directory">the directory containing the file(s)</param>
        /// <param name="webRootPath">the physical path to the app's webroot</param>
        /// <param name="filterPattern"> A file filter pattern such as *.* or *.cs</param>
        /// <param name="searchOption">A <see cref="T:System.IO.SearchOption"/> search option to define the scope of the search</param>
        private void AddToManifest( Manifest manifest, string directory, string webRootPath,
            string filterPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories )
        {
            if ( !Directory.Exists( directory ) )
            {
                return;
            }

            // In our trivial case, the files we're adding need to have a target folder under the "content\"
            // folder and the source path suffix will be the 
            var files = from file in Directory.EnumerateFiles( directory, filterPattern, searchOption )
                        let pathSuffix = file.Substring( webRootPath.Length + 1 )
                        select new ManifestFile()
                        {
                            Source = Path.Combine( "..", "..", "..", pathSuffix ),
                            Target = Path.Combine( "content", pathSuffix )
                        };

            manifest.Files.AddRange( files );
        }

        private IEnumerable<BlockType> FindNewBlockTypes( Page page, IEnumerable<BlockType> installedBlockTypes )
        {
            var newBlockTypes = new List<BlockType>();
            


            return newBlockTypes;
        }

        /// <summary>
        /// Validates the import data.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="newBlockTypes">Collection of newly created BlockTypes</param>
        private void ValidateImportData( Page page, IEnumerable<BlockType> newBlockTypes )
        {
            ScrubIds( page );
            page.PageContexts.ToList().ForEach( ScrubIds );
            page.PageRoutes.ToList().ForEach( ScrubIds );
            var blockTypes = newBlockTypes.ToList();

            foreach ( var block in page.Blocks )
            {
                var blockType = blockTypes.FirstOrDefault( bt => block.BlockType.Path == bt.Path );

                if ( blockType == null )
                {
                    throw new System.ArgumentException(string.Format( "BlockType was not found by its path: {0}", block.BlockType.Path ));
                }

                ScrubIds( block );
                block.PageId = 0;
                block.BlockTypeId = blockType.Id;

                // TODO: Remove any BlockTypes that are already installed...
            }

            foreach ( var childPage in page.Pages )
            {
                if ( childPage.Pages.Count > 0 )
                {
                    ValidateImportData( childPage, blockTypes );
                }
            }
        }

        private static void ScrubIds( IEntity entity )
        {
            entity.Id = 0;
            entity.Guid = Guid.NewGuid();
        }

        /// <summary>
        /// Expands the files.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="packageFiles">The package files.</param>
        /// <param name="path">The path.</param>
        private void ExpandFiles( IPackage package, IEnumerable<IPackageFile> packageFiles, string path )
        {
            // Remove any currently installed BlockTypes from the list of files to be unzipped
            var filesToUnzip = packageFiles.Where( f => !f.Path.Contains( "export.json" ) ).ToList();
            var blockTypeService = new BlockTypeService();
            var installedBlockTypes = blockTypeService.Queryable();

            foreach ( var blockType in installedBlockTypes )
            {
                var blockFileName = blockType.Path.Substring( blockType.Path.LastIndexOf( "/", StringComparison.InvariantCultureIgnoreCase ) );
                blockFileName = blockFileName.Replace( '/', Path.PathSeparator );  // Convert relative URL separator to OS file system path separator
                filesToUnzip.RemoveAll( f => f.Path.Contains( blockFileName ) );
            }

            // Unzip files into the correct locations
            var fileSystem = new PhysicalFileSystem( HttpContext.Current.Server.MapPath( "~" ) );
            var pathResolver = new DefaultPackagePathResolver( path );
            var packageDirectory = pathResolver.GetPackageDirectory( package );
            fileSystem.AddFiles( filesToUnzip, packageDirectory );
        }
    }
}