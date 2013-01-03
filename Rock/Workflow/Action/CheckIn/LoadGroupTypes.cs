﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;

namespace Rock.Workflow.Action.CheckIn
{
    /// <summary>
    /// Loads the group types allowed for each person in a family
    /// </summary>
    [Description("Loads the group types allowed for each person in a family")]
    [Export(typeof(ActionComponent))]
    [ExportMetadata( "ComponentName", "Load Group Types" )]
    public class LoadGroupTypes : ActionComponent
    {
        public override bool Execute( Model.WorkflowAction action, Data.IEntity entity, out List<string> errorMessages )
        {
            throw new NotImplementedException();
        }
    }
}