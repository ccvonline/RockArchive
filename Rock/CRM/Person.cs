//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;
using System.Web;

using Rock.Data;

namespace Rock.CRM
{
    /// <summary>
    /// Person POCO Entity.
    /// </summary>
    [Table( "crmPerson" )]
    public partial class Person : ModelWithAttributes<Person>, IAuditable
    {
		/// <summary>
		/// Gets or sets the System.
		/// </summary>
		/// <value>
		/// System.
		/// </value>
		[Required]
		[DataMember]
		public bool IsSystem { get; set; }
		
		/// <summary>
		/// Gets or sets the Record Type Id.
		/// </summary>
		/// <value>
		/// .
		/// </value>
		[DataMember]
		public int? RecordTypeId { get; set; }
		
		/// <summary>
		/// Gets or sets the Record Status Id.
		/// </summary>
		/// <value>
		/// .
		/// </value>
		[DataMember]
		public int? RecordStatusId { get; set; }
		
		/// <summary>
		/// Gets or sets the Record Status Reason Id.
		/// </summary>
		/// <value>
		/// .
		/// </value>
		[DataMember]
		public int? RecordStatusReasonId { get; set; }
		
		/// <summary>
		/// Gets or sets the Person Status Id.
		/// </summary>
		/// <value>
		/// .
		/// </value>
		[DataMember]
		public int? PersonStatusId { get; set; }
		
		/// <summary>
		/// Gets or sets the Title Id.
		/// </summary>
		/// <value>
		/// .
		/// </value>
		[DataMember]
		public int? TitleId { get; set; }
		
		/// <summary>
		/// Gets or sets the Given Name.
		/// </summary>
		/// <value>
		/// Given Name.
		/// </value>
		[MaxLength( 50 )]
		[DataMember]
		public string GivenName { get; set; }

        /// <summary>
		/// Gets or sets the Nick Name.
		/// </summary>
		/// <value>
		/// Nick Name.
		/// </value>
		[MaxLength( 50 )]
		[TrackChanges]
		[DataMember]
		public string NickName { get; set; }

        /// <summary>
		/// Gets or sets the Last Name.
		/// </summary>
		/// <value>
		/// Last Name.
		/// </value>
		[MaxLength( 50 )]
		[TrackChanges]
		[DataMember]
		public string LastName { get; set; }
		
		/// <summary>
		/// Gets or sets the Suffix Id.
		/// </summary>
		/// <value>
		/// .
		/// </value>
		[DataMember]
		public int? SuffixId { get; set; }
		
		/// <summary>
		/// Gets or sets the Photo Id.
		/// </summary>
		/// <value>
		/// Photo Id.
		/// </value>
		[DataMember]
		public int? PhotoId { get; set; }
		
		/// <summary>
		/// Gets or sets the Birth Day.
		/// </summary>
		/// <value>
		/// Birth Day.
		/// </value>
		[DataMember]
		public int? BirthDay { get; set; }
		
		/// <summary>
		/// Gets or sets the Birth Month.
		/// </summary>
		/// <value>
		/// Birth Month.
		/// </value>
		[DataMember]
		public int? BirthMonth { get; set; }
		
		/// <summary>
		/// Gets or sets the Birth Year.
		/// </summary>
		/// <value>
		/// Birth Year.
		/// </value>
		[DataMember]
		public int? BirthYear { get; set; }
		
		/// <summary>
		/// Gets or sets the Gender.
		/// </summary>
		/// <value>
		/// Enum[Gender].
		/// </value>
        [Required]
		[DataMember]
        public Gender Gender { get; set; }

		/// <summary>
		/// Gets or sets the Marital Status Id.
		/// </summary>
		/// <value>
		/// .
		/// </value>
		[DataMember]
		public int? MaritalStatusId { get; set; }
		
		/// <summary>
		/// Gets or sets the Anniversary Date.
		/// </summary>
		/// <value>
		/// Anniversary Date.
		/// </value>
		[DataMember]
		public DateTime? AnniversaryDate { get; set; }
		
