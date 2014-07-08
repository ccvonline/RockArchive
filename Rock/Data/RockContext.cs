﻿// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Web;
using Rock.Model;
using Rock.Utility;
using Rock.Workflow;

namespace Rock.Data
{
    /// <summary>
    /// Entity Framework Context
    /// </summary>
    public class RockContext : Rock.Data.DbContext
    {

        //public RockContext()
        //{
        //    this.Database.Log = s => System.Diagnostics.Debug.WriteLine( s );
        //}

        #region Models

        /// <summary>
        /// Gets or sets the attendances.
        /// </summary>
        /// <value>
        /// The attendances.
        /// </value>
        public DbSet<Attendance> Attendances { get; set; }

        /// <summary>
        /// Gets or sets the attendance codes.
        /// </summary>
        /// <value>
        /// The attendance codes.
        /// </value>
        public DbSet<AttendanceCode> AttendanceCodes { get; set; }

        /// <summary>
        /// Gets or sets the Attributes.
        /// </summary>
        /// <value>
        /// the Attributes.
        /// </value>
        public DbSet<Rock.Model.Attribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the Attribute Qualifiers.
        /// </summary>
        /// <value>
        /// the Attribute Qualifiers.
        /// </value>
        public DbSet<AttributeQualifier> AttributeQualifiers { get; set; }

        /// <summary>
        /// Gets or sets the Attribute Values.
        /// </summary>
        /// <value>
        /// the Attribute Values.
        /// </value>
        public DbSet<AttributeValue> AttributeValues { get; set; }

        /// <summary>
        /// Gets or sets the Attribute Values.
        /// </summary>
        /// <value>
        /// the Attribute Values.
        /// </value>
        public DbSet<Audit> Audits { get; set; }

        /// <summary>
        /// Gets or sets the Auths.
        /// </summary>
        /// <value>
        /// the Auths.
        /// </value>
        public DbSet<Auth> Auths { get; set; }

        /// <summary>
        /// Gets or sets the Files.
        /// </summary>
        /// <value>
        /// the Files.
        /// </value>
        public DbSet<Model.BinaryFile> BinaryFiles { get; set; }

        /// <summary>
        /// Gets or sets the Files data.
        /// </summary>
        /// <value>
        /// the Files data
        /// </value>
        public DbSet<BinaryFileData> BinaryFilesData { get; set; }

        /// <summary>
        /// Gets or sets the Binary File Types.
        /// </summary>
        /// <value>
        /// the Binary File Types.
        /// </value>
        public DbSet<BinaryFileType> BinaryFileTypes { get; set; }

        /// <summary>
        /// Gets or sets the Blocks.
        /// </summary>
        /// <value>
        /// the Blocks.
        /// </value>
        public DbSet<Block> Blocks { get; set; }

        /// <summary>
        /// Gets or sets the Block Types.
        /// </summary>
        /// <value>
        /// the Block Types.
        /// </value>
        public DbSet<BlockType> BlockTypes { get; set; }

        /// <summary>
        /// Gets or sets the Campuses.
        /// </summary>
        /// <value>
        /// the Campuses.
        /// </value>
        public DbSet<Campus> Campuses { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the communications.
        /// </summary>
        /// <value>
        /// The communications.
        /// </value>
        public DbSet<Rock.Model.Communication> Communications { get; set; }

        /// <summary>
        /// Gets or sets the communication recipients.
        /// </summary>
        /// <value>
        /// The communication recipients.
        /// </value>
        public DbSet<CommunicationRecipient> CommunicationRecipients { get; set; }

        /// <summary>
        /// Gets or sets the communication templates.
        /// </summary>
        /// <value>
        /// The communication templates.
        /// </value>
        public DbSet<Rock.Model.CommunicationTemplate> CommunicationTemplates { get; set; }

        /// <summary>
        /// Gets or sets the data views.
        /// </summary>
        /// <value>
        /// The data views.
        /// </value>
        public DbSet<DataView> DataViews { get; set; }

        /// <summary>
        /// Gets or sets the data view filters.
        /// </summary>
        /// <value>
        /// The data view filters.
        /// </value>
        public DbSet<DataViewFilter> DataViewFilters { get; set; }

        /// <summary>
        /// Gets or sets the Defined Types.
        /// </summary>
        /// <value>
        /// the Defined Types.
        /// </value>
        public DbSet<DefinedType> DefinedTypes { get; set; }

        /// <summary>
        /// Gets or sets the Defined Values.
        /// </summary>
        /// <value>
        /// the Defined Values.
        /// </value>
        public DbSet<DefinedValue> DefinedValues { get; set; }

        /// <summary>
        /// Gets or sets the devices.
        /// </summary>
        /// <value>
        /// The devices.
        /// </value>
        public DbSet<Device> Devices { get; set; }

        /// <summary>
        /// Gets or sets the Email Templates.
        /// </summary>
        /// <value>
        /// the Email Templates.
        /// </value>
        public DbSet<SystemEmail> EmailTemplates { get; set; }

        /// <summary>
        /// Gets or sets the entity types.
        /// </summary>
        /// <value>
        /// The entity types.
        /// </value>
        public DbSet<EntityType> EntityTypes { get; set; }

        /// <summary>
        /// Gets or sets the Exception Logs.
        /// </summary>
        /// <value>
        /// the Exception Logs.
        /// </value>
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }

        /// <summary>
        /// Gets or sets the Field Types.
        /// </summary>
        /// <value>
        /// the Field Types.
        /// </value>
        public DbSet<FieldType> FieldTypes { get; set; }

        /// <summary>
        /// Gets or sets the accounts.
        /// </summary>
        /// <value>
        /// The fund.
        /// </value>
        public DbSet<FinancialAccount> FinancialAccounts { get; set; }

        /// <summary>
        /// Gets or sets the batches.
        /// </summary>
        /// <value>
        /// The batches.
        /// </value>
        public DbSet<FinancialBatch> FinancialBatches { get; set; }

        /// <summary>
        /// Gets or sets the pledges.
        /// </summary>
        /// <value>
        /// The pledges.
        /// </value>
        public DbSet<FinancialPledge> FinancialPledges { get; set; }

        /// <summary>
        /// Gets or sets the financial person bank account.
        /// </summary>
        /// <value>
        /// The financial person bank account.
        /// </value>
        public DbSet<FinancialPersonBankAccount> FinancialPersonBankAccounts { get; set; }

        /// <summary>
        /// Gets or sets the financial person saved account.
        /// </summary>
        /// <value>
        /// The financial person saved account.
        /// </value>
        public DbSet<FinancialPersonSavedAccount> FinancialPersonSavedAccounts { get; set; }

        /// <summary>
        /// Gets or sets the financial scheduled transactions.
        /// </summary>
        /// <value>
        /// The financial scheduled transactions.
        /// </value>
        public DbSet<FinancialScheduledTransaction> FinancialScheduledTransactions { get; set; }

        /// <summary>
        /// Gets or sets the financial scheduled transaction details.
        /// </summary>
        /// <value>
        /// The financial scheduled transaction details.
        /// </value>
        public DbSet<FinancialScheduledTransactionDetail> FinancialScheduledTransactionDetails { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>
        /// The transactions.
        /// </value>
        public DbSet<FinancialTransaction> FinancialTransactions { get; set; }

        /// <summary>
        /// Gets or sets the transaction details.
        /// </summary>
        /// <value>
        /// The transaction details.
        /// </value>
        public DbSet<FinancialTransactionDetail> FinancialTransactionDetails { get; set; }

        /// <summary>
        /// Gets or sets the transaction details.
        /// </summary>
        /// <value>
        /// The transaction details.
        /// </value>
        public DbSet<FinancialTransactionImage> FinancialTransactionImages { get; set; }

        /// <summary>
        /// Gets or sets the financial transaction refunds.
        /// </summary>
        /// <value>
        /// The financial transaction refunds.
        /// </value>
        public DbSet<FinancialTransactionRefund> FinancialTransactionRefunds { get; set; }

        /// <summary>
        /// Gets or sets the followings.
        /// </summary>
        /// <value>
        /// The followings.
        /// </value>
        public DbSet<Following> Followings { get; set; }

        /// <summary>
        /// Gets or sets the Groups.
        /// </summary>
        /// <value>
        /// the Groups.
        /// </value>
        public DbSet<Group> Groups { get; set; }

        /// <summary>
        /// Gets or sets the Group Locations.
        /// </summary>
        /// <value>
        /// the Group Locations.
        /// </value>
        public DbSet<GroupLocation> GroupLocations { get; set; }

        /// <summary>
        /// Gets or sets the Members.
        /// </summary>
        /// <value>
        /// the Members.
        /// </value>
        public DbSet<GroupMember> GroupMembers { get; set; }

        /// <summary>
        /// Gets or sets the Group Roles.
        /// </summary>
        /// <value>
        /// the Group Roles.
        /// </value>
        public DbSet<GroupTypeRole> GroupRoles { get; set; }

        /// <summary>
        /// Gets or sets the Group Types.
        /// </summary>
        /// <value>
        /// the Group Types.
        /// </value>
        public DbSet<GroupType> GroupTypes { get; set; }

        /// <summary>
        /// Gets or sets the histories.
        /// </summary>
        /// <value>
        /// The histories.
        /// </value>
        public DbSet<History> Histories { get; set; }
        
        /// <summary>
        /// Gets or sets the Html Contents.
        /// </summary>
        /// <value>
        /// the Html Contents.
        /// </value>
        public DbSet<HtmlContent> HtmlContents { get; set; }

        /// <summary>
        /// Gets or sets the Location.
        /// </summary>
        /// <value>
        /// the Location.
        /// </value>
        public DbSet<Location> Locations { get; set; }

        /// <summary>
        /// Gets or sets the marketing campaigns.
        /// </summary>
        /// <value>
        /// The marketing campaigns.
        /// </value>
        public DbSet<MarketingCampaign> MarketingCampaigns { get; set; }

        /// <summary>
        /// Gets or sets the marketing campaign ads.
        /// </summary>
        /// <value>
        /// The marketing campaign ads.
        /// </value>
        public DbSet<MarketingCampaignAd> MarketingCampaignAds { get; set; }

        /// <summary>
        /// Gets or sets the marketing campaign ad types.
        /// </summary>
        /// <value>
        /// The marketing campaign ad types.
        /// </value>
        public DbSet<MarketingCampaignAdType> MarketingCampaignAdTypes { get; set; }

        /// <summary>
        /// Gets or sets the marketing campaign audiences.
        /// </summary>
        /// <value>
        /// The marketing campaign audiences.
        /// </value>
        public DbSet<MarketingCampaignAudience> MarketingCampaignAudiences { get; set; }

        /// <summary>
        /// Gets or sets the marketing campaign campuses.
        /// </summary>
        /// <value>
        /// The marketing campaign campuses.
        /// </value>
        public DbSet<MarketingCampaignCampus> MarketingCampaignCampuses { get; set; }

        /// <summary>
        /// Gets or sets the metaphones.
        /// </summary>
        /// <value>
        /// The metaphones.
        /// </value>
        public DbSet<Metaphone> Metaphones { get; set; }

        /// <summary>
        /// Gets or sets the Defined Types.
        /// </summary>
        /// <value>
        /// the Defined Types.
        /// </value>
        public DbSet<Metric> Metrics { get; set; }

        /// <summary>
        /// Gets or sets the Defined Values.
        /// </summary>
        /// <value>
        /// the Defined Values.
        /// </value>
        public DbSet<MetricValue> MetricValues { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public DbSet<Note> Notes { get; set; }

        /// <summary>
        /// Gets or sets the note types.
        /// </summary>
        /// <value>
        /// The note types.
        /// </value>
        public DbSet<NoteType> NoteTypes { get; set; }
        
        /// <summary>
        /// Gets or sets the Pages.
        /// </summary>
        /// <value>
        /// the Pages.
        /// </value>
        public DbSet<Page> Pages { get; set; }

        /// <summary>
        /// Gets or sets the page contexts.
        /// </summary>
        /// <value>
        /// The page contexts.
        /// </value>
        public DbSet<PageContext> PageContexts { get; set; } 

        /// <summary>
        /// Gets or sets the Page Routes.
        /// </summary>
        /// <value>
        /// the Page Routes.
        /// </value>
        public DbSet<PageRoute> PageRoutes { get; set; }

        /// <summary>
        /// Gets or sets the page views.
        /// </summary>
        /// <value>
        /// The page views.
        /// </value>
        public DbSet<PageView> PageViews { get; set; }

        /// <summary>
        /// Gets or sets the People.
        /// </summary>
        /// <value>
        /// the People.
        /// </value>
        public DbSet<Person> People { get; set; }

        /// <summary>
        /// Gets or sets the Person Aliases.
        /// </summary>
        /// <value>
        /// the Person aliases.
        /// </value>
        public DbSet<PersonAlias> PersonAliases { get; set; }

        /// <summary>
        /// Gets or sets the person badge types.
        /// </summary>
        /// <value>
        /// The person badge types.
        /// </value>
        public DbSet<PersonBadge> PersonBadges { get; set; }

        /// <summary>
        /// Gets or sets the Person Vieweds.
        /// </summary>
        /// <value>
        /// the Person Vieweds.
        /// </value>
        public DbSet<PersonViewed> PersonVieweds { get; set; }

        /// <summary>
        /// Gets or sets the Phone Numbers.
        /// </summary>
        /// <value>
        /// the Phone Numbers.
        /// </value>
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        /// <summary>
        /// Gets or sets the plugin migrations.
        /// </summary>
        /// <value>
        /// The plugin migrations.
        /// </value>
        public DbSet<PluginMigration> PluginMigrations { get; set; }

        /// <summary>
        /// Gets or sets the prayer requests.
        /// </summary>
        /// <value>
        /// The prayer requests.
        /// </value>
        public DbSet<PrayerRequest> PrayerRequests { get; set; }

        /// <summary>
        /// Gets or sets the reports.
        /// </summary>
        /// <value>
        /// The reports.
        /// </value>
        public DbSet<Report> Reports { get; set; }

        /// <summary>
        /// Gets or sets the report fields.
        /// </summary>
        /// <value>
        /// The report fields.
        /// </value>
        public DbSet<ReportField> ReportFields { get; set; }

        /// <summary>
        /// Gets or sets the REST Actions.
        /// </summary>
        /// <value>
        /// The reports.
        /// </value>
        public DbSet<RestAction> RestActions { get; set; }

        /// <summary>
        /// Gets or sets the REST Controllers.
        /// </summary>
        /// <value>
        /// The reports.
        /// </value>
        public DbSet<RestController> RestControllers { get; set; }

        /// <summary>
        /// Gets or sets the schedules.
        /// </summary>
        /// <value>
        /// The schedules.
        /// </value>
        public DbSet<Schedule> Schedules { get; set; }

        /// <summary>
        /// Gets or sets the Jobs.
        /// </summary>
        /// <value>
        /// the Jobs.
        /// </value>
        public DbSet<ServiceJob> ServiceJobs { get; set; }

        /// <summary>
        /// Gets or sets the Service Logs.
        /// </summary>
        /// <value>
        /// the Service Logs.
        /// </value>
        public DbSet<ServiceLog> ServiceLogs { get; set; }

        /// <summary>
        /// Gets or sets the Sites.
        /// </summary>
        /// <value>
        /// the Sites.
        /// </value>
        public DbSet<Site> Sites { get; set; }

        /// <summary>
        /// Gets or sets the Site Domains.
        /// </summary>
        /// <value>
        /// the Site Domains.
        /// </value>
        public DbSet<SiteDomain> SiteDomains { get; set; }

        /// <summary>
        /// Gets or sets the Tags.
        /// </summary>
        /// <value>
        /// the Tags.
        /// </value>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the Tagged Items.
        /// </summary>
        /// <value>
        /// the Tagged Items.
        /// </value>
        public DbSet<TaggedItem> TaggedItems { get; set; }

        /// <summary>
        /// Gets or sets the Users.
        /// </summary>
        /// <value>
        /// the Users.
        /// </value>
        public DbSet<UserLogin> UserLogins { get; set; }

        /// <summary>
        /// Gets or sets the workflows.
        /// </summary>
        /// <value>
        /// The workflows.
        /// </value>
        public DbSet<Rock.Model.Workflow> Workflows { get; set; }

        /// <summary>
        /// Gets or sets the workflow actions.
        /// </summary>
        /// <value>
        /// The workflow actions.
        /// </value>
        public DbSet<WorkflowAction> WorkflowActions { get; set; }

        /// <summary>
        /// Gets or sets the workflow action types.
        /// </summary>
        /// <value>
        /// The workflow action types.
        /// </value>
        public DbSet<WorkflowActionType> WorkflowActionTypes { get; set; }

        /// <summary>
        /// Gets or sets the workflow activities.
        /// </summary>
        /// <value>
        /// The workflow activities.
        /// </value>
        public DbSet<WorkflowActivity> WorkflowActivities { get; set; }

        /// <summary>
        /// Gets or sets the workflow activity types.
        /// </summary>
        /// <value>
        /// The workflow activity types.
        /// </value>
        public DbSet<WorkflowActivityType> WorkflowActivityTypes { get; set; }

        /// <summary>
        /// Gets or sets the workflow action form.
        /// </summary>
        /// <value>
        /// The workflow action form.
        /// </value>
        public DbSet<WorkflowActionForm> WorkflowActionForms { get; set; }

        /// <summary>
        /// Gets or sets the workflow form attributes.
        /// </summary>
        /// <value>
        /// The workflow form attributes.
        /// </value>
        public DbSet<WorkflowActionFormAttribute> WorkflowActionFormAttributes { get; set; }

        /// <summary>
        /// Gets or sets the workflow logs.
        /// </summary>
        /// <value>
        /// The workflow logs.
        /// </value>
        public DbSet<WorkflowLog> WorkflowLogs { get; set; }

        /// <summary>
        /// Gets or sets the workflow triggers.
        /// </summary>
        /// <value>
        /// The entity type workflow triggers.
        /// </value>
        public DbSet<WorkflowTrigger> WorkflowTriggers { get; set; }

        /// <summary>
        /// Gets or sets the workflow types.
        /// </summary>
        /// <value>
        /// The workflow types.
        /// </value>
        public DbSet<WorkflowType> WorkflowTypes { get; set; }

        #endregion

        /// <summary>
        /// This method is called when the context has been initialized, but
        /// before the model has been locked down and used to initialize the context. 
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            ContextHelper.AddConfigurations( modelBuilder );
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class ContextHelper
    {
        /// <summary>
        /// Adds the configurations.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void AddConfigurations(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.AddFromAssembly( typeof( RockContext ).Assembly );
        }
    }
}