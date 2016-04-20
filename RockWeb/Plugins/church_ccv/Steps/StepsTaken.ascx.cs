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
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;

using church.ccv.Steps;
using church.ccv.Steps.Model;
using System.Drawing;
using System.Data.Entity;

namespace RockWeb.Plugins.church_ccv.Steps
{
    /// <summary>
    /// A block to show a dasboard of measure for the pastor.
    /// </summary>
    [DisplayName( "Steps Taken" )]
    [Category( "CCV > Steps" )]
    [Description( "A block to show the number of steps taken." )]

    public partial class StepsTaken : Rock.Web.UI.RockBlock
    {
        #region Fields



        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the active tab.
        /// </summary>
        /// <value>
        /// The active tab.
        /// </value>
        protected string ActiveTab { get; set; }

        /// <summary>
        /// Gets or sets the date range.
        /// </summary>
        /// <value>
        /// The date range.
        /// </value>
        protected DateRange DateRange { get; set; }
        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

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

            gfStepDetails.ApplyFilterClick += GfStepDetails_ApplyFilterClick;
            gfStepDetails.DisplayFilterValue += GfStepDetails_DisplayFilterValue;
            gStepDetails.GridRebind += GStepDetails_GridRebind;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            DateRange = SlidingDateRangePicker.CalculateDateRangeFromDelimitedValues( drpDateRange.DelimitedValues );

            if ( !Page.IsPostBack )
            {
                drpDateRange.SlidingDateRangeMode = SlidingDateRangePicker.SlidingDateRangeType.Current;
                drpDateRange.TimeUnit = SlidingDateRangePicker.TimeUnitType.Year;

                DateRange = SlidingDateRangePicker.CalculateDateRangeFromDelimitedValues( drpDateRange.DelimitedValues );

                SetDateLabel();
                LoadCampuses();
                LoadStepTypes();
            }
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        /// <summary>
        /// Handles the Click event of the lbSetDateRange control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbSetDateRange_Click( object sender, EventArgs e )
        {
            SetDateLabel();
        }

