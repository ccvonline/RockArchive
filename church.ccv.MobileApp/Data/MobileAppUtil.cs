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
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using System.Data.Entity;
using church.ccv.MobileApp.Models;

namespace church.ccv.MobileApp
{
    public static class MobileAppUtil
    {
        // the workflow type id for the alert note re-route
        // ProductionId = 166
        const int AlertNoteReReouteWorkflowId = 1165;

        // the attribute Id for the Mobile App's version
        const int MobileAppVersionAttributeId = 29543;

        public static LaunchData GetLaunchData( )
        {
            RockContext rockContext = new RockContext( );

            LaunchData launchData = new LaunchData( );

            // setup the campuses
            launchData.Campuses = new List<Campus>( );
            foreach ( CampusCache campus in CampusCache.All( false ) )
            {
                Campus campusModel = new Campus( );
                campusModel.Guid = campus.Guid;
                campusModel.Id = campus.Id;
                campusModel.Name = campus.Name;
                launchData.Campuses.Add( campusModel );
            }
            
            // setup the prayer categories
            launchData.PrayerCategories = new List<KeyValuePair<string, int>>( );
            CategoryCache prayerCategories = CategoryCache.Read( 1 );
            foreach( CategoryCache category in prayerCategories.Categories )
            {
                launchData.PrayerCategories.Add( new KeyValuePair<string, int>( category.Name, category.Id ) );
            }
                        
            // get the latest mobile app version
            var mobileAppAttribute = new AttributeValueService( rockContext ).Queryable( ).Where( av => av.AttributeId == MobileAppVersionAttributeId ).SingleOrDefault( );
            int.TryParse( mobileAppAttribute.Value, out launchData.MobileAppVersion );
            
            return launchData;
        }

