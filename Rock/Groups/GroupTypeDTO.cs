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

namespace Rock.Groups
{
	/// <summary>
	/// Data Transfer Object for GroupType object
	/// </summary>
	public partial class GroupTypeDto : IDto
	{

#pragma warning disable 1591
		public bool IsSystem { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int? DefaultGroupRoleId { get; set; }
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
		public GroupTypeDto ()
		{
		}

		/// <summary>
		/// Instantiates a new DTO object from the model
		/// </summary>
		/// <param name="groupType"></param>
		public GroupTypeDto ( GroupType groupType )
		{
			CopyFromModel( groupType );
		}

		/// <summary>
		/// Copies the model property values to the DTO properties
		/// </summary>
		/// <param name="groupType"></param>
		public void CopyFromModel( IModel model )
		{
			if ( model is GroupType )
			{
				var groupType = (GroupType)model;
				this.IsSystem = groupType.IsSystem;
				this.Name = groupType.Name;
				this.Description = groupType.Description;
				this.DefaultGroupRoleId = groupType.DefaultGroupRoleId;
				this.CreatedDateTime = groupType.CreatedDateTime;
				this.ModifiedDateTime = groupType.ModifiedDateTime;
				this.CreatedByPersonId = groupType.CreatedByPersonId;
				this.ModifiedByPersonId = groupType.ModifiedByPersonId;
				this.Id = groupType.Id;
				this.Guid = groupType.Guid;
			}
		}

		/// <summary>
		/// Copies the DTO property values to the model properties
		/// </summary>
		/// <param name="groupType"></param>
		public void CopyToModel ( IModel model )
		{
			if ( model is GroupType )
			{
				var groupType = (GroupType)model;
				groupType.IsSystem = this.IsSystem;
				groupType.Name = this.Name;
				groupType.Description = this.Description;
				groupType.DefaultGroupRoleId = this.DefaultGroupRoleId;
				groupType.CreatedDateTime = this.CreatedDateTime;
				groupType.ModifiedDateTime = this.ModifiedDateTime;
				groupType.CreatedByPersonId = this.CreatedByPersonId;
				groupType.ModifiedByPersonId = this.ModifiedByPersonId;
				groupType.Id = this.Id;
				groupType.Guid = this.Guid;
			}
		}
	}
}
