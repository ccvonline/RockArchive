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

namespace Rock.Groups
{
	/// <summary>
	/// GroupType Service class
	/// </summary>
	public partial class GroupTypeService : Service<GroupType, GroupTypeDTO>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GroupTypeService"/> class
		/// </summary>
		public GroupTypeService() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GroupTypeService"/> class
		/// </summary>
		public GroupTypeService(IRepository<GroupType> repository) : base(repository)
		{
		}

		/// <summary>
		/// Creates a new model
		/// </summary>
		public override GroupType CreateNew()
		{
			return new GroupType();
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public override IQueryable<GroupTypeDTO> QueryableDTO()
		{
			return this.Queryable().Select( m => new GroupTypeDTO()
				{
					IsSystem = m.IsSystem,
					Name = m.Name,
					Description = m.Description,
					DefaultGroupRoleId = m.DefaultGroupRoleId,
					CreatedDateTime = m.CreatedDateTime,
					ModifiedDateTime = m.ModifiedDateTime,
					CreatedByPersonId = m.CreatedByPersonId,
					ModifiedByPersonId = m.ModifiedByPersonId,
					Id = m.Id,
					Guid = m.Guid,				});
		}
	}
}
