﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rock.Plugin;

namespace church.ccv.PersonalizationEngine.Migrations
{
    [MigrationNumber( 1, "1.0.0" )]
    public class CreateDb : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            Sql( @"
                    CREATE TABLE [dbo].[_church_ccv_PersonalizationEngine_Persona] (
                        [Id] [int] NOT NULL IDENTITY,
                        [Name] [nvarchar](MAX) NOT NULL,
                        [Description] [nvarchar](MAX) NOT NULL,
                        [RockSQL] [nvarchar](MAX) NOT NULL,

                        [CreatedDateTime] [datetime],
                        [ModifiedDateTime] [datetime],
                        [CreatedByPersonAliasId] [int],
                        [ModifiedByPersonAliasId] [int],
                        [Guid] [uniqueidentifier] NOT NULL,
                        [ForeignId] [int] NULL,
                        [ForeignGuid] [uniqueidentifier] NULL,
                        [ForeignKey] [nvarchar](100) NULL   
                    )

                    CREATE TABLE [dbo].[_church_ccv_PersonalizationEngine_CampaignType] (
                        [Id] [int] NOT NULL IDENTITY,
                        [Name] [nvarchar](MAX) NOT NULL,
                        [Description] [nvarchar](MAX) NOT NULL,
                        [DebugUrl] [nvarchar](MAX),
                        [JsonTemplate] [nvarchar](MAX) NOT NULL,

                        [CreatedDateTime] [datetime],
                        [ModifiedDateTime] [datetime],
                        [CreatedByPersonAliasId] [int],
                        [ModifiedByPersonAliasId] [int],
                        [Guid] [uniqueidentifier] NOT NULL,
                        [ForeignId] [int] NULL,
                        [ForeignGuid] [uniqueidentifier] NULL,
                        [ForeignKey] [nvarchar](100) NULL   
                    )

                    CREATE TABLE [dbo].[_church_ccv_PersonalizationEngine_Campaign] (
                        [Id] [int] NOT NULL IDENTITY,
                        [Name] [nvarchar](MAX) NOT NULL,
                        [Description] [nvarchar](MAX) NOT NULL,
                        [StartDate] [datetime] NOT NULL,
                        [EndDate] [datetime],
                        [Type] [nvarchar](MAX) NOT NULL,
                        [Priority] [int] NOT NULL,
                        [ContentJson] [nvarchar](MAX),

                        [CreatedDateTime] [datetime],
                        [ModifiedDateTime] [datetime],
                        [CreatedByPersonAliasId] [int],
                        [ModifiedByPersonAliasId] [int],
                        [Guid] [uniqueidentifier] NOT NULL,
                        [ForeignId] [int] NULL,
                        [ForeignGuid] [uniqueidentifier] NULL,
                        [ForeignKey] [nvarchar](100) NULL   
                    )

                    CREATE TABLE [dbo].[_church_ccv_PersonalizationEngine_Linkage] (
                        [Id] [int] NOT NULL IDENTITY,
                        [PersonaId] [int] NOT NULL,
                        [CampaignId] [int] NOT NULL,

                        [CreatedDateTime] [datetime],
                        [ModifiedDateTime] [datetime],
                        [CreatedByPersonAliasId] [int],
                        [ModifiedByPersonAliasId] [int],
                        [Guid] [uniqueidentifier] NOT NULL,
                        [ForeignId] [int] NULL,
                        [ForeignGuid] [uniqueidentifier] NULL,
                        [ForeignKey] [nvarchar](100) NULL   
                    )" );
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            Sql( @"
                   DROP TABLE [dbo].[_church_ccv_PersonalizationEngine_Persona]
                   DROP TABLE [dbo].[_church_ccv_PersonalizationEngine_CampaignType]
                   DROP TABLE [dbo].[_church_ccv_PersonalizationEngine_Campaign]
                   DROP TABLE [dbo].[_church_ccv_PersonalizationEngine_Linkage]" );
        }
    }
}
