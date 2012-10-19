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

namespace Rock.Financial
{
    /// <summary>
    /// Data Transfer Object for Batch object
    /// </summary>
    public partial class BatchDto : IDto
    {

#pragma warning disable 1591
        public string Name { get; set; }
        public DateTime? BatchDate { get; set; }
        public bool IsClosed { get; set; }
        public int? CampusId { get; set; }
        public string Entity { get; set; }
        public int? EntityId { get; set; }
        public string ForeignReference { get; set; }
        public int Id { get; set; }
        public Guid Guid { get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public BatchDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="batch"></param>
        public BatchDto ( Batch batch )
        {
            CopyFromModel( batch );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add( "Name", this.Name );
            dictionary.Add( "BatchDate", this.BatchDate );
            dictionary.Add( "IsClosed", this.IsClosed );
            dictionary.Add( "CampusId", this.CampusId );
            dictionary.Add( "Entity", this.Entity );
            dictionary.Add( "EntityId", this.EntityId );
            dictionary.Add( "ForeignReference", this.ForeignReference );
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
            expando.Name = this.Name;
            expando.BatchDate = this.BatchDate;
            expando.IsClosed = this.IsClosed;
            expando.CampusId = this.CampusId;
            expando.Entity = this.Entity;
            expando.EntityId = this.EntityId;
            expando.ForeignReference = this.ForeignReference;
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
            if ( model is Batch )
            {
                var batch = (Batch)model;
                this.Name = batch.Name;
                this.BatchDate = batch.BatchDate;
                this.IsClosed = batch.IsClosed;
                this.CampusId = batch.CampusId;
                this.Entity = batch.Entity;
                this.EntityId = batch.EntityId;
                this.ForeignReference = batch.ForeignReference;
                this.Id = batch.Id;
                this.Guid = batch.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is Batch )
            {
                var batch = (Batch)model;
                batch.Name = this.Name;
                batch.BatchDate = this.BatchDate;
                batch.IsClosed = this.IsClosed;
                batch.CampusId = this.CampusId;
                batch.Entity = this.Entity;
                batch.EntityId = this.EntityId;
                batch.ForeignReference = this.ForeignReference;
                batch.Id = this.Id;
                batch.Guid = this.Guid;
            }
        }
    }
}
