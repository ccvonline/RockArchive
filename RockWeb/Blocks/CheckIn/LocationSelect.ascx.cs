﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.CheckIn;
using Rock.Model;

namespace RockWeb.Blocks.CheckIn
{
    [Description( "Check-In Location Select block" )]
    public partial class LocationSelect : CheckInBlock
    {
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( CurrentWorkflow == null || CurrentCheckInState == null )
            {
                GoToWelcomePage();
            }
            else
            {
                if ( !Page.IsPostBack )
                {
                    var family = CurrentCheckInState.CheckIn.Families.Where( f => f.Selected ).FirstOrDefault();
                    if ( family != null )
                    {
                        var person = family.People.Where( p => p.Selected ).FirstOrDefault();
                        if ( person != null )
                        {
                            var groupType = person.GroupTypes.Where( g => g.Selected ).FirstOrDefault();
                            if ( groupType != null )
                            {
                                lGroupTypeName.Text = groupType.ToString();

                                if ( groupType.Locations.Count == 1 )
                                {
                                    if ( UserBackedUp )
                                    {
                                        GoBack();
                                    }
                                    else
                                    {
                                        foreach ( var location in groupType.Locations )
                                        {
                                            location.Selected = true;
                                        }

                                        ProcessSelection();
                                    }
                                }
                                else
                                {
                                    int? defaultId = groupType.Locations.OrderByDescending( l => l.LastCheckIn ).Select( l => l.Location.Id ).FirstOrDefault();
                                    foreach ( var location in groupType.Locations )
                                    {
                                        ListItem item = new ListItem( location.ToString(), location.Location.Id.ToString() );
                                        if ( defaultId.HasValue && location.Location.Id == defaultId.Value )
                                        {
                                            item.Selected = true;
                                        }

                                        lbLocations.Items.Add( item );
                                    }
                                }
                            }
                            else
                            {
                                GoBack();
                            }
                        }
                        else
                        {
                            GoBack();
                        }
                    }
                    else
                    {
                        GoBack();
                    }
                }
            }
        }

        protected void lbSelect_Click( object sender, EventArgs e )
        {
            if ( KioskCurrentlyActive )
            {
                if ( lbLocations.SelectedItem != null )
                {
                    var family = CurrentCheckInState.CheckIn.Families.Where( f => f.Selected ).FirstOrDefault();
                    if ( family != null )
                    {
                        var person = family.People.Where( p => p.Selected ).FirstOrDefault();
                        if ( person != null )
                        {
                            var groupType = person.GroupTypes.Where( g => g.Selected ).FirstOrDefault();
                            if ( groupType != null )
                            {
                                int id = Int32.Parse( lbLocations.SelectedItem.Value );
                                var location = groupType.Locations.Where( l => l.Location.Id == id ).FirstOrDefault();
                                if ( location != null )
                                {
                                    location.Selected = true;
                                    ProcessSelection();
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void lbBack_Click( object sender, EventArgs e )
        {
            GoBack();
        }

        protected void lbCancel_Click( object sender, EventArgs e )
        {
            CancelCheckin();
        }

        private void GoBack()
        {
            foreach ( var family in CurrentCheckInState.CheckIn.Families )
            {
                foreach( var person in family.People)
                {
                    foreach ( var groupType in person.GroupTypes )
                    {
                        groupType.Selected = false;
                        groupType.Locations = new List<CheckInLocation>();
                    }
                }
            }

            SaveState();

            GoToPersonSelectPage( true );
        }

        private void ProcessSelection()
        {
            var errors = new List<string>();
            if ( ProcessActivity( "Group Search", out errors ) )
            {
                SaveState();
                GoToGroupSelectPage();
            }
            else
            {
                string errorMsg = "<ul><li>" + errors.AsDelimited( "</li><li>" ) + "</li></ul>";
                maWarning.Show( errorMsg, Rock.Web.UI.Controls.ModalAlertType.Warning );
            }
        }


    }
}