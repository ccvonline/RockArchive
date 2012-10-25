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
using System.Collections.Generic;
using System.Dynamic;

using Rock.Data;

namespace Rock.Crm
{
    /// <summary>
    /// Data Transfer Object for GroupMember object
    /// </summary>
    public partial class GroupMemberDto : IDto
    {

#pragma warning disable 1591
        public bool IsSystem { get; set; }
        public int GroupId { get; set; }
        public int PersonId { get; set; }
        public int GroupRoleId { get; set; }
        public int Id { get; set; }
        public Guid Guid { get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public GroupMemberDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="groupMember"></param>
        public GroupMemberDto ( GroupMember groupMember )
        {
            CopyFromModel( groupMember );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add( "IsSystem", this.IsSystem );
            dictionary.Add( "GroupId", this.GroupId );
            dictionary.Add( "PersonId", this.PersonId );
            dictionary.Add( "GroupRoleId", this.GroupRoleId );
            dictionary.Add( "Id", this.Id );
            dictionary.Add( "Guid", this.Guid );
            return dictionary;
        }

        /// <summary>
        /// Creates a dynamic object.
        /// </summary>
        /// <returns></returns>
        public virtual dynamic ToDynamic()
        {
            dynamic expando = new ExpandoObject();
            expando.IsSystem = this.IsSystem;
            expando.GroupId = this.GroupId;
            expando.PersonId = this.PersonId;
            expando.GroupRoleId = this.GroupRoleId;
            expando.Id = this.Id;
            expando.Guid = this.Guid;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyFromModel( IEntity model )
        {
            if ( model is GroupMember )
            {
                var groupMember = (GroupMember)model;
                this.IsSystem = groupMember.IsSystem;
                this.GroupId = groupMember.GroupId;
                this.PersonId = groupMember.PersonId;
                this.GroupRoleId = groupMember.GroupRoleId;
                this.Id = groupMember.Id;
                this.Guid = groupMember.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is GroupMember )
            {
                var groupMember = (GroupMember)model;
                groupMember.IsSystem = this.IsSystem;
                groupMember.GroupId = this.GroupId;
                groupMember.PersonId = this.PersonId;
                groupMember.GroupRoleId = this.GroupRoleId;
                groupMember.Id = this.Id;
                groupMember.Guid = this.Guid;
            }
        }
    }
}
