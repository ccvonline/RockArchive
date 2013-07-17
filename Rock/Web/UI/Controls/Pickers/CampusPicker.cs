//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Rock.Constants;
using Rock.Model;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class CampusPicker : LabeledDropDownList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampusPicker" /> class.
        /// </summary>
        public CampusPicker()
            : base()
        {
            LabelText = "Campus";
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LabeledTextBox" /> is required.
        /// </summary>
        /// <value><c>true</c> if required; otherwise, <c>false</c>.</value>
        public override bool Required
        {
            get
            {
                return base.Required;
            }
            set
            {
                var li = this.Items.FindByValue( None.IdValue );

                if ( value )
                {
                    if ( li != null )
                    {
                        this.Items.Remove( li );
                    }
                }
                else
                {
                    if ( li == null )
                    {
                        this.Items.Insert( 0, new ListItem( string.Empty, None.IdValue ) );
                    }
                }

                base.Required = value;
            }
        }

        /// <summary>
        /// Gets or sets the campuses.
        /// </summary>
        /// <value>
        /// The campuses.
        /// </value>
        public List<Campus> Campuses
        {
            set
            {
                this.Items.Clear();

                if ( !Required )
                {
                    this.Items.Add( new ListItem( string.Empty, None.IdValue ) );
                }

                foreach ( Campus campus in value )
                {
                    ListItem campusItem = new ListItem();
                    campusItem.Value = campus.Id.ToString();
                    campusItem.Text = campus.Name;
                    this.Items.Add( campusItem );
                }

            }
        }

        /// <summary>
        /// Gets the selected campus ids.
        /// </summary>
        /// <value>
        /// The selected campus ids.
        /// </value>
        public int? SelectedCampusId
        {
            get
            {
                return this.SelectedValueAsInt();
            }
            set
            {
                int id = value.HasValue ? value.Value : 0;
                var li = this.Items.FindByValue( id.ToString() );
                if ( li != null )
                {
                    li.Selected = true;
                }
            }
        }

    }
}