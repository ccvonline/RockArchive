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
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using Rock.Financial;

namespace RockWeb.Blocks.Finance
{
    /// <summary>
    /// Block used to download any scheduled payment transactions that were processed by payment gateway during a specified date range.
    /// </summary>
    [DisplayName( "Scheduled Payment Download" )]
    [Category( "Finance" )]
    [Description( "Block used to download any scheduled payment transactions that were processed by payment gateway during a specified date range." )]

    [TextField( "Batch Name Prefix", "The batch prefix name to use when creating a new batch", false, "Online Giving - ", "", 2 )]
    public partial class ScheduledPaymentDownload : Rock.Web.UI.RockBlock
    {

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

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
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            // Set timeout for up to 15 minutes (just like installer)
            Server.ScriptTimeout = 900;
            ScriptManager.GetCurrent( Page ).AsyncPostBackTimeout = 900;

            nbSuccess.Visible = false;
            nbError.Visible = false;

            if ( !Page.IsPostBack )
            {
                var gateway = GetSelectedGateway();
                if ( gateway != null )
                {
                    var today = RockDateTime.Today;
                    var days = today.DayOfWeek == DayOfWeek.Monday ? new TimeSpan( 3, 0, 0, 0 ) : new TimeSpan( 1, 0, 0, 0 );
                    var endDateTime = today.Add( gateway.BatchTimeOffset );

                    drpDates.UpperValue = RockDateTime.Now.CompareTo( endDateTime ) < 0 ? today.AddDays( -1 ) : today;
                    drpDates.LowerValue = drpDates.UpperValue.Value.Subtract( days );
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        protected void btnDownload_Click( object sender, EventArgs e )
        {
            string batchNamePrefix = GetAttributeValue( "BatchNamePrefix" );

            DateTime? startDateTime = drpDates.LowerValue;
            DateTime? endDateTime = drpDates.UpperValue;

            if (startDateTime.HasValue && endDateTime.HasValue && endDateTime.Value.CompareTo(startDateTime.Value) >= 0)
            {
                var gateway = GetSelectedGateway();
                if (gateway != null)
                {
                    DateTime start = startDateTime.Value;
                    DateTime end = endDateTime.Value.AddDays( 1 );

                    string errorMessage = string.Empty;
                    var payments = gateway.GetPayments( start, end, out errorMessage );

                    if ( string.IsNullOrWhiteSpace( errorMessage ) )
                    {
                        string resultSummary = FinancialScheduledTransactionService.ProcessPayments( gateway, batchNamePrefix, payments );
                        if (!string.IsNullOrWhiteSpace(resultSummary))
                        {
                            nbSuccess.Text = string.Format( "<ul class='list-padded'>{0}</ul>", resultSummary );
                            nbSuccess.Visible = true;
                        }

                    }
                    else
                    {
                        ShowError( errorMessage );
                    }
                }
                else
                {
                    ShowError("Please select a valid Payment Gateway!");
                }
            }
            else
            {
                ShowError("Please select a valid Date Range!");
            }

        }

        #endregion

        #region Methods

        private GatewayComponent GetSelectedGateway()
        {
            Guid? gatewayGuid = cpGateway.SelectedValueAsGuid();
            if ( gatewayGuid.HasValue )
            {
                return GatewayContainer.GetComponent( gatewayGuid.Value.ToString() );
            }
            return null;
        }

        private void ShowError(string message)
        {
            nbError.Text = message;
            nbError.Visible = true;
        }

        #endregion

}
}