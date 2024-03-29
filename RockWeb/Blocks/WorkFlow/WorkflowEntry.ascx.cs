﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using Rock.Security;
using Rock.Workflow;

namespace RockWeb.Blocks.WorkFlow
{
    /// <summary>
    /// Used to enter information for a workflow form entry action.
    /// </summary>
    [DisplayName( "Workflow Entry" )]
    [Category( "WorkFlow" )]
    [Description( "Used to enter information for a workflow form entry action." )]

    [WorkflowTypeField( "Workflow Type", "Type of workflow to start." )]
    [BooleanField( "Show Summary View", "If workflow has been completed, should the summary view be displayed?", false, "", 1)]
    public partial class WorkflowEntry : Rock.Web.UI.RockBlock, IPostBackEventHandler
    {
        #region Fields

        private RockContext _rockContext = null;
        private WorkflowService _workflowService = null;

        private WorkflowTypeCache _workflowType = null;
        private WorkflowActionTypeCache _actionType = null;
        private Workflow _workflow = null;
        private WorkflowActivity _activity = null;
        private WorkflowAction _action = null;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the workflow type identifier.
        /// </summary>
        /// <value>
        /// The workflow type identifier.
        /// </value>
        public int? WorkflowTypeId
        {
            get { return ViewState["WorkflowTypeId"] as int?; }
            set { ViewState["WorkflowTypeId"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the workflow type was set by attribute.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [configured type]; otherwise, <c>false</c>.
        /// </value>
        public bool ConfiguredType
        {
            get { return ViewState["ConfiguredType"] as bool? ?? false; }
            set { ViewState["ConfiguredType"] = value; }
        }

        /// <summary>
        /// Gets or sets the workflow identifier.
        /// </summary>
        /// <value>
        /// The workflow identifier.
        /// </value>
        public int? WorkflowId
        {
            get { return ViewState["WorkflowId"] as int?; }
            set { ViewState["WorkflowId"] = value; }
        }

        /// <summary>
        /// Gets or sets the action type identifier.
        /// </summary>
        /// <value>
        /// The action type identifier.
        /// </value>
        public int? ActionTypeId
        {
            get { return ViewState["ActionTypeId"] as int?; }
            set { ViewState["ActionTypeId"] = value; }
        }

        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Restores the view-state information from a previous user control request that was saved by the <see cref="M:System.Web.UI.UserControl.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object" /> that represents the user control state to be restored.</param>
        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );

            if ( HydrateObjects() )
            {
                BuildForm( false );
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );

            if ( _workflowType != null && !ConfiguredType )
            {
                RockPage.PageTitle = _workflowType.Name;

                // we can only override if the page does not have a icon
                if ( string.IsNullOrWhiteSpace( RockPage.PageIcon ) && !string.IsNullOrWhiteSpace( _workflowType.IconCssClass ) )
                {
                    RockPage.PageIcon = _workflowType.IconCssClass;
                }
            }
            if ( _workflowType != null )
            {
                lTitle.Text = string.Format( "{0} Entry", _workflowType.WorkTerm );

                if ( ! string.IsNullOrWhiteSpace( _workflowType.IconCssClass ) )
                {
                    lIconHtml.Text = string.Format( "<i class='{0}' ></i>", _workflowType.IconCssClass );
                }
            }
        }


        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            nbMessage.Visible = false;

            if ( !Page.IsPostBack )
            {
                if ( HydrateObjects() )
                {
                    BuildForm( true );
                    ProcessActionRequest();
                }
            }
        }

        /// <summary>
        /// Returns breadcrumbs specific to the block that should be added to navigation
        /// based on the current page reference.  This function is called during the page's
        /// oninit to load any initial breadcrumbs.
        /// </summary>
        /// <param name="pageReference">The <see cref="Rock.Web.PageReference" />.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{BreadCrumb}" /> of block related <see cref="Rock.Web.UI.BreadCrumb">BreadCrumbs</see>.
        /// </returns>
        public override List<BreadCrumb> GetBreadCrumbs( Rock.Web.PageReference pageReference )
        {
            var breadCrumbs = new List<BreadCrumb>();

            LoadWorkflowType();

            if ( _workflowType != null && !ConfiguredType )
            {
                breadCrumbs.Add( new BreadCrumb( _workflowType.Name, pageReference ) );
            }

            return breadCrumbs;
        }

