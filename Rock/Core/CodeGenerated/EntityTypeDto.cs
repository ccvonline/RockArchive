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
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Data Transfer Object for EntityType object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class EntityTypeDto : IDto, DotLiquid.ILiquidizable
    {
        /// <summary />
        [DataMember]
        public string Name { get; set; }

        /// <summary />
        [DataMember]
        public string FriendlyName { get; set; }

        /// <summary />
        [DataMember]
        public int Id { get; set; }

        /// <summary />
        [DataMember]
        public Guid Guid { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public EntityTypeDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="entityType"></param>
        public EntityTypeDto ( EntityType entityType )
        {
            CopyFromModel( entityType );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add( "Name", this.Name );
            dictionary.Add( "FriendlyName", this.FriendlyName );
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
            expando.FriendlyName = this.FriendlyName;
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
            if ( model is EntityType )
            {
                var entityType = (EntityType)model;
                this.Name = entityType.Name;
                this.FriendlyName = entityType.FriendlyName;
                this.Id = entityType.Id;
                this.Guid = entityType.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is EntityType )
            {
                var entityType = (EntityType)model;
                entityType.Name = this.Name;
                entityType.FriendlyName = this.FriendlyName;
                entityType.Id = this.Id;
                entityType.Guid = this.Guid;
            }
        }

        /// <summary>
        /// Converts to liquidizable object for dotLiquid templating
        /// </summary>
        /// <returns></returns>
        public object ToLiquid()
        {
            return this.ToDictionary();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class EntityTypeDtoExtension
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static EntityType ToModel( this EntityTypeDto value )
        {
            EntityType result = new EntityType();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<EntityType> ToModel( this List<EntityTypeDto> value )
        {
            List<EntityType> result = new List<EntityType>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<EntityTypeDto> ToDto( this List<EntityType> value )
        {
            List<EntityTypeDto> result = new List<EntityTypeDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static EntityTypeDto ToDto( this EntityType value )
        {
            return new EntityTypeDto( value );
        }

    }
}