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

using Rock.Data;

namespace Rock.Core
{
    /// <summary>
    /// Data Transfer Object for ServiceLog object
    /// </summary>
    public partial class ServiceLogDto : IDto
    {

#pragma warning disable 1591
        public DateTime? Time { get; set; }
        public string Input { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
        public bool Success { get; set; }
        public int Id { get; set; }
        public Guid Guid { get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public ServiceLogDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="serviceLog"></param>
        public ServiceLogDto ( ServiceLog serviceLog )
        {
            CopyFromModel( serviceLog );
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyFromModel( IEntity model )
        {
            if ( model is ServiceLog )
            {
                var serviceLog = (ServiceLog)model;
                this.Time = serviceLog.Time;
                this.Input = serviceLog.Input;
                this.Type = serviceLog.Type;
                this.Name = serviceLog.Name;
                this.Result = serviceLog.Result;
                this.Success = serviceLog.Success;
                this.Id = serviceLog.Id;
                this.Guid = serviceLog.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is ServiceLog )
            {
                var serviceLog = (ServiceLog)model;
                serviceLog.Time = this.Time;
                serviceLog.Input = this.Input;
                serviceLog.Type = this.Type;
                serviceLog.Name = this.Name;
                serviceLog.Result = this.Result;
                serviceLog.Success = this.Success;
                serviceLog.Id = this.Id;
                serviceLog.Guid = this.Guid;
            }
        }
    }
}
