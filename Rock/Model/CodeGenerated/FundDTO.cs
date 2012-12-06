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
    /// Data Transfer Object for Fund object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class FundDto : DtoSecured<FundDto>
    {
        /// <summary />
        [DataMember]
        public string Name { get; set; }

        /// <summary />
        [DataMember]
        public string PublicName { get; set; }

        /// <summary />
        [DataMember]
        public string Description { get; set; }

        /// <summary />
        [DataMember]
        public int? ParentFundId { get; set; }

        /// <summary />
        [DataMember]
        public bool IsTaxDeductible { get; set; }

        /// <summary />
        [DataMember]
        public int Order { get; set; }

        /// <summary />
        [DataMember]
        public bool IsActive { get; set; }

        /// <summary />
        [DataMember]
        public DateTime? StartDate { get; set; }

        /// <summary />
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary />
        [DataMember]
        public bool IsPledgable { get; set; }

        /// <summary />
        [DataMember]
        public string GlCode { get; set; }

        /// <summary />
        [DataMember]
        public int? FundTypeId { get; set; }

        /// <summary />
        [DataMember]
        public string Entity { get; set; }

        /// <summary />
        [DataMember]
        public int? EntityId { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public FundDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="fund"></param>
        public FundDto ( Fund fund )
        {
            CopyFromModel( fund );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> ToDictionary()
        {
            var dictionary = base.ToDictionary();
            dictionary.Add( "Name", this.Name );
            dictionary.Add( "PublicName", this.PublicName );
            dictionary.Add( "Description", this.Description );
            dictionary.Add( "ParentFundId", this.ParentFundId );
            dictionary.Add( "IsTaxDeductible", this.IsTaxDeductible );
            dictionary.Add( "Order", this.Order );
            dictionary.Add( "IsActive", this.IsActive );
            dictionary.Add( "StartDate", this.StartDate );
            dictionary.Add( "EndDate", this.EndDate );
            dictionary.Add( "IsPledgable", this.IsPledgable );
            dictionary.Add( "GlCode", this.GlCode );
            dictionary.Add( "FundTypeId", this.FundTypeId );
            dictionary.Add( "Entity", this.Entity );
            dictionary.Add( "EntityId", this.EntityId );
            return dictionary;
        }

        /// <summary>
        /// Creates a dynamic object.
        /// </summary>
        /// <returns></returns>
        public override dynamic ToDynamic()
        {
            dynamic expando = base.ToDynamic();
            expando.Name = this.Name;
            expando.PublicName = this.PublicName;
            expando.Description = this.Description;
            expando.ParentFundId = this.ParentFundId;
            expando.IsTaxDeductible = this.IsTaxDeductible;
            expando.Order = this.Order;
            expando.IsActive = this.IsActive;
            expando.StartDate = this.StartDate;
            expando.EndDate = this.EndDate;
            expando.IsPledgable = this.IsPledgable;
            expando.GlCode = this.GlCode;
            expando.FundTypeId = this.FundTypeId;
            expando.Entity = this.Entity;
            expando.EntityId = this.EntityId;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is Fund )
            {
                var fund = (Fund)model;
                this.Name = fund.Name;
                this.PublicName = fund.PublicName;
                this.Description = fund.Description;
                this.ParentFundId = fund.ParentFundId;
                this.IsTaxDeductible = fund.IsTaxDeductible;
                this.Order = fund.Order;
                this.IsActive = fund.IsActive;
                this.StartDate = fund.StartDate;
                this.EndDate = fund.EndDate;
                this.IsPledgable = fund.IsPledgable;
                this.GlCode = fund.GlCode;
                this.FundTypeId = fund.FundTypeId;
                this.Entity = fund.Entity;
                this.EntityId = fund.EntityId;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyToModel ( IEntity model )
        {
            base.CopyToModel( model );

            if ( model is Fund )
            {
                var fund = (Fund)model;
                fund.Name = this.Name;
                fund.PublicName = this.PublicName;
                fund.Description = this.Description;
                fund.ParentFundId = this.ParentFundId;
                fund.IsTaxDeductible = this.IsTaxDeductible;
                fund.Order = this.Order;
                fund.IsActive = this.IsActive;
                fund.StartDate = this.StartDate;
                fund.EndDate = this.EndDate;
                fund.IsPledgable = this.IsPledgable;
                fund.GlCode = this.GlCode;
                fund.FundTypeId = this.FundTypeId;
                fund.Entity = this.Entity;
                fund.EntityId = this.EntityId;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class FundDtoExtension
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Fund ToModel( this FundDto value )
        {
            Fund result = new Fund();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<Fund> ToModel( this List<FundDto> value )
        {
            List<Fund> result = new List<Fund>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<FundDto> ToDto( this List<Fund> value )
        {
            List<FundDto> result = new List<FundDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static FundDto ToDto( this Fund value )
        {
            return new FundDto( value );
        }

    }
}