        protected override void Render( HtmlTextWriter writer )
        {
            base.Render( writer );
        }
        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        /// <summary>
        /// When implemented by a class, enables a server control to process an event raised when a form is posted to the server.
        /// </summary>
        /// <param name="eventArgument">A <see cref="T:System.String" /> that represents an optional event argument to be passed to the event handler.</param>
        public void RaisePostBackEvent( string eventArgument )
        {
            GetFormValues();
            CompleteFormAction( eventArgument );
        }

        #endregion

        #region Methods

        private bool HydrateObjects()
        {
            LoadWorkflowType();

            // Set the note type if this is first request
            if ( !Page.IsPostBack )
            {
                var entityType = EntityTypeCache.Read( typeof( Rock.Model.Workflow ) );
                var noteTypes = NoteTypeCache.GetByEntity( entityType.Id, string.Empty, string.Empty );
                ncWorkflowNotes.NoteTypes = noteTypes;
            }

            if ( _workflowType == null )
            {
                ShowNotes( false );
                ShowMessage( NotificationBoxType.Danger, "Configuration Error", "Workflow type was not configured or specified correctly." );
                return false;
            }

            if ( !_workflowType.IsAuthorized( Authorization.VIEW, CurrentPerson ) )
            {
                ShowNotes( false );
                ShowMessage( NotificationBoxType.Warning, "Sorry", "You are not authorized to view this type of workflow." );
                return false;
            }

            if ( !(_workflowType.IsActive ?? true) )
            {
                ShowNotes( false );
                ShowMessage( NotificationBoxType.Warning, "Sorry", "This type of workflow is not active." );
                return false;
            }

            // If operating against an existing workflow, get the workflow and load attributes
            if ( !WorkflowId.HasValue )
            {
                WorkflowId = PageParameter( "WorkflowId" ).AsIntegerOrNull();
                if ( !WorkflowId.HasValue )
                {
                    Guid guid = PageParameter( "WorkflowGuid" ).AsGuid();
                    if ( !guid.IsEmpty() )
                    {
                        _workflow = _workflowService.Queryable()
                            .Where( w => w.Guid.Equals( guid ) && w.WorkflowTypeId == _workflowType.Id )
                            .FirstOrDefault();
                        if ( _workflow != null )
                        {
                            WorkflowId = _workflow.Id;
                        }
                    }
                }
            }

            if ( WorkflowId.HasValue )
            {
                if ( _workflow == null )
                {
                    _workflow = _workflowService.Queryable()
                        .Where( w => w.Id == WorkflowId.Value && w.WorkflowTypeId == _workflowType.Id )
                        .FirstOrDefault();
                }
                if ( _workflow != null )
                {
                    hlblWorkflowId.Text = _workflow.WorkflowId;

                    _workflow.LoadAttributes();
                    foreach ( var activity in _workflow.Activities )
                    {
                        activity.LoadAttributes();
                    }
                }

            }

            // If an existing workflow was not specified, activate a new instance of workflow and start processing
            if ( _workflow == null )
            {
                string workflowName = PageParameter( "WorkflowName" );
                if ( string.IsNullOrWhiteSpace(workflowName))
                {
                    workflowName = "New " + _workflowType.WorkTerm;
                }

                _workflow = Rock.Model.Workflow.Activate( _workflowType, workflowName);
                if ( _workflow != null )
                {
                    // If a PersonId or GroupId parameter was included, load the corresponding
                    // object and pass that to the actions for processing
                    object entity = null;
                    int? personId = PageParameter( "PersonId" ).AsIntegerOrNull();
                    if ( personId.HasValue )
                    {
                        entity = new PersonService( _rockContext ).Get( personId.Value );
                    }
                    else
                    {
                        int? groupId = PageParameter( "GroupId" ).AsIntegerOrNull();
                        if ( groupId.HasValue )
                        {
                            entity = new GroupService( _rockContext ).Get( groupId.Value );
                        }
                    }

                    // Loop through all the query string parameters and try to set any workflow
                    // attributes that might have the same key
                    foreach ( var param in RockPage.PageParameters() )
                    {
                        if ( param.Value != null && param.Value.ToString().IsNotNullOrWhitespace() )
                        {
                            _workflow.SetAttributeValue( param.Key, param.Value.ToString() );
                        }
                    }

                    List<string> errorMessages;
                    if ( !_workflowService.Process( _workflow, entity, out errorMessages ) )
                    {
                        ShowNotes( false );
                        ShowMessage( NotificationBoxType.Danger, "Workflow Processing Error(s):",
                            "<ul><li>" + errorMessages.AsDelimited( "</li><li>" ) + "</li></ul>" );
                        return false;
                    }
                    if ( _workflow.Id != 0 )
                    {
                        WorkflowId = _workflow.Id;
                    }
                }
            }

            if ( _workflow == null )
            {
                ShowNotes( false );
                ShowMessage( NotificationBoxType.Danger, "Workflow Activation Error", "Workflow could not be activated." );
                return false;
            }

            var canEdit = UserCanEdit || _workflow.IsAuthorized( Authorization.EDIT, CurrentPerson );

            if ( _workflow.IsActive )
            {
                if ( ActionTypeId.HasValue )
                {
                    foreach ( var activity in _workflow.ActiveActivities )
                    {
                        _action = activity.ActiveActions.Where( a => a.ActionTypeId == ActionTypeId.Value ).FirstOrDefault();
                        if ( _action != null )
                        {
                            _activity = activity;
                            _activity.LoadAttributes();

                            _actionType = _action.ActionTypeCache;
                            ActionTypeId = _actionType.Id;
                            return true; 
                        }
                    }
                }

                // Find first active action form
                int personId = CurrentPerson != null ? CurrentPerson.Id : 0;
                int? actionId = PageParameter( "ActionId" ).AsIntegerOrNull();
                foreach ( var activity in _workflow.Activities
                    .Where( a =>
                        a.IsActive &&
                        ( !actionId.HasValue || a.Actions.Any( ac => ac.Id == actionId.Value ) ) &&
                        (
                            ( canEdit ) ||
                            ( !a.AssignedGroupId.HasValue && !a.AssignedPersonAliasId.HasValue ) ||
                            ( a.AssignedPersonAlias != null && a.AssignedPersonAlias.PersonId == personId ) ||
                            ( a.AssignedGroup != null && a.AssignedGroup.Members.Any( m => m.PersonId == personId ) )
                        )
                    )
                    .ToList()
                    .OrderBy( a => a.ActivityTypeCache.Order ) )
                {
                    if ( canEdit || ( activity.ActivityTypeCache.IsAuthorized( Authorization.VIEW, CurrentPerson ) ) )
                    {
                        foreach ( var action in activity.ActiveActions
                            .Where( a => ( !actionId.HasValue || a.Id == actionId.Value ) ) )
                        {
                            if ( action.ActionTypeCache.WorkflowForm != null && action.IsCriteriaValid )
                            {
                                _activity = activity;
                                _activity.LoadAttributes();

                                _action = action;
                                _actionType = _action.ActionTypeCache;
                                ActionTypeId = _actionType.Id;
                                return true;
                            }
                        }
                    }
                }

                lSummary.Text = string.Empty;

            }
            else
            {
                if ( GetAttributeValue("ShowSummaryView").AsBoolean() && !string.IsNullOrWhiteSpace( _workflowType.SummaryViewText ) )
                {
                    var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( this.RockPage, this.CurrentPerson );
                    mergeFields.Add( "Action", _action );
                    mergeFields.Add( "Activity", _activity );
                    mergeFields.Add( "Workflow", _workflow );

                    lSummary.Text = _workflowType.SummaryViewText.ResolveMergeFields( mergeFields, CurrentPerson );
                    lSummary.Visible = true;
                }
            }

            if ( lSummary.Text.IsNullOrWhiteSpace() )
            {
                if ( _workflowType.NoActionMessage.IsNullOrWhiteSpace() )
                {
                    ShowMessage( NotificationBoxType.Warning, string.Empty, "The selected workflow is not in a state that requires you to enter information." );
                }
                else
                {
                    var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( this.RockPage, this.CurrentPerson );
                    mergeFields.Add( "Action", _action );
                    mergeFields.Add( "Activity", _activity );
                    mergeFields.Add( "Workflow", _workflow );
                    ShowMessage( NotificationBoxType.Warning, string.Empty, _workflowType.NoActionMessage.ResolveMergeFields( mergeFields, CurrentPerson ) );
                }
            }

            ShowNotes( false );
            return false;
        }

