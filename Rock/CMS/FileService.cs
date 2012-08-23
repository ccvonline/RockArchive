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

namespace Rock.CMS
{
	/// <summary>
	/// File Service class
	/// </summary>
	public partial class FileService : Service<File, FileDTO>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileService"/> class
		/// </summary>
		public FileService() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileService"/> class
		/// </summary>
		public FileService(IRepository<File> repository) : base(repository)
		{
		}

		/// <summary>
		/// Creates a new model
		/// </summary>
		public override File CreateNew()
		{
			return new File();
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public override IQueryable<FileDTO> QueryableDTO()
		{
			return this.Queryable().Select( m => new FileDTO()
				{
					IsTemporary = m.IsTemporary,
					IsSystem = m.IsSystem,
					Data = m.Data,
					Url = m.Url,
					FileName = m.FileName,
					MimeType = m.MimeType,
					Description = m.Description,
					CreatedDateTime = m.CreatedDateTime,
					ModifiedDateTime = m.ModifiedDateTime,
					CreatedByPersonId = m.CreatedByPersonId,
					ModifiedByPersonId = m.ModifiedByPersonId,
					Id = m.Id,
					Guid = m.Guid,				});
		}
	}
}
