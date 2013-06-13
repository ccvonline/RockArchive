﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;

using Rock.CheckIn;
using Rock.Model;

namespace Rock.Workflow.Action.CheckIn
{
    /// <summary>
    /// Loads the group types allowed for each person in a family
    /// </summary>
    [Description( "Loads the group types allowed for each person in a family" )]
    [Export( typeof( ActionComponent ) )]
    [ExportMetadata( "ComponentName", "Attended Load Group Types" )]
    public class AttendedLoadGroupTypes : CheckInActionComponent
    {
        /// <summary>
        /// Executes the specified workflow.
        /// </summary>
        /// <param name="action">The workflow action.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool Execute( Model.WorkflowAction action, Data.IEntity entity, out List<string> errorMessages )
        {
            
            // This seems to only pull the top level ministry (Creativity, KidSpring, whatever), and doesn't go all the way down to the room level. So this needs to be changed
            // to pull from something like CurrentRoomGroupTypes or something like that.

            var checkInState = GetCheckInState( action, out errorMessages );
            if ( checkInState != null )
            {
                foreach ( var family in checkInState.CheckIn.Families.Where( f => f.Selected ) )
                {
                    foreach ( var person in family.People )
                    {
                        foreach ( var kioskGroupType in checkInState.Kiosk.KioskGroupTypes.Where( g => g.KioskLocations.Any( l => l.Location.IsActive ) ) )
                        {
                            if ( !person.GroupTypes.Any( g => g.GroupType.Id == kioskGroupType.GroupType.Id ) )
                            {
                                var checkinGroupType = new CheckInGroupType();
                                checkinGroupType.GroupType = kioskGroupType.GroupType.Clone( false );
                                checkinGroupType.GroupType.CopyAttributesFrom( kioskGroupType.GroupType );
                                person.GroupTypes.Add( checkinGroupType );
                            }
                        }
                    }
                }

                SetCheckInState( action, checkInState );
                return true;
            }

            return false;
        }
    }
}
