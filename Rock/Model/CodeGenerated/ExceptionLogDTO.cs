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
    /// Data Transfer Object for ExceptionLog object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class ExceptionLogDto : DtoSecured<ExceptionLogDto>
    {
        /// <summary />
        [DataMember]
        public int? ParentId { get; set; }

        /// <summary />
        [DataMember]
        public int? SiteId { get; set; }

        /// <summary />
        [DataMember]
        public int? PageId { get; set; }

        /// <summary />
        [DataMember]
        public DateTime ExceptionDate { get; set; }

        /// <summary />
        [DataMember]
        public int? CreatedByPersonId { get; set; }

        /// <summary />
        [DataMember]
        public bool? HasInnerException { get; set; }

        /// <summary />
        [DataMember]
        public string StatusCode { get; set; }

        /// <summary />
        [DataMember]
        public string ExceptionType { get; set; }

        /// <summary />
        [DataMember]
        public string Description { get; set; }

        /// <summary />
        [DataMember]
        public string Source { get; set; }

        /// <summary />
        [DataMember]
        public string StackTrace { get; set; }

        /// <summary />
        [DataMember]
        public string PageUrl { get; set; }

        /// <summary />
        [DataMember]
        public string ServerVariables { get; set; }

        /// <summary />
        [DataMember]
        public string QueryString { get; set; }

        /// <summary />
        [DataMember]
        public string Form { get; set; }

        /// <summary />
        [DataMember]
        public string Cookies { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public ExceptionLogDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="exceptionLog"></param>
        public ExceptionLogDto ( ExceptionLog exceptionLog )
        {
            CopyFromModel( exceptionLog );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> ToDictionary()
        {
            var dictionary = base.ToDictionary();
            dictionary.Add( "ParentId", this.ParentId );
            dictionary.Add( "SiteId", this.SiteId );
            dictionary.Add( "PageId", this.PageId );
            dictionary.Add( "ExceptionDate", this.ExceptionDate );
            dictionary.Add( "CreatedByPersonId", this.CreatedByPersonId );
            dictionary.Add( "HasInnerException", this.HasInnerException );
            dictionary.Add( "StatusCode", this.StatusCode );
            dictionary.Add( "ExceptionType", this.ExceptionType );
            dictionary.Add( "Description", this.Description );
            dictionary.Add( "Source", this.Source );
            dictionary.Add( "StackTrace", this.StackTrace );
            dictionary.Add( "PageUrl", this.PageUrl );
            dictionary.Add( "ServerVariables", this.ServerVariables );
            dictionary.Add( "QueryString", this.QueryString );
            dictionary.Add( "Form", this.Form );
            dictionary.Add( "Cookies", this.Cookies );
            return dictionary;
        }

        /// <summary>
        /// Creates a dynamic object.
        /// </summary>
        /// <returns></returns>
        public override dynamic ToDynamic()
        {
            dynamic expando = base.ToDynamic();
            expando.ParentId = this.ParentId;
            expando.SiteId = this.SiteId;
            expando.PageId = this.PageId;
            expando.ExceptionDate = this.ExceptionDate;
            expando.CreatedByPersonId = this.CreatedByPersonId;
            expando.HasInnerException = this.HasInnerException;
            expando.StatusCode = this.StatusCode;
            expando.ExceptionType = this.ExceptionType;
            expando.Description = this.Description;
            expando.Source = this.Source;
            expando.StackTrace = this.StackTrace;
            expando.PageUrl = this.PageUrl;
            expando.ServerVariables = this.ServerVariables;
            expando.QueryString = this.QueryString;
            expando.Form = this.Form;
            expando.Cookies = this.Cookies;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is ExceptionLog )
            {
                var exceptionLog = (ExceptionLog)model;
                this.ParentId = exceptionLog.ParentId;
                this.SiteId = exceptionLog.SiteId;
                this.PageId = exceptionLog.PageId;
                this.ExceptionDate = exceptionLog.ExceptionDate;
                this.CreatedByPersonId = exceptionLog.CreatedByPersonId;
                this.HasInnerException = exceptionLog.HasInnerException;
                this.StatusCode = exceptionLog.StatusCode;
                this.ExceptionType = exceptionLog.ExceptionType;
                this.Description = exceptionLog.Description;
                this.Source = exceptionLog.Source;
                this.StackTrace = exceptionLog.StackTrace;
                this.PageUrl = exceptionLog.PageUrl;
                this.ServerVariables = exceptionLog.ServerVariables;
                this.QueryString = exceptionLog.QueryString;
                this.Form = exceptionLog.Form;
                this.Cookies = exceptionLog.Cookies;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyToModel ( IEntity model )
        {
            base.CopyToModel( model );

            if ( model is ExceptionLog )
            {
                var exceptionLog = (ExceptionLog)model;
                exceptionLog.ParentId = this.ParentId;
                exceptionLog.SiteId = this.SiteId;
                exceptionLog.PageId = this.PageId;
                exceptionLog.ExceptionDate = this.ExceptionDate;
                exceptionLog.CreatedByPersonId = this.CreatedByPersonId;
                exceptionLog.HasInnerException = this.HasInnerException;
                exceptionLog.StatusCode = this.StatusCode;
                exceptionLog.ExceptionType = this.ExceptionType;
                exceptionLog.Description = this.Description;
                exceptionLog.Source = this.Source;
                exceptionLog.StackTrace = this.StackTrace;
                exceptionLog.PageUrl = this.PageUrl;
                exceptionLog.ServerVariables = this.ServerVariables;
                exceptionLog.QueryString = this.QueryString;
                exceptionLog.Form = this.Form;
                exceptionLog.Cookies = this.Cookies;
            }
        }

    }


    /// <summary>
    /// ExceptionLog Extension Methods
    /// </summary>
    public static class ExceptionLogExtensions
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ExceptionLog ToModel( this ExceptionLogDto value )
        {
            ExceptionLog result = new ExceptionLog();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<ExceptionLog> ToModel( this List<ExceptionLogDto> value )
        {
            List<ExceptionLog> result = new List<ExceptionLog>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<ExceptionLogDto> ToDto( this List<ExceptionLog> value )
        {
            List<ExceptionLogDto> result = new List<ExceptionLogDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ExceptionLogDto ToDto( this ExceptionLog value )
        {
            return new ExceptionLogDto( value );
        }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        public static string ToJson( this ExceptionLog value, bool deep = false )
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject( ToDynamic( value, deep ) );
        }

        /// <summary>
        /// To the dynamic.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static List<dynamic> ToDynamic( this ICollection<ExceptionLog> values )
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
        public static dynamic ToDynamic( this ExceptionLog value, bool deep = false )
        {
            dynamic dynamicExceptionLog = new ExceptionLogDto( value ).ToDynamic();

            if ( !deep )
            {
                return dynamicExceptionLog;
            }

            dynamicExceptionLog.CreatedByPerson = value.CreatedByPerson.ToDynamic();

            return dynamicExceptionLog;
        }

        /// <summary>
        /// Froms the dynamic.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        public static void FromDynamic( this ExceptionLog value, object obj, bool deep = false )
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

                        new PersonDto().FromDynamic( dict["CreatedByPerson"] ).CopyToModel(value.CreatedByPerson);

                    }
                }
            }
        }

    }
}