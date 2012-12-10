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
    /// Data Transfer Object for WorkflowActivity object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class WorkflowActivityDto : DtoSecured<WorkflowActivityDto>
    {
        /// <summary />
        [DataMember]
        public int WorkflowId { get; set; }

        /// <summary />
        [DataMember]
        public int ActivityTypeId { get; set; }

        /// <summary />
        [DataMember]
        public DateTime? ActivatedDateTime { get; set; }

        /// <summary />
        [DataMember]
        public DateTime? LastProcessedDateTime { get; set; }

        /// <summary />
        [DataMember]
        public DateTime? CompletedDateTime { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public WorkflowActivityDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="workflowActivity"></param>
        public WorkflowActivityDto ( WorkflowActivity workflowActivity )
        {
            CopyFromModel( workflowActivity );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> ToDictionary()
        {
            var dictionary = base.ToDictionary();
            dictionary.Add( "WorkflowId", this.WorkflowId );
            dictionary.Add( "ActivityTypeId", this.ActivityTypeId );
            dictionary.Add( "ActivatedDateTime", this.ActivatedDateTime );
            dictionary.Add( "LastProcessedDateTime", this.LastProcessedDateTime );
            dictionary.Add( "CompletedDateTime", this.CompletedDateTime );
            return dictionary;
        }

        /// <summary>
        /// Creates a dynamic object.
        /// </summary>
        /// <returns></returns>
        public override dynamic ToDynamic()
        {
            dynamic expando = base.ToDynamic();
            expando.WorkflowId = this.WorkflowId;
            expando.ActivityTypeId = this.ActivityTypeId;
            expando.ActivatedDateTime = this.ActivatedDateTime;
            expando.LastProcessedDateTime = this.LastProcessedDateTime;
            expando.CompletedDateTime = this.CompletedDateTime;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is WorkflowActivity )
            {
                var workflowActivity = (WorkflowActivity)model;
                this.WorkflowId = workflowActivity.WorkflowId;
                this.ActivityTypeId = workflowActivity.ActivityTypeId;
                this.ActivatedDateTime = workflowActivity.ActivatedDateTime;
                this.LastProcessedDateTime = workflowActivity.LastProcessedDateTime;
                this.CompletedDateTime = workflowActivity.CompletedDateTime;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyToModel ( IEntity model )
        {
            base.CopyToModel( model );

            if ( model is WorkflowActivity )
            {
                var workflowActivity = (WorkflowActivity)model;
                workflowActivity.WorkflowId = this.WorkflowId;
                workflowActivity.ActivityTypeId = this.ActivityTypeId;
                workflowActivity.ActivatedDateTime = this.ActivatedDateTime;
                workflowActivity.LastProcessedDateTime = this.LastProcessedDateTime;
                workflowActivity.CompletedDateTime = this.CompletedDateTime;
            }
        }

    }


    /// <summary>
    /// WorkflowActivity Extension Methods
    /// </summary>
    public static class WorkflowActivityExtensions
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static WorkflowActivity ToModel( this WorkflowActivityDto value )
        {
            WorkflowActivity result = new WorkflowActivity();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<WorkflowActivity> ToModel( this List<WorkflowActivityDto> value )
        {
            List<WorkflowActivity> result = new List<WorkflowActivity>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<WorkflowActivityDto> ToDto( this List<WorkflowActivity> value )
        {
            List<WorkflowActivityDto> result = new List<WorkflowActivityDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static WorkflowActivityDto ToDto( this WorkflowActivity value )
        {
            return new WorkflowActivityDto( value );
        }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        public static string ToJson( this WorkflowActivity value, bool deep = false )
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject( ToDynamic( value, deep ) );
        }

        /// <summary>
        /// To the dynamic.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static List<dynamic> ToDynamic( this ICollection<WorkflowActivity> values )
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
        public static dynamic ToDynamic( this WorkflowActivity value, bool deep = false )
        {
            dynamic dynamicWorkflowActivity = new WorkflowActivityDto( value ).ToDynamic();

            if ( !deep )
            {
                return dynamicWorkflowActivity;
            }


            if (value.Workflow != null)
            {
                dynamicWorkflowActivity.Workflow = value.Workflow.ToDynamic();
            }

            if (value.ActivityType != null)
            {
                dynamicWorkflowActivity.ActivityType = value.ActivityType.ToDynamic();
            }

            if (value.Actions != null)
            {
                dynamicWorkflowActivity.Actions = value.Actions.ToDynamic();
            }

            return dynamicWorkflowActivity;
        }

        /// <summary>
        /// Froms the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="json">The json.</param>
        public static void FromJson( this WorkflowActivity value, string json )
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
        public static void FromDynamic( this WorkflowActivity value, object obj, bool deep = false )
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

                        // Workflow
                        if (dict.ContainsKey("Workflow"))
                        {
                            value.Workflow = new Workflow();
                            new WorkflowDto().FromDynamic( dict["Workflow"] ).CopyToModel(value.Workflow);
                        }

                        // ActivityType
                        if (dict.ContainsKey("ActivityType"))
                        {
                            value.ActivityType = new WorkflowActivityType();
                            new WorkflowActivityTypeDto().FromDynamic( dict["ActivityType"] ).CopyToModel(value.ActivityType);
                        }

                        // Actions
                        if (dict.ContainsKey("Actions"))
                        {
                            var ActionsList = dict["Actions"] as List<object>;
                            if (ActionsList != null)
                            {
                                value.Actions = new List<WorkflowAction>();
                                foreach(object childObj in ActionsList)
                                {
                                    var WorkflowAction = new WorkflowAction();
                                    new WorkflowActionDto().FromDynamic(childObj).CopyToModel(WorkflowAction);
                                    value.Actions.Add(WorkflowAction);
                                }
                            }
                        }

                    }
                }
            }
        }

    }
}