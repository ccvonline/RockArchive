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
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock;
using Rock.Constants;
using Rock.Model;
using Rock.Security;
using Rock.Web;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Administration
{

    /// <summary>
    /// User input controls for metric details
    /// </summary>
    [DisplayName( "Metric Detail" )]
    [Category( "Administration" )]
    [Description( "Displays the details of a specific metric." )]
    public partial class MetricDetail : Rock.Web.UI.RockBlock
    {
        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                string itemId = PageParameter( "metricId" );
                if ( !string.IsNullOrWhiteSpace( itemId ) )
                {
                    ShowDetail( "metricId", int.Parse( itemId ) );
                }
                else
                {
                    pnlDetails.Visible = false;
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the lbEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbEdit_Click( object sender, EventArgs e )
        {
            var metricService = new MetricService();
            var metric = metricService.Get( hfMetricId.ValueAsInt() );
            ShowEdit( metric );
        }

        /// <summary>
        /// Handles the Click event of the lbSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbSave_Click( object sender, EventArgs e )
        {
            using ( new Rock.Data.UnitOfWorkScope() )
            {
                int metricId = hfMetricId.ValueAsInt();
                var metricService = new MetricService();
                Metric metric = null;                

                if ( metricId == 0 )
                {
                    metric = new Metric();
                    metric.IsSystem = false;
                    metricService.Add( metric, CurrentPersonAlias );
                }
                else
                {
                    metric = metricService.Get( metricId );
                }

                metric.Category = tbCategory.Text;
                metric.Title = tbTitle.Text;
                metric.Subtitle = tbSubtitle.Text;
                metric.Description = tbDescription.Text;
                metric.MinValue = tbMinValue.Text.AsType<int?>();
                metric.MaxValue = tbMaxValue.Text.AsType<int?>();
                metric.Type = cbType.Checked;
                metric.CollectionFrequencyValueId = Int32.Parse( ddlCollectionFrequency.SelectedValue );
                metric.Source = tbSource.Text;
                metric.SourceSQL = tbSourceSQL.Text;

                if ( !metric.IsValid )
                {
                    // Controls will render the error messages                    
                    return;
                }

                metricService.Save( metric, CurrentPersonAlias );
                hfMetricId.SetValue( metric.Id );
            }

            var savedMetric = new MetricService().Get( hfMetricId.ValueAsInt() );
            ShowReadOnly( savedMetric );
        }

        /// <summary>
        /// Handles the Click event of the lbCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbCancel_Click( object sender, EventArgs e )
        {
            var metric = new MetricService().Get( hfMetricId.ValueAsInt() );
            ShowReadOnly( metric );
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Binds the collection frequencies.
        /// </summary>
        private void BindCollectionFrequencies()
        {
            ddlCollectionFrequency.Items.Clear();

            var dTCollectionFrequency = DefinedTypeCache.Read( new Guid( Rock.SystemGuid.DefinedType.METRIC_COLLECTION_FREQUENCY ) );

            if ( dTCollectionFrequency != null && dTCollectionFrequency.DefinedValues.Any() )
            {
                var definedValues = dTCollectionFrequency.DefinedValues.OrderBy( dv => dv.Order ).ToList();
                foreach ( var value in definedValues )
                {
                    ddlCollectionFrequency.Items.Add( new ListItem( value.Name, value.Id.ToString() ) );
                }
            }            
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="metric">The metric.</param>
        private void ShowReadOnly( Metric metric )
        {
            SetEditMode( false );

            hfMetricId.SetValue( metric.Id );            
            lDetails.Text = new DescriptionList()
                .Add( "Title", metric.Title )                
                .Add( "Description", metric.Description )
                .Add( "Category", metric.Category )
                .Add( "Collection Frequency", metric.CollectionFrequencyValue.Name )
                .Add( "MinValue", metric.MinValue )
                .Add( "MaxValue", metric.MaxValue )
                .Add( "Source", metric.Source )
                .Html;
        }        

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="metric">The metric.</param>
        private void ShowEdit( Metric metric )
        {
            if ( metric.Id > 0 )
            {
                lActionTitle.Text = ActionTitle.Edit( DefinedType.FriendlyTypeName );
            }
            else
            {
                lActionTitle.Text = ActionTitle.Add( DefinedType.FriendlyTypeName );
            }

            SetEditMode( true );

            BindCollectionFrequencies();
            hfMetricId.Value = metric.Id.ToString();
            tbCategory.Text = metric.Category;
            tbTitle.Text = metric.Title;
            tbSubtitle.Text = metric.Subtitle;
            tbDescription.Text = metric.Description;
            tbMinValue.Text = metric.MinValue.ToString();
            tbMaxValue.Text = metric.MaxValue.ToString();
            cbType.Checked = metric.Type;
            ddlCollectionFrequency.SelectedValue = metric.CollectionFrequencyValueId.ToString();
            tbSource.Text = metric.Source;
            tbSourceSQL.Text = metric.SourceSQL;
        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;            
            fieldsetViewDetails.Visible = !editable;

            this.HideSecondaryBlocks( editable );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="itemKey">The item key.</param>
        /// <param name="itemKeyValue">The item key value.</param>
        private void ShowDetail( string itemKey, int itemKeyValue )
        {
            if ( !itemKey.Equals( "metricId" ) )
            {
                return;
            }
            
            Metric metric = null;
            if ( !itemKeyValue.Equals( 0 ) )
            {
                metric = new MetricService().Get( itemKeyValue );                
            }
            else
            {
                metric = new Metric { Id = 0 };                
            }

            bool readOnly = false;
            if ( !IsUserAuthorized( Authorization.EDIT ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Metric.FriendlyTypeName );
            }

            if ( !readOnly )
            {
                lbEdit.Visible = true;
                if ( metric.Id > 0 )
                {
                    ShowReadOnly( metric );
                }
                else
                {
                    ShowEdit( metric );
                }                
            }
            else
            {
                lbEdit.Visible = false;
                ShowReadOnly( metric );                
            }
                                    
            lbSave.Visible = !readOnly;
        }        

        #endregion
    }
}