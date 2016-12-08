﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
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
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;

using Humanizer;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

namespace RockWeb.Blocks.Reporting
{
    /// <summary>
    /// 
    /// </summary>
    [DisplayName( "Calendar Dimension Settings" )]
    [Category( "Reporting" )]
    [Description( "Helps configure and generate the AnalyticsDimDate table for BI Analytics" )]

    [DateField( "StartDate", "", false, "", "CustomSetting", 0 )]
    [DateField( "EndDate", "", false, "", "CustomSetting", 0 )]
    [IntegerField( "FiscalStartMonth", "", false, 1, "CustomSetting", 0 )]
    [BooleanField( "GivingMonthUseSundayDate", "", false, "CustomSetting", 1 )]
    public partial class CalendarDimensionSettings : Rock.Web.UI.RockBlock
    {
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
                ShowDetail();
            }
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        public void ShowDetail()
        {
            LoadDropDowns();

            DateTime? startDate = this.GetAttributeValue( "StartDate" ).AsDateTime() ?? RockDateTime.Today.AddYears( -150 );
            DateTime? endDate = this.GetAttributeValue( "EndDate" ).AsDateTime() ?? RockDateTime.Today.AddYears( 100 );

            dpStartDate.SelectedDate = startDate;
            dpEndDate.SelectedDate = endDate;

            int? fiscalStartMonth = this.GetAttributeValue( "FiscalStartMonth" ).AsIntegerOrNull() ?? 1;
            monthDropDownList.SetValue( fiscalStartMonth );

            bool givingMonthUseSundayDate = this.GetAttributeValue( "GivingMonthUseSundayDate" ).AsBoolean();
            cbGivingMonthUseSundayDate.Checked = givingMonthUseSundayDate;
        }

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            monthDropDownList.Items.Clear();
            monthDropDownList.Items.Add( new ListItem( string.Empty, string.Empty ) );
            DateTime date = new DateTime( 2000, 1, 1 );
            for ( int i = 0; i <= 11; i++ )
            {
                monthDropDownList.Items.Add( new ListItem( date.AddMonths( i ).ToString( "MMMM" ), ( i + 1 ).ToString() ) );
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
        /// Handles the Click event of the btnGenerate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnGenerate_Click( object sender, EventArgs e )
        {
            this.SetAttributeValue( "StartDate", dpStartDate.SelectedDate.Value.ToString( "o" ) );
            this.SetAttributeValue( "EndDate", dpEndDate.SelectedDate.Value.ToString( "o" ) );
            this.SetAttributeValue( "FiscalStartMonth", monthDropDownList.SelectedValue );
            this.SetAttributeValue( "GivingMonthUseSundayDate", cbGivingMonthUseSundayDate.Checked.ToTrueFalse() );

            int fiscalStartMonth = monthDropDownList.SelectedValue.AsIntegerOrNull() ?? 1;

            bool givingMonthUseSundayDate = cbGivingMonthUseSundayDate.Checked;

            // remove all the rows and rebuild
            new RockContext().Database.ExecuteSqlCommand( string.Format( "TRUNCATE TABLE {0}", typeof( AnalyticsDimDate ).GetCustomAttribute<TableAttribute>().Name ) );

            List<AnalyticsDimDate> generatedDates = new List<AnalyticsDimDate>();

            // NOTE: AnalyticsDimDate is not an Rock.Model.Entity table and therefore doesn't have a Service<T>, so just use update using rockContext.AnalyticsDimDates 
            var generateDate = dpStartDate.SelectedDate.Value;
            var currentYear = generateDate.Year;
            var holidayDatesForYear = HolidayHelper.GetHolidayList( currentYear );
            var easterSundayForYear = HolidayHelper.EasterSunday( currentYear );

            while ( generateDate <= dpEndDate.SelectedDate.Value )
            {
                if ( currentYear != generateDate.Year )
                {
                    currentYear = generateDate.Year;
                    holidayDatesForYear = HolidayHelper.GetHolidayList( currentYear );
                    easterSundayForYear = HolidayHelper.EasterSunday( currentYear );
                }

                AnalyticsDimDate analyticsDimDate = new AnalyticsDimDate();
                analyticsDimDate.DateKey = generateDate.ToString( "yyyyMMdd" ).AsInteger();
                analyticsDimDate.Date = generateDate.Date;
                analyticsDimDate.FullDateDescription = string.Empty;
                analyticsDimDate.DayOfWeek = generateDate.DayOfWeek.ConvertToInt();
                analyticsDimDate.DayOfWeekName = generateDate.DayOfWeek.ConvertToString();

                // luckily, DayOfWeek abbreviations are just the first 3 chars
                analyticsDimDate.DayOfWeekAbbreviated = analyticsDimDate.DayOfWeekName.Substring( 0, 3 );
                analyticsDimDate.DayNumberInCalendarMonth = generateDate.Day;
                analyticsDimDate.DayNumberInCalendarYear = generateDate.DayOfYear;

                // DayNumberInFiscalMonth is simply the same as DayNumberInCalendarMonth since we only let them pick a FiscalStartMonth ( not a Month and Day )
                analyticsDimDate.DayNumberInFiscalMonth = analyticsDimDate.DayNumberInCalendarMonth;

                if ( fiscalStartMonth == 0 )
                {
                    analyticsDimDate.DayNumberInFiscalYear = analyticsDimDate.DayNumberInCalendarYear;
                }
                else
                {
                    // TODO: what about leap year, and double-check that is just the Number Of days since the FiscalStartMonth
                    analyticsDimDate.DayNumberInFiscalYear = 0;
                }

                // from http://stackoverflow.com/a/4655207/1755417
                DateTime endOfMonth = new DateTime( generateDate.Year, generateDate.Month, DateTime.DaysInMonth( generateDate.Year, generateDate.Month ) );

                analyticsDimDate.LastDayInMonthIndictor = generateDate == endOfMonth;

                // TODO: Double-check we want to base it on Monday instead of Sunday
                analyticsDimDate.WeekNumberInMonth = generateDate.GetWeekOfMonth( RockDateTime.FirstDayOfWeek );

                analyticsDimDate.SundayDate = generateDate.SundayDate();

                // TODO: Is this right?
                analyticsDimDate.CalendarMonthNumberInYear = generateDate.Month;
                analyticsDimDate.CalendarMonth = generateDate.Month;
                analyticsDimDate.CalendarMonthName = generateDate.ToString( "MMMM" );
                analyticsDimDate.CalendarMonthNameAbbrevated = generateDate.ToString( "MMM" );

                analyticsDimDate.CalendarYearMonth = generateDate.ToString( "yyyyMM" );
                int quarter = ( generateDate.Month + 2 ) / 3;

                analyticsDimDate.CalendarQuarter = string.Format( "Q{0}", quarter );
                analyticsDimDate.CalendarYearQuarter = string.Format( "{0} Q{1}", generateDate.Year, quarter );
                analyticsDimDate.CalendarYear = generateDate.Year;

                /* Fiscal Stuff. Doublecheck exactly how to do these */


                // figure out the fiscalMonthNumber and QTR.  For example, if the Fiscal Start Month is April, and Today is April 1st, the Fiscal Month Number would be 1
                int fiscalMonthNumber = new System.DateTime( generateDate.Year, generateDate.Month, 1 ).AddMonths( 1 - fiscalStartMonth ).Month;
                int fiscalQuarter = ( fiscalMonthNumber + 2 ) / 3;
                int fiscalYear = generateDate.AddMonths( -( 1 + fiscalStartMonth ) ).Year;


                // TODO: NOTE: This is complicated, see comments on AnalyticsDimDate.GivingMonth
                // analyticsDimDate.GivingMonth
                // analyticsDimDate.GivingMonthName

                DateTime fiscalStartDate = new DateTime( generateDate.Year, fiscalStartMonth, 1 );
                int fiscalWeekOffset = fiscalStartDate.GetWeekOfYear( RockDateTime.FirstDayOfWeek ) - 1;
                int fiscalWeek = generateDate.GetWeekOfYear( RockDateTime.FirstDayOfWeek ) - fiscalWeekOffset;
                fiscalWeek = fiscalWeek < 1 ? fiscalWeek + 52 : fiscalWeek;

                analyticsDimDate.FiscalWeek = fiscalWeek;
                analyticsDimDate.FiscalWeekNumberInYear = generateDate.GetWeekOfYear( RockDateTime.FirstDayOfWeek );
                analyticsDimDate.FiscalMonth = generateDate.ToString( "MMMM" );
                analyticsDimDate.FiscalMonthAbbrevated = generateDate.ToString( "MMM" );
                analyticsDimDate.FiscalMonthNumberInYear = generateDate.Month;
                analyticsDimDate.FiscalMonthYear = generateDate.ToString( "MM yyyy" );
                analyticsDimDate.FiscalQuarter = string.Format( "Q{0}", fiscalQuarter );
                analyticsDimDate.FiscalYearQuarter = string.Format( "{0} Q{1}", fiscalYear, fiscalQuarter );
                analyticsDimDate.FiscalHalfYear = fiscalQuarter < 3 ? "First" : "Second";
                analyticsDimDate.FiscalYear = fiscalYear;

                analyticsDimDate.EasterIndicator = generateDate == easterSundayForYear;

                // Easter Week starts the Sunday before Easter (Palm Sunday) and ends on Holy Saturday (the day before Easter Sunday)
                int daysUntilEaster = ( easterSundayForYear - generateDate ).Days;
                analyticsDimDate.EasterWeekIndicator = daysUntilEaster >= 1 && daysUntilEaster < 8;

                analyticsDimDate.ChristmasIndicator = generateDate.Month == 12 && generateDate.Day == 25;

                // Christmas week is 12/24-12/30
                analyticsDimDate.ChristmasWeekIndicator = generateDate.Minute == 12 && ( generateDate.Day >= 24 && generateDate.Day <= 30 );

                analyticsDimDate.HolidayIndicator = holidayDatesForYear.Any( a => a.Date == generateDate ) || analyticsDimDate.ChristmasIndicator || analyticsDimDate.EasterIndicator;
                analyticsDimDate.WeekHolidayIndicator = analyticsDimDate.ChristmasWeekIndicator || analyticsDimDate.EasterWeekIndicator;

                generatedDates.Add( analyticsDimDate );

                generateDate = generateDate.AddDays( 1 );
            }

            // this might take a few minutes
            while ( generatedDates.Any() )
            {
                using ( var rockContext = new RockContext() )
                {
                    rockContext.Configuration.AutoDetectChangesEnabled = false;
                    rockContext.Configuration.ValidateOnSaveEnabled = false;
                    var insertRange = generatedDates.Take( 1000 ).ToList();
                    rockContext.AnalyticsDimDates.AddRange( insertRange );
                    insertRange.ForEach( a => generatedDates.Remove( a ) );
                    rockContext.SaveChanges( true );
                }
            }

            //var rockContext = new RockContext();
            //AnalyticsDimDate.BulkInsert( rockContext, generatedDates );
        }

        // 


        #endregion
    }
}