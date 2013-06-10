//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rock.Field.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoFieldType : FieldType
    {
        /// <summary>
        /// Creates the control(s) neccessary for prompting user for a new value
        /// </summary>
        /// <param name="configurationValues">The configuration values.</param>
        /// <returns>
        /// The control
        /// </returns>
        public override Control EditControl( Dictionary<string, ConfigurationValue> configurationValues, string id )
        {
            return new TextBox { ID = id, TextMode = TextBoxMode.MultiLine, Rows = 3 };
        }
    }
}