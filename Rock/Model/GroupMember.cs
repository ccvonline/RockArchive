﻿// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Serialization;
using Humanizer;
using Rock.Data;
using Rock.Web.Cache;

namespace Rock.Model
{
    /// <summary>
    /// Represents a member of a group in Rock. A group member is a <see cref="Rock.Model.Person"/> who has a relationship with a <see cref="Rock.Model.Group"/>.
    /// </summary>
    [Table( "GroupMember" )]
    [DataContract]
    public partial class GroupMember : Model<GroupMember>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets a flag indicating if this GroupMember is part of the Rock core system/framework. This property is required.
        /// </summary>
        /// <value>
        /// A <see cref="System.Boolean"/> value that is <c>true</c> if this GroupMember is a part of the Rock core system/framework; otherwise <c>false</c>.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public bool IsSystem { get; set; }
        
        /// <summary>
        /// Gets or sets the Id of the <see cref="Rock.Model.Group"/> that this GroupMember is associated with. This property is required.
        /// </summary>
        /// <value>
        /// An <see cref="System.Int32"/> representing the Id of the <see cref="Rock.Model.Group"/> that the GroupMember is associated with.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int GroupId { get; set; }
        
        /// <summary>
        /// Gets or sets the Id of the <see cref="Rock.Model.Person"/> that is represented by the GroupMember. This property is required.
        /// </summary>
        /// <value>
        /// An <see cref="System.Int32"/> representing the Id of the <see cref="Rock.Model.Person"/> who is reprensented by the GroupMember.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int PersonId { get; set; }
        
        /// <summary>
        /// Gets or sets the Id of the GroupMember's <see cref="GroupRole"/> in the <see cref="Rock.Model.Group"/>. This property is required.
        /// </summary>
        /// <value>
        /// An <see cref="System.Int32"/> representing the Id of the <see cref="Rock.Model.GroupTypeRole"/> that the Group Member is in.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int GroupRoleId { get; set; }

        /// <summary>
        /// Gets or sets the GroupMember's status in the Group. This value is required.
        /// </summary>
        /// <value>
        /// A <see cref="Rock.Model.GroupMemberStatus"/> enum value that represents the GroupMember's status in the group.  A <c>GroupMemberStatus.Active</c> indicates that the GroupMember is active,
        /// A <c>GroupMemberStatus.Inactive</c> value indicates that the GroupMember is not active, otherwise their GroupMemberStatus will be <c>GroupMemberStatus.Pending</c>.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public GroupMemberStatus GroupMemberStatus { get; set; }

