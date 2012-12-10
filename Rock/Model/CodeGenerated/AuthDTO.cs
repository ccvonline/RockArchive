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
    /// Data Transfer Object for Auth object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class AuthDto : DtoSecured<AuthDto>
    {
        /// <summary />
        [DataMember]
        public int EntityTypeId { get; set; }

        /// <summary />
        [DataMember]
        public int? EntityId { get; set; }

        /// <summary />
        [DataMember]
        public int Order { get; set; }

        /// <summary />
        [DataMember]
        public string Action { get; set; }

        /// <summary />
        [DataMember]
        public string AllowOrDeny { get; set; }

        /// <summary />
        [DataMember]
        public SpecialRole SpecialRole { get; set; }

        /// <summary />
        [DataMember]
        public int? PersonId { get; set; }

        /// <summary />
        [DataMember]
        public int? GroupId { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public AuthDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="auth"></param>
        public AuthDto ( Auth auth )
        {
            CopyFromModel( auth );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> ToDictionary()
        {
            var dictionary = base.ToDictionary();
            dictionary.Add( "EntityTypeId", this.EntityTypeId );
            dictionary.Add( "EntityId", this.EntityId );
            dictionary.Add( "Order", this.Order );
            dictionary.Add( "Action", this.Action );
            dictionary.Add( "AllowOrDeny", this.AllowOrDeny );
            dictionary.Add( "SpecialRole", this.SpecialRole );
            dictionary.Add( "PersonId", this.PersonId );
            dictionary.Add( "GroupId", this.GroupId );
            return dictionary;
        }

        /// <summary>
        /// Creates a dynamic object.
        /// </summary>
        /// <returns></returns>
        public override dynamic ToDynamic()
        {
            dynamic expando = base.ToDynamic();
            expando.EntityTypeId = this.EntityTypeId;
            expando.EntityId = this.EntityId;
            expando.Order = this.Order;
            expando.Action = this.Action;
            expando.AllowOrDeny = this.AllowOrDeny;
            expando.SpecialRole = this.SpecialRole;
            expando.PersonId = this.PersonId;
            expando.GroupId = this.GroupId;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is Auth )
            {
                var auth = (Auth)model;
                this.EntityTypeId = auth.EntityTypeId;
                this.EntityId = auth.EntityId;
                this.Order = auth.Order;
                this.Action = auth.Action;
                this.AllowOrDeny = auth.AllowOrDeny;
                this.SpecialRole = auth.SpecialRole;
                this.PersonId = auth.PersonId;
                this.GroupId = auth.GroupId;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyToModel ( IEntity model )
        {
            base.CopyToModel( model );

            if ( model is Auth )
            {
                var auth = (Auth)model;
                auth.EntityTypeId = this.EntityTypeId;
                auth.EntityId = this.EntityId;
                auth.Order = this.Order;
                auth.Action = this.Action;
                auth.AllowOrDeny = this.AllowOrDeny;
                auth.SpecialRole = this.SpecialRole;
                auth.PersonId = this.PersonId;
                auth.GroupId = this.GroupId;
            }
        }

    }


    /// <summary>
    /// Auth Extension Methods
    /// </summary>
    public static class AuthExtensions
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Auth ToModel( this AuthDto value )
        {
            Auth result = new Auth();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<Auth> ToModel( this List<AuthDto> value )
        {
            List<Auth> result = new List<Auth>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<AuthDto> ToDto( this List<Auth> value )
        {
            List<AuthDto> result = new List<AuthDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static AuthDto ToDto( this Auth value )
        {
            return new AuthDto( value );
        }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        public static string ToJson( this Auth value, bool deep = false )
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject( ToDynamic( value, deep ) );
        }

        /// <summary>
        /// To the dynamic.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static List<dynamic> ToDynamic( this ICollection<Auth> values )
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
        public static dynamic ToDynamic( this Auth value, bool deep = false )
        {
            dynamic dynamicAuth = new AuthDto( value ).ToDynamic();

            if ( !deep )
            {
                return dynamicAuth;
            }


            if (value.Group != null)
            {
                dynamicAuth.Group = value.Group.ToDynamic();
            }

            if (value.Person != null)
            {
                dynamicAuth.Person = value.Person.ToDynamic();
            }

            if (value.EntityType != null)
            {
                dynamicAuth.EntityType = value.EntityType.ToDynamic();
            }

            return dynamicAuth;
        }

        /// <summary>
        /// Froms the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="json">The json.</param>
        public static void FromJson( this Auth value, string json )
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
        public static void FromDynamic( this Auth value, object obj, bool deep = false )
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

                        // Group
                        if (dict.ContainsKey("Group"))
                        {
                            value.Group = new Group();
                            new GroupDto().FromDynamic( dict["Group"] ).CopyToModel(value.Group);
                        }

                        // Person
                        if (dict.ContainsKey("Person"))
                        {
                            value.Person = new Person();
                            new PersonDto().FromDynamic( dict["Person"] ).CopyToModel(value.Person);
                        }

                        // EntityType
                        if (dict.ContainsKey("EntityType"))
                        {
                            value.EntityType = new EntityType();
                            new EntityTypeDto().FromDynamic( dict["EntityType"] ).CopyToModel(value.EntityType);
                        }

                    }
                }
            }
        }

    }
}