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
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Core
{
    /// <summary>
    /// Metric POCO Entity.
    /// </summary>
    [Table( "coreMetric" )]
    public partial class Metric : Model<Metric>, IAuditable, IOrdered
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
		/// Gets or sets the Type.
		/// </summary>
		/// <value>
		/// Type.
		/// </value>
		[DataMember]
		public bool Type { get; set; }
		
		/// <summary>
		/// Gets or sets the Category.
		/// </summary>
		/// <value>
		/// Category.
		/// </value>
		[MaxLength( 100 )]
		[DataMember]
		public string Category { get; set; }
		
		/// <summary>
		/// Gets or sets the Title.
		/// </summary>
		/// <value>
		/// Title.
		/// </value>
		[Required]
		[MaxLength( 100 )]
		[DataMember]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the Subtitle.
		/// </summary>
		/// <value>
		/// Subtitle.
		/// </value>
		[MaxLength( 100 )]
		[DataMember]
		public string Subtitle { get; set; }
	
		/// <summary>
		/// Gets or sets the Description.
		/// </summary>
		/// <value>
		/// Description.
		/// </value>
		[DataMember]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the MinValue.
		/// </summary>
		/// <value>
		/// MinValue.
		/// </value>
		[DataMember]
		public int? MinValue { get; set; }

		/// <summary>
		/// Gets or sets the MaxValue.
		/// </summary>
		/// <value>
		/// MaxValue.
		/// </value>
		[DataMember]
		public int? MaxValue { get; set; }

		/// <summary>
		/// Gets or sets the CollectionFrequency.
		/// </summary>
		/// <value>
		/// CollectionFrequency.
		/// </value>
		[DataMember]
		public int? CollectionFrequencyId { get; set; }

		/// <summary>
		/// Gets or sets the LastCollected Date Time.
		/// </summary>
		/// <value>
		/// LastCollected Date Time.
		/// </value>
		[DataMember]
		public DateTime? LastCollected { get; set; }

		/// <summary>
		/// Gets or sets the Source.
		/// </summary>
		/// <value>
		/// Source.
		/// </value>
		[MaxLength( 100 )]
		[DataMember]
		public string Source { get; set; }

		/// <summary>
		/// Gets or sets the SourceSQL.
		/// </summary>
		/// <value>
		/// SourceSQL.
		/// </value>
		[DataMember]
		public string SourceSQL { get; set; }

		/// <summary>
		/// Gets or sets the Order.
		/// </summary>
		/// <value>
		/// Order.
		/// </value>
		[Required]
		[DataMember]
		public int Order { get; set; }
		
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
		public static Metric Read( int id )
		{
			return Read<Metric>( id );
		}

        /// <summary>
        /// Gets the auth entity.
        /// </summary>
		[NotMapped]
		public override string EntityTypeName { get { return "Core.Metric"; } }
        
		/// <summary>
        /// Gets or sets the Metric Values.
        /// </summary>
        /// <value>
        /// Collection of Metric Values.
        /// </value>
		public virtual ICollection<MetricValue> MetricValues { get; set; }

		/// <summary>
		/// Gets or sets the CollectionFrequency.
		/// </summary>
		/// <value>
		/// A <see cref="Core.DefinedValue"/> object.
		/// </value>
		public virtual Core.DefinedValue CollectionFrequency { get; set; }

        /// <summary>
        /// Gets the parent authority.
        /// </summary>
        public override Security.ISecured ParentAuthority
        {
            get { return new Security.GenericEntity( "Global" ); }
        }

    }
    /// <summary>
    /// Metric Configuration class.
    /// </summary>
    public partial class MetricConfiguration : EntityTypeConfiguration<Metric>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetricConfiguration"/> class.
        /// </summary>
        public MetricConfiguration()
        {
			this.HasOptional( p => p.CollectionFrequency ).WithMany().HasForeignKey( p => p.CollectionFrequencyId ).WillCascadeOnDelete( false );
		}
    }
}
