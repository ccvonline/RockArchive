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
using System.Web.UI.HtmlControls;

namespace RockWeb.Plugins.church_ccv.Promotions
{
    [DisplayName( "Promotion Request List" )]
    [Category( "CCV > Promotions" )]
    [Description( "Lists requested promotions for events." )]
    [TextField("Content Channel Item Campus Key", "The key to use when reading / writing the Campus to a Content Channel Item that uses a single campus. (This should be the same across all content channel items.)", true)]
    [TextField("Content Channel Item Multi Campus Key", "The key to use when reading / writing the Campus to a Content Channel Item that uses multiple campuses. (This should be the same across all content channel items.)", true)]
    public partial class PromotionRequestList : RockBlock
    {
        public object RockTransactionScope { get; private set; }

        string ContentChannelItemCampusKey { get; set; }
        string ContentChannelItemMultiCampusKey { get; set; }

        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            PopulatePromoTypesControl( );

            PopulateCampusSelectorControls( );
            
            ContentChannelItemCampusKey = GetAttributeValue( "ContentChannelItemCampusKey" );
            ContentChannelItemMultiCampusKey = GetAttributeValue( "ContentChannelItemMultiCampusKey" );

            foreach ( var campus in CampusCache.All() )
            {
                ddlCampus.Items.Add( new ListItem( campus.Name, campus.Id.ToString().ToUpper() ) );
            }
            ddlCampus.SelectedIndex = 0;
            
            rFilter.ApplyFilterClick += rFilter_ApplyFilterClick;
            rFilter.DisplayFilterValue += rFilter_DisplayFilterValue;
            
            gPromotions.DataKeyNames = new string[] { "Id" };

            gPromotions.Actions.Visible = false;
            gPromotions.GridRebind += gPromotions_GridRebind;

            // Block Security and special attributes (RockPage takes care of View)
            bool canAddEditDelete = IsUserAuthorized( Authorization.EDIT );
            gPromotions.IsDeleteEnabled = true;

            mdCampusSelect.Title = "Multi-Campus Event";
            mdCampusSelect.SaveButtonText = "Approve";
            mdCampusSelect.SaveClick += mdCampusSelect_SaveClick;
            mdCampusSelect.CancelLinkVisible = false;
        }

