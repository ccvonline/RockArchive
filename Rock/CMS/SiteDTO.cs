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

namespace Rock.Cms
{
	/// <summary>
	/// Data Transfer Object for Site object
	/// </summary>
	public partial class SiteDto : Dto<Site>
	{

#pragma warning disable 1591
		public bool IsSystem { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Theme { get; set; }
		public int? DefaultPageId { get; set; }
		public string FaviconUrl { get; set; }
		public string AppleTouchIconUrl { get; set; }
		public string FacebookAppId { get; set; }
		public string FacebookAppSecret { get; set; }
		public string LoginPageReference { get; set; }
		public string RegistrationPageReference { get; set; }
		public string ErrorPage { get; set; }
#pragma warning restore 1591

		/// <summary>
		/// Instantiates a new DTO object
		/// </summary>
		public SiteDto ()
		{
		}

		/// <summary>
		/// Instantiates a new DTO object from the model
		/// </summary>
		/// <param name="site"></param>
		public SiteDto ( Site site )
		{
			CopyFromModel( site );
		}

		/// <summary>
		/// Copies the model property values to the DTO properties
		/// </summary>
		/// <param name="site"></param>
		public override void CopyFromModel( Site site )
		{
			this.IsSystem = site.IsSystem;
			this.Name = site.Name;
			this.Description = site.Description;
			this.Theme = site.Theme;
			this.DefaultPageId = site.DefaultPageId;
			this.FaviconUrl = site.FaviconUrl;
			this.AppleTouchIconUrl = site.AppleTouchIconUrl;
			this.FacebookAppId = site.FacebookAppId;
			this.FacebookAppSecret = site.FacebookAppSecret;
			this.LoginPageReference = site.LoginPageReference;
			this.RegistrationPageReference = site.RegistrationPageReference;
			this.ErrorPage = site.ErrorPage;
			this.CreatedDateTime = site.CreatedDateTime;
			this.ModifiedDateTime = site.ModifiedDateTime;
			this.CreatedByPersonId = site.CreatedByPersonId;
			this.ModifiedByPersonId = site.ModifiedByPersonId;
			this.Id = site.Id;
			this.Guid = site.Guid;
		}

		/// <summary>
		/// Copies the DTO property values to the model properties
		/// </summary>
		/// <param name="site"></param>
		public override void CopyToModel ( Site site )
		{
			site.IsSystem = this.IsSystem;
			site.Name = this.Name;
			site.Description = this.Description;
			site.Theme = this.Theme;
			site.DefaultPageId = this.DefaultPageId;
			site.FaviconUrl = this.FaviconUrl;
			site.AppleTouchIconUrl = this.AppleTouchIconUrl;
			site.FacebookAppId = this.FacebookAppId;
			site.FacebookAppSecret = this.FacebookAppSecret;
			site.LoginPageReference = this.LoginPageReference;
			site.RegistrationPageReference = this.RegistrationPageReference;
			site.ErrorPage = this.ErrorPage;
			site.CreatedDateTime = this.CreatedDateTime;
			site.ModifiedDateTime = this.ModifiedDateTime;
			site.CreatedByPersonId = this.CreatedByPersonId;
			site.ModifiedByPersonId = this.ModifiedByPersonId;
			site.Id = this.Id;
			site.Guid = this.Guid;
		}
	}
}
