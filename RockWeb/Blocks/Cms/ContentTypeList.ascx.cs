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
using System.Linq;
using System.Web.UI;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using System.ComponentModel;
using Rock.Security;
using Rock.Web.Cache;
using System.Collections.Generic;

namespace RockWeb.Blocks.Cms
{
    /// <summary>
    /// 
    /// </summary>
    [DisplayName("Content Type List")]
    [Category("CMS")]
    [Description("Lists content types in the system.")]

    [LinkedPage("Detail Page")]
    public partial class ContentTypeList : RockBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;

            // Block Security and special attributes (RockPage takes care of View)
            bool canAddEditDelete = IsUserAuthorized( Authorization.EDIT );

            gContentType.DataKeyNames = new string[] { "id" };
            gContentType.Actions.AddClick += gContentType_Add;
            gContentType.GridRebind += gContentType_GridRebind;
            gContentType.Actions.ShowAdd = canAddEditDelete;
            gContentType.IsDeleteEnabled = canAddEditDelete;

            SecurityField securityField = gContentType.Columns.OfType<SecurityField>().FirstOrDefault();
            if ( securityField != null )
            {
                securityField.EntityTypeId = EntityTypeCache.Read( typeof( Rock.Model.ContentType ) ).Id;
            }

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
        /// Handles the BlockUpdated event of the Block control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            BindGrid();
        }
        
        /// <summary>
        /// Handles the Add event of the gContentType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gContentType_Add( object sender, EventArgs e )
        {
            NavigateToLinkedPage( "DetailPage", "contentTypeId", 0 );
        }

        /// <summary>
        /// Handles the Edit event of the gContentType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gContentType_Edit( object sender, RowEventArgs e )
        {
            NavigateToLinkedPage( "DetailPage", "contentTypeId", e.RowKeyId );
        }

        /// <summary>
        /// Handles the Delete event of the gContentType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gContentType_Delete( object sender, RowEventArgs e )
        {
            var rockContext = new RockContext();
            ContentTypeService contentTypeService = new ContentTypeService( rockContext );
            ContentType contentType = contentTypeService.Get( (int)e.RowKeyValue );

            if ( contentType != null )
            {
                string errorMessage;
                if ( !contentTypeService.CanDelete( contentType, out errorMessage ) )
                {
                    mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                    return;
                }

                contentTypeService.Delete( contentType );
                rockContext.SaveChanges();
            }

            BindGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gContentType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void gContentType_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            ContentTypeService contentTypeService = new ContentTypeService( new RockContext() );
            SortProperty sortProperty = gContentType.SortProperty;
            var qry = contentTypeService.Queryable( "Channels" );

            List<ContentType> types = null;
            if ( sortProperty != null )
            {
                types = qry.Sort( sortProperty ).ToList();
            }
            else
            {
                types = qry.OrderBy( p => p.Name ).ToList();
            }

            gContentType.DataSource = types.Select( t => new
            {
                t.Id,
                t.Name,
                t.IsSystem,
                Channels = t.Channels.Count()
            } )
            .ToList();
            gContentType.DataBind();
        }

        #endregion
    }
}