//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupRolePicker : CompositeControl, IRockControl
    {
        #region IRockControl implementation (Custom implementation)

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        /// <value>
        /// The label text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The text for the label." )
        ]
        public string Label
        {
            get { return ViewState["Label"] as string ?? string.Empty; }
            set { ViewState["Label"] = value; }
        }

        /// <summary>
        /// Gets or sets the help text.
        /// </summary>
        /// <value>
        /// The help text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The help block." )
        ]
        public string Help
        {
            get
            {
                return HelpBlock != null ? HelpBlock.Text : string.Empty;
            }
            set
            {
                if ( HelpBlock != null )
                {
                    HelpBlock.Text = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RockTextBox"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        [
        Bindable( true ),
        Category( "Behavior" ),
        DefaultValue( "false" ),
        Description( "Is the value required?" )
        ]
        public bool Required
        {
            get { return _ddlGroupRole.Required; }
            set { _ddlGroupRole.Required = value; }
        }

        /// <summary>
        /// Gets or sets the required error message.  If blank, the LabelName name will be used
        /// </summary>
        /// <value>
        /// The required error message.
        /// </value>
        public string RequiredErrorMessage
        {
            get
            {
                return RequiredFieldValidator != null ? RequiredFieldValidator.ErrorMessage : string.Empty;
            }
            set
            {
                if ( RequiredFieldValidator != null )
                {
                    RequiredFieldValidator.ErrorMessage = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsValid
        {
            get
            {
                return !Required || RequiredFieldValidator == null || RequiredFieldValidator.IsValid;
            }
        }

        /// <summary>
        /// Gets or sets the help block.
        /// </summary>
        /// <value>
        /// The help block.
        /// </value>
        public HelpBlock HelpBlock { get; set; }

        /// <summary>
        /// Gets or sets the required field validator.
        /// </summary>
        /// <value>
        /// The required field validator.
        /// </value>
        public RequiredFieldValidator RequiredFieldValidator { get; set; }

        #endregion

        #region Controls

        private RockDropDownList _ddlGroupType;
        private RockDropDownList _ddlGroupRole;

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets or sets the group type id.
        /// </summary>
        /// <value>
        /// The group type id.
        /// </value>
        public int? GroupTypeId
        {
            get { return ViewState["GroupTypeId"] as int?; }
            set 
            { 
                ViewState["GroupTypeId"] = value;
                LoadGroupRoles( value.Value );
            }
        }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        /// <value>
        /// The role id.
        /// </value>
        public int? GroupRoleId
        {
            get
            {
                int groupRoleId = int.MinValue;
                if ( int.TryParse( _ddlGroupRole.SelectedValue, out groupRoleId ) && groupRoleId > 0 )
                {
                    return groupRoleId;
                }

                return null;
            }

            set
            {
                int groupRoleId = value.HasValue ? value.Value : 0;
                if ( _ddlGroupRole.SelectedValue != groupRoleId.ToString() )
                {
                    if ( !GroupTypeId.HasValue )
                    {
                        var groupRole = new Rock.Model.GroupRoleService().Get( groupRoleId );
                        if ( groupRole != null &&
                            groupRole.GroupTypeId.HasValue &&
                            _ddlGroupType.SelectedValue != groupRole.GroupTypeId.ToString() )
                        {
                            _ddlGroupType.SelectedValue = groupRole.GroupTypeId.ToString();

                            LoadGroupRoles( groupRole.GroupTypeId.Value );
                        }
                    }

                    var selectedItem = _ddlGroupRole.Items.FindByValue( groupRoleId.ToString() );
                    if ( selectedItem != null )
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupRolePicker"/> class.
        /// </summary>
        public GroupRolePicker()
            : base()
        {
            _ddlGroupType = new RockDropDownList();
            LoadGroupTypes();

            _ddlGroupRole = new RockDropDownList();
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();
            RockControlHelper.CreateChildControls( this, Controls );

            _ddlGroupType.ID = this.ID + "_ddlGroupType";
            _ddlGroupType.AutoPostBack = true;
            _ddlGroupType.SelectedIndexChanged += _ddlGroupType_SelectedIndexChanged;
            Controls.Add( _ddlGroupType );

            _ddlGroupRole.ID = this.ID;
            Controls.Add( _ddlGroupRole );
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( this.Visible )
            {
                if ( !GroupTypeId.HasValue )
                {
                    _ddlGroupType.Label = this.Label;
                    _ddlGroupType.Help = this.Help;
                    _ddlGroupType.RenderControl( writer );

                    _ddlGroupRole.Label = (_ddlGroupType.SelectedItem != null ? _ddlGroupType.SelectedItem.Text : this.Label) + " Role";
                }
                else
                {
                    _ddlGroupRole.Label = this.Label;
                    _ddlGroupRole.Help = this.Help;
                }

                _ddlGroupRole.RenderControl( writer );
            }
        }

        /// <summary>
        /// Renders the base control.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void RenderBaseControl( HtmlTextWriter writer )
        {
        }

        private void LoadGroupTypes()
        {
            _ddlGroupType.Items.Clear();

            var groupTypeService = new Rock.Model.GroupTypeService();
            var groupTypes = groupTypeService.Queryable().OrderBy( a => a.Name ).ToList();
            groupTypes.ForEach( g => 
                _ddlGroupType.Items.Add( new ListItem( g.Name, g.Id.ToString().ToUpper() ))
            );
        }

        private void LoadGroupRoles(int? groupTypeId)
        {
            _ddlGroupRole.Items.Clear();
            if ( groupTypeId.HasValue )
            {
                if ( !Required )
                {
                    _ddlGroupRole.Items.Add( new ListItem( string.Empty, Rock.Constants.None.IdValue ) );
                }

                var groupRoleService = new Rock.Model.GroupRoleService();
                var groupRoles = groupRoleService.Queryable().Where( r => r.GroupTypeId == groupTypeId.Value ).OrderBy( a => a.Name ).ToList();
                groupRoles.ForEach( r =>
                    _ddlGroupRole.Items.Add( new ListItem( r.Name, r.Id.ToString().ToUpper() ) )
                );
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the _ddlGroupType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void _ddlGroupType_SelectedIndexChanged( object sender, EventArgs e )
        {
            int groupTypeId = int.MinValue;
            if ( int.TryParse( _ddlGroupType.SelectedValue, out groupTypeId ) && groupTypeId > 0 )
            {
                LoadGroupRoles( groupTypeId );
            }
        }
    }
}