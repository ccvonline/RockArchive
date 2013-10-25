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
    /// TaggedItem Service class
    /// </summary>
    public partial class TaggedItemService : Service<TaggedItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaggedItemService"/> class
        /// </summary>
        public TaggedItemService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaggedItemService"/> class
        /// </summary>
        /// <param name="repository">The repository.</param>
        public TaggedItemService(IRepository<TaggedItem> repository) : base(repository)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaggedItemService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public TaggedItemService(RockContext context) : base(context)
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
        public bool CanDelete( TaggedItem item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class TaggedItemExtensionMethods
    {
        /// <summary>
        /// Clones this TaggedItem object to a new TaggedItem object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static TaggedItem Clone( this TaggedItem source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as TaggedItem;
            }
            else
            {
                var target = new TaggedItem();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another TaggedItem object to this TaggedItem object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this TaggedItem target, TaggedItem source )
        {
            target.IsSystem = source.IsSystem;
            target.TagId = source.TagId;
            target.EntityGuid = source.EntityGuid;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
