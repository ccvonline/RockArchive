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
using Rock.Communication;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

namespace Rock.Workflow.Action
{
    /// <summary>
    /// Sends email
    /// </summary>
    [Description( "Sends an email.  The recipient can either be a person or email address determined by the 'To Attribute' value, or an email address entered in the 'To' field." )]
    [Export( typeof( ActionComponent ) )]
    [ExportMetadata( "ComponentName", "Send Email" )]

    [WorkflowTextOrAttribute("Send To Email Address", "Attribute Value", "The email address or an attribute that contains the person or email address that email should be sent to", true, "", "", 0, "To")]
    [TextField( "From", "The From address that email should be sent from  (will default to organization email).", false, "", "", 1 )]
    [TextField( "Subject", "The subject that should be used when sending email.", false, "", "", 2 )]
    [CodeEditorField( "Body", "The body of the email that should be sent", Web.UI.Controls.CodeEditorMode.Html, Web.UI.Controls.CodeEditorTheme.Rock, 200, false, "", "", 3 )]
    public class SendEmail : ActionComponent
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

            var recipients = new List<string>();

            string nameValue = GetAttributeValue( action, "To" );
            Guid guid = nameValue.AsGuid();
            if ( !guid.IsEmpty() )
            {
                var attribute = AttributeCache.Read( guid );
                if ( attribute != null )
                {
                    string toValue = action.GetWorklowAttributeValue( guid );
                    if ( !string.IsNullOrWhiteSpace( toValue ) )
                    {
                        switch ( attribute.FieldType.Class )
                        {
                            case "Rock.Field.Types.TextFieldType":
                                {
                                    recipients.Add( toValue );
                                    break;
                                }
                            case "Rock.Field.Types.PersonFieldType":
                                {
                                    Guid personAliasGuid = toValue.AsGuid();
                                    if ( !personAliasGuid.IsEmpty() )
                                    {
                                        string to = new PersonAliasService( new RockContext() ).Queryable()
                                            .Where( a => a.Guid.Equals( personAliasGuid ) )
                                            .Select( a => a.Person.Email )
                                            .FirstOrDefault();
                                        if ( !string.IsNullOrWhiteSpace( to ) )
                                        {
                                            recipients.Add( to );
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
                string to = GetAttributeValue( action, "To" );
                if ( !string.IsNullOrWhiteSpace( to ) )
                {
                    recipients.Add( to );
                }
            }

            if ( recipients.Any() )
            {
                var mergeFields = GetMergeFields( action );

                var channelData = new Dictionary<string, string>();
                channelData.Add( "From", GetAttributeValue( action, "From" ) );
                channelData.Add( "Subject", GetAttributeValue( action, "Subject" ).ResolveMergeFields( mergeFields ) );

                string body = GetAttributeValue( action, "Body" ).ResolveMergeFields( mergeFields );
                channelData.Add( "Body", System.Text.RegularExpressions.Regex.Replace( body, @"\[\[\s*UnsubscribeOption\s*\]\]", string.Empty ) );

                var channelEntity = EntityTypeCache.Read( Rock.SystemGuid.EntityType.COMMUNICATION_CHANNEL_EMAIL.AsGuid() );
                if ( channelEntity != null )
                {
                    var channel = ChannelContainer.GetComponent( channelEntity.Name );
                    if ( channel != null && channel.IsActive )
                    {
                        var transport = channel.Transport;
                        if ( transport != null && transport.IsActive )
                        {
                            var appRoot = GlobalAttributesCache.Read().GetValue( "InternalApplicationRoot" );
                            transport.Send( channelData, recipients, appRoot, string.Empty );
                        }
                    }
                }
            }

            return true;
        }
    }
}