namespace Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
#pragma warning disable 1591
    public partial class Metric2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey( "dbo.coreMetric", "CreatedByPersonId", "crmPerson" );
            DropForeignKey( "dbo.coreMetric", "ModifiedByPersonId", "crmPerson" );
            DropForeignKey( "dbo.coreMetricValue", "MetricId", "coreMetric" );
            DropForeignKey( "dbo.coreMetricValue", "CreatedByPersonId", "crmPerson" );
            DropForeignKey( "dbo.coreMetricValue", "ModifiedByPersonId", "crmPerson" );
            DropIndex( "coreMetric", new[] { "CreatedByPersonId" } );
            DropIndex( "coreMetric", new[] { "ModifiedByPersonId" } );
            DropIndex( "coreMetricValue", new[] { "MetricId" } );
            DropIndex( "coreMetricValue", new[] { "CreatedByPersonId" } );
            DropIndex( "coreMetricValue", new[] { "ModifiedByPersonId" } );
            DropTable( "dbo.coreMetric" );
            DropTable( "dbo.coreMetricValue" );
            
            CreateTable(
                "dbo.coreMetric",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsSystem = c.Boolean(nullable: false),
                        Type = c.Boolean(nullable: false),
                        Category = c.String(maxLength: 100),
                        Title = c.String(nullable: false, maxLength: 100),
                        Subtitle = c.String(maxLength: 100),
                        Description = c.String(),
                        MinValue = c.Int(),
                        MaxValue = c.Int(),
                        CollectionFrequencyId = c.Int(nullable: false),
                        LastCollected = c.DateTime(),
                        Source = c.String(maxLength: 100),
                        SourceSQL = c.String(),
                        Order = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonId = c.Int(),
                        ModifiedByPersonId = c.Int(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.coreDefinedValue", t => t.CollectionFrequencyId, cascadeDelete: true)
                .ForeignKey("dbo.crmPerson", t => t.CreatedByPersonId)
                .ForeignKey("dbo.crmPerson", t => t.ModifiedByPersonId)
                .Index(t => t.CollectionFrequencyId)
                .Index(t => t.CreatedByPersonId)
                .Index(t => t.ModifiedByPersonId);
            
            CreateTable(
                "dbo.coreMetricValue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsSystem = c.Boolean(nullable: false),
                        MetricId = c.Int(nullable: false),
                        Value = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        xValue = c.String(nullable: false),
                        isDateBased = c.Boolean(nullable: false),
                        Label = c.String(),
                        Order = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonId = c.Int(),
                        ModifiedByPersonId = c.Int(),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.crmPerson", t => t.CreatedByPersonId)
                .ForeignKey("dbo.crmPerson", t => t.ModifiedByPersonId)
                .ForeignKey("dbo.coreMetric", t => t.MetricId, cascadeDelete: true)
                .Index(t => t.CreatedByPersonId)
                .Index(t => t.ModifiedByPersonId)
                .Index(t => t.MetricId);

            DataUp();
        }
        
        public override void Down()
        {
            DataDown();

            DropIndex("dbo.coreMetricValue", new[] { "MetricId" });
            DropIndex("dbo.coreMetricValue", new[] { "ModifiedByPersonId" });
            DropIndex("dbo.coreMetricValue", new[] { "CreatedByPersonId" });
            DropIndex("dbo.coreMetric", new[] { "ModifiedByPersonId" });
            DropIndex("dbo.coreMetric", new[] { "CreatedByPersonId" });
            DropIndex("dbo.coreMetric", new[] { "CollectionFrequencyId" });
            DropForeignKey("dbo.coreMetricValue", "MetricId", "dbo.coreMetric");
            DropForeignKey("dbo.coreMetricValue", "ModifiedByPersonId", "dbo.crmPerson");
            DropForeignKey("dbo.coreMetricValue", "CreatedByPersonId", "dbo.crmPerson");
            DropForeignKey("dbo.coreMetric", "ModifiedByPersonId", "dbo.crmPerson");
            DropForeignKey("dbo.coreMetric", "CreatedByPersonId", "dbo.crmPerson");
            DropForeignKey("dbo.coreMetric", "CollectionFrequencyId", "dbo.coreDefinedValue");
            DropTable("dbo.coreMetricValue");
            DropTable("dbo.coreMetric");

            CreateTable(
                "dbo.coreMetricValue",
                c => new
                {
                    Id = c.Int( nullable: false, identity: true ),
                    IsSystem = c.Boolean( nullable: false ),
                    MetricId = c.Int( nullable: false ),
                    Value = c.String( nullable: false, maxLength: 100 ),
                    Description = c.String(),
                    xValue = c.Int( nullable: false ),
                    isDateBased = c.Boolean( nullable: false ),
                    Label = c.String(),
                    Order = c.Int( nullable: false ),
                    CreatedDateTime = c.DateTime(),
                    ModifiedDateTime = c.DateTime(),
                    CreatedByPersonId = c.Int(),
                    ModifiedByPersonId = c.Int(),
                    Guid = c.Guid( nullable: false ),
                } )
                .PrimaryKey( t => t.Id );

            CreateTable(
                "dbo.coreMetric",
                c => new
                {
                    Id = c.Int( nullable: false, identity: true ),
                    IsSystem = c.Boolean( nullable: false ),
                    Type = c.Boolean( nullable: false ),
                    Category = c.String( maxLength: 100 ),
                    Title = c.String( nullable: false, maxLength: 100 ),
                    Subtitle = c.String( maxLength: 100 ),
                    Description = c.String(),
                    MinValue = c.Int( nullable: false ),
                    MaxValue = c.Int( nullable: false ),
                    CollectionFrequency = c.Int( nullable: false ),
                    LastCollected = c.DateTime( nullable: false ),
                    Source = c.String( maxLength: 100 ),
                    SourceSQL = c.String(),
                    Order = c.Int( nullable: false ),
                    CreatedDateTime = c.DateTime(),
                    ModifiedDateTime = c.DateTime(),
                    CreatedByPersonId = c.Int(),
                    ModifiedByPersonId = c.Int(),
                    Guid = c.Guid( nullable: false ),
                } )
                .PrimaryKey( t => t.Id );

            CreateIndex( "coreMetricValue", "ModifiedByPersonId" );
            CreateIndex( "coreMetricValue", "CreatedByPersonId" );
            CreateIndex( "coreMetricValue", "MetricId" );
            CreateIndex( "coreMetric", "ModifiedByPersonId" );
            CreateIndex( "coreMetric", "CreatedByPersonId" );
            AddForeignKey( "dbo.coreMetricValue", "ModifiedByPersonId", "crmPerson", "Id" );
            AddForeignKey( "dbo.coreMetricValue", "CreatedByPersonId", "crmPerson", "Id" );
            AddForeignKey( "dbo.coreMetricValue", "MetricId", "coreMetric", "Id", cascadeDelete: true );
            AddForeignKey( "dbo.coreMetric", "ModifiedByPersonId", "crmPerson", "Id" );
            AddForeignKey( "dbo.coreMetric", "CreatedByPersonId", "crmPerson", "Id" );
        }
    }
}