		/// <summary>
		/// Gets or sets the Graduation Date.
		/// </summary>
		/// <value>
		/// Graduation Date.
		/// </value>
		[DataMember]
		public DateTime? GraduationDate { get; set; }
		
		/// <summary>
		/// Gets or sets the Email.
		/// </summary>
		/// <value>
		/// Email.
		/// </value>
		[MaxLength( 75 )]
		[DataMember]
		public string Email { get; set; }
		
		/// <summary>
		/// Gets or sets the Email Is Active.
		/// </summary>
		/// <value>
		/// Email Is Active.
		/// </value>
		[DataMember]
		public bool? IsEmailActive { get; set; }
		
		/// <summary>
		/// Gets or sets the Email Note.
		/// </summary>
		/// <value>
		/// Email Note.
		/// </value>
		[MaxLength( 250 )]
		[DataMember]
		public string EmailNote { get; set; }
		
		/// <summary>
		/// Gets or sets the Do Not Email.
		/// </summary>
		/// <value>
		/// Do Not Email.
		/// </value>
		[Required]
		[DataMember]
		public bool DoNotEmail { get; set; }
		
		/// <summary>
		/// Gets or sets the System Note.
		/// </summary>
		/// <value>
		/// System Note.
		/// </value>
		[MaxLength( 1000 )]
		[DataMember]
		public string SystemNote { get; set; }
		
		/// <summary>
		/// Gets or sets the Viewed Count.
		/// </summary>
		/// <value>
		/// Viewed Count.
		/// </value>
		[DataMember]
		public int? ViewedCount { get; set; }
		
		/// <summary>
		/// Gets or sets the Created Date Time.
		/// </summary>
		/// <value>
		/// Created Date Time.
		/// </value>
		[DataMember]
		public DateTime? CreatedDateTime { get; set; }
		
		/// <summary>
		/// Gets or sets the Modified Date Time.
		/// </summary>
		/// <value>
		/// Modified Date Time.
		/// </value>
		[DataMember]
		public DateTime? ModifiedDateTime { get; set; }
		
		/// <summary>
		/// Gets or sets the Created By Person Id.
		/// </summary>
		/// <value>
		/// Created By Person Id.
		/// </value>
		[DataMember]
		public int? CreatedByPersonId { get; set; }
		
		/// <summary>
		/// Gets or sets the Modified By Person Id.
		/// </summary>
		/// <value>
		/// Modified By Person Id.
		/// </value>
		[DataMember]
		public int? ModifiedByPersonId { get; set; }
		
        /// <summary>
        /// Gets the auth entity.
        /// </summary>
		[NotMapped]
		public override string AuthEntity { get { return "CRM.Person"; } }
        
		/// <summary>
        /// Gets or sets the Users.
        /// </summary>
        /// <value>
        /// Collection of Users.
        /// </value>
		public virtual ICollection<CMS.User> Users { get; set; }
        
		/// <summary>
        /// Gets or sets the Email Templates.
        /// </summary>
        /// <value>
        /// Collection of Email Templates.
        /// </value>
		public virtual ICollection<EmailTemplate> EmailTemplates { get; set; }
        
		/// <summary>
        /// Gets or sets the Phone Numbers.
        /// </summary>
        /// <value>
        /// Collection of Phone Numbers.
        /// </value>
		public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
        
		/// <summary>
        /// Gets or sets the Members.
        /// </summary>
        /// <value>
        /// Collection of Members.
        /// </value>
		public virtual ICollection<Groups.Member> Members { get; set; }

        /// <summary>
        /// Gets or sets the Pledges.
        /// </summary>
        /// <value>
        /// Collection of Pledges.
        /// </value>
        public virtual ICollection<Financial.Pledge> Pledges { get; set; }

        /// <summary>
        /// Gets or sets the PersonAccountLookups.
        /// </summary>
        /// <value>
        /// Collection of PersonAccountLookups.
        /// </value>
        public virtual ICollection<Financial.PersonAccountLookup> PersonAccountLookups { get; set; }

