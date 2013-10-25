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
    /// Block Service class
    /// </summary>
    public partial class BlockService : Service<Block>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockService"/> class
        /// </summary>
        public BlockService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockService"/> class
        /// </summary>
        /// <param name="repository">The repository.</param>
        public BlockService(IRepository<Block> repository) : base(repository)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public BlockService(RockContext context) : base(context)
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
        public bool CanDelete( Block item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class BlockExtensionMethods
    {
        /// <summary>
        /// Clones this Block object to a new Block object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static Block Clone( this Block source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as Block;
            }
            else
            {
                var target = new Block();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another Block object to this Block object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this Block target, Block source )
        {
            target.IsSystem = source.IsSystem;
            target.PageId = source.PageId;
            target.LayoutId = source.LayoutId;
            target.BlockTypeId = source.BlockTypeId;
            target.Zone = source.Zone;
            target.Order = source.Order;
            target.Name = source.Name;
            target.OutputCacheDuration = source.OutputCacheDuration;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
