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
using System.Linq;

using Rock.Data;

namespace Rock.Cms
{
	/// <summary>
	/// HtmlContent Service class
	/// </summary>
	public partial class HtmlContentService : Service<HtmlContent, HtmlContentDto>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlContentService"/> class
		/// </summary>
		public HtmlContentService()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlContentService"/> class
		/// </summary>
		public HtmlContentService(IRepository<HtmlContent> repository) : base(repository)
		{
		}

		/// <summary>
		/// Creates a new model
		/// </summary>
		public override HtmlContent CreateNew()
		{
			return new HtmlContent();
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public override IQueryable<HtmlContentDto> QueryableDto( )
		{
			return QueryableDto( this.Queryable() );
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public IQueryable<HtmlContentDto> QueryableDto( IQueryable<HtmlContent> items )
		{
			return items.Select( m => new HtmlContentDto()
				{
					BlockId = m.BlockId,
					EntityValue = m.EntityValue,
					Version = m.Version,
					Content = m.Content,
					IsApproved = m.IsApproved,
					ApprovedByPersonId = m.ApprovedByPersonId,
					ApprovedDateTime = m.ApprovedDateTime,
					CreatedDateTime = m.CreatedDateTime,
					ModifiedDateTime = m.ModifiedDateTime,
					CreatedByPersonId = m.CreatedByPersonId,
					ModifiedByPersonId = m.ModifiedByPersonId,
					StartDateTime = m.StartDateTime,
					ExpireDateTime = m.ExpireDateTime,
					Id = m.Id,
					Guid = m.Guid,				});
		}
	}
}
