using Rock.Plugin;

namespace church.ccv.CCVCore.Migrations
{
    [MigrationNumber( 4, "1.7.6" )]
    public class PushNotification_CreateDb : Migration
    {
        public override void Up()
        {
            Sql( @"
                CREATE TABLE [dbo]._church_ccv_PushNotification_Device (
		                [Id] [int] IDENTITY(1,1) NOT NULL,
                        [PersonAliasId] [int] NULL,
                        [DeviceId] [nvarchar](256) NOT NULL,
		                [Platform] [nvarchar] (100) NOT NULL,
                        [LastPushedDateTime] [datetime] NULL,
                        [LastSeenDateTime] [datetime] NOT NULL,
	                    [Guid] [uniqueidentifier] NOT NULL,
	                    [CreatedDateTime] [datetime] NOT NULL,
	                    [ModifiedDateTime] [datetime] NOT NULL,
	                    [CreatedByPersonAliasId] [int] NULL,
	                    [ModifiedByPersonAliasId] [int] NULL,
                        [ForeignKey] [nvarchar](100) NULL,
                        [ForeignGuid] [uniqueidentifier] NULL,
                        [ForeignId] [int] NULL  
                ) 
            " );
        }

        public override void Down()
        {
        }
    }
}
