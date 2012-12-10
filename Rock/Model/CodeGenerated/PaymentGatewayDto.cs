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
    /// Data Transfer Object for PaymentGateway object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class PaymentGatewayDto : DtoSecured<PaymentGatewayDto>
    {
        /// <summary />
        [DataMember]
        public string Name { get; set; }

        /// <summary />
        [DataMember]
        public string Description { get; set; }

        /// <summary />
        [DataMember]
        public string ApiUrl { get; set; }

        /// <summary />
        [DataMember]
        public string ApiKey { get; set; }

        /// <summary />
        [DataMember]
        public string ApiSecret { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public PaymentGatewayDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="paymentGateway"></param>
        public PaymentGatewayDto ( PaymentGateway paymentGateway )
        {
            CopyFromModel( paymentGateway );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> ToDictionary()
        {
            var dictionary = base.ToDictionary();
            dictionary.Add( "Name", this.Name );
            dictionary.Add( "Description", this.Description );
            dictionary.Add( "ApiUrl", this.ApiUrl );
            dictionary.Add( "ApiKey", this.ApiKey );
            dictionary.Add( "ApiSecret", this.ApiSecret );
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
            expando.Description = this.Description;
            expando.ApiUrl = this.ApiUrl;
            expando.ApiKey = this.ApiKey;
            expando.ApiSecret = this.ApiSecret;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is PaymentGateway )
            {
                var paymentGateway = (PaymentGateway)model;
                this.Name = paymentGateway.Name;
                this.Description = paymentGateway.Description;
                this.ApiUrl = paymentGateway.ApiUrl;
                this.ApiKey = paymentGateway.ApiKey;
                this.ApiSecret = paymentGateway.ApiSecret;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyToModel ( IEntity model )
        {
            base.CopyToModel( model );

            if ( model is PaymentGateway )
            {
                var paymentGateway = (PaymentGateway)model;
                paymentGateway.Name = this.Name;
                paymentGateway.Description = this.Description;
                paymentGateway.ApiUrl = this.ApiUrl;
                paymentGateway.ApiKey = this.ApiKey;
                paymentGateway.ApiSecret = this.ApiSecret;
            }
        }

    }


    /// <summary>
    /// PaymentGateway Extension Methods
    /// </summary>
    public static class PaymentGatewayExtensions
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static PaymentGateway ToModel( this PaymentGatewayDto value )
        {
            PaymentGateway result = new PaymentGateway();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<PaymentGateway> ToModel( this List<PaymentGatewayDto> value )
        {
            List<PaymentGateway> result = new List<PaymentGateway>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<PaymentGatewayDto> ToDto( this List<PaymentGateway> value )
        {
            List<PaymentGatewayDto> result = new List<PaymentGatewayDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static PaymentGatewayDto ToDto( this PaymentGateway value )
        {
            return new PaymentGatewayDto( value );
        }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        public static string ToJson( this PaymentGateway value, bool deep = false )
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject( ToDynamic( value, deep ) );
        }

        /// <summary>
        /// To the dynamic.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static List<dynamic> ToDynamic( this ICollection<PaymentGateway> values )
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
        public static dynamic ToDynamic( this PaymentGateway value, bool deep = false )
        {
            dynamic dynamicPaymentGateway = new PaymentGatewayDto( value ).ToDynamic();

            if ( !deep )
            {
                return dynamicPaymentGateway;
            }


            if (value.Transactions != null)
            {
                dynamicPaymentGateway.Transactions = value.Transactions.ToDynamic();
            }

            return dynamicPaymentGateway;
        }

        /// <summary>
        /// Froms the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="json">The json.</param>
        public static void FromJson( this PaymentGateway value, string json )
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
        public static void FromDynamic( this PaymentGateway value, object obj, bool deep = false )
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

                        // Transactions
                        if (dict.ContainsKey("Transactions"))
                        {
                            var TransactionsList = dict["Transactions"] as List<object>;
                            if (TransactionsList != null)
                            {
                                value.Transactions = new List<FinancialTransaction>();
                                foreach(object childObj in TransactionsList)
                                {
                                    var FinancialTransaction = new FinancialTransaction();
                                    new FinancialTransactionDto().FromDynamic(childObj).CopyToModel(FinancialTransaction);
                                    value.Transactions.Add(FinancialTransaction);
                                }
                            }
                        }

                    }
                }
            }
        }

    }
}