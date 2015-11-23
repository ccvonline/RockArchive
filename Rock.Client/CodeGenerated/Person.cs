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
    /// Base client model for Person that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class PersonEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public DateTime? AnniversaryDate { get; set; }

        /// <summary />
        public int? BirthDay { get; set; }

        /// <summary />
        public int? BirthMonth { get; set; }

        /// <summary />
        public int? BirthYear { get; set; }

        /// <summary />
        public int? ConnectionStatusValueId { get; set; }

        /// <summary />
        public string Email { get; set; }

        /// <summary />
        public string EmailNote { get; set; }

        /// <summary />
        public Rock.Client.Enums.EmailPreference EmailPreference { get; set; }

        /// <summary />
        public string FirstName { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public Rock.Client.Enums.Gender Gender { get; set; }

        /// <summary />
        public int? GivingGroupId { get; set; }

        /// <summary>
        /// The Grade Offset of the person, which is the number of years until their graduation date. See GradeFormatted to see their current Grade. [Readonly]
        /// </summary>
        public int? GradeOffset { get; set; }

        /// <summary />
        public int? GraduationYear { get; set; }

        /// <summary />
        public string InactiveReasonNote { get; set; }

        /// <summary />
        public bool IsDeceased { get; set; }

        /// <summary />
        public bool IsEmailActive
        {
            get { return _IsEmailActive; }
            set { _IsEmailActive = value; }
        }
        private bool _IsEmailActive = true;

        /// <summary />
        public bool IsSystem { get; set; }

        /// <summary />
        public string LastName { get; set; }

        /// <summary />
        public int? MaritalStatusValueId { get; set; }

        /// <summary />
        public string MiddleName { get; set; }

        /// <summary>
        /// If the ModifiedByPersonAliasId is being set manually and should not be overwritten with current user when saved, set this value to true
        /// </summary>
        public bool ModifiedAuditValuesAlreadyUpdated { get; set; }

        /// <summary />
        public string NickName { get; set; }

        /// <summary />
        public int? PhotoId { get; set; }

        /// <summary />
        public DateTime? RecordStatusLastModifiedDateTime { get; set; }

        /// <summary />
        public int? RecordStatusReasonValueId { get; set; }

        /// <summary />
        public int? RecordStatusValueId { get; set; }

        /// <summary />
        public int? RecordTypeValueId { get; set; }

        /// <summary />
        public string ReviewReasonNote { get; set; }

        /// <summary />
        public int? ReviewReasonValueId { get; set; }

        /// <summary />
        public int? SuffixValueId { get; set; }

        /// <summary />
        public string SystemNote { get; set; }

        /// <summary />
        public int? TitleValueId { get; set; }

        /// <summary />
        public int? ViewedCount { get; set; }

        /// <summary>
        /// Leave this as NULL to let Rock set this
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// This does not need to be set or changed. Rock will always set this to the current date/time when saved to the database.
        /// </summary>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Leave this as NULL to let Rock set this
        /// </summary>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// If you need to set this manually, set ModifiedAuditValuesAlreadyUpdated=True to prevent Rock from setting it
        /// </summary>
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source Person object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( Person source )
        {
            this.Id = source.Id;
            this.AnniversaryDate = source.AnniversaryDate;
            this.BirthDay = source.BirthDay;
            this.BirthMonth = source.BirthMonth;
            this.BirthYear = source.BirthYear;
            this.ConnectionStatusValueId = source.ConnectionStatusValueId;
            this.Email = source.Email;
            this.EmailNote = source.EmailNote;
            this.EmailPreference = source.EmailPreference;
            this.FirstName = source.FirstName;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.Gender = source.Gender;
            this.GivingGroupId = source.GivingGroupId;
            this.GradeOffset = source.GradeOffset;
            this.GraduationYear = source.GraduationYear;
            this.InactiveReasonNote = source.InactiveReasonNote;
            this.IsDeceased = source.IsDeceased;
            this.IsEmailActive = source.IsEmailActive;
            this.IsSystem = source.IsSystem;
            this.LastName = source.LastName;
            this.MaritalStatusValueId = source.MaritalStatusValueId;
            this.MiddleName = source.MiddleName;
            this.ModifiedAuditValuesAlreadyUpdated = source.ModifiedAuditValuesAlreadyUpdated;
            this.NickName = source.NickName;
            this.PhotoId = source.PhotoId;
            this.RecordStatusLastModifiedDateTime = source.RecordStatusLastModifiedDateTime;
            this.RecordStatusReasonValueId = source.RecordStatusReasonValueId;
            this.RecordStatusValueId = source.RecordStatusValueId;
            this.RecordTypeValueId = source.RecordTypeValueId;
            this.ReviewReasonNote = source.ReviewReasonNote;
            this.ReviewReasonValueId = source.ReviewReasonValueId;
            this.SuffixValueId = source.SuffixValueId;
            this.SystemNote = source.SystemNote;
            this.TitleValueId = source.TitleValueId;
            this.ViewedCount = source.ViewedCount;
            this.CreatedDateTime = source.CreatedDateTime;
            this.ModifiedDateTime = source.ModifiedDateTime;
            this.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            this.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for Person that includes all the fields that are available for GETs. Use this for GETs (use PersonEntity for POST/PUTs)
    /// </summary>
    public partial class Person : PersonEntity
    {
        /// <summary />
        public DateTime? BirthDate { get; set; }

        /// <summary />
        public DefinedValue ConnectionStatusValue { get; set; }

        /// <summary />
        public int? DaysUntilBirthday { get; set; }

        /// <summary />
        public string GivingId { get; set; }

        /// <summary />
        public int GivingLeaderId { get; set; }

        /// <summary />
        public DefinedValue MaritalStatusValue { get; set; }

        /// <summary />
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }

        /// <summary />
        public BinaryFile Photo { get; set; }

        /// <summary>
        /// The Primary PersonAliasId of the Person
        /// </summary>
        public int? PrimaryAliasId { get; set; }

        /// <summary />
        public DefinedValue RecordStatusReasonValue { get; set; }

        /// <summary />
        public DefinedValue RecordStatusValue { get; set; }

        /// <summary />
        public DefinedValue RecordTypeValue { get; set; }

        /// <summary />
        public DefinedValue ReviewReasonValue { get; set; }

        /// <summary />
        public DefinedValue SuffixValue { get; set; }

        /// <summary />
        public DefinedValue TitleValue { get; set; }

        /// <summary />
        public ICollection<UserLogin> Users { get; set; }

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
