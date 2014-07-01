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
using System.ComponentModel.Composition;
using System.Linq;

using Rock.Attribute;
using Rock.Data;

namespace Rock.Workflow.Action.CheckIn
{
    /// <summary>
    /// Removes the location's "special needs" groups for each selected family member
    /// if the person is not "special needs".  The filter can ALSO be configured to 
    /// remove normal (non-special needs) groups when the person is "special needs".
    /// </summary>
    [Description( "Removes the groups for each selected family member that are not specific to their special needs attribute." )]
    [Export( typeof( ActionComponent ) )]
    [ExportMetadata( "ComponentName", "Filter Groups By Special Needs" )]
    [BooleanField( "Remove Special Needs Groups", "If set to true, special-needs groups will be removed if the person is NOT special needs. This basically prevents non-special-needs kids from getting put into special needs classes.  Default true.", true, key: "RemoveSpecialNeedsGroups" )]
    [BooleanField( "Remove Non-Special Needs Groups", "If set to true, non-special-needs groups will be removed if the person is special needs.  This basically prevents special needs kids from getting put into regular classes.  Default false.", false, key: "RemoveNonSpecialNeedsGroups" )]
    public class FilterGroupsBySpecialNeeds : CheckInActionComponent
    {
        /// <summary>
        /// Executes the specified workflow.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="action">The workflow action.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool Execute( RockContext rockContext, Model.WorkflowAction action, Object entity, out List<string> errorMessages )
        {
            var checkInState = GetCheckInState( entity, out errorMessages );
            if ( checkInState == null )
            {
                return false;
            }

            bool removeSNGroups = bool.Parse( GetAttributeValue( action, "RemoveSpecialNeedsGroups" ) ?? "true" );
            bool removeNonSNGroups = bool.Parse( GetAttributeValue( action, "RemoveNonSpecialNeedsGroups" ) ?? "false" );

            var family = checkInState.CheckIn.Families.FirstOrDefault( f => f.Selected );
            if ( family != null )
            {
                foreach ( var person in family.People.Where( p => p.Selected ) )
                {
                    bool isSNPerson = bool.Parse( person.Person.GetAttributeValue( "IsSpecialNeeds" ) ?? "false" );
                    foreach ( var groupType in person.GroupTypes.Where( g => g.Selected ).ToList() )
                    {
                        foreach ( var group in groupType.Groups.ToList() )
                        {
                            bool isSNGroup = bool.Parse( group.Group.GetAttributeValue( "IsSpecialNeeds" ) ?? "false" );

                            // If the group is special needs but the person is not, then remove it.
                            if ( removeSNGroups && isSNGroup && !( isSNPerson ) )
                            {
                                groupType.Groups.Remove( group );
                            }

                            // or if the setting is enabled and the person is SN but the group is not.
                            if ( removeNonSNGroups && isSNPerson && !isSNGroup )
                            {
                                groupType.Groups.Remove( group );
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}