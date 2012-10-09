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
    /// Data Transfer Object for PhoneNumber object
    /// </summary>
    public partial class PhoneNumberDto : IDto
    {

#pragma warning disable 1591
		public bool IsSystem { get; set; }
		public int PersonId { get; set; }
		public string Number { get; set; }
		public string Extension { get; set; }
		public int? NumberTypeId { get; set; }
		public bool IsMessagingEnabled { get; set; }
		public bool IsUnlisted { get; set; }
		public string Description { get; set; }
		public int Id { get; set; }
		public Guid Guid { get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public PhoneNumberDto ()
        {
        }

		/// <summary>
		/// Instantiates a new DTO object from the entity
		/// </summary>
		/// <param name="phoneNumber"></param>
		public PhoneNumberDto ( PhoneNumber phoneNumber )
		{
			CopyFromModel( phoneNumber );
		}

		/// <summary>
		/// Copies the model property values to the DTO properties
		/// </summary>
		/// <param name="model">The model</param>
		public void CopyFromModel( IEntity model )
		{
			if ( model is PhoneNumber )
			{
				var phoneNumber = (PhoneNumber)model;
				this.IsSystem = phoneNumber.IsSystem;
				this.PersonId = phoneNumber.PersonId;
				this.Number = phoneNumber.Number;
				this.Extension = phoneNumber.Extension;
				this.NumberTypeId = phoneNumber.NumberTypeId;
				this.IsMessagingEnabled = phoneNumber.IsMessagingEnabled;
				this.IsUnlisted = phoneNumber.IsUnlisted;
				this.Description = phoneNumber.Description;
				this.Id = phoneNumber.Id;
				this.Guid = phoneNumber.Guid;
			}
		}

		/// <summary>
		/// Copies the DTO property values to the entity properties
		/// </summary>
		/// <param name="model">The model</param>
		public void CopyToModel ( IEntity model )
		{
			if ( model is PhoneNumber )
			{
				var phoneNumber = (PhoneNumber)model;
				phoneNumber.IsSystem = this.IsSystem;
				phoneNumber.PersonId = this.PersonId;
				phoneNumber.Number = this.Number;
				phoneNumber.Extension = this.Extension;
				phoneNumber.NumberTypeId = this.NumberTypeId;
				phoneNumber.IsMessagingEnabled = this.IsMessagingEnabled;
				phoneNumber.IsUnlisted = this.IsUnlisted;
				phoneNumber.Description = this.Description;
				phoneNumber.Id = this.Id;
				phoneNumber.Guid = this.Guid;
			}
		}
	}
}