        private void LoadWorkflowType()
        {
            if ( _rockContext == null )
            {
                _rockContext = new RockContext();
            }

            if ( _workflowService == null )
            {
                _workflowService = new WorkflowService( _rockContext );
            }

            // Get the workflow type id (initial page request)
            if ( !WorkflowTypeId.HasValue )
            {
                // Get workflow type set by attribute value
                Guid workflowTypeguid = GetAttributeValue( "WorkflowType" ).AsGuid();
                if ( !workflowTypeguid.IsEmpty() )
                {
                    _workflowType = WorkflowTypeCache.Read( workflowTypeguid );
                }

                // If an attribute value was not provided, check for query/route value
                if ( _workflowType != null )
                {
                    WorkflowTypeId = _workflowType.Id;
                    ConfiguredType = true;
                }
                else
                {
                    WorkflowTypeId = PageParameter( "WorkflowTypeId" ).AsIntegerOrNull();
                    ConfiguredType = false;
                }
            }

            // Get the workflow type 
            if ( _workflowType == null && WorkflowTypeId.HasValue )
            {
                _workflowType = WorkflowTypeCache.Read( WorkflowTypeId.Value );
            }
        }

        private void ProcessActionRequest()
        {
            string action = PageParameter( "Command" );
            if ( !string.IsNullOrWhiteSpace( action ) )
            {
                CompleteFormAction( action );
            }
        }

