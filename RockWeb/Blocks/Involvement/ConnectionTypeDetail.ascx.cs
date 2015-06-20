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
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Newtonsoft.Json;

using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using Attribute = Rock.Model.Attribute;

namespace RockWeb.Blocks.Involvement
{
    [DisplayName( "Connection Type Detail" )]
    [Category( "Involvement" )]
    [Description( "Displays the details of the given Connection Type for editing." )]
    public partial class ConnectionTypeDetail : RockBlock, IDetailBlock
    {
        #region Properties

        private List<Attribute> AttributesState { get; set; }

        private List<ConnectionActivityType> ActivityTypesState { get; set; }

        private List<ConnectionStatus> StatusesState { get; set; }

        private ViewStateList<ConnectionWorkflow> WorkflowsState { get; set; }

        #endregion

        #region Control Methods

        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );

            string json = ViewState["AttributesState"] as string;
            if ( string.IsNullOrWhiteSpace( json ) )
            {
                AttributesState = new List<Attribute>();
            }
            else
            {
                AttributesState = JsonConvert.DeserializeObject<List<Attribute>>( json );
            }

            json = ViewState["ActivityTypesState"] as string;
            if ( string.IsNullOrWhiteSpace( json ) )
            {
                ActivityTypesState = new List<ConnectionActivityType>();
            }
            else
            {
                ActivityTypesState = JsonConvert.DeserializeObject<List<ConnectionActivityType>>( json );
            }

            json = ViewState["StatusesState"] as string;
            if ( string.IsNullOrWhiteSpace( json ) )
            {
                StatusesState = new List<ConnectionStatus>();
            }
            else
            {
                StatusesState = JsonConvert.DeserializeObject<List<ConnectionStatus>>( json );
            }

