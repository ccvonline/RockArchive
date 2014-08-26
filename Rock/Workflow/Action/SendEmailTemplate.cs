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
using Rock.Communication;
using Rock.Model;
using Rock.Web.Cache;

namespace Rock.Workflow.Action
{
    /// <summary>
    /// Sends email
    /// </summary>
    [Description( "Email the configured recipient the name of the thing being operated against." )]
    [Export(typeof(ActionComponent))]
    [ExportMetadata("ComponentName", "Send Email Template")]

    [EmailTemplateField( "EmailTemplate", "The email template to send. The email templates must be assigned to the 'Workflow' category in order to be displayed on the list." )]
    [WorkflowTextOrAttribute( "Send To Email Address", "Attribute Value", "The email address or an attribute that contains the person or email address that email should be sent to", true, "", "", 1, "Recipient" )]
    public class SendEmailTemplate : ActionComponent
    {
        /// <summary>
        /// Executes the specified workflow.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="action">The action.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        public override bool Execute( RockContext rockContext, WorkflowAction action, Object entity, out List<string> errorMessages )
        {
            errorMessages = new List<string>();

            var mergeFields = GetMergeFields( action );
            var recipients = new Dictionary<string, Dictionary<string, object>>();

            string to = GetAttributeValue( action, "To" );

            Guid? guid = to.AsGuidOrNull();
            if ( guid.HasValue )
            {
                var attribute = AttributeCache.Read( guid.Value, rockContext );
                if ( attribute != null )
                {
                    string toValue = action.GetWorklowAttributeValue( guid.Value );
                    if ( !string.IsNullOrWhiteSpace( toValue ) )
                    {
                        switch ( attribute.FieldType.Class )
                        {
                            case "Rock.Field.Types.TextFieldType":
                                {
                                    recipients.Add( toValue, mergeFields );
                                    break;
                                }
                            case "Rock.Field.Types.PersonFieldType":
                                {
                                    Guid personAliasGuid = toValue.AsGuid();
                                    if ( !personAliasGuid.IsEmpty() )
                                    {
                                        var person = new PersonAliasService( rockContext ).Queryable()
                                            .Where( a => a.Guid.Equals( personAliasGuid ) )
                                            .Select( a => a.Person )
                                            .FirstOrDefault();
                                        if ( person == null )
                                        {
                                            action.AddLogEntry( "Invalid Recipient: Person not found", true );
                                        }
                                        else if ( string.IsNullOrWhiteSpace( person.Email ) )
                                        {
                                            action.AddLogEntry( "Email was not sent: Recipient does not have an email address", true );
                                        }
                                        else if ( !( person.IsEmailActive ?? true ) )
                                        {
                                            action.AddLogEntry( "Email was not sent: Recipient email is not active", true );
                                        }
                                        else if ( person.EmailPreference == EmailPreference.DoNotEmail )
                                        {
                                            action.AddLogEntry( "Email was not sent: Recipient has requested 'Do Not Email'", true );
                                        }
                                        else
                                        {
                                            var personDict = new Dictionary<string, object>( mergeFields );
                                            personDict.Add( "Person", person );
                                            recipients.Add( person.Email, personDict );
                                        }
                                    }
                                    break;
                                }
                            case "Rock.Field.Types.GroupFieldType":
                                {
                                    int? groupId = toValue.AsIntegerOrNull();
                                    if ( !groupId.HasValue )
                                    {
                                        foreach ( var person in new GroupMemberService( rockContext )
                                            .GetByGroupId( groupId.Value )
                                            .Where( m => m.GroupMemberStatus == GroupMemberStatus.Active )
                                            .Select( m => m.Person ) )
                                        {
                                            if ( ( person.IsEmailActive ?? true ) &&
                                                person.EmailPreference != EmailPreference.DoNotEmail &&
                                                !string.IsNullOrWhiteSpace( person.Email ) )
                                            {
                                                var personDict = new Dictionary<string, object>( mergeFields );
                                                personDict.Add( "Person", person );
                                                recipients.Add( person.Email, personDict );
                                            }
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                recipients.Add( to, mergeFields );
            }

            if ( recipients.Any() )
            {
                Email.Send( GetAttributeValue( action, "EmailTemplate" ).AsGuid(), recipients );
            }

            return true;
        }
    }
}