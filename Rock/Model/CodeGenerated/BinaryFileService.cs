//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// BinaryFile Service class
    /// </summary>
    public partial class BinaryFileService : Service<BinaryFile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryFileService"/> class
        /// </summary>
        public BinaryFileService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryFileService"/> class
        /// </summary>
        public BinaryFileService(IRepository<BinaryFile> repository) : base(repository)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( BinaryFile item, out string errorMessage )
        {
            errorMessage = string.Empty;
            
            // ignoring BinaryFileType,IconSmallFileId 
            
            // ignoring BinaryFileType,IconLargeFileId 
            
            // ignoring Category,IconSmallFileId 
            
            // ignoring Category,IconLargeFileId 
 
            if ( new Service<FinancialTransactionImage>().Queryable().Any( a => a.BinaryFileId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", BinaryFile.FriendlyTypeName, FinancialTransactionImage.FriendlyTypeName );
                return false;
            }  
            
            // ignoring GroupType,IconSmallFileId 
            
            // ignoring GroupType,IconLargeFileId 
 
            if ( new Service<Page>().Queryable().Any( a => a.IconFileId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", BinaryFile.FriendlyTypeName, Page.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<Person>().Queryable().Any( a => a.PhotoId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", BinaryFile.FriendlyTypeName, Person.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static class BinaryFileExtensionMethods
    {
        /// <summary>
        /// Clones this BinaryFile object to a new BinaryFile object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static BinaryFile Clone( this BinaryFile source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as BinaryFile;
            }
            else
            {
                var target = new BinaryFile();
                target.IsTemporary = source.IsTemporary;
                target.IsSystem = source.IsSystem;
                target.BinaryFileTypeId = source.BinaryFileTypeId;
                target.Data = source.Data;
                target.Url = source.Url;
                target.FileName = source.FileName;
                target.MimeType = source.MimeType;
                target.LastModifiedDateTime = source.LastModifiedDateTime;
                target.Description = source.Description;
                target.Id = source.Id;
                target.Guid = source.Guid;

            
                return target;
            }
        }
    }
}
