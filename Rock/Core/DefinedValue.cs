//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Core
{
    /// <summary>
    /// Defined Value POCO Entity.
    /// </summary>
    [Table( "coreDefinedValue" )]
    public partial class DefinedValue : ModelWithAttributes<DefinedValue>, IAuditable, IOrdered
    {
		/// <summary>
		/// Gets or sets the System.
		/// </summary>
		/// <value>
		/// System.
		/// </value>
		[DataMember]
		public bool System { get; set; }
		
		/// <summary>
		/// Gets or sets the Defined Type Id.
		/// </summary>
		/// <value>
		/// Defined Type Id.
		/// </value>
		[DataMember]
		public int DefinedTypeId { get; set; }
		
		/// <summary>
		/// Gets or sets the Order.
		/// </summary>
		/// <value>
		/// Order.
		/// </value>
		[DataMember]
		public int Order { get; set; }
		
		/// <summary>
		/// Gets or sets the Name.
		/// </summary>
		/// <value>
		/// Name.
		/// </value>
		[MaxLength( 100 )]
		[DataMember]
		public string Name { get; set; }
		
		/// <summary>
		/// Gets or sets the Description.
		/// </summary>
		/// <value>
		/// Description.
		/// </value>
		[DataMember]
		public string Description { get; set; }
		
		/// <summary>
		/// Gets or sets the Created Date Time.
		/// </summary>
		/// <value>
		/// Created Date Time.
		/// </value>
		[DataMember]
		public DateTime? CreatedDateTime { get; set; }
		
		/// <summary>
		/// Gets or sets the Modified Date Time.
		/// </summary>
		/// <value>
		/// Modified Date Time.
		/// </value>
		[DataMember]
		public DateTime? ModifiedDateTime { get; set; }
		
		/// <summary>
		/// Gets or sets the Created By Person Id.
		/// </summary>
		/// <value>
		/// Created By Person Id.
		/// </value>
		[DataMember]
		public int? CreatedByPersonId { get; set; }
		
		/// <summary>
		/// Gets or sets the Modified By Person Id.
		/// </summary>
		/// <value>
		/// Modified By Person Id.
		/// </value>
		[DataMember]
		public int? ModifiedByPersonId { get; set; }
		
		/// <summary>
        /// Gets a Data Transfer Object (lightweight) version of this object.
        /// </summary>
        /// <value>
        /// A <see cref="Rock.Core.DTO.DefinedValue"/> object.
        /// </value>
		public Rock.Core.DTO.DefinedValue DataTransferObject
		{
			get 
			{ 
				Rock.Core.DTO.DefinedValue dto = new Rock.Core.DTO.DefinedValue();
				dto.Id = this.Id;
				dto.Guid = this.Guid;
				dto.System = this.System;
				dto.DefinedTypeId = this.DefinedTypeId;
				dto.Order = this.Order;
				dto.Name = this.Name;
				dto.Description = this.Description;
				dto.CreatedDateTime = this.CreatedDateTime;
				dto.ModifiedDateTime = this.ModifiedDateTime;
				dto.CreatedByPersonId = this.CreatedByPersonId;
				dto.ModifiedByPersonId = this.ModifiedByPersonId;
				return dto; 
			}
		}

        /// <summary>
        /// Gets the auth entity.
        /// </summary>
		[NotMapped]
		public override string AuthEntity { get { return "Core.DefinedValue"; } }
        
		/// <summary>
        /// Gets or sets the Defined Type.
        /// </summary>
        /// <value>
        /// A <see cref="DefinedType"/> object.
        /// </value>
		public virtual DefinedType DefinedType { get; set; }
        
		/// <summary>
        /// Gets or sets the Created By Person.
        /// </summary>
        /// <value>
        /// A <see cref="CRM.Person"/> object.
        /// </value>
		public virtual CRM.Person CreatedByPerson { get; set; }
        
		/// <summary>
        /// Gets or sets the Modified By Person.
        /// </summary>
        /// <value>
        /// A <see cref="CRM.Person"/> object.
        /// </value>
		public virtual CRM.Person ModifiedByPerson { get; set; }

    }
    /// <summary>
    /// Defined Value Configuration class.
    /// </summary>
    public partial class DefinedValueConfiguration : EntityTypeConfiguration<DefinedValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinedValueConfiguration"/> class.
        /// </summary>
        public DefinedValueConfiguration()
        {
			this.HasRequired( p => p.DefinedType ).WithMany( p => p.DefinedValues ).HasForeignKey( p => p.DefinedTypeId ).WillCascadeOnDelete(false);
			this.HasOptional( p => p.CreatedByPerson ).WithMany().HasForeignKey( p => p.CreatedByPersonId ).WillCascadeOnDelete(false);
			this.HasOptional( p => p.ModifiedByPerson ).WithMany().HasForeignKey( p => p.ModifiedByPersonId ).WillCascadeOnDelete(false);
		}
    }
}
