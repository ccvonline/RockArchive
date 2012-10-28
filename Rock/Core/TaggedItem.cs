//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Core
{
    /// <summary>
    /// Tagged Ited POCO Entity.
    /// </summary>
    [Table( "coreTaggedItem" )]
    public partial class TaggedItem : Model<TaggedItem>
    {
        /// <summary>
        /// Gets or sets the System.
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        [Required]
        [DataMember]
        public bool IsSystem { get; set; }
        
        /// <summary>
        /// Gets or sets the Attribute Id.
        /// </summary>
        /// <value>
        /// Attribute Id.
        /// </value>
        [Required]
        [DataMember]
        public int TagId { get; set; }
        
        /// <summary>
        /// Gets or sets the Entity Id.
        /// </summary>
        /// <value>
        /// Entity Id.
        /// </value>
        [DataMember]
        public int? EntityId { get; set; }
        
        /// <summary>
        /// Gets the auth entity.
        /// </summary>
        [NotMapped]
        public string DeprecatedEntityTypeName { get { return "Core.TaggedItem"; } }
        
        /// <summary>
        /// Gets or sets the Tag
        /// </summary>
        /// <value>
        /// A <see cref="Tag"/> object.
        /// </value>
        public virtual Tag Tag { get; set; }
        
        /// <summary>
        /// Static Method to return an object based on the id
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static AttributeValue Read( int id )
        {
            return Read<AttributeValue>( id );
        }

        /// <summary>
        /// Gets the parent authority.
        /// </summary>
        public override Security.ISecured ParentAuthority
        {
            get { return new Security.GenericEntity( "Global" ); }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.EntityId.HasValue ? this.EntityId.ToString() : "";
        }
    }

    /// <summary>
    /// Attribute Value Configuration class.
    /// </summary>
    public partial class TaggeedItemConfiguration : EntityTypeConfiguration<TaggedItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeValueConfiguration"/> class.
        /// </summary>
        public TaggeedItemConfiguration()
        {
            this.HasRequired( p => p.Tag ).WithMany( p => p.TaggedItems ).HasForeignKey( p => p.TagId ).WillCascadeOnDelete(true);
        }
    }
}
