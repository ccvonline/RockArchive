﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using Rock.Attribute;
using Rock.CheckIn;
using Rock.Model;

namespace Rock.Workflow.Action.CheckIn
{
    /// <summary>
    /// Loads the groups available for each location.
    /// TODO: Ask David about whether or not the "Load for all Locations" still makes sense since this now comes before Locations are selected.
    /// </summary>
    [Description("Loads the groups available for each selected (or optionally all) loction(s)")]
    [Export(typeof(ActionComponent))]
    [ExportMetadata( "ComponentName", "Load Groups" )]
    [BooleanField( "Load All", "By default groups are only loaded for the selected person, group type, and location.  Select this option to load groups for all the loaded people, group types, and locations." )]
    [BooleanField( "Load for all Locations", "If 'Load All' is not selected, this option can be used to load groups for all locations, including those for non-selected locations.", false, key: "IncludeNonSelectedLocations" )]
    public class LoadGroups : CheckInActionComponent
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
            bool loadAll = false;
            if ( bool.TryParse( GetAttributeValue( "LoadAll" ), out loadAll ) && loadAll )
            {
                loadAll = true;
            }

            //TBD  - no longer used.
            //bool loadAllLocations = false;
            //if ( bool.TryParse( GetAttributeValue( "IncludeNonSelectedLocations" ), out loadAllLocations ) && loadAllLocations )
            //{
            //    loadAllLocations = true;
            //}

            var checkInState = GetCheckInState( action, out errorMessages );
            if ( checkInState != null )
            {
                foreach ( var family in checkInState.CheckIn.Families.Where( f => f.Selected ) )
                {
                    foreach ( var person in family.People.Where( p => p.Selected || loadAll) )
                    {
                        foreach ( var groupType in person.GroupTypes.Where( g => g.Selected || loadAll) )
                        {
                            var kioskGroupType = checkInState.Kiosk.FilteredGroupTypes(checkInState.ConfiguredGroupTypes).Where( g => g.GroupType.Id == groupType.GroupType.Id ).FirstOrDefault();
                            if ( kioskGroupType != null )
                            {
                                foreach ( var kioskGroup in kioskGroupType.KioskGroups )
                                {
                                    if ( !groupType.Groups.Any( g => g.Group.Id == kioskGroup.Group.Id ) )
                                    {
                                        var checkInGroup = new CheckInGroup();
                                        checkInGroup.Group = kioskGroup.Group.Clone( false );
                                        checkInGroup.Group.CopyAttributesFrom( kioskGroup.Group );
                                        groupType.Groups.Add( checkInGroup );
                                    }
                                }
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