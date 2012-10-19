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

namespace Rock.Groups
{
    /// <summary>
    /// Data Transfer Object for GroupRole object
    /// </summary>
    public partial class GroupRoleDto : IDto
    {

#pragma warning disable 1591
        public bool IsSystem { get; set; }
        public int? GroupTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }
        public int Id { get; set; }
        public Guid Guid { get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public GroupRoleDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="groupRole"></param>
        public GroupRoleDto ( GroupRole groupRole )
        {
            CopyFromModel( groupRole );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add( "IsSystem", this.IsSystem );
            dictionary.Add( "GroupTypeId", this.GroupTypeId );
            dictionary.Add( "Name", this.Name );
            dictionary.Add( "Description", this.Description );
            dictionary.Add( "Order", this.Order );
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
            expando.GroupTypeId = this.GroupTypeId;
            expando.Name = this.Name;
            expando.Description = this.Description;
            expando.Order = this.Order;
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
            if ( model is GroupRole )
            {
                var groupRole = (GroupRole)model;
                this.IsSystem = groupRole.IsSystem;
                this.GroupTypeId = groupRole.GroupTypeId;
                this.Name = groupRole.Name;
                this.Description = groupRole.Description;
                this.Order = groupRole.Order;
                this.Id = groupRole.Id;
                this.Guid = groupRole.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is GroupRole )
            {
                var groupRole = (GroupRole)model;
                groupRole.IsSystem = this.IsSystem;
                groupRole.GroupTypeId = this.GroupTypeId;
                groupRole.Name = this.Name;
                groupRole.Description = this.Description;
                groupRole.Order = this.Order;
                groupRole.Id = this.Id;
                groupRole.Guid = this.Guid;
            }
        }
    }
}
