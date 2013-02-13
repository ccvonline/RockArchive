﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Prayer
{
    [AdditionalActions( new string[] { "Approve" } )]
    [DetailPage( Order = 0 )]
    [IntegerField( 1, "Group Category Id", "-1", null, "Filtering", "The id of a 'top level' Category.  Only prayer requests under this category will be shown." )]
    public partial class PrayerRequestList : Rock.Web.UI.RockBlock
    {
        #region Private BlockType Attributes
        private static readonly string PrayerRequestKeyParameter = "prayerRequestId";
        int _blockInstanceGroupCategoryId = -1;
        protected int? _prayerRequestEntityTypeId = null;
        protected NoteType _noteType;
        bool canAddEditDelete = false;
        bool canApprove = false;
        #endregion

        #region Control Methods

        /// <summary>
        /// Handles the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            Int32.TryParse( GetAttributeValue( "GroupCategoryId" ), out _blockInstanceGroupCategoryId );
            PrayerRequest prayerRequest = new PrayerRequest();
            Type type = prayerRequest.GetType();
            _prayerRequestEntityTypeId = Rock.Web.Cache.EntityTypeCache.GetId( type.FullName );

            BindFilter();

            gPrayerRequests.DataKeyNames = new string[] { "id" };
            gPrayerRequests.Actions.AddClick += gPrayerRequests_Add;
            gPrayerRequests.GridRebind += gPrayerRequests_GridRebind;

            // Block Security and special attributes (RockPage takes care of "View")
            canApprove = IsUserAuthorized( "Approve" );
            canAddEditDelete = IsUserAuthorized( "Edit" );
            gPrayerRequests.Actions.IsAddEnabled = canAddEditDelete;
            gPrayerRequests.IsDeleteEnabled = canAddEditDelete;
        }

        /// <summary>
        /// Handles the <see cref="E:System.Web.UI.Control.Load" /> event.
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

        #region Grid Filter
        /// <summary>
        /// Binds any needed data to the Grid Filter also using the user's stored
        /// preferences.
        /// </summary>
        private void BindFilter()
        {
            int selectedGroupCategoryId = All.Id;

            // Set the selected prayer group category to the block instance attribute unless defaulted to "All".
            if ( _blockInstanceGroupCategoryId != All.Id )
            {
                selectedGroupCategoryId = _blockInstanceGroupCategoryId;
            }
            else
            {
                int.TryParse( rFilter.GetUserPreference( "Group Category" ), out selectedGroupCategoryId );
            }

            CategoryService categoryService = new CategoryService();

            var categories = categoryService.GetByEntityTypeId( _prayerRequestEntityTypeId ).AsQueryable();
            var subCategories = categories;

            ddlGroupCategoryFilter.Items.Clear();
            ddlGroupCategoryFilter.Items.Add( new ListItem( All.Text, All.Id.ToString() ) );

            // Only enable the Prayer Group Category dropdown if the block attribute's "GroupCategoryId" is set to All;
            // otherwise we don't show this first drop down and instead bind the second prayer category dropdown to
            // only categories which are sub-categories of this configured block attribute.
            if ( _blockInstanceGroupCategoryId != All.Id )
            {
                ddlGroupCategoryFilter.Enabled = false;
            }

            // Get only "root" categories (parent is null)
            var prayerGroupCategories = categories.Where( c => c.ParentCategoryId == null );

            foreach ( Category category in prayerGroupCategories.OrderBy( a => a.Name ) )
            {
                ListItem li = new ListItem( category.Name, category.Id.ToString() );
                li.Selected = category.Id == selectedGroupCategoryId;
                ddlGroupCategoryFilter.Items.Add( li );
            }

            // If the first dropdownlist is still set to all, then don't populate the sub list yet.
            if ( selectedGroupCategoryId == All.Id )
            {
                ddlPrayerCategoryFilter.Enabled = false;
            }
            else
            {
                ddlPrayerCategoryFilter.Enabled = true;
                // Bind the correct prayer sub Categories based on the SELECTED prayer group category
                subCategories = subCategories.Where( c => c.ParentCategoryId == selectedGroupCategoryId );

                ddlPrayerCategoryFilter.Items.Clear();
                ddlPrayerCategoryFilter.Items.Add( new ListItem( All.Text, All.Id.ToString() ) );

                foreach ( Category category in subCategories.OrderBy( a => a.Name ) )
                {
                    ListItem li = new ListItem( category.Name, category.Id.ToString() );
                    li.Selected = category.Id.ToString() == rFilter.GetUserPreference( "Prayer Category" );
                    ddlPrayerCategoryFilter.Items.Add( li );
                }
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the ddlGroupCategoryFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlGroupCategoryFilter_TextChanged( object sender, EventArgs e )
        {
            // Reset the selected category if the prayer GROUP category changes.
            ddlPrayerCategoryFilter.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the Apply Filter event for the GridFilter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            // Here we'll only save the Category preferences since the other filters are
            // typically more transient in nature.
            rFilter.SaveUserPreference( "Group Category", ddlGroupCategoryFilter.SelectedValue );
            rFilter.SaveUserPreference( "Prayer Category", ddlPrayerCategoryFilter.SelectedValue );
            BindFilter();
            BindGrid();
        }

        /// <summary>
        /// Handles displaying the stored filter values.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e as DisplayFilterValueArgs (hint: e.Key and e.Value).</param>
        protected void rFilter_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
        {
            switch ( e.Key )
            {
                case "Group Category":
                case "Prayer Category":

                    int categoryId = All.Id;
                    if ( int.TryParse( e.Value, out categoryId ) )
                    {
                        if ( categoryId == All.Id )
                        {
                            e.Value = "[All]";
                        }
                        else
                        {
                            var service = new CategoryService();
                            var category = service.Get( categoryId );
                            if ( category != null )
                            {
                                e.Value = category.Name;
                            }
                        }
                    }
                    break;
            }
        }

        #endregion
        
        #region Prayer Request Grid

        /// <summary>
        /// Binds the grid to a list of Prayer Requests.
        /// </summary>
        private void BindGrid()
        {
            // Get the selected items from the two dropdown lists.
            int selectedGroupCategoryID = All.Id;
            int.TryParse( ddlGroupCategoryFilter.SelectedValue, out selectedGroupCategoryID );

            int selectedPrayerCategoryID = All.Id;
            int.TryParse( ddlPrayerCategoryFilter.SelectedValue, out selectedPrayerCategoryID );

            PrayerRequestService prayerRequestService = new PrayerRequestService();
            SortProperty sortProperty = gPrayerRequests.SortProperty;

            IQueryable<PrayerRequest> prayerRequests = prayerRequestService.Queryable();

            // Filter by prayer GROUP category...
            if ( selectedGroupCategoryID != All.Id )
            {
                prayerRequests = prayerRequestService.GetByCategoryId( _prayerRequestEntityTypeId, selectedGroupCategoryID ).AsQueryable();
            }

            // Filter by prayer category if one is selected...
            if ( selectedPrayerCategoryID != All.Id )
            {
                prayerRequests = prayerRequests.Where( c => c.CategoryId == selectedPrayerCategoryID );
            }

            // Filter by approved/unapproved
            if ( !cbShowApproved.Checked )
            {
                prayerRequests = prayerRequests.Where( a => a.IsApproved == false || !a.IsApproved.HasValue );
            }

            // Filter by EnteredDate
            if ( dtRequestEnteredDateRangeStartDate.SelectedDate != null )
            {
                prayerRequests = prayerRequests.Where( a => a.EnteredDate >= dtRequestEnteredDateRangeStartDate.SelectedDate );
            }
            if ( dtRequestEnteredDateRangeEndDate.SelectedDate != null )
            {
                prayerRequests = prayerRequests.Where( a => a.EnteredDate <= dtRequestEnteredDateRangeEndDate.SelectedDate );
            }

            // Sort by the given property otherwise sort by the EnteredDate
            if ( sortProperty != null )
            {
                gPrayerRequests.DataSource = prayerRequests.Sort( sortProperty ).ToList();
            }
            else
            {
                // TODO Figure out how to tell Grid what Direction and Property it's sorting on
                //sortProperty.Direction = SortDirection.Ascending;
                //sortProperty.Property = "EnteredDate";
                gPrayerRequests.DataSource = prayerRequests.OrderBy( p => p.EnteredDate ).ToList();
            }

            gPrayerRequests.DataBind();
        }

        /// <summary>
        /// Handles the Add event of the gPrayerRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void gPrayerRequests_Add( object sender, EventArgs e )
        {
            NavigateToDetailPage( PrayerRequestKeyParameter, 0 );
        }

        /// <summary>
        /// Handles the Edit event of the gPrayerRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gPrayerRequests_Edit( object sender, RowEventArgs e )
        {
            NavigateToDetailPage( PrayerRequestKeyParameter, (int)e.RowKeyValue );
        }

        /// <summary>
        /// Handles the CheckChanged event of the gPrayerRequests IsApproved field.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gPrayerRequests_CheckChanged( object sender, RowEventArgs e )
        {
            bool failure = true;

            if ( e.RowKeyValue != null )
            {
                PrayerRequestService prayerRequestService = new PrayerRequestService();
                PrayerRequest prayerRequest = prayerRequestService.Get( (int)e.RowKeyValue );

                if ( prayerRequest != null )
                {
                    failure = false;
                    // if it was approved, set it to unapproved... otherwise
                    if ( prayerRequest.IsApproved ?? false )
                    {
                        prayerRequest.IsApproved = false;
                    }
                    else
                    {
                        prayerRequest.IsApproved = true;
                        prayerRequest.ApprovedByPersonId = CurrentPerson.Id;
                        prayerRequest.ApprovedOnDate = DateTime.UtcNow;
                        // reset the flag count only to zero ONLY if it had a value previously.
                        if ( prayerRequest.FlagCount.HasValue && prayerRequest.FlagCount > 0 )
                        {
                            prayerRequest.FlagCount = 0;
                        }
                    }

                    prayerRequestService.Save( prayerRequest, CurrentPersonId );
                }

                BindGrid();
            }

            if ( failure )
            {
                mdGridWarning.Show( "Unable to approve that prayer request", ModalAlertType.Warning );
            }

        }

        /// <summary>
        /// Handles the Delete event of the gPrayerRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gPrayerRequests_Delete( object sender, RowEventArgs e )
        {
            RockTransactionScope.WrapTransaction( () =>
            {
                PrayerRequestService prayerRequestService = new PrayerRequestService();
                PrayerRequest prayerRequest = prayerRequestService.Get( (int)e.RowKeyValue );

                if ( prayerRequest != null )
                {
                    string errorMessage;
                    if ( !prayerRequestService.CanDelete( prayerRequest, out errorMessage ) )
                    {
                        mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                        return;
                    }

                    prayerRequestService.Delete( prayerRequest, CurrentPersonId );
                    prayerRequestService.Save( prayerRequest, CurrentPersonId );
                }
            } );

            BindGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gPrayerRequests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void gPrayerRequests_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }
        
        /// <summary>
        /// Handles disabling the Toggle fields if the user does not have Approval rights.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs" /> instance containing the event data.</param>
        protected void gPrayerRequests_RowDataBound( object sender, GridViewRowEventArgs e )
        {
            if ( canApprove )
                return;

            if ( e.Row.RowType == DataControlRowType.DataRow )
            {
                foreach ( TableCell cell in e.Row.Cells )
                {
                    foreach ( Control c in cell.Controls )
                    {
                        Toggle toggle = c as Toggle;
                        if ( toggle != null )
                        {
                            toggle.Enabled = false;
                        }
                    }
                }
            }
        }

        #endregion
    }
}