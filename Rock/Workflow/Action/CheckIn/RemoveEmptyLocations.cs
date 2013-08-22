﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;

namespace Rock.Workflow.Action.CheckIn
{
    /// <summary>
    /// Removes locations (from groups) if the location has no schedule.
    /// </summary>
    [Description( "Removes locations (from groups) if the location has no schedule." )]
    [Export(typeof(ActionComponent))]
    [ExportMetadata( "ComponentName", "Remove Empty Locations" )]
    public class RemoveEmptyLocations : CheckInActionComponent
    {
        /// <summary>
        /// Executes the specified workflow.
        /// </summary>
        /// <param name="action">The workflow action.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool Execute( Model.WorkflowAction action, Object entity, out List<string> errorMessages )
        {
            var checkInState = GetCheckInState( entity, out errorMessages );
            if ( checkInState != null )
            {
                foreach ( var family in checkInState.CheckIn.Families.ToList() )
                {
                    foreach ( var person in family.People.ToList() )
                    {
                        foreach ( var groupType in person.GroupTypes.ToList() )
                        {
                            foreach ( var group in groupType.Groups.ToList() )
                            {
                                foreach ( var location in group.Locations.ToList() )
                                {
                                    if ( location.Schedules.Count == 0 )
                                    {
                                        group.Locations.Remove( location );
                                    }
                                }
                            }
                        }
                    }
                }

                return true;

            }

            return false;
        }
    }
}