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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Web.UI;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using church.ccv.Promotions;
using church.ccv.Promotions.Model;
using church.ccv.Promotions.Data;
using System.Linq;
using Rock;
using Rock.Web.Cache;
using Newtonsoft.Json;
using System.Web.UI.WebControls;

namespace RockWeb.Plugins.church_ccv.Promotions
{
    [DisplayName( "Promotion List" )]
    [Category( "CCV > Promotions" )]
    [Description( "Lists promotions with active content items." )]
    [LinkedPage( "Detail Page" )]
    public partial class PromotionList : RockBlock
    {
        public object RockTransactionScope { get; private set; }
        
        /// <summary>
        /// Used by the Promotion Occurrences grid to know which promotion/event to enumerate
        /// </summary>
        int SelectedEventId { get; set; }

        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            PopulatePromoTypesControl( );
                        
            foreach ( var campus in CampusCache.All() )
            {
                ddlCampus.Items.Add( new ListItem( campus.Name, campus.Id.ToString().ToUpper() ) );
            }
            ddlCampus.SelectedIndex = 0;
            
            InitPromotionsFilter( );
            InitPromotionOccurrencesGrid( );
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
                BindPromotionsFilter();

                RestoreDateControls( );

                BindPromotionOccurrencesGrid( );
            }
        }

        void PopulatePromoTypesControl( )
        {
            // Filter Promos
            ddlPromoType.Items.Clear( );

            // first, get all of the promotions we're currently managing
            PromotionsService<PromotionRequest> promoService = new PromotionsService<PromotionRequest>( new PromotionsContext( ) );
            var eventOccurrenceIds = promoService.Queryable( ).Select( pr => pr.EventItemOccurrenceId ).ToList( );

            // now, we want to get all the relevant content channels. how do we know which ones?
            // An event calendar has a list of supported content channels. So we want all content channels for all the event calendars we're using.
            RockContext rockContext = new RockContext();
            var eventItemIds = new EventItemOccurrenceService( rockContext ).Queryable( ).Where( eio => eventOccurrenceIds.Contains( eio.Id ) ).Select( ei => ei.EventItemId );
            var eventCalIds = new EventCalendarItemService( rockContext ).Queryable( ).Where( eci => eventItemIds.Contains( eci.EventItemId ) ).Select( eci => eci.EventCalendarId );
            var contentChannelIds = new EventCalendarContentChannelService( rockContext ).Queryable( ).Where( ecc => eventCalIds.Contains( ecc.EventCalendarId ) ).Select( ecc => ecc.ContentChannelId );
            
            // got a list! Now pull it into memory
            var contentChannels = new ContentChannelService( rockContext ).Queryable()
                .Where( c=> contentChannelIds.Contains( c.Id ) )
                .ToList();
            
            // add each item to the filter
            foreach( ContentChannel cc in contentChannels )
            {
                ddlPromoType.Items.Add( new ListItem( cc.Name, cc.Id.ToString( ) ) );
            }
            ddlPromoType.Items.Insert( 0, new ListItem( "", "" ) );

            ddlPromoType.DataBind();
        }

        void RestoreDateControls( )
        {
            string campusId = GetUserPreference( "Campus" );
            ddlCampus.SelectedValue = campusId;
            
            dpTargetPromoDate.SelectedDate = GetUserPreference( "TargetPromoDate" ).AsDateTime( );

            ddlPromoType.SetValue( GetUserPreference( "PromotionType" ) );
        }

        void SaveDateControls( )
        {
            SetUserPreference( "TargetPromoDate", dpTargetPromoDate.SelectedDate.ToString( ) );

            SetUserPreference( "Campus", ddlCampus.SelectedValue );

            SetUserPreference( "PromotionType", ddlPromoType.Items[ ddlPromoType.SelectedIndex ].Value );
        }
        
        protected override object SaveViewState()
        {
            ViewState[ "SelectedRowIndex" ] = SelectedEventId;

            return base.SaveViewState();
        }

        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );

            SelectedEventId = (Int32)ViewState[ "SelectedRowIndex" ];
        }

        #endregion

        #region Promotions Filter Events
        void InitPromotionsFilter( )
        {
            rPromotionsFilter.ApplyFilterClick += PromotionsFilter_ApplyFilterClick;
            rPromotionsFilter.DisplayFilterValue += PromotionsFilter_DisplayFilterValue;
        }

        private void BindPromotionsFilter()
        {
            tbTitle.Text = rPromotionsFilter.GetUserPreference( "Title" );
        }

        /// <summary>
        /// Rs the filter_ display filter value.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void PromotionsFilter_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
        {
            if( e.Key == "Title" )
            {
                e.Value = tbTitle.Text;
            }
            else
            {
                e.Value = string.Empty;
            }
        }

        void PromotionsFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            rPromotionsFilter.SaveUserPreference( "Title", tbTitle.Text );
            
            BindPromotionOccurrencesGrid( );
        }
        #endregion

        #region Button Clicks
        protected void ApplyDates( object sender, EventArgs e )
        {
            SaveDateControls( );

            BindPromotionOccurrencesGrid( );
        }

        protected void PromotionOccurrencesGrid_AddClick( object sender, EventArgs e )
        {
            // restore the date controls, so that if htey were changed before clicking 'apply', it reverts to what they've last APPLIED.
            RestoreDateControls( );

            // require that a promotion type be set.
            ListItem promoItem = ddlPromoType.Items[ ddlPromoType.SelectedIndex ];
            if( string.IsNullOrEmpty( promoItem.Value ) == false )
            {
                RockContext rockContext = new RockContext();
                var contentChannel = new ContentChannelService( rockContext).Get( promoItem.Value.AsInteger( ) );

                // figure out if this is a single or multi-campus channel type.
                var campusObj = new CampusService( rockContext ).Get( ddlCampus.SelectedValue.AsInteger( ) );
                bool multiCampus = PromotionsUtil.IsContentChannelMultiCampus( contentChannel.Id );

                // the campus attribute type (multi or single) will determine how we setup the data
                string campusGuid = multiCampus == true ? Rock.SystemGuid.FieldType.CAMPUSES : Rock.SystemGuid.FieldType.CAMPUS;
                
                PromotionsUtil.CreatePromotionOccurrence( contentChannel.Id,
                                                          contentChannel.ContentChannelTypeId,
                                                          dpTargetPromoDate.SelectedDate.HasValue ? dpTargetPromoDate.SelectedDate.Value : DateTime.Now,
                                                          CurrentPersonAliasId,
                                                          "New Promotion Occurrence",
                                                          string.Empty,
                                                          campusGuid,
                                                          campusObj.Guid.ToString(),
                                                          null );

                BindPromotionOccurrencesGrid( );
            }
        }
        #endregion

        #region Promotion Occurrences Grid Methods

        void InitPromotionOccurrencesGrid( )
        {
            gPromotionOccurrencesGrid.DataKeyNames = new string[] { "Id" };

            // turn on only the 'add' button
            gPromotionOccurrencesGrid.Actions.Visible = true;
            gPromotionOccurrencesGrid.Actions.Enabled = true;
            gPromotionOccurrencesGrid.Actions.ShowBulkUpdate = false;
            gPromotionOccurrencesGrid.Actions.ShowCommunicate = false;
            gPromotionOccurrencesGrid.Actions.ShowExcelExport = false;
            gPromotionOccurrencesGrid.Actions.ShowMergePerson = false;
            gPromotionOccurrencesGrid.Actions.ShowMergeTemplate = false;

            gPromotionOccurrencesGrid.Actions.ShowAdd = true;
            gPromotionOccurrencesGrid.Actions.AddClick += PromotionOccurrencesGrid_AddClick;

            gPromotionOccurrencesGrid.GridRebind += PromotionOccurrencesGrid_Rebind;
        }

        protected void PromotionOccurrencesGrid_Remove( object sender, RowEventArgs e )
        {
            // delete the clicked promo occurrence
            PromotionsContext promoContext = new PromotionsContext( );
            PromotionsService<PromotionOccurrence> promoOccurrService = new PromotionsService<PromotionOccurrence>( promoContext );
            PromotionOccurrence promoOccur = promoOccurrService.Queryable( ).Where( po => po.Id == e.RowKeyId ).SingleOrDefault( );
            
            // get its content channel item
            RockContext rockContext = new RockContext( );
            ContentChannelItem contentChannelItem = new ContentChannelItemService( rockContext ).Get( promoOccur.ContentChannelItemId );

            rockContext.ContentChannelItems.Remove( contentChannelItem );
            rockContext.SaveChanges( );

            promoContext.PromotionOccurrence.Remove( promoOccur );
            promoContext.SaveChanges( );

            // refresh
            BindPromotionOccurrencesGrid( );
        }

        protected void PromotionOccurrencesGrid_RowSelected( object sender, RowEventArgs e )
        {
            PromotionsContext promoContext = new PromotionsContext( );
            PromotionsService<PromotionOccurrence> promoOccurrService = new PromotionsService<PromotionOccurrence>( promoContext );
            PromotionOccurrence promoOccur = promoOccurrService.Queryable( ).Where( po => po.Id == e.RowKeyId ).SingleOrDefault( );

            NavigateToDetailPage( promoOccur.ContentChannelItemId, promoOccur.ContentChannelItem.ContentChannelId );
        }

        /// <summary>
        /// Handles the GridRebind event of the gPromotions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void PromotionOccurrencesGrid_Rebind( object sender, EventArgs e )
        {
            BindPromotionOccurrencesGrid();
        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindPromotionOccurrencesGrid()
        {
            // A Promotion Occurrence is literally just a content channel item. Using the "SelectedPromotionId",
            // enumerate the content items associated.

            // get the event item occurrence service
            RockContext rockContext = new RockContext();
            EventItemOccurrenceService eventService = new EventItemOccurrenceService( rockContext );

            // start by getting every promotion occurrence
            PromotionsContext promoContext = new PromotionsContext();
            PromotionsService<PromotionOccurrence> promoService = new PromotionsService<PromotionOccurrence>( promoContext );

            // Temp workaround until we transition systems. Content Channel Items SHOULD NOT be deleted,
            // but if they are, this will protect it.
            var promoOccurrences = promoService.Queryable( ).ToList( ).Where( po => po.ContentChannelItem != null ).ToList( );
            
            // ---- Apply Filters ----

            // Since Date and Content Channel Type require the ContentChannelItem, we'll work "backwards". First,
            // we'll eliminate any promotion occurrences that are outside the filter. 
            
            // Then, we'll create a unique list of the remaining events. This effectively filters the events.
            promoOccurrences = promoOccurrences.Where( po => 
                    
            // If there's only a start date, then it needs to be the target date selected.
                (po.ContentChannelItem.ExpireDateTime.HasValue == false && 
                po.ContentChannelItem.StartDateTime == dpTargetPromoDate.SelectedDate.Value) ||

                // otherwise, if there's an expire date, then the target needs to be inbetween start & end
                (po.ContentChannelItem.ExpireDateTime.HasValue == true &&
                po.ContentChannelItem.StartDateTime <= dpTargetPromoDate.SelectedDate.Value &&
                po.ContentChannelItem.ExpireDateTime >= dpTargetPromoDate.SelectedDate.Value)
                      
            ).ToList( );


            // Here, want to only show Content Items whose campus matches our selection.

            // First, look for Content Items with a Single Campus that DOES NOT match, OR IS BLANK. 
            // ( this would mean their campus doesn't match what is being filtered, so it shouldn't display)
            // IF THEY ARE MULTI-CAMPUS, they won't return in this query, which is a GOOD thing.
            Guid selectedCampusGuid = CampusCache.Read( ddlCampus.SelectedValue.AsInteger( ) ).Guid;
            var campusAttribValList = new AttributeValueService( rockContext ).Queryable( ).Where( av => av.Attribute.FieldType.Guid == new Guid( Rock.SystemGuid.FieldType.CAMPUS ) ).ToList( );

            var excludedPromotionOccurrenceIds = promoOccurrences.Join( campusAttribValList, 
                                                                      po => po.ContentChannelItem.Id, ca=> ca.EntityId, ( po, ca ) => new { Promotion = po, Campus = ca }  
                                                                      ).Where( po => string.IsNullOrWhiteSpace( po.Campus.Value ) == true || selectedCampusGuid != Guid.Parse( po.Campus.Value ) ).Select( po => po.Promotion.Id );

            promoOccurrences = promoOccurrences.Where( po => excludedPromotionOccurrenceIds.Contains( po.Id ) == false ).ToList( );


            // build MULTI-campus content items that should be excluded
            // For each item, we look at it's list of campus guids, and do a search for the selected campus guid IN it. If NOT found, we add it to our exclusion list.
            // NOTE: We treat "all unchecked" as "all campuses". So if the campus list is blank, we'll never exclude it.

            // Like above, this works because items that are not multi-campus simply won't be returned in the query.
            
            var campusesAttribValList = new AttributeValueService( rockContext ).Queryable( ).Where( av => av.Attribute.FieldType.Guid == new Guid( Rock.SystemGuid.FieldType.CAMPUSES ) ).ToList( );
            var excludedCampusesPromotionRequestIds = promoOccurrences.Join( campusesAttribValList, 
                                                                             
                po => po.ContentChannelItem.Id, ca=> ca.EntityId, ( po, ca ) => new { Promotion = po, Campuses = ca } )
                
                // Exclude the item IF it has at least one campus checked, and that campus is NOT what's selected.
                .Where( po => (string.IsNullOrWhiteSpace( po.Campuses.Value ) == false && po.Campuses.Value.Contains( selectedCampusGuid.ToString( ) ) == false) )
                
                // Take just the IDs
                .Select( po => po.Promotion.Id );

            promoOccurrences = promoOccurrences.Where( po => excludedCampusesPromotionRequestIds.Contains( po.Id ) == false ).ToList( );

            
            

            // Content Channel Type (Promo Type)
            ListItem promoItem = ddlPromoType.Items[ ddlPromoType.SelectedIndex ];
            if( string.IsNullOrEmpty( promoItem.Value ) == false )
            {
                int filteredChannelId = promoItem.Value.AsInteger( );
                promoOccurrences = promoOccurrences.Where( po => po.ContentChannelItem.ContentChannelId == filteredChannelId ).ToList( );
            }

            // in the promo table, we just filtered out all promos with a date / content channel type we don't care about.
            // That will implicitely filter out any event that only has content items out of that range.
            
            // Done Filtering
            gPromotionOccurrencesGrid.DataSource = promoOccurrences.Select( i => new
            {
                Id = i.Id,
                Guid = i.Guid,
                Title = i.ContentChannelItem.Title,
                Campus = GetCampus( i.ContentChannelItem ),
                PromoDate = GetPromoDate( i.ContentChannelItem ),
                EventDate = GetDate( eventService, i ),
                Priority = i.ContentChannelItem.Priority,
                PromoType = string.Format( "<span class='{0}'></span> {1}", i.ContentChannelItem.ContentChannel.IconCssClass, i.ContentChannelItem.ContentChannel.Name )
            }).ToList( );

            gPromotionOccurrencesGrid.DataBind( );
        }

        string GetDate( EventItemOccurrenceService eventService, PromotionOccurrence promoOccurr )
        {
            if( promoOccurr.PromotionRequest != null )
            {
                var eventItem = eventService.Get( promoOccurr.PromotionRequest.EventItemOccurrenceId );

                if( eventItem.NextStartDateTime.HasValue )
                {
                    return eventItem.NextStartDateTime.Value.ToShortDateString();
                }
                else
                {
                    return "N/A";
                }
            }
            else
            {
                return "None";
            }
        }

        string GetCampus( ContentChannelItem channelItem )
        {
            channelItem.LoadAttributes( );

            // is it a single select campus channel?
            var singleCampusAttrib = channelItem.Attributes.Where( a => a.Value.FieldType.Guid == new Guid( Rock.SystemGuid.FieldType.CAMPUS ) ).Select( a => a.Value ).FirstOrDefault( );
            if( singleCampusAttrib != null )
            {
                return CampusCache.Read( new Guid( channelItem.AttributeValues[ singleCampusAttrib.Key ].Value ) ).Name;
            }
            // then it should be a multi-campus channel
            else
            {
                var multiCampusAttrib = channelItem.Attributes.Where( a => a.Value.FieldType.Guid == new Guid( Rock.SystemGuid.FieldType.CAMPUSES ) ).Select( a => a.Value ).FirstOrDefault( );
                
                // break the guids into a list
                List<string> campusGuids = channelItem.AttributeValues[ multiCampusAttrib.Key ].Value.Split( new char[] {  ',' } ).ToList( );

                // first, if this is for all campuses, just say all campuses.
                if( campusGuids.Count == CampusCache.All( ).Count )
                {
                    return "All Campuses";
                }
                else
                {
                    // concatenate the campus names
                    string campusNames = string.Empty;
                    foreach( string campusGuid in campusGuids )
                    {
                        campusNames += CampusCache.Read( new Guid( campusGuid ) ).Name + ", ";
                    }

                    // remove the trailing ,
                    campusNames = campusNames.Substring( 0, campusNames.Length - 2 );

                    return campusNames;
                }
            }
        }

        string GetPromoDate( ContentChannelItem channelItem )
        {
            if( channelItem.ExpireDateTime.HasValue == false )
            {
                return channelItem.StartDateTime.ToShortDateString( );
            }
            else
            {
                return channelItem.StartDateTime.ToShortDateString( ) + " - " + channelItem.ExpireDateTime.Value.ToShortDateString( );
            }
        }
        #endregion

        private void NavigateToDetailPage( int contentItemId, int? contentChannelId = null )
        {
            var qryParams = new Dictionary<string, string>();
            qryParams.Add( "EventCalendarId", PageParameter( "EventCalendarId" ) );
            qryParams.Add( "EventItemId", PageParameter( "EventItemId" ) );
            qryParams.Add( "EventItemOccurrenceId", PageParameter( "EventItemOccurrenceId" ) );
            qryParams.Add( "ContentItemId", contentItemId.ToString() );
            if ( contentChannelId.HasValue )
            {
                qryParams.Add( "ContentChannelId", contentChannelId.Value.ToString() );
            }
            
            NavigateToLinkedPage( "DetailPage", qryParams );
        }
    }
}
