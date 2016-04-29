﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Rock.Data;
using Rock.Model;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// Report Filter control
    /// </summary>
    [ToolboxData( "<{0}:CheckinGroup runat=server></{0}:CheckinGroup>" )]
    public class CheckinGroup : CompositeControl, IHasValidationGroup
    {
        private HiddenField _hfGroupGuid;
        private HiddenField _hfGroupId;
        private HiddenField _hfGroupTypeId;
        private Literal _lblGroupName;

        private DataTextBox _tbGroupName;
        private PlaceHolder _phGroupAttributes;
        private Grid _gLocations;

        /// <summary>
        /// Gets or sets the validation group.
        /// </summary>
        /// <value>
        /// The validation group.
        /// </value>
        public string ValidationGroup
        {
            get
            {
                return ViewState["ValidationGroup"] as string;
            }
            set
            {
                ViewState["ValidationGroup"] = value;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( Page.IsPostBack )
            {
                HandleGridEvents();
            }
        }

        /// <summary>
        /// Handles the grid events.
        /// </summary>
        private void HandleGridEvents()
        {
            // manually wireup the grid events since they don't seem to do it automatically 
            string eventTarget = Page.Request.Params["__EVENTTARGET"];
            if ( eventTarget.StartsWith( _gLocations.UniqueID ) )
            {
                List<string> subTargetList = eventTarget.Replace( _gLocations.UniqueID, string.Empty ).Split( new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries ).ToList();
                EnsureChildControls();
                string lblAddControlId = subTargetList.Last();
                var lblAdd = _gLocations.Actions.FindControl( lblAddControlId );
                if ( lblAdd != null )
                {
                    AddLocation_Click( this, new EventArgs() );
                }
                else
                {
                    // rowIndex is determined by the numeric suffix of the control id after the Grid, subtract 2 (one for the header, and another to convert from 0 to 1 based index)
                    int rowIndex = subTargetList.First().AsNumeric().AsInteger() - 2;
                    RowEventArgs rowEventArgs = new RowEventArgs( rowIndex, this.Locations.OrderBy( l => l.FullNamePath ).ToList()[rowIndex].LocationId );
                    DeleteLocation_Click( this, rowEventArgs );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class LocationGridItem
        {
            /// <summary>
            /// Gets or sets the location unique identifier.
            /// </summary>
            /// <value>
            /// The location unique identifier.
            /// </value>
            [DataMember]
            public int LocationId { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            [DataMember]
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the name in the format ParentLocation1 > ParentLocation0 > Name
            /// </summary>
            /// <value>
            /// The full name path.
            /// </value>
            [DataMember]
            public string FullNamePath { get; set; }

            /// <summary>
            /// Gets or sets the parent location identifier.
            /// </summary>
            /// <value>
            /// The parent location identifier.
            /// </value>
            [DataMember]
            public int? ParentLocationId { get; set; }
        }

        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        /// <value>
        /// The locations.
        /// </value>
        public List<LocationGridItem> Locations
        {
            get
            {
                return ViewState["Locations"] as List<LocationGridItem>;
            }

            set
            {
                ViewState["Locations"] = value;
            }
        }

        /// <summary>
        /// Gets the group unique identifier.
        /// </summary>
        /// <value>
        /// The group unique identifier.
        /// </value>
        public Guid GroupGuid
        {
            get
            {
                EnsureChildControls();
                return new Guid( _hfGroupGuid.Value );
            }
        }

        /// <summary>
        /// Gets the group type unique identifier.
        /// </summary>
        /// <value>
        /// The group type unique identifier.
        /// </value>
        public int GroupTypeId
        {
            get
            {
                EnsureChildControls();
                return _hfGroupTypeId.ValueAsInt();
            }
        }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        /// <value>
        /// The group.
        /// </value>
        public void GetGroupValues( Group group )
        {
            group.Name = _tbGroupName.Text;

            Rock.Attribute.Helper.GetEditValues( _phGroupAttributes, group );
        }

        /// <summary>
        /// Sets the group.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="rockContext">The rock context.</param>
        public void SetGroup( Group value, RockContext rockContext )
        {
            EnsureChildControls();

            //// NOTE:  A Group that was added will have an Id since it hasn't been saved to the database. 
            //// So, we'll use Guid to uniquely identify in this Control since that'll work in both Saved and Unsaved cases.
            //// If it is saved, we do need the Id so that Attributes will work

            _hfGroupGuid.Value = value.Guid.ToString();
            _hfGroupId.Value = value.Id.ToString();
            _hfGroupTypeId.Value = value.GroupTypeId.ToString();
            _tbGroupName.Text = value.Name;

            CreateGroupAttributeControls( value, rockContext );
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            _hfGroupGuid = new HiddenField();
            _hfGroupGuid.ID = this.ID + "_hfGroupGuid";

            _hfGroupId = new HiddenField();
            _hfGroupId.ID = this.ID + "_hfGroupId";

            _hfGroupTypeId = new HiddenField();
            _hfGroupTypeId.ID = this.ID + "_hfGroupTypeId";

            _lblGroupName = new Literal();
            _lblGroupName.ID = this.ID + "_lblGroupName";

            _tbGroupName = new DataTextBox();
            _tbGroupName.ID = this.ID + "_tbGroupName";
            _tbGroupName.Label = "Check-in Group Name";

            // set label when they exit the edit field
            _tbGroupName.Attributes["onblur"] = string.Format( "javascript: $('#{0}').text($(this).val());", _lblGroupName.ID );
            _tbGroupName.SourceTypeName = "Rock.Model.Group, Rock";
            _tbGroupName.PropertyName = "Name";

            _phGroupAttributes = new PlaceHolder();
            _phGroupAttributes.ID = this.ID + "_phGroupAttributes";

            Controls.Add( _hfGroupGuid );
            Controls.Add( _hfGroupId );
            Controls.Add( _hfGroupTypeId );
            Controls.Add( _lblGroupName );
            Controls.Add( _tbGroupName );
            Controls.Add( _phGroupAttributes );

            // Locations Grid
            CreateLocationsGrid();
        }

        /// <summary>
        /// Creates the locations grid.
        /// </summary>
        private void CreateLocationsGrid()
        {
            _gLocations = new Grid();

            _gLocations.ID = this.ID + "_gCheckinLabels";

            _gLocations.DisplayType = GridDisplayType.Light;
            _gLocations.ShowActionRow = true;
            _gLocations.RowItemText = "Location";
            _gLocations.Actions.ShowAdd = true;

            //// Handle AddClick manually in OnLoad()
            _gLocations.Actions.AddClick += AddLocation_Click;

            _gLocations.DataKeyNames = new string[] { "LocationId" };
            _gLocations.TooltipField = "Name";
            _gLocations.Columns.Add( new BoundField { DataField = "FullNamePath", HeaderText = "Name" } );

            DeleteField deleteField = new DeleteField();

            //// handle manually in OnLoad()
            deleteField.Click += DeleteLocation_Click;

            _gLocations.Columns.Add( deleteField );

            Controls.Add( _gLocations );
        }

        /// <summary>
        /// Writes the <see cref="T:System.Web.UI.WebControls.CompositeControl" /> content to the specified <see cref="T:System.Web.UI.HtmlTextWriter" /> object, for display on the client.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the output stream to render HTML content on the client.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( this.Visible )
            {
                _hfGroupGuid.RenderControl( writer );
                _hfGroupId.RenderControl( writer );
                _hfGroupTypeId.RenderControl( writer );

                writer.AddAttribute( HtmlTextWriterAttribute.Style, "margin-top:0;" );
                writer.RenderBeginTag( HtmlTextWriterTag.H3 );
                _lblGroupName.Text = _tbGroupName.Text;
                _lblGroupName.RenderControl( writer );
                writer.RenderEndTag();

                _tbGroupName.RenderControl( writer );
                _phGroupAttributes.RenderControl( writer );

                writer.WriteLine( "<h3>Locations</h3>" );
                if ( this.Locations != null )
                {
                    _gLocations.DataSource = this.Locations.OrderBy( l => l.FullNamePath ).ToList();
                    _gLocations.DataBind();
                }
                _gLocations.RenderControl( writer );
            }
        }

        /// <summary>
        /// Creates the group attribute controls.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="rockContext">The rock context.</param>
        public void CreateGroupAttributeControls( Group group, RockContext rockContext )
        {
            EnsureChildControls();

            _phGroupAttributes.Controls.Clear();

            if ( group != null )
            {
                if ( group.Attributes == null )
                {
                    group.LoadAttributes( rockContext );
                }
                Rock.Attribute.Helper.AddEditControls( group, _phGroupAttributes, true, this.ValidationGroup );
            }
        }

        /// <summary>
        /// Handles the Click event of the DeleteLocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void DeleteLocation_Click( object sender, RowEventArgs e )
        {
            if ( DeleteLocationClick != null )
            {
                DeleteLocationClick( sender, e );
            }
        }

        /// <summary>
        /// Handles the Click event of the AddLocation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void AddLocation_Click( object sender, EventArgs e )
        {
            if ( AddLocationClick != null )
            {
                AddLocationClick( sender, e );
            }
        }

        /// <summary>
        /// Occurs when [delete location click].
        /// </summary>
        public event EventHandler<RowEventArgs> DeleteLocationClick;

        /// <summary>
        /// Occurs when [add location click].
        /// </summary>
        public event EventHandler AddLocationClick;
    }
}