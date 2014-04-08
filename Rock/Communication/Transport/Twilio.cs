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
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

using Twilio;

namespace Rock.Communication.Transport
{
    /// <summary>
    /// Communication transport for sending SMS messages using Twilio
    /// </summary>
    [Description( "Sends a communication through Twilio API" )]
    [Export( typeof( TransportComponent ) )]
    [ExportMetadata( "ComponentName", "Twilio" )]
    [TextField( "SID", "Your Twilio Account SID (find at https://www.twilio.com/user/account)", true, "", "", 0 )]
    [TextField( "Token", "Your Twilio Account Token", true, "", "", 1 )]
    public class Twilio : TransportComponent
    {
        /// <summary>
        /// Sends the specified communication.
        /// </summary>
        /// <param name="communication">The communication.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Send( Rock.Model.Communication communication )
        {
            var rockContext = new RockContext();

            // Requery the Communication
            communication = new CommunicationService( rockContext ).Get( communication.Id );

            if ( communication != null &&
                communication.Status == Model.CommunicationStatus.Approved &&
                communication.Recipients.Where( r => r.Status == Model.CommunicationRecipientStatus.Pending ).Any() &&
                ( !communication.FutureSendDateTime.HasValue || communication.FutureSendDateTime.Value.CompareTo( RockDateTime.Now ) > 0 ) )
            {
                string fromPhone = string.Empty;
                string fromValue = communication.GetChannelDataValue( "FromValue" );
                int fromValueId = int.MinValue;
                if ( int.TryParse( fromValue, out fromValueId ) )
                {
                    fromPhone = DefinedValueCache.Read( fromValueId ).Name;
                }

                if ( !string.IsNullOrWhiteSpace( fromPhone ) )
                {
                    string accountSid = GetAttributeValue( "SID" );
                    string authToken = GetAttributeValue( "Token" );
                    var twilio = new TwilioRestClient( accountSid, authToken );

                    var recipientService = new CommunicationRecipientService( rockContext );

                    var globalConfigValues = GlobalAttributesCache.GetMergeFields( null );

                    bool recipientFound = true;
                    while ( recipientFound )
                    {
                        var recipient = recipientService.Get( communication.Id, CommunicationRecipientStatus.Pending ).FirstOrDefault();
                        if ( recipient != null )
                        {
                            try
                            {
                                var phoneNumbers = recipient.Person.PhoneNumbers
                                    .Where( p => 
                                        p.IsMessagingEnabled &&
                                        p.Number != null &&
                                        p.Number != "" )
                                    .ToList();

                                if (phoneNumbers.Any())
                                {
                                    // Create merge field dictionary
                                    var mergeObjects = MergeValues( globalConfigValues, recipient );
                                    string message = communication.GetChannelDataValue( "Message" );
                                    message = message.ResolveMergeFields( mergeObjects );

                                    foreach ( var phoneNumber in phoneNumbers )
                                    {
                                        string twillioNumber = phoneNumber.Number;
                                        if ( !string.IsNullOrWhiteSpace( phoneNumber.CountryCode ) )
                                        {
                                            twillioNumber = "+" + phoneNumber.CountryCode + phoneNumber.Number;
                                        }

                                        twilio.SendMessage( fromPhone, twillioNumber, message );
                                    }

                                    recipient.Status = CommunicationRecipientStatus.Delivered;

                                }
                                else
                                {
                                    recipient.Status = CommunicationRecipientStatus.Failed;
                                    recipient.StatusNote = "No Phone Number with Messaging Enabled";
                                }
                            }
                            catch ( Exception ex )
                            {
                                recipient.Status = CommunicationRecipientStatus.Failed;
                                recipient.StatusNote = "Twilio Exception: " + ex.Message;
                            }

                            rockContext.SaveChanges();
                        }
                        else
                        {
                            recipientFound = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sends the specified template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="recipients">The recipients.</param>
        /// <param name="appRoot"></param>
        /// <param name="themeRoot"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Send( SystemEmail template, Dictionary<string, Dictionary<string, object>> recipients, string appRoot, string themeRoot )
        {
            throw new NotImplementedException();
        }

    }
}
