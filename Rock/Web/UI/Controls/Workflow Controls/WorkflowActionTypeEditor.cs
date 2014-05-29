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
    /// Control used by WorkflowTypeDetail block to edit a workflow action type
    /// </summary>
    [ToolboxData( "<{0}:WorkflowActionTypeEditor runat=server></{0}:WorkflowActionTypeEditor>" )]
    public class WorkflowActionTypeEditor : CompositeControl, IHasValidationGroup
    {
        private HiddenField _hfExpanded;
        private HiddenField _hfActionTypeGuid;
        private Label _lblActionTypeName;
        private LinkButton _lbDeleteActionType;

        private RockDropDownList _ddlCriteriaAttribute;
        private RockDropDownList _ddlCriteriaComparisonType;
        private RockTextOrDropDownList _tbddlCriteriaValue;

        private RockTextBox _tbActionTypeName;
        private RockDropDownList _ddlEntityType;
        private RockCheckBox _cbIsActionCompletedOnSuccess;
        private RockCheckBox _cbIsActivityCompletedOnSuccess;
        private WorkflowFormEditor _formEditor;
        private PlaceHolder _phActionAttributes;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WorkflowActionTypeEditor"/> is expanded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if expanded; otherwise, <c>false</c>.
        /// </value>
        public bool Expanded
        {
            get
            {
                EnsureChildControls();

                bool expanded = false;
                if ( !bool.TryParse( _hfExpanded.Value, out expanded ) )
                {
                    expanded = false;
                }

                return expanded;
            }

            set
            {
                EnsureChildControls();
                _hfExpanded.Value = value.ToString();
            }
        }

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
        /// Gets or sets the activity type unique identifier.
        /// </summary>
        /// <value>
        /// The activity type unique identifier.
        /// </value>
        public Guid ActionTypeGuid
        {
            get
            {
                EnsureChildControls();
                return _hfActionTypeGuid.Value.AsGuid();
            }
        }

        /// <summary>
        /// Gets the form editor.
        /// </summary>
        /// <value>
        /// The form editor.
        /// </value>
        public WorkflowFormEditor FormEditor
        {
            get
            {
                EnsureChildControls();
                return _formEditor;
            }
        }

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

    $expanded = $(this).children('input.filter-expanded');
    $expanded.val($expanded.val() == 'True' ? 'False' : 'True');

    $('i.workflow-action-state', this).toggleClass('fa-chevron-down');
    $('i.workflow-action-state', this).toggleClass('fa-chevron-up');
});

// fix so that the Remove button will fire its event, but not the parent event 
$('.workflow-action a.js-action-delete').click(function (event) {
    event.stopImmediatePropagation();
    return Rock.dialogs.confirmDelete(event, 'Action Type', 'This will also delete all the actions of this type from any existing persisted workflows!');
});

$('.workflow-action a.js-workflow-action-criteria').click(function (event) {
    event.stopImmediatePropagation();
    $(this).closest('.workflow-action').find('div.conditional-run-criteria').slideToggle();
});

$(document).on('focusout', '.js-conditional-run-criteria', function (e) {
    if ( $(this).val() == '' ) {
        $(this).closest('.workflow-action').find('a.js-workflow-action-criteria').removeClass('criteria-exists');
    } else {
        $(this).closest('.workflow-action').find('a.js-workflow-action-criteria').addClass('criteria-exists');
    }
});

$('.js-action-criteria-comparison').change( function (event) {
    var $valueRow = $(this).closest('div.conditional-run-criteria').find('div.js-text-or-ddl-row');
    if ($(this).val() == '32' || $(this).val() == '64') {
        $valueRow.slideUp();
    } else {
        $valueRow.slideDown();
    }
});

// fix so that the Reorder button will fire its event, but not the parent event 
$('.workflow-action a.workflow-action-reorder').click(function (event) {
    event.stopImmediatePropagation();
});

$('a.workflow-formfield-reorder').click(function (event) {
    event.stopImmediatePropagation();
});

