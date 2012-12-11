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
    /// Data Transfer Object for WorkflowActivityType object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class WorkflowActivityTypeDto : DtoSecured<WorkflowActivityTypeDto>
    {
        /// <summary />
        [DataMember]
        public bool? IsActive { get; set; }

        /// <summary />
        [DataMember]
        public int WorkflowTypeId { get; set; }

        /// <summary />
        [DataMember]
        public string Name { get; set; }

        /// <summary />
        [DataMember]
        public string Description { get; set; }

        /// <summary />
        [DataMember]
        public bool IsActivatedWithWorkflow { get; set; }

        /// <summary />
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public WorkflowActivityTypeDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="workflowActivityType"></param>
        public WorkflowActivityTypeDto ( WorkflowActivityType workflowActivityType )
        {
            CopyFromModel( workflowActivityType );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> ToDictionary()
        {
            var dictionary = base.ToDictionary();
            dictionary.Add( "IsActive", this.IsActive );
            dictionary.Add( "WorkflowTypeId", this.WorkflowTypeId );
            dictionary.Add( "Name", this.Name );
            dictionary.Add( "Description", this.Description );
            dictionary.Add( "IsActivatedWithWorkflow", this.IsActivatedWithWorkflow );
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
            expando.IsActive = this.IsActive;
            expando.WorkflowTypeId = this.WorkflowTypeId;
            expando.Name = this.Name;
            expando.Description = this.Description;
            expando.IsActivatedWithWorkflow = this.IsActivatedWithWorkflow;
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

            if ( model is WorkflowActivityType )
            {
                var workflowActivityType = (WorkflowActivityType)model;
                this.IsActive = workflowActivityType.IsActive;
                this.WorkflowTypeId = workflowActivityType.WorkflowTypeId;
                this.Name = workflowActivityType.Name;
                this.Description = workflowActivityType.Description;
                this.IsActivatedWithWorkflow = workflowActivityType.IsActivatedWithWorkflow;
                this.Order = workflowActivityType.Order;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyToModel ( IEntity model )
        {
            base.CopyToModel( model );

            if ( model is WorkflowActivityType )
            {
                var workflowActivityType = (WorkflowActivityType)model;
                workflowActivityType.IsActive = this.IsActive;
                workflowActivityType.WorkflowTypeId = this.WorkflowTypeId;
                workflowActivityType.Name = this.Name;
                workflowActivityType.Description = this.Description;
                workflowActivityType.IsActivatedWithWorkflow = this.IsActivatedWithWorkflow;
                workflowActivityType.Order = this.Order;
            }
        }

    }


    /// <summary>
    /// WorkflowActivityType Extension Methods
    /// </summary>
    public static class WorkflowActivityTypeExtensions
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static WorkflowActivityType ToModel( this WorkflowActivityTypeDto value )
        {
            WorkflowActivityType result = new WorkflowActivityType();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<WorkflowActivityType> ToModel( this List<WorkflowActivityTypeDto> value )
        {
            List<WorkflowActivityType> result = new List<WorkflowActivityType>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<WorkflowActivityTypeDto> ToDto( this List<WorkflowActivityType> value )
        {
            List<WorkflowActivityTypeDto> result = new List<WorkflowActivityTypeDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static WorkflowActivityTypeDto ToDto( this WorkflowActivityType value )
        {
            return new WorkflowActivityTypeDto( value );
        }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        public static string ToJson( this WorkflowActivityType value, bool deep = false )
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject( ToDynamic( value, deep ) );
        }

        /// <summary>
        /// To the dynamic.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static List<dynamic> ToDynamic( this ICollection<WorkflowActivityType> values )
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
        public static dynamic ToDynamic( this WorkflowActivityType value, bool deep = false )
        {
            dynamic dynamicWorkflowActivityType = new WorkflowActivityTypeDto( value ).ToDynamic();

            if ( !deep )
            {
                return dynamicWorkflowActivityType;
            }


            if (value.WorkflowType != null)
            {
                dynamicWorkflowActivityType.WorkflowType = value.WorkflowType.ToDynamic();
            }

            if (value.ActionTypes != null)
            {
                dynamicWorkflowActivityType.ActionTypes = value.ActionTypes.ToDynamic();
            }

            return dynamicWorkflowActivityType;
        }

        /// <summary>
        /// Froms the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="json">The json.</param>
        public static void FromJson( this WorkflowActivityType value, string json )
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
        public static void FromDynamic( this WorkflowActivityType value, object obj, bool deep = false )
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

                        // WorkflowType
                        if (dict.ContainsKey("WorkflowType"))
                        {
                            value.WorkflowType = new WorkflowType();
                            new WorkflowTypeDto().FromDynamic( dict["WorkflowType"] ).CopyToModel(value.WorkflowType);
                        }

                        // ActionTypes
                        if (dict.ContainsKey("ActionTypes"))
                        {
                            var ActionTypesList = dict["ActionTypes"] as List<object>;
                            if (ActionTypesList != null)
                            {
                                value.ActionTypes = new List<WorkflowActionType>();
                                foreach(object childObj in ActionTypesList)
                                {
                                    var WorkflowActionType = new WorkflowActionType();
                                    WorkflowActionType.FromDynamic(childObj, true);
                                    value.ActionTypes.Add(WorkflowActionType);
                                }
                            }
                        }

                    }
                }
            }
        }

    }
}