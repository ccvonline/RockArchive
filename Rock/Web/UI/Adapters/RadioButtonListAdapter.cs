﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;

namespace Rock.Web.UI.Adapters
{
    /// <summary>
    /// Control adapter for radio button list
    /// </summary>
    public class RadioButtonListAdapter : WebControlAdapter
    {
        /// <summary>
        /// Creates the beginning tag for the Web control in the markup that is transmitted to the target browser.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> containing methods to render the target-specific output.</param>
        protected override void RenderBeginTag( System.Web.UI.HtmlTextWriter writer )
        {
        }

        /// <summary>
        /// Creates the ending tag for the Web control in the markup that is transmitted to the target browser.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> containing methods to render the target-specific output.</param>
        protected override void RenderEndTag( System.Web.UI.HtmlTextWriter writer )
        {
        }

        /// <summary>
        /// Generates the target-specific inner markup for the Web control to which the control adapter is attached.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> containing methods to render the target-specific output.</param>
        protected override void RenderContents( System.Web.UI.HtmlTextWriter writer )
        {
            RadioButtonList rbl = Control as RadioButtonList;
            if ( rbl != null )
            {
                int i = 0;
                foreach ( ListItem li in rbl.Items )
                {
                    writer.WriteLine();
                    writer.AddAttribute( "class", "radio" + ( rbl.RepeatDirection == RepeatDirection.Horizontal ? " inline" : string.Empty ) );
                    writer.RenderBeginTag( HtmlTextWriterTag.Label );

                    string itemId = string.Format( "{0}_{1}", rbl.ClientID, i++ );
                    writer.AddAttribute( "id", itemId );
                    writer.AddAttribute( "type", "radio" );
                    writer.AddAttribute( "name", rbl.UniqueID );
                    writer.AddAttribute( "value", li.Value );
                    if ( li.Selected )
                    {
                        writer.AddAttribute( "checked", "checked" );
                    }
                    writer.RenderBeginTag( HtmlTextWriterTag.Input );
                    writer.RenderEndTag();

                    writer.Write( li.Text );

                    writer.RenderEndTag();

                    if ( Page != null && Page.ClientScript != null )
                    {
                        Page.ClientScript.RegisterForEventValidation( rbl.UniqueID, li.Value );
                    }
                }

                if ( Page != null && Page.ClientScript != null )
                {
                    Page.ClientScript.RegisterForEventValidation( rbl.UniqueID );
                }
            }
        }
    }
}