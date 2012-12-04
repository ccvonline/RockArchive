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
    /// Data Transfer Object for FieldType object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class FieldTypeDto : IDto, DotLiquid.ILiquidizable
    {
        /// <summary />
        [DataMember]
        public bool IsSystem { get; set; }

        /// <summary />
        [DataMember]
        public string Name { get; set; }

        /// <summary />
        [DataMember]
        public string Description { get; set; }

        /// <summary />
        [DataMember]
        public string Assembly { get; set; }

        /// <summary />
        [DataMember]
        public string Class { get; set; }

        /// <summary />
        [DataMember]
        public int Id { get; set; }

        /// <summary />
        [DataMember]
        public Guid Guid { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public FieldTypeDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="fieldType"></param>
        public FieldTypeDto ( FieldType fieldType )
        {
            CopyFromModel( fieldType );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add( "IsSystem", this.IsSystem );
            dictionary.Add( "Name", this.Name );
            dictionary.Add( "Description", this.Description );
            dictionary.Add( "Assembly", this.Assembly );
            dictionary.Add( "Class", this.Class );
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
            expando.Name = this.Name;
            expando.Description = this.Description;
            expando.Assembly = this.Assembly;
            expando.Class = this.Class;
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
            if ( model is FieldType )
            {
                var fieldType = (FieldType)model;
                this.IsSystem = fieldType.IsSystem;
                this.Name = fieldType.Name;
                this.Description = fieldType.Description;
                this.Assembly = fieldType.Assembly;
                this.Class = fieldType.Class;
                this.Id = fieldType.Id;
                this.Guid = fieldType.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is FieldType )
            {
                var fieldType = (FieldType)model;
                fieldType.IsSystem = this.IsSystem;
                fieldType.Name = this.Name;
                fieldType.Description = this.Description;
                fieldType.Assembly = this.Assembly;
                fieldType.Class = this.Class;
                fieldType.Id = this.Id;
                fieldType.Guid = this.Guid;
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
    public static class FieldTypeDtoExtension
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static FieldType ToModel( this FieldTypeDto value )
        {
            FieldType result = new FieldType();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<FieldType> ToModel( this List<FieldTypeDto> value )
        {
            List<FieldType> result = new List<FieldType>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<FieldTypeDto> ToDto( this List<FieldType> value )
        {
            List<FieldTypeDto> result = new List<FieldTypeDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static FieldTypeDto ToDto( this FieldType value )
        {
            return new FieldTypeDto( value );
        }

    }
}