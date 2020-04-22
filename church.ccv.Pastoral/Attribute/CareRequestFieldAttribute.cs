using System;

using Rock.Attribute;

namespace church.ccv.Pastoral.Attribute
{
    /// <summary>
    /// Field Attribute to select a Care Request
    /// </summary>
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = true, Inherited = true )]
    public class CareRequestFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CareRequestFieldAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="required">if set to <c>true</c> [required].</param>
        /// <param name="defaultRequestId">The default request identifier.</param>
        /// <param name="category">The category.</param>
        /// <param name="order">The order.</param>
        /// <param name="key">The key.</param>
        public CareRequestFieldAttribute( string name = "Request", string description = "", bool required = true, string defaultRequestId = "", string category = "", int order = 0, string key = null )
            : base( name, description, required, defaultRequestId, category, order, key, typeof( Field.Types.CareRequestFieldType ).FullName, "ccv.church.Pastoral" )
        {
        }
    }
}