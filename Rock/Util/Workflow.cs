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
using System.Linq;

using Rock.Data;

namespace Rock.Util
{
    /// <summary>
    /// Workflow POCO Entity.
    /// </summary>
    [Table( "utilWorkflow" )]
    public partial class Workflow : Model<Workflow>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the workflow type id.
        /// </summary>
        /// <value>
        /// The workflow type id.
        /// </value>
        public int WorkflowTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the workflow.
        /// </summary>
        /// <value>
        /// The type of the workflow.
        /// </value>
        public virtual WorkflowType WorkflowType { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [Required]
        [MaxLength( 100 )]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is processing.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is processing; otherwise, <c>false</c>.
        /// </value>
        public bool IsProcessing { get; set; }

        /// <summary>
        /// Gets or sets the activated date time.
        /// </summary>
        /// <value>
        /// The activated date time.
        /// </value>
        public DateTime? ActivatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the last processed date time.
        /// </summary>
        /// <value>
        /// The last processed date time.
        /// </value>
        public DateTime? LastProcessedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the completed date time.
        /// </summary>
        /// <value>
        /// The completed date time.
        /// </value>
        public DateTime? CompletedDateTime { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsActive
        {
            get
            {
                return ActivatedDateTime.HasValue && !CompletedDateTime.HasValue;
            }
        }

        /// <summary>
        /// Gets or sets the activities.
        /// </summary>
        /// <value>
        /// The activities.
        /// </value>
        public virtual ICollection<Activity> Activities { get; set; }

        /// <summary>
        /// Gets the active activities.
        /// </summary>
        /// <value>
        /// The active activities.
        /// </value>
        public virtual IEnumerable<Activity> ActiveActivities
        {
            get
            {
                return this.Activities.Where( a => a.IsActive );
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has active activities.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has active activities; otherwise, <c>false</c>.
        /// </value>
        public virtual bool HasActiveActivities
        {
            get
            {
                return this.Activities.Any( a => a.IsActive );
            }
        }

        /// <summary>
        /// Gets or sets the log entries.
        /// </summary>
        /// <value>
        /// The log entries.
        /// </value>
        public virtual ICollection<WorkflowLog> LogEntries { get; set; }

        /// <summary>
        /// Gets the parent authority.
        /// </summary>
        /// <value>
        /// The parent authority.
        /// </value>
        public override Security.ISecured ParentAuthority
        {
            get
            {
                return this.WorkflowType;
            }
        }

        #endregion

        #region Methods

        public virtual void Process()
        {
            DateTime processStartTime = DateTime.Now;
            while ( ProcessActivity( processStartTime ) ) { }
        }

        public virtual bool ProcessActivity( DateTime processStartTime )
        {
            foreach ( var activity in this.Activities )
            {
                if ( activity.NeedsProcessing( processStartTime ) )
                {
                    activity.Process();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format( "{0} : {1}", this.WorkflowType.ToString(), this.Id );
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Static Method to return an object based on the id
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static Workflow Read( int id )
        {
            return Read<Workflow>( id );
        }

        /// <summary>
        /// Reads the specified GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        public static Workflow Read( Guid guid )
        {
            return Read<Workflow>( guid );
        }

        #endregion
    }

    #region EF Configuration

    /// <summary>
    /// Workflow Configuration class.
    /// </summary>
    public partial class WorkflowConfiguration : EntityTypeConfiguration<Workflow>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowConfiguration"/> class.
        /// </summary>
        public WorkflowConfiguration()
        {
            this.HasRequired( m => m.WorkflowType ).WithMany().HasForeignKey( m => m.WorkflowTypeId ).WillCascadeOnDelete( true );
        }
    }

    #endregion
}

