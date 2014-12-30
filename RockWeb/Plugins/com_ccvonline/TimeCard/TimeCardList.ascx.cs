using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using com.ccvonline.TimeCard.Data;
using com.ccvonline.TimeCard.Model;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.com_ccvonline.TimeCard
{
    /// <summary>
    /// Lists all the Referral Agencies.
    /// </summary>
    [DisplayName( "Time Card List" )]
    [Category( "CCV > Time Card" )]
    [Description( "Lists all the time cards for a specific employee." )]

    [LinkedPage( "Detail Page" )]
    public partial class TimeCardList : Rock.Web.UI.RockBlock
    {
        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            bool canEdit = IsUserAuthorized( Rock.Security.Authorization.EDIT );
            
            // TimeCard/Time Card Pay Period is auto created based on current date
            gList.Actions.ShowAdd = false;

            gList.IsDeleteEnabled = false;
            gList.Actions.AddClick += gList_AddClick;
            gList.GridRebind += gList_GridRebind;
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
                BindGrid();
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
            //
        }

        /// <summary>
        /// Handles the GridRebind event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void gList_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        /// <summary>
        /// Handles the AddClick event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void gList_AddClick( object sender, EventArgs e )
        {
            NavigateToLinkedPage( "DetailPage", "TimeCardId", 0 );
        }

        /// <summary>
        /// Handles the RowSelected event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gList_RowSelected( object sender, RowEventArgs e )
        {
            NavigateToLinkedPage( "DetailPage", "TimeCardId", e.RowKeyId);
        }

        /// <summary>
        /// Handles the Delete event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gList_Delete( object sender, RowEventArgs e )
        {
            var dataContext = new TimeCardContext();
            var timeCardService = new TimeCardService<com.ccvonline.TimeCard.Model.TimeCard>( dataContext );
            var timeCard = timeCardService.Get( e.RowKeyId );
            if (timeCard != null)
            {
                string errorMessage;
                if ( !timeCardService.CanDelete( timeCard, out errorMessage ) )
                {
                    mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                    return;
                }

                timeCardService.Delete( timeCard );
                dataContext.SaveChanges();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the hours HTML.
        /// </summary>
        /// <param name="timeCardDay">The time card day.</param>
        /// <returns></returns>
        public string GetHoursHtml( TimeCardDay timeCardDay )
        {
            return "TODO!";
        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            var dataContext = new TimeCardContext();
            var timeCardService = new TimeCardService<com.ccvonline.TimeCard.Model.TimeCard>( dataContext );
            timeCardService.EnsureCurrentPayPeriod();
            SortProperty sortProperty = gList.SortProperty;

            var qry = timeCardService.Queryable().Where( a => a.PersonAliasId == this.CurrentPersonAliasId );

            if (sortProperty != null)
            {
                qry = qry.Sort( sortProperty );
            }
            else
            {
                qry = qry.OrderByDescending( a => a.TimeCardPayPeriod.StartDate );
            }

            gList.DataSource = qry.ToList();
            gList.DataBind();
        }

        #endregion
    }
}