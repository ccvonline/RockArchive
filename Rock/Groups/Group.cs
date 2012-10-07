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

namespace Rock.Groups
{
    /// <summary>
    /// Group POCO Entity.
    /// </summary>
    [Table( "groupsGroup" )]
    public partial class Group : Model<Group>, IAuditable
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
		/// Gets or sets the Parent Group Id.
		/// </summary>
		/// <value>
		/// Parent Group Id.
		/// </value>
		[DataMember]
		public int? ParentGroupId { get; set; }
		
		/// <summary>
		/// Gets or sets the Group Type Id.
		/// </summary>
		/// <value>
		/// Group Type Id.
		/// </value>
		[Required]
		[DataMember]
		public int GroupTypeId { get; set; }

		/// <summary>
		/// Gets or sets the Campus Id.
		/// </summary>
		/// <value>
		/// Campus Id.
		/// </value>
		[DataMember]
		public int? CampusId { get; set; }

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
		/// Gets or sets the Is Security Role.
		/// </summary>
		/// <value>
		/// Is Security Role.
		/// </value>
		[Required]
		[DataMember]
		public bool IsSecurityRole { get; set; }
		
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
		public static Group Read( int id )
		{
			return Read<Group>( id );
		}
		
        /// <summary>
        /// Gets the auth entity.
        /// </summary>
		[NotMapped]
		public override string EntityTypeName { get { return "Groups.Group"; } }
        
		/// <summary>
        /// Gets or sets the Groups.
        /// </summary>
        /// <value>
        /// Collection of Groups.
        /// </value>
		public virtual ICollection<Group> Groups { get; set; }
        
		/// <summary>
        /// Gets or sets the Members.
        /// </summary>
        /// <value>
        /// Collection of Members.
        /// </value>
		public virtual ICollection<Member> Members { get; set; }

		/// <summary>
		/// Gets or sets the Locations.
		/// </summary>
		/// <value>
		/// Collection of Locations.
		/// </value>
		public virtual ICollection<GroupLocation> Locations { get; set; }

		/// <summary>
        /// Gets or sets the Parent Group.
        /// </summary>
        /// <value>
        /// A <see cref="Group"/> object.
        /// </value>
		public virtual Group ParentGroup { get; set; }
        
		/// <summary>
        /// Gets or sets the Group Type.
        /// </summary>
        /// <value>
        /// A <see cref="GroupType"/> object.
        /// </value>
		public virtual GroupType GroupType { get; set; }

		/// <summary>
		/// Gets or sets the Campus.
		/// </summary>
		/// <value>
		/// A <see cref="Rock.Crm.Campus"/> object.
		/// </value>
		public virtual Rock.Crm.Campus Campus { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.Name;
		}

	}

    /// <summary>
    /// Group Configuration class.
    /// </summary>
    public partial class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupConfiguration"/> class.
        /// </summary>
        public GroupConfiguration()
        {
			this.HasOptional( p => p.ParentGroup ).WithMany( p => p.Groups ).HasForeignKey( p => p.ParentGroupId ).WillCascadeOnDelete(false);
			this.HasRequired( p => p.GroupType ).WithMany( p => p.Groups ).HasForeignKey( p => p.GroupTypeId ).WillCascadeOnDelete(false);
			this.HasOptional( p => p.Campus ).WithMany().HasForeignKey( p => p.CampusId).WillCascadeOnDelete( false );
		}
    }
}