            json = ViewState["WorkflowsState"] as string;
            if ( string.IsNullOrWhiteSpace( json ) )
            {
                AttributesState = new List<Attribute>();
            }
            else
            {
                AttributesState = JsonConvert.DeserializeObject<List<Attribute>>( json );
            }
        }


        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            bool editAllowed = IsUserAuthorized( Authorization.ADMINISTRATE );

            gAttributes.DataKeyNames = new string[] { "Guid" };
            gAttributes.Actions.ShowAdd = editAllowed;
            gAttributes.Actions.AddClick += gAttributes_Add;
            gAttributes.EmptyDataText = Server.HtmlEncode( None.Text );
            gAttributes.GridRebind += gAttributes_GridRebind;
            gAttributes.GridReorder += gAttributes_GridReorder;

            gActivityTypes.DataKeyNames = new string[] { "Guid" };
            gActivityTypes.Actions.ShowAdd = true;
            gActivityTypes.Actions.AddClick += gActivityTypes_Add;
            gActivityTypes.GridRebind += gActivityTypes_GridRebind;

            gStatuses.DataKeyNames = new string[] { "Guid" };
            gStatuses.Actions.ShowAdd = true;
            gStatuses.Actions.AddClick += gStatuses_Add;
            gStatuses.GridRebind += gStatuses_GridRebind;

            gWorkflows.DataKeyNames = new string[] { "Guid" };
            gWorkflows.Actions.ShowAdd = true;
            gWorkflows.Actions.AddClick += gWorkflows_Add;
            gWorkflows.GridRebind += gWorkflows_GridRebind;

            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upConnectionType );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                ShowDetail( PageParameter( "ConnectionTypeId" ).AsInteger() );
            }
            else
            {
                ShowDialog();
            }
        }

        /// <summary>
        /// Saves any user control view-state changes that have occurred since the last page postback.
        /// </summary>
        /// <returns>
        /// Returns the user control's current view state. If there is no view state associated with the control, it returns null.
        /// </returns>
        protected override object SaveViewState()
        {
            var jsonSetting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new Rock.Utility.IgnoreUrlEncodedKeyContractResolver()
            };

            ViewState["AttributesState"] = JsonConvert.SerializeObject( AttributesState, Formatting.None, jsonSetting );
            ViewState["ActivityTypesState"] = JsonConvert.SerializeObject( ActivityTypesState, Formatting.None, jsonSetting );
            ViewState["StatusesState"] = JsonConvert.SerializeObject( StatusesState, Formatting.None, jsonSetting );
            ViewState["WorkflowsState"] = JsonConvert.SerializeObject( WorkflowsState, Formatting.None, jsonSetting );

            return base.SaveViewState();
        }

        /// <summary>
        /// Returns breadcrumbs specific to the block that should be added to navigation
        /// based on the current page reference.  This function is called during the page's
        /// oninit to load any initial breadcrumbs
        /// </summary>
        /// <param name="pageReference">The page reference.</param>
        /// <returns></returns>
        public override List<BreadCrumb> GetBreadCrumbs( PageReference pageReference )
        {
            var breadCrumbs = new List<BreadCrumb>();

            int? connectionTypeId = PageParameter( pageReference, "ConnectionTypeId" ).AsIntegerOrNull();
            if ( connectionTypeId != null )
            {
                ConnectionType connectionType = new ConnectionTypeService( new RockContext() ).Get( connectionTypeId.Value );
                if ( connectionType != null )
                {
                    breadCrumbs.Add( new BreadCrumb( connectionType.Name, pageReference ) );
                }
                else
                {
                    breadCrumbs.Add( new BreadCrumb( "New Connection Type", pageReference ) );
                }
            }
            else
            {
                // don't show a breadcrumb if we don't have a pageparam to work with
            }

            return breadCrumbs;
        }

        #endregion

        #region Events

        #region Control Events

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnEdit_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            var connectionType = new EventCalendarService( rockContext ).Get( hfConnectionTypeId.Value.AsInteger() );

            LoadStateDetails( connectionType, rockContext );
            ShowEditDetails( connectionType );
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnDeleteConfirm_Click( object sender, EventArgs e )
        {
            using ( var rockContext = new RockContext() )
            {

                ConnectionWorkflowService connectionWorkflowService = new ConnectionWorkflowService( rockContext );
                ConnectionTypeService connectionTypeService = new ConnectionTypeService( rockContext );
                AuthService authService = new AuthService( rockContext );
                ConnectionType connectionType = connectionTypeService.Get( int.Parse( hfConnectionTypeId.Value ) );

                if ( connectionType != null )
                {
                    if ( !connectionType.IsAuthorized( Authorization.ADMINISTRATE, this.CurrentPerson ) )
                    {
                        mdDeleteWarning.Show( "You are not authorized to delete this calendar.", ModalAlertType.Information );
                        return;
                    }

                    string errorMessage;
                    if ( !connectionTypeService.CanDelete( connectionType, out errorMessage ) )
                    {
                        mdDeleteWarning.Show( errorMessage, ModalAlertType.Information );
                        return;
                    }

                    connectionTypeService.Delete( connectionType );

                    rockContext.SaveChanges();
                }
            }
            NavigateToParentPage();
        }

        /// <summary>
        /// Handles the Click event of the btnDeleteCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnDeleteCancel_Click( object sender, EventArgs e )
        {
            btnDelete.Visible = true;
            btnEdit.Visible = true;
            pnlDeleteConfirm.Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnDelete_Click( object sender, EventArgs e )
        {
            btnDelete.Visible = false;
            btnEdit.Visible = false;
            pnlDeleteConfirm.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            ConnectionType connectionType;
            using ( var rockContext = new RockContext() )
            {
                ConnectionTypeService connectionTypeService = new ConnectionTypeService( rockContext );
                ConnectionActivityTypeService connectionActivityTypeService = new ConnectionActivityTypeService( rockContext );
                ConnectionStatusService connectionStatusService = new ConnectionStatusService( rockContext );
                ConnectionWorkflowService connectionWorkflowService = new ConnectionWorkflowService( rockContext );
                AttributeService attributeService = new AttributeService( rockContext );
                AttributeQualifierService qualifierService = new AttributeQualifierService( rockContext );

                int connectionTypeId = int.Parse( hfConnectionTypeId.Value );

                if ( connectionTypeId == 0 )
                {
                    connectionType = new ConnectionType();
                    connectionTypeService.Add( connectionType );
                }
                else
                {
                    connectionType = connectionTypeService.Queryable( "ConnectionActivityTypes, ConnectionWorkflows" ).Where( c => c.Id == connectionTypeId ).FirstOrDefault();

                    var selectedConnectionWorkflows = WorkflowsState.Select( l => l.Guid );
                    foreach ( var connectionWorkflow in connectionType.ConnectionWorkflows.Where( l => !selectedConnectionWorkflows.Contains( l.Guid ) ).ToList() )
                    {
                        connectionType.ConnectionWorkflows.Remove( connectionWorkflow );
                        connectionWorkflowService.Delete( connectionWorkflow );
                    }

                    var selectedConnectionActivityTypes = ActivityTypesState.Select( r => r.Guid );
                    foreach ( var connectionActivityType in connectionType.ConnectionActivityTypes.Where( r => !selectedConnectionActivityTypes.Contains( r.Guid ) ).ToList() )
                    {
                        connectionType.ConnectionActivityTypes.Remove( connectionActivityType );
                        connectionActivityTypeService.Delete( connectionActivityType );
                    }

                    var selectedConnectionStatuses = StatusesState.Select( r => r.Guid );
                    foreach ( var connectionStatus in connectionType.ConnectionStatuses.Where( r => !selectedConnectionStatuses.Contains( r.Guid ) ).ToList() )
                    {
                        connectionType.ConnectionStatuses.Remove( connectionStatus );
                        connectionStatusService.Delete( connectionStatus );
                    }
                }

                connectionType.Name = tbName.Text;
                connectionType.Description = tbDescription.Text;
                connectionType.IconCssClass = tbIconCssClass.Text;
                connectionType.EnableFutureFollowup = cbFutureFollowUp.Checked;
                connectionType.EnableFullActivityList = cbFullActivityList.Checked;

                foreach ( var connectionActivityTypeState in ActivityTypesState )
                {
                    ConnectionActivityType connectionActivityType = connectionType.ConnectionActivityTypes.Where( a => a.Guid == connectionActivityTypeState.Guid ).FirstOrDefault();
                    if ( connectionActivityType == null )
                    {
                        connectionActivityType = new ConnectionActivityType();
                        connectionType.ConnectionActivityTypes.Add( connectionActivityType );
                    }

                    connectionActivityType.CopyPropertiesFrom( connectionActivityTypeState );
                }

                foreach ( var connectionStatusState in StatusesState )
                {
                    ConnectionStatus connectionStatus = connectionType.ConnectionStatuses.Where( a => a.Guid == connectionStatusState.Guid ).FirstOrDefault();
                    if ( connectionStatus == null )
                    {
                        connectionStatus = new ConnectionStatus();
                        connectionType.ConnectionStatuses.Add( connectionStatus );
                    }
                    connectionStatus.CopyPropertiesFrom( connectionStatusState );
                    connectionStatus.ConnectionTypeId = connectionType.Id;
                }

                foreach ( ConnectionWorkflow connectionWorkflowState in WorkflowsState )
                {
                    ConnectionWorkflow connectionWorkflow = connectionType.ConnectionWorkflows.Where( a => a.Guid == connectionWorkflowState.Guid ).FirstOrDefault();
                    if ( connectionWorkflow == null )
                    {
                        connectionWorkflow = new ConnectionWorkflow();
                        connectionType.ConnectionWorkflows.Add( connectionWorkflow );
                    }
                    else
                    {
                        connectionWorkflowState.Id = connectionWorkflow.Id;
                        connectionWorkflowState.Guid = connectionWorkflow.Guid;
                    }

                    connectionWorkflow.CopyPropertiesFrom( connectionWorkflowState );
                    connectionWorkflow.ConnectionTypeId = connectionTypeId;
                }

                if ( !connectionType.IsValid )
                {
                    // Controls will render the error messages
                    return;
                }

                // need WrapTransaction due to Attribute saves
                rockContext.WrapTransaction( () =>
                {
                    rockContext.SaveChanges();

                    /* Save Attributes */
                    string qualifierValue = connectionType.Id.ToString();
                    SaveAttributes( new ConnectionOpportunity().TypeId, "ConnectionTypeId", qualifierValue, AttributesState, rockContext );

                    // Reload calendar and make sure that the person who may have just added a calendar has security to view/edit/administrate the calendar
                    connectionType = connectionTypeService.Get( connectionType.Id );
                    if ( connectionType != null )
                    {
                        if ( !connectionType.IsAuthorized( Authorization.VIEW, CurrentPerson ) )
                        {
                            connectionType.AllowPerson( Authorization.VIEW, CurrentPerson, rockContext );
                        }
                        if ( !connectionType.IsAuthorized( Authorization.EDIT, CurrentPerson ) )
                        {
                            connectionType.AllowPerson( Authorization.EDIT, CurrentPerson, rockContext );
                        }
                        if ( !connectionType.IsAuthorized( Authorization.ADMINISTRATE, CurrentPerson ) )
                        {
                            connectionType.AllowPerson( Authorization.ADMINISTRATE, CurrentPerson, rockContext );
                        }
                    }
                } );
            }

            var qryParams = new Dictionary<string, string>();
            qryParams["ConnectionTypeId"] = connectionType.Id.ToString();

            NavigateToPage( RockPage.Guid, qryParams );
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            if ( hfConnectionTypeId.Value.Equals( "0" ) )
            {
                NavigateToParentPage();
            }
            else
            {
                ShowReadonlyDetails( GetConnectionType( hfConnectionTypeId.ValueAsInt(), new RockContext() ) );
            }
        }

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            var currentConnectionType = GetConnectionType( hfConnectionTypeId.Value.AsInteger() );
            if ( currentConnectionType != null )
            {
                ShowReadonlyDetails( currentConnectionType );
            }
            else
            {
                string connectionTypeId = PageParameter( "ConnectionTypeId" );
                if ( !string.IsNullOrWhiteSpace( connectionTypeId ) )
                {
                    ShowDetail( connectionTypeId.AsInteger() );
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
        }

        #endregion

        #region ConnectionActivityType Events

        protected void gActivityTypes_Delete( object sender, RowEventArgs e )
        {
            Guid rowGuid = (Guid)e.RowKeyValue;
            ActivityTypesState.RemoveEntity( rowGuid );
            BindConnectionActivityTypesGrid();
        }

        protected void btnAddConnectionActivityType_Click( object sender, EventArgs e )
        {
            ConnectionActivityType connectionActivityType = new ConnectionActivityType();
            connectionActivityType.Name = tbConnectionActivityTypeName.Text;
            if ( !connectionActivityType.IsValid )
            {
                return;
            }
            if ( ActivityTypesState.Any( a => a.Guid.Equals( connectionActivityType.Guid ) ) )
            {
                ActivityTypesState.RemoveEntity( connectionActivityType.Guid );
            }
            ActivityTypesState.Add( connectionActivityType );

            BindConnectionActivityTypesGrid();

            HideDialog();
        }

        private void gActivityTypes_GridRebind( object sender, EventArgs e )
        {
            BindConnectionActivityTypesGrid();
        }

        private void gActivityTypes_Add( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            tbConnectionActivityTypeName.Text = string.Empty;

            ShowDialog( "ConnectionActivityTypes", true );
        }

        private void BindConnectionActivityTypesGrid()
        {
            SetConnectionActivityTypeListOrder( ActivityTypesState );
            gActivityTypes.DataSource = ActivityTypesState.OrderBy( a => a.Name ).ToList();

            gActivityTypes.DataBind();
        }

        /// <summary>
        /// Sets the attribute list order.
        /// </summary>
        /// <param name="attributeList">The attribute list.</param>
        private void SetConnectionActivityTypeListOrder( ViewStateList<ConnectionActivityType> connectionActivityTypeList )
        {
            if ( connectionActivityTypeList != null )
            {
                if ( connectionActivityTypeList.Any() )
                {
                    connectionActivityTypeList.OrderBy( a => a.Name ).ToList();
                }
            }
        }

        #endregion

        #region ConnectionStatus Events

        protected void gStatuses_Delete( object sender, RowEventArgs e )
        {
            Guid rowGuid = (Guid)e.RowKeyValue;
            StatusesState.RemoveEntity( rowGuid );
            BindConnectionStatusesGrid();
        }

        protected void btnAddConnectionStatus_Click( object sender, EventArgs e )
        {
            ConnectionStatus connectionStatus = new ConnectionStatus();
            connectionStatus.Name = tbConnectionStatusName.Text;
            connectionStatus.Description = tbConnectionStatusDescription.Text;
            if ( cbIsDefault.Checked == true )
            {
                foreach ( var connectionStatusState in StatusesState )
                {
                    connectionStatusState.IsDefault = false;
                }
            }
            connectionStatus.IsActive = cbIsActive.Checked;
            connectionStatus.IsDefault = cbIsDefault.Checked;
            connectionStatus.IsCritical = cbIsCritical.Checked;
            if ( !connectionStatus.IsValid )
            {
                return;
            }
            if ( StatusesState.Any( a => a.Guid.Equals( connectionStatus.Guid ) ) )
            {
                StatusesState.RemoveEntity( connectionStatus.Guid );
            }
            StatusesState.Add( connectionStatus );

            BindConnectionStatusesGrid();

            HideDialog();
        }

        private void gStatuses_GridRebind( object sender, EventArgs e )
        {
            BindConnectionStatusesGrid();
        }

        private void gStatuses_Add( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            tbConnectionStatusName.Text = string.Empty;
            tbConnectionStatusDescription.Text = string.Empty;
            cbIsActive.Checked = true;
            cbIsDefault.Checked = false;
            cbIsCritical.Checked = false;
            ShowDialog( "ConnectionStatuses", true );
        }

        private void BindConnectionStatusesGrid()
        {
            SetConnectionStatusListOrder( StatusesState );
            gStatuses.DataSource = StatusesState.OrderBy( a => a.Name ).ToList();

            gStatuses.DataBind();
        }

        /// <summary>
        /// Sets the attribute list order.
        /// </summary>
        /// <param name="attributeList">The attribute list.</param>
        private void SetConnectionStatusListOrder( ViewStateList<ConnectionStatus> connectionStatusList )
        {
            if ( connectionStatusList != null )
            {
                if ( connectionStatusList.Any() )
                {
                    connectionStatusList.OrderBy( a => a.Name ).ToList();
                }
            }
        }

        #endregion

        #region ConnectionWorkflow Events

        protected void dlgConnectionWorkflow_SaveClick( object sender, EventArgs e )
        {
            ConnectionWorkflow connectionWorkflow = null;
            Guid guid = hfAddConnectionWorkflowGuid.Value.AsGuid();
            if ( !guid.IsEmpty() )
            {
                connectionWorkflow = WorkflowsState.FirstOrDefault( l => l.Guid.Equals( guid ) );
            }

            if ( connectionWorkflow == null )
            {
                connectionWorkflow = new ConnectionWorkflow();
            }
            try
            {
                connectionWorkflow.WorkflowType = new WorkflowTypeService( new RockContext() ).Get( ddlWorkflowType.SelectedValueAsId().Value );
            }
            catch { }
            connectionWorkflow.WorkflowTypeId = ddlWorkflowType.SelectedValueAsId().Value;
            connectionWorkflow.TriggerType = ddlTriggerType.SelectedValueAsEnum<ConnectionWorkflowTriggerType>();
            connectionWorkflow.QualifierValue = String.Format( "|{0}|{1}|", ddlPrimaryQualifier.SelectedValue, ddlSecondaryQualifier.SelectedValue );
            connectionWorkflow.ConnectionTypeId = 0;
            if ( !connectionWorkflow.IsValid )
            {
                return;
            }
            if ( WorkflowsState.Any( a => a.Guid.Equals( connectionWorkflow.Guid ) ) )
            {
                WorkflowsState.RemoveEntity( connectionWorkflow.Guid );
            }

            WorkflowsState.Add( connectionWorkflow );

            BindConnectionWorkflowsGrid();

            HideDialog();
        }

        protected void gWorkflows_Delete( object sender, RowEventArgs e )
        {
            Guid rowGuid = (Guid)e.RowKeyValue;
            WorkflowsState.RemoveEntity( rowGuid );

            BindConnectionWorkflowsGrid();
        }

        private void gWorkflows_GridRebind( object sender, EventArgs e )
        {
            BindConnectionWorkflowsGrid();
        }

        protected void gWorkflows_Edit( object sender, RowEventArgs e )
        {
            Guid connectionWorkflowGuid = (Guid)e.RowKeyValue;
            gWorkflows_ShowEdit( connectionWorkflowGuid );
        }

        protected void gWorkflows_ShowEdit( Guid connectionWorkflowGuid )
        {
            ConnectionWorkflow connectionWorkflow = WorkflowsState.FirstOrDefault( l => l.Guid.Equals( connectionWorkflowGuid ) );
            if ( connectionWorkflow != null )
            {
                ddlTriggerType.BindToEnum<ConnectionWorkflowTriggerType>();
                ddlWorkflowType.Items.Clear();
                ddlWorkflowType.Items.Add( new ListItem( string.Empty, string.Empty ) );

                foreach ( var workflowType in new WorkflowTypeService( new RockContext() ).Queryable().OrderBy( w => w.Name ) )
                {
                    if ( workflowType.IsAuthorized( Authorization.VIEW, CurrentPerson ) )
                    {
                        ddlWorkflowType.Items.Add( new ListItem( workflowType.Name, workflowType.Id.ToString() ) );
                    }
                }
                if ( connectionWorkflow.WorkflowTypeId == null )
                {
                    ddlWorkflowType.SelectedValue = "0";
                }
                else
                {
                    ddlWorkflowType.SelectedValue = connectionWorkflow.WorkflowTypeId.ToString();
                }
                ddlTriggerType.SelectedValue = connectionWorkflow.TriggerType.ConvertToInt().ToString();

                hfAddConnectionWorkflowGuid.Value = connectionWorkflowGuid.ToString();
                UpdateTriggerQualifiers();
                ShowDialog( "ConnectionWorkflows", true );
            }
        }

        private void gWorkflows_Add( object sender, EventArgs e )
        {
            ddlTriggerType.BindToEnum<ConnectionWorkflowTriggerType>();
            ddlWorkflowType.Items.Clear();
            ddlWorkflowType.Items.Add( new ListItem( string.Empty, string.Empty ) );

            foreach ( var workflowType in new WorkflowTypeService( new RockContext() ).Queryable().OrderBy( w => w.Name ) )
            {
                if ( workflowType.IsAuthorized( Authorization.VIEW, CurrentPerson ) )
                {
                    ddlWorkflowType.Items.Add( new ListItem( workflowType.Name, workflowType.Id.ToString() ) );
                }
            }
            hfAddConnectionWorkflowGuid.Value = Guid.Empty.ToString();
            UpdateTriggerQualifiers();
            ShowDialog( "ConnectionWorkflows", true );
        }

        protected void ddlTriggerType_SelectedIndexChanged( object sender, EventArgs e )
        {
            UpdateTriggerQualifiers();
        }

        private void UpdateTriggerQualifiers()
        {
            RockContext rockContext = new RockContext();
            String[] qualifierValues = new String[2];
            ConnectionWorkflow connectionWorkflow = WorkflowsState.FirstOrDefault( l => l.Guid.Equals( hfAddConnectionWorkflowGuid.Value.AsGuid() ) );
            ConnectionWorkflowTriggerType connectionWorkflowTriggerType = ddlTriggerType.SelectedValueAsEnum<ConnectionWorkflowTriggerType>();
            int connectionTypeId = int.Parse( hfConnectionTypeId.Value );
            switch ( connectionWorkflowTriggerType )
            {
                case ConnectionWorkflowTriggerType.RequestStarted:
                    ddlPrimaryQualifier.Visible = false;
                    ddlPrimaryQualifier.Items.Clear();
                    ddlSecondaryQualifier.Visible = false;
                    ddlSecondaryQualifier.Items.Clear();
                    break;
                case ConnectionWorkflowTriggerType.RequestCompleted:
                    ddlPrimaryQualifier.Visible = false;
                    ddlPrimaryQualifier.Items.Clear();
                    ddlSecondaryQualifier.Visible = false;
                    ddlSecondaryQualifier.Items.Clear();
                    break;
                case ConnectionWorkflowTriggerType.Manual:
                    ddlPrimaryQualifier.Visible = false;
                    ddlPrimaryQualifier.Items.Clear();
                    ddlSecondaryQualifier.Visible = false;
                    ddlSecondaryQualifier.Items.Clear();
                    break;
                case ConnectionWorkflowTriggerType.StateChanged:
                    ddlPrimaryQualifier.Label = "From";
                    ddlPrimaryQualifier.Visible = true;
                    ddlPrimaryQualifier.BindToEnum<ConnectionState>();
                    ddlPrimaryQualifier.Items.Insert( 0, new ListItem( string.Empty, string.Empty ) );
                    ddlSecondaryQualifier.Label = "To";
                    ddlSecondaryQualifier.Visible = true;
                    ddlSecondaryQualifier.BindToEnum<ConnectionState>();
                    ddlSecondaryQualifier.Items.Insert( 0, new ListItem( string.Empty, string.Empty ) );
                    break;
                case ConnectionWorkflowTriggerType.StatusChanged:
                    var statusList = new ConnectionStatusService( rockContext ).Queryable().Where( s => s.ConnectionTypeId == connectionTypeId || s.ConnectionTypeId == null ).ToList();
                    ddlPrimaryQualifier.Label = "From";
                    ddlPrimaryQualifier.Visible = true;
                    ddlPrimaryQualifier.Items.Clear();
                    ddlPrimaryQualifier.Items.Add( new ListItem( string.Empty, string.Empty ) );
                    foreach ( var status in statusList )
                    {
                        ddlPrimaryQualifier.Items.Add( new ListItem( status.Name, status.Id.ToString().ToUpper() ) );
                    }
                    ddlSecondaryQualifier.Label = "To";
                    ddlSecondaryQualifier.Visible = true;
                    ddlSecondaryQualifier.Items.Clear();
                    ddlSecondaryQualifier.Items.Add( new ListItem( string.Empty, string.Empty ) );
                    foreach ( var status in statusList )
                    {
                        ddlSecondaryQualifier.Items.Add( new ListItem( status.Name, status.Id.ToString().ToUpper() ) );
                    }
                    break;
                case ConnectionWorkflowTriggerType.ActivityAdded:
                    var activityList = new ConnectionActivityTypeService( rockContext ).Queryable().Where( a => a.ConnectionTypeId == connectionTypeId || a.ConnectionTypeId == null ).ToList();
                    ddlPrimaryQualifier.Label = "Activity Type";
                    ddlPrimaryQualifier.Visible = true;
                    ddlPrimaryQualifier.Items.Clear();
                    ddlPrimaryQualifier.Items.Add( new ListItem( string.Empty, string.Empty ) );
                    foreach ( var activity in activityList )
                    {
                        ddlPrimaryQualifier.Items.Add( new ListItem( activity.Name, activity.Id.ToString().ToUpper() ) );
                    }
                    ddlSecondaryQualifier.Visible = false;
                    ddlSecondaryQualifier.Items.Clear();
                    break;
                case ConnectionWorkflowTriggerType.ActivityGroupAssigned:
                    var groupList = new GroupService( rockContext ).Queryable().ToList();
                    ddlPrimaryQualifier.Label = "Activity Group";
                    ddlPrimaryQualifier.Visible = true;
                    ddlPrimaryQualifier.Items.Clear();
                    ddlPrimaryQualifier.Items.Add( new ListItem( string.Empty, string.Empty ) );
                    foreach ( var group in groupList )
                    {
                        ddlPrimaryQualifier.Items.Add( new ListItem( group.Name, group.Id.ToString().ToUpper() ) );
                    }
                    ddlSecondaryQualifier.Visible = false;
                    ddlSecondaryQualifier.Items.Clear();
                    break;
            }
            if ( connectionWorkflow != null )
            {
                if ( connectionWorkflow.TriggerType == ddlTriggerType.SelectedValueAsEnum<ConnectionWorkflowTriggerType>() )
                {
                    qualifierValues = connectionWorkflow.QualifierValue.SplitDelimitedValues();
                    if ( ddlPrimaryQualifier.Visible )
                    {
                        ddlPrimaryQualifier.SelectedValue = qualifierValues[0];
                    }
                    if ( ddlSecondaryQualifier.Visible )
                    {
                        ddlSecondaryQualifier.SelectedValue = qualifierValues[1];
                    }
                }
            }

        }

        private void BindConnectionWorkflowsGrid()
        {
            SetConnectionWorkflowListOrder( WorkflowsState );
            gWorkflows.DataSource = WorkflowsState.Select( c => new
            {
                c.Id,
                c.Guid,
                WorkflowType = c.WorkflowType.Name,
                Trigger = c.TriggerType.ConvertToString()
            } ).ToList();
            gWorkflows.DataBind();
        }

        private void SetConnectionWorkflowListOrder( ViewStateList<ConnectionWorkflow> connectionWorkflowList )
        {
            if ( connectionWorkflowList != null )
            {
                if ( connectionWorkflowList.Any() )
                {
                    connectionWorkflowList.OrderBy( c => c.WorkflowType.Name ).ThenBy( c => c.TriggerType.ConvertToString() ).ToList();
                }
            }
        }

        #endregion

        #endregion

        #region Internal Methods

        /// <summary>
        /// Shows the edit.
        /// </summary>
        /// <param name="connectionTypeId">The Connection Type Type identifier.</param>
        public void ShowDetail( int connectionTypeId )
        {
            pnlDetails.Visible = false;

            ConnectionType connectionType = null;
            RockContext rockContext = null;

            if ( !connectionTypeId.Equals( 0 ) )
            {
                connectionType = GetConnectionType( connectionTypeId, rockContext );
            }

            if ( connectionType == null )
            {
                connectionType = new ConnectionType { Id = 0 };
            }

            bool editAllowed = connectionType.IsAuthorized( Authorization.EDIT, CurrentPerson );

            pnlDetails.Visible = true;
            hfConnectionTypeId.Value = connectionType.Id.ToString();

            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !editAllowed || !IsUserAuthorized( Authorization.ADMINISTRATE ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( ConnectionType.FriendlyTypeName );
            }
            if ( !connectionTypeId.Equals( 0 ) )
            {
                ShowReadonlyDetails( connectionType );
            }
            else
            {
                if ( IsUserAuthorized( Authorization.ADMINISTRATE ) )
                {
                    ShowEditDetails( connectionType );
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="connectionType">the event calendar</param>
        private void ShowEditDetails( ConnectionType connectionType )
        {
            if ( connectionType == null )
            {
                connectionType = new ConnectionType();
                connectionType.IconCssClass = "fa fa-compress";
            }
            if ( connectionType.Id == 0 )
            {
                lReadOnlyTitle.Text = ActionTitle.Add( ConnectionType.FriendlyTypeName ).FormatAsHtmlTitle();
            }
            else
            {
                lReadOnlyTitle.Text = connectionType.Name.FormatAsHtmlTitle();
            }

            lIcon.Text = string.Format( "<i class='{0}'></i>", connectionType.IconCssClass );
            SetEditMode( true );

            var rockContext = new RockContext();

            var connectionTypeService = new ConnectionTypeService( rockContext );
            var attributeService = new AttributeService( rockContext );

            // General
            tbName.Text = connectionType.Name;

            tbDescription.Text = connectionType.Description;

            tbIconCssClass.Text = connectionType.IconCssClass;

            cbFullActivityList.Checked = connectionType.EnableFullActivityList;

            cbFutureFollowUp.Checked = connectionType.EnableFutureFollowup;

            if ( ActivityTypesState == null )
            {
                ActivityTypesState = new ViewStateList<ConnectionActivityType>();
                if ( connectionType.ConnectionActivityTypes != null )
                {
                    ActivityTypesState.AddAll( connectionType.ConnectionActivityTypes.ToList() );
                }
            }

            if ( StatusesState == null )
            {
                StatusesState = new ViewStateList<ConnectionStatus>();
                if ( connectionType.ConnectionStatuses != null )
                {
                    StatusesState.AddAll( connectionType.ConnectionStatuses.ToList() );
                }
            }

            if ( WorkflowsState == null )
            {
                WorkflowsState = new ViewStateList<ConnectionWorkflow>();
                if ( connectionType.ConnectionWorkflows != null )
                {
                    WorkflowsState.AddAll( connectionType.ConnectionWorkflows.ToList() );
                }
            }

            // Attributes
            string qualifierValue = connectionType.Id.ToString();

            AttributesState = new ViewStateList<Attribute>();
            AttributesState.AddAll( attributeService.GetByEntityTypeId( new ConnectionOpportunity().TypeId ).AsQueryable()
                .Where( a =>
                    a.EntityTypeQualifierColumn.Equals( "ConnectionTypeId", StringComparison.OrdinalIgnoreCase ) &&
                    a.EntityTypeQualifierValue.Equals( qualifierValue ) )
                .OrderBy( a => a.Order )
                .ThenBy( a => a.Name )
                .ToList() );
            BindConnectionTypeAttributesGrid();
            BindConnectionActivityTypesGrid();
            BindConnectionWorkflowsGrid();
            BindConnectionStatusesGrid();

        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="connectionType">The event calendar.</param>
        private void ShowReadonlyDetails( ConnectionType connectionType )
        {
            SetEditMode( false );

            hfConnectionTypeId.SetValue( connectionType.Id );
            lReadOnlyTitle.Text = connectionType.Name.FormatAsHtmlTitle();

            lConnectionTypeDescription.Text = connectionType.Description;

            DescriptionList descriptionList = new DescriptionList();
            descriptionList.Add( string.Empty, string.Empty );
            lblMainDetails.Text = descriptionList.Html;

            if ( !connectionType.IsAuthorized( Authorization.EDIT, CurrentPerson ) || !IsUserAuthorized( Authorization.ADMINISTRATE ) )
            {
                btnEdit.Visible = false;
                btnDelete.Visible = false;
            }
        }

        /// <summary>
        /// Gets the event calendar.
        /// </summary>
        /// <param name="connectionTypeId">The event calendar identifier.</param>
        /// <returns></returns>
        private ConnectionType GetConnectionType( int connectionTypeId, RockContext rockContext = null )
        {
            string key = string.Format( "ConnectionType:{0}", connectionTypeId );
            ConnectionType connectionType = RockPage.GetSharedItem( key ) as ConnectionType;
            if ( connectionType == null )
            {
                rockContext = rockContext ?? new RockContext();
                connectionType = new ConnectionTypeService( rockContext ).Queryable()
                    .Where( c => c.Id == connectionTypeId )
                    .FirstOrDefault();
                RockPage.SaveSharedItem( key, connectionType );
            }

            return connectionType;
        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;
            pnlViewDetails.Visible = !editable;

            this.HideSecondaryBlocks( editable );
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="dialog">The dialog.</param>
        /// <param name="setValues">if set to <c>true</c> [set values].</param>
        private void ShowDialog( string dialog, bool setValues = false )
        {
            hfActiveDialog.Value = dialog.ToUpper().Trim();
            ShowDialog( setValues );
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="setValues">if set to <c>true</c> [set values].</param>
        private void ShowDialog( bool setValues = false )
        {
            switch ( hfActiveDialog.Value )
            {
                case "CONNECTIONTYPEATTRIBUTES":
                    dlgConnectionTypeAttribute.Show();
                    break;
                case "CONNECTIONACTIVITYTYPES":
                    dlgActivityTypes.Show();
                    break;
                case "CONNECTIONSTATUSES":
                    dlgStatuses.Show();
                    break;
                case "CONNECTIONWORKFLOWS":
                    dlgConnectionWorkflow.Show();
                    break;
            }
        }

        /// <summary>
        /// Hides the dialog.
        /// </summary>
        private void HideDialog()
        {
            switch ( hfActiveDialog.Value )
            {
                case "CONNECTIONTYPEATTRIBUTES":
                    dlgConnectionTypeAttribute.Hide();
                    break;
                case "CONNECTIONACTIVITYTYPES":
                    dlgActivityTypes.Hide();
                    break;
                case "CONNECTIONSTATUSES":
                    dlgStatuses.Hide();
                    break;
                case "CONNECTIONWORKFLOWS":
                    dlgConnectionWorkflow.Hide();
                    break;
            }

            hfActiveDialog.Value = string.Empty;
        }

        /// <summary>
        /// Sets the attribute list order.
        /// </summary>
        /// <param name="itemList">The item list.</param>
        private void SetAttributeListOrder( ViewStateList<Attribute> itemList )
        {
            int order = 0;
            itemList.OrderBy( a => a.Order ).ToList().ForEach( a => a.Order = order++ );
        }

        /// <summary>
        /// Reorders the attribute list.
        /// </summary>
        /// <param name="itemList">The item list.</param>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        private void ReorderAttributeList( ViewStateList<Attribute> itemList, int oldIndex, int newIndex )
        {
            var movedItem = itemList.Where( a => a.Order == oldIndex ).FirstOrDefault();
            if ( movedItem != null )
            {
                if ( newIndex < oldIndex )
                {
                    // Moved up
                    foreach ( var otherItem in itemList.Where( a => a.Order < oldIndex && a.Order >= newIndex ) )
                    {
                        otherItem.Order = otherItem.Order + 1;
                    }
                }
                else
                {
                    // Moved Down
                    foreach ( var otherItem in itemList.Where( a => a.Order > oldIndex && a.Order <= newIndex ) )
                    {
                        otherItem.Order = otherItem.Order - 1;
                    }
                }

                movedItem.Order = newIndex;
            }
        }

        /// <summary>
        /// Saves the attributes.
        /// </summary>
        /// <param name="entityTypeId">The entity type identifier.</param>
        /// <param name="qualifierColumn">The qualifier column.</param>
        /// <param name="qualifierValue">The qualifier value.</param>
        /// <param name="viewStateAttributes">The view state attributes.</param>
        /// <param name="attributeService">The attribute service.</param>
        /// <param name="qualifierService">The qualifier service.</param>
        /// <param name="categoryService">The category service.</param>
        private void SaveAttributes( int entityTypeId, string qualifierColumn, string qualifierValue, List<Attribute> viewStateAttributes, RockContext rockContext )
        {
            // Get the existing attributes for this entity type and qualifier value
            var attributeService = new AttributeService( rockContext );
            var attributes = attributeService.Get( entityTypeId, qualifierColumn, qualifierValue );

            // Delete any of those attributes that were removed in the UI
            var selectedAttributeGuids = viewStateAttributes.Select( a => a.Guid );
            foreach ( var attr in attributes.Where( a => !selectedAttributeGuids.Contains( a.Guid ) ) )
            {
                attributeService.Delete( attr );
                rockContext.SaveChanges();
                Rock.Web.Cache.AttributeCache.Flush( attr.Id );
            }

            // Update the Attributes that were assigned in the UI
            foreach ( var attributeState in viewStateAttributes )
            {
                Helper.SaveAttributeEdits( attributeState, entityTypeId, qualifierColumn, qualifierValue, rockContext );
            }
        }

        #endregion

        #region ConnectionTypeAttributes Grid and Picker

        /// <summary>
        /// Handles the Add event of the gAttributes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gAttributes_Add( object sender, EventArgs e )
        {
            gAttributes_ShowEdit( Guid.Empty );
        }

        /// <summary>
        /// Handles the Edit event of the gAttributes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gAttributes_Edit( object sender, RowEventArgs e )
        {
            Guid attributeGuid = (Guid)e.RowKeyValue;
            gAttributes_ShowEdit( attributeGuid );
        }

        /// <summary>
        /// Gets the event calendar's attributes_ show edit.
        /// </summary>
        /// <param name="attributeGuid">The attribute GUID.</param>
        protected void gAttributes_ShowEdit( Guid attributeGuid )
        {
            Attribute attribute;
            if ( attributeGuid.Equals( Guid.Empty ) )
            {
                attribute = new Attribute();
                attribute.FieldTypeId = FieldTypeCache.Read( Rock.SystemGuid.FieldType.TEXT ).Id;
                edtConnectionTypeAttributes.ActionTitle = ActionTitle.Add( "attribute for Events of Calendar type " + tbName.Text );
            }
            else
            {
                attribute = AttributesState.First( a => a.Guid.Equals( attributeGuid ) );
                edtConnectionTypeAttributes.ActionTitle = ActionTitle.Edit( "attribute for Events of Calendar type " + tbName.Text );
            }

            var reservedKeyNames = new List<string>();
            AttributesState.Where( a => !a.Guid.Equals( attributeGuid ) ).Select( a => a.Key ).ToList().ForEach( a => reservedKeyNames.Add( a ) );
            edtConnectionTypeAttributes.ReservedKeyNames = reservedKeyNames.ToList();

            edtConnectionTypeAttributes.SetAttributeProperties( attribute, typeof( ConnectionType ) );

            ShowDialog( "ConnectionTypeAttributes" );
        }

        /// <summary>
        /// Handles the GridReorder event of the gAttributes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridReorderEventArgs"/> instance containing the event data.</param>
        protected void gAttributes_GridReorder( object sender, GridReorderEventArgs e )
        {
            ReorderAttributeList( AttributesState, e.OldIndex, e.NewIndex );
            BindConnectionTypeAttributesGrid();
        }

        /// <summary>
        /// Handles the Delete event of the gAttributes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected void gAttributes_Delete( object sender, RowEventArgs e )
        {
            Guid attributeGuid = (Guid)e.RowKeyValue;
            AttributesState.RemoveEntity( attributeGuid );

            BindConnectionTypeAttributesGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gAttributes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gAttributes_GridRebind( object sender, EventArgs e )
        {
            BindConnectionTypeAttributesGrid();
        }

        /// <summary>
        /// Handles the SaveClick event of the dlgConnectionTypeAttribute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void dlgConnectionTypeAttribute_SaveClick( object sender, EventArgs e )
        {
            Rock.Model.Attribute attribute = new Rock.Model.Attribute();
            edtConnectionTypeAttributes.GetAttributeProperties( attribute );

            // Controls will show warnings
            if ( !attribute.IsValid )
            {
                return;
            }

            if ( AttributesState.Any( a => a.Guid.Equals( attribute.Guid ) ) )
            {
                attribute.Order = AttributesState.Where( a => a.Guid.Equals( attribute.Guid ) ).FirstOrDefault().Order;
                AttributesState.RemoveEntity( attribute.Guid );
            }
            else
            {
                attribute.Order = AttributesState.Any() ? AttributesState.Max( a => a.Order ) + 1 : 0;
            }
            AttributesState.Add( attribute );

            BindConnectionTypeAttributesGrid();
            HideDialog();
        }

        /// <summary>
        /// Binds the Connection Type Type attributes grid.
        /// </summary>
        private void BindConnectionTypeAttributesGrid()
        {
            gAttributes.AddCssClass( "attribute-grid" );
            SetAttributeListOrder( AttributesState );
            gAttributes.DataSource = AttributesState
                .Select( a => new
                {
                    a.Id,
                    a.Guid,
                    Name = a.Name,
                    FieldType = a.FieldType != null ? a.FieldType.ToString() : FieldTypeCache.GetName( a.FieldTypeId ),
                    AllowSearch = a.AllowSearch,
                    Order = a.Order
                } )
                .OrderBy( a => a.Order ).ThenBy( a => a.Name ).ToList();
            gAttributes.DataBind();
        }

        #endregion

    }
}