        private void BuildForm( bool setValues )
        {
            var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( this.RockPage, this.CurrentPerson );
            mergeFields.Add( "Action", _action );
            mergeFields.Add( "Activity", _activity );
            mergeFields.Add( "Workflow", _workflow );

            var form = _actionType.WorkflowForm;

            if ( setValues )
            {
                lheadingText.Text = form.Header.ResolveMergeFields( mergeFields );
                lFootingText.Text = form.Footer.ResolveMergeFields( mergeFields );
            }

            if ( _workflow != null && _workflow.CreatedDateTime.HasValue )
            {
                hlblDateAdded.Text = String.Format( "Added: {0}", _workflow.CreatedDateTime.Value.ToShortDateString() );
            }
            else
            {
                hlblDateAdded.Visible = false;
            }

            phAttributes.Controls.Clear();

            foreach ( var formAttribute in form.FormAttributes.OrderBy( a => a.Order ) )
            {
                if ( formAttribute.IsVisible )
                {
                    var attribute = AttributeCache.Read( formAttribute.AttributeId );

                    string value = attribute.DefaultValue;
                    if ( _workflow != null && _workflow.AttributeValues.ContainsKey( attribute.Key ) && _workflow.AttributeValues[attribute.Key] != null )
                    {
                        value = _workflow.AttributeValues[attribute.Key].Value;
                    }
                    // Now see if the key is in the activity attributes so we can get it's value
                    else if ( _activity != null && _activity.AttributeValues.ContainsKey( attribute.Key ) && _activity.AttributeValues[attribute.Key] != null )
                    {
                        value = _activity.AttributeValues[attribute.Key].Value;
                    }

                    if ( !string.IsNullOrWhiteSpace( formAttribute.PreHtml))
                    {
                        phAttributes.Controls.Add( new LiteralControl( formAttribute.PreHtml.ResolveMergeFields(mergeFields) ) );
                    }

                    if ( formAttribute.IsReadOnly )
                    {
                        var field = attribute.FieldType.Field;

                        string formattedValue = null;

                        // get formatted value 
                        if ( attribute.FieldType.Class == typeof( Rock.Field.Types.ImageFieldType ).FullName )
                        {
                            formattedValue = attribute.FieldType.Field.FormatValueAsHtml( phAttributes, attribute.EntityTypeId, _activity.Id, value, attribute.QualifierValues, true );
                        }
                        else
                        {
                            formattedValue = field.FormatValueAsHtml( phAttributes, attribute.EntityTypeId, _activity.Id, value, attribute.QualifierValues );
                        }

                        if ( formAttribute.HideLabel )
                        {
                            phAttributes.Controls.Add( new LiteralControl( formattedValue ) );
                        }
                        else
                        {
                            RockLiteral lAttribute = new RockLiteral();
                            lAttribute.ID = "lAttribute_" + formAttribute.Id.ToString();
                            lAttribute.Label = attribute.Name;

                            if ( field is Rock.Field.ILinkableFieldType )
                            {
                                string url = ( (Rock.Field.ILinkableFieldType)field ).UrlLink( value, attribute.QualifierValues );
                                url = ResolveRockUrl( "~" ).EnsureTrailingForwardslash() + url;
                                lAttribute.Text = string.Format( "<a href='{0}' target='_blank'>{1}</a>", url, formattedValue );
                            }
                            else
                            {
                                lAttribute.Text = formattedValue;
                            }

                            phAttributes.Controls.Add( lAttribute );
                        }
                    }
                    else
                    {
                        attribute.AddControl( phAttributes.Controls, value, BlockValidationGroup, setValues, true, formAttribute.IsRequired, 
                            ( formAttribute.HideLabel ? string.Empty : attribute.Name ) );
                    }

                    if ( !string.IsNullOrWhiteSpace( formAttribute.PostHtml ) )
                    {
                        phAttributes.Controls.Add( new LiteralControl( formAttribute.PostHtml.ResolveMergeFields( mergeFields) ) );
                    }

                }
            }

            if ( form.AllowNotes.HasValue && form.AllowNotes.Value && _workflow != null && _workflow.Id != 0 )
            {
                ncWorkflowNotes.EntityId = _workflow.Id;
                ncWorkflowNotes.RebuildNotes( setValues );
                ShowNotes( true );
            }
            else
            {
                ShowNotes( false );
            }

            phActions.Controls.Clear();
            foreach ( var action in form.Actions.Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries ) )
            {
                var details = action.Split( new char[] { '^' } );
                if ( details.Length > 0 )
                {
                    // Get the button html
                    string buttonHtml = string.Empty;
                    if ( details.Length > 1 )
                    {
                        var definedValue = DefinedValueCache.Read( details[1].AsGuid() );
                        if ( definedValue != null )
                        {
                            buttonHtml = definedValue.GetAttributeValue( "ButtonHTML" );
                        }
                    }

                    if ( string.IsNullOrWhiteSpace( buttonHtml ) )
                    {
                        buttonHtml = "<a href=\"{{ ButtonLink }}\" onclick=\"{{ ButtonClick }}\" class='btn btn-primary' data-loading-text='<i class=\"fa fa-refresh fa-spin\"></i> {{ ButtonText }}'>{{ ButtonText }}</a>";
                    }

                    var buttonMergeFields = new Dictionary<string, object>();
                    buttonMergeFields.Add( "ButtonText", details[0].EscapeQuotes() );
                    buttonMergeFields.Add( "ButtonClick",
                            string.Format( "if ( Page_ClientValidate('{0}') ) {{ $(this).button('loading'); return true; }} else {{ return false; }}",
                            BlockValidationGroup ) );
                    buttonMergeFields.Add( "ButtonLink", Page.ClientScript.GetPostBackClientHyperlink( this, details[0] ) );

                    buttonHtml = buttonHtml.ResolveMergeFields( buttonMergeFields );

                    phActions.Controls.Add( new LiteralControl( buttonHtml ) );
                    phActions.Controls.Add( new LiteralControl( " " ) );
                }
            }

        }

