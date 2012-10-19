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

namespace Rock.Util
{
    /// <summary>
    /// Job POCO Entity.
    /// </summary>
    [Table( "utilJob" )]
    public partial class Job : Model<Job>
    {
        /// <summary>
        /// Gets or sets the System.
        /// </summary>
        /// <value>
        /// Determines whether the job is a system job..
        /// </value>
        [DataMember]
        public bool IsSystem { get; set; }
        
        /// <summary>
        /// Gets or sets the Active.
        /// </summary>
        /// <value>
        /// Determines is the job is currently active..
        /// </value>
        [DataMember]
        public bool? IsActive { get; set; }
        
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// Friendly name for the job..
        /// </value>
        [Required]
        [MaxLength( 100 )]
        [DataMember]
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// Notes about the job..
        /// </value>
        [MaxLength( 500 )]
        [DataMember]
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the Assemby.
        /// </summary>
        /// <value>
        /// Assembly (.dll) that contains the job class..
        /// </value>
        [MaxLength( 100 )]
        [DataMember]
        public string Assemby { get; set; }
        
        /// <summary>
        /// Gets or sets the Class.
        /// </summary>
        /// <value>
        /// The class name of the job to run..
        /// </value>
        [Required]
        [MaxLength( 100 )]
        [DataMember]
        public string Class { get; set; }
        
        /// <summary>
        /// Gets or sets the Cron Expression.
        /// </summary>
        /// <value>
        /// The cron expression that is used to determine the schedule of the job (see http://www.quartz-scheduler.org/documentation/quartz-1.x/tutorials/crontrigger for syntax.).
        /// </value>
        [Required]
        [MaxLength( 120 )]
        [DataMember]
        public string CronExpression { get; set; }
        
        /// <summary>
        /// Gets or sets the Last Successful Run.
        /// </summary>
        /// <value>
        /// Date and time the job last completed successfully..
        /// </value>
        [DataMember]
        public DateTimeOffset? LastSuccessfulRun { get; set; }
        
        /// <summary>
        /// Gets or sets the Last Run Date.
        /// </summary>
        /// <value>
        /// Last date and time the job attempted to run..
        /// </value>
        [DataMember]
        public DateTimeOffset? LastRunDate { get; set; }
        
        /// <summary>
        /// Gets or sets the Last Run Duration.
        /// </summary>
        /// <value>
        /// Number of seconds that the last job took to finish..
        /// </value>
        [DataMember]
        public int? LastRunDuration { get; set; }
        
        /// <summary>
        /// Gets or sets the Last Status.
        /// </summary>
        /// <value>
        /// The completion status from the last time the job was run (valid values 'Success', 'Exception', 'Error Loading Job')..
        /// </value>
        [MaxLength( 50 )]
        [DataMember]
        public string LastStatus { get; set; }
        
        /// <summary>
        /// Gets or sets the Last Status Message.
        /// </summary>
        /// <value>
        /// Message from the last run.  Usually used to store the exception message..
        /// </value>
        [DataMember]
        public string LastStatusMessage { get; set; }
        
        /// <summary>
        /// Gets or sets the Last Run Scheduler Name.
        /// </summary>
        /// <value>
        /// Name of the scheduler that the job ran under.  This is used to determine if a job ran in IIS or the Windows service..
        /// </value>
        [MaxLength( 40 )]
        [DataMember]
        public string LastRunSchedulerName { get; set; }
        
        /// <summary>
        /// Gets or sets the Notification Emails.
        /// </summary>
        /// <value>
        /// Email addresses (separated with commas) to be used for notification..
        /// </value>
        [MaxLength( 1000 )]
        [DataMember]
        public string NotificationEmails { get; set; }
        
        /// <summary>
        /// Gets or sets the Notification Status.
        /// </summary>
        /// <value>
        /// Enum[JobNotificationStatus].
        /// </value>
        [Required]
        [DataMember]
        public JobNotificationStatus NotificationStatus { get; set; }

        /// <summary>
        /// Static Method to return an object based on the id
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static Job Read( int id )
        {
            return Read<Job>( id );
        }

        /// <summary>
        /// Gets the auth entity.
        /// </summary>
        [NotMapped]
        public override string EntityTypeName { get { return "Util.Job"; } }

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
    /// Job Configuration class.
    /// </summary>
    public partial class JobConfiguration : EntityTypeConfiguration<Job>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobConfiguration"/> class.
        /// </summary>
        public JobConfiguration()
        {
        }
    }

    /// <summary>
    /// Job notification status
    /// </summary>
    public enum JobNotificationStatus
    {
        /// <summary>
        /// Notify on all status
        /// </summary>
        All = 1,

        /// <summary>
        /// Notify when successful
        /// </summary>
        /// 
        Success = 2,

        /// <summary>
        /// Notify when an error occurs
        /// </summary>
        Error = 3,

        /// <summary>
        /// Notify when a warning occurs
        /// </summary>
        None = 4
    }
}
