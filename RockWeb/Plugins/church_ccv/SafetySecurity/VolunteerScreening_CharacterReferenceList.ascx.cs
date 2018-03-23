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
using church.ccv.SafetySecurity.Model;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;

namespace RockWeb.Plugins.church_ccv.SafetySecurity
{
    [DisplayName( "Volunteer Screening Character Reference List" )]
    [Category( "CCV > Safety and Security" )]
    [Description( "Lists completed character references." )]
    
    [LinkedPage( "Detail Page" )]
    public partial class VolunteerScreening_CharacterReferenceList : RockBlock
    {
        public object RockTransactionScope { get; private set; }
        
        const int sCharacterReferenceWorkflowId = 203;
                
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            
            InitInstancesGrid( );
        }

        void InitInstancesGrid( )
        {
            gGrid.DataKeyNames = new string[] { "Id" };

            // turn on only the 'add' button
            gGrid.Actions.Visible = true;
            gGrid.Actions.Enabled = true;
            gGrid.Actions.ShowBulkUpdate = false;
            gGrid.Actions.ShowCommunicate = false;
            gGrid.Actions.ShowExcelExport = false;
            gGrid.Actions.ShowMergePerson = false;
            gGrid.Actions.ShowMergeTemplate = false;
            
            gGrid.GridRebind += gGrid_Rebind;
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
                using ( RockContext rockContext = new RockContext( ) )
                {
                    Render( rockContext );
                }
            }
        }
        
        #endregion
        
        #region Grid Events (main grid)
        
        /// <summary>
        /// Handles the GridRebind event of the gPromotions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void gGrid_Rebind( object sender, EventArgs e )
        {
            using ( RockContext rockContext = new RockContext( ) )
            {
                Render( rockContext );
            }
        }

        protected void gGrid_Edit( object sender, RowEventArgs e )
        {
            var qryParams = new Dictionary<string, string>();
            qryParams.Add( "CharacterReferenceWorkflowId", e.RowKeyId.ToString() );
            
            NavigateToLinkedPage( "DetailPage", qryParams );
        }

        #endregion

        #region Internal Methods

        class WorkflowWithAttribs
        {
            public int WorkflowId;
            public string WorkflowName;
            public DateTime? CreatedDateTime;
            public List<KeyValuePair<string, string>> AttribValues;

            public string GetAttribValue( string key )
            {
                return AttribValues.Where( av => av.Key == key ).SingleOrDefault( ).Value;
            }
        }

        private void Render( RockContext rockContext )
        {
            // get all completed character references
            var charRefWorkflows = new WorkflowService( rockContext ).Queryable( ).AsNoTracking( ).Where( wf => wf.WorkflowTypeId == sCharacterReferenceWorkflowId && wf.Status == "Completed" );

            var attribQuery = new AttributeService( rockContext ).Queryable( ).AsNoTracking( );
            var avQuery = new AttributeValueService( rockContext ).Queryable( ).AsNoTracking( );
            var attribWithValue = attribQuery.Join( avQuery, a => a.Id, av => av.AttributeId, ( a, av ) => new { Attribute = a, AttribValue = av } )
                                             .Where( a => a.Attribute.EntityTypeQualifierColumn.Equals( "WorkflowTypeId", StringComparison.OrdinalIgnoreCase ) );

            // now for each pending covenant, get the key / value for the attributes we need
            List<WorkflowWithAttribs> workflowList = new List<WorkflowWithAttribs>( );
            foreach( var workflow in charRefWorkflows )
            {
                WorkflowWithAttribs workflowWithAttribs = new WorkflowWithAttribs( );
                workflowWithAttribs.WorkflowId = workflow.Id;
                workflowWithAttribs.WorkflowName = workflow.Name;
                workflowWithAttribs.CreatedDateTime = workflow.CreatedDateTime;

                workflowWithAttribs.AttribValues = attribWithValue.Where( av => av.AttribValue.EntityId == workflow.Id &&
                                                                                (av.Attribute.Key == "CompletionNumber" || 
                                                                                 av.Attribute.Key == "ApplicantFirstName" ||
                                                                                 av.Attribute.Key == "ApplicantLastName") )
                                                                    .Select( a => new { a.Attribute.Key, a.AttribValue.Value } ).AsEnumerable( )
                                                                    .Select( o => new KeyValuePair<string, string>( o.Key, o.Value ) )
                                                                    .ToList( );

                workflowList.Add( workflowWithAttribs );
            }
            
            // the header should show the "total completed". We could use the total number of completed, but security wanted to continue the counting
            // from their prior system, so the base isn't 0. The number is set once in the Workflow, so we just sort the workflows by their CompletionNumber, and take the top.
            workflowList = workflowList.OrderByDescending( wf => int.Parse( wf.GetAttribValue( "CompletionNumber") ) ).ToList( );

            // update the header text (take the highest numbered completion number)
            lHeader.Text = string.Format( "<h4>Total Responses ({0})</h4>", workflowList.First( ).GetAttribValue("CompletionNumber") );

            // render the grid
            BindGrid( rockContext, workflowList );
        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid( RockContext rockContext, List<WorkflowWithAttribs> charRefWorkflows )
        {
            gGrid.DataSource = charRefWorkflows.Select( wf => 
                    new {
                            CompletionNumber = wf.GetAttribValue("CompletionNumber"),
                            Id = wf.WorkflowId,
                            Date = wf.CreatedDateTime.Value.ToShortDateString( ),
                            VolunteerApplicantsName = wf.GetAttribValue("ApplicantFirstName") + " " + wf.GetAttribValue("ApplicantLastName")
                        } ).ToList( );

            gGrid.DataBind();
        }
        
        #endregion
    }
}
