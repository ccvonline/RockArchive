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

namespace Rock.Com.CCVOnline.Service
{
	/// <summary>
	/// Recording Service class
	/// </summary>
	public partial class RecordingService : Service<Recording, RecordingDto>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RecordingService"/> class
		/// </summary>
		public RecordingService()
			: base( new EFRepository<Recording>( new Rock.Com.CCVOnline.Data.Context() ) )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RecordingService"/> class
		/// </summary>
		public RecordingService(IRepository<Recording> repository) : base(repository)
		{
		}

		/// <summary>
		/// Creates a new model
		/// </summary>
		public override Recording CreateNew()
		{
			return new Recording();
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public override IQueryable<RecordingDto> QueryableDto( )
		{
			return QueryableDto( this.Queryable() );
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public IQueryable<RecordingDto> QueryableDto( IQueryable<Recording> items )
		{
			return items.Select( m => new RecordingDto()
				{
					Date = m.Date,
					CampusId = m.CampusId,
					Label = m.Label,
					App = m.App,
					StreamName = m.StreamName,
					RecordingName = m.RecordingName,
					StartTime = m.StartTime,
					StartResponse = m.StartResponse,
					StopTime = m.StopTime,
					StopResponse = m.StopResponse,
					CreatedDateTime = m.CreatedDateTime,
					ModifiedDateTime = m.ModifiedDateTime,
					CreatedByPersonId = m.CreatedByPersonId,
					ModifiedByPersonId = m.ModifiedByPersonId,
					Id = m.Id,
					Guid = m.Guid,				});
		}
	}
}
