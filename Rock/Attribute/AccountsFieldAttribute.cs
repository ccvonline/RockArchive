﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using Rock.Field.Types;

namespace Rock.Attribute
{
    class AccountsFieldAttribute : FieldAttribute
    {
        public AccountsFieldAttribute( string name, string description = "", bool required = true, string defaultValue = "", string category = "", int order = 0, string key = null ) : 
            base( name, description, required, defaultValue, category, order, key, typeof( AccountsType ).FullName )
        {
        }
    }
}