        /// <summary>
        /// Handles the Click event of the lbTab control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbTab_Click( object sender, EventArgs e )
        {
            LinkButton lb = sender as LinkButton;
            if ( lb != null )
            {
                ActiveTab = lb.ID;
                ShowTab();
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cpDetailCampus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void cpDetailCampus_SelectedIndexChanged( object sender, EventArgs e )
        {
            ShowStepDetails();
        }

        /// <summary>
        /// Handles the GridRebind event of the GStepDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridRebindEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void GStepDetails_GridRebind( object sender, GridRebindEventArgs e )
        {
            ShowStepDetails();
        }

        /// <summary>
        /// Handles the ApplyFilterClick event of the GfStepDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void GfStepDetails_ApplyFilterClick( object sender, EventArgs e )
        {
            gfStepDetails.SaveUserPreference( "Measure Type", ddlMeasureType.SelectedValue != string.Empty ? ddlMeasureType.SelectedValue : string.Empty );

            ShowStepDetails();
        }

        /// <summary>
        /// Gfs the step details_ display filter value.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void GfStepDetails_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
        {
            switch ( e.Key )
            {
                case "Measure Type":
                    int? measureId = e.Value.AsInteger();

                    if ( measureId.HasValue )
                    {
                        e.Value = new StepMeasureService( new RockContext() ).Get( measureId.Value).Title;
                    }

                    break;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Loads the step types.
        /// </summary>
        private void LoadStepTypes()
        {
            using ( RockContext rockContext = new RockContext() ) {

                var measureTypes = new StepMeasureService( rockContext ).Queryable()
                                    .Where( m => m.IsActive && !m.IsTbd )
                                    .Select( m => new { m.Id, m.Title } )
                                    .OrderBy( m => m.Title);

                ddlMeasureType.DataTextField = "Title";
                ddlMeasureType.DataValueField = "Id";
                ddlMeasureType.DataSource = measureTypes.ToList();
                ddlMeasureType.DataBind();

                ddlMeasureType.Items.Insert( 0, "" );

                ddlMeasureType.SelectedValue = gfStepDetails.GetUserPreference( "Measure Type" );
            }
        }

        /// <summary>
        /// Sets the date label.
        /// </summary>
        private void SetDateLabel()
        {
            if ( DateRange.Start.HasValue && DateRange.End.HasValue )
            {
                hlDate.Text = string.Format( "{0} - {1}", DateRange.Start.Value.ToShortDateString(), DateRange.End.Value.ToShortDateString() );
            }
            else if ( DateRange.Start.HasValue )
            {
                hlDate.Text = string.Format( "Since {0}", DateRange.Start.Value.ToShortDateString() );
            }
            else if ( DateRange.End.HasValue )
            {
                hlDate.Text = string.Format( "Before {0}", DateRange.End.Value.ToShortDateString() );
            }
            else
            {
                hlDate.Text = "No Date Range Provided";
            }
        }

        /// <summary>
        /// Shows the tab.
        /// </summary>
        private void ShowTab()
        {
            liCampus.RemoveCssClass( "active" );
            pnlCampus.Visible = false;

            liPastor.RemoveCssClass( "active" );
            pnlPastor.Visible = false;

            liAdults.RemoveCssClass( "active" );
            pnlAdults.Visible = false;

            liStepDetails.RemoveCssClass( "active" );
            pnlStepDetails.Visible = false;

            switch ( ActiveTab ?? string.Empty )
            {
                case "lbPastor":
                    {
                        liPastor.AddCssClass( "active" );
                        pnlPastor.Visible = true;
                        //BindPaymentsGrid();
                        break;
                    }

                case "lbAdults":
                    {
                        liAdults.AddCssClass( "active" );
                        pnlAdults.Visible = true;
                        //BindLinkagesGrid();
                        break;
                    }

                case "lbStepDetails":
                    {
                        liStepDetails.AddCssClass( "active" );
                        pnlStepDetails.Visible = true;
                        ShowStepDetails();
                        break;
                    }

                default:
                    {
                        liCampus.AddCssClass( "active" );
                        pnlCampus.Visible = true;
                        //BindRegistrationsGrid();
                        break;
                    }
            }
        }

        /// <summary>
        /// Loads the campuses.
        /// </summary>
        private void LoadCampuses()
        {
            cpDetailCampus.Campuses = CampusCache.All();
        }

        private void ShowStepDetails(int? campusId = null )
        {
            if ( cpDetailCampus.SelectedCampusId == null )
            {
                lDetailCampus.Text = "All Campuses ";
            }
            else
            {
                campusId = cpDetailCampus.SelectedItem.Value.AsInteger();
                lDetailCampus.Text = string.Format( "{0} Campus", cpDetailCampus.SelectedItem.Text );
            }

            using (RockContext rockContext = new RockContext() )
            {
                StepTakenService stepTakenService = new StepTakenService( rockContext );

                var query = stepTakenService.Queryable("StepMeasure").AsNoTracking();
                
                if ( campusId.HasValue )
                {
                    query = query.Where( s => s.CampusId == campusId.Value );
                }
                
                if ( DateRange != null && DateRange.Start.HasValue )
                {
                    query = query.Where( s => s.DateTaken >= DateRange.Start.Value );
                }
                
                if ( DateRange != null && DateRange.End.HasValue )
                {
                    query = query.Where( s => s.DateTaken <= DateRange.End.Value );
                }

                if ( !string.IsNullOrWhiteSpace(ddlMeasureType.SelectedValue ))
                {
                    int? measureId = ddlMeasureType.SelectedValue.AsInteger();
                    query = query.Where( s => s.StepMeasureId == measureId );
                }

                var results = query.Select( s =>
                                new {
                                    s.Id,
                                    s.DateTaken,
                                    StepMeasureTitle = s.StepMeasure.Title,
                                    s.StepMeasureId,
                                    s.SundayDate,
                                    s.PersonAlias.PersonId,
                                    FirstName = s.PersonAlias.Person.FirstName,
                                    NickName = s.PersonAlias.Person.NickName,
                                    LastName = s.PersonAlias.Person.LastName,
                                    FullName = s.PersonAlias.Person.LastName + ", " + s.PersonAlias.Person.NickName,
                                    Campus = s.Campus.Name
                                }
                );

                gStepDetails.SetLinqDataSource( results.OrderBy(s => s.DateTaken) );
                gStepDetails.DataBind();
            }
        }
        #endregion
    }
}