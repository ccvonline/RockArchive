﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Linq;
using System.Web.UI;
using Rock.Attribute;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace RockWeb.Blocks.Administration
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ExternalFileList : RockBlock
    {
        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            gBinaryFile.DataKeyNames = new string[] { "id" };
            gBinaryFile.Actions.ShowAdd = false;
            gBinaryFile.GridRebind += gBinaryFile_GridRebind;
            gBinaryFile.RowItemText = "External File";
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
        /// Handles the Click event of the Download control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void Download_Click( object sender, RowEventArgs e )
        {
            string fileUrl = string.Format( "{0}GetFile.ashx?id={1}", ResolveUrl( "~" ), e.RowKeyId );
            Response.Redirect( fileUrl, false );
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void gBinaryFile_RowDataBound( object sender, System.Web.UI.WebControls.GridViewRowEventArgs e )
        {
            var site = RockPage.Site;
            if ( e.Row.RowType == DataControlRowType.DataRow )
            {
                var downloadCellIndex = gBinaryFile.Columns.IndexOf( gBinaryFile.Columns.OfType<HyperLinkField>().First( a => a.Text == "Download" ) );
                if ( downloadCellIndex >= 0 )
                {
                    string fileUrl = string.Format( "{0}GetFile.ashx?id={1}", ResolveUrl( "~" ), e.Row.DataItem.GetPropertyValue( "Id" ).ToString() );
                    e.Row.Cells[downloadCellIndex].Text = string.Format( "<a href='{0}' class='btn btn-action btn-xs'><i class='fa fa-download'></i> Download</a>", fileUrl );
                }

                Literal lAppName = e.Row.FindControl( "lAppName" ) as Literal;
                if ( lAppName != null )
                {
                    var binaryFile = e.Row.DataItem as BinaryFile;
                    binaryFile.LoadAttributes();
                    lAppName.Text = binaryFile.GetAttributeValue( "Name" );
                }

            }
        }

        /// <summary>
        /// Handles the GridRebind event of the gBinaryFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void gBinaryFile_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            Guid binaryFileTypeGuid = Rock.SystemGuid.BinaryFiletype.EXTERNAL_FILE.AsGuid();

            var binaryFileService = new BinaryFileService();
            var queryable = binaryFileService.Queryable().Where( f => f.BinaryFileType.Guid == binaryFileTypeGuid );

            var sortProperty = gBinaryFile.SortProperty;

            List<BinaryFile> list;

            if ( sortProperty != null )
            {
                list = queryable.Sort( sortProperty ).ToList();
            }
            else
            {
                list = queryable.OrderBy( d => d.FileName ).ToList();
            }

            foreach ( var item in list )
            {
                item.LoadAttributes();
            }

            gBinaryFile.DataSource = list;

            gBinaryFile.DataBind();
        }


        #endregion

    }
}