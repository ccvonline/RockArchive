﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ScheduleBuilder : CompositeControl, ILabeledControl
    {
        private Label _label;
        private LinkButton _btnDialogCancelX;

        private DateTimePicker _dpStartDateTime;
        private NumberBox _tbDurationHours;
        private NumberBox _tbDurationMinutes;

        private RadioButton _radOneTime;
        private RadioButton _radRecurring;

        private RadioButton _radSpecificDates;
        private RadioButton _radDaily;
        private RadioButton _radWeekly;
        private RadioButton _radMonthly;

        private HiddenField _hfSpecificDateListValues;

        // specific date panel
        private DatePicker _dpSpecificDate;

        // daily recurrence
        private RadioButton _radDailyEveryXDays;
        private NumberBox _tbDailyEveryXDays;
        private RadioButton _radDailyEveryWeekday;
        private RadioButton _radDailyEveryWeekendDay;

        // weekly recurrence
        private NumberBox _tbWeeklyEveryX;
        private CheckBox _cbWeeklySunday;
        private CheckBox _cbWeeklyMonday;
        private CheckBox _cbWeeklyTuesday;
        private CheckBox _cbWeeklyWednesday;
        private CheckBox _cbWeeklyThursday;
        private CheckBox _cbWeeklyFriday;
        private CheckBox _cbWeeklySaturday;

        // monthly
        private RadioButton _radMonthlyDayX;
        private NumberBox _tbMonthlyDayX;
        private NumberBox _tbMonthlyXMonths;
        private RadioButton _radMonthlyNth;
        private DropDownList _ddlMonthlyNth;
        private DropDownList _ddlMonthlyDayName;

        // end date
        private RadioButton _radEndByNone;
        private RadioButton _radEndByDate;
        private DatePicker _dpEndBy;
        private RadioButton _radEndByOccurrenceCount;
        private NumberBox _tbEndByOccurrenceCount;

        // exclusions
        private HiddenField _hfExclusionDateRangeListValues;
        private DatePicker _dpExclusionDateStart;
        private DatePicker _dpExclusionDateEnd;

        // action buttons
        private LinkButton _btnSaveSchedule;
        private LinkButton _btnCancelSchedule;

        private ScriptManagerProxy _smProxy;

        // consts
        private readonly Dictionary<string, int> nthNames = new Dictionary<string, int> { 
            {"First", 1}, 
            {"Second", 2}, 
            {"Third", 3}, 
            {"Fourth", 4}, 
            {"Last", -1} 
        };

        private const string iCalendarContentEmptyEvent = @"
BEGIN:VCALENDAR
VERSION:2.0
BEGIN:VEVENT
END:VEVENT
END:VCALENDAR
";

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleBuilder"/> class.
        /// </summary>
        public ScheduleBuilder()
        {
            // control
            _label = new Label();

            // modal header
            _btnDialogCancelX = new LinkButton();

            // modal body
            _dpStartDateTime = new DateTimePicker();

            _tbDurationHours = new NumberBox();
            _tbDurationMinutes = new NumberBox();

            _radOneTime = new RadioButton();
            _radRecurring = new RadioButton();

            _radSpecificDates = new RadioButton();
            _radDaily = new RadioButton();
            _radWeekly = new RadioButton();
            _radMonthly = new RadioButton();

            // specific date
            _hfSpecificDateListValues = new HiddenField();

            _dpSpecificDate = new DatePicker();

            // daily
            _radDailyEveryXDays = new RadioButton();
            _tbDailyEveryXDays = new NumberBox();
            _radDailyEveryWeekday = new RadioButton();
            _radDailyEveryWeekendDay = new RadioButton();

            // weekly
            _tbWeeklyEveryX = new NumberBox();
            _cbWeeklySunday = new CheckBox();
            _cbWeeklyMonday = new CheckBox();
            _cbWeeklyTuesday = new CheckBox();
            _cbWeeklyWednesday = new CheckBox();
            _cbWeeklyThursday = new CheckBox();
            _cbWeeklyFriday = new CheckBox();
            _cbWeeklySaturday = new CheckBox();

            // monthly
            _radMonthlyDayX = new RadioButton();
            _tbMonthlyDayX = new NumberBox();
            _tbMonthlyXMonths = new NumberBox();
            _radMonthlyNth = new RadioButton();
            _ddlMonthlyNth = new DropDownList();
            _ddlMonthlyDayName = new DropDownList();

            // end date
            _radEndByNone = new RadioButton();
            _radEndByDate = new RadioButton();
            _dpEndBy = new DatePicker();
            _radEndByOccurrenceCount = new RadioButton();
            _tbEndByOccurrenceCount = new NumberBox();

            // exclusions
            _hfExclusionDateRangeListValues = new HiddenField();
            _dpExclusionDateStart = new DatePicker();
            _dpExclusionDateEnd = new DatePicker();

            // modal footer
            _btnSaveSchedule = new LinkButton();
            _btnCancelSchedule = new LinkButton();

            _smProxy = new ScriptManagerProxy();
            
            iCalendarContent = iCalendarContentEmptyEvent;

        }

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        /// <value>
        /// The label text.
        /// </value>
        public string LabelText
        {
            get { return _label.Text; }
            set { _label.Text = value; }
        }

        /// <summary>
        /// Gets or sets the content of the i calendar.
        /// </summary>
        /// <value>
        /// The content of the i calendar.
        /// </value>
        public string iCalendarContent
        {
            get
            {
                EnsureChildControls();

                DDay.iCal.Event calendarEvent = new DDay.iCal.Event();
                calendarEvent.DTStart = new DDay.iCal.iCalDateTime( _dpStartDateTime.SelectedDateTime.Value );
                
                int durationHours = _tbDurationHours.Text.AsInteger() ?? 1;
                int durationMins = _tbDurationMinutes.Text.AsInteger() ?? 0;
                calendarEvent.Duration = new TimeSpan( durationHours, durationMins, 0 );
                
                
                //StringReader stringReader = new StringReader( iCalendarContentEmptyEvent );
                //var calendarList = DDay.iCal.iCalendar.LoadFromStream( stringReader );

                DDay.iCal.iCalendar calendar = new iCalendar();
                calendar.Events.Add(calendarEvent);

                iCalendarSerializer s = new iCalendarSerializer( calendar );

                return s.SerializeToString( calendar );
            }

            set
            {
                EnsureChildControls();

                //// iCal is stored as a list of Calendar's each with a list of Events, etc.  
                //// We just need one Calendar and one Event

                StringReader stringReader = new StringReader( value );
                var calendarList = DDay.iCal.iCalendar.LoadFromStream( stringReader );
                DDay.iCal.Event calendarEvent = null;
                if ( calendarList.Count > 0 )
                {
                    var calendar = calendarList[0] as DDay.iCal.iCalendar;
                    if ( calendar != null )
                    {
                        calendarEvent = calendar.Events[0] as DDay.iCal.Event;

                        if ( calendarEvent.DTStart != null )
                        {
                            _dpStartDateTime.SelectedDateTime = calendarEvent.DTStart.Value;
                            _tbDurationHours.Text = calendarEvent.Duration.Hours.ToString();
                            _tbDurationMinutes.Text = calendarEvent.Duration.Minutes.ToString();
                        }
                        else
                        {
                            _dpStartDateTime.SelectedDateTime = null;
                            _tbDurationHours.Text = string.Empty;
                            _tbDurationMinutes.Text = string.Empty;
                        }

                        if ( calendarEvent.RecurrenceRules.Count == 0 )
                        {
                            // One-Time
                            _radRecurring.Checked = false;
                            _radOneTime.Checked = true;
                        }
                        else
                        {
                            // Recurring
                            _radRecurring.Checked = true;
                            _radOneTime.Checked = false;
                        }

                        if ( _radRecurring.Checked )
                        {
                            IRecurrencePattern rrule = calendarEvent.RecurrenceRules[0];
                            switch ( rrule.Frequency )
                            {
                                case FrequencyType.Daily:
                                    #region daily

                                    _radDaily.Checked = true;
                                    _radDailyEveryXDays.Checked = true;
                                    _tbDailyEveryXDays.Text = rrule.Interval.ToString();

                                    // NOTE:  Daily Every Weekday/Weekend Day is actually Weekly on Day(s)OfWeek in iCal
                                    break;

                                    #endregion
                                case FrequencyType.Weekly:
                                    #region weekly

                                    _radWeekly.Checked = true;
                                    _tbWeeklyEveryX.Text = rrule.Interval.ToString();

                                    foreach ( DayOfWeek dow in rrule.ByDay.Select( a => a.DayOfWeek ) )
                                    {
                                        switch ( dow )
                                        {
                                            case DayOfWeek.Sunday:
                                                _cbWeeklySunday.Checked = true;
                                                break;
                                            case DayOfWeek.Monday:
                                                _cbWeeklyMonday.Checked = true;
                                                break;
                                            case DayOfWeek.Tuesday:
                                                _cbWeeklyTuesday.Checked = true;
                                                break;
                                            case DayOfWeek.Wednesday:
                                                _cbWeeklyWednesday.Checked = true;
                                                break;
                                            case DayOfWeek.Thursday:
                                                _cbWeeklyThursday.Checked = true;
                                                break;
                                            case DayOfWeek.Friday:
                                                _cbWeeklyFriday.Checked = true;
                                                break;
                                            case DayOfWeek.Saturday:
                                                _cbWeeklySaturday.Checked = true;
                                                break;
                                        }
                                    }

                                    break;

                                    #endregion
                                case FrequencyType.Monthly:
                                    #region monthly

                                    _radMonthly.Checked = true;
                                    if ( rrule.ByMonthDay.Count > 0 )
                                    {
                                        // Day X of every X Months
                                        _radMonthlyDayX.Checked = true;
                                        int monthDay = rrule.ByMonthDay[0];

                                        _tbMonthlyDayX.Text = monthDay.ToString();
                                        _tbMonthlyXMonths.Text = rrule.Interval.ToString();
                                    }
                                    else if ( rrule.ByDay.Count > 0 )
                                    {
                                        // The Nth <DayOfWeekName>
                                        _radMonthlyNth.Checked = true;
                                        IWeekDay bydate = rrule.ByDay[0];
                                        _ddlMonthlyNth.SelectedValue = bydate.Offset.ToString();
                                        _ddlMonthlyDayName.SelectedValue = bydate.DayOfWeek.ConvertToInt().ToString();
                                    }

                                    break;

                                    #endregion
                                default:
                                    _radSpecificDates.Checked = true;
                                    break;
                            }

                            // Continue Until
                            if ( rrule.Until > DateTime.MinValue )
                            {
                                _radEndByDate.Checked = true;
                                _dpEndBy.SelectedDate = rrule.Until;
                            }
                            else if ( rrule.Count > 0 )
                            {
                                _radEndByOccurrenceCount.Checked = true;
                                _tbEndByOccurrenceCount.Text = rrule.Count.ToString();
                            }
                            else
                            {
                                _radEndByNone.Checked = true;
                            }

                            // Exclusions
                            if ( calendarEvent.ExceptionDates.Count > 0 )
                            {
                                // convert individual ExceptionDates into a list of Date Ranges
                                List<Period> exDateRanges = new List<Period>();
                                List<IDateTime> dates = calendarEvent.ExceptionDates[0].Select( a => a.StartTime ).OrderBy( a => a ).ToList();
                                IDateTime previousDate = dates[0].AddDays( -1 );
                                var dateRange = new Period { StartTime = dates[0] };
                                foreach ( var date in dates )
                                {
                                    if ( !date.Equals( previousDate.AddDays( 1 ) ) )
                                    {
                                        dateRange.EndTime = previousDate;
                                        exDateRanges.Add( dateRange );
                                        dateRange = new Period { StartTime = date };
                                    }

                                    previousDate = date;
                                }

                                dateRange.EndTime = dates.Last();
                                exDateRanges.Add( dateRange );

                                _hfExclusionDateRangeListValues.Value = exDateRanges.Select( a => string.Format( "{0} - {1}", a.StartTime, a.EndTime ) ).ToList().AsDelimited( "," );
                            }
                        }
                        else
                        {
                            _radSpecificDates.Checked = true;

                            if ( calendarEvent.RecurrenceDates.Count > 0 )
                            {
                                // Specific Dates
                                _radOneTime.Checked = false;
                                _radRecurring.Checked = true;
                                _radEndByNone.Checked = true;
                                _radEndByDate.Checked = false;
                                _radEndByOccurrenceCount.Checked = false;
                                IPeriodList dates = calendarEvent.RecurrenceDates[0];
                                _hfSpecificDateListValues.Value = dates.Select( a => a.StartTime ).ToList().AsDelimited( "," );
                            }

                            _radEndByNone.Checked = true;
                            _hfExclusionDateRangeListValues.Value = string.Empty;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when [save schedule].
        /// </summary>
        public event EventHandler SaveSchedule;

        /// <summary>
        /// Occurs when [cancel schedule].
        /// </summary>
        public event EventHandler CancelSchedule;

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            RegisterJavaScript();
            var sm = ScriptManager.GetCurrent( this.Page );

            if ( sm != null )
            {
                sm.RegisterAsyncPostBackControl( _btnSaveSchedule );
                sm.RegisterAsyncPostBackControl( _btnCancelSchedule );
                sm.RegisterAsyncPostBackControl( _btnDialogCancelX );
            }
        }

        /// <summary>
        /// Registers the java script.
        /// </summary>
        protected virtual void RegisterJavaScript()
        {
            // todo
        }

        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Controls.Clear();

            _btnDialogCancelX.ClientIDMode = ClientIDMode.Static;
            _btnDialogCancelX.CssClass = "close modal-control-cancel";
            _btnDialogCancelX.ID = "btnDialogCancelX";
            _btnDialogCancelX.Click += btnCancelSchedule_Click;
            _btnDialogCancelX.Text = "&times;";

            _dpStartDateTime.ClientIDMode = ClientIDMode.Static;
            _dpStartDateTime.ID = "dpStartDateTime";
            _dpStartDateTime.LabelText = "Start Date / Time";
            _dpStartDateTime.Required = true;

            _tbDurationHours.ClientIDMode = ClientIDMode.Static;
            _tbDurationHours.ID = "tbDurationHours";
            _tbDurationHours.CssClass = "input-mini";
            _tbDurationHours.MinimumValue = "0";
            _tbDurationHours.MaximumValue = "24";
            
            _tbDurationMinutes.ClientIDMode = ClientIDMode.Static;
            _tbDurationMinutes.ID = "tbDurationMinutes";
            _tbDurationMinutes.CssClass = "input-mini";
            _tbDurationMinutes.MinimumValue = "0";
            _tbDurationMinutes.MaximumValue = "59";

            _radOneTime.ClientIDMode = ClientIDMode.Static;
            _radOneTime.ID = "radOneTime";
            _radOneTime.GroupName = "ScheduleTypeGroup";
            _radOneTime.InputAttributes["class"] = "schedule-type";
            _radOneTime.Text = "One Time";
            _radOneTime.InputAttributes["data-schedule-type"] = "schedule-onetime";

            _radRecurring.ClientIDMode = ClientIDMode.Static;
            _radRecurring.ID = "radRecurring";
            _radRecurring.GroupName = "ScheduleTypeGroup";
            _radRecurring.InputAttributes["class"] = "schedule-type";
            _radRecurring.Text = "Recurring";
            _radRecurring.InputAttributes["data-schedule-type"] = "schedule-Recurring";

            _radSpecificDates.ClientIDMode = ClientIDMode.Static;
            _radSpecificDates.ID = "radSpecificDates";
            _radSpecificDates.GroupName = "recurrence-pattern-radio";
            _radSpecificDates.InputAttributes["class"] = "recurrence-pattern-radio";
            _radSpecificDates.Text = "Specific Dates";
            _radSpecificDates.InputAttributes["data-recurrence-pattern"] = "recurrence-pattern-specific-date";

            _radDaily.ClientIDMode = ClientIDMode.Static;
            _radDaily.ID = "radDaily";
            _radDaily.GroupName = "recurrence-pattern-radio";
            _radDaily.InputAttributes["class"] = "recurrence-pattern-radio";
            _radDaily.Text = "Daily";
            _radDaily.InputAttributes["data-recurrence-pattern"] = "recurrence-pattern-daily";

            _radWeekly.ClientIDMode = ClientIDMode.Static;
            _radWeekly.ID = "radWeekly";
            _radWeekly.GroupName = "recurrence-pattern-radio";
            _radWeekly.InputAttributes["class"] = "recurrence-pattern-radio";
            _radWeekly.Text = "Weekly";
            _radWeekly.InputAttributes["data-recurrence-pattern"] = "recurrence-pattern-weekly";

            _radMonthly.ClientIDMode = ClientIDMode.Static;
            _radMonthly.ID = "radMonthly";
            _radMonthly.GroupName = "recurrence-pattern-radio";
            _radMonthly.InputAttributes["class"] = "recurrence-pattern-radio";
            _radMonthly.Text = "Monthly";
            _radMonthly.InputAttributes["data-recurrence-pattern"] = "recurrence-pattern-monthly";

            _hfSpecificDateListValues.ClientIDMode = ClientIDMode.Static;
            _hfSpecificDateListValues.ID = "hfSpecificDateListValues";

            // specific date
            _dpSpecificDate.ClientIDMode = ClientIDMode.Static;
            _dpSpecificDate.ID = "dpSpecificDate";

            // daily recurrence
            _radDailyEveryXDays.ClientIDMode = ClientIDMode.Static;
            _radDailyEveryXDays.ID = "radDailyEveryXDays";
            _radDailyEveryXDays.GroupName = "daily-options";

            _tbDailyEveryXDays.ClientIDMode = ClientIDMode.Static;
            _tbDailyEveryXDays.ID = "txtDailyEveryXDays";
            _tbDailyEveryXDays.CssClass = "input-mini";

            _radDailyEveryWeekday.ClientIDMode = ClientIDMode.Static;
            _radDailyEveryWeekday.ID = "radDailyEveryWeekday";
            _radDailyEveryWeekday.GroupName = "daily-options";

            _radDailyEveryWeekendDay.ClientIDMode = ClientIDMode.Static;
            _radDailyEveryWeekendDay.ID = "radDailyEveryWeekendDay";
            _radDailyEveryWeekendDay.GroupName = "daily-options";

            // weekly recurrence
            _tbWeeklyEveryX.ClientIDMode = ClientIDMode.Static;
            _tbWeeklyEveryX.ID = "tbWeeklyEveryX";
            _tbWeeklyEveryX.CssClass = "input-mini";
            _tbWeeklyEveryX.MinimumValue = "1";
            _tbWeeklyEveryX.MaximumValue = "52";

            _cbWeeklySunday.ClientIDMode = ClientIDMode.Static;
            _cbWeeklySunday.ID = "cbWeeklySunday";
            _cbWeeklySunday.Text = "Sun";
            _cbWeeklyMonday.ClientIDMode = ClientIDMode.Static;
            _cbWeeklyMonday.ID = "cbWeeklyMonday";
            _cbWeeklyMonday.Text = "Mon";
            _cbWeeklyTuesday.ClientIDMode = ClientIDMode.Static;
            _cbWeeklyTuesday.ID = "cbWeeklyTuesday";
            _cbWeeklyTuesday.Text = "Tue";
            _cbWeeklyWednesday.ClientIDMode = ClientIDMode.Static;
            _cbWeeklyWednesday.ID = "cbWeeklyWednesday";
            _cbWeeklyWednesday.Text = "Wed";
            _cbWeeklyThursday.ClientIDMode = ClientIDMode.Static;
            _cbWeeklyThursday.ID = "cbWeeklyThursday";
            _cbWeeklyThursday.Text = "Thu";
            _cbWeeklyFriday.ClientIDMode = ClientIDMode.Static;
            _cbWeeklyFriday.ID = "cbWeeklyFriday";
            _cbWeeklyFriday.Text = "Fri";
            _cbWeeklySaturday.ClientIDMode = ClientIDMode.Static;
            _cbWeeklySaturday.ID = "cbWeeklySaturday";
            _cbWeeklySaturday.Text = "Sat";

            // monthly
            _radMonthlyDayX.ClientIDMode = ClientIDMode.Static;
            _radMonthlyDayX.ID = "radMonthlyDayX";
            _radMonthlyDayX.GroupName = "monthly-options";

            _tbMonthlyDayX.ClientIDMode = ClientIDMode.Static;
            _tbMonthlyDayX.ID = "tbMonthlyDayX";
            _tbMonthlyDayX.CssClass = "input-mini";
            _tbMonthlyDayX.MinimumValue = "1";
            _tbMonthlyDayX.MaximumValue = "31";

            _tbMonthlyXMonths.ClientIDMode = ClientIDMode.Static;
            _tbMonthlyXMonths.ID = "tbMonthlyXMonths";
            _tbMonthlyXMonths.CssClass = "input-mini";
            _tbMonthlyXMonths.MinimumValue = "1";
            _tbMonthlyXMonths.MaximumValue = "12";

            _radMonthlyNth.ClientIDMode = ClientIDMode.Static;
            _radMonthlyNth.ID = "radMonthlyNth";
            _radMonthlyNth.GroupName = "monthly-options";

            _ddlMonthlyNth.ClientIDMode = ClientIDMode.Static;
            _ddlMonthlyNth.ID = "ddlMonthlyNth";
            _ddlMonthlyNth.CssClass = "input-small";

            _ddlMonthlyNth.Items.Add( string.Empty );
            foreach ( var nth in nthNames )
            {
                _ddlMonthlyNth.Items.Add( new ListItem( nth.Key, nth.Value.ToString() ) );
            }

            _ddlMonthlyDayName.ClientIDMode = ClientIDMode.Static;
            _ddlMonthlyDayName.ID = "ddlMonthlyDayName";
            _ddlMonthlyDayName.CssClass = "input-medium";

            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
            _ddlMonthlyDayName.Items.Add( string.Empty );
            for ( int dayNum = 0; dayNum < dateTimeFormatInfo.DayNames.Length; dayNum++ )
            {
                _ddlMonthlyDayName.Items.Add( new ListItem( dateTimeFormatInfo.DayNames[dayNum], dayNum.ToString() ) );
            }

            // end date
            _radEndByNone.ClientIDMode = ClientIDMode.Static;
            _radEndByNone.ID = "radEndByNone";
            _radEndByNone.GroupName = "end-by";

            _radEndByDate.ClientIDMode = ClientIDMode.Static;
            _radEndByDate.ID = "radEndByDate";
            _radEndByDate.GroupName = "end-by";

            _dpEndBy.ClientIDMode = ClientIDMode.Static;
            _dpEndBy.ID = "dpEndBy";

            _radEndByOccurrenceCount.ClientIDMode = ClientIDMode.Static;
            _radEndByOccurrenceCount.ID = "radEndByOccurrenceCount";
            _radEndByOccurrenceCount.GroupName = "end-by";

            _tbEndByOccurrenceCount.ClientIDMode = ClientIDMode.Static;
            _tbEndByOccurrenceCount.ID = "tbEndByOccurrenceCount";
            _tbEndByOccurrenceCount.CssClass = "input-mini";
            _tbEndByOccurrenceCount.MinimumValue = "1";
            _tbEndByOccurrenceCount.MaximumValue = "999";

            // exclusions
            _hfExclusionDateRangeListValues.ClientIDMode = ClientIDMode.Static;
            _hfExclusionDateRangeListValues.ID = "hfExclusionDateRangeListValues";

            _dpExclusionDateStart.ClientIDMode = ClientIDMode.Static;
            _dpExclusionDateStart.ID = "dpExclusionDateStart";

            _dpExclusionDateEnd.ClientIDMode = ClientIDMode.Static;
            _dpExclusionDateEnd.ID = "dpExclusionDateEnd";

            // action buttons
            _btnCancelSchedule.ClientIDMode = ClientIDMode.Static;
            _btnCancelSchedule.ID = "btnCancelSchedule";
            _btnCancelSchedule.CssClass = "btn modal-control-cancel";
            _btnCancelSchedule.Click += btnCancelSchedule_Click;
            _btnCancelSchedule.Text = "Cancel";

            _btnSaveSchedule.ClientIDMode = ClientIDMode.Static;
            _btnSaveSchedule.ID = "btnSaveSchedule";
            
            _btnSaveSchedule.CssClass = "btn btn-primary modal-control-ok";
            _btnSaveSchedule.Click += btnSaveSchedule_Click;
            _btnSaveSchedule.Text = "Save Schedule";

            _smProxy.Scripts.Add( new ScriptReference( "~/Scripts/Rock/Rock.schedulebuilder.js" ) );

            Controls.Add( _btnDialogCancelX );
            Controls.Add( _dpStartDateTime );
            Controls.Add( _tbDurationHours );
            Controls.Add( _tbDurationMinutes );
            Controls.Add( _radOneTime );
            Controls.Add( _radRecurring );

            Controls.Add( _radSpecificDates );
            Controls.Add( _radDaily );
            Controls.Add( _radWeekly );
            Controls.Add( _radMonthly );

            Controls.Add( _hfSpecificDateListValues );
            Controls.Add( _dpSpecificDate );

            // daily recurrence
            Controls.Add( _radDailyEveryXDays );
            Controls.Add( _tbDailyEveryXDays );
            Controls.Add( _radDailyEveryWeekday );
            Controls.Add( _radDailyEveryWeekendDay );

            // weekly recurrence
            Controls.Add( _tbWeeklyEveryX );
            Controls.Add( _cbWeeklySunday );
            Controls.Add( _cbWeeklyMonday );
            Controls.Add( _cbWeeklyTuesday );
            Controls.Add( _cbWeeklyWednesday );
            Controls.Add( _cbWeeklyThursday );
            Controls.Add( _cbWeeklyFriday );
            Controls.Add( _cbWeeklySaturday );

            // monthly
            Controls.Add( _radMonthlyDayX );
            Controls.Add( _tbMonthlyDayX );
            Controls.Add( _tbMonthlyXMonths );
            Controls.Add( _radMonthlyNth );
            Controls.Add( _ddlMonthlyNth );
            Controls.Add( _ddlMonthlyDayName );

            // end date
            Controls.Add( _radEndByNone );
            Controls.Add( _radEndByDate );
            Controls.Add( _dpEndBy );
            Controls.Add( _radEndByOccurrenceCount );
            Controls.Add( _tbEndByOccurrenceCount );

            // exclusions
            Controls.Add( _hfExclusionDateRangeListValues );
            Controls.Add( _dpExclusionDateStart );
            Controls.Add( _dpExclusionDateEnd );

            Controls.Add( _btnSaveSchedule );
            Controls.Add( _btnCancelSchedule );

            Controls.Add( _smProxy );
        }

        /// <summary>
        /// Handles the Click event of the btnSaveSchedule control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSaveSchedule_Click( object sender, EventArgs e )
        {
            var s = iCalendarContent;
            if ( SaveSchedule != null )
            {
                SaveSchedule( sender, e );
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCancelSchedule control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancelSchedule_Click( object sender, EventArgs e )
        {
            if ( CancelSchedule != null )
            {
                CancelSchedule( sender, e );
            }
        }

        /// <summary>
        /// Writes the <see cref="T:System.Web.UI.WebControls.CompositeControl" /> content to the specified <see cref="T:System.Web.UI.HtmlTextWriter" /> object, for display on the client.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the output stream to render HTML content on the client.</param>
        protected override void Render( HtmlTextWriter writer )
        {
            string controlHtmlFragment = @"
    <a id='schedule-builder-button' role='button' class='btn btn-small'>
        <i class='icon-calendar'></i> ";

            writer.Write( controlHtmlFragment );

            _label.RenderControl( writer );

            controlHtmlFragment = @"
    </a>

    <div id='schedule-builder-modal' class='modal hide fade schedule-builder'>
        <div class='modal-header'>";

            writer.Write( controlHtmlFragment );

            _btnDialogCancelX.RenderControl( writer );

            controlHtmlFragment = @"
            <h3>Schedule Builder</h3>
        </div>
        <div class='modal-body'>
            <div id='modal-scroll-container' class='scroll-container'>
                <div class='scrollbar'>
                    <div class='track'>
                        <div class='thumb'>
                            <div class='end'></div>
                        </div>
                    </div>
                </div>
                <div class='viewport'>
                    <div class='overview'>

                        <!-- modal body -->
                        <div class='form-horizontal'>";

            // Start DateTime
            writer.Write( controlHtmlFragment );
            _dpStartDateTime.RenderControl( writer );

            // Duration
            writer.AddAttribute( "class", "control-group" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            writer.Write( "<span class='control-label'>Duration</span>" );
            writer.AddAttribute( "class", "controls" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _tbDurationHours.RenderControl( writer );
            writer.Write( " hrs " );
            _tbDurationMinutes.RenderControl( writer );
            writer.Write( " mins " );
            writer.RenderEndTag();
            writer.RenderEndTag();

            // One-time/Recurring Radiobuttons
            writer.AddAttribute( "class", "control-group" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            writer.AddAttribute( "class", "controls" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _radOneTime.RenderControl( writer );
            _radRecurring.RenderControl( writer );
            writer.RenderEndTag();
            writer.RenderEndTag();

            // Recurrence Panel: Start
            if ( _radOneTime.Checked )
            {
                writer.AddStyleAttribute( HtmlTextWriterStyle.Display, "none" );
            }

            writer.AddAttribute( "id", "schedule-recurrence-panel" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.AddAttribute( "class", "legend-small" );
            writer.RenderBeginTag( HtmlTextWriterTag.Legend );
            writer.Write( "Recurrence" );
            writer.RenderEndTag();

            // OccurrencePattern Radiobuttons
            writer.AddAttribute( "class", "control-group" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            writer.Write( "<span class='control-label'>Occurrence Pattern</span>" );
            writer.AddAttribute( "class", "controls" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _radSpecificDates.RenderControl( writer );
            _radDaily.RenderControl( writer );
            _radWeekly.RenderControl( writer );
            _radMonthly.RenderControl( writer );
            writer.RenderEndTag();
            writer.RenderEndTag();

            // Specific Date Panel
            writer.AddAttribute( "class", "recurrence-pattern-type control-group controls" );
            writer.AddAttribute( "id", "recurrence-pattern-specific-date" );

            if ( !_radSpecificDates.Checked )
            {
                writer.AddStyleAttribute( HtmlTextWriterStyle.Display, "none" );
            }

            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _hfSpecificDateListValues.RenderControl( writer );


            writer.Write( @"
                <ul id='lstSpecificDates'>" );

            foreach ( var dateValue in _hfSpecificDateListValues.Value.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ) )
            {
                writer.Write( "<li><span>" + dateValue + "</span><a href='#' style='display: none'><i class='icon-remove'></i></a></li>" );
            }

            writer.Write( @"
                </ul>
                <a class='btn btn-small' id='add-specific-date'><i class='icon-plus'></i>
                    <span> Add Date</span>
                </a>" );

            writer.AddAttribute( "id", "add-specific-date-group" );

            writer.AddStyleAttribute( HtmlTextWriterStyle.Display, "none" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _dpSpecificDate.RenderControl( writer );
            writer.Write( @"
                <a class='btn btn-primary btn-mini' id='add-specific-date-ok'></i>
                    <span>OK</span>
                </a>
                <a class='btn btn-mini' id='add-specific-date-cancel'></i>
                    <span>Cancel</span>
                </a>" );

            writer.RenderEndTag();
            writer.RenderEndTag();

            // daily recurrence panel
            writer.AddAttribute( "id", "recurrence-pattern-daily" );
            writer.AddAttribute( "class", "recurrence-pattern-type" );

            if ( !_radDaily.Checked )
            {
                writer.AddStyleAttribute( HtmlTextWriterStyle.Display, "none" );
            }

            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            writer.AddAttribute( "class", "control-group controls" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _radDailyEveryXDays.RenderControl( writer );
            writer.Write( "<span>Every </span>" );
            _tbDailyEveryXDays.RenderControl( writer );
            writer.Write( "<span> days</span>" );
            writer.RenderEndTag();

            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _radDailyEveryWeekday.RenderControl( writer );
            writer.Write( "<span>Every weekday</span>" );
            writer.RenderEndTag();

            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _radDailyEveryWeekendDay.RenderControl( writer );
            writer.Write( "<span>Every weekend day</span>" );
            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();

            // weekly recurrence panel
            writer.AddAttribute( "id", "recurrence-pattern-weekly" );
            writer.AddAttribute( "class", "recurrence-pattern-type" );

            if ( !_radWeekly.Checked )
            {
                writer.AddStyleAttribute( HtmlTextWriterStyle.Display, "none" );
            }

            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.AddAttribute( "class", "control-group controls" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            writer.Write( "<span>Every </span>" );
            _tbWeeklyEveryX.RenderControl( writer );
            writer.Write( "<span> weeks on</span></br>" );
            writer.RenderEndTag();

            writer.AddAttribute( "class", "control-group controls" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _cbWeeklySunday.RenderControl( writer );
            _cbWeeklyMonday.RenderControl( writer );
            _cbWeeklyTuesday.RenderControl( writer );
            _cbWeeklyWednesday.RenderControl( writer );
            _cbWeeklyThursday.RenderControl( writer );
            _cbWeeklyFriday.RenderControl( writer );
            _cbWeeklySaturday.RenderControl( writer );
            writer.RenderEndTag();

            writer.RenderEndTag();

            // monthly
            writer.AddAttribute( "id", "recurrence-pattern-monthly" );
            writer.AddAttribute( "class", "recurrence-pattern-type" );

            if ( !_radMonthly.Checked )
            {
                writer.AddStyleAttribute( HtmlTextWriterStyle.Display, "none" );
            }

            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            writer.AddAttribute( "class", "control-group controls" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _radMonthlyDayX.RenderControl( writer );
            writer.Write( "<span>Day </span>" );
            _tbMonthlyDayX.RenderControl( writer );
            writer.Write( "<span> of every </span>" );
            _tbMonthlyXMonths.RenderControl( writer );
            writer.Write( "<span> months</span>" );
            writer.RenderEndTag();

            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _radMonthlyNth.RenderControl( writer );
            writer.Write( "<span>The </span>" );
            _ddlMonthlyNth.RenderControl( writer );
            writer.Write( "<span> </span>" );
            _ddlMonthlyDayName.RenderControl( writer );
            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();

            // end date
            writer.Write( @"<div class='controls'><hr /></div>" );
            writer.AddAttribute( "class", "control-group" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            writer.Write( "<label class='control-label'>Continue Until</label>" );
            writer.AddAttribute( "class", "controls" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _radEndByNone.RenderControl( writer );
            writer.Write( "<span>No End</span>" );
            writer.RenderEndTag();

            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _radEndByDate.RenderControl( writer );
            writer.Write( "<span>End by </span>" );
            _dpEndBy.RenderControl( writer );
            writer.RenderEndTag();

            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _radEndByOccurrenceCount.RenderControl( writer );
            writer.Write( "<span>End after </span>" );
            _tbEndByOccurrenceCount.RenderControl( writer );
            writer.Write( "<span> occurrences</span>" );

            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();

            // exclusions
            writer.Write( @"<div class='controls'><hr /></div>" );
            writer.Write( @"<label class='control-label'>Exclusions</label>" );
            writer.AddAttribute( "id", "recurrence-pattern-exclusions" );
            writer.AddAttribute( "class", "control-group controls" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _hfExclusionDateRangeListValues.RenderControl( writer );

            writer.Write( @"
                <ul id='lstExclusionDateRanges'>" );

            foreach ( var dateRangeValue in _hfExclusionDateRangeListValues.Value.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ) )
            {
                writer.Write( "<li><span>" + dateRangeValue + "</span><a href='#' style='display: none'><i class='icon-remove'></i></a></li>" );
            }

            writer.Write( @"
                </ul>
                <a class='btn btn-small' id='add-exclusion-daterange'><i class='icon-plus'></i>
                    <span> Add Date Range</span>
                </a>" );

            writer.AddAttribute( "id", "add-exclusion-daterange-group" );
            writer.AddStyleAttribute( HtmlTextWriterStyle.Display, "none" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _dpExclusionDateStart.RenderControl( writer );
            writer.Write( "<span> to </span>" );
            _dpExclusionDateEnd.RenderControl( writer );
            writer.Write( @"
                <a class='btn btn-primary btn-mini' id='add-exclusion-daterange-ok'></i>
                    <span>OK</span>
                </a>
                <a class='btn btn-mini' id='add-exclusion-daterange-cancel'></i>
                    <span>Cancel</span>
                </a>" );

            writer.RenderEndTag();

            writer.RenderEndTag();

            // Recurrence Panel: End
            writer.RenderEndTag();

            // write out the closing divs that go after the recurrence panel
            writer.Write( @"
                            </div>
                        </div>
                    </div>
                </div>
            </div>
" );

            writer.AddAttribute( "class", "modal-footer" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _btnCancelSchedule.RenderControl( writer );
            _btnSaveSchedule.RenderControl( writer );
            writer.RenderEndTag();

            // write out the closing divs that go after the modal footer
            writer.Write( "</div>" );

            _smProxy.RenderControl( writer );
        }
    }
}
