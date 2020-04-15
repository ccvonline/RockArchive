
using System.Collections.Generic;
using System.Web.UI.WebControls;

using church.ccv.Pastoral.Model;

using Rock;
using Rock.Web.UI.Controls;

namespace church.ccv.Pastoral.Web.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class CareRequestPicker : RockDropDownList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CareRequestPicker" /> class.
        /// </summary>
        public CareRequestPicker()
            : base()
        {
            Label = "Request";
        }

        /// <summary>
        /// Gets or sets the accounts.
        /// </summary>
        /// <value>
        /// The accounts.
        /// </value>
        public List<CareRequest> Requests
        {
            set
            {
                this.Items.Clear();
                this.Items.Add( new ListItem() );

                foreach ( var request in value )
                {
                    this.Items.Add( new ListItem( request.FirstName + " " + request.LastName + " (" + request.RequestDateTime.ToString( "MM/dd/yyyy" ) + ")", request.Id.ToString() ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected account identifier.
        /// </summary>
        /// <value>
        /// The selected account identifier.
        /// </value>
        public int? SelectedRequestId
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