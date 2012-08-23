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
	/// BlockInstance Service class
	/// </summary>
	public partial class BlockInstanceService : Service<BlockInstance, BlockInstanceDTO>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlockInstanceService"/> class
		/// </summary>
		public BlockInstanceService() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockInstanceService"/> class
		/// </summary>
		public BlockInstanceService(IRepository<BlockInstance> repository) : base(repository)
		{
		}

		/// <summary>
		/// Creates a new model
		/// </summary>
		public override BlockInstance CreateNew()
		{
			return new BlockInstance();
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public override IQueryable<BlockInstanceDTO> QueryableDTO()
		{
			return this.Queryable().Select( m => new BlockInstanceDTO()
				{
					IsSystem = m.IsSystem,
					PageId = m.PageId,
					Layout = m.Layout,
					BlockId = m.BlockId,
					Zone = m.Zone,
					Order = m.Order,
					Name = m.Name,
					OutputCacheDuration = m.OutputCacheDuration,
					CreatedDateTime = m.CreatedDateTime,
					ModifiedDateTime = m.ModifiedDateTime,
					CreatedByPersonId = m.CreatedByPersonId,
					ModifiedByPersonId = m.ModifiedByPersonId,
					Id = m.Id,
					Guid = m.Guid,				});
		}
	}
}
