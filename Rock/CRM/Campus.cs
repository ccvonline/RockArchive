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
using System.Web;

using Rock.Data;

namespace Rock.CRM
{
    /// <summary>
    /// Campus POCO Entity.
    /// </summary>
    [Table( "crmCampus" )]
    public partial class Campus : ModelWithAttributes<Campus>, IAuditable
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
        /// Given Name.
        /// </value>
        [MaxLength( 100 )]
        [DataMember]
        public string Name { get; set; }

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
        /// Gets the auth entity.
        /// </summary>
        [NotMapped]
        public override string AuthEntity { get { return "CRM.Campus"; } }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

    }

    /// <summary>
    /// Campus Configuration class.
    /// </summary>
    public partial class CampusConfiguration : EntityTypeConfiguration<Campus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampusConfiguration"/> class.
        /// </summary>
        public CampusConfiguration()
        {
        }
    }

    /// <summary>
    /// Data Transformation Object
    /// </summary>
    public partial class CampusDTO : DTO<Campus>
    {
        /// <summary>
        /// Gets or sets the System.
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// Given Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Instantiate new DTO object
        /// </summary>
        public CampusDTO()
        {
        }

        /// <summary>
        /// Instantiate new DTO object from Model
        /// </summary>
        /// <param name="auth"></param>
        public CampusDTO( Campus campus )
        {
            CopyFromModel( campus );
        }

        /// <summary>
        /// Copy DTO to Model
        /// </summary>
        /// <param name="campus"></param>
        public override void CopyFromModel( Campus campus )
        {
            this.Id = campus.Id;
            this.Guid = campus.Guid;
            this.IsSystem = campus.IsSystem;
            this.Name = campus.Name;
        }

        /// <summary>
        /// Copy Model to DTO
        /// </summary>
        /// <param name="campus"></param>
        public override void CopyToModel( Campus campus )
        {
            campus.Id = this.Id;
            campus.Guid = this.Guid;
            campus.IsSystem = this.IsSystem;
            campus.Name = this.Name;
        }
    }
}
