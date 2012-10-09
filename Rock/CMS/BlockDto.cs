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
    /// Data Transfer Object for Block object
    /// </summary>
    public partial class BlockDto : IDto
    {

#pragma warning disable 1591
		public bool IsSystem { get; set; }
		public int? PageId { get; set; }
		public string Layout { get; set; }
		public int BlockTypeId { get; set; }
		public string Zone { get; set; }
		public int Order { get; set; }
		public string Name { get; set; }
		public int OutputCacheDuration { get; set; }
		public int Id { get; set; }
		public Guid Guid { get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public BlockDto ()
        {
        }

		/// <summary>
		/// Instantiates a new DTO object from the entity
		/// </summary>
		/// <param name="block"></param>
		public BlockDto ( Block block )
		{
			CopyFromModel( block );
		}

		/// <summary>
		/// Copies the model property values to the DTO properties
		/// </summary>
		/// <param name="model">The model</param>
		public void CopyFromModel( IEntity model )
		{
			if ( model is Block )
			{
				var block = (Block)model;
				this.IsSystem = block.IsSystem;
				this.PageId = block.PageId;
				this.Layout = block.Layout;
				this.BlockTypeId = block.BlockTypeId;
				this.Zone = block.Zone;
				this.Order = block.Order;
				this.Name = block.Name;
				this.OutputCacheDuration = block.OutputCacheDuration;
				this.Id = block.Id;
				this.Guid = block.Guid;
			}
		}

		/// <summary>
		/// Copies the DTO property values to the entity properties
		/// </summary>
		/// <param name="model">The model</param>
		public void CopyToModel ( IEntity model )
		{
			if ( model is Block )
			{
				var block = (Block)model;
				block.IsSystem = this.IsSystem;
				block.PageId = this.PageId;
				block.Layout = this.Layout;
				block.BlockTypeId = this.BlockTypeId;
				block.Zone = this.Zone;
				block.Order = this.Order;
				block.Name = this.Name;
				block.OutputCacheDuration = this.OutputCacheDuration;
				block.Id = this.Id;
				block.Guid = this.Guid;
			}
		}
	}
}
