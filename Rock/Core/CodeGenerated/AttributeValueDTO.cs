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

namespace Rock.Core
{
    /// <summary>
    /// Data Transfer Object for AttributeValue object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class AttributeValueDto : IDto, DotLiquid.ILiquidizable
    {
        /// <summary />
        [DataMember]
        public bool IsSystem { get; set; }

        /// <summary />
        [DataMember]
        public int AttributeId { get; set; }

        /// <summary />
        [DataMember]
        public int? EntityId { get; set; }

        /// <summary />
        [DataMember]
        public int? Order { get; set; }

        /// <summary />
        [DataMember]
        public string Value { get; set; }

        /// <summary />
        [DataMember]
        public int Id { get; set; }

        /// <summary />
        [DataMember]
        public Guid Guid { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public AttributeValueDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="attributeValue"></param>
        public AttributeValueDto ( AttributeValue attributeValue )
        {
            CopyFromModel( attributeValue );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add( "IsSystem", this.IsSystem );
            dictionary.Add( "AttributeId", this.AttributeId );
            dictionary.Add( "EntityId", this.EntityId );
            dictionary.Add( "Order", this.Order );
            dictionary.Add( "Value", this.Value );
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
            expando.AttributeId = this.AttributeId;
            expando.EntityId = this.EntityId;
            expando.Order = this.Order;
            expando.Value = this.Value;
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
            if ( model is AttributeValue )
            {
                var attributeValue = (AttributeValue)model;
                this.IsSystem = attributeValue.IsSystem;
                this.AttributeId = attributeValue.AttributeId;
                this.EntityId = attributeValue.EntityId;
                this.Order = attributeValue.Order;
                this.Value = attributeValue.Value;
                this.Id = attributeValue.Id;
                this.Guid = attributeValue.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is AttributeValue )
            {
                var attributeValue = (AttributeValue)model;
                attributeValue.IsSystem = this.IsSystem;
                attributeValue.AttributeId = this.AttributeId;
                attributeValue.EntityId = this.EntityId;
                attributeValue.Order = this.Order;
                attributeValue.Value = this.Value;
                attributeValue.Id = this.Id;
                attributeValue.Guid = this.Guid;
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
    public static class AttributeValueDtoExtension
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static AttributeValue ToModel( this AttributeValueDto value )
        {
            AttributeValue result = new AttributeValue();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<AttributeValue> ToModel( this List<AttributeValueDto> value )
        {
            List<AttributeValue> result = new List<AttributeValue>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<AttributeValueDto> ToDto( this List<AttributeValue> value )
        {
            List<AttributeValueDto> result = new List<AttributeValueDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static AttributeValueDto ToDto( this AttributeValue value )
        {
            return new AttributeValueDto( value );
        }

    }
}