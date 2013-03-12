//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rock.Attribute
{
    /// <summary>
    /// Attribute used to specify an EntityType
    /// Value returns EntityType.Name
    /// </summary>
    public class EntityTypeAttribute : FieldAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTypeAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public EntityTypeAttribute(string name, string description = "", bool required = true, string category = "", int order = 0, string key = null)
            : base( name, description, required, "", category, order, key, typeof( Rock.Field.Types.EntityTypeFieldType ).FullName )
        {
        }
    }
}