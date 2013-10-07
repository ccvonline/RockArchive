//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock.Model;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class LocationPicker : CompositeControl
    {
        #region Controls

        private Panel _pnlModeSelection;
        private RadioButton _radNamedLocation;
        private RadioButton _radAddress;
        private RadioButton _radLatLong;

        private LocationItemPicker _locationItemPicker;
        private LocationAddressPicker _locationAddressPicker;
        private GeoPicker _locationGeoPicker;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [limit automatic named locations].
        /// </summary>
        /// <value>
        /// <c>true</c> if [limit automatic named locations]; otherwise, <c>false</c>.
        /// </value>
        public bool LimitToNamedLocations
        {
            get
            {
                return ( ViewState["LimitToNamedLocations"] as bool? ) ?? true;
            }

            set
            {
                ViewState["LimitToNamedLocations"] = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationPicker"/> class.
        /// </summary>
        public LocationPicker()
            : base()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( Page.IsPostBack )
            {
                // manually wire up events for radio buttons
                string eventTarget = Page.Request.Params["__EVENTTARGET"];
                if ( !string.IsNullOrWhiteSpace( eventTarget ) )
                {
                    RadioButton radButton = _pnlModeSelection.Controls.OfType<RadioButton>().FirstOrDefault( a => a.UniqueID == eventTarget );
                    if ( radButton != null )
                    {
                        _radMode_CheckedChanged( radButton, e );
                    }
                }
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Controls.Clear();

            // Mode Selection Panel and Controls
            _pnlModeSelection = new Panel { ID = "pnlModeSelection" };
            _pnlModeSelection.CssClass = "picker-mode-options";
            
            _pnlModeSelection.Visible = !this.LimitToNamedLocations;

            _radNamedLocation = new RadioButton { ID = "radNamedLocation" };
            _radNamedLocation.CheckedChanged += _radMode_CheckedChanged;
            _radNamedLocation.Text = "Named Location";
            _radNamedLocation.Checked = true;
            _radNamedLocation.GroupName = "radiogroup-location-mode_" + this.ClientID;
            _radNamedLocation.AutoPostBack = true;
            _pnlModeSelection.Controls.Add( _radNamedLocation );

            _radAddress = new RadioButton { ID = "radAddress" };
            _radAddress.CheckedChanged += _radMode_CheckedChanged;
            _radAddress.Text = "Address";
            _radAddress.GroupName = "radiogroup-location-mode_" + this.ClientID;
            _radAddress.AutoPostBack = true;
            _pnlModeSelection.Controls.Add( _radAddress );

            _radLatLong = new RadioButton { ID = "radLatLong" };
            _radLatLong.CheckedChanged += _radMode_CheckedChanged;
            _radLatLong.Text = "Lat/Long";
            _radLatLong.GroupName = "radiogroup-location-mode_" + this.ClientID;
            _radLatLong.AutoPostBack = true;
            _pnlModeSelection.Controls.Add( _radLatLong );

            _locationItemPicker = new LocationItemPicker();
            _locationItemPicker.ID = this.ID + "_locationItemPicker";
            _locationAddressPicker = new LocationAddressPicker();
            _locationAddressPicker.ID = this.ID + "_locationAddressPicker";
            _locationAddressPicker.Visible = false;
            _locationGeoPicker = new GeoPicker();
            _locationGeoPicker.ID = this.ID + "_locationGeoPicker";
            _locationGeoPicker.Visible = false;

            _locationItemPicker.ModePanel = _pnlModeSelection;
            _locationGeoPicker.ModePanel = _pnlModeSelection;

            Controls.Add( _locationItemPicker );
            Controls.Add( _locationAddressPicker );
            Controls.Add( _locationGeoPicker );
        }

        /// <summary>
        /// Handles the CheckedChanged event of the _radMode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void _radMode_CheckedChanged( object sender, EventArgs e )
        {
            _radNamedLocation.Checked = sender == _radNamedLocation;
            _radAddress.Checked = sender == _radAddress;
            _radLatLong.Checked = sender == _radLatLong;
            
            _locationItemPicker.Visible = _radNamedLocation.Checked;
            _locationAddressPicker.Visible = _radAddress.Checked;
            _locationGeoPicker.Visible = _radLatLong.Checked;

            _locationGeoPicker.ShowDropDown = _radLatLong.Checked;
        }

        #endregion
    }
}