		/// <summary>
        /// Gets or sets the Marital Status.
        /// </summary>
        /// <value>
        /// A <see cref="Core.DefinedValue"/> object.
        /// </value>
		public virtual Core.DefinedValue MaritalStatus { get; set; }
        
		/// <summary>
        /// Gets or sets the Person Status.
        /// </summary>
        /// <value>
        /// A <see cref="Core.DefinedValue"/> object.
        /// </value>
		public virtual Core.DefinedValue PersonStatus { get; set; }
        
		/// <summary>
        /// Gets or sets the Record Status.
        /// </summary>
        /// <value>
        /// A <see cref="Core.DefinedValue"/> object.
        /// </value>
		public virtual Core.DefinedValue RecordStatus { get; set; }
        
		/// <summary>
        /// Gets or sets the Record Status Reason.
        /// </summary>
        /// <value>
        /// A <see cref="Core.DefinedValue"/> object.
        /// </value>
		public virtual Core.DefinedValue RecordStatusReason { get; set; }
        
		/// <summary>
        /// Gets or sets the Record Type.
        /// </summary>
        /// <value>
        /// A <see cref="Core.DefinedValue"/> object.
        /// </value>
		public virtual Core.DefinedValue RecordType { get; set; }
        
		/// <summary>
        /// Gets or sets the Suffix.
        /// </summary>
        /// <value>
        /// A <see cref="Core.DefinedValue"/> object.
        /// </value>
		public virtual Core.DefinedValue Suffix { get; set; }
        
		/// <summary>
        /// Gets or sets the Title.
        /// </summary>
        /// <value>
        /// A <see cref="Core.DefinedValue"/> object.
        /// </value>
		public virtual Core.DefinedValue Title { get; set; }
        
		/// <summary>
        /// Gets or sets the Created By Person.
        /// </summary>
        /// <value>
        /// A <see cref="Person"/> object.
        /// </value>
		public virtual Person CreatedByPerson { get; set; }
        
		/// <summary>
        /// Gets or sets the Modified By Person.
        /// </summary>
        /// <value>
        /// A <see cref="Person"/> object.
        /// </value>
		public virtual Person ModifiedByPerson { get; set; }

        /// <summary>
        /// Gets NickName if not null, otherwise gets GivenName.
        /// </summary>
        public string FirstName
        {
            get
            {
                return NickName ?? GivenName;
            }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        [NotMapped]
        public DateTime? BirthDate
        {
            // notes
            // if no birthday is available then DateTime.MinValue is returned
            // if no birth year is given then the birth year will be DateTime.MinValue.Year
            get
            {
                if ( BirthDay == null || BirthMonth == null )
                {
                    return null;
                }
                else
                {
                    string birthYear = ( BirthYear ?? DateTime.MinValue.Year ).ToString();
                    return Convert.ToDateTime( BirthMonth.ToString() + "/" + BirthDay.ToString() + "/" + birthYear );
                }
            }

            set
            {
                if ( value.HasValue )
                {
                    BirthMonth = value.Value.Month;
                    BirthDay = value.Value.Day;
                    BirthYear = value.Value.Year;
                }
                else
                {
                    BirthMonth = null;
                    BirthDay = null;
                    BirthYear = null;
                }
            }
        }

        /// <summary>
        /// Gets the impersonation parameter.
        /// </summary>
        public string ImpersonationParameter
        {
            get
            {
                return "rckipid=" + HttpUtility.UrlEncode( this.EncryptedKey );
            }
        }

        public Rock.CMS.User ImpersonatedUser
        {
            get
            {
                Rock.CMS.User user = new CMS.User();
                user.UserName = this.FullName;
                user.PersonId = this.Id;
                user.Person = this;
                return user;
            }
        }

    }

