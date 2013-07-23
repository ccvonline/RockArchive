//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Attribute Value POCO Entity.
    /// </summary>
    [Table( "AttributeValue" )]
    [DataContract]
    public partial class AttributeValue : Model<AttributeValue>
    {
        /// <summary>
        /// Gets or sets the System.
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public bool IsSystem { get; set; }
        
        /// <summary>
        /// Gets or sets the Attribute Id.
        /// </summary>
        /// <value>
        /// Attribute Id.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int AttributeId { get; set; }
        
        /// <summary>
        /// Gets or sets the Entity Id.
        /// </summary>
        /// <value>
        /// Entity Id.
        /// </value>
        [DataMember]
        public int? EntityId { get; set; }
        
        /// <summary>
        /// Gets or sets the Order.
        /// </summary>
        /// <value>
        /// Order.
        /// </value>
        [DataMember]
        public int? Order { get; set; }
        
        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        /// <value>
        /// Value.
        /// </value>
        [DataMember]
        public string Value 
        {
            get
            {
                Rock.Field.IFieldType fieldType = Rock.Web.Cache.AttributeCache.Read( this.AttributeId ).FieldType.Field;
                if ( fieldType is Rock.Field.Types.EncryptedFieldType )
                {
                    return Rock.Security.Encryption.DecryptString( _value );
                }
                else
                {
                    return _value;
                }
            }

            set
            {
                Rock.Field.IFieldType fieldType = Rock.Web.Cache.AttributeCache.Read( this.AttributeId ).FieldType.Field;
                if ( fieldType is Rock.Field.Types.EncryptedFieldType )
                {
                    _value = Rock.Security.Encryption.EncryptString( value );
                }
                else
                {
                    _value = value;
                }

            }
        }
        private string _value;
        
        /// <summary>
        /// Gets or sets the Attribute.
        /// </summary>
        /// <value>
        /// A <see cref="Attribute"/> object.
        /// </value>
        [DataMember]
        public virtual Attribute Attribute { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            Rock.Field.IFieldType fieldType = Rock.Web.Cache.AttributeCache.Read( this.AttributeId ).FieldType.Field;
            if ( fieldType is Rock.Field.Types.EncryptedFieldType )
            {
                return "**********";
            }
            else
            {
                return this.Value;
            }
        }
    }

    /// <summary>
    /// Attribute Value Configuration class.
    /// </summary>
    public partial class AttributeValueConfiguration : EntityTypeConfiguration<AttributeValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeValueConfiguration"/> class.
        /// </summary>
        public AttributeValueConfiguration()
        {
            this.HasRequired( p => p.Attribute ).WithMany().HasForeignKey( p => p.AttributeId ).WillCascadeOnDelete( true );
        }
    }
}