        void PopulateCampusSelectorControls( )
        {
            foreach( CampusCache campus in CampusCache.All( ) )
            {
                var campusCheckBox = new RockCheckBox( );
                campusCheckBox.Text = campus.Name;
                campusCheckBox.ID = campus.Guid.ToString( );
                campusCheckBox.SelectedIconCssClass = "fa fa-check-square-o fa-lg";
                campusCheckBox.UnSelectedIconCssClass = "fa fa-square-o fa-lg";

                phCampuses.Controls.Add( campusCheckBox );
                
                var spacerDiv = new HtmlGenericControl( "div" );
                spacerDiv.AddCssClass( "margin-v-sm" );
                phCampuses.Controls.Add( spacerDiv );
            }
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
                BindFilter();

                RestoreDateControls( );
                
                BindGrid();
            }
        }

        void PopulatePromoTypesControl( )
        {
            // Filter Promos
            ddlPromoType.Items.Clear( );

            // first, get all of the promotions we're currently managing
            PromotionsService<PromotionRequest> promoService = new PromotionsService<PromotionRequest>( new PromotionsContext( ) );
            var eventOccurrenceIds = promoService.Queryable( ).Select( p => p.EventItemOccurrenceId ).ToList( );

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

            drpFutureWeeks.DelimitedValues = GetUserPreference( "FutureWeeksDateRange" );

            ddlPromoType.SetValue( GetUserPreference( "PromotionType" ) );
        }
        
        void SaveDateControls( )
        {
            SetUserPreference( "PromotionType", ddlPromoType.Items[ ddlPromoType.SelectedIndex ].Value );

            SetUserPreference( "TargetPromoDate", dpTargetPromoDate.SelectedDate.ToString( ) );

            SetUserPreference( "FutureWeeksDateRange", drpFutureWeeks.DelimitedValues );

            SetUserPreference( "Campus", ddlCampus.SelectedValue );
        }

        #endregion

        #region Filter Events
        private void BindFilter()
        {
            tbTitle.Text = rFilter.GetUserPreference( "Title" );
        }

        void rFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            rFilter.SaveUserPreference( "Title", tbTitle.Text );
            
            BindGrid();
        }

        /// <summary>
        /// Rs the filter_ display filter value.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void rFilter_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
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
        #endregion

        #region Button Clicks
        protected void ApplyDates( object sender, EventArgs e )
        {
            SaveDateControls( );

            BindGrid( );
        }

        protected void gPromotions_Approve( object sender, RowEventArgs e )
        {
            // this is more like, "publish for this week"
            PromotionsContext promoContext = new PromotionsContext( );
            PromotionsService<PromotionRequest> promoService = new PromotionsService<PromotionRequest>( promoContext );

            // grab the item that was clicked
            PromotionRequest promoRequest = promoService.Get( e.RowKeyId );

            // get the event item linked to the request
            RockContext rockContext = new RockContext( );
            EventItemOccurrenceService eventService = new EventItemOccurrenceService( rockContext );
            var eventItem = eventService.Get( promoRequest.EventItemOccurrenceId );
            
            // figure out if this promo should be using single "Campus" or multiple "Campuses"
            var campusObj = new CampusService( rockContext ).Get( ddlCampus.SelectedValue.AsInteger( ) );
            
            bool multiCampus = PromotionsUtil.IsContentChannelMultiCampus( promoRequest.ContentChannel.Id, ContentChannelItemMultiCampusKey );

            // the campus attribute type (multi or single), along with the event's campus,
            // will determine how we setup the data
            
            // if it's multi-campus, and the event is an all-campus event
            if ( multiCampus && eventItem.Campus == null )
            {
                // throw up the dialog so they can decide.
                HandleMultiCampusPromotion( promoRequest );
            }
            else
            {
                // otherwise, it's either a single-campus event and a multi-campus promo type,
                // or it's a single campus promo type.
                string campusKey = string.Empty;
                string campusGuids = campusObj.Guid.ToString( );

                // the only difference will be the key
                if( multiCampus )
                {
                    campusKey = ContentChannelItemMultiCampusKey;
                }
                else
                {
                    campusKey = ContentChannelItemCampusKey;
                }
                
                PromotionsUtil.CreatePromotionOccurrence( promoRequest.ContentChannel.Id, 
                                                          promoRequest.ContentChannel.ContentChannelTypeId, 
                                                          dpTargetPromoDate.SelectedDate.HasValue ? dpTargetPromoDate.SelectedDate.Value : DateTime.Now, 
                                                          CurrentPersonAliasId, 
                                                          eventItem.EventItem.Name,
                                                          BuildPromoContent( eventItem ),
                                                          campusKey,
                                                          campusGuids,
                                                          promoRequest.Id );


                BindGrid();
            }
        }
        
        protected void HandleMultiCampusPromotion( PromotionRequest promoRequest )
        {
            PromotionsContext promoContext = new PromotionsContext( );

            // grab all promo occurrences tied to this promotion request, and pull them into memory
            PromotionsService<PromotionOccurrence> promoOccurrenceService = new PromotionsService<PromotionOccurrence>( promoContext );
            var promoOccurrenceList = promoOccurrenceService.Queryable( ).Where( po => po.PromotionRequestId == promoRequest.Id ).ToList( );
            
            // we need to see, for the selected date, what campuses this promotion is running on
            promoOccurrenceList = promoOccurrenceList
                .Where( po =>
                        // If there's only a start date, then it needs to be the target date selected.
                        (po.ContentChannelItem.ExpireDateTime.HasValue == false && 
                        po.ContentChannelItem.StartDateTime == dpTargetPromoDate.SelectedDate.Value) ||

                        // otherwise, if there's an expire date, then the target needs to be inbetween start & end
                        (po.ContentChannelItem.ExpireDateTime.HasValue == true &&
                         po.ContentChannelItem.StartDateTime <= dpTargetPromoDate.SelectedDate.Value &&
                         po.ContentChannelItem.ExpireDateTime >= dpTargetPromoDate.SelectedDate.Value) &&
                    
                         po.ContentChannelItem.ContentChannelId == promoRequest.ContentChannelId )
                .ToList( );
                
            // now, for each promotion occurrence, join with its "CampusesKey" attribute value
            var campusesAttribValList = new AttributeValueService( new RockContext( ) ).Queryable( ).Where( av => av.Attribute.Key == ContentChannelItemMultiCampusKey ).ToList( );

            var campusGuids = promoOccurrenceList.Join( campusesAttribValList, 

                po => po.ContentChannelItem.Id, ca=> ca.EntityId, ( pr, ca ) => new { Promotion = pr, Campuses = ca } )
                    
                // Take just the string list of campus guids
                .Select( pr => pr.Campuses.Value ).ToList( );

            // now we JUST have a list of campus guids

            // go thru each check box
            foreach( RockCheckBox checkBox in mdCampusSelect.ControlsOfTypeRecursive<RockCheckBox>( ) )
            {
                // default it to enabled and checkable
                checkBox.Checked = false;
                checkBox.Enabled = true;
                checkBox.SelectedIconCssClass = "fa fa-check-square-o fa-lg";

                // if its ID (which is a campus guid) shows up in any of our returned results, we know
                // there's some type of occurrence for this campus
                foreach( string campusGuidList in campusGuids )
                {
                    if ( campusGuidList.Contains( checkBox.ID ) )
                    {
                        // so check its box and disable it.
                        checkBox.Checked = true;
                        checkBox.Enabled = false;
                        checkBox.SelectedIconCssClass = "fa fa-square";
                        break;
                    }
                }
            }
            
            hfPromoRequestId.Value = promoRequest.Id.ToString( );

            // set the title for the modal using the event item
            EventItemOccurrenceService eventService = new EventItemOccurrenceService( new RockContext( ) );
            var eventItem = eventService.Get( promoRequest.EventItemOccurrenceId );

            string campusString = eventItem.Campus != null ? eventItem.Campus.Name : "All Campuses";
            
            lbCampusSelectEventInfo.Text = "Event Details: " + eventItem.EventItem.Name + ", " + campusString + ", " + eventItem.NextStartDateTime.Value.ToShortDateString( );
            mdCampusSelect.Show( );
        }
                
        /*protected void OnBtnSeperateClicked( object sender, EventArgs e )
        {
            PromotionsContext promoContext = new PromotionsContext( );
            PromotionsService<PromotionRequest> promoService = new PromotionsService<PromotionRequest>( promoContext );

            // grab the item that was clicked
            PromotionRequest promoRequest = promoService.Get( hfPromoRequestId.Value.AsInteger( ) );

            // get the event item linked to the request
            RockContext rockContext = new RockContext( );
            EventItemOccurrenceService eventService = new EventItemOccurrenceService( rockContext );
            var eventItem = eventService.Get( promoRequest.EventItemOccurrenceId );
            
            // create a new promotion occurrence for each item clicked
            foreach( RockCheckBox checkBox in mdCampusSelect.ControlsOfTypeRecursive<RockCheckBox>( ) )
            {
                // if its enabled AND checked, they clicked it.
                if( checkBox.Enabled == true && checkBox.Checked == true )
                {
                    PromotionsUtil.CreatePromotionOccurrence( promoRequest.ContentChannel.Id, 
                                                          promoRequest.ContentChannel.ContentChannelTypeId, 
                                                          dpTargetPromoDate.SelectedDate.HasValue ? dpTargetPromoDate.SelectedDate.Value : DateTime.Now, 
                                                          CurrentPersonAliasId, 
                                                          eventItem.EventItem.Name,
                                                          BuildPromoContent( eventItem ),
                                                          ContentChannelItemMultiCampusKey,
                                                          checkBox.ID,
                                                          promoRequest.Id );
                }
            }

            mdCampusSelect.Hide( );
            BindGrid( );
        }*/

        protected void mdCampusSelect_SaveClick( object sender, EventArgs e )
        {
            PromotionsContext promoContext = new PromotionsContext( );
            PromotionsService<PromotionRequest> promoService = new PromotionsService<PromotionRequest>( promoContext );

            // grab the item that was clicked
            PromotionRequest promoRequest = promoService.Get( hfPromoRequestId.Value.AsInteger( ) );

            // get the event item linked to the request
            RockContext rockContext = new RockContext( );
            EventItemOccurrenceService eventService = new EventItemOccurrenceService( rockContext );
            var eventItem = eventService.Get( promoRequest.EventItemOccurrenceId );

            string campusGuids = string.Empty;
            foreach( RockCheckBox checkBox in mdCampusSelect.ControlsOfTypeRecursive<RockCheckBox>( ) )
            {
                // if its enabled AND checked, they clicked it.
                if( checkBox.Enabled == true && checkBox.Checked == true )
                {
                    campusGuids += checkBox.ID + ",";
                }
            }

            // remove the trailing comma
            campusGuids = campusGuids.Substring( 0, campusGuids.Length - 1 );

            // create the combined promo occurrence
            PromotionsUtil.CreatePromotionOccurrence( promoRequest.ContentChannel.Id, 
                                                      promoRequest.ContentChannel.ContentChannelTypeId, 
                                                      dpTargetPromoDate.SelectedDate.HasValue ? dpTargetPromoDate.SelectedDate.Value : DateTime.Now, 
                                                      CurrentPersonAliasId, 
                                                      eventItem.EventItem.Name,
                                                      BuildPromoContent( eventItem ),
                                                      ContentChannelItemMultiCampusKey,
                                                      campusGuids,
                                                      promoRequest.Id );

            mdCampusSelect.Hide( );
            BindGrid( );

        }

        string BuildPromoContent( EventItemOccurrence eventItem )
        {
            return "<p>" + eventItem.EventItem.Summary + "</p>" +
                    "<p>" + "Contact Person: " + (eventItem.ContactPersonAlias != null ? eventItem.ContactPersonAlias.Person.FullName : string.Empty) + "</p>" +
                    "<p>" + "Contact Email: " + eventItem.ContactEmail + "</p>" +
                    "<p>" + "Contact Phone: " + eventItem.ContactPhone + "</p>" +
                    "<p>" + "Location: " + eventItem.Location + "</p>" +
                    "<p>" + "Event Date: " + eventItem.NextStartDateTime + "</p>";
        }
        #endregion

        #region Grid Events (main grid)

        /// <summary>
        /// Handles the GridRebind event of the gPromotions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void gPromotions_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            if( dpTargetPromoDate.SelectedDate.HasValue && drpFutureWeeks.LowerValue.HasValue && drpFutureWeeks.UpperValue.HasValue )
            {
                // get the event item occurrence service
                RockContext rockContext = new RockContext();
                EventItemOccurrenceService eventService = new EventItemOccurrenceService( rockContext );

                // get the all promotion requests
                PromotionsContext promoContext = new PromotionsContext();
                PromotionsService<PromotionRequest> promoService = new PromotionsService<PromotionRequest>( promoContext );
                var promoRequestQuery = promoService.Queryable().Where( pr => pr.IsActive == true );

                // Now, we need to REMOVE any request where:
                // For the selected working date
                // For the selected campus
                // For the given content channel type
                // If there's a Promotion Occurrence that matches these 3 things, hide it.
                PromotionsService<PromotionOccurrence> promoOccurrenceService = new PromotionsService<PromotionOccurrence>( promoContext );
                var promoOccurrenceQuery = promoOccurrenceService.Queryable( );

                // get the guid of the selected campus
                Guid selectedCampusGuid = CampusCache.Read( ddlCampus.SelectedValue.AsInteger( ) ).Guid;

                // Example:
                // The selected working date is 2/1
                // The selected campus is Peoria
                //
                // There are 3 Requests
                //
                // Event: 2/26 Baptism, Campus: Peoria, Type: Program
                // Event: 2/26 Baptism, Campus: Peoria, Type: Talking Point
                // Event: 2/26 Baptism, Campus: All Campuses, Type: Mobile App
                //
                // If there is a Promotion Occurrence for: 
                // Event: 2/26 Baptism, Type: Program, Promotion Date: 2/1, Campus: Peoria
                //
                // Then that first request will NOT SHOW UP.
                // If the working date were changed to something other than 2/1, it WOULD show up. Make sense? Hope so.
                //
                // The Campus is important for events maked "All Campuses".
                // When a Promotion Request is approved, it looks at the Promotion Type (Program, Talking Point, Mobile App.) If the promotion type
                // supports multiple campuses, a dialog appears, asking the user to select which campuses to create the promotion on. (And whether to make one that's combined, or individual)
                //
                // When deciding whether to show a Promotion Request, for Types that are "Multi Campus", we see if one of the campuses is the campus selected, and if so, hide it.
                // So, in the above example:
                // 
                // Event: 2/26 Baptism, Campus: All Campuses, Type: Mobile App
                //
                // If there's a Promotion Ocurrence for:
                // Event: 2/26 Baptism, Type: Mobile App, Promotion Date: 2/1, Campus: Peoria/Surprise/Avondale
                // Then this event WOULD NOT show up.
                // If you changed the campus selection to East Valley (not one of the checked campuses in the above example occurrence), it WOULD show up.

                
                /////////////////////////
                // now lets actually do the query.
                // First, join the Request and Occurrence tables on the Request's ID
                var excludedPromotionRequestsQuery = promoRequestQuery.Join( promoOccurrenceQuery, pr => pr.Id, po => po.PromotionRequestId, ( pr, po ) => new { PromotionRequest = pr, PromotionOccurrence = po } )

                    // Where a promotion occurrence's start date matches the working date on the page
                    // and the promotion occurrences content channel type matches the promotion requests' content channel type
                    .Where( prpo => 
                    
                        // If there's only a start date, then it needs to be the target date selected.
                        (prpo.PromotionOccurrence.ContentChannelItem.ExpireDateTime.HasValue == false && 
                        prpo.PromotionOccurrence.ContentChannelItem.StartDateTime == dpTargetPromoDate.SelectedDate.Value) ||

                        // otherwise, if there's an expire date, then the target needs to be inbetween start & end
                        (prpo.PromotionOccurrence.ContentChannelItem.ExpireDateTime.HasValue == true &&
                        prpo.PromotionOccurrence.ContentChannelItem.StartDateTime <= dpTargetPromoDate.SelectedDate.Value &&
                        prpo.PromotionOccurrence.ContentChannelItem.ExpireDateTime >= dpTargetPromoDate.SelectedDate.Value) &&
                    
                        prpo.PromotionOccurrence.ContentChannelItem.ContentChannelId == prpo.PromotionRequest.ContentChannelId ).ToList( );
                
                // build single campus content items that should be excluded
                var campusAttribValList = new AttributeValueService( rockContext ).Queryable( ).Where( av => av.Attribute.Key == ContentChannelItemCampusKey ).ToList( );
                var excludedCampusPromotionRequestIds = excludedPromotionRequestsQuery.Join( campusAttribValList, 
                    
                    pr => pr.PromotionOccurrence.ContentChannelItem.Id, ca=> ca.EntityId, ( pr, ca ) => new { Promotion = pr, Campus = ca } )
                    
                    .Where( pr => string.IsNullOrWhiteSpace( pr.Campus.Value ) == false && selectedCampusGuid == Guid.Parse( pr.Campus.Value ) )
                    
                    .Select( pr => pr.Promotion.PromotionRequest.Id );

                promoRequestQuery = promoRequestQuery.Where( pr => excludedCampusPromotionRequestIds.Contains( pr.Id ) == false );


                // build MULTI-campus content items that should be excluded
                var campusesAttribValList = new AttributeValueService( rockContext ).Queryable( ).Where( av => av.Attribute.Key == ContentChannelItemMultiCampusKey ).ToList( );
                var excludedCampusesPromotionRequestIds = excludedPromotionRequestsQuery.Join( campusesAttribValList, 

                    pr => pr.PromotionOccurrence.ContentChannelItem.Id, ca=> ca.EntityId, ( pr, ca ) => new { Promotion = pr, Campuses = ca } )
                    
                    // Exclude the item IF it has a blank "campuses" value (since that means ALL campuses), OR if it has a checked campus that matches our selected one.
                    .Where( pr => string.IsNullOrWhiteSpace( pr.Campuses.Value ) == true || pr.Campuses.Value.Contains( selectedCampusGuid.ToString( ) ) )
                    
                    // Take just the IDs
                    .Select( pr => pr.Promotion.PromotionRequest.Id );
                
                // if it's not in either campus exclusion list, it may stay.
                promoRequestQuery = promoRequestQuery.Where( pr => excludedCampusesPromotionRequestIds.Contains( pr.Id ) == false );
                /////////////////////

                // Apply Working Campus
                int selectedCampusId = ddlCampus.SelectedValue.AsInteger( );
                promoRequestQuery = promoRequestQuery.Where( a => a.EventItemOccurrence.CampusId == null || (a.EventItemOccurrence.CampusId.HasValue && selectedCampusId == a.EventItemOccurrence.CampusId.Value ) );


                // Apply Future Weeks Date
                
                // convert to a list in memory so we can filter the dates
                var promoItems = promoRequestQuery.ToList( );

                // target range (remove any events that are before our target promotion date) (we do this in case the "Lower Range" is before the target weekend. That would be a mistake but we want to help them.)
                promoItems = promoItems.Where( pr => eventService.Get( pr.EventItemOccurrenceId ).NextStartDateTime.Value >= dpTargetPromoDate.SelectedDate.Value ).ToList( );

                // lower range
                promoItems = promoItems.Where( pr => eventService.Get( pr.EventItemOccurrenceId ).NextStartDateTime.Value >= drpFutureWeeks.LowerValue.Value ).ToList( );

                // upper range
                promoItems = promoItems.Where( pr => eventService.Get( pr.EventItemOccurrenceId ).NextStartDateTime.Value <= drpFutureWeeks.UpperValue.Value ).ToList( );

                // put it back to a query
                promoRequestQuery = promoItems.AsQueryable( );


                // ---- Apply Filters ----
                // Title
                if ( string.IsNullOrWhiteSpace( tbTitle.Text ) == false )
                {
                    promoRequestQuery = promoRequestQuery.Where( pr => pr.EventItemOccurrence.EventItem.Name.ToLower( ).Contains( tbTitle.Text.ToLower( ) ) );
                }

                // Content Channel Type (Promo Type)
                ListItem promoItem = ddlPromoType.Items[ ddlPromoType.SelectedIndex ];
                if( string.IsNullOrEmpty( promoItem.Value ) == false )
                {
                    int filteredChannelId = promoItem.Value.AsInteger( );
                    promoRequestQuery = promoRequestQuery.Where( pr => pr.ContentChannelId == filteredChannelId );
                }
                // -----
            
                // Sort
                /*SortProperty sortProperty = gPromotions.SortProperty;
                if ( sortProperty != null )
                {
                    promoRequestQuery = promoRequestQuery.Sort( sortProperty );
                }
                else
                {
                    promoRequestQuery = promoRequestQuery.OrderBy( pr => pr.EventItemOccurrence.EventItem.Name );
                }*/

                // Done Filtering
                var promoRequestList = promoRequestQuery.ToList( );
            
                gPromotions.DataSource = promoRequestList.Select( i => new
                {
                    Id = i.Id,
                    Guid = i.Guid,
                    Title = i.EventItemOccurrence.EventItem.Name,
                    Campus = i.EventItemOccurrence.Campus != null ? i.EventItemOccurrence.Campus.Name : "All Campuses",
                    EventDate = GetDate(eventService, i ),
                    PromoType = string.Format( "<span class='{0}'></span> {1}", i.ContentChannel.IconCssClass, i.ContentChannel.Name )
                }).OrderBy( a => a.EventDate ).ToList( );
            }

            gPromotions.DataBind();
        }

        string GetDate( EventItemOccurrenceService eventService, PromotionRequest promoRequest )
        {
            var eventItem = eventService.Get( promoRequest.EventItemOccurrenceId );

            return eventItem.NextStartDateTime.Value.ToShortDateString( );
        }
        
        #endregion
    }
}