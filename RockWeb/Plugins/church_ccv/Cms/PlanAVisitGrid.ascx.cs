﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using church.ccv.CCVCore.PlanAVisit.Model;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.church_ccv.Cms
{
    [DisplayName( "Plan A Visit Grid" )]
    [Category( "CCV > Cms" )]
    [Description( "Grid used to display / manage Plan A Visit" )]
    [CampusField( "Campus", "Limit results to campus specified", false, "", "", 0)]
    [SchedulesField( "Service Schedules", "Service Schedules available for use", true, "", "", 1 )]
    public partial class PlanAVisitGrid : RockBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            InitFilter();
            InitGrid();

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
                BindFilter();
                BindGrid();
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
            BindGrid();
        }

        #region Filter Events

        /// <summary>
        /// Handles the rFilter on display filter value event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rFilter_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
        {
            switch ( e.Key )
            {
                case "Campus":
                    {
                        // show campus names in filter
                        List<string> values = new List<string>();

                        foreach ( string value in e.Value.Split( ';' ) )
                        {
                            ListItem item = cblCampusFilter.Items.FindByValue( value );

                            if ( item.IsNotNull() )
                            {
                                values.Add( item.Text );
                            }
                        }

                        e.Value = values.AsDelimited( ", " );
                        break;
                    }
                case "Scheduled Service":
                    {
                        // show schedule name in filter
                        List<string> values = new List<string>();

                        foreach ( string value in e.Value.Split( ';' ) )
                        {
                            ListItem item = cblScheduledServiceFilter.Items.FindByValue( value );

                            if ( item.IsNotNull() )
                            {
                                values.Add( item.Text );
                            }
                        }

                        e.Value = values.AsDelimited( "," );
                        break;
                    }
                case "Scheduled Date":
                    {
                        e.Value = DateRangePicker.FormatDelimitedValues( e.Value );

                        break;
                    }
                case "Person's Name":
                    {
                        e.Value = rFilter.GetUserPreference( "Person's Name" );

                        break;
                    }
                case "Bringing Spouse":
                    {
                        e.Value = rFilter.GetUserPreference( "Bringing Spouse" );

                        break;
                    }
                case "Bringing Kids":
                    {
                        e.Value = rFilter.GetUserPreference( "Bringing Kids" );

                        break;
                    }
                case "Has Attended":
                    {
                        e.Value = rFilter.GetUserPreference( "Has Attended" );

                        break;
                    }
                case "Attended Service":
                    {
                        // show schedule name in filter
                        List<string> values = new List<string>();

                        foreach ( string value in e.Value.Split( ';' ) )
                        {
                            ListItem item = cblAttendedServiceFilter.Items.FindByValue( value );

                            if ( item.IsNotNull() )
                            {
                                values.Add( item.Text );
                            }
                        }

                        e.Value = values.AsDelimited( "," );
                        break;
                    }
                case "Attended Date":
                    {
                        e.Value = DateRangePicker.FormatDelimitedValues( e.Value );

                        break;
                    }
                default:
                    e.Value = null;
                    break;
            }
        }

        /// <summary>
        /// Handles the rFilter apply filter click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            rFilter.SaveUserPreference( "Campus", cblCampusFilter.SelectedValues.AsDelimited( ";" ) );
            rFilter.SaveUserPreference( "Scheduled Service", cblScheduledServiceFilter.SelectedValues.AsDelimited( ";" ) );
            rFilter.SaveUserPreference( "Scheduled Date", drpScheduledDateFilter.DelimitedValues );
            rFilter.SaveUserPreference( "Person's Name", tbPersonNameFilter.Text );
            rFilter.SaveUserPreference( "Bringing Spouse", ddlBringingAnotherAdultFilter.SelectedValue );
            rFilter.SaveUserPreference( "Bringing Kids", ddlBringingChildrenFilter.SelectedValue );
            rFilter.SaveUserPreference( "Has Attended", ddlHasAttendedFilter.SelectedValue );
            rFilter.SaveUserPreference( "Attended Service", cblAttendedServiceFilter.SelectedValues.AsDelimited( ";" ) );
            rFilter.SaveUserPreference( "Attended Date", drpAttendedDateFilter.DelimitedValues );

            BindFilter();
            BindGrid();
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// Handles the gGrid rowSelected event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gGrid_RowSelected( object sender, RowEventArgs e )
        {
            // load the visit and person of the row that was clicked
            RockContext rockContext = new RockContext();

            PlanAVisit visit = new Service<PlanAVisit>( rockContext ).Get( e.RowKeyId );

            Person adultOne = new PersonAliasService( rockContext ).Get( visit.AdultOnePersonAliasId ).Person;

            mdManageVisit.Title = string.Format( "When did {0} attend?", adultOne.FullName );
        
            // update date picker
            if ( visit.AttendedDate.HasValue )
            {
                // set date picker to attended date
                dpDateAttended.SelectedDate = visit.AttendedDate;
            }
            else
            {
                // reset date picker to no date
                dpDateAttended.SelectedDate = new DateTime( 0 );
            }

            // Bind the proper options for service time dropdown
            BindServicesTimesDropdown( visit.CampusId, dpDateAttended.SelectedDate );

            // if visit has an attended service, select it
            ddlServiceAttended.SelectedIndex = ddlServiceAttended.Items.IndexOf( ddlServiceAttended.Items.FindByValue( visit.AttendedServiceScheduleId.ToString() ) );

            // set hidden field with visit id for save event
            hfModalVisitId.Value = e.RowKeyId.ToString();

            mdManageVisit.Show();
        }
       
        /// <summary>
        /// Handles the mdManageVisit save click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void mdManageVisit_SaveClick( object sender, EventArgs e )
        {
            RockContext rockContext = new RockContext();

            PlanAVisit visit = new Service<PlanAVisit>( rockContext ).Get( int.Parse( hfModalVisitId.Value ) );

            if ( visit.IsNotNull() )
            {
                // update attended date if it doesnt match existing value
                if ( dpDateAttended.SelectedDate != visit.AttendedDate )
                {
                    visit.AttendedDate = dpDateAttended.SelectedDate;
                }

                // update service time if selected doesnt match existing value
                if ( ddlServiceAttended.SelectedValue != visit.AttendedServiceScheduleId.ToString() )
                {
                    visit.AttendedServiceScheduleId = ddlServiceAttended.SelectedValue.AsInteger() > 0 ? ddlServiceAttended.SelectedValue.AsInteger() : null as int?;
                }

                rockContext.SaveChanges();

                // reset date picker to empty
                dpDateAttended.SelectedDate = new DateTime( 0 );

                mdManageVisit.Hide();

                BindGrid();
            }
        }

        /// <summary>
        /// Handles the gGrid delete click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gGrid_Delete( object sender, RowEventArgs e )
        {
            // ensure current user has edit access before deleting
            if ( IsUserAuthorized( Authorization.EDIT ) )
            {
                RockContext rockContext = new RockContext();

                Service<PlanAVisit> pavService = new Service<PlanAVisit>( rockContext );

                PlanAVisit visit = pavService.Get( e.RowKeyId );

                pavService.Delete( visit );

                rockContext.SaveChanges();

                BindGrid();
            }                
        }

        /// <summary>
        /// Handle the gGrid rebind event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gGrid_Rebind( object sender, EventArgs e )
        {
            BindFilter();
            BindGrid();
        }

        /// <summary>
        /// Handles the dpDateAttneded text changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpDateAttended_TextChanged( object sender, EventArgs e )
        {
            // load the visit being updated
            PlanAVisit visit = new Service<PlanAVisit>( new RockContext() ).Get( int.Parse( hfModalVisitId.Value ) );

            // update dropdown to include service times for selected date
            BindServicesTimesDropdown( visit.CampusId, dpDateAttended.SelectedDate );
        }

        #endregion

        #endregion

        #region Methods

        #region Filter Methods

        /// <summary>
        /// Initialize the grid filter
        /// </summary>
        void InitFilter()
        {
            rFilter.ApplyFilterClick += rFilter_ApplyFilterClick;
        }

        /// <summary>
        /// Bind the grid filter
        /// </summary>
        private void BindFilter()
        {
            // bind campuses
            cblCampusFilter.DataSource = CampusCache.All( false );
            cblCampusFilter.DataBind();

            // bind service times
            BindServiceTimesCheckBoxList();

            //
            // load user preferences
            //

            string campusValue = rFilter.GetUserPreference( "Campus" );

            if ( campusValue.IsNotNullOrWhitespace() )
            {
                cblCampusFilter.SetValues( campusValue.Split( ';' ).ToList() );
            }

            string scheduledServiceValue = rFilter.GetUserPreference( "Scheduled Service" );

            if ( scheduledServiceValue.IsNotNullOrWhitespace() )
            {
                cblScheduledServiceFilter.SetValues( scheduledServiceValue.Split( ';' ).ToList() );
            }

            drpScheduledDateFilter.DelimitedValues = rFilter.GetUserPreference( "Scheduled Date" );

            tbPersonNameFilter.Text = rFilter.GetUserPreference( "Person's Name" );

            ddlBringingAnotherAdultFilter.SelectedValue = rFilter.GetUserPreference( "Bringing Spouse" );

            ddlBringingChildrenFilter.SelectedValue = rFilter.GetUserPreference( "Bringing Kids" );

            ddlHasAttendedFilter.SelectedValue = rFilter.GetUserPreference( "Has Attended" );

            string attendedServiceValue = rFilter.GetUserPreference( "Attended Service" );

            if ( attendedServiceValue.IsNotNullOrWhitespace() )
            {
                cblAttendedServiceFilter.SetValues( attendedServiceValue.Split( ';' ).ToList() );
            }

            drpAttendedDateFilter.DelimitedValues = rFilter.GetUserPreference( "Attended Date" );
        }

        /// <summary>
        /// Binds the service times checkbox list in the grid filter
        /// </summary>
        private void BindServiceTimesCheckBoxList()
        {
            // Reset checkbox lists
            cblScheduledServiceFilter.Items.Clear();
            cblAttendedServiceFilter.Items.Clear();

            // setup schedule service, schedule lookup list, and selected schedules
            ScheduleService scheduleService = new ScheduleService( new RockContext() );

            var scheduleLookupList = scheduleService.Queryable().Where( a => a.Name != null && a.Name != "" ).ToList().Select( a => new
            {
                a.Id,
                a.Name
            } );

            var selectedScheduleIds = scheduleService.GetByGuids( this.GetAttributeValue( "ServiceSchedules" ).SplitDelimitedValues().AsGuidList() ).Select( a => a.Id ).ToList();

            // loop through each selected schedule and add to checkbox lists
            foreach ( var selectedSchedule in selectedScheduleIds )
            {
                var scheduleLookup = scheduleLookupList.FirstOrDefault( a => a.Id == selectedSchedule );

                if ( scheduleLookup.IsNotNull() )
                {
                    // yes we need unique list items for ScheduledService and AttendedService
                    ListItem itemScheduled = new ListItem( scheduleLookup.Name, scheduleLookup.Id.ToString() );

                    cblScheduledServiceFilter.Items.Add( itemScheduled );

                    ListItem itemAttended = new ListItem( scheduleLookup.Name, scheduleLookup.Id.ToString() );

                    cblAttendedServiceFilter.Items.Add( itemAttended );
                }
            }
        }

        #endregion

        #region Grid Methods

        /// <summary>
        /// Initialize the grid
        /// </summary>
        void InitGrid()
        {
            gGrid.DataKeyNames = new string[] { "Id" };

            gGrid.Actions.Visible = true;
            gGrid.Actions.Enabled = true;
            gGrid.Actions.ShowBulkUpdate = false;
            gGrid.Actions.ShowCommunicate = true;
            gGrid.Actions.ShowExcelExport = true;
            gGrid.Actions.ShowMergePerson = false;
            gGrid.Actions.ShowMergeTemplate = true;

            gGrid.CommunicationRecipientPersonIdFields = new List<string> { "PersonId" };

            // show Delete column if current user has edit access
            if ( IsUserAuthorized( Authorization.EDIT ) )
            {
                gGrid.Columns[10].Visible = true;
            }

            gGrid.GridRebind += gGrid_Rebind;
        }

        /// <summary>
        /// Bind the grid
        /// </summary>
        private void BindGrid()
        {
            RockContext rockContext = new RockContext();

            // services for query
            var planAVisitTable = new Service<PlanAVisit>( rockContext ).Queryable().AsNoTracking();
            var campusTable = new CampusService( rockContext ).Queryable().AsNoTracking();
            var personAliasTable = new PersonAliasService( rockContext ).Queryable().AsNoTracking();
            var personTable = new PersonService( rockContext ).Queryable().AsNoTracking();
            var scheduleTable = new ScheduleService( rockContext ).Queryable().AsNoTracking();

            var pavQuery = 
                from planAVisit in planAVisitTable
                join campus in campusTable on planAVisit.CampusId equals campus.Id into campuses
                from campus in campuses.DefaultIfEmpty()
                join personAlias in personAliasTable on planAVisit.AdultOnePersonAliasId equals personAlias.Id into personAliases
                from personAlias in personAliases.DefaultIfEmpty()
                join person in personTable on personAlias.PersonId equals person.Id into people
                from person in people.DefaultIfEmpty()
                join scheduledSchedule in scheduleTable on planAVisit.ScheduledServiceScheduleId equals scheduledSchedule.Id into scheduleSchedules
                from scheduledSchedule in scheduleSchedules.DefaultIfEmpty()
                join attendedSchedule in scheduleTable on planAVisit.AttendedServiceScheduleId equals attendedSchedule.Id into attendedSchedules
                from attendedSchedule in attendedSchedules.DefaultIfEmpty()
                select new
                {
                    planAVisit.Id,
                    planAVisit.AdultOnePersonAliasId,
                    PersonId = person.Id,
                    person.FirstName,
                    person.NickName,
                    person.LastName,
                    planAVisit.CampusId,
                    CampusName = campus.Name,
                    CampusGuid = campus.Guid,
                    planAVisit.ScheduledDate,
                    planAVisit.ScheduledServiceScheduleId,
                    ScheduledServiceName = scheduledSchedule.Name,
                    BringingAnotherAdult = planAVisit.AdultTwoPersonAliasId.HasValue,
                    planAVisit.BringingChildren,
                    planAVisit.AttendedDate,
                    planAVisit.AttendedServiceScheduleId,
                    AttendedServiceName = attendedSchedule.Name                    
                };

            // limit results to campus if specified in block settings
            string avCampus = GetAttributeValue( "Campus" );

            if ( avCampus.IsNotNullOrWhitespace() )
            {
                Guid campusGuid = avCampus.AsGuid();

                pavQuery = pavQuery.Where( a => a.CampusGuid == campusGuid );
            }

            //
            // apply user filters if specified
            //
            var filteredQuery = pavQuery.AsQueryable();

            // by campus
            if ( cblCampusFilter.SelectedValues.Count > 0 )
            {
                filteredQuery = filteredQuery.Where( a => cblCampusFilter.SelectedValues.Contains( a.CampusId.ToString() ) );
            }

            // by Scheduled Service
            if ( cblScheduledServiceFilter.SelectedValues.Count > 0 )
            {
                filteredQuery = filteredQuery.Where( a => cblScheduledServiceFilter.SelectedValues.Contains( a.ScheduledServiceScheduleId.ToString() ) );
            }

            // by Scheduled Date
            if ( drpScheduledDateFilter.DelimitedValues.IsNotNullOrWhitespace() )
            {
                DateTime? startDate = drpScheduledDateFilter.DateRange.Start;
                DateTime? endDate = drpScheduledDateFilter.DateRange.End;

                if ( startDate.HasValue )
                {
                    filteredQuery = filteredQuery.Where( a => a.ScheduledDate >= startDate );
                }

                if ( endDate.HasValue )
                {
                    filteredQuery = filteredQuery.Where( a => a.ScheduledDate <= endDate );
                }
            }

            // by person's name
            if ( tbPersonNameFilter.Text.IsNotNullOrWhitespace() )
            {
                filteredQuery = filteredQuery.Where( a => a.FirstName.Contains( tbPersonNameFilter.Text ) || a.NickName.Contains( tbPersonNameFilter.Text ) || a.LastName.Contains( tbPersonNameFilter.Text ) );
            }

            // by Bringing Spouse
            if ( ddlBringingAnotherAdultFilter.SelectedValue == "Yes")
            {
                filteredQuery = filteredQuery.Where( a => a.BringingAnotherAdult == true );
            }
            else if ( ddlBringingAnotherAdultFilter.SelectedValue == "No")
            {
                filteredQuery = filteredQuery.Where( a => a.BringingAnotherAdult == false );
            }

            // by bringing Children
            if ( ddlBringingChildrenFilter.SelectedValue == "Yes")
            {
                filteredQuery = filteredQuery.Where( a => a.BringingChildren == true );
            }
            else if ( ddlBringingChildrenFilter.SelectedValue == "No")
            {
                filteredQuery = filteredQuery.Where( a => a.BringingChildren == false );
            }

            // by Attended
            if ( ddlHasAttendedFilter.SelectedValue == "Yes")
            {
                filteredQuery = filteredQuery.Where( a => a.AttendedDate.HasValue == true );

            }
            else if ( ddlHasAttendedFilter.SelectedValue == "No")
            {
                filteredQuery = filteredQuery.Where( a => a.AttendedDate.HasValue == false );
            }
                      
            // by Attended Service
            if ( cblAttendedServiceFilter.SelectedValues.Count > 0)
            {
                filteredQuery = filteredQuery.Where( a => cblAttendedServiceFilter.SelectedValues.Contains( a.AttendedServiceScheduleId.ToString() ) );
            }

            // by Attended Dated
            if ( drpAttendedDateFilter.DelimitedValues.IsNotNullOrWhitespace() )
            {
                DateTime? startDate = drpAttendedDateFilter.DateRange.Start;
                DateTime? endDate = drpAttendedDateFilter.DateRange.End;

                if ( startDate.HasValue )
                {
                    filteredQuery = filteredQuery.Where( a => a.AttendedDate >= startDate );
                }

                if ( endDate.HasValue )
                {
                    filteredQuery = filteredQuery.Where( a => a.AttendedDate <= endDate );
                }
            }

            // end filters
            
            // bind list to the grind
            var filteredList = filteredQuery.AsEnumerable().Select( pav => 
                                    new {
                                        pav.Id,
                                        pav.ScheduledDate,
                                        pav.ScheduledServiceName,
                                        Campus = pav.CampusName,
                                        pav.PersonId,
                                        Person = GetPersonLink(pav.AdultOnePersonAliasId),
                                        BringingAnotherAdult = pav.BringingAnotherAdult ? "Yes" : "No",
                                        BringingChildren = pav.BringingChildren ? "Yes" : "No",
                                        pav.AttendedDate,
                                        pav.AttendedServiceName,
                                    } ).ToList().AsQueryable();

            // sort the results
            SortProperty sortProperty = gGrid.SortProperty;

            if ( sortProperty.IsNotNull() )
            {
                filteredList = filteredList.Sort( sortProperty );

            }
            else
            {
                filteredList = filteredList.OrderByDescending( a => a.ScheduledDate );
            }

            gGrid.DataSource = filteredList.ToList();

            gGrid.DataBind();
        }

        #endregion

        /// <summary>
        /// Bind the service times dropdown in the manage visit modal
        /// </summary>
        /// <param name="campusId"></param>
        /// <param name="selectedDate"></param>
        private void BindServicesTimesDropdown( int campusId, DateTime? selectedDate )
        {
            bool dayHasService = false;

            // Reset drop down list
            ddlServiceAttended.Items.Clear();
            
            // setup schedule service, schedule lookup list, and selected schedules
            RockContext rockContext = new RockContext();
            ScheduleService scheduleService = new ScheduleService( rockContext );

            var scheduleLookupList = scheduleService.Queryable().Where( a => a.Name != null && a.Name != "" ).ToList().Select( a => new
            {
                a.Id,
                a.Name
            } );

            var selectedScheduleIds = scheduleService.GetByGuids( this.GetAttributeValue( "ServiceSchedules" ).SplitDelimitedValues().AsGuidList() ).Select( a => a.Id ).ToList();

            // get the campus
            CampusCache campus = CampusCache.Read( campusId, rockContext );

            if ( campus.IsNotNull() )
            {
                if ( !selectedDate.HasValue )
                {
                    // no date selected
                    ddlServiceAttended.Items.Clear();
                    ddlServiceAttended.Items.Add( new ListItem( "Select a Date above", "" ) );
                }
                else
                {
                    // date selected - add valid service times for that day / campus
                    foreach ( var serviceTime in campus.ServiceTimes )
                    {
                        if ( serviceTime.Day == selectedDate.Value.DayOfWeek.ToString() )
                        {
                            dayHasService = true;

                            string time = serviceTime.Time.Replace( "%", "" ).Replace( "*", "" ).Trim();

                            // look for a matching schedule from schedules block setting
                            string scheduleName = String.Format( "{0} {1}", serviceTime.Day, time.RemoveSpaces() );

                            var scheduleLookup = scheduleLookupList.FirstOrDefault( a => a.Name == scheduleName );

                            if ( scheduleLookup != null && selectedScheduleIds.Contains( scheduleLookup.Id ) )
                            {
                                // matching schedule found, add to drop down list
                                ListItem item = new ListItem( scheduleLookup.Name, scheduleLookup.Id.ToString() );

                                ddlServiceAttended.Items.Add( item );
                            }
                        }
                    }

                    // add a default message if no services
                    if ( !dayHasService )
                    {
                        ddlServiceAttended.Items.Add( new ListItem( "No services on selected day", "" ) );
                    }
                }
            }
        }
       
        #endregion

        #region Helper Methods

        /// <summary>
        /// Returns a clickable person link using a person alias Id
        /// </summary>
        /// <param name="personAliasId"></param>
        /// <returns></returns>
        private object GetPersonLink( int personAliasId )
        {
            Person person = new PersonAliasService( new RockContext() ).Get( personAliasId ).Person;

            return String.Format( "<a href=/person/{0}>{1}</a>", person.Id, person.FullName);
        }

        #endregion

    }
}