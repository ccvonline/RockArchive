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
using Rock.Model;
using Rock.Security;

namespace RockWeb.Blocks.Utility
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Person Profile" )]
    [Category( "Check-in > Manager" )]
    [Description( "Displays person and details about recent check-ins." )]
    public partial class Stark : Rock.Web.UI.RockBlock
    {
        #region Fields

        // used for private variables

        #endregion

        #region Properties

        // used for public / protected properties

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
                if ( IsUserAuthorized( Authorization.VIEW ) )
                {
                    Guid? personGuid = PageParameter( "Person" ).AsGuidOrNull();
                    if ( personGuid.HasValue )
                    {
                        ShowDetail( personGuid.Value );
                    }
                }
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

        #endregion

        #region Methods

        private void ShowDetail(Guid personGuid)
        {
            var rockContext = new RockContext();
            var personService = new PersonService( new RockContext() );
            
            var person = personService.Get( personGuid );

            if (person != null)
            {
                lName.Text = person.FullName;

                string photoTag = Person.GetPhotoImageTag( person, 120, 120 );
                if ( person.PhotoId.HasValue )
                {
                    lPhoto.Text = string.Format( "<a href='{0}'>{1}</a>", person.PhotoUrl, photoTag );
                }
                else
                {
                    lPhoto.Text = photoTag;
                }

                lEmail.Visible = !string.IsNullOrWhiteSpace( person.Email );
                lEmail.Text = person.GetEmailTag( ResolveRockUrl( "/" ) );

                var childGuid = Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_CHILD.AsGuid();
                var isFamilyChild = new Dictionary<int, bool>();

                var allFamilyMembers = person.GetFamilyMembers( true ).ToList();
                allFamilyMembers.Where( m => m.PersonId == person.Id ).ToList().ForEach(
                    m => isFamilyChild.Add( m.GroupId, m.GroupRole.Guid.Equals( childGuid ) ) );

                string urlRoot = Request.Url.ToString().ReplaceCaseInsensitive( personGuid.ToString(), "" );

                var familyMembers = allFamilyMembers.Where( m => m.PersonId != person.Id )
                    .OrderBy( m => m.GroupId )
                    .ThenBy( m => m.Person.BirthDate )
                    .Select( m => new
                    {
                        Url = urlRoot + m.Person.Guid.ToString(),
                        FullName = m.Person.FullName,
                        Note = isFamilyChild[m.GroupId] ?
                            ( m.GroupRole.Guid.Equals( childGuid ) ? " (Sibling)" : "(Parent)" ) :
                            ( m.GroupRole.Guid.Equals( childGuid ) ? " (Child)" : "" )
                    } )
                    .ToList();

                rcwFamily.Visible = familyMembers.Any();
                rptrFamily.DataSource = familyMembers;
                rptrFamily.DataBind();

                rptrPhones.DataSource = person.PhoneNumbers.Where( p => !p.IsUnlisted ).ToList();
                rptrPhones.DataBind();

                var schedules = new ScheduleService( rockContext ).Queryable()
                        .Where( s => s.CheckInStartOffsetMinutes.HasValue ) 
                        .Select( s => s.Id)
                        .ToList();

                var startDate = RockDateTime.Now.AddYears(-2);

                var attendance = new AttendanceService( rockContext )
                    .Queryable( "Schedule,Group,Location" )
                    .Where( a =>
                        a.PersonId.HasValue &&
                        a.PersonId == person.Id &&
                        a.ScheduleId.HasValue &&
                        a.GroupId.HasValue &&
                        a.LocationId.HasValue &&
                        a.StartDateTime > startDate &&
                        a.DidAttend &&
                        schedules.Contains( a.ScheduleId.Value ) )
                    .OrderByDescending( a =>
                        a.StartDateTime )
                    .Select( a => new
                    {
                        Date = a.StartDateTime,
                        Group = a.Group.Name,
                        Location = a.Location.Name,
                        Schedule = a.Schedule.Name
                    } ).ToList();

                rcwCheckinHistory.Visible = attendance.Any();
                gHistory.DataSource = attendance;
                gHistory.DataBind();
            }
        }

        #endregion
    }
}