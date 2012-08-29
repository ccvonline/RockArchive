//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;

using Rock.Data;

namespace Rock.Core
{
	/// <summary>
	/// Field Type POCO Service class
	/// </summary>
    public partial class FieldTypeService : Service<FieldType, FieldTypeDTO>
    {
		/// <summary>
		/// Gets Field Types by Name
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>An enumerable list of FieldType objects.</returns>
	    public IEnumerable<FieldType> GetByName( string name )
        {
            return Repository.Find( t => t.Name == name );
        }
    }
}
