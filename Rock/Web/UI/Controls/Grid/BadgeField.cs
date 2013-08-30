﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Rock;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// <see cref="Grid"/> Column for displaying a badge
    /// </summary>
    [ToolboxData( "<{0}:BadgeField runat=server></{0}:BadgeField>" )]
    public class BadgeField : BoundField
    {
        /// <summary>
        /// Gets or sets the important minimum value rule.
        /// </summary>
        /// <value>
        /// The minimum value to be considered Important.
        /// </value>
        public int ImportantMin
        {
            get
            {
                int? i = ViewState["ImportantMin"] as int?;
                return ( i == null ) ? int.MaxValue : i.Value;
            }
            set
            {
                ViewState["ImportantMin"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the important max.
        /// </summary>
        /// <value>
        /// The maximum value to be considered Important.
        /// </value>
        public int ImportantMax
        {
            get
            {
                int? i = ViewState["ImportantMax"] as int?;
                return ( i == null ) ? int.MaxValue : i.Value;
            }
            set
            {
                ViewState["ImportantMax"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Warning minimum value rule.
        /// </summary>
        /// <value>
        /// The minimum value to be considered Warning.
        /// </value>
        public int WarningMin
        {
            get
            {
                int? i = ViewState["WarningMin"] as int?;
                return ( i == null ) ? int.MaxValue : i.Value;
            }
            set
            {
                ViewState["WarningMin"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Warning maximum value rule.
        /// </summary>
        /// <value>
        /// The maximum value to be considered Warning.
        /// </value>
        public int WarningMax
        {
            get
            {
                int? i = ViewState["WarningMax"] as int?;
                return ( i == null ) ? int.MaxValue : i.Value;
            }
            set
            {
                ViewState["WarningMax"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Success minimum value rule.
        /// </summary>
        /// <value>
        /// The minimum value to be considered Success.
        /// </value>
        public int SuccessMin
        {
            get
            {
                int? i = ViewState["SuccessMin"] as int?;
                return ( i == null ) ? int.MaxValue : i.Value;
            }
            set
            {
                ViewState["SuccessMin"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Success maximum value rule.
        /// </summary>
        /// <value>
        /// The maximum value to be considered Success.
        /// </value>
        public int SuccessMax
        {
            get
            {
                int? i = ViewState["SuccessMax"] as int?;
                return ( i == null ) ? int.MaxValue : i.Value;
            }
            set
            {
                ViewState["SuccessMax"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Info minimum value rule.
        /// </summary>
        /// <value>
        /// The minimum value to be considered Info.
        /// </value>
        public int InfoMin
        {
            get
            {
                int? i = ViewState["InfoMin"] as int?;
                return ( i == null ) ? int.MaxValue : i.Value;
            }
            set
            {
                ViewState["InfoMin"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Info maximum value rule.
        /// </summary>
        /// <value>
        /// The maximum value to be considered Info.
        /// </value>
        public int InfoMax
        {
            get
            {
                int? i = ViewState["InfoMax"] as int?;
                return ( i == null ) ? int.MaxValue : i.Value;
            }
            set
            {
                ViewState["InfoMax"] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeField" /> class.
        /// </summary>
        public BadgeField()
            : base()
        {
            this.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            this.HeaderStyle.CssClass = "span1";
        }

        /// <summary>
        /// Formats the specified field value for a cell in the <see cref="T:System.Web.UI.WebControls.BoundField" /> object.
        /// </summary>
        /// <param name="dataValue">The field value to format.</param>
        /// <param name="encode">true to encode the value; otherwise, false.</param>
        /// <returns>
        /// The field value converted to the format specified by <see cref="P:System.Web.UI.WebControls.BoundField.DataFormatString" />.
        /// </returns>
        protected override string FormatDataValue( object dataValue, bool encode )
        {
            BadgeRowEventArgs eventArg = new BadgeRowEventArgs( dataValue );

            SetBadgeTypeByRules( eventArg );

            if ( SetBadgeType != null )
            {
                SetBadgeType( this, eventArg );
            }

            string css = "badge";
            if( !string.IsNullOrWhiteSpace( eventArg.BadgeType ) )
            {
                css += " badge-" + eventArg.BadgeType.ToLower();
            }

            string fieldValue = base.FormatDataValue( eventArg.FieldValue, encode );

            return string.Format( "<span class='{0}'>{1}</span>", css, fieldValue );
        }

        private void SetBadgeTypeByRules( BadgeRowEventArgs e )
        {
            if ( !( e.FieldValue is int ) )
                return;

            int count = (int)e.FieldValue;

            if ( ImportantMin <= count && count <= ImportantMax )
            {
                e.BadgeType = "Important";
            }
            else if ( WarningMin <= count && count <= WarningMax )
            {
                e.BadgeType = "Warning";
            }
            else if ( SuccessMin <= count && count <= SuccessMax )
            {
                e.BadgeType = "Success";
            }
            else if ( InfoMin <= count && count <= InfoMax )
            {
                e.BadgeType = "Info";
            }
        }
        
        /// <summary>
        /// Occurs when badge field is being formatted.  Use to set the badge type
        /// based on the current row's field value.
        /// </summary>
        public event EventHandler<BadgeRowEventArgs> SetBadgeType;
    }


    /// <summary>
    /// 
    /// </summary>
    public class BadgeRowEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <value>
        /// The field value.
        /// </value>
        public object FieldValue { get; private set; }

        /// <summary>
        /// Gets or sets the type of the badge.
        /// </summary>
        /// <value>
        /// The type of the badge.
        /// </value>
        public string BadgeType { get; set; }

        public BadgeRowEventArgs(object fieldValue)
        {
            FieldValue = fieldValue;
            BadgeType = string.Empty;
        }
    }
}