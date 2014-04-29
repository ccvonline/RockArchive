﻿// <copyright>
// Copyright 2013 by the Spark Development Network
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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Workflow;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// Report Filter control
    /// </summary>
    [ToolboxData( "<{0}:WorkflowActionTypeEditor runat=server></{0}:WorkflowActionTypeEditor>" )]
    public class WorkflowActionEditor : CompositeControl
    {
        private HiddenField _hfActionTypeGuid;
        private Label _lblActionTypeName;
        private LinkButton _lbDeleteActionType;

        private DataTextBox _tbActionTypeName;
        private RockDropDownList _ddlEntityType;
        private RockCheckBox _cbIsActionCompletedOnSuccess;
        private RockCheckBox _cbIsActivityCompletedOnSuccess;
        private WorkflowFormEditor _formEditor;
        private PlaceHolder _phActionAttributes;

        /// <summary>
        /// Gets or sets a value indicating whether to force content visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [force content visible]; otherwise, <c>false</c>.
        /// </value>
        public bool ForceContentVisible { get; set; }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            string script = @"
// action animation
$('.workflow-action > header').click(function () {
    $(this).siblings('.panel-body').slideToggle();

    $('i.workflow-action-state', this).toggleClass('fa-chevron-down');
    $('i.workflow-action-state', this).toggleClass('fa-chevron-up');
});

// fix so that the Remove button will fire its event, but not the parent event 
$('.workflow-action a.btn-danger').click(function (event) {
    event.stopImmediatePropagation();
});

// fix so that the Reorder button will fire its event, but not the parent event 
$('.workflow-action a.workflow-action-reorder').click(function (event) {
    event.stopImmediatePropagation();
});

$('a.workflow-formfield-reorder').click(function (event) {
    event.stopImmediatePropagation();
});
";
            ScriptManager.RegisterStartupScript( this.Page, this.Page.GetType(), "WorkflowActionTypeEditorScript", script, true );
        }

        /// <summary>
        /// Sets the workflow attributes.
        /// </summary>
        /// <value>
        /// The workflow attributes.
        /// </value>
        public Dictionary<Guid, string> WorkflowAttributes
        {
            set
            {
                EnsureChildControls();
                _formEditor.WorkflowAttributes = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is delete enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is delete enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleteEnabled
        {
            get
            {
                bool? b = ViewState["IsDeleteEnabled"] as bool?;
                return ( b == null ) ? true : b.Value;
            }

            set
            {
                ViewState["IsDeleteEnabled"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the workflow activity.
        /// </summary>
        /// <value>
        /// The type of the workflow activity.
        /// </value>
        public WorkflowActionType WorkflowActionType
        {
            get
            {
                EnsureChildControls();
                WorkflowActionType result = new WorkflowActionType();
                result.Guid = new Guid( _hfActionTypeGuid.Value );
                result.Name = _tbActionTypeName.Text;
                result.EntityTypeId = _ddlEntityType.SelectedValueAsInt() ?? 0;
                result.IsActionCompletedOnSuccess = _cbIsActionCompletedOnSuccess.Checked;
                result.IsActivityCompletedOnSuccess = _cbIsActivityCompletedOnSuccess.Checked;

                var entityType = EntityTypeCache.Read( result.EntityTypeId );
                if ( entityType != null && entityType.Name == typeof( Rock.Workflow.Action.UserEntryForm ).FullName )
                {
                    result.WorkflowForm = _formEditor.Form ?? new WorkflowActionForm { Actions = "Submit^Submit" };
                }
                else
                {
                    result.WorkflowForm = null;
                }

                result.LoadAttributes();
                Rock.Attribute.Helper.GetEditValues( _phActionAttributes, result );
                return result;
            }

            set
            {
                EnsureChildControls();
                _hfActionTypeGuid.Value = value.Guid.ToString();
                _tbActionTypeName.Text = value.Name;
                _ddlEntityType.SetValue( value.EntityTypeId );
                _cbIsActionCompletedOnSuccess.Checked = value.IsActionCompletedOnSuccess;
                _cbIsActivityCompletedOnSuccess.Checked = value.IsActivityCompletedOnSuccess;

                var entityType = EntityTypeCache.Read( value.EntityTypeId );
                if ( entityType != null && entityType.Name == typeof( Rock.Workflow.Action.UserEntryForm ).FullName )
                {
                    _formEditor.Form = value.WorkflowForm ?? new WorkflowActionForm { Actions = "Submit^Submit" };
                }
                else
                {
                    _formEditor.Form = null;
                }

                var action = EntityTypeCache.Read( value.EntityTypeId );
                if ( action != null )
                {
                    var rockContext = new RockContext();
                    Rock.Attribute.Helper.UpdateAttributes( action.GetEntityType(), value.TypeId, "EntityTypeId", value.EntityTypeId.ToString(), rockContext );
                    value.LoadAttributes( rockContext );
                }

                _phActionAttributes.Controls.Clear();
                Rock.Attribute.Helper.AddEditControls( value, _phActionAttributes, true, string.Empty, new List<string>() { "Active", "Order" } );
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            _hfActionTypeGuid = new HiddenField();
            _hfActionTypeGuid.ID = this.ID + "_hfActionTypeGuid";

            _lblActionTypeName = new Label();
            _lblActionTypeName.ClientIDMode = ClientIDMode.Static;
            _lblActionTypeName.ID = this.ID + "_lblActionTypeName";

            _lbDeleteActionType = new LinkButton();
            _lbDeleteActionType.CausesValidation = false;
            _lbDeleteActionType.ID = this.ID + "_lbDeleteActionType";
            _lbDeleteActionType.CssClass = "btn btn-xs btn-danger";
            _lbDeleteActionType.Click += lbDeleteActionType_Click;

            var iDelete = new HtmlGenericControl( "i" );
            _lbDeleteActionType.Controls.Add( iDelete );
            iDelete.AddCssClass( "fa fa-times" );

            _tbActionTypeName = new DataTextBox();
            _tbActionTypeName.ID = this.ID + "_tbActionTypeName";
            _tbActionTypeName.Label = "Name";

            _ddlEntityType = new RockDropDownList();
            _ddlEntityType.ID = this.ID + "_ddlEntityType";
            _ddlEntityType.Label = "Action Type";

            // make it autopostback since Attributes are dependant on which EntityType is selected
            _ddlEntityType.AutoPostBack = true;
            _ddlEntityType.SelectedIndexChanged += ddlEntityType_SelectedIndexChanged;

            foreach ( var item in WorkflowActionContainer.Instance.Components.Values.OrderBy( a => a.Value.EntityType.FriendlyName ) )
            {
                var type = item.Value.GetType();
                if (type != null)
                {
                    var entityType = EntityTypeCache.Read( type );
                    var li = new ListItem( entityType.FriendlyName, entityType.Id.ToString() );

                    // Get description
                    string description = string.Empty;
                    var descAttributes = type.GetCustomAttributes( typeof( System.ComponentModel.DescriptionAttribute ), false );
                    if ( descAttributes != null )
                    {
                        foreach ( System.ComponentModel.DescriptionAttribute descAttribute in descAttributes )
                        {
                            description = descAttribute.Description;
                        }
                    }
                    if ( !string.IsNullOrWhiteSpace( description ) )
                    {
                        li.Attributes.Add( "title", description );
                    }

                    _ddlEntityType.Items.Add( li );
                }
            }

            // set label when they exit the edit field
            _tbActionTypeName.Attributes["onblur"] = string.Format( "javascript: $('#{0}').text($(this).val());", _lblActionTypeName.ID );
            _tbActionTypeName.SourceTypeName = "Rock.Model.WorkflowActionType, Rock";
            _tbActionTypeName.PropertyName = "Name";

            _cbIsActionCompletedOnSuccess = new RockCheckBox { Text = "Action is Completed on Success" };
            _cbIsActionCompletedOnSuccess.ID = this.ID + "_cbIsActionCompletedOnSuccess";

            _cbIsActivityCompletedOnSuccess = new RockCheckBox { Text = "Activity is Completed on Success" };
            _cbIsActivityCompletedOnSuccess.ID = this.ID + "_cbIsActivityCompletedOnSuccess";

            _formEditor = new WorkflowFormEditor();
            _formEditor.ID = this.ID + "_formEditor";

            _phActionAttributes = new PlaceHolder();
            _phActionAttributes.ID = this.ID + "_phActionAttributes";

            Controls.Add( _hfActionTypeGuid );
            Controls.Add( _lblActionTypeName );
            Controls.Add( _tbActionTypeName );
            Controls.Add( _ddlEntityType );
            Controls.Add( _cbIsActionCompletedOnSuccess );
            Controls.Add( _cbIsActivityCompletedOnSuccess );
            Controls.Add( _formEditor );
            Controls.Add( _phActionAttributes );
            Controls.Add( _lbDeleteActionType );
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlEntityType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlEntityType_SelectedIndexChanged( object sender, EventArgs e )
        {
            WorkflowActionType workflowActionType = WorkflowActionType;
            workflowActionType.EntityTypeId = _ddlEntityType.SelectedValueAsInt() ?? 0;
            WorkflowActionType = workflowActionType;
            this.ForceContentVisible = true;
        }

        /// <summary>
        /// Writes the <see cref="T:System.Web.UI.WebControls.CompositeControl" /> content to the specified <see cref="T:System.Web.UI.HtmlTextWriter" /> object, for display on the client.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the output stream to render HTML content on the client.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            writer.AddAttribute( HtmlTextWriterAttribute.Class, "panel panel-widget workflow-action" );
            writer.AddAttribute( "data-key", _hfActionTypeGuid.Value );
            writer.RenderBeginTag( "article" );

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "clearfix clickable panel-heading" );
            writer.RenderBeginTag( "header" );

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "pull-left" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _lblActionTypeName.Text = _tbActionTypeName.Text;
            _lblActionTypeName.RenderControl( writer );
            writer.RenderEndTag();

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "pull-right" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.WriteLine( "<a class='btn btn-xs btn-link workflow-action-reorder'><i class='fa fa-bars'></i></a>" );
            writer.WriteLine( "<a class='btn btn-xs btn-link'><i class='workflow-action-state fa fa-chevron-down'></i></a>" );

            if ( IsDeleteEnabled )
            {
                _lbDeleteActionType.Visible = true;

                _lbDeleteActionType.RenderControl( writer );
            }
            else
            {
                _lbDeleteActionType.Visible = false;
            }

            // Add/ChevronUpDown/Delete div
            writer.RenderEndTag();

            // header div
            writer.RenderEndTag();

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "panel-body" );

            bool forceContentVisible = !WorkflowActionType.IsValid || ForceContentVisible;

            if ( !forceContentVisible )
            {
                // hide details if the name has already been filled in
                writer.AddStyleAttribute( "display", "none" );
            }

            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            // action edit fields
            writer.AddAttribute( HtmlTextWriterAttribute.Class, "row" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-md-6" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _tbActionTypeName.RenderControl( writer );
            _ddlEntityType.RenderControl( writer );
            writer.RenderEndTag();

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-md-6" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _cbIsActionCompletedOnSuccess.RenderControl( writer );
            _cbIsActivityCompletedOnSuccess.RenderControl( writer );
            writer.RenderEndTag();

            writer.RenderEndTag();

            _formEditor.RenderControl( writer );

            _phActionAttributes.RenderControl( writer );

            // widget-content div
            writer.RenderEndTag();

            // article tag
            writer.RenderEndTag();
        }

        /// <summary>
        /// Handles the Click event of the lbDeleteActionType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbDeleteActionType_Click( object sender, EventArgs e )
        {
            if ( DeleteActionTypeClick != null )
            {
                DeleteActionTypeClick( this, e );
            }
        }

        /// <summary>
        /// Occurs when [delete action type click].
        /// </summary>
        public event EventHandler DeleteActionTypeClick;
    }
}