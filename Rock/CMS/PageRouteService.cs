//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
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
using System.Linq;

using Rock.Data;

namespace Rock.CMS
{
	/// <summary>
	/// Page Route POCO Service Layer class
	/// </summary>
    public partial class PageRouteService : Service<Rock.CMS.PageRoute>
    {
		/// <summary>
		/// Gets Page Routes by Page Id
		/// </summary>
		/// <param name="pageId">Page Id.</param>
		/// <returns>An enumerable list of PageRoute objects.</returns>
	    public IEnumerable<Rock.CMS.PageRoute> GetByPageId( int pageId )
        {
            return Repository.Find( t => t.PageId == pageId );
        }
		
    }
}