        private void ShowNotes(bool visible)
        {
            divNotes.Visible = visible;

            if ( visible )
            {
                divForm.RemoveCssClass( "col-md-12" );
                divForm.AddCssClass( "col-md-6" );
            }
            else
            {
                divForm.AddCssClass( "col-md-12" );
                divForm.RemoveCssClass( "col-md-6" );
            }
        }

        private void GetFormValues()
        {
            if ( _workflow != null && _actionType != null )
            {
                var form = _actionType.WorkflowForm;

                var values = new Dictionary<int, string>();
                foreach ( var formAttribute in form.FormAttributes.OrderBy( a => a.Order ) )
                {
                    if ( formAttribute.IsVisible && !formAttribute.IsReadOnly )
                    {
                        var attribute = AttributeCache.Read( formAttribute.AttributeId );
                        var control = phAttributes.FindControl( string.Format( "attribute_field_{0}", formAttribute.AttributeId ) );

                        if ( attribute != null && control != null)
                        {
                            IHasAttributes item = null;
                            if ( attribute.EntityTypeId == _workflow.TypeId )
                            {
                                item = _workflow;
                            }
                            else if ( attribute.EntityTypeId == _activity.TypeId )
                            {
                                item = _activity;
                            }

                            if ( item != null )
                            {
                                item.SetAttributeValue( attribute.Key, attribute.FieldType.Field.GetEditValue( attribute.GetControl( control ), attribute.QualifierValues ) );
                            }
                        }
                    }
                }
            }
        }

