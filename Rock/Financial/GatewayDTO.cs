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

namespace Rock.Financial
{
    /// <summary>
    /// Data Transfer Object for Gateway object
    /// </summary>
    public partial class GatewayDto : IDto
    {

#pragma warning disable 1591
		public string Name { get; set; }
		public string Description { get; set; }
		public string ApiUrl { get; set; }
		public string ApiKey { get; set; }
		public string ApiSecret { get; set; }
		public int Id { get; set; }
		public Guid Guid { get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public GatewayDto ()
        {
        }

		/// <summary>
		/// Instantiates a new DTO object from the entity
		/// </summary>
		/// <param name="gateway"></param>
		public GatewayDto ( Gateway gateway )
		{
			CopyFromModel( gateway );
		}

		/// <summary>
		/// Copies the model property values to the DTO properties
		/// </summary>
		/// <param name="model">The model</param>
		public void CopyFromModel( IEntity model )
		{
			if ( model is Gateway )
			{
				var gateway = (Gateway)model;
				this.Name = gateway.Name;
				this.Description = gateway.Description;
				this.ApiUrl = gateway.ApiUrl;
				this.ApiKey = gateway.ApiKey;
				this.ApiSecret = gateway.ApiSecret;
				this.Id = gateway.Id;
				this.Guid = gateway.Guid;
			}
		}

		/// <summary>
		/// Copies the DTO property values to the entity properties
		/// </summary>
		/// <param name="model">The model</param>
		public void CopyToModel ( IEntity model )
		{
			if ( model is Gateway )
			{
				var gateway = (Gateway)model;
				gateway.Name = this.Name;
				gateway.Description = this.Description;
				gateway.ApiUrl = this.ApiUrl;
				gateway.ApiKey = this.ApiKey;
				gateway.ApiSecret = this.ApiSecret;
				gateway.Id = this.Id;
				gateway.Guid = this.Guid;
			}
		}
	}
}
