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
using System.Linq;

using Rock.Data;

namespace Rock.Util
{
    /// <summary>
    /// Action Service class
    /// </summary>
    public partial class ActionService : Service<Action, ActionDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionService"/> class
        /// </summary>
        public ActionService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionService"/> class
        /// </summary>
        public ActionService(IRepository<Action> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override Action CreateNew()
        {
            return new Action();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<ActionDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<ActionDto> QueryableDto( IQueryable<Action> items )
        {
            return items.Select( m => new ActionDto()
                {
                    ActivityId = m.ActivityId,
                    ActionTypeId = m.ActionTypeId,
                    ActivatedDateTime = m.ActivatedDateTime,
                    LastProcessedDateTime = m.LastProcessedDateTime,
                    CompletedDateTime = m.CompletedDateTime,
                    Id = m.Id,
                    Guid = m.Guid,
                });
        }
    }
}
