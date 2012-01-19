//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

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
		[DataMember]
		public bool System { get; set; }
		
		/// <summary>
		/// Gets or sets the First Name.
		/// </summary>
		/// <value>
		/// First Name.
		/// </value>
		[MaxLength( 50 )]
		[TrackChanges]
		[Required( ErrorMessage = "First Name must be between 1 and 12 characters" )]
		[StringLength( 12, ErrorMessage = "First Name is required" )]
		[DataMember]
		public string FirstName { get; set; }
		
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
		/// Gets or sets the Gender.
		/// </summary>
		/// <value>
		/// Enum[Gender].
		/// </value>
		[DataMember]
		internal int? GenderInternal { get; set; }

		/// <summary>
		/// Gets or sets the Gender.
		/// </summary>
		/// <value>
		/// Enum[Gender].
		/// </value>
		[NotMapped]
		public Gender Gender
		{
			get { return (Gender)this.GenderInternal; }
			set { this.GenderInternal = (int)value; }
		}
		
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
		/// Gets or sets the Birth Month.
		/// </summary>
		/// <value>
		/// Birth Month.
		/// </value>
		[DataMember]
		public int? BirthMonth { get; set; }
		
		/// <summary>
		/// Gets or sets the Birth Day.
		/// </summary>
		/// <value>
		/// Birth Day.
		/// </value>
		[DataMember]
		public int? BirthDay { get; set; }
		
		/// <summary>
		/// Gets or sets the Birth Year.
		/// </summary>
		/// <value>
		/// Birth Year.
		/// </value>
		[DataMember]
		public int? BirthYear { get; set; }
		
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
        /// Gets a Data Transfer Object (lightweight) version of this object.
        /// </summary>
        /// <value>
        /// A <see cref="Rock.CRM.DTO.Person"/> object.
        /// </value>
		public Rock.CRM.DTO.Person DataTransferObject
		{
			get 
			{ 
				Rock.CRM.DTO.Person dto = new Rock.CRM.DTO.Person();
				dto.Id = this.Id;
				dto.Guid = this.Guid;
				dto.System = this.System;
				dto.FirstName = this.FirstName;
				dto.NickName = this.NickName;
				dto.LastName = this.LastName;
				dto.Gender = this.GenderInternal;
				dto.Email = this.Email;
				dto.BirthMonth = this.BirthMonth;
				dto.BirthDay = this.BirthDay;
				dto.BirthYear = this.BirthYear;
				dto.CreatedDateTime = this.CreatedDateTime;
				dto.ModifiedDateTime = this.ModifiedDateTime;
				dto.CreatedByPersonId = this.CreatedByPersonId;
				dto.ModifiedByPersonId = this.ModifiedByPersonId;
				return dto; 
			}
		}

        /// <summary>
        /// Gets the auth entity.
        /// </summary>
		[NotMapped]
		public override string AuthEntity { get { return "CRM.Person"; } }
        
		/// <summary>
        /// Gets or sets the Blog Posts.
        /// </summary>
        /// <value>
        /// Collection of Blog Posts.
        /// </value>
		public virtual ICollection<CMS.BlogPost> BlogPosts { get; set; }
        
		/// <summary>
        /// Gets or sets the Blog Post Comments.
        /// </summary>
        /// <value>
        /// Collection of Blog Post Comments.
        /// </value>
		public virtual ICollection<CMS.BlogPostComment> BlogPostComments { get; set; }
        
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
			this.Property( p => p.GenderInternal ).HasColumnName( "Gender" );
			this.HasOptional( p => p.CreatedByPerson ).WithMany().HasForeignKey( p => p.CreatedByPersonId );
			this.HasOptional( p => p.ModifiedByPerson ).WithMany().HasForeignKey( p => p.ModifiedByPersonId );
		}
    }
}
