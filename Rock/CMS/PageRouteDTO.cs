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
	/// Data Transfer Object for PageRoute object
	/// </summary>
	public partial class PageRouteDto : IDto
	{

#pragma warning disable 1591
		public bool IsSystem { get; set; }
		public int PageId { get; set; }
		public string Route { get; set; }
		public DateTime? CreatedDateTime { get; set; }
		public DateTime? ModifiedDateTime { get; set; }
		public int? CreatedByPersonId { get; set; }
		public int? ModifiedByPersonId { get; set; }
		public int Id { get; set; }
		public Guid Guid { get; set; }
#pragma warning restore 1591

		/// <summary>
		/// Instantiates a new DTO object
		/// </summary>
		public PageRouteDto ()
		{
		}

		/// <summary>
		/// Instantiates a new DTO object from the model
		/// </summary>
		/// <param name="pageRoute"></param>
		public PageRouteDto ( PageRoute pageRoute )
		{
			CopyFromModel( pageRoute );
		}

		/// <summary>
		/// Copies the model property values to the DTO properties
		/// </summary>
		/// <param name="pageRoute"></param>
		public void CopyFromModel( IModel model )
		{
			if ( model is PageRoute )
			{
				var pageRoute = (PageRoute)model;
				this.IsSystem = pageRoute.IsSystem;
				this.PageId = pageRoute.PageId;
				this.Route = pageRoute.Route;
				this.CreatedDateTime = pageRoute.CreatedDateTime;
				this.ModifiedDateTime = pageRoute.ModifiedDateTime;
				this.CreatedByPersonId = pageRoute.CreatedByPersonId;
				this.ModifiedByPersonId = pageRoute.ModifiedByPersonId;
				this.Id = pageRoute.Id;
				this.Guid = pageRoute.Guid;
			}
		}

		/// <summary>
		/// Copies the DTO property values to the model properties
		/// </summary>
		/// <param name="pageRoute"></param>
		public void CopyToModel ( IModel model )
		{
			if ( model is PageRoute )
			{
				var pageRoute = (PageRoute)model;
				pageRoute.IsSystem = this.IsSystem;
				pageRoute.PageId = this.PageId;
				pageRoute.Route = this.Route;
				pageRoute.CreatedDateTime = this.CreatedDateTime;
				pageRoute.ModifiedDateTime = this.ModifiedDateTime;
				pageRoute.CreatedByPersonId = this.CreatedByPersonId;
				pageRoute.ModifiedByPersonId = this.ModifiedByPersonId;
				pageRoute.Id = this.Id;
				pageRoute.Guid = this.Guid;
			}
		}
	}
}
