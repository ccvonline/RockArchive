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
using System.ComponentModel;
using System.Web.UI;

using Rock;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.UI;

namespace RockWeb.Blocks.Reporting
{
    [DisplayName( "Metric Value Detail" )]
    [Category( "Reporting" )]
    [Description( "Displays the details of a particular metric value." )]
    public partial class MetricValueDetail : RockBlock, IDetailBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                int? itemId = PageParameter( "MetricValueId" ).AsInteger( false );
                int? metricId = PageParameter( "MetricId" ).AsInteger( false );
                if ( itemId.HasValue )
                {
                    ShowDetail( "MetricValueId", itemId.Value, metricId );
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
        }

        #endregion

        #region Edit Events

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            NavigateToParentPage();
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            MetricValue metricValue;
            var rockContext = new RockContext();
            MetricValueService metricValueService = new MetricValueService( rockContext );

            int metricValueId = int.Parse( hfMetricValueId.Value );

            if ( metricValueId == 0 )
            {
                metricValue = new MetricValue();
                metricValueService.Add( metricValue );
            }
            else
            {
                metricValue = metricValueService.Get( metricValueId );
            }

            metricValue.MetricValueType = ddlMetricValueType.SelectedValueAsEnum<MetricValueType>();
            metricValue.XValue = tbXValue.Text;
            metricValue.YValue = tbYValue.Text.AsDecimal( false );
            metricValue.MetricId = hfMetricId.ValueAsInt();
            metricValue.Note = tbNote.Text;
            metricValue.MetricValueDateTime = dtpMetricValueDateTime.SelectedDateTimeIsBlank ? null : dtpMetricValueDateTime.SelectedDateTime;
            metricValue.CampusId = cpCampus.SelectedCampusId;

            if ( !metricValue.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            rockContext.SaveChanges();

            NavigateToParentPage();
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        public void ShowDetail( string itemKey, int itemKeyValue )
        {
            ShowDetail( itemKey, itemKeyValue, null );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        /// <param name="metricId">The metric identifier.</param>
        public void ShowDetail( string itemKey, int itemKeyValue, int? metricId )
        {
            // return if unexpected itemKey 
            if ( itemKey != "MetricValueId" )
            {
                return;
            }

            pnlDetails.Visible = true;

            // Load depending on Add(0) or Edit
            MetricValue metricValue = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                metricValue = new MetricValueService( new RockContext() ).Get( itemKeyValue );
                lActionTitle.Text = ActionTitle.Edit( MetricValue.FriendlyTypeName ).FormatAsHtmlTitle();
            }
            else
            {
                metricValue = new MetricValue { Id = 0 };
                lActionTitle.Text = ActionTitle.Add( MetricValue.FriendlyTypeName ).FormatAsHtmlTitle();
            }

            hfMetricValueId.Value = metricValue.Id.ToString();

            ddlMetricValueType.SelectedValue = metricValue.MetricValueType.ConvertToInt().ToString();
            tbXValue.Text = metricValue.XValue.ToString();
            tbYValue.Text = metricValue.YValue.ToString();
            hfMetricId.Value = metricId.ToString();
            tbNote.Text = metricValue.Note;
            dtpMetricValueDateTime.SelectedDateTime = metricValue.MetricValueDateTime;
            cpCampus.SelectedCampusId = metricValue.CampusId;


            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( Authorization.EDIT ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( MetricValue.FriendlyTypeName );
            }

            if ( readOnly )
            {
                lActionTitle.Text = ActionTitle.View( MetricValue.FriendlyTypeName );
                btnCancel.Text = "Close";
            }

            ddlMetricValueType.Enabled = !readOnly;
            tbXValue.ReadOnly = readOnly;
            tbYValue.ReadOnly = readOnly;
            tbNote.ReadOnly = readOnly;
            dtpMetricValueDateTime.Enabled = !readOnly;
            cpCampus.Enabled = !readOnly;

            btnSave.Visible = !readOnly;
        }

        #endregion
    }
}