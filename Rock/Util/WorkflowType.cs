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

using Rock.Cms;
using Rock.Core;
using Rock.Data;

namespace Rock.Util
{
    /// <summary>
    /// WorkflowType POCO Entity.
    /// </summary>
    [Table( "utilWorkflowType" )]
    public partial class WorkflowType : Model<WorkflowType>, IOrdered
    {
        /// <summary>
        /// Gets or sets the System.
        /// </summary>
        /// <value>
        /// Determines whether the job is a system job..
        /// </value>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        /// <value>
        /// The is active.
        /// </value>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// Friendly name for the job..
        /// </value>
        [Required]
        [MaxLength( 100 )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// Notes about the job..
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets or sets the Order.
        /// </summary>
        /// <value>
        /// Order.
        /// </value>
        [Required]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the file id.
        /// </summary>
        /// <value>
        /// The file id.
        /// </value>
        public int? FileId { get; set; }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public virtual File File { get; set; }

        /// <summary>
        /// Gets or sets the work term.
        /// </summary>
        /// <value>
        /// The work term.
        /// </value>
        [Required]
        [MaxLength( 100 )]
        public string WorkTerm { get; set; }

        /// <summary>
        /// Gets or sets the entry activity type id.
        /// </summary>
        /// <value>
        /// The entry activity type id.
        /// </value>
        public int? EntryActivityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the entry activity.
        /// </summary>
        /// <value>
        /// The type of the entry activity.
        /// </value>
        public virtual ActivityType EntryActivityType { get; set; }

        /// <summary>
        /// Gets or sets the processing interval seconds.
        /// </summary>
        /// <value>
        /// The processing interval seconds.
        /// </value>
        public int? ProcessingIntervalSeconds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is persisted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is persisted; otherwise, <c>false</c>.
        /// </value>
        public bool IsPersisted { get; set; }

        /// <summary>
        /// Gets or sets the activity types.
        /// </summary>
        /// <value>
        /// The activity types.
        /// </value>
        public virtual ICollection<ActivityType> ActivityTypes { get; set; }

        /// <summary>
        /// Static Method to return an object based on the id
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static WorkflowType Read( int id )
        {
            return Read<WorkflowType>( id );
        }

        /// <summary>
        /// Reads the specified GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        public static WorkflowType Read( Guid guid )
        {
            return Read<WorkflowType>( guid );
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

    }

    /// <summary>
    /// WorkflowType Configuration class.
    /// </summary>
    public partial class WorkflowTypeConfiguration : EntityTypeConfiguration<WorkflowType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowTypeConfiguration"/> class.
        /// </summary>
        public WorkflowTypeConfiguration()
        {
            this.HasOptional( m => m.EntryActivityType ).WithMany().HasForeignKey( m => m.EntryActivityTypeId ).WillCascadeOnDelete( false );
            this.HasOptional( m => m.Category ).WithMany().HasForeignKey( m => m.CategoryId ).WillCascadeOnDelete( false );
            this.HasOptional( m => m.File ).WithMany().HasForeignKey( m => m.FileId ).WillCascadeOnDelete( false );
        }
    }
}

