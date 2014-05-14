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
using System.ComponentModel.Composition;
using System.Linq;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

namespace Rock.Workflow.Action
{
    /// <summary>
    /// Activates a new activity for a given activity type
    /// </summary>
    [Description( "Activates a new activity instance and all of its actions." )]
    [Export( typeof( ActionComponent ) )]
    [ExportMetadata( "ComponentName", "Activate Activity" )]

    [WorkflowActivityType( "Activity", "The activity type to activate", true, "", "", 0 )]
    [BooleanField( "Stop Further Actions", "Indicates if current activity should be marked complete if selected activity was successfully activated (so no more actions are processed for this activity).", false, "", 1, "CompleteActivity" )]
    public class ActivateActivity : CompareAction
    {
        /// <summary>
        /// Executes the specified workflow.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="action">The action.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        public override bool Execute( RockContext rockContext, WorkflowAction action, Object entity, out List<string> errorMessages )
        {
            errorMessages = new List<string>();

            Guid guid = GetAttributeValue( action, "Activity" ).AsGuid();
            if ( guid.IsEmpty() )
            {
                action.AddLogEntry( "Invalid Activity Property" );
                return false;
            }

            var workflow = action.Activity.Workflow;

            var activityType = new WorkflowActivityTypeService( rockContext ).Queryable()
                .Where( a => a.Guid.Equals( guid ) ).FirstOrDefault();

            if ( activityType == null )
            {
                action.AddLogEntry( "Invalid Activity Property" );
                return false;
            }

            if ( TestCompare( action ) )
            {
                WorkflowActivity.Activate( activityType, workflow );
                action.AddLogEntry( string.Format( "Activated new '{0}' activity", activityType.ToString() ) );

                if ( GetAttributeValue( action, "CompleteActivity" ).AsBoolean() )
                {
                    action.Activity.MarkComplete();
                }
            }

            return true;
        }

    }
}