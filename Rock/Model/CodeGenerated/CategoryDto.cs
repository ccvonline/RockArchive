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
    /// Data Transfer Object for Category object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class CategoryDto : Dto
    {
        /// <summary />
        [DataMember]
        public bool IsSystem { get; set; }

        /// <summary />
        [DataMember]
        public int? ParentCategoryId { get; set; }

        /// <summary />
        [DataMember]
        public int EntityTypeId { get; set; }

        /// <summary />
        [DataMember]
        public string EntityTypeQualifierColumn { get; set; }

        /// <summary />
        [DataMember]
        public string EntityTypeQualifierValue { get; set; }

        /// <summary />
        [DataMember]
        public string Name { get; set; }

        /// <summary />
        [DataMember]
        public int? FileId { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public CategoryDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="category"></param>
        public CategoryDto ( Category category )
        {
            CopyFromModel( category );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> ToDictionary()
        {
            var dictionary = base.ToDictionary();
            dictionary.Add( "IsSystem", this.IsSystem );
            dictionary.Add( "ParentCategoryId", this.ParentCategoryId );
            dictionary.Add( "EntityTypeId", this.EntityTypeId );
            dictionary.Add( "EntityTypeQualifierColumn", this.EntityTypeQualifierColumn );
            dictionary.Add( "EntityTypeQualifierValue", this.EntityTypeQualifierValue );
            dictionary.Add( "Name", this.Name );
            dictionary.Add( "FileId", this.FileId );
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
            expando.ParentCategoryId = this.ParentCategoryId;
            expando.EntityTypeId = this.EntityTypeId;
            expando.EntityTypeQualifierColumn = this.EntityTypeQualifierColumn;
            expando.EntityTypeQualifierValue = this.EntityTypeQualifierValue;
            expando.Name = this.Name;
            expando.FileId = this.FileId;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is Category )
            {
                var category = (Category)model;
                this.IsSystem = category.IsSystem;
                this.ParentCategoryId = category.ParentCategoryId;
                this.EntityTypeId = category.EntityTypeId;
                this.EntityTypeQualifierColumn = category.EntityTypeQualifierColumn;
                this.EntityTypeQualifierValue = category.EntityTypeQualifierValue;
                this.Name = category.Name;
                this.FileId = category.FileId;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyToModel ( IEntity model )
        {
            base.CopyToModel( model );

            if ( model is Category )
            {
                var category = (Category)model;
                category.IsSystem = this.IsSystem;
                category.ParentCategoryId = this.ParentCategoryId;
                category.EntityTypeId = this.EntityTypeId;
                category.EntityTypeQualifierColumn = this.EntityTypeQualifierColumn;
                category.EntityTypeQualifierValue = this.EntityTypeQualifierValue;
                category.Name = this.Name;
                category.FileId = this.FileId;
            }
        }

    }


    /// <summary>
    /// Category Extension Methods
    /// </summary>
    public static class CategoryExtensions
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Category ToModel( this CategoryDto value )
        {
            Category result = new Category();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<Category> ToModel( this List<CategoryDto> value )
        {
            List<Category> result = new List<Category>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<CategoryDto> ToDto( this List<Category> value )
        {
            List<CategoryDto> result = new List<CategoryDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static CategoryDto ToDto( this Category value )
        {
            return new CategoryDto( value );
        }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        public static string ToJson( this Category value, bool deep = false )
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject( ToDynamic( value, deep ) );
        }

        /// <summary>
        /// To the dynamic.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static List<dynamic> ToDynamic( this ICollection<Category> values )
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
        public static dynamic ToDynamic( this Category value, bool deep = false )
        {
            dynamic dynamicCategory = new CategoryDto( value ).ToDynamic();

            if ( !deep )
            {
                return dynamicCategory;
            }


            if (value.ParentCategory != null)
            {
                dynamicCategory.ParentCategory = value.ParentCategory.ToDynamic();
            }

            if (value.ChildCategories != null)
            {
                dynamicCategory.ChildCategories = value.ChildCategories.ToDynamic();
            }

            if (value.EntityType != null)
            {
                dynamicCategory.EntityType = value.EntityType.ToDynamic();
            }

            if (value.File != null)
            {
                dynamicCategory.File = value.File.ToDynamic();
            }

            return dynamicCategory;
        }

        /// <summary>
        /// Froms the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="json">The json.</param>
        public static void FromJson( this Category value, string json )
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
        public static void FromDynamic( this Category value, object obj, bool deep = false )
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

                        // ParentCategory
                        if (dict.ContainsKey("ParentCategory"))
                        {
                            value.ParentCategory = new Category();
                            new CategoryDto().FromDynamic( dict["ParentCategory"] ).CopyToModel(value.ParentCategory);
                        }

                        // ChildCategories
                        if (dict.ContainsKey("ChildCategories"))
                        {
                            var ChildCategoriesList = dict["ChildCategories"] as List<object>;
                            if (ChildCategoriesList != null)
                            {
                                value.ChildCategories = new List<Category>();
                                foreach(object childObj in ChildCategoriesList)
                                {
                                    var Category = new Category();
                                    new CategoryDto().FromDynamic(childObj).CopyToModel(Category);
                                    value.ChildCategories.Add(Category);
                                }
                            }
                        }

                        // EntityType
                        if (dict.ContainsKey("EntityType"))
                        {
                            value.EntityType = new EntityType();
                            new EntityTypeDto().FromDynamic( dict["EntityType"] ).CopyToModel(value.EntityType);
                        }

                        // File
                        if (dict.ContainsKey("File"))
                        {
                            value.File = new BinaryFile();
                            new BinaryFileDto().FromDynamic( dict["File"] ).CopyToModel(value.File);
                        }

                    }
                }
            }
        }

    }
}