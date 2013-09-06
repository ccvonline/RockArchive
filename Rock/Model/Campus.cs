//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Represents a physical or virtual Campus/Site for a church or ministry where worship services and other events are held.  
    /// </summary>
    /// <example>
    /// Three campuses for Christ's Church of the Valley: Peoria, Scottsdale and Surprise.
    /// </example>
    [Table( "Campus" )]
    [DataContract]
    public partial class Campus : Model<Campus>
    {
        #region Entity Properties


        /// <summary>
        /// Gets or sets a flag indicating if the Campus is a part of the RockChMS system/framework. 
        /// </summary>
        /// <value>
        /// A <see cref="System.Boolean"/> that is <c>true</c> if this Block is part of the RockChMS core system/framework, otherwise is <c>false</c>.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// Given Name.
        /// </value>
        [Required]
        [MaxLength( 100 )]
        [AlternateKey]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the campus abbreviation/short code.
        /// </summary>
        /// <value>
        /// Short Code.
        /// </value>
        [MaxLength( 50 )]
        [DataMember]
        public string ShortCode { get; set; }

        /// <summary>
        /// Gets or sets the location id.
        /// </summary>
        /// <value>
        /// The location id.
        /// </value>
        [DataMember]
        public int? LocationId { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [DataMember]
        public virtual Location Location { get; set; }

        #endregion

        #region Public Methods

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

        #endregion

    }

    #region Entity Configuration

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
            this.HasOptional( c => c.Location ).WithMany().HasForeignKey( c => c.LocationId ).WillCascadeOnDelete( false );
        }
    }

    #endregion

}
