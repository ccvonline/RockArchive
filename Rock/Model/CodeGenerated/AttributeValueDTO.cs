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
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Data Transfer Object for AttributeValue object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class AttributeValueDto : DtoSecured<AttributeValueDto>
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
        public override Dictionary<string, object> ToDictionary()
        {
            var dictionary = base.ToDictionary();
            dictionary.Add( "IsSystem", this.IsSystem );
            dictionary.Add( "AttributeId", this.AttributeId );
            dictionary.Add( "EntityId", this.EntityId );
            dictionary.Add( "Order", this.Order );
            dictionary.Add( "Value", this.Value );
            return dictionary;
        }

        /// <summary>
        /// Creates a dynamic object.
        /// </summary>
        /// <returns></returns>
        public override dynamic ToDynamic()
        {
            dynamic expando = base.ToDynamic();
            expando.IsSystem = this.IsSystem;
            expando.AttributeId = this.AttributeId;
            expando.EntityId = this.EntityId;
            expando.Order = this.Order;
            expando.Value = this.Value;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is AttributeValue )
            {
                var attributeValue = (AttributeValue)model;
                this.IsSystem = attributeValue.IsSystem;
                this.AttributeId = attributeValue.AttributeId;
                this.EntityId = attributeValue.EntityId;
                this.Order = attributeValue.Order;
                this.Value = attributeValue.Value;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyToModel ( IEntity model )
        {
            base.CopyToModel( model );

            if ( model is AttributeValue )
            {
                var attributeValue = (AttributeValue)model;
                attributeValue.IsSystem = this.IsSystem;
                attributeValue.AttributeId = this.AttributeId;
                attributeValue.EntityId = this.EntityId;
                attributeValue.Order = this.Order;
                attributeValue.Value = this.Value;
            }
        }

    }


    /// <summary>
    /// AttributeValue Extension Methods
    /// </summary>
    public static class AttributeValueExtensions
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

        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        public static string ToJson( this AttributeValue value, bool deep = false )
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject( ToDynamic( value, deep ) );
        }

        /// <summary>
        /// To the dynamic.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static List<dynamic> ToDynamic( this ICollection<AttributeValue> values )
        {
            var dynamicList = new List<dynamic>();
            foreach ( var value in values )
            {
                dynamicList.Add( value.ToDynamic( true ) );
            }
            return dynamicList;
        }

        /// <summary>
        /// To the dynamic.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        public static dynamic ToDynamic( this AttributeValue value, bool deep = false )
        {
            dynamic dynamicAttributeValue = new AttributeValueDto( value ).ToDynamic();

            if ( !deep )
            {
                return dynamicAttributeValue;
            }

            dynamicAttributeValue.Attribute = value.Attribute.ToDynamic();

            return dynamicAttributeValue;
        }

        /// <summary>
        /// Froms the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="json">The json.</param>
        public static void FromJson( this Page value, string json )
        {
            //Newtonsoft.Json.JsonConvert.PopulateObject( json, value );
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject( json, typeof( ExpandoObject ) );
            value.FromDynamic( obj, true );
        }

        /// <summary>
        /// Froms the dynamic.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        public static void FromDynamic( this AttributeValue value, object obj, bool deep = false )
        {
            new PageDto().FromDynamic(obj).CopyToModel(value);

            if (deep)
            {
                var expando = obj as ExpandoObject;
                if (obj != null)
                {
                    var dict = obj as IDictionary<string, object>;
                    if (dict != null)
                    {
                        new AttributeDto().FromDynamic( dict["Attribute"] ).CopyToModel(value.Attribute);

                    }
                }
            }
        }

    }
}