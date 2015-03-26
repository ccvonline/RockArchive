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
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using Rock;
using Rock.Data;
using Rock.MergeTemplates;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Core
{
    /// <summary>
    /// 
    /// </summary>
    [DisplayName( "Merge Template Entry" )]
    [Category( "Core" )]
    [Description( "Used for merging data into output documents, such as Word, Html, using a pre-defined template." )]
    public partial class MergeTemplateEntry : RockBlock
    {
        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                int? entitySetId = this.PageParameter( "Set" ).AsIntegerOrNull();
                pnlEntry.Visible = entitySetId.HasValue;

                if ( entitySetId.HasValue )
                {
                    ShowMergeForEntitySetId( entitySetId.Value );
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            //
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shows the merge for entity set identifier.
        /// </summary>
        /// <param name="entitySetId">The entity set identifier.</param>
        protected void ShowMergeForEntitySetId( int entitySetId )
        {
            hfEntitySetId.Value = entitySetId.ToString();
            var rockContext = new RockContext();
            var entitySetService = new EntitySetService( rockContext );
            var entitySetItemsService = new EntitySetItemService( rockContext );
            var entitySet = entitySetService.Get( entitySetId );
            if ( entitySet == null )
            {
                nbWarningMessage.Text = "Merge Records not found";
                nbWarningMessage.Title = "Warning";
                nbWarningMessage.NotificationBoxType = NotificationBoxType.Warning;
                pnlEntry.Visible = false;
                return;
            }

            if ( entitySet.EntityTypeId.HasValue )
            {
                var entityTypeCache = EntityTypeCache.Read( entitySet.EntityTypeId.Value );
                bool isPersonEntitySet = entityTypeCache.Guid == Rock.SystemGuid.EntityType.PERSON.AsGuid();
                cbCombineFamilyMembers.Visible = isPersonEntitySet;
            }
            else
            {
                cbCombineFamilyMembers.Visible = false;
            }

            int itemsCount = entitySetItemsService.Queryable().Where( a => a.EntitySetId == entitySetId ).Count();

            nbNumberOfRecords.Text = string.Format( "There are {0} {1} to merge", itemsCount, "row".PluralizeIf( itemsCount != 1 ) );
        }

        /// <summary>
        /// Handles the Click event of the btnMerge control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnMerge_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();

            List<Dictionary<string, object>> mergeObjectsList = GetMergeObjectList( rockContext );

            var mergeTemplate = new MergeTemplateService( rockContext ).Get( mtPicker.SelectedValue.AsInteger() );
            if ( mergeTemplate == null )
            {
                nbWarningMessage.Text = "Unable to get merge template";
                nbWarningMessage.NotificationBoxType = NotificationBoxType.Danger;
                nbWarningMessage.Visible = true;
                return;
            }

            var mergeTemplateTypeEntityType = EntityTypeCache.Read( mergeTemplate.MergeTemplateTypeEntityTypeId );
            if ( mergeTemplateTypeEntityType == null )
            {
                nbWarningMessage.Text = "Unable to get merge template";
                nbWarningMessage.NotificationBoxType = NotificationBoxType.Danger;
                nbWarningMessage.Visible = true;
                return;
            }

            var mergeTemplateType = MergeTemplateTypeContainer.GetComponent( mergeTemplateTypeEntityType.Name );
            if ( mergeTemplateType == null )
            {
                nbWarningMessage.Text = "Unable to get merge template type";
                nbWarningMessage.NotificationBoxType = NotificationBoxType.Danger;
                nbWarningMessage.Visible = true;
                return;
            }

            var outputBinaryFileDoc = mergeTemplateType.CreateDocument( mergeTemplate, mergeObjectsList );

            if ( mergeTemplateType.Exceptions != null && mergeTemplateType.Exceptions.Any() )
            {
                if ( mergeTemplateType.Exceptions.Count == 1 )
                {
                    this.LogException( mergeTemplateType.Exceptions[0] );
                }
                else if ( mergeTemplateType.Exceptions.Count > 50 )
                {
                    this.LogException( new AggregateException( string.Format( "Exceptions merging template {0}. See InnerExceptions for top 50.", mergeTemplate.Name ), mergeTemplateType.Exceptions.Take( 50 ).ToList() ) );
                }
                else
                {
                    this.LogException( new AggregateException( string.Format( "Exceptions merging template {0}. See InnerExceptions", mergeTemplate.Name ), mergeTemplateType.Exceptions.ToList() ) );
                }
            }

            Response.Redirect( outputBinaryFileDoc.Url, false );
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        /// <summary>
        /// Gets the merge object list for the current EntitySet
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="fetchCount">The fetch count.</param>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetMergeObjectList( RockContext rockContext, int? fetchCount = null )
        {
            int entitySetId = hfEntitySetId.Value.AsInteger();
            var entitySetService = new EntitySetService( rockContext );
            var entitySet = entitySetService.Get( entitySetId );
            Dictionary<int, Dictionary<string, object>> mergeObjectsDictionary = new Dictionary<int, Dictionary<string, object>>();

            // If this EntitySet contains IEntity Items, add those first
            if ( entitySet.EntityTypeId.HasValue )
            {
                var qryEntity = entitySetService.GetEntityQuery( entitySetId );

                if ( fetchCount.HasValue )
                {
                    qryEntity = qryEntity.Take( fetchCount.Value );
                }

                var globalMergeObjects = GlobalAttributesCache.GetMergeFields( this.CurrentPerson );

                var entityTypeCache = EntityTypeCache.Read( entitySet.EntityTypeId.Value );
                bool isPersonEntityType = entityTypeCache != null && entityTypeCache.Guid == Rock.SystemGuid.EntityType.PERSON.AsGuid();
                bool combineFamilyMembers = cbCombineFamilyMembers.Visible && cbCombineFamilyMembers.Checked;

                if ( isPersonEntityType && combineFamilyMembers )
                {
                    var qryPersons = qryEntity;
                    Guid familyGroupType = Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuid();
                    var qryFamilyGroupMembers = new GroupMemberService( rockContext ).Queryable()
                        .Where( a => a.Group.GroupType.Guid == familyGroupType )
                        .Where( a => qryPersons.Any( aa => aa.Id == a.PersonId ) );

                    var qryCombined = qryFamilyGroupMembers.Join( 
                        qryPersons, 
                        m => m.PersonId,
                        p => p.Id,
                        ( m, p ) => new { GroupMember = m, Person = p } )
                        .GroupBy( a => a.GroupMember.GroupId )
                        .Select( x => new
                        {
                            GroupId = x.Key,
                            CombinedPerson = x.FirstOrDefault().Person,
                            Persons = x.Select( xx => xx.Person )
                        } );

                    foreach ( var combinedFamilyItem in qryCombined )
                    {
                        var mergeObjects = new Dictionary<string, object>();
                        foreach ( var g in globalMergeObjects )
                        {
                            mergeObjects.Add( g.Key, g.Value );
                        }

                        string commaPersonIds = combinedFamilyItem.Persons.Select( a => a.Id ).Distinct().ToList().AsDelimited( "," );

                        int combinedPersonId = combinedFamilyItem.CombinedPerson != null ? combinedFamilyItem.CombinedPerson.Id : 0;

                        var familyTitle = RockUdfHelper.ufnCrm_GetFamilyTitle( rockContext, null, combinedFamilyItem.GroupId, commaPersonIds );

                        //TODO, have Lava do this
                        familyTitle = familyTitle.EncodeHtml();

                        mergeObjects.Add( "CurrentPerson", this.CurrentPerson );
                        var combinedPerson = combinedFamilyItem.CombinedPerson.Clone() as Person;
                        combinedPerson.FirstName = familyTitle;
                        combinedPerson.NickName = familyTitle;
                        combinedPerson.LastName = null;
                        mergeObjects.Add( "Row", combinedPerson );
                        mergeObjectsDictionary.AddOrIgnore( combinedPersonId, mergeObjects );
                    }
                }
                else
                {
                    foreach ( var item in qryEntity )
                    {
                        var mergeObjects = new Dictionary<string, object>();
                        foreach ( var g in globalMergeObjects )
                        {
                            mergeObjects.Add( g.Key, g.Value );
                        }

                        mergeObjects.Add( "CurrentPerson", this.CurrentPerson );
                        mergeObjects.Add( "Row", item );
                        mergeObjectsDictionary.Add( item.Id, mergeObjects );
                    }
                }
            }

            var entitySetItemService = new EntitySetItemService( rockContext );
            var entitySetItemMergeValuesQry = entitySetItemService.GetByEntitySetId( entitySetId, true ).Where( a => a.AdditionalMergeValuesJson != "" );
            if ( fetchCount.HasValue )
            {
                entitySetItemMergeValuesQry = entitySetItemMergeValuesQry.Take( fetchCount.Value );
            }

            // now, add the additional MergeValues regardless of if the EntitySet contains IEntity items or just Non-IEntity items
            foreach ( var additionalMergeValuesItem in entitySetItemMergeValuesQry )
            {
                Dictionary<string, object> mergeObjects;
                if ( mergeObjectsDictionary.ContainsKey( additionalMergeValuesItem.EntityId ) )
                {
                    mergeObjects = mergeObjectsDictionary[additionalMergeValuesItem.EntityId];
                }
                else
                {
                    mergeObjects = new Dictionary<string, object>();
                    mergeObjectsDictionary.Add( additionalMergeValuesItem.EntityId, mergeObjects );
                }

                foreach ( var additionalMergeValue in additionalMergeValuesItem.AdditionalMergeValues )
                {
                    mergeObjects.AddOrIgnore( additionalMergeValue.Key, additionalMergeValue.Value );
                }
            }

            var result = mergeObjectsDictionary.Select( a => a.Value );
            if ( fetchCount.HasValue )
            {
                // make sure the result is limited to fetchCount (even though the above queries are also limited to fetch count)
                result = result.Take( fetchCount.Value );
            }

            return result.ToList();
        }

        /// <summary>
        /// Handles the Click event of the btnShowDataPreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnShowDataPreview_Click( object sender, EventArgs e )
        {
            if ( pnlPreview.Visible )
            {
                pnlPreview.Visible = false;
                return;
            }

            var rockContext = new RockContext();

            int entitySetId = hfEntitySetId.Value.AsInteger();
            var entitySetService = new EntitySetService( rockContext );
            var entitySet = entitySetService.Get( entitySetId );
            if ( entitySet.EntityTypeId.HasValue )
            {
                var qry = entitySetService.GetEntityQuery( entitySetId ).Take( 15 );

                EntityTypeCache itemEntityType = EntityTypeCache.Read( entitySet.EntityTypeId ?? 0 );
                gPreview.CreatePreviewColumns( itemEntityType.GetEntityType() );

                gPreview.DataSource = qry.ToList();
                gPreview.DataBind();
            }
            else
            {
                var entitySetItemService = new EntitySetItemService( rockContext );
                var qry = entitySetItemService.GetByEntitySetId( entitySetId, true ).Take( 15 );
                var list = qry.ToList().Select( a => a.AdditionalMergeValuesJson.FromJsonOrNull<Dictionary<string, object>>() ).ToList();
                if ( list.Any() )
                {
                    gPreview.Columns.Clear();
                    foreach ( var s in list[0] )
                    {
                        var gridField = Grid.GetGridField( s.Value != null ? s.Value.GetType() : typeof( string ) );
                        gridField.HeaderText = s.Key.SplitCase();
                        gridField.DataField = s.Key;
                        gPreview.Columns.Add( gridField );
                    }

                    gPreview.DataSource = qry.ToList().Select( a => a.AdditionalMergeValuesJson.FromJsonOrNull<object>() ).ToList();
                    gPreview.DataBind();
                }
            }

            pnlPreview.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the btnShowMergeFieldsHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnShowMergeFieldsHelp_Click( object sender, EventArgs e )
        {
            pnlMergeFieldsHelp.Visible = !pnlMergeFieldsHelp.Visible;

            if ( pnlMergeFieldsHelp.Visible )
            {
                List<Dictionary<string, object>> mergeObjectsList = GetMergeObjectList( new RockContext(), 1 );

                if ( mergeObjectsList.Count == 1 )
                {
                    lShowMergeFields.Text = mergeObjectsList[0].lavaDebugInfo();
                }
            }
        }

        #endregion
    }
}