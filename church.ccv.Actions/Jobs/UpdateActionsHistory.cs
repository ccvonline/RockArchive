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
using Quartz;
using Rock.Data;

namespace church.ccv.Actions
{
    [DisallowConcurrentExecution]
    public class UpdateActionsHistory : IJob
    {
        /// <summary> 
        /// Empty constructor for job initialization
        /// <para>
        /// Jobs require a public empty constructor so that the
        /// scheduler can instantiate the class whenever it needs.
        /// </para>
        /// </summary>
        public UpdateActionsHistory()
        {
            // determine the last Sunday that was run
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute( IJobExecutionContext context )
        {
            using ( RockContext rockContext = new RockContext() )
            {
                // This job typically completes in under 3 minutes, but give it 5 just in case.
                rockContext.Database.CommandTimeout = 300;

                // Simply run the stored procedure.
                // This will insert a row per-campus per-region with total numbers for how many people
                // are performing various CCV Actions.

                // The tables updated are: 
                // _church_ccv_Actions_History_Adult
                // _church_ccv_Actions_History_Student
                rockContext.Database.ExecuteSqlCommand( "_church_ccv_spActions_Build_Full_History" );

                // This is designed to be run once a week. 
                // The date stamps will be the day this job is run.
                // Currently, the UpdateStepsMeasure job uses this, but anything needing general totals can.

                context.Result = "CCV Actions History Updated.";
            }
        }
    }
}