    /// <summary>
    /// Person Configuration class.
    /// </summary>
    public partial class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonConfiguration"/> class.
        /// </summary>
        public PersonConfiguration()
        {
			this.HasOptional( p => p.MaritalStatus ).WithMany().HasForeignKey( p => p.MaritalStatusId ).WillCascadeOnDelete(false);
			this.HasOptional( p => p.PersonStatus ).WithMany().HasForeignKey( p => p.PersonStatusId ).WillCascadeOnDelete(false);
			this.HasOptional( p => p.RecordStatus ).WithMany().HasForeignKey( p => p.RecordStatusId ).WillCascadeOnDelete(false);
			this.HasOptional( p => p.RecordStatusReason ).WithMany().HasForeignKey( p => p.RecordStatusReasonId ).WillCascadeOnDelete(false);
			this.HasOptional( p => p.RecordType ).WithMany().HasForeignKey( p => p.RecordTypeId ).WillCascadeOnDelete(false);
			this.HasOptional( p => p.Suffix ).WithMany().HasForeignKey( p => p.SuffixId ).WillCascadeOnDelete(false);
			this.HasOptional( p => p.Title ).WithMany().HasForeignKey( p => p.TitleId ).WillCascadeOnDelete(false);
			this.HasOptional( p => p.CreatedByPerson ).WithMany().HasForeignKey( p => p.CreatedByPersonId ).WillCascadeOnDelete(false);
			this.HasOptional( p => p.ModifiedByPerson ).WithMany().HasForeignKey( p => p.ModifiedByPersonId ).WillCascadeOnDelete(false);
		}
    }

    /// <summary>
    /// Data Transformation Object
    /// </summary>
    public partial class PersonDTO : DTO<Person>
    {
        /// <summary>
        /// Gets or sets the System.
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the Record Type Id.
        /// </summary>
        /// <value>
        /// Record Type Id.
        /// </value>
        public int? RecordTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Record Status Id.
        /// </summary>
        /// <value>
        /// Record Status Id.
        /// </value>
        public int? RecordStatusId { get; set; }

        /// <summary>
        /// Gets or sets the Record Status Reason Id.
        /// </summary>
        /// <value>
        /// Record Status Reason Id.
        /// </value>
        public int? RecordStatusReasonId { get; set; }

        /// <summary>
        /// Gets or sets the Person Status Id.
        /// </summary>
        /// <value>
        /// Person Status Id.
        /// </value>
        public int? PersonStatusId { get; set; }

        /// <summary>
        /// Gets or sets the Title Id.
        /// </summary>
        /// <value>
        /// Title Id.
        /// </value>
        public int? TitleId { get; set; }

        /// <summary>
        /// Gets or sets the Given Name.
        /// </summary>
        /// <value>
        /// Given Name.
        /// </value>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the Nick Name.
        /// </summary>
        /// <value>
        /// Nick Name.
        /// </value>
        public string NickName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name.
        /// </summary>
        /// <value>
        /// Last Name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Suffix Id.
        /// </summary>
        /// <value>
        /// Suffix Id.
        /// </value>
        public int? SuffixId { get; set; }

        /// <summary>
        /// Gets or sets the Photo Id.
        /// </summary>
        /// <value>
        /// Photo Id.
        /// </value>
        public int? PhotoId { get; set; }

        /// <summary>
        /// Gets or sets the Birth Day.
        /// </summary>
        /// <value>
        /// Birth Day.
        /// </value>
        public int? BirthDay { get; set; }

        /// <summary>
        /// Gets or sets the Birth Month.
        /// </summary>
        /// <value>
        /// Birth Month.
        /// </value>
        public int? BirthMonth { get; set; }

        /// <summary>
        /// Gets or sets the Birth Year.
        /// </summary>
        /// <value>
        /// Birth Year.
        /// </value>
        public int? BirthYear { get; set; }

        /// <summary>
        /// Gets or sets the Gender.
        /// </summary>
        /// <value>
        /// Gender.
        /// </value>
        public int? Gender { get; set; }

