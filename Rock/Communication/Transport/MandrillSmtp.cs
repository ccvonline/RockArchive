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
using System.Net.Mime;
using System.Text.RegularExpressions;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

namespace Rock.Communication.Transport
{
    /// <summary>
    /// Sends a communication through SMTP protocol
    /// </summary>
    [Description( "Sends a communication through Mandrill's SMTP API" )]
    [Export( typeof( TransportComponent ) )]
    [ExportMetadata( "ComponentName", "Mandrill SMTP" )]
    [TextField( "Username", "The SMTP username provided by Mandrill", true, "", "", 1 )]
    [TextField( "Password", "Any valid Mandrill API key (not the password you use to login to Mandrill)", true, "", "", 2 )]
    [BooleanField( "Use SSL", "", false, "", 3 )]
    [BooleanField( "Inline CSS", "Enable Mandrill's CSS Inliner feature.", true, "", 4 )]
    public class MandrillSmtp : TransportComponent
    {
        string serverName = "smtp.mandrillapp.com";
        int serverPort = 25;
        bool inlineCss = true;

        /// <summary>
        /// Gets a value indicating whether transport has ability to track recipients opening the communication.
        /// </summary>
        /// <value>
        /// <c>true</c> if transport can track opens; otherwise, <c>false</c>.
        /// </value>
        public override bool CanTrackOpens
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Sends the specified communication.
        /// </summary>
        /// <param name="communication">The communication.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Send( Rock.Model.Communication communication )
        {
            var rockContext = new RockContext();

            if ( GetAttributeValue( "UseSSL" ).AsBoolean() )
            {
                serverPort = 465;
            }

            inlineCss = bool.TryParse( GetAttributeValue( "InlineCSS" ), out inlineCss ) && inlineCss;


            // Requery the Communication object
            communication = new CommunicationService( rockContext ).Get( communication.Id );

            if ( communication != null &&
                communication.Status == Model.CommunicationStatus.Approved &&
                communication.Recipients.Where( r => r.Status == Model.CommunicationRecipientStatus.Pending ).Any() &&
                ( !communication.FutureSendDateTime.HasValue || communication.FutureSendDateTime.Value.CompareTo( RockDateTime.Now ) <= 0 ) )
            {
                var globalAttributes = Rock.Web.Cache.GlobalAttributesCache.Read();

                string fromAddress = communication.GetChannelDataValue( "FromAddress" );
                string replyTo = communication.GetChannelDataValue( "ReplyTo" );

                // Check to make sure sending domain is a safe sender
                var safeDomains = globalAttributes.GetValue( "SafeSenderDomains" ).SplitDelimitedValues().ToList();
                var emailParts = fromAddress.Split( new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries );
                if ( emailParts.Length != 2 || !safeDomains.Contains( emailParts[1], StringComparer.OrdinalIgnoreCase ) )
                {
                    if ( string.IsNullOrWhiteSpace( replyTo ) )
                    {
                        replyTo = fromAddress;
                    }
                    fromAddress = globalAttributes.GetValue( "OrganizationEmail" );
                }


                // From
                MailMessage message = new MailMessage();
                message.From = new MailAddress(
                    fromAddress,
                    communication.GetChannelDataValue( "FromName" ) );

                // Reply To
                if ( !string.IsNullOrWhiteSpace( replyTo ) )
                {
                    message.ReplyToList.Add( new MailAddress( replyTo ) );
                }

                // CC
                string cc = communication.GetChannelDataValue( "CC" );
                if ( !string.IsNullOrWhiteSpace( cc ) )
                {
                    foreach ( string ccRecipient in cc.SplitDelimitedValues() )
                    {
                        message.CC.Add( new MailAddress( ccRecipient ) );
                    }
                }

                // BCC
                string bcc = communication.GetChannelDataValue( "BCC" );
                if ( !string.IsNullOrWhiteSpace( bcc ) )
                {
                    foreach ( string bccRecipient in bcc.SplitDelimitedValues() )
                    {
                        message.Bcc.Add( new MailAddress( bccRecipient ) );
                    }
                }

                message.IsBodyHtml = true;
                message.Priority = MailPriority.Normal;

                var smtpClient = GetSmtpClient();

                // Add Attachments
                string attachmentIds = communication.GetChannelDataValue( "Attachments" );
                if ( !string.IsNullOrWhiteSpace( attachmentIds ) )
                {
                    var binaryFileService = new BinaryFileService( rockContext );

                    foreach ( string idVal in attachmentIds.SplitDelimitedValues() )
                    {
                        int binaryFileId = int.MinValue;
                        if ( int.TryParse( idVal, out binaryFileId ) )
                        {
                            var binaryFile = binaryFileService.Get( binaryFileId );
                            if ( binaryFile != null )
                            {
                                Stream stream = new MemoryStream( binaryFile.Data.Content );
                                message.Attachments.Add( new Attachment( stream, binaryFile.FileName ) );
                            }
                        }
                    }
                }

                var recipientService = new CommunicationRecipientService( rockContext );

                var globalConfigValues = Rock.Web.Cache.GlobalAttributesCache.GetMergeFields( null );

                bool recipientFound = true;
                while ( recipientFound )
                {
                    var recipient = recipientService.Get( communication.Id, CommunicationRecipientStatus.Pending ).FirstOrDefault();
                    if ( recipient != null )
                    {
                        if ( string.IsNullOrWhiteSpace( recipient.Person.Email ) )
                        {
                            recipient.Status = CommunicationRecipientStatus.Failed;
                            recipient.StatusNote = "No Email Address";
                        }
                        else
                        {
                            message.To.Clear();
                            message.To.Add( new MailAddress( recipient.Person.Email, recipient.Person.FullName ) );

                            // Create merge field dictionary
                            var mergeObjects = recipient.CommunicationMergeValues( globalConfigValues );

                            message.Subject = communication.Subject.ResolveMergeFields( mergeObjects );

                            // add mandrill headers
                            message.Headers.Add( "X-MC-Track", "opens, clicks" );
                            message.Headers.Add( "X-MC-InlineCSS", inlineCss.ToString() );
                            message.Headers.Add( "X-MC-Metadata", String.Format( @"{{ ""communication_recipient_guid"":""{0}"" }}", recipient.Guid.ToString() ) );

                            // Add text view first as last view is usually treated as the preferred view by email readers (gmail)
                            string plainTextBody = Rock.Communication.Channel.Email.ProcessTextBody( communication, globalAttributes, mergeObjects );
                            if ( !string.IsNullOrWhiteSpace( plainTextBody ) )
                            {
                                AlternateView plainTextView = AlternateView.CreateAlternateViewFromString( plainTextBody, new ContentType( MediaTypeNames.Text.Plain ) );
                                message.AlternateViews.Add( plainTextView );
                            }

                            string htmlBody = Rock.Communication.Channel.Email.ProcessHtmlBody( communication, globalAttributes, mergeObjects );
                            if ( !string.IsNullOrWhiteSpace( htmlBody ) )
                            {
                                AlternateView htmlView = AlternateView.CreateAlternateViewFromString( htmlBody, new ContentType( MediaTypeNames.Text.Html ) );
                                message.AlternateViews.Add( htmlView );
                            }

                            try
                            {
                                smtpClient.Send( message );
                                recipient.Status = CommunicationRecipientStatus.Delivered;
                                recipient.StatusNote = String.Format( "Email was recieved for delivery by Mandrill ({0})", RockDateTime.Now );
                                recipient.TransportEntityTypeName = this.GetType().FullName;
                            }
                            catch ( Exception ex )
                            {
                                recipient.Status = CommunicationRecipientStatus.Failed;
                                recipient.StatusNote = "SMTP Exception: " + ex.Message;
                            }
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

        /// <summary>
        /// Sends the specified template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="recipients">The recipients.</param>
        /// <param name="appRoot"></param>
        /// <param name="themeRoot"></param>
        public override void Send( SystemEmail template, Dictionary<string, Dictionary<string, object>> recipients, string appRoot, string themeRoot )
        {
            var globalAttributes = GlobalAttributesCache.Read();

            string from = template.From;
            if ( string.IsNullOrWhiteSpace( from ) )
            {
                from = globalAttributes.GetValue( "OrganizationEmail" );
            }

            string fromName = template.FromName;
            if ( string.IsNullOrWhiteSpace( fromName ) )
            {
                fromName = globalAttributes.GetValue( "OrganizationName" );
            }

            if ( !string.IsNullOrWhiteSpace( from ) )
            {
                MailMessage message = new MailMessage();
                if ( string.IsNullOrWhiteSpace( fromName ) )
                {
                    message.From = new MailAddress( from );
                }
                else
                {
                    message.From = new MailAddress( from, fromName );
                }

                if ( !string.IsNullOrWhiteSpace( template.Cc ) )
                {
                    foreach ( string ccRecipient in template.Cc.SplitDelimitedValues() )
                    {
                        message.CC.Add( new MailAddress( ccRecipient ) );
                    }
                }

                if ( !string.IsNullOrWhiteSpace( template.Bcc ) )
                {
                    foreach ( string ccRecipient in template.Bcc.SplitDelimitedValues() )
                    {
                        message.CC.Add( new MailAddress( ccRecipient ) );
                    }
                }

                message.IsBodyHtml = true;
                message.Priority = MailPriority.Normal;

                var smtpClient = GetSmtpClient();

                var globalConfigValues = Rock.Web.Cache.GlobalAttributesCache.GetMergeFields( null );

                foreach ( KeyValuePair<string, Dictionary<string, object>> recipient in recipients )
                {
                    var mergeObjects = recipient.Value;
                    globalConfigValues.ToList().ForEach( g => mergeObjects[g.Key] = g.Value );

                    List<string> sendTo = SplitRecipient( template.To );
                    sendTo.Add( recipient.Key );
                    foreach ( string to in sendTo )
                    {
                        string subject = template.Subject;
                        string body = template.Body;
                        if ( !string.IsNullOrWhiteSpace( themeRoot ) )
                        {
                            subject = subject.Replace( "~~/", themeRoot );
                            body = body.Replace( "~~/", themeRoot );
                        }

                        if ( !string.IsNullOrWhiteSpace( appRoot ) )
                        {
                            subject = subject.Replace( "~/", appRoot );
                            body = body.Replace( "~/", appRoot );
                            body = body.Replace( @" src=""/", @" src=""" + appRoot );
                            body = body.Replace( @" href=""/", @" href=""" + appRoot );
                        }

                        message.To.Clear();
                        message.To.Add( to );
                        message.Subject = subject.ResolveMergeFields( mergeObjects );
                        message.Body = Regex.Replace( body.ResolveMergeFields( mergeObjects ), @"\[\[\s*UnsubscribeOption\s*\]\]", string.Empty );
                        smtpClient.Send( message );
                    }
                }
            }
        }

        private SmtpClient GetSmtpClient()
        {
            // Create SMTP Client
            SmtpClient smtpClient = new SmtpClient( serverName );
            smtpClient.Port = serverPort;

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            bool useSSL = false;
            smtpClient.EnableSsl = bool.TryParse( GetAttributeValue( "UseSSL" ), out useSSL ) && useSSL;

            string userName = GetAttributeValue( "Username" );
            string password = GetAttributeValue( "Password" );
            if ( !string.IsNullOrEmpty( userName ) )
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential( userName, password );
            }

            return smtpClient;
        }

        private List<string> SplitRecipient( string recipients )
        {
            if ( String.IsNullOrWhiteSpace( recipients ) )
                return new List<string>();
            else
                return new List<string>( recipients.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ) );
        }



    }
}
