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

namespace Rock.Core
{
	/// <summary>
	/// Data Transfer Object for Attribute object
	/// </summary>
	public partial class AttributeDto : IDto
	{

#pragma warning disable 1591
		public bool IsSystem { get; set; }
		public int FieldTypeId { get; set; }
		public string Entity { get; set; }
		public string EntityQualifierColumn { get; set; }
		public string EntityQualifierValue { get; set; }
		public string Key { get; set; }
		public string Name { get; set; }
		public string Category { get; set; }
		public string Description { get; set; }
		public int Order { get; set; }
		public bool IsGridColumn { get; set; }
		public string DefaultValue { get; set; }
		public bool IsMultiValue { get; set; }
		public bool IsRequired { get; set; }
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
		public AttributeDto ()
		{
		}

		/// <summary>
		/// Instantiates a new DTO object from the model
		/// </summary>
		/// <param name="attribute"></param>
		public AttributeDto ( Attribute attribute )
		{
			CopyFromModel( attribute );
		}

		/// <summary>
		/// Copies the model property values to the DTO properties
		/// </summary>
		/// <param name="attribute"></param>
		public void CopyFromModel( IModel model )
		{
			if ( model is Attribute )
			{
				var attribute = (Attribute)model;
				this.IsSystem = attribute.IsSystem;
				this.FieldTypeId = attribute.FieldTypeId;
				this.Entity = attribute.Entity;
				this.EntityQualifierColumn = attribute.EntityQualifierColumn;
				this.EntityQualifierValue = attribute.EntityQualifierValue;
				this.Key = attribute.Key;
				this.Name = attribute.Name;
				this.Category = attribute.Category;
				this.Description = attribute.Description;
				this.Order = attribute.Order;
				this.IsGridColumn = attribute.IsGridColumn;
				this.DefaultValue = attribute.DefaultValue;
				this.IsMultiValue = attribute.IsMultiValue;
				this.IsRequired = attribute.IsRequired;
				this.CreatedDateTime = attribute.CreatedDateTime;
				this.ModifiedDateTime = attribute.ModifiedDateTime;
				this.CreatedByPersonId = attribute.CreatedByPersonId;
				this.ModifiedByPersonId = attribute.ModifiedByPersonId;
				this.Id = attribute.Id;
				this.Guid = attribute.Guid;
			}
		}

		/// <summary>
		/// Copies the DTO property values to the model properties
		/// </summary>
		/// <param name="attribute"></param>
		public void CopyToModel ( IModel model )
		{
			if ( model is Attribute )
			{
				var attribute = (Attribute)model;
				attribute.IsSystem = this.IsSystem;
				attribute.FieldTypeId = this.FieldTypeId;
				attribute.Entity = this.Entity;
				attribute.EntityQualifierColumn = this.EntityQualifierColumn;
				attribute.EntityQualifierValue = this.EntityQualifierValue;
				attribute.Key = this.Key;
				attribute.Name = this.Name;
				attribute.Category = this.Category;
				attribute.Description = this.Description;
				attribute.Order = this.Order;
				attribute.IsGridColumn = this.IsGridColumn;
				attribute.DefaultValue = this.DefaultValue;
				attribute.IsMultiValue = this.IsMultiValue;
				attribute.IsRequired = this.IsRequired;
				attribute.CreatedDateTime = this.CreatedDateTime;
				attribute.ModifiedDateTime = this.ModifiedDateTime;
				attribute.CreatedByPersonId = this.CreatedByPersonId;
				attribute.ModifiedByPersonId = this.ModifiedByPersonId;
				attribute.Id = this.Id;
				attribute.Guid = this.Guid;
			}
		}
	}
}
