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
using Rock.Attribute;
using Rock.Model;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// User control for managing metrics
    /// </summary>
    [DisplayName( "Metric List" )]
    [Category( "Administration" )]
    [Description( "Displays a list of metrics defined in the system." )]
    [LinkedPage( "Detail Page" )]
    public partial class MetricList : Rock.Web.UI.RockBlock
    {        
        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            gMetrics.DataKeyNames = new string[] { "id" };
            gMetrics.Actions.ShowAdd = true;
            gMetrics.Actions.AddClick += gMetrics_Add;
            gMetrics.GridRebind += gMetrics_GridRebind;                    
           
            gfFilter.ApplyFilterClick += gfFilter_ApplyFilterClick;
            BindCategoryFilter();                
                       
            bool canAddEditDelete = IsUserAuthorized( "Edit" );
            gMetrics.Actions.ShowAdd = canAddEditDelete;
            gMetrics.IsDeleteEnabled = canAddEditDelete;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                BindGrid();
            }

            base.OnLoad( e );
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the ApplyFilterClick event of the rFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void gfFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            gfFilter.SaveUserPreference( "Category", ddlCategoryFilter.SelectedValue );
            BindGrid();
        }

        /// <summary>
        /// Handles the Add event of the gMetrics control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gMetrics_Add( object sender, EventArgs e )
        {
            NavigateToLinkedPage( "DetailPage", "metricId", 0 );
        }

        /// <summary>
        /// Handles the Edit event of the gMetrics control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gMetrics_Edit( object sender, RowEventArgs e )
        {
            NavigateToLinkedPage( "DetailPage", "metricId", (int)e.RowKeyValue );
        }
        
        /// <summary>
        /// Handles the Delete event of the gMetrics control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gMetrics_Delete( object sender, RowEventArgs e )
        {
            var metricService = new MetricService();

            Metric metric = metricService.Get( (int)e.RowKeyValue );
            if ( metric != null )
            {
                metricService.Delete( metric, CurrentPersonAlias );
                metricService.Save( metric, CurrentPersonAlias );
            }

            BindGrid();
        }
        
        /// <summary>
        /// Handles the GridRebind event of the gMetrics control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void gMetrics_GridRebind( object sender, EventArgs e )
        {
            BindCategoryFilter();
            BindGrid();
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Binds the category filter.
        /// </summary>
        private void BindCategoryFilter()
        {
            ddlCategoryFilter.Items.Clear();
            ddlCategoryFilter.Items.Add( Rock.Constants.All.Text );

            var metricService = new MetricService();
            var items = metricService.Queryable().
                Where( a => a.Category != "" && a.Category != null ).
                OrderBy( a => a.Category ).
                Select( a => a.Category ).
                Distinct().ToList();

            foreach ( var item in items )
            {
                var li = new ListItem( item );
                li.Selected = ( !Page.IsPostBack && gfFilter.GetUserPreference( "Category" ) == item );
                ddlCategoryFilter.Items.Add( li );
            }
        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            var queryable = new MetricService().Queryable();

            if ( ddlCategoryFilter.SelectedValue != Rock.Constants.All.Text )
            {
                queryable = queryable.Where( a => a.Category == ddlCategoryFilter.SelectedValue );
            }

            var sortProperty = gMetrics.SortProperty;
            if ( sortProperty != null )
            {
                queryable = queryable.Sort( sortProperty );
            }
            else
            {
                queryable = queryable.OrderBy( a => a.Category ).ThenBy( a => a.Title );
            }

            gMetrics.DataSource = queryable.ToList();
            gMetrics.DataBind();
        }       

        #endregion
    }
}