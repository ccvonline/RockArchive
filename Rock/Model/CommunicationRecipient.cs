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
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Communication Recipient POCO Entity.
    /// </summary>
    [Table( "CommunicationRecipient" )]
    [DataContract]
    public partial class CommunicationRecipient : Model<CommunicationRecipient>
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the PersonId of the <see cref="Rock.Model.Person"/> who is being sent the <see cref="Rock.Model.Communication"/>.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the PersonId of the <see cref="Rock.Model.Person"/> who is being sent the <see cref="Rock.Model.Communication"/>.
        /// </value>
        [DataMember]
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the the CommunicationId of the <see cref="Rock.Model.Communication"/>.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the CommunicationId of the <see cref="Rock.Model.Communication"/>.
        /// </value>
        [DataMember]
        public int CommunicationId { get; set; }

        /// <summary>
        /// Gets or sets the status of the Communication submission to the recipient.
        /// </summary>
        /// <value>
        /// A <see cref="Rock.Model.CommunicationRecipientStatus"/> Enum representing the status of <see cref="Rock.Model.Communication"/> submission to the recipient.
        /// This property will be  <c>CommunicationRecipientStatus.Pending</c> when Rock is waiting to send the <see cref="Rock.Model.Communication"/> to the recipient;
        /// <c>CommunicationRecipientStatus.Success</c> when Rock has successfully sent the <see cref="Rock.Model.Communication"/> to the recipient;
        /// <c>CommunicationRecipientStatus.Failed</c> when the attempt to send the <see cref="Rock.Model.Communication"/> failed.
        /// <c>CommunicaitonRecipientStatus.Cancelled</c> when the attempt to send the <see cref="Rock.Model.Communication"/> was canceled.
        /// </value>
        [DataMember]
        public CommunicationRecipientStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the status note.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the status note.
        /// </value>
        [DataMember]
        public string StatusNote { get; set; }

        /// <summary>
        /// Gets or sets the datetime that communication was opened by recipient.
        /// </summary>
        /// <value>
        /// The opened date time.
        /// </value>
        [DataMember]
        public DateTime? OpenedDateTime { get; set; }

        /// <summary>
        /// Gets or sets type of client that the recipient used to open the communication.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        [DataMember]
        [MaxLength(200)]
        public string OpenedClient { get; set; }

        /// <summary>
        /// Gets or sets the AdditionalMergeValues as a Json string.
        /// </summary>
        /// <value>
        /// A Json formatted <see cref="System.String"/> containing the AdditionalMergeValues for the communication recipient. 
        /// </value>
        [DataMember]
        public string AdditionalMergeValuesJson
        {
            get
            {
                return AdditionalMergeValues.ToJson();
            }

            set
            {
                if ( string.IsNullOrWhiteSpace( value ) )
                {
                    AdditionalMergeValues = new Dictionary<string, string>();
                }
                else
                {
                    AdditionalMergeValues = JsonConvert.DeserializeObject<Dictionary<string, string>>( value );
                }
            }
        }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.Person"/> who is receiving the <see cref="Rock.Model.Communication"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.Person"/> who is receiving the <see cref="Rock.Model.Communication"/>.
        /// </value>
        [DataMember]
        public virtual Person Person { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.Communication"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.Communication"/>
        /// </value>
        [DataMember]
        public virtual Communication Communication { get; set; }

        /// <summary>
        /// Gets or sets a collection containing the <see cref="Rock.Model.CommunicationRecipient">CommunicationRecipients</see> for the Communication.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.CommunicationRecipient">CommunicationRecipients</see> of the Communication.
        /// </value>
        [DataMember]
        public virtual ICollection<CommunicationRecipientActivity> Activities
        {
            get { return _activities ?? ( _activities = new Collection<CommunicationRecipientActivity>() ); }
            set { _activities = value; }
        }
        private ICollection<CommunicationRecipientActivity> _activities;

        /// <summary>
        /// Gets or sets a dictionary containing the Additional Merge values for this communication
        /// </summary>
        /// <value>
        ///  A <see cref="System.Collections.Generic.Dictionary{String,String}"/> of <see cref="System.String"/> objects containing additional merge values for the <see cref="Rock.Model.Communication"/>
        /// </value>
        [DataMember]
        public virtual Dictionary<string, string> AdditionalMergeValues
        {
            get { return _additionalMergeValues; }
            set { _additionalMergeValues = value; }
        }
        private Dictionary<string, string> _additionalMergeValues = new Dictionary<string, string>();


        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Person.ToStringSafe();
        }

        #endregion

        #region Private Methods

        #endregion

        #region Static Methods

        #endregion

    }

    #region Entity Configuration

    /// <summary>
    /// Communication Recipient Configuration class.
    /// </summary>
    public partial class CommunicationRecipientConfiguration : EntityTypeConfiguration<CommunicationRecipient>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationRecipientConfiguration"/> class.
        /// </summary>
        public CommunicationRecipientConfiguration()
        {
            this.HasRequired( r => r.Person).WithMany().HasForeignKey( r => r.PersonId ).WillCascadeOnDelete( false );
            this.HasRequired( r => r.Communication ).WithMany( c => c.Recipients ).HasForeignKey( r => r.CommunicationId ).WillCascadeOnDelete( true );
        }
    }

    #endregion

    /// <summary>
    /// The status of communication being sent to recipient
    /// </summary>
    public enum CommunicationRecipientStatus
    {
        /// <summary>
        /// Communication has not yet been sent to recipient
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Communication was successfully sent to recipient
        /// </summary>
        Success = 1,

        /// <summary>
        /// Communication failed to be sent to recipient
        /// </summary>
        Failed = 2,

        /// <summary>
        /// Communication was cancelled prior to sending to the recipient
        /// </summary>
        Cancelled = 3,

        /// <summary>
        /// Communication was sent and opened (viewed) by the recipient
        /// </summary>
        Opened = 4
    }
}