        private void CompleteFormAction( string formAction )
        { 
            if ( !string.IsNullOrWhiteSpace( formAction ) &&
                _workflow != null &&
                _actionType != null &&
                _actionType.WorkflowForm != null &&
                _activity != null &&
                _action != null )
            {
                var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( this.RockPage, this.CurrentPerson );
                mergeFields.Add( "Action", _action );
                mergeFields.Add( "Activity", _activity );
                mergeFields.Add( "Workflow", _workflow );
                
                Guid activityTypeGuid = Guid.Empty;
                string responseText = "Your information has been submitted successfully.";

                foreach ( var action in _actionType.WorkflowForm.Actions.Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries ) )
                {
                    var actionDetails = action.Split( new char[] { '^' } );
                    if ( actionDetails.Length > 0 && actionDetails[0] == formAction )
                    {
                        if ( actionDetails.Length > 2 )
                        {
                            activityTypeGuid = actionDetails[2].AsGuid();
                        }

                        if ( actionDetails.Length > 3 && !string.IsNullOrWhiteSpace( actionDetails[3] ) )
                        {
                            responseText = actionDetails[3].ResolveMergeFields( mergeFields );
                        }
                        break;
                    }
                }

                _action.MarkComplete();
                _action.FormAction = formAction;
                _action.AddLogEntry( "Form Action Selected: " + _action.FormAction );

                if (_action.ActionTypeCache.IsActivityCompletedOnSuccess)
                {
                    _action.Activity.MarkComplete();
                }

                if ( _actionType.WorkflowForm.ActionAttributeGuid.HasValue )
                {
                    var attribute = AttributeCache.Read( _actionType.WorkflowForm.ActionAttributeGuid.Value );
                    if ( attribute != null )
                    {
                        IHasAttributes item = null;
                        if ( attribute.EntityTypeId == _workflow.TypeId )
                        {
                            item = _workflow;
                        }
                        else if ( attribute.EntityTypeId == _activity.TypeId )
                        {
                            item = _activity;
                        }

                        if ( item != null )
                        {
                            item.SetAttributeValue( attribute.Key, formAction );
                        }
                    }
                }

                if ( !activityTypeGuid.IsEmpty() )
                {
                    var activityType = _workflowType.ActivityTypes.Where( a => a.Guid.Equals( activityTypeGuid ) ).FirstOrDefault();
                    if ( activityType != null )
                    {
                        WorkflowActivity.Activate( activityType, _workflow );
                    }
                }

                /*
                 * Temporary Persistence Fix
                 * --------------------------
                 * There is a bug that occurs when a newly launched workflow (without auto persist) processes too fast.
                 * This would cause the workflow to sometimes not persist.  We have traced it down to line 594 of Workflow.cs:
                 *
                 *    LINE 594: activity.LastProcessedDateTime.Value.CompareTo( processStartTime ) < 0 )
                 * 
                 * In some cases, the activity's process start time would be equal to the last processed time.  This would cause the
                 * activity to be skipped and not persisted.
                 * 
                 * Adding the Sleep() step below fixes the issue since it is no longer possible for timestamps to be equal.  I tested
                 * against core Rock v9 and 10 in the same server environment but was unable to replicate this issue so it could be 
                 * possible this was fixed somewhere else in code.  Once we upgrade to a newer version of Rock, this can be removed
                 * and tested again.
                 */
                System.Threading.Thread.Sleep( 1 );

                List<string> errorMessages;
                if ( _workflowService.Process( _workflow, out errorMessages ) )
                {
                    Guid? previousActionGuid = null;
                    
                    if ( _action != null )
                    {
                        // Compare GUIDs since the IDs are DB generated and will be 0 if the workflow is not persisted.
                        previousActionGuid = _action.Guid;
                    }

                    ActionTypeId = null;
                    _action = null;
                    _actionType = null;
                    _activity = null;
                    bool hydrateObjectsResult = HydrateObjects();

                    if ( hydrateObjectsResult && _action != null && _action.Guid != previousActionGuid )
                    {
                        // The block reloads the page with the workflow IDs as a parameter. At this point the workflow must be persisted regardless of user settings in order for the workflow to work.
                        _workflowService.PersistImmediately( _action );

                        // If we are already being directed (presumably from the Redirect Action), don't redirect again.
                        if (!Response.IsRequestBeingRedirected)
                        {
                            var cb = CurrentPageReference;
                            cb.Parameters.AddOrReplace( "WorkflowId", _workflow.Id.ToString() );
                            foreach ( var key in cb.QueryString.AllKeys.Where( k => !k.Equals( "Command", StringComparison.OrdinalIgnoreCase ) ) )
                            {
                                cb.Parameters.AddOrIgnore( key, cb.QueryString[key] );
                            }
                            cb.QueryString = new System.Collections.Specialized.NameValueCollection();
                            Response.Redirect( cb.BuildUrl(), false );
                            Context.ApplicationInstance.CompleteRequest();
                        }
                    }
                    else
                    {
                        if ( lSummary.Text.IsNullOrWhiteSpace() )
                        {
                            ShowMessage( NotificationBoxType.Success, string.Empty, responseText, ( _action == null || _action.Guid != previousActionGuid ) );
                        }
                        else
                        {
                            pnlForm.Visible = false;
                        }
                    }
                }
                else
                {
                    ShowMessage( NotificationBoxType.Danger, "Workflow Processing Error(s):", 
                        "<ul><li>" + errorMessages.AsDelimited( "</li><li>", null, true ) + "</li></ul>" );
                }
                if ( _workflow.Id != 0 )
                {
                    WorkflowId = _workflow.Id;
                }
            }
        }

        private void ShowMessage( NotificationBoxType type, string title, string message, bool hideForm = true )
        {
            nbMessage.NotificationBoxType = type;
            nbMessage.Title = title;
            nbMessage.Text = message;
            nbMessage.Visible = true;
            nbMessage.Dismissable = false;

            if ( hideForm )
            {
                pnlForm.Visible = false;
            }

        }

        #endregion

    }

}
