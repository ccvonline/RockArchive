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

namespace Rock.Core
{
    /// <summary>
    /// Data Transfer Object for Audit object
    /// </summary>
    public partial class AuditDto : IDto
    {

#pragma warning disable 1591
        public int? EntityTypeId { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public AuditType AuditType { get; set; }
        public string Properties { get; set; }
        public DateTime? DateTime { get; set; }
        public int? PersonId { get; set; }
        public int Id { get; set; }
        public Guid Guid { get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public AuditDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="audit"></param>
        public AuditDto ( Audit audit )
        {
            CopyFromModel( audit );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add( "EntityTypeId", this.EntityTypeId );
            dictionary.Add( "EntityId", this.EntityId );
            dictionary.Add( "EntityName", this.EntityName );
            dictionary.Add( "AuditType", this.AuditType );
            dictionary.Add( "Properties", this.Properties );
            dictionary.Add( "DateTime", this.DateTime );
            dictionary.Add( "PersonId", this.PersonId );
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
            expando.EntityTypeId = this.EntityTypeId;
            expando.EntityId = this.EntityId;
            expando.EntityName = this.EntityName;
            expando.AuditType = this.AuditType;
            expando.Properties = this.Properties;
            expando.DateTime = this.DateTime;
            expando.PersonId = this.PersonId;
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
            if ( model is Audit )
            {
                var audit = (Audit)model;
                this.EntityTypeId = audit.EntityTypeId;
                this.EntityId = audit.EntityId;
                this.EntityName = audit.EntityName;
                this.AuditType = audit.AuditType;
                this.Properties = audit.Properties;
                this.DateTime = audit.DateTime;
                this.PersonId = audit.PersonId;
                this.Id = audit.Id;
                this.Guid = audit.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is Audit )
            {
                var audit = (Audit)model;
                audit.EntityTypeId = this.EntityTypeId;
                audit.EntityId = this.EntityId;
                audit.EntityName = this.EntityName;
                audit.AuditType = this.AuditType;
                audit.Properties = this.Properties;
                audit.DateTime = this.DateTime;
                audit.PersonId = this.PersonId;
                audit.Id = this.Id;
                audit.Guid = this.Guid;
            }
        }
    }
}
