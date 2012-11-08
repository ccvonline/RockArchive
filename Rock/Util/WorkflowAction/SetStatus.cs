﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.ComponentModel;
using System.ComponentModel.Composition;

using Rock.Web.UI;

namespace Rock.Util.WorkflowAction
{
    /// <summary>
    /// Sets a workflow status
    /// </summary>
    [Description( "Set the workflow status" )]
    [Export(typeof(WorkflowActionComponent))]
    [ExportMetadata("ComponentName", "Set Status")]
    [BlockProperty( 0, "Status", "The status to set workflow to", true )]
    public class SetStatus : WorkflowActionComponent
    {
        /// <summary>
        /// Executes the specified workflow.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public override bool Execute(Action action)
        {
            string status = GetAttributeValue( action, "Status" );
            action.Activity.Workflow.Status = status;
            action.AddLogEntry( string.Format( "Set Status to '{0}'", status ) );
            return true;
        }
    }
}