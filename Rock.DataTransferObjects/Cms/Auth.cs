//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;

namespace Rock.CMS.DTO
{
    /// <summary>
    /// Auth Data Transfer Object.
    /// </summary>
	/// <remarks>
	/// Data Transfer Objects are a lightweight version of the Entity object that are used
	/// in situations like serializing the object in the REST api
	/// </remarks>
    public partial class Auth: Rock.DTO<Auth>
    {
		/// <summary>
		/// Gets or sets the Entity Type.
		/// </summary>
		/// <value>
		/// Entity Type.
		/// </value>
		public string EntityType { get; set; }

		/// <summary>
		/// Gets or sets the Entity Id.
		/// </summary>
		/// <value>
		/// Entity Id.
		/// </value>
		public int? EntityId { get; set; }

		/// <summary>
		/// Gets or sets the Order.
		/// </summary>
		/// <value>
		/// Order.
		/// </value>
		public int Order { get; set; }

		/// <summary>
		/// Gets or sets the Action.
		/// </summary>
		/// <value>
		/// Action.
		/// </value>
		public string Action { get; set; }

		/// <summary>
		/// Gets or sets the Allow Or Deny.
		/// </summary>
		/// <value>
		/// Allow Or Deny.
		/// </value>
		public string AllowOrDeny { get; set; }

		/// <summary>
		/// Gets or sets the Special Role.
		/// </summary>
		/// <value>
		/// Special Role.
		/// </value>
		public int SpecialRole { get; set; }

		/// <summary>
		/// Gets or sets the Person Id.
		/// </summary>
		/// <value>
		/// Person Id.
		/// </value>
		public int? PersonId { get; set; }

		/// <summary>
		/// Gets or sets the Group Id.
		/// </summary>
		/// <value>
		/// Group Id.
		/// </value>
		public int? GroupId { get; set; }
	}
}
