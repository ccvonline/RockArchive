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
    /// Data Transfer Object for EntityChange object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class EntityChangeDto : DtoSecured<EntityChangeDto>
    {
        /// <summary />
        [DataMember]
        public Guid ChangeSet { get; set; }

        /// <summary />
        [DataMember]
        public string ChangeType { get; set; }

        /// <summary />
        [DataMember]
        public int EntityTypeId { get; set; }

        /// <summary />
        [DataMember]
        public int EntityId { get; set; }

        /// <summary />
        [DataMember]
        public string Property { get; set; }

        /// <summary />
        [DataMember]
        public string OriginalValue { get; set; }

        /// <summary />
        [DataMember]
        public string CurrentValue { get; set; }

        /// <summary />
        [DataMember]
        public DateTime? CreatedDateTime { get; set; }

        /// <summary />
        [DataMember]
        public int? CreatedByPersonId { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public EntityChangeDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="entityChange"></param>
        public EntityChangeDto ( EntityChange entityChange )
        {
            CopyFromModel( entityChange );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> ToDictionary()
        {
            var dictionary = base.ToDictionary();
            dictionary.Add( "ChangeSet", this.ChangeSet );
            dictionary.Add( "ChangeType", this.ChangeType );
            dictionary.Add( "EntityTypeId", this.EntityTypeId );
            dictionary.Add( "EntityId", this.EntityId );
            dictionary.Add( "Property", this.Property );
            dictionary.Add( "OriginalValue", this.OriginalValue );
            dictionary.Add( "CurrentValue", this.CurrentValue );
            dictionary.Add( "CreatedDateTime", this.CreatedDateTime );
            dictionary.Add( "CreatedByPersonId", this.CreatedByPersonId );
            return dictionary;
        }

        /// <summary>
        /// Creates a dynamic object.
        /// </summary>
        /// <returns></returns>
        public override dynamic ToDynamic()
        {
            dynamic expando = base.ToDynamic();
            expando.ChangeSet = this.ChangeSet;
            expando.ChangeType = this.ChangeType;
            expando.EntityTypeId = this.EntityTypeId;
            expando.EntityId = this.EntityId;
            expando.Property = this.Property;
            expando.OriginalValue = this.OriginalValue;
            expando.CurrentValue = this.CurrentValue;
            expando.CreatedDateTime = this.CreatedDateTime;
            expando.CreatedByPersonId = this.CreatedByPersonId;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is EntityChange )
            {
                var entityChange = (EntityChange)model;
                this.ChangeSet = entityChange.ChangeSet;
                this.ChangeType = entityChange.ChangeType;
                this.EntityTypeId = entityChange.EntityTypeId;
                this.EntityId = entityChange.EntityId;
                this.Property = entityChange.Property;
                this.OriginalValue = entityChange.OriginalValue;
                this.CurrentValue = entityChange.CurrentValue;
                this.CreatedDateTime = entityChange.CreatedDateTime;
                this.CreatedByPersonId = entityChange.CreatedByPersonId;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyToModel ( IEntity model )
        {
            base.CopyToModel( model );

            if ( model is EntityChange )
            {
                var entityChange = (EntityChange)model;
                entityChange.ChangeSet = this.ChangeSet;
                entityChange.ChangeType = this.ChangeType;
                entityChange.EntityTypeId = this.EntityTypeId;
                entityChange.EntityId = this.EntityId;
                entityChange.Property = this.Property;
                entityChange.OriginalValue = this.OriginalValue;
                entityChange.CurrentValue = this.CurrentValue;
                entityChange.CreatedDateTime = this.CreatedDateTime;
                entityChange.CreatedByPersonId = this.CreatedByPersonId;
            }
        }

    }


    /// <summary>
    /// EntityChange Extension Methods
    /// </summary>
    public static class EntityChangeExtensions
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static EntityChange ToModel( this EntityChangeDto value )
        {
            EntityChange result = new EntityChange();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<EntityChange> ToModel( this List<EntityChangeDto> value )
        {
            List<EntityChange> result = new List<EntityChange>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<EntityChangeDto> ToDto( this List<EntityChange> value )
        {
            List<EntityChangeDto> result = new List<EntityChangeDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static EntityChangeDto ToDto( this EntityChange value )
        {
            return new EntityChangeDto( value );
        }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        public static string ToJson( this EntityChange value, bool deep = false )
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject( ToDynamic( value, deep ) );
        }

        /// <summary>
        /// To the dynamic.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static List<dynamic> ToDynamic( this ICollection<EntityChange> values )
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
        public static dynamic ToDynamic( this EntityChange value, bool deep = false )
        {
            dynamic dynamicEntityChange = new EntityChangeDto( value ).ToDynamic();

            if ( !deep )
            {
                return dynamicEntityChange;
            }

            dynamicEntityChange.EntityType = value.EntityType.ToDynamic();
            dynamicEntityChange.CreatedByPerson = value.CreatedByPerson.ToDynamic();

            return dynamicEntityChange;
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
        public static void FromDynamic( this EntityChange value, object obj, bool deep = false )
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
                        new EntityTypeDto().FromDynamic( dict["EntityType"] ).CopyToModel(value.EntityType);
                        new PersonDto().FromDynamic( dict["CreatedByPerson"] ).CopyToModel(value.CreatedByPerson);

                    }
                }
            }
        }

    }
}