using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;
using Rock.Data;
using Rock.Model;

namespace church.ccv.CCVCore.PushNotification.Model
{
    [Table( "_church_ccv_PushNotification_Device" )]
    [DataContract]
    public class PushNotificationDevice : Model<PushNotificationDevice>, IRockEntity
    {
        [DataMember]
        public int? PersonAliasId { get; set; }

        [DataMember]
        public string DeviceId { get; set; }

        [DataMember]
        public string Platform { get; set; }

        [DataMember]
        public DateTime? LastPushedDateTime { get; set; }

        [DataMember]
        public DateTime LastSeenDateTime { get; set; }

        public virtual PersonAlias PersonAlias { get; set; }
    }

    public class PushNotificationDeviceConfiguration : EntityTypeConfiguration<PushNotificationDevice>
    {
        public PushNotificationDeviceConfiguration()
        {
            this.HasEntitySetName( "PushNotificationDevice" );

            this.HasRequired( a => a.PersonAlias ).WithMany().HasForeignKey( a => a.PersonAliasId ).WillCascadeOnDelete( false );
        }
    }
}
