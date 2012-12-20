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
    /// AttributeQualifier Service class
    /// </summary>
    public partial class AttributeQualifierService : Service<AttributeQualifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeQualifierService"/> class
        /// </summary>
        public AttributeQualifierService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeQualifierService"/> class
        /// </summary>
        public AttributeQualifierService(IRepository<AttributeQualifier> repository) : base(repository)
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
        public bool CanDelete( AttributeQualifier item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static class AttributeQualifierExtensionMethods
    {
        /// <summary>
        /// Perform a shallow copy of this AttributeQualifier to another
        /// </summary>
        public static void ShallowCopy( this AttributeQualifier source, AttributeQualifier target )
        {
            target.IsSystem = source.IsSystem;
            target.AttributeId = source.AttributeId;
            target.Key = source.Key;
            target.Value = source.Value;
            target.Id = source.Id;
            target.Guid = source.Guid;

        }
    }
}
