﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// Displays a bootstrap badge
    /// </summary>
    [ToolboxData( "<{0}:Badge runat=server></{0}:Badge>" )]
    public class Badge : PlaceHolder
    {
        /// <summary>
        /// Gets or sets the type of the badge.
        /// </summary>
        /// <value>
        /// The type of the badge.
        /// </value>
        public BadgeType BadgeType
        {
            get
            {
                string s = ViewState["BadgeType"] as string;
                return s == null ? BadgeType.None : s.ConvertToEnum<BadgeType>();
            }
            set
            {
                ViewState["BadgeType"] = value.ConvertToString();
            }
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( this.Visible )
            {
                string css = "badge";
                if ( BadgeType != BadgeType.None )
                {
                    css += " badge-" + BadgeType.ConvertToString().ToLower();
                }

                writer.AddAttribute( "class", css );
                writer.RenderBeginTag( HtmlTextWriterTag.Span );

                base.RenderControl( writer );

                writer.RenderEndTag();
            }
        }
    }

    /// <summary>
    /// The type of notification box to display
    /// </summary>
    public enum BadgeType
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Success
        /// </summary>
        Success,

        /// <summary>
        /// Warning
        /// </summary>
        Warning,

        /// <summary>
        /// Important
        /// </summary>
        Important,

        /// <summary>
        /// Info
        /// </summary>
        Info,

        /// <summary>
        /// Inverse
        /// </summary>
        Inverse,
    };
}