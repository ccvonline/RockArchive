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
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Finance
{
    [DisplayName( "Business List" )]
    [Category( "Finance" )]
    [Description( "Lists all businesses and provides filtering by business name and owner" )]
    [LinkedPage( "Detail Page" )]
    public partial class BusinessList : Rock.Web.UI.RockBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            bool canEdit = IsUserAuthorized( Authorization.EDIT );

            gfBusinessFilter.ApplyFilterClick += gfBusinessFilter_ApplyFilterClick;
            gfBusinessFilter.DisplayFilterValue += gfBusinessFilter_DisplayFilterValue;

            gBusinessList.DataKeyNames = new string[] { "id" };
            gBusinessList.Actions.ShowAdd = canEdit;
            gBusinessList.Actions.AddClick += gBusinessList_AddClick;
            gBusinessList.GridRebind += gBusinessList_GridRebind;
            gBusinessList.IsDeleteEnabled = canEdit;
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
                BindFilter();
                BindGrid();
            }
        }

        #endregion Control Methods

        #region Events

        private void gfBusinessFilter_DisplayFilterValue( object sender, Rock.Web.UI.Controls.GridFilter.DisplayFilterValueArgs e )
        {
            switch ( e.Key )
            {
                case "Business Name":
                    break;

                case "Owner":
                    break;
            }
        }

        private void gfBusinessFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            gfBusinessFilter.SaveUserPreference( "Business Name", tbBusinessName.Text );
            gfBusinessFilter.SaveUserPreference( "Owner", ppBusinessOwner.PersonId.ToString() );
            BindGrid();
        }

        private void gBusinessList_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        private void gBusinessList_AddClick( object sender, EventArgs e )
        {
            var parms = new Dictionary<string, string>();
            parms.Add( "businessId", "0" );
            NavigateToLinkedPage( "DetailPage", parms );
        }

        protected void gBusinessList_RowDataBound( object sender, GridViewRowEventArgs e )
        {
            if ( e.Row.RowType == DataControlRowType.DataRow )
            {
                var business = e.Row.DataItem as Person;
                var location = business.GivingGroup.GroupLocations.FirstOrDefault().Location;
                if ( !string.IsNullOrWhiteSpace( location.Street2 ) )
                {
                    PlaceHolder phStreet2 = e.Row.FindControl( "phStreet2" ) as PlaceHolder;
                    if ( phStreet2 != null )
                    {
                        phStreet2.Controls.Add( new LiteralControl( string.Format( "{0}</br>", location.Street2 ) ) );
                    }
                }
            }
        }

        protected void gBusinessList_RowSelected( object sender, Rock.Web.UI.Controls.RowEventArgs e )
        {
            var parms = new Dictionary<string, string>();
            var businessId = (int)e.RowKeyValue;
            parms.Add( "businessId", businessId.ToString() );
            NavigateToLinkedPage( "DetailPage", parms );
        }

        protected void gBusinessList_Edit( object sender, Rock.Web.UI.Controls.RowEventArgs e )
        {
        }

        protected void gBusinessList_Delete( object sender, Rock.Web.UI.Controls.RowEventArgs e )
        {
        }

        #endregion Events

        #region Internal Methods

        /// <summary>
        /// Binds the filter.
        /// </summary>
        private void BindFilter()
        {
            var rockContext = new RockContext();

            // Business Name Filter
            tbBusinessName.Text = gfBusinessFilter.GetUserPreference( "Business Name" );

            // Owner Filter
            int businessId = 0;
            if ( int.TryParse( gfBusinessFilter.GetUserPreference( "Owner" ), out businessId ) )
            {
                var businessService = new PersonService( rockContext );
                var business = businessService.Get( businessId );
                if ( business != null )
                {
                    ppBusinessOwner.SetValue( business );
                }
            }
        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            var rockContext = new RockContext();
            var queryable = new PersonService( rockContext ).Queryable();
            var recordTypeValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_TYPE_BUSINESS.AsGuid() ).Id;
            queryable = queryable.Where( q => q.RecordTypeValueId == recordTypeValueId );

            // Business Name Filter
            var businessName = gfBusinessFilter.GetUserPreference( "Business Name" );
            if ( !string.IsNullOrWhiteSpace( businessName ) )
            {
                queryable = queryable.Where( a => a.FirstName.Contains( businessName ) );
            }

            // Owner Filter
            int businessId = 0;
            if ( int.TryParse( gfBusinessFilter.GetUserPreference( "Owner" ), out businessId ) && businessId != 0 )
            {
                queryable = queryable.Where( a => a.Id == businessId );
            }

            SortProperty sortProperty = gBusinessList.SortProperty;
            if ( sortProperty != null )
            {
                gBusinessList.DataSource = queryable.Sort( sortProperty ).ToList();
            }
            else
            {
                gBusinessList.DataSource = queryable.OrderBy( q => q.FirstName ).ToList();
            }

            gBusinessList.DataBind();
        }

        /// <summary>
        /// Shows the detail form.
        /// </summary>
        /// <param name="id">The id.</param>
        protected void ShowDetailForm( int id )
        {
            NavigateToLinkedPage( "DetailPage", "businessId", id );
        }

        #endregion Internal Methods
    }
}