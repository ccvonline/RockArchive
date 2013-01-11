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
    /// BlockType Service class
    /// </summary>
    public partial class BlockTypeService : Service<BlockType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockTypeService"/> class
        /// </summary>
        public BlockTypeService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockTypeService"/> class
        /// </summary>
        public BlockTypeService(IRepository<BlockType> repository) : base(repository)
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
        public bool CanDelete( BlockType item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static class BlockTypeExtensionMethods
    {
        /// <summary>
        /// Clones this BlockType object to a new BlockType object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static BlockType Clone( this BlockType source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as BlockType;
            }
            else
            {
                var target = new BlockType();
                target.IsSystem = source.IsSystem;
                target.Path = source.Path;
                target.Name = source.Name;
                target.Description = source.Description;
                target.Id = source.Id;
                target.Guid = source.Guid;

            
                return target;
            }
        }
    }
}