        public static bool AddPersonToGroup( GroupRegModel regModel )
        {
            bool success = false;

            // setup all variables we'll need
            var rockContext = new RockContext();
            var personService = new PersonService( rockContext );
            var groupService = new GroupService( rockContext );

            DefinedValueCache connectionStatusPending = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_CONNECTION_STATUS_WEB_PROSPECT );
            DefinedValueCache recordStatusPending = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_PENDING );
            DefinedValueCache homeAddressType = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME );
            
            Person person = null;
            Person spouse = null;
            Group family = null;
            GroupLocation homeLocation = null;

            // setup history tracking
            var changes = new List<string>();
            var familyChanges = new List<string>();

            // first, get the group the person wants to join
            Group requestedGroup = groupService.Get( regModel.RequestedGroupId );
            if ( requestedGroup != null )
            {
               // Try to find person by name/email 
                var matches = personService.GetByMatch( regModel.FirstName.Trim(), regModel.LastName.Trim(), regModel.Email.Trim() );
                if ( matches.Count() == 1 )
                {
                    person = matches.First();
                }

                // Check to see if this is a new person
                if ( person == null )
                {
                    // If so, create the person and family record for the new person
                    person = new Person();
                    person.FirstName = regModel.FirstName.Trim();
                    person.LastName = regModel.LastName.Trim();
                    person.Email = regModel.Email.Trim();
                    person.IsEmailActive = true;
                    person.EmailPreference = EmailPreference.EmailAllowed;
                    person.RecordTypeValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_TYPE_PERSON.AsGuid() ).Id;
                    person.ConnectionStatusValueId = connectionStatusPending.Id;
                    person.RecordStatusValueId = recordStatusPending.Id;
                    person.Gender = Gender.Unknown;

                    family = PersonService.SaveNewPerson( person, rockContext, requestedGroup.CampusId, false );
                }
                else
                {
                    // updating existing person
                    History.EvaluateChange( changes, "Email", person.Email, regModel.Email );
                    person.Email = regModel.Email;

                    // Get the current person's families
                    var families = person.GetFamilies( rockContext );

                    // look for first family with a home location
                    foreach ( var aFamily in families )
                    {
                        homeLocation = aFamily.GroupLocations
                            .Where( l =>
                                l.GroupLocationTypeValueId == homeAddressType.Id &&
                                l.IsMappedLocation )
                            .FirstOrDefault();
                        if ( homeLocation != null )
                        {
                            family = aFamily;
                            break;
                        }
                    }

                    // If a family wasn't found with a home location, use the person's first family
                    if ( family == null )
                    {
                        family = families.FirstOrDefault();
                    }
                }

                // if provided, store their phone number
                if ( string.IsNullOrWhiteSpace( regModel.Phone ) == false )
                {
                    DefinedValueCache mobilePhoneType = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_MOBILE );
                    person.UpdatePhoneNumber( mobilePhoneType.Id, PhoneNumber.DefaultCountryCode(), regModel.Phone, null, null, rockContext );
                }


                // Check for a spouse. 
                if ( string.IsNullOrWhiteSpace( regModel.SpouseName ) == false )
                {
                    // this is super simple. get the person's spouse
                    Person tempSpouse = person.GetSpouse();

                    // if they have one...
                    if ( tempSpouse != null )
                    {
                        // split out the first and last name provided
                        string[] spouseName = regModel.SpouseName.Split( ' ' );
                        string spouseFirstName = spouseName[0];
                        string spouseLastName = spouseName.Count() > 1 ? spouseName[1] : "";

                        // we'll take them as a spouse if they match the provided name
                        if ( tempSpouse.FirstName.Equals( spouseFirstName, StringComparison.OrdinalIgnoreCase ) )
                        {
                            // if there was no last name, or it matches, we're good.
                            if ( string.IsNullOrWhiteSpace( spouseLastName ) == true ||
                                tempSpouse.LastName.Equals( spouseLastName, StringComparison.OrdinalIgnoreCase ) )
                            {
                                spouse = tempSpouse;
                            }
                        }
                    }
                }

                // Save all changes
                rockContext.SaveChanges();

                HistoryService.SaveChanges( rockContext, typeof( Person ),
                    Rock.SystemGuid.Category.HISTORY_PERSON_DEMOGRAPHIC_CHANGES.AsGuid(), person.Id, changes );

                HistoryService.SaveChanges( rockContext, typeof( Person ),
                    Rock.SystemGuid.Category.HISTORY_PERSON_FAMILY_CHANGES.AsGuid(), person.Id, familyChanges );


                // now, it's time to either add them to the group, or kick off the Alert Re-Route workflow
                // (Or nothing if there's no problem but they're already in the group)
                GroupMember primaryGroupMember = PersonToGroupMember( rockContext, person, requestedGroup );

                GroupMember spouseGroupMember = null;
                if ( spouse != null )
                {
                    spouseGroupMember = PersonToGroupMember( rockContext, spouse, requestedGroup );
                }

                // prep the workflow service
                var workflowTypeService = new WorkflowTypeService( rockContext );

                bool addToGroup = true;

                // First, check to see if an alert re-route workflow should be launched
                WorkflowType alertRerouteWorkflowType = workflowTypeService.Get( AlertNoteReReouteWorkflowId );

                // do either of the people registering have alert notes?
                int alertNoteCount = new NoteService( rockContext ).Queryable().Where( n => n.EntityId == person.Id && n.IsAlert == true ).Count();

                if ( spouse != null )
                {
                    alertNoteCount += new NoteService( rockContext ).Queryable().Where( n => n.EntityId == spouse.Id && n.IsAlert == true ).Count();
                }

                if ( alertNoteCount > 0 )
                {
                    // yes they do. so first, flag that we should NOT put them in the group
                    addToGroup = false;

                    // and kick off the re-route workflow so security can review.
                    LaunchWorkflow( rockContext, alertRerouteWorkflowType, primaryGroupMember );

                    if ( spouseGroupMember != null )
                    {
                        LaunchWorkflow( rockContext, alertRerouteWorkflowType, spouseGroupMember );
                    }
                }

                // if above, we didn't flag that they should not join the group, let's add them
                if ( addToGroup == true )
                {
                    // try to add them to the group (would only fail if the're already in it)
                    TryAddGroupMemberToGroup( rockContext, primaryGroupMember, requestedGroup );

                    if ( spouseGroupMember != null )
                    {
                        TryAddGroupMemberToGroup( rockContext, spouseGroupMember, requestedGroup );
                    }
                }

                // if we mae it here, all is good!
                success = true;
            }

            return success;
        }

        private static GroupMember PersonToGroupMember( RockContext rockContext, Person person, Group group )
        {
            // puts a person into a group member object, so that we can pass it to a workflow
            GroupMember newGroupMember = new GroupMember();
            newGroupMember.PersonId = person.Id;
            newGroupMember.GroupRoleId = group.GroupType.DefaultGroupRole.Id;
            newGroupMember.GroupMemberStatus = GroupMemberStatus.Pending;
            newGroupMember.GroupId = group.Id;

            return newGroupMember;
        }

        /// <summary>
        /// Adds the group member to the group if they aren't already in it
        /// </summary>
        private static void TryAddGroupMemberToGroup( RockContext rockContext, GroupMember newGroupMember, Group group )
        {
            if ( !group.Members.Any( m => 
                                      m.PersonId == newGroupMember.PersonId &&
                                      m.GroupRoleId == group.GroupType.DefaultGroupRole.Id ) )
            {
                var groupMemberService = new GroupMemberService(rockContext);
                groupMemberService.Add( newGroupMember );
                    
                rockContext.SaveChanges();
            }
        }

        private static void LaunchWorkflow( RockContext rockContext, WorkflowType workflowType, GroupMember groupMember )
        {
            try
            {
                List<string> workflowErrors;
                var workflow = Workflow.Activate( workflowType, workflowType.Name );
                new WorkflowService( rockContext ).Process( workflow, groupMember, out workflowErrors );
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogException( ex, null );
            }
        }
    }
}
