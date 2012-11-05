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
using Rock.Data;

namespace Rock.Core
{
    /// <summary>
    /// Entity Change POCO Entity.
    /// </summary>
    [NotAudited]
    [Table( "coreEntityChange" )]
    public partial class EntityChange : Model<EntityChange>
    {
        /// <summary>
        /// Gets or sets the Change Set.
        /// </summary>
        /// <value>
        /// Change Set.
        /// </value>
        [Required]
        public Guid ChangeSet { get; set; }
        
        /// <summary>
        /// Gets or sets the Change Type.
        /// </summary>
        /// <value>
        /// Change Type.
        /// </value>
        [Required]
        [MaxLength( 10 )]
        public string ChangeType { get; set; }

        /// <summary>
        /// Gets or sets the Entity Type Id.
        /// </summary>
        /// <value>
        /// Entity Type Id.
        /// </value>
        [Required]
        public int EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Entity Id.
        /// </summary>
        /// <value>
        /// Entity Id.
        /// </value>
        [Required]
        public int EntityId { get; set; }
        
        /// <summary>
        /// Gets or sets the Property.
        /// </summary>
        /// <value>
        /// Property.
        /// </value>
        [Required]
        [MaxLength( 100 )]
        public string Property { get; set; }
        
        /// <summary>
        /// Gets or sets the Original Value.
        /// </summary>
        /// <value>
        /// Original Value.
        /// </value>
        public string OriginalValue { get; set; }
        
        /// <summary>
        /// Gets or sets the Current Value.
        /// </summary>
        /// <value>
        /// Current Value.
        /// </value>
        public string CurrentValue { get; set; }
        
        /// <summary>
        /// Gets or sets the Created Date Time.
        /// </summary>
        /// <value>
        /// Created Date Time.
        /// </value>
        public DateTime? CreatedDateTime { get; set; }
        
        /// <summary>
        /// Gets or sets the Created By Person Id.
        /// </summary>
        /// <value>
        /// Created By Person Id.
        /// </value>
        public int? CreatedByPersonId { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public virtual Core.EntityType EntityType { get; set; }

        /// <summary>
        /// Gets or sets the created by person.
        /// </summary>
        /// <value>
        /// The created by person.
        /// </value>
        public virtual Rock.Crm.Person CreatedByPerson { get; set; }

        /// <summary>
        /// Static Method to return an object based on the id
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static EntityChange Read( int id )
        {
            return Read<EntityChange>( id );
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.CurrentValue;
        }
    }
    
    /// <summary>
    /// Entity Change Configuration class.
    /// </summary>
    public partial class EntityChangeConfiguration : EntityTypeConfiguration<EntityChange>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityChangeConfiguration"/> class.
        /// </summary>
        public EntityChangeConfiguration()
        {
            this.HasOptional( p => p.CreatedByPerson ).WithMany().HasForeignKey( p => p.CreatedByPersonId ).WillCascadeOnDelete( true );
            this.HasRequired( p => p.EntityType ).WithMany().HasForeignKey( p => p.EntityTypeId ).WillCascadeOnDelete( false );
        }
    }
}
