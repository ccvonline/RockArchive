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

namespace Rock.Model
{
    /// <summary>
    /// ExceptionLog Service class
    /// </summary>
    public partial class ExceptionLogService : Service<ExceptionLog, ExceptionLogDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionLogService"/> class
        /// </summary>
        public ExceptionLogService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionLogService"/> class
        /// </summary>
        public ExceptionLogService(IRepository<ExceptionLog> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override ExceptionLog CreateNew()
        {
            return new ExceptionLog();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<ExceptionLogDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<ExceptionLogDto> QueryableDto( IQueryable<ExceptionLog> items )
        {
            return items.Select( m => new ExceptionLogDto()
                {
                    ParentId = m.ParentId,
                    SiteId = m.SiteId,
                    PageId = m.PageId,
                    ExceptionDate = m.ExceptionDate,
                    CreatedByPersonId = m.CreatedByPersonId,
                    HasInnerException = m.HasInnerException,
                    StatusCode = m.StatusCode,
                    ExceptionType = m.ExceptionType,
                    Description = m.Description,
                    Source = m.Source,
                    StackTrace = m.StackTrace,
                    PageUrl = m.PageUrl,
                    ServerVariables = m.ServerVariables,
                    QueryString = m.QueryString,
                    Form = m.Form,
                    Cookies = m.Cookies,
                    Id = m.Id,
                    Guid = m.Guid,
                });
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( ExceptionLog item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }
}
