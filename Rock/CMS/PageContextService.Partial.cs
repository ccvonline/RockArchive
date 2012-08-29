//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;

using Rock.Data;

namespace Rock.CMS
{
	/// <summary>
	/// Page Context POCO Service class
	/// </summary>
    public partial class PageContextService : Service<PageContext, PageContextDTO>
    {
		/// <summary>
		/// Gets Page Contexts by Page Id
		/// </summary>
		/// <param name="pageId">Page Id.</param>
		/// <returns>An enumerable list of PageContext objects.</returns>
	    public IEnumerable<PageContext> GetByPageId( int pageId )
        {
            return Repository.Find( t => t.PageId == pageId );
        }
    }
}
