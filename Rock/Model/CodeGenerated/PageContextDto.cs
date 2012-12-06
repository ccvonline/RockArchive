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
    /// Data Transfer Object for PageContext object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class PageContextDto : DtoSecured<PageContextDto>
    {
        /// <summary />
        [DataMember]
        public bool IsSystem { get; set; }

        /// <summary />
        [DataMember]
        public int PageId { get; set; }

        /// <summary />
        [DataMember]
        public string Entity { get; set; }

        /// <summary />
        [DataMember]
        public string IdParameter { get; set; }

        /// <summary />
        [DataMember]
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public PageContextDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="pageContext"></param>
        public PageContextDto ( PageContext pageContext )
        {
            CopyFromModel( pageContext );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> ToDictionary()
        {
            var dictionary = base.ToDictionary();
            dictionary.Add( "IsSystem", this.IsSystem );
            dictionary.Add( "PageId", this.PageId );
            dictionary.Add( "Entity", this.Entity );
            dictionary.Add( "IdParameter", this.IdParameter );
            dictionary.Add( "CreatedDateTime", this.CreatedDateTime );
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
            expando.PageId = this.PageId;
            expando.Entity = this.Entity;
            expando.IdParameter = this.IdParameter;
            expando.CreatedDateTime = this.CreatedDateTime;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is PageContext )
            {
                var pageContext = (PageContext)model;
                this.IsSystem = pageContext.IsSystem;
                this.PageId = pageContext.PageId;
                this.Entity = pageContext.Entity;
                this.IdParameter = pageContext.IdParameter;
                this.CreatedDateTime = pageContext.CreatedDateTime;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyToModel ( IEntity model )
        {
            base.CopyToModel( model );

            if ( model is PageContext )
            {
                var pageContext = (PageContext)model;
                pageContext.IsSystem = this.IsSystem;
                pageContext.PageId = this.PageId;
                pageContext.Entity = this.Entity;
                pageContext.IdParameter = this.IdParameter;
                pageContext.CreatedDateTime = this.CreatedDateTime;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class PageContextDtoExtension
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static PageContext ToModel( this PageContextDto value )
        {
            PageContext result = new PageContext();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<PageContext> ToModel( this List<PageContextDto> value )
        {
            List<PageContext> result = new List<PageContext>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<PageContextDto> ToDto( this List<PageContext> value )
        {
            List<PageContextDto> result = new List<PageContextDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static PageContextDto ToDto( this PageContext value )
        {
            return new PageContextDto( value );
        }

    }
}