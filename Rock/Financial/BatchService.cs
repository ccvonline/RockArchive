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

namespace Rock.Financial
{
	/// <summary>
	/// Batch Service class
	/// </summary>
	public partial class BatchService : Service<Batch, BatchDTO>
	{
		/// <summary>
		/// Creates a new model
		/// </summary>
		public override Batch CreateNew()
		{
			return new Batch();
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public override IQueryable<BatchDTO> QueryableDTO()
		{
			return this.Queryable().Select( m => new BatchDTO()
				{
					Name = m.Name,
					BatchDate = m.BatchDate,
					IsClosed = m.IsClosed,
					CampusId = m.CampusId,
					Entity = m.Entity,
					EntityId = m.EntityId,
					ForeignReference = m.ForeignReference,
					ModifiedDateTime = m.ModifiedDateTime,
					CreatedDateTime = m.CreatedDateTime,
					CreatedByPersonId = m.CreatedByPersonId,
					ModifiedByPersonId = m.ModifiedByPersonId,
					Id = m.Id,
					Guid = m.Guid,				});
		}
	}
}
