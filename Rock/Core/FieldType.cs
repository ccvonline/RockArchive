//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Core
{
    /// <summary>
    /// Field Type POCO Entity.
    /// </summary>
    [Table( "coreFieldType" )]
    public partial class FieldType : Model<FieldType>, IAuditable
    {
		/// <summary>
		/// Gets or sets the System.
		/// </summary>
		/// <value>
		/// System.
		/// </value>
		[Required]
		[DataMember]
		public bool IsSystem { get; set; }
		
		/// <summary>
		/// Gets or sets the Name.
		/// </summary>
		/// <value>
		/// Name.
		/// </value>
		[Required]
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
		/// Gets or sets the Assembly.
		/// </summary>
		/// <value>
		/// Assembly.
		/// </value>
		[Required]
		[MaxLength( 100 )]
		[DataMember]
		public string Assembly { get; set; }
		
		/// <summary>
		/// Gets or sets the Class.
		/// </summary>
		/// <value>
		/// Class.
		/// </value>
		[Required]
		[MaxLength( 100 )]
		[DataMember]
		public string Class { get; set; }
		
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
		/// Static Method to return an object based on the id
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public static FieldType Read( int id )
		{
			return Read<FieldType>( id );
		}
				
        /// <summary>
        /// Gets the auth entity.
        /// </summary>
		[NotMapped]
		public override string EntityTypeName { get { return "Core.FieldType"; } }
        
		/// <summary>
        /// Gets or sets the Defined Types.
        /// </summary>
        /// <value>
        /// Collection of Defined Types.
        /// </value>
		public virtual ICollection<DefinedType> DefinedTypes { get; set; }

		/// <summary>
		/// Gets or sets the Metrics.
		/// </summary>
		/// <value>
		/// Collection of Metrics.
		/// </value>
		//public virtual ICollection<Metric> Metrics { get; set; }
        
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

    }

    /// <summary>
    /// Field Type Configuration class.
    /// </summary>
    public partial class FieldTypeConfiguration : EntityTypeConfiguration<FieldType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldTypeConfiguration"/> class.
        /// </summary>
        public FieldTypeConfiguration()
        {
		}
    }
}
