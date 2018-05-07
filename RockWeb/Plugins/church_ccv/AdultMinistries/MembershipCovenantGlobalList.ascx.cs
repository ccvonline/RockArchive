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
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace RockWeb.Plugins.church_ccv.AdultMinistries
{
    [DisplayName( "Membership Covenant Global List" )]
    [Category( "CCV > Adult Ministries" )]
    [Description( "Lists all covenant workflows in the Pending state." )]
    
    [LinkedPage( "Detail Page" )]
    [CampusesField( "Campuses", "List of which campuses to show volunteer screening instances for.", required: false, includeInactive: true )]
    public partial class MembershipCovenantGlobalList : RockBlock
    {
        #region Control Methods

        static int sWorkflowTypeId = 246;
        
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            
            InitFilter( );
            InitGrid( );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }
        
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );
            
            if ( !Page.IsPostBack )
            {
                BindFilter( );
                BindGrid( );
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
            BindFilter();
            BindGrid();
        }

        #endregion

        #region Filter Methods

        void InitFilter( )
        {
            rFilter.ApplyFilterClick += rFilter_ApplyFilterClick;
        }

        protected void rFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            rFilter.SaveUserPreference( "Campus", "Campus", cblCampus.SelectedValues.AsDelimited( ";" ) );
            rFilter.SaveUserPreference( "Applicant Name", tbApplicantName.Text );
            rFilter.SaveUserPreference( "Starting Point", drpDates.DelimitedValues );

            BindFilter( );
            BindGrid( );
        }

        protected void rFilter_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
        {
            switch ( e.Key )
            {
                case "Campus":
                {
                    var values = new List<string>();
                    foreach ( string value in e.Value.Split( ';' ) )
                    {
                        var item = cblCampus.Items.FindByValue( value );
                        if ( item != null )
                        {
                            values.Add( item.Text );
                        }
                    }
                    e.Value = values.AsDelimited( ", " );
                    break;
                }
                                    
                case "Applicant Name":
                {
                    e.Value = rFilter.GetUserPreference( "Applicant Name" );
                    break;
                }

                case "Starting Point":
                {
                    // first see if there's a lower value
                    if ( drpDates.LowerValue != null )
                    {
                        // if there's also an upper value, its a between range
                        if ( drpDates.UpperValue != null )
                        {
                            e.Value = string.Format( "{0:M/dd/yy} thru {1:M/dd/yy}", drpDates.LowerValue, drpDates.UpperValue );
                        }
                        else
                        {
                            // otherwise there's only a lower value, so it's anything on or after
                            e.Value = string.Format( "{0:M/dd/yy} or later", drpDates.LowerValue );
                        }
                    }
                    // is there only an upper value?
                    else if ( drpDates.UpperValue != null )
                    {
                        // than nothing later than that date
                        e.Value = "No later than " + string.Format( "{0:M/dd/yy}", drpDates.UpperValue );
                    }
                    else
                    {
                        e.Value = string.Empty;
                    }
                    break;
                }
                    
                default:
                {
                    e.Value = string.Empty;
                    break;
                }
            }
        }

        private void BindFilter()
        {
            // setup the campus
            // if Block Campus filter is applied, update User campus filter to only show respective campuses
            // if not applied, show all campuses
            if ( string.IsNullOrWhiteSpace( GetAttributeValue( "Campuses" ) ) == false )
            {
                List<Guid> selectedCampusesAttribute = Array.ConvertAll( GetAttributeValue( "Campuses" ).Split( ',' ), s => new Guid( s ) ).ToList();

                var selectedCampuses = CampusCache.All();

                selectedCampuses = selectedCampuses.Where( vs => selectedCampusesAttribute.Contains( vs.Guid ) ).ToList();

                cblCampus.DataSource = selectedCampuses;
                cblCampus.DataBind();

            }
            else
            {
                cblCampus.DataSource = CampusCache.All( false );
                cblCampus.DataBind();
            }


            string campusValue = rFilter.GetUserPreference( "Campus" );
            if ( !string.IsNullOrWhiteSpace( campusValue ) )
            {
                cblCampus.SetValues( campusValue.Split( ';' ).ToList() );
            }
            
            // setup the Applicant Name
            tbApplicantName.Text = rFilter.GetUserPreference( "Applicant Name" );

            // setup Starting Point
            drpDates.DelimitedValues = rFilter.GetUserPreference( "Starting Point" );
        }
        #endregion

        #region Grid Methods

        void InitGrid( )
        {
            gGrid.DataKeyNames = new string[] { "Id" };
            
            gGrid.Actions.Visible = true;
            gGrid.Actions.Enabled = true;
            gGrid.Actions.ShowBulkUpdate = false;
            gGrid.Actions.ShowCommunicate = false;
            gGrid.Actions.ShowExcelExport = false;
            gGrid.Actions.ShowMergePerson = false;
            gGrid.Actions.ShowMergeTemplate = false;

            gGrid.GridRebind += gGrid_Rebind;
        }

        private void gGrid_Rebind( object sender, EventArgs e )
        {
            BindFilter( );
            BindGrid( );
        }

        protected void gGrid_Edit( object sender, RowEventArgs e )
        {
            var qryParams = new Dictionary<string, string>();
            qryParams.Add( "WorkflowId", e.RowKeyId.ToString() );

            PageReference pageRef = new PageReference( new Uri( ResolveRockUrlIncludeRoot("~/WorkflowEntry/" + sWorkflowTypeId + "/" + e.RowKeyId.ToString() ) ), HttpContext.Current.Request.ApplicationPath );
            NavigateToPage( pageRef );
        }

        class WorkflowWithAttribs
        {
            public int WorkflowId;
            public string WorkflowName;
            public List<KeyValuePair<string, string>> AttribValues;
        }
        
        private void BindGrid( )
        {
            using ( RockContext rockContext = new RockContext( ) )
            {
                List<CampusCache> campusCache = CampusCache.All( );
                PersonAliasService paService = new PersonAliasService( rockContext );
                
                // First, get all the covenant workflows that are in pending status (Note that we only select the properties we actually need)
                var wfQuery = new Service<Workflow>( rockContext ).Queryable( ).AsNoTracking( );
                var pendingCovenantsQuery = wfQuery.Where( wf => wf.WorkflowTypeId == sWorkflowTypeId && wf.Status == "Pending" ).Select( wf => new { wf.Id, wf.Name } ).AsEnumerable( );
                
                
                // Now, we want to get all the attributes that are displayed in our list. First join the attributes with their values
                var attribQuery = new AttributeService( rockContext ).Queryable( ).AsNoTracking( );
                var avQuery = new AttributeValueService( rockContext ).Queryable( ).AsNoTracking( );
                var attribWithValue = attribQuery.Join( avQuery, a => a.Id, av => av.AttributeId, ( a, av ) => new { Attribute = a, AttribValue = av } )
                                                 .Where( a => a.Attribute.EntityTypeQualifierColumn.Equals( "WorkflowTypeId", StringComparison.OrdinalIgnoreCase ) );
                
                // now for each pending covenant, get the key / value for the attributes we need
                List<WorkflowWithAttribs> workflowList = new List<WorkflowWithAttribs>( );
                foreach( var workflow in pendingCovenantsQuery )
                {
                    WorkflowWithAttribs workflowWithAttribs = new WorkflowWithAttribs( );
                    workflowWithAttribs.WorkflowId = workflow.Id;
                    workflowWithAttribs.WorkflowName = workflow.Name;

                    workflowWithAttribs.AttribValues = attribWithValue.Where( av => av.AttribValue.EntityId == workflow.Id &&
                                                                                   (av.Attribute.Key == "Applicant" || 
                                                                                    av.Attribute.Key == "Campus" ||
                                                                                    av.Attribute.Key == "CompletedStartingPoint_Date") )
                                                                      .Select( a => new { a.Attribute.Key, a.AttribValue.Value } ).AsEnumerable( )
                                                                      .Select( o => new KeyValuePair<string, string>( o.Key, o.Value ) )
                                                                      .ToList( );

                    workflowList.Add( workflowWithAttribs );
                }
                
                var filteredList = workflowList;

                // First apply Campus Block Setting Filter
                if ( string.IsNullOrWhiteSpace(GetAttributeValue( "Campuses" )) == false )
                {
                    List<Guid> selectedCampuses = Array.ConvertAll( GetAttributeValue( "Campuses" ).Split( ',' ), s => new Guid( s ) ).ToList();
                    if ( selectedCampuses.Count > 0 )
                    {
                        filteredList = filteredList.Where( wf => selectedCampuses.Contains( wf.AttribValues.Where( av => av.Key == "Campus" ).FirstOrDefault( ).Value.ToStringSafe( ).AsGuid( ) ) ).ToList();
                    }
                }

                // Now apply user filters
                // Campus
                List<int> campusIds = cblCampus.SelectedValuesAsInt;
                if( campusIds.Count > 0 )
                {
                    // the workflows store the campus by guid, so convert the selected Ids to guids
                    List<Guid> selectedCampusNames = campusCache.Where( cc => campusIds.Contains( cc.Id ) ).Select( cc => cc.Guid ).ToList( );
                    filteredList = filteredList.Where( wf => wf.AttribValues.Where( av => av.Key == "Campus" ).FirstOrDefault().Value.IsNullOrWhiteSpace() || selectedCampusNames.Contains( wf.AttribValues.Where( av => av.Key == "Campus" ).FirstOrDefault( ).Value.ToStringSafe( ).AsGuid( ) ) ).ToList();
                }
                
                // Build Query so that the Applicant is populated for filtering / sorting
                var inMemoryQuery = filteredList.Select( wf =>
                                            new {
                                                Id = wf.WorkflowId,
                                                Campus = TryGetCampus( wf.AttribValues.Where( av => av.Key == "Campus" ).FirstOrDefault( ).Value.ToStringSafe( ).AsGuid( ), campusCache ),
                                                Applicant = TryGetApplicant( wf.AttribValues.Where( av => av.Key == "Applicant" ).FirstOrDefault( ).Value.ToStringSafe( ).AsGuid( ), paService ),
                                                StartingPoint = wf.AttribValues.Where( av => av.Key == "CompletedStartingPoint_Date" ).FirstOrDefault( ).Value.AsDateTime( )
                                            } ).ToList();


                // Filter Dates - First see if there's a lower value
                if ( drpDates.LowerValue != null )
                {
                    inMemoryQuery = inMemoryQuery.Where( wf => wf.StartingPoint.HasValue == false || (wf.StartingPoint.Value >= drpDates.LowerValue ) ).ToList( );
                }

                // If there's an upper value, clamp to that as well
                if ( drpDates.UpperValue != null )
                {
                    inMemoryQuery = inMemoryQuery.Where( wf => wf.StartingPoint.HasValue == false || (wf.StartingPoint.Value <= drpDates.UpperValue) ).ToList( );
                }
                
                // Now that we have the Applicant's name as text, we can filter it based on what the user typed in
                string applicantName = rFilter.GetUserPreference( "Applicant Name" );
                if( string.IsNullOrWhiteSpace( applicantName ) == false )
                {
                    inMemoryQuery = inMemoryQuery.Where( wf => wf.Applicant.ToLower( ).Contains( applicantName.ToLower( ).Trim( ) ) ).ToList( );
                }
                
                // ---- End Filters ----

                // Sort grid
                SortProperty sortProperty = gGrid.SortProperty;

                if ( sortProperty != null )
                {     
                    if ( sortProperty.Direction == System.Web.UI.WebControls.SortDirection.Ascending )
                    {
                        switch ( sortProperty.Property )
                        {
                            case "Applicant":
                                inMemoryQuery = inMemoryQuery.OrderBy( o => o.Applicant ).ToList();
                                break;
                            case "StartingPoint":
                                inMemoryQuery = inMemoryQuery.OrderBy( o => o.StartingPoint ).ToList();
                                break;
                            case "Campus":
                                inMemoryQuery= inMemoryQuery.OrderBy( o => o.Campus ).ToList();
                                break;
                        }
                    }
                    else
                    {
                        switch ( sortProperty.Property )
                        {
                            case "Applicant":
                                inMemoryQuery = inMemoryQuery.OrderByDescending( o => o.Applicant ).ToList();
                                break;
                            case "StartingPoint":
                                inMemoryQuery = inMemoryQuery.OrderByDescending( o => o.StartingPoint ).ToList();
                                break;
                            case "Campus":
                                inMemoryQuery = inMemoryQuery.OrderByDescending( o => o.Campus ).ToList();
                                break;
                        }
                    }                  
                }

                // Bind filter to grid
                if ( inMemoryQuery.Count( ) > 0 )
                {
                    gGrid.DataSource = inMemoryQuery;
                }

                gGrid.DataBind( );
            }
        }

        #endregion

        #region Helper Methods
        string TryGetCampus( Guid campusGuid, List<CampusCache> campusCache )
        {
            if ( campusGuid.IsEmpty( ) == false )
            {
                return campusCache.Where( c => c.Guid == campusGuid ).FirstOrDefault( ).Name;
            }

            return string.Empty;
        }

        string TryGetApplicant( Guid personGuid, PersonAliasService paService )
        {
            // make sure the applicant exists
            if ( personGuid.IsEmpty( ) == false )
            {
                // make sure the person exists
                Person person = paService.Get( personGuid ).Person;
                if ( person != null )
                {
                    return person.FullName;
                }
            }

            return string.Empty;
        }

        #endregion
    }
}
