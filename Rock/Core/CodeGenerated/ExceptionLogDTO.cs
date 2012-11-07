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
    /// Data Transfer Object for ExceptionLog object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class ExceptionLogDto : IDto
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

        /// <summary />
        [DataMember]
        public int Id { get; set; }

        /// <summary />
        [DataMember]
        public Guid Guid { get; set; }

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
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
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
                this.Id = exceptionLog.Id;
                this.Guid = exceptionLog.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
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
                exceptionLog.Id = this.Id;
                exceptionLog.Guid = this.Guid;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionLogDtoExtension
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
            value.ForEach( a => result.Add( new ExceptionLogDto( a ) ) );
            return result;
        }
    }
}