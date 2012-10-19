//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;

using Rock.Data;

namespace Rock.Crm
{
    /// <summary>
    /// Data Transfer Object for Person object
    /// </summary>
    public partial class PersonDto : IDto
    {

#pragma warning disable 1591
        public bool IsSystem { get; set; }
        public int? RecordTypeId { get; set; }
        public int? RecordStatusId { get; set; }
        public int? RecordStatusReasonId { get; set; }
        public int? PersonStatusId { get; set; }
        public int? TitleId { get; set; }
        public string GivenName { get; set; }
        public string NickName { get; set; }
        public string LastName { get; set; }
        public int? SuffixId { get; set; }
        public int? PhotoId { get; set; }
        public int? BirthDay { get; set; }
        public int? BirthMonth { get; set; }
        public int? BirthYear { get; set; }
        public Gender Gender { get; set; }
        public int? MaritalStatusId { get; set; }
        public DateTimeOffset? AnniversaryDate { get; set; }
        public DateTimeOffset? GraduationDate { get; set; }
        public string Email { get; set; }
        public bool? IsEmailActive { get; set; }
        public string EmailNote { get; set; }
        public bool DoNotEmail { get; set; }
        public string SystemNote { get; set; }
        public int? ViewedCount { get; set; }
        public int Id { get; set; }
        public Guid Guid { get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public PersonDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="person"></param>
        public PersonDto ( Person person )
        {
            CopyFromModel( person );
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyFromModel( IEntity model )
        {
            if ( model is Person )
            {
                var person = (Person)model;
                this.IsSystem = person.IsSystem;
                this.RecordTypeId = person.RecordTypeId;
                this.RecordStatusId = person.RecordStatusId;
                this.RecordStatusReasonId = person.RecordStatusReasonId;
                this.PersonStatusId = person.PersonStatusId;
                this.TitleId = person.TitleId;
                this.GivenName = person.GivenName;
                this.NickName = person.NickName;
                this.LastName = person.LastName;
                this.SuffixId = person.SuffixId;
                this.PhotoId = person.PhotoId;
                this.BirthDay = person.BirthDay;
                this.BirthMonth = person.BirthMonth;
                this.BirthYear = person.BirthYear;
                this.Gender = person.Gender;
                this.MaritalStatusId = person.MaritalStatusId;
                this.AnniversaryDate = person.AnniversaryDate;
                this.GraduationDate = person.GraduationDate;
                this.Email = person.Email;
                this.IsEmailActive = person.IsEmailActive;
                this.EmailNote = person.EmailNote;
                this.DoNotEmail = person.DoNotEmail;
                this.SystemNote = person.SystemNote;
                this.ViewedCount = person.ViewedCount;
                this.Id = person.Id;
                this.Guid = person.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is Person )
            {
                var person = (Person)model;
                person.IsSystem = this.IsSystem;
                person.RecordTypeId = this.RecordTypeId;
                person.RecordStatusId = this.RecordStatusId;
                person.RecordStatusReasonId = this.RecordStatusReasonId;
                person.PersonStatusId = this.PersonStatusId;
                person.TitleId = this.TitleId;
                person.GivenName = this.GivenName;
                person.NickName = this.NickName;
                person.LastName = this.LastName;
                person.SuffixId = this.SuffixId;
                person.PhotoId = this.PhotoId;
                person.BirthDay = this.BirthDay;
                person.BirthMonth = this.BirthMonth;
                person.BirthYear = this.BirthYear;
                person.Gender = this.Gender;
                person.MaritalStatusId = this.MaritalStatusId;
                person.AnniversaryDate = this.AnniversaryDate;
                person.GraduationDate = this.GraduationDate;
                person.Email = this.Email;
                person.IsEmailActive = this.IsEmailActive;
                person.EmailNote = this.EmailNote;
                person.DoNotEmail = this.DoNotEmail;
                person.SystemNote = this.SystemNote;
                person.ViewedCount = this.ViewedCount;
                person.Id = this.Id;
                person.Guid = this.Guid;
            }
        }
    }
}