        /// <summary>
        /// Gets or sets the Marital Status Id.
        /// </summary>
        /// <value>
        /// Marital Status Id.
        /// </value>
        public int? MaritalStatusId { get; set; }

        /// <summary>
        /// Gets or sets the Anniversary Date.
        /// </summary>
        /// <value>
        /// Anniversary Date.
        /// </value>
        public DateTime? AnniversaryDate { get; set; }

        /// <summary>
        /// Gets or sets the Graduation Date.
        /// </summary>
        /// <value>
        /// Graduation Date.
        /// </value>
        public DateTime? GraduationDate { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        /// <value>
        /// Email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Email Is Active.
        /// </summary>
        /// <value>
        /// Email Is Active.
        /// </value>
        public bool? IsEmailActive { get; set; }

        /// <summary>
        /// Gets or sets the Email Note.
        /// </summary>
        /// <value>
        /// Email Note.
        /// </value>
        public string EmailNote { get; set; }

        /// <summary>
        /// Gets or sets the Do Not Email.
        /// </summary>
        /// <value>
        /// Do Not Email.
        /// </value>
        public bool DoNotEmail { get; set; }

        /// <summary>
        /// Gets or sets the System Note.
        /// </summary>
        /// <value>
        /// System Note.
        /// </value>
        public string SystemNote { get; set; }

        /// <summary>
        /// Gets or sets the Viewed Count.
        /// </summary>
        /// <value>
        /// Viewed Count.
        /// </value>
        public int? ViewedCount { get; set; }

        /// <summary>
        /// Instantiate new DTO object
        /// </summary>
        public PersonDTO()
        {
        }

        /// <summary>
        /// Instantiate new DTO object from Model
        /// </summary>
        /// <param name="auth"></param>
        public PersonDTO( Person person )
        {
            CopyFromModel( person );
        }

        /// <summary>
        /// Copy DTO to Model
        /// </summary>
        /// <param name="person"></param>
        public override void CopyFromModel( Person person )
        {
            this.Id = person.Id;
            this.Guid = person.Guid;
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
            this.Gender = ( int )person.Gender;
            this.MaritalStatusId = person.MaritalStatusId;
            this.AnniversaryDate = person.AnniversaryDate;
            this.GraduationDate = person.GraduationDate;
            this.Email = person.Email;
            this.IsEmailActive = person.IsEmailActive;
            this.EmailNote = person.EmailNote;
            this.DoNotEmail = person.DoNotEmail;
            this.SystemNote = person.SystemNote;
            this.ViewedCount = person.ViewedCount;
            this.CreatedDateTime = person.CreatedDateTime;
            this.ModifiedDateTime = person.ModifiedDateTime;
            this.CreatedByPersonId = person.CreatedByPersonId;
            this.ModifiedByPersonId = person.ModifiedByPersonId;
        }

        /// <summary>
        /// Copy Model to DTO
        /// </summary>
        /// <param name="person"></param>
        public override void CopyToModel( Person person )
        {
            person.Id = this.Id;
            person.Guid = this.Guid;
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
            person.Gender = ( Gender )this.Gender;
            person.MaritalStatusId = this.MaritalStatusId;
            person.AnniversaryDate = this.AnniversaryDate;
            person.GraduationDate = this.GraduationDate;
            person.Email = this.Email;
            person.IsEmailActive = this.IsEmailActive;
            person.EmailNote = this.EmailNote;
            person.DoNotEmail = this.DoNotEmail;
            person.SystemNote = this.SystemNote;
            person.ViewedCount = this.ViewedCount;
            person.CreatedDateTime = this.CreatedDateTime;
            person.ModifiedDateTime = this.ModifiedDateTime;
            person.CreatedByPersonId = this.CreatedByPersonId;
            person.ModifiedByPersonId = this.ModifiedByPersonId;
        }
    }

    /// <summary>
    /// The gender of a person
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Male
        /// </summary>
        Male = 1,

        /// <summary>
        /// Female
        /// </summary>
        Female = 2
    }

}
