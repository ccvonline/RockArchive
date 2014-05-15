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
    /// Workflow Action Form Editor
    /// </summary>
    [ToolboxData( "<{0}:WorkflowActionFormEditor runat=server></{0}:WorkflowActionFormEditor>" )]
    public class WorkflowFormEditor : CompositeControl, IHasValidationGroup
    {
        private HiddenField _hfFormGuid;
        private CodeEditor _ceHeaderText;
        private CodeEditor _ceFooterText;
        private RockTextBox _tbInactiveMessage;
        private RockControlWrapper _rcwActions;
        private WorkflowFormActionList _falActions;


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
        /// Gets or sets the form.
        /// </summary>
        /// <value>
        /// The form.
        /// </value>
        public WorkflowActionForm Form
        {
            get
            {
                EnsureChildControls();
                var form = new WorkflowActionForm();
                form.Guid = _hfFormGuid.Value.AsGuid();
                if ( form.Guid != Guid.Empty )
                {
                    form.Header = _ceHeaderText.Text;
                    form.Footer = _ceFooterText.Text;
                    form.Actions = _falActions.Value;
                    form.InactiveMessage = _tbInactiveMessage.Text;

                    foreach ( var row in AttributeRows )
                    {
                        var formAttribute = new WorkflowActionFormAttribute();
                        formAttribute.Attribute = new Rock.Model.Attribute { Guid = row.AttributeGuid, Name = row.AttributeName };
                        formAttribute.Guid = row.Guid;
                        formAttribute.Order = row.Order;
                        formAttribute.IsVisible = row.IsVisible;
                        formAttribute.IsReadOnly = !row.IsEditable;
                        formAttribute.IsRequired = row.IsRequired;
                        form.FormAttributes.Add( formAttribute );
                    }

                    return form;
                }
                return null;
            }

            set
            {
                EnsureChildControls();

                if ( value != null )
                {
                    _hfFormGuid.Value = value.Guid.ToString();
                    _ceHeaderText.Text = value.Header;
                    _ceFooterText.Text = value.Footer;
                    _falActions.Value = value.Actions;
                    _tbInactiveMessage.Text = value.InactiveMessage;

                    // Remove any existing rows (shouldn't be any)
                    foreach ( var attributeRow in Controls.OfType<WorkflowFormAttributeRow>() )
                    {
                        Controls.Remove( attributeRow );
                    }

                    foreach ( var formAttribute in value.FormAttributes.OrderBy( a => a.Order ) )
                    {
                        var row = new WorkflowFormAttributeRow();
                        row.AttributeGuid = formAttribute.Attribute.Guid;
                        row.AttributeName = formAttribute.Attribute.Name;
                        row.Guid = formAttribute.Guid;
                        row.IsVisible = formAttribute.IsVisible;
                        row.IsEditable = !formAttribute.IsReadOnly;
                        row.IsRequired = formAttribute.IsRequired;
                        Controls.Add( row );
                    }
                }
                else
                {
                    _hfFormGuid.Value = string.Empty;
                    _ceHeaderText.Text = string.Empty;
                    _ceFooterText.Text = string.Empty;
                    _falActions.Value = "Submit^Submit^Your information has been submitted succesfully.";
                    _tbInactiveMessage.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets the workflow activities.
        /// </summary>
        /// <value>
        /// The workflow activities.
        /// </value>
        public Dictionary<string, string> WorkflowActivities
        {
            get
            {
                EnsureChildControls();
                return _falActions.Activities;
            }

            set
            {
                EnsureChildControls();
                _falActions.Activities = value;
            }
        }

        /// <summary>
        /// Gets the attribute rows.
        /// </summary>
        /// <value>
        /// The attribute rows.
        /// </value>
        public List<WorkflowFormAttributeRow> AttributeRows
        {
            get
            {
                var rows = new List<WorkflowFormAttributeRow>();

                foreach ( Control control in Controls )
                {
                    if ( control is WorkflowFormAttributeRow )
                    {
                        var workflowFormAttributeRow = control as WorkflowFormAttributeRow;
                        if ( workflowFormAttributeRow != null )
                        {
                            rows.Add( workflowFormAttributeRow );
                        }
                    }
                }

                return rows.OrderBy( r => r.Order ).ToList();
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            _hfFormGuid = new HiddenField();
            _hfFormGuid.ID = this.ID + "_hfFormGuid";
            Controls.Add( _hfFormGuid );

            _ceHeaderText = new CodeEditor();
            _ceHeaderText.Label = "Form Header";
            _ceHeaderText.Help = "Text to display to user above the form fields.";
            _ceHeaderText.ID = this.ID + "_tbHeaderText";
            _ceHeaderText.EditorMode = CodeEditorMode.Html;
            _ceHeaderText.EditorTheme = CodeEditorTheme.Rock;
            _ceHeaderText.EditorHeight = "100";
            Controls.Add( _ceHeaderText );

            _ceFooterText = new CodeEditor();
            _ceFooterText.Label = "Form Footer";
            _ceFooterText.Help = "Text to display to user below the form fields.";
            _ceFooterText.ID = this.ID + "_tbFooterText";
            _ceFooterText.EditorMode = CodeEditorMode.Html;
            _ceFooterText.EditorTheme = CodeEditorTheme.Rock;
            _ceFooterText.EditorHeight = "100";
            Controls.Add( _ceFooterText );

            _rcwActions = new RockControlWrapper();
            _rcwActions.Label = "Action Buttons";
            _rcwActions.Help = "The Action button text and the action value to save when user clicks the action.";
            _rcwActions.ID = this.ID + "_rcwActions";
            Controls.Add( _rcwActions );

            _falActions = new WorkflowFormActionList();
            _falActions.ID = this.ID + "_falActions";
            _rcwActions.Controls.Add( _falActions );

            _tbInactiveMessage = new RockTextBox();
            _tbInactiveMessage.Label = "Inactive Message";
            _tbInactiveMessage.Help = "Text to display to user when attempting to view entry form when action or activity is not active.";
            _tbInactiveMessage.ID = this.ID + "_tbInactiveMessage";
            _tbInactiveMessage.TextMode = TextBoxMode.MultiLine;
            _tbInactiveMessage.Rows = 2;
            Controls.Add( _tbInactiveMessage );
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( _hfFormGuid.Value.AsGuid() != Guid.Empty )
            {
                _ceHeaderText.ValidationGroup = ValidationGroup;
                _ceHeaderText.RenderControl( writer );

                // Attributes
                if ( AttributeRows.Any() )
                {
                    writer.AddAttribute( HtmlTextWriterAttribute.Class, "form-group" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );

                    writer.AddAttribute( HtmlTextWriterAttribute.Class, "control-label" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Label );
                    writer.Write( "Form Fields" );

                    writer.AddAttribute( "class", "help" );
                    writer.AddAttribute( "href", "#" );
                    writer.RenderBeginTag( HtmlTextWriterTag.A );
                    writer.AddAttribute( "class", "fa fa-question-circle" );
                    writer.RenderBeginTag( HtmlTextWriterTag.I );
                    writer.RenderEndTag();
                    writer.RenderEndTag();

                    writer.AddAttribute( "class", "alert alert-info" );
                    writer.AddAttribute( "style", "display:none" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    writer.RenderBeginTag( HtmlTextWriterTag.Small );
                    writer.Write( "The fields (attributes) to display on the entry form" );
                    writer.RenderEndTag();
                    writer.RenderEndTag();

                    writer.RenderEndTag();      // Label

                    writer.AddAttribute( HtmlTextWriterAttribute.Class, "grid-table table table-condensed table-light" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Table );

                    writer.RenderBeginTag( HtmlTextWriterTag.Thead );
                    writer.RenderBeginTag( HtmlTextWriterTag.Tr );

                    writer.AddAttribute( HtmlTextWriterAttribute.Class, "grid-columncommand" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Th );
                    writer.Write( "&nbsp;" );
                    writer.RenderEndTag();

                    writer.AddAttribute( HtmlTextWriterAttribute.Scope, "col" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Th );
                    writer.Write( "Field" );
                    writer.RenderEndTag();

                    writer.AddAttribute( HtmlTextWriterAttribute.Scope, "col" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Th );
                    writer.Write( "Visible" );
                    writer.RenderEndTag();

                    writer.AddAttribute( HtmlTextWriterAttribute.Scope, "col" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Th );
                    writer.Write( "Editable" );
                    writer.RenderEndTag();

                    writer.AddAttribute( HtmlTextWriterAttribute.Scope, "col" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Th );
                    writer.Write( "Required" );
                    writer.RenderEndTag();

                    writer.RenderEndTag();  // tr
                    writer.RenderEndTag();  // thead

                    writer.AddAttribute( HtmlTextWriterAttribute.Class, "workflow-formfield-list" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Tbody );

                    foreach ( var row in AttributeRows )
                    {
                        row.RenderControl( writer );
                    }

                    writer.RenderEndTag();  // tbody

                    writer.RenderEndTag();  // table

                    writer.RenderEndTag();  // Div.form-group
                }

                _ceFooterText.ValidationGroup = ValidationGroup;
                _ceFooterText.RenderControl( writer );
                _rcwActions.ValidationGroup = ValidationGroup;
                _rcwActions.RenderControl( writer );

                // Don't render (not used)
                //_tbInactiveMessage.ValidationGroup = ValidationGroup;
                //_tbInactiveMessage.RenderControl( writer );
            }
        }

        /// <summary>
        /// Clears the rows.
        /// </summary>
        public void ClearRows()
        {
            for ( int i = Controls.Count - 1; i >= 0; i-- )
            {
                if ( Controls[i] is WorkflowFormAttributeRow )
                {
                    Controls.RemoveAt( i );
                }
            }
        }
    }
}