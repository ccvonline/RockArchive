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

namespace Rock.Util
{
    /// <summary>
    /// Data Transfer Object for WorkflowLog object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class WorkflowLogDto : IDto, DotLiquid.ILiquidizable
    {
        /// <summary />
        [DataMember]
        public int WorkflowId { get; set; }

        /// <summary />
        [DataMember]
        public DateTime LogDateTime { get; set; }

        /// <summary />
        [DataMember]
        public string LogText { get; set; }

        /// <summary />
        [DataMember]
        public int Id { get; set; }

        /// <summary />
        [DataMember]
        public Guid Guid { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public WorkflowLogDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="workflowLog"></param>
        public WorkflowLogDto ( WorkflowLog workflowLog )
        {
            CopyFromModel( workflowLog );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add( "WorkflowId", this.WorkflowId );
            dictionary.Add( "LogDateTime", this.LogDateTime );
            dictionary.Add( "LogText", this.LogText );
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
            expando.WorkflowId = this.WorkflowId;
            expando.LogDateTime = this.LogDateTime;
            expando.LogText = this.LogText;
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
            if ( model is WorkflowLog )
            {
                var workflowLog = (WorkflowLog)model;
                this.WorkflowId = workflowLog.WorkflowId;
                this.LogDateTime = workflowLog.LogDateTime;
                this.LogText = workflowLog.LogText;
                this.Id = workflowLog.Id;
                this.Guid = workflowLog.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is WorkflowLog )
            {
                var workflowLog = (WorkflowLog)model;
                workflowLog.WorkflowId = this.WorkflowId;
                workflowLog.LogDateTime = this.LogDateTime;
                workflowLog.LogText = this.LogText;
                workflowLog.Id = this.Id;
                workflowLog.Guid = this.Guid;
            }
        }

        /// <summary>
        /// Converts to liquidizable object for dotLiquid templating
        /// </summary>
        /// <returns></returns>
        public object ToLiquid()
        {
            return this.ToDictionary();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class WorkflowLogDtoExtension
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static WorkflowLog ToModel( this WorkflowLogDto value )
        {
            WorkflowLog result = new WorkflowLog();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<WorkflowLog> ToModel( this List<WorkflowLogDto> value )
        {
            List<WorkflowLog> result = new List<WorkflowLog>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<WorkflowLogDto> ToDto( this List<WorkflowLog> value )
        {
            List<WorkflowLogDto> result = new List<WorkflowLogDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static WorkflowLogDto ToDto( this WorkflowLog value )
        {
            return new WorkflowLogDto( value );
        }

    }
}