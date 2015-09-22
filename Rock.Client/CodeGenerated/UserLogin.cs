//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
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


namespace Rock.Client
{
    /// <summary>
    /// Base client model for UserLogin that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class UserLoginEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public string ApiKey { get; set; }

        /// <summary />
        public int? EntityTypeId { get; set; }

        /// <summary />
        public int? FailedPasswordAttemptCount { get; set; }

        /// <summary />
        public DateTime? FailedPasswordAttemptWindowStartDateTime { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public bool? IsConfirmed { get; set; }

        /// <summary />
        public bool? IsLockedOut { get; set; }

        /// <summary />
        public bool? IsOnLine { get; set; }

        /// <summary />
        public DateTime? LastActivityDateTime { get; set; }

        /// <summary />
        public DateTime? LastLockedOutDateTime { get; set; }

        /// <summary />
        public DateTime? LastLoginDateTime { get; set; }

        /// <summary />
        public DateTime? LastPasswordChangedDateTime { get; set; }

        /// <summary />
        public DateTime? LastPasswordExpirationWarningDateTime { get; set; }

        /// <summary>
        /// If the ModifiedByPersonAliasId and ModifiedDateTime properties are being set manually and should not be overwritten with current time/user when saved, set this value to true
        /// </summary>
        public bool ModifiedAuditValuesAlreadyUpdated { get; set; }

        /// <summary />
        public string Password { get; set; }

        /// <summary />
        public int? PersonId { get; set; }

        /// <summary />
        public string UserName { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source UserLogin object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( UserLogin source )
        {
            this.Id = source.Id;
            this.ApiKey = source.ApiKey;
            this.EntityTypeId = source.EntityTypeId;
            this.FailedPasswordAttemptCount = source.FailedPasswordAttemptCount;
            this.FailedPasswordAttemptWindowStartDateTime = source.FailedPasswordAttemptWindowStartDateTime;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.IsConfirmed = source.IsConfirmed;
            this.IsLockedOut = source.IsLockedOut;
            this.IsOnLine = source.IsOnLine;
            this.LastActivityDateTime = source.LastActivityDateTime;
            this.LastLockedOutDateTime = source.LastLockedOutDateTime;
            this.LastLoginDateTime = source.LastLoginDateTime;
            this.LastPasswordChangedDateTime = source.LastPasswordChangedDateTime;
            this.LastPasswordExpirationWarningDateTime = source.LastPasswordExpirationWarningDateTime;
            this.ModifiedAuditValuesAlreadyUpdated = source.ModifiedAuditValuesAlreadyUpdated;
            this.Password = source.Password;
            this.PersonId = source.PersonId;
            this.UserName = source.UserName;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for UserLogin that includes all the fields that are available for GETs. Use this for GETs (use UserLoginEntity for POST/PUTs)
    /// </summary>
    public partial class UserLogin : UserLoginEntity
    {
        /// <summary />
        public EntityType EntityType { get; set; }

        /// <summary />
        public DateTime? CreatedDateTime { get; set; }

        /// <summary />
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary />
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary />
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary>
        /// NOTE: Attributes are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.Attribute> Attributes { get; set; }

        /// <summary>
        /// NOTE: AttributeValues are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.AttributeValue> AttributeValues { get; set; }
    }
}