        /// <summary>
        /// Gets or sets the number of additional guests that member will be bring to group.  Only applies when group has the 'AllowGuests' flag set to true.
        /// </summary>
        /// <value>
        /// The guest count.
        /// </value>
        [DataMember]
        public int? GuestCount { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.Person"/> representing the GroupMember.
        /// </summary>
        /// <value>
        /// A <see cref="Rock.Model.Person"/> representing the person who is the GroupMember.
        /// </value>
        [DataMember]
        public virtual Model.Person Person { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.Group"/> that the GroupMember belongs to.
        /// </summary>
        /// <value>
        /// A <see cref="Rock.Model.Group"/> representing the Group that the GroupMember is a part of.
        /// </value>
        public virtual Group Group { get; set; }
        
        /// <summary>
        /// Gets or sets the the GroupMember's role in the <see cref="Rock.Model.Group"/>.
        /// </summary>
        /// <value>
        /// A <see cref="Rock.Model.GroupTypeRole"/> representing the GroupMember's <see cref="Rock.Model.GroupTypeRole"/> in the <see cref="Rock.Model.Group"/>.
        /// </value>
        [DataMember]
        public virtual GroupTypeRole GroupRole { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Person.ToStringSafe();
        }

        /// <summary>
        /// Pres the save changes.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="state">The state.</param>
        public override void PreSaveChanges( DbContext dbContext, System.Data.Entity.EntityState state )
        {
            string action = string.Empty;
            if ( state == System.Data.Entity.EntityState.Added)
            {
                action = "Added to group.";
            }
            else if ( state == System.Data.Entity.EntityState.Deleted)
            {
                action = "Removed from group.";
            }

            if (!string.IsNullOrWhiteSpace(action))
            {
                var rockContext = (RockContext)dbContext;

                var group = this.Group;
                if ( group == null )
                {
                    group = new GroupService( rockContext ).Get( this.GroupId );
                }

                if (group != null)
                {
                    var personEntityTypeId = EntityTypeCache.Read( "Rock.Model.Person" ).Id;
                    var groupEntityTypeId = EntityTypeCache.Read( "Rock.Model.Group" ).Id;
                    var groupMembershipCategoryId = CategoryCache.Read( Rock.SystemGuid.Category.HISTORY_PERSON_GROUP_MEMBERSHIP.AsGuid(), rockContext ).Id;

                    new HistoryService( rockContext ).Add( new History
                    {
                        EntityTypeId = personEntityTypeId,
                        CategoryId = groupMembershipCategoryId,
                        EntityId = this.PersonId,
                        Summary = action,
                        Caption = group.Name,
                        RelatedEntityTypeId = groupEntityTypeId,
                        RelatedEntityId = this.GroupId
                    } );
                }
            }

            base.PreSaveChanges( dbContext, state );
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public override bool IsValid
        {
            get
            {
                var result = base.IsValid;
                if ( result )
                {
                    string errorMessage;
                    if ( !ValidateGroupMembership( new RockContext(), out errorMessage ) )
                    {
                        ValidationResults.Add( new ValidationResult( errorMessage ) );
                        result = false;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Validates the group membership.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        private bool ValidateGroupMembership( RockContext rockContext, out string errorMessage )
        {
            errorMessage = string.Empty;
            GroupMemberService groupMemberService = new GroupMemberService( rockContext );
            var groupRole = this.GroupRole ?? new GroupTypeRoleService( rockContext ).Get( this.GroupRoleId );

            // check to see if the person is alread a member of the gorup/role
            var existingGroupMembership = groupMemberService.GetByGroupIdAndPersonId( this.GroupId, this.PersonId );
            if ( existingGroupMembership.Any( a => a.GroupRoleId == this.GroupRoleId && a.Id != this.Id ) )
            {
                var person = this.Person ?? new PersonService( rockContext ).Get( this.PersonId );

                errorMessage = string.Format(
                    "{0} already belongs to the {1} role for this {2}, and cannot be added again with the same role",
                    person,
                    groupRole.Name,
                    groupRole.GroupType.GroupTerm );

                return false;
            }

            var databaseRecord = existingGroupMembership.FirstOrDefault( a => a.Id == this.Id );

            int memberCountInRole = new GroupMemberService( rockContext ).Queryable()
                .Where( m =>
                    m.GroupId == this.GroupId &&
                    m.GroupRoleId == this.GroupRoleId &&
                    m.GroupMemberStatus == GroupMemberStatus.Active )
                .Count();

            bool roleMembershipAboveMax = false;

            // if adding new active group member..
            if ( this.Id.Equals( 0 ) && this.GroupMemberStatus == GroupMemberStatus.Active )
            {
                // verify that active count has not exceeded the max
                if ( groupRole.MaxCount != null && ( memberCountInRole + 1 ) > groupRole.MaxCount )
                {
                    roleMembershipAboveMax = true;
                }
            }
            else if ( this.Id > 0 && ( this.GroupRoleId != databaseRecord.GroupRoleId || this.GroupMemberStatus != databaseRecord.GroupMemberStatus )
                    && this.GroupMemberStatus == GroupMemberStatus.Active )
            {
                // if existing group member changing role or status..
                // verify that active count has not exceeded the max
                if ( groupRole.MaxCount != null && ( memberCountInRole + 1 ) > groupRole.MaxCount )
                {
                    roleMembershipAboveMax = true;
                }
            }

            // throw error if above max.. do not proceed
            if ( roleMembershipAboveMax )
            {
                errorMessage = string.Format(
            "The number of {0} for this {1} is at or above its maximum allowed limit of {2:N0} active {3}.",
            groupRole.Name.Pluralize(),
            groupRole.GroupType.GroupTerm,
            groupRole.MaxCount,
            groupRole.GroupType.GroupMemberTerm.Pluralize( groupRole.MaxCount == 1 ) );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool IsEqualTo( GroupMember other )
        {
            return ( 
                this.GroupId == other.GroupId && 
                this.PersonId == other.PersonId && 
                this.GroupRoleId == other.GroupRoleId 
           );
        }

        #endregion

    }

    #region Entity Configuration

    /// <summary>
    /// Member Configuration class.
    /// </summary>
    public partial class GroupMemberConfiguration : EntityTypeConfiguration<GroupMember>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupMemberConfiguration"/> class.
        /// </summary>
        public GroupMemberConfiguration()
        {
            this.HasRequired( p => p.Person ).WithMany( p => p.Members ).HasForeignKey( p => p.PersonId ).WillCascadeOnDelete(true);
            this.HasRequired( p => p.Group ).WithMany( p => p.Members ).HasForeignKey( p => p.GroupId ).WillCascadeOnDelete(true);
            this.HasRequired( p => p.GroupRole ).WithMany().HasForeignKey( p => p.GroupRoleId ).WillCascadeOnDelete(false);
        }
    }

    #endregion

    #region Enumerations

    /// <summary>
    /// Represents the status of a <see cref="Rock.Model.GroupMember"/> in a <see cref="Rock.Model.Group"/>.
    /// </summary>
    public enum GroupMemberStatus
    {
        /// <summary>
        /// The <see cref="Rock.Model.GroupMember"/> is not an active member of the <see cref="Rock.Model.Group"/>.
        /// </summary>
        Inactive = 0,

        /// <summary>
        /// The <see cref="Rock.Model.GroupMember"/> is an active member of the <see cref="Rock.Model.Group"/>.
        /// </summary>
        Active = 1,
        
        /// <summary>
        /// The <see cref="Rock.Model.GroupMember">GroupMember's</see> membership in the <see cref="Rock.Model.Group"/> is pending.
        /// </summary>
        Pending = 2 
    }

    #endregion
}
