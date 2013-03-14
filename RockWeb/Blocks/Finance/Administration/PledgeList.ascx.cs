﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock;
using Rock.Attribute;
using Rock.Model;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Finance.Administration
{
    [DetailPage]
    public partial class PledgeList : RockBlock
    {
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            gPledges.DataKeyNames = new[] { "id" };
            gPledges.Actions.IsAddEnabled = true;
            //gPledges.Actions.AddClick += gPledges_Add;
            //gPledges.GridRebind += gPledges_GridRebind;

            bool canAddEditDelete = IsUserAuthorized( "Edit" );
            gPledges.Actions.IsAddEnabled = canAddEditDelete;
            gPledges.IsDeleteEnabled = canAddEditDelete;
        }

        protected override void OnLoad( EventArgs e )
        {
            //if ( !Page.IsPostBack )
            //{
            //    BindGrid();
            //}
            
            base.OnLoad( e );
        }

        //private void BindGrid()
        //{
        //    var pledgeService = new PledgeService();
        //    var sortProperty = gPledges.SortProperty;
        //    var pledges = pledgeService.Queryable();

        //    // TODO: Add filter criteria

        //    gPledges.DataSource = sortProperty != null ? pledges.Sort( sortProperty ).ToList() : pledges.OrderBy( p => p.FundId ).ToList();
        //    gPledges.DataBind();
        //}

        public IQueryable<Pledge> GetPledges([QueryString("person")]string personId, [QueryString("fund")]string fundId)
        {
            var pledgeService = new PledgeService();
            var sortProperty = gPledges.SortProperty;
            var pledges = pledgeService.Queryable();
            int pid, fid;

            if ( personId != null && int.TryParse( personId, out pid ) )
            {
                pledges = pledges.Where( p => p.PersonId == pid );
            }

            if ( fundId != null && int.TryParse( fundId, out fid ) )
            {
                pledges = pledges.Where( p => p.FundId == fid );
            }

            return sortProperty != null ? pledges.Sort( sortProperty ) : pledges.OrderBy( p => p.FundId );
        }

        public void DeletePledge( Pledge pledge )
        {
            //throw new NotImplementedException();
        }
    }
}