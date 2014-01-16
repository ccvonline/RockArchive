﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using Rock.Model;
using Rock.Web.UI;

namespace RockWeb.Blocks.Utility
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "CkEditor MergeField" )]
    [Category( "Utility" )]
    [Description( "Block to be used as part of the RockMergeField CKEditor Plugin" )]
    public partial class CkEditorMergeFieldPicker : RockBlock
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

            string mergeFields = PageParameter( "mergeFields" );

            if ( !string.IsNullOrEmpty( mergeFields ) )
            {
                mfpCkEditor.MergeFields = mergeFields.Split( ',' ).ToList();
            }

            if ( !this.Page.IsPostBack )
            {
                // we are using the Dialog layout, but don't want the Header and Footer to show
                string startupScript = @" 
var iframeHead = $('head');
$('<style> .modal-header { display: none; } .modal-footer { display: none; } </style>').appendTo(iframeHead);
";
                ScriptManager.RegisterStartupScript( this, this.GetType(), "script_" + this.ID, startupScript, true );
            }
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the PageLiquid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            //
        }

        /// <summary>
        /// Handles the SelectItem event of the mfpCkEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void mfpCkEditor_SelectItem( object sender, EventArgs e )
        {
            // set the result of the dialog so that the CkEditor plugin can grab it
            hfResultValue.Value = mfpCkEditor.SelectedValue;
        }

        #endregion

        #region Methods

        //// helper functional methods (like BindGrid(), etc.)

        #endregion
    }
}