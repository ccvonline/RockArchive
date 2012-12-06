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
    /// Data Transfer Object for MetricValue object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class MetricValueDto : DtoSecured<MetricValueDto>
    {
        /// <summary />
        [DataMember]
        public bool IsSystem { get; set; }

        /// <summary />
        [DataMember]
        public int MetricId { get; set; }

        /// <summary />
        [DataMember]
        public string Value { get; set; }

        /// <summary />
        [DataMember]
        public string Description { get; set; }

        /// <summary />
        [DataMember]
        public string xValue { get; set; }

        /// <summary />
        [DataMember]
        public bool isDateBased { get; set; }

        /// <summary />
        [DataMember]
        public string Label { get; set; }

        /// <summary />
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public MetricValueDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="metricValue"></param>
        public MetricValueDto ( MetricValue metricValue )
        {
            CopyFromModel( metricValue );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> ToDictionary()
        {
            var dictionary = base.ToDictionary();
            dictionary.Add( "IsSystem", this.IsSystem );
            dictionary.Add( "MetricId", this.MetricId );
            dictionary.Add( "Value", this.Value );
            dictionary.Add( "Description", this.Description );
            dictionary.Add( "xValue", this.xValue );
            dictionary.Add( "isDateBased", this.isDateBased );
            dictionary.Add( "Label", this.Label );
            dictionary.Add( "Order", this.Order );
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
            expando.MetricId = this.MetricId;
            expando.Value = this.Value;
            expando.Description = this.Description;
            expando.xValue = this.xValue;
            expando.isDateBased = this.isDateBased;
            expando.Label = this.Label;
            expando.Order = this.Order;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is MetricValue )
            {
                var metricValue = (MetricValue)model;
                this.IsSystem = metricValue.IsSystem;
                this.MetricId = metricValue.MetricId;
                this.Value = metricValue.Value;
                this.Description = metricValue.Description;
                this.xValue = metricValue.xValue;
                this.isDateBased = metricValue.isDateBased;
                this.Label = metricValue.Label;
                this.Order = metricValue.Order;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyToModel ( IEntity model )
        {
            base.CopyToModel( model );

            if ( model is MetricValue )
            {
                var metricValue = (MetricValue)model;
                metricValue.IsSystem = this.IsSystem;
                metricValue.MetricId = this.MetricId;
                metricValue.Value = this.Value;
                metricValue.Description = this.Description;
                metricValue.xValue = this.xValue;
                metricValue.isDateBased = this.isDateBased;
                metricValue.Label = this.Label;
                metricValue.Order = this.Order;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class MetricValueDtoExtension
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static MetricValue ToModel( this MetricValueDto value )
        {
            MetricValue result = new MetricValue();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<MetricValue> ToModel( this List<MetricValueDto> value )
        {
            List<MetricValue> result = new List<MetricValue>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<MetricValueDto> ToDto( this List<MetricValue> value )
        {
            List<MetricValueDto> result = new List<MetricValueDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static MetricValueDto ToDto( this MetricValue value )
        {
            return new MetricValueDto( value );
        }

    }
}