$('.workflow-action > .panel-body').on('validation-error', function() {
    var $header = $(this).siblings('header');
    $(this).slideDown();

    $expanded = $header.children('input.filter-expanded');
    $expanded.val('True');

    $('i.workflow-action-state', $header).removeClass('fa-chevron-down');
    $('i.workflow-action-state', $header).addClass('fa-chevron-up');
});
";
            ScriptManager.RegisterStartupScript( this, this.GetType(), "WorkflowActionTypeEditorScript", script, true );
        }

        /// <summary>
        /// Sets the workflow activities.
        /// </summary>
        /// <value>
        /// The workflow activities.
        /// </value>
        public Dictionary<string, string> WorkflowActivities
        {
            set
            {
                EnsureChildControls();
                _formEditor.WorkflowActivities = value;
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
        /// Gets the type of the workflow action.
        /// </summary>
        /// <returns></returns>
        public WorkflowActionType GetWorkflowActionType( bool expandInvalid )
        {
            EnsureChildControls();
            WorkflowActionType result = new WorkflowActionType();
            result.Guid = new Guid( _hfActionTypeGuid.Value );
            
            result.CriteriaAttributeGuid = _ddlCriteriaAttribute.SelectedValueAsGuid();
            result.CriteriaComparisonType = _ddlCriteriaComparisonType.SelectedValueAsEnum<ComparisonType>();
            result.CriteriaValue = _tbddlCriteriaValue.SelectedValue;

            result.Name = _tbActionTypeName.Text;
            result.EntityTypeId = _ddlEntityType.SelectedValueAsInt() ?? 0;
            result.IsActionCompletedOnSuccess = _cbIsActionCompletedOnSuccess.Checked;
            result.IsActivityCompletedOnSuccess = _cbIsActivityCompletedOnSuccess.Checked;

            var entityType = EntityTypeCache.Read( result.EntityTypeId );
            if ( entityType != null && entityType.Name == typeof( Rock.Workflow.Action.UserEntryForm ).FullName )
            {
                result.WorkflowForm = _formEditor.Form ?? new WorkflowActionForm { Actions = "Submit^^^" };
            }
            else
            {
                result.WorkflowForm = null;
            }

            result.LoadAttributes();
            Rock.Attribute.Helper.GetEditValues( _phActionAttributes, result );

            if (expandInvalid && !result.IsValid)
            {
                Expanded = true;
            }

            return result;
        }

        /// <summary>
        /// Sets the type of the workflow action.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetWorkflowActionType(WorkflowActionType value, Dictionary<Guid, string> workflowTypeAttributes )
        {
            EnsureChildControls();
            _hfActionTypeGuid.Value = value.Guid.ToString();

            _ddlCriteriaAttribute.Items.Clear();
            _ddlCriteriaAttribute.Items.Add( new ListItem() );

            _tbddlCriteriaValue.DropDownList.Items.Clear();
            _tbddlCriteriaValue.DropDownList.Items.Add( new ListItem() );
            foreach ( var attribute in workflowTypeAttributes )
            {
                var li = new ListItem( attribute.Value, attribute.Key.ToString() );
                li.Selected = value.CriteriaAttributeGuid.HasValue && value.CriteriaAttributeGuid.Value.ToString() == li.Value; 
                _ddlCriteriaAttribute.Items.Add( li );

                _tbddlCriteriaValue.DropDownList.Items.Add( new ListItem( attribute.Value, attribute.Key.ToString() ) );
            }

            _ddlCriteriaComparisonType.SetValue( value.CriteriaComparisonType.ConvertToInt() );
            _tbddlCriteriaValue.SelectedValue = value.CriteriaValue;

            _tbActionTypeName.Text = value.Name;
            _ddlEntityType.SetValue( value.EntityTypeId );
            _cbIsActivityCompletedOnSuccess.Checked = value.IsActivityCompletedOnSuccess;

            var entityType = EntityTypeCache.Read( value.EntityTypeId );
            if ( entityType != null && entityType.Name == typeof( Rock.Workflow.Action.UserEntryForm ).FullName )
            {
                _formEditor.Form = value.WorkflowForm ?? new WorkflowActionForm { Actions = "Submit^^^" };
                _cbIsActionCompletedOnSuccess.Checked = true;
                _cbIsActionCompletedOnSuccess.Enabled = false;
            }
            else
            {
                _formEditor.Form = null;
                _cbIsActionCompletedOnSuccess.Checked = value.IsActionCompletedOnSuccess;
                _cbIsActionCompletedOnSuccess.Enabled = true;
            }

            _phActionAttributes.Controls.Clear();
            Rock.Attribute.Helper.AddEditControls( value, _phActionAttributes, true, ValidationGroup, new List<string>() { "Active", "Order" } );
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            _hfExpanded = new HiddenField();
            Controls.Add( _hfExpanded );
            _hfExpanded.ID = this.ID + "_hfExpanded";
            _hfExpanded.Value = "False";

            _hfActionTypeGuid = new HiddenField();
            Controls.Add( _hfActionTypeGuid );
            _hfActionTypeGuid.ID = this.ID + "_hfActionTypeGuid";

            _lblActionTypeName = new Label();
            Controls.Add( _lblActionTypeName );
            _lblActionTypeName.ClientIDMode = ClientIDMode.Static;
            _lblActionTypeName.ID = this.ID + "_lblActionTypeName";

            _lbDeleteActionType = new LinkButton();
            Controls.Add( _lbDeleteActionType );
            _lbDeleteActionType.CausesValidation = false;
            _lbDeleteActionType.ID = this.ID + "_lbDeleteActionType";
            _lbDeleteActionType.CssClass = "btn btn-xs btn-danger js-action-delete";
            _lbDeleteActionType.Click += lbDeleteActionType_Click;

            var iDelete = new HtmlGenericControl( "i" );
            _lbDeleteActionType.Controls.Add( iDelete );
            iDelete.AddCssClass( "fa fa-times" );

            _ddlCriteriaAttribute = new RockDropDownList();
            Controls.Add( _ddlCriteriaAttribute );
            _ddlCriteriaAttribute.ID = this.ID + "_ddlCriteriaAttribute";
            _ddlCriteriaAttribute.CssClass = "js-conditional-run-criteria";
            _ddlCriteriaAttribute.Label = "Conditional Run Criteria";
            _ddlCriteriaAttribute.Help = "Optional criteria to prevent action from running.  If the criteria is not met, the action will be skipped when this activity is processed.";

            _ddlCriteriaComparisonType = new RockDropDownList();
            Controls.Add( _ddlCriteriaComparisonType );
            _ddlCriteriaComparisonType.ID = this.ID + "_ddlCriteriaComparisonType";
            _ddlCriteriaComparisonType.CssClass = "js-action-criteria-comparison";
            _ddlCriteriaComparisonType.BindToEnum( typeof( ComparisonType ) );
            _ddlCriteriaComparisonType.Label = "&nbsp;";

            _tbddlCriteriaValue = new RockTextOrDropDownList();
            Controls.Add( _tbddlCriteriaValue );
            _tbddlCriteriaValue.ID = this.ID + "_tbddlCriteriaValue";
            _tbddlCriteriaValue.TextBox.Label = "Text Value";
            _tbddlCriteriaValue.DropDownList.Label = "Attribute Value";

            _tbActionTypeName = new RockTextBox();
            Controls.Add( _tbActionTypeName );
            _tbActionTypeName.ID = this.ID + "_tbActionTypeName";
            _tbActionTypeName.Label = "Name";
            _tbActionTypeName.Required = true;
            _tbActionTypeName.Attributes["onblur"] = string.Format( "javascript: $('#{0}').text($(this).val());", _lblActionTypeName.ID );

            _ddlEntityType = new RockDropDownList();
            Controls.Add( _ddlEntityType );
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

            _cbIsActionCompletedOnSuccess = new RockCheckBox { Text = "Action is Completed on Success" };
            Controls.Add( _cbIsActionCompletedOnSuccess );
            _cbIsActionCompletedOnSuccess.ID = this.ID + "_cbIsActionCompletedOnSuccess";

            _cbIsActivityCompletedOnSuccess = new RockCheckBox { Text = "Activity is Completed on Success" };
            Controls.Add( _cbIsActivityCompletedOnSuccess );
            _cbIsActivityCompletedOnSuccess.ID = this.ID + "_cbIsActivityCompletedOnSuccess";

            _formEditor = new WorkflowFormEditor();
            Controls.Add( _formEditor );
            _formEditor.ID = this.ID + "_formEditor";

            _phActionAttributes = new PlaceHolder();
            Controls.Add( _phActionAttributes );
            _phActionAttributes.ID = this.ID + "_phActionAttributes";

        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlEntityType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlEntityType_SelectedIndexChanged( object sender, EventArgs e )
        {
            var workflowActionType = GetWorkflowActionType( false );
            workflowActionType.EntityTypeId = _ddlEntityType.SelectedValueAsInt() ?? 0;

            _hfActionTypeGuid.Value = workflowActionType.Guid.ToString();

            if ( ChangeActionTypeClick != null )
            {
                ChangeActionTypeClick( this, e );
            }

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

            // Hidden Field to track expansion
            writer.AddAttribute( HtmlTextWriterAttribute.Class, "filter-expanded" );
            _hfExpanded.RenderControl( writer );

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "pull-left workflow-action-name" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _lblActionTypeName.Text = _tbActionTypeName.Text;
            _lblActionTypeName.RenderControl( writer );
            writer.RenderEndTag();

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "pull-right" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            string criteriaExistsClass = _ddlCriteriaAttribute.SelectedValueAsGuid().HasValue ? " criteria-exists" : string.Empty;
            writer.WriteLine( string.Format( "<a class='btn btn-xs btn-link js-workflow-action-criteria{0}'><i class='fa fa-filter'></i></a>", criteriaExistsClass ) );
            writer.WriteLine( "<a class='btn btn-xs btn-link workflow-action-reorder'><i class='fa fa-bars'></i></a>" );
            writer.WriteLine( string.Format( "<a class='btn btn-xs btn-link'><i class='workflow-action-state fa {0}'></i></a>",
                Expanded ? "fa fa-chevron-up" : "fa fa-chevron-down" ) );

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

            if ( !Expanded )
            {
                // hide details if the name has already been filled in
                writer.AddStyleAttribute( "display", "none" );
            }
            writer.AddAttribute( HtmlTextWriterAttribute.Class, "panel-body" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            // add Criteria fields
            writer.AddAttribute( HtmlTextWriterAttribute.Class, "row conditional-run-criteria alert-warning" );
            if ( !_ddlCriteriaAttribute.SelectedValueAsGuid().HasValue )
            {
                writer.AddStyleAttribute( "display", "none" );
            }
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-lg-6" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "row" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-xs-7" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _ddlCriteriaAttribute.RenderControl( writer );
            writer.RenderEndTag();

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-xs-5" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _ddlCriteriaComparisonType.RenderControl( writer );
            writer.RenderEndTag();

            writer.RenderEndTag();  // row

            writer.RenderEndTag();  // col-md-6

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-lg-6" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            var comparisonType = _ddlCriteriaComparisonType.SelectedValueAsEnum<ComparisonType>();
            _tbddlCriteriaValue.Style["display"] = ( comparisonType == ComparisonType.IsBlank || comparisonType == ComparisonType.IsNotBlank ) ? "none" : "block";
            _tbddlCriteriaValue.RenderControl( writer );
            writer.RenderEndTag();

            writer.RenderEndTag();

            // action edit fields
            writer.AddAttribute( HtmlTextWriterAttribute.Class, "row" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-md-6" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _tbActionTypeName.ValidationGroup = ValidationGroup;
            _tbActionTypeName.RenderControl( writer );
            _ddlEntityType.ValidationGroup = ValidationGroup;
            _ddlEntityType.RenderControl( writer );
            writer.RenderEndTag();

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-md-6" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _cbIsActionCompletedOnSuccess.ValidationGroup = ValidationGroup;
            _cbIsActionCompletedOnSuccess.RenderControl( writer );
            _cbIsActivityCompletedOnSuccess.ValidationGroup = ValidationGroup;
            _cbIsActivityCompletedOnSuccess.RenderControl( writer );
            writer.RenderEndTag();

            writer.RenderEndTag();

            _formEditor.ValidationGroup = ValidationGroup;
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

        /// <summary>
        /// Occurs when [change action type click].
        /// </summary>
        public event EventHandler ChangeActionTypeClick;

    }
}