//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
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
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// PersonAlias Service class
    /// </summary>
    public partial class PersonAliasService : Service<PersonAlias>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonAliasService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public PersonAliasService(RockContext context) : base(context)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( PersonAlias item, out string errorMessage )
        {
            errorMessage = string.Empty;
            
            // ignoring Attendance,CreatedByPersonAliasId 
            
            // ignoring Attendance,ModifiedByPersonAliasId 
            
            // ignoring Attribute,CreatedByPersonAliasId 
            
            // ignoring Attribute,ModifiedByPersonAliasId 
            
            // ignoring AttributeValue,CreatedByPersonAliasId 
            
            // ignoring AttributeValue,ModifiedByPersonAliasId 
            
            // ignoring Auth,CreatedByPersonAliasId 
            
            // ignoring Auth,ModifiedByPersonAliasId 
            
            // ignoring BinaryFile,CreatedByPersonAliasId 
            
            // ignoring BinaryFile,ModifiedByPersonAliasId 
            
            // ignoring BinaryFileData,CreatedByPersonAliasId 
            
            // ignoring BinaryFileData,ModifiedByPersonAliasId 
            
            // ignoring BinaryFileType,CreatedByPersonAliasId 
            
            // ignoring BinaryFileType,ModifiedByPersonAliasId 
            
            // ignoring Block,CreatedByPersonAliasId 
            
            // ignoring Block,ModifiedByPersonAliasId 
            
            // ignoring BlockType,CreatedByPersonAliasId 
            
            // ignoring BlockType,ModifiedByPersonAliasId 
            
            // ignoring Campus,CreatedByPersonAliasId 
            
            // ignoring Campus,ModifiedByPersonAliasId 
            
            // ignoring Category,CreatedByPersonAliasId 
            
            // ignoring Category,ModifiedByPersonAliasId 
            
            // ignoring Communication,CreatedByPersonAliasId 
            
            // ignoring Communication,ModifiedByPersonAliasId 
            
            // ignoring CommunicationRecipient,CreatedByPersonAliasId 
            
            // ignoring CommunicationRecipient,ModifiedByPersonAliasId 
            
            // ignoring CommunicationRecipientActivity,CreatedByPersonAliasId 
            
            // ignoring CommunicationRecipientActivity,ModifiedByPersonAliasId 
            
            // ignoring CommunicationTemplate,CreatedByPersonAliasId 
            
            // ignoring CommunicationTemplate,ModifiedByPersonAliasId 
            
            // ignoring CommunicationTemplate,SenderPersonAliasId 
            
            // ignoring DataView,CreatedByPersonAliasId 
            
            // ignoring DataView,ModifiedByPersonAliasId 
            
            // ignoring DataViewFilter,CreatedByPersonAliasId 
            
            // ignoring DataViewFilter,ModifiedByPersonAliasId 
            
            // ignoring DefinedType,CreatedByPersonAliasId 
            
            // ignoring DefinedType,ModifiedByPersonAliasId 
            
            // ignoring DefinedValue,CreatedByPersonAliasId 
            
            // ignoring DefinedValue,ModifiedByPersonAliasId 
            
            // ignoring Device,CreatedByPersonAliasId 
            
            // ignoring Device,ModifiedByPersonAliasId 
            
            // ignoring ExceptionLog,CreatedByPersonAliasId 
            
            // ignoring ExceptionLog,ModifiedByPersonAliasId 
            
            // ignoring FieldType,CreatedByPersonAliasId 
            
            // ignoring FieldType,ModifiedByPersonAliasId 
            
            // ignoring FinancialAccount,CreatedByPersonAliasId 
            
            // ignoring FinancialAccount,ModifiedByPersonAliasId 
            
            // ignoring FinancialBatch,CreatedByPersonAliasId 
            
            // ignoring FinancialBatch,ModifiedByPersonAliasId 
            
            // ignoring FinancialPersonBankAccount,CreatedByPersonAliasId 
            
            // ignoring FinancialPersonBankAccount,ModifiedByPersonAliasId 
            
            // ignoring FinancialPersonSavedAccount,CreatedByPersonAliasId 
            
            // ignoring FinancialPersonSavedAccount,ModifiedByPersonAliasId 
            
            // ignoring FinancialPledge,CreatedByPersonAliasId 
            
            // ignoring FinancialPledge,ModifiedByPersonAliasId 
            
            // ignoring FinancialScheduledTransaction,CreatedByPersonAliasId 
            
            // ignoring FinancialScheduledTransaction,ModifiedByPersonAliasId 
            
            // ignoring FinancialScheduledTransactionDetail,CreatedByPersonAliasId 
            
            // ignoring FinancialScheduledTransactionDetail,ModifiedByPersonAliasId 
            
            // ignoring FinancialTransaction,CreatedByPersonAliasId 
            
            // ignoring FinancialTransaction,ModifiedByPersonAliasId 
            
            // ignoring FinancialTransactionDetail,CreatedByPersonAliasId 
            
            // ignoring FinancialTransactionDetail,ModifiedByPersonAliasId 
            
            // ignoring FinancialTransactionImage,CreatedByPersonAliasId 
            
            // ignoring FinancialTransactionImage,ModifiedByPersonAliasId 
            
            // ignoring FinancialTransactionRefund,CreatedByPersonAliasId 
            
            // ignoring FinancialTransactionRefund,ModifiedByPersonAliasId 
            
            // ignoring Following,CreatedByPersonAliasId 
            
            // ignoring Following,ModifiedByPersonAliasId 
            
            // ignoring Group,CreatedByPersonAliasId 
            
            // ignoring Group,ModifiedByPersonAliasId 
            
            // ignoring GroupLocation,CreatedByPersonAliasId 
            
            // ignoring GroupLocation,ModifiedByPersonAliasId 
            
            // ignoring GroupMember,CreatedByPersonAliasId 
            
            // ignoring GroupMember,ModifiedByPersonAliasId 
            
            // ignoring GroupType,CreatedByPersonAliasId 
            
            // ignoring GroupType,ModifiedByPersonAliasId 
            
            // ignoring GroupTypeRole,CreatedByPersonAliasId 
            
            // ignoring GroupTypeRole,ModifiedByPersonAliasId 
            
            // ignoring History,CreatedByPersonAliasId 
            
            // ignoring History,ModifiedByPersonAliasId 
            
            // ignoring HtmlContent,CreatedByPersonAliasId 
            
            // ignoring HtmlContent,ModifiedByPersonAliasId 
            
            // ignoring Layout,CreatedByPersonAliasId 
            
            // ignoring Layout,ModifiedByPersonAliasId 
            
            // ignoring Location,CreatedByPersonAliasId 
            
            // ignoring Location,ModifiedByPersonAliasId 
            
            // ignoring MarketingCampaign,CreatedByPersonAliasId 
            
            // ignoring MarketingCampaign,ModifiedByPersonAliasId 
            
            // ignoring MarketingCampaignAd,CreatedByPersonAliasId 
            
            // ignoring MarketingCampaignAd,ModifiedByPersonAliasId 
            
            // ignoring MarketingCampaignAdType,CreatedByPersonAliasId 
            
            // ignoring MarketingCampaignAdType,ModifiedByPersonAliasId 
            
            // ignoring MarketingCampaignAudience,CreatedByPersonAliasId 
            
            // ignoring MarketingCampaignAudience,ModifiedByPersonAliasId 
            
            // ignoring MarketingCampaignCampus,CreatedByPersonAliasId 
            
            // ignoring MarketingCampaignCampus,ModifiedByPersonAliasId 
            
            // ignoring Metric,AdminPersonAliasId 
            
            // ignoring Metric,CreatedByPersonAliasId 
            
            // ignoring Metric,MetricChampionPersonAliasId 
            
            // ignoring Metric,ModifiedByPersonAliasId 
            
            // ignoring MetricValue,CreatedByPersonAliasId 
            
            // ignoring MetricValue,ModifiedByPersonAliasId 
            
            // ignoring Note,CreatedByPersonAliasId 
            
            // ignoring Note,ModifiedByPersonAliasId 
            
            // ignoring NoteType,CreatedByPersonAliasId 
            
            // ignoring NoteType,ModifiedByPersonAliasId 
            
            // ignoring Page,CreatedByPersonAliasId 
            
            // ignoring Page,ModifiedByPersonAliasId 
            
            // ignoring PageContext,CreatedByPersonAliasId 
            
            // ignoring PageContext,ModifiedByPersonAliasId 
            
            // ignoring PageRoute,CreatedByPersonAliasId 
            
            // ignoring PageRoute,ModifiedByPersonAliasId 
 
            if ( new Service<PageView>( Context ).Queryable().Any( a => a.PersonAliasId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", PersonAlias.FriendlyTypeName, PageView.FriendlyTypeName );
                return false;
            }  
            
            // ignoring Person,CreatedByPersonAliasId 
            
            // ignoring Person,ModifiedByPersonAliasId 
            
            // ignoring PersonBadge,CreatedByPersonAliasId 
            
            // ignoring PersonBadge,ModifiedByPersonAliasId 
            
            // ignoring PersonViewed,TargetPersonAliasId 
            
            // ignoring PersonViewed,ViewerPersonAliasId 
            
            // ignoring PhoneNumber,CreatedByPersonAliasId 
            
            // ignoring PhoneNumber,ModifiedByPersonAliasId 
            
            // ignoring PrayerRequest,CreatedByPersonAliasId 
            
            // ignoring PrayerRequest,ModifiedByPersonAliasId 
            
            // ignoring Report,CreatedByPersonAliasId 
            
            // ignoring Report,ModifiedByPersonAliasId 
            
            // ignoring ReportField,CreatedByPersonAliasId 
            
            // ignoring ReportField,ModifiedByPersonAliasId 
            
            // ignoring RestAction,CreatedByPersonAliasId 
            
            // ignoring RestAction,ModifiedByPersonAliasId 
            
            // ignoring RestController,CreatedByPersonAliasId 
            
            // ignoring RestController,ModifiedByPersonAliasId 
            
            // ignoring Schedule,CreatedByPersonAliasId 
            
            // ignoring Schedule,ModifiedByPersonAliasId 
            
            // ignoring ServiceJob,CreatedByPersonAliasId 
            
            // ignoring ServiceJob,ModifiedByPersonAliasId 
            
            // ignoring ServiceLog,CreatedByPersonAliasId 
            
            // ignoring ServiceLog,ModifiedByPersonAliasId 
            
            // ignoring Site,CreatedByPersonAliasId 
            
            // ignoring Site,ModifiedByPersonAliasId 
            
            // ignoring SiteDomain,CreatedByPersonAliasId 
            
            // ignoring SiteDomain,ModifiedByPersonAliasId 
            
            // ignoring SystemEmail,CreatedByPersonAliasId 
            
            // ignoring SystemEmail,ModifiedByPersonAliasId 
            
            // ignoring Tag,CreatedByPersonAliasId 
            
            // ignoring Tag,ModifiedByPersonAliasId 
            
            // ignoring TaggedItem,CreatedByPersonAliasId 
            
            // ignoring TaggedItem,ModifiedByPersonAliasId 
            
            // ignoring UserLogin,CreatedByPersonAliasId 
            
            // ignoring UserLogin,ModifiedByPersonAliasId 
            
            // ignoring Workflow,CreatedByPersonAliasId 
            
            // ignoring Workflow,ModifiedByPersonAliasId 
            
            // ignoring WorkflowAction,CreatedByPersonAliasId 
            
            // ignoring WorkflowAction,ModifiedByPersonAliasId 
            
            // ignoring WorkflowActionType,CreatedByPersonAliasId 
            
            // ignoring WorkflowActionType,ModifiedByPersonAliasId 
            
            // ignoring WorkflowActivity,CreatedByPersonAliasId 
            
            // ignoring WorkflowActivity,ModifiedByPersonAliasId 
            
            // ignoring WorkflowActivityType,CreatedByPersonAliasId 
            
            // ignoring WorkflowActivityType,ModifiedByPersonAliasId 
            
            // ignoring WorkflowType,CreatedByPersonAliasId 
            
            // ignoring WorkflowType,ModifiedByPersonAliasId 
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class PersonAliasExtensionMethods
    {
        /// <summary>
        /// Clones this PersonAlias object to a new PersonAlias object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static PersonAlias Clone( this PersonAlias source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as PersonAlias;
            }
            else
            {
                var target = new PersonAlias();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another PersonAlias object to this PersonAlias object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this PersonAlias target, PersonAlias source )
        {
            target.Name = source.Name;
            target.PersonId = source.PersonId;
            target.AliasPersonId = source.AliasPersonId;
            target.AliasPersonGuid = source.AliasPersonGuid;
            target.Id = source.Id;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}
