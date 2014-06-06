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
using System.Linq;
using System.Web.UI;
using com.ccvonline.Residency.Data;
using com.ccvonline.Residency.Model;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.com_ccvonline.Residency
{
    /// <summary>
    /// 
    /// </summary>
    [LinkedPage("Detail Page")]
    [BooleanField( "Show Add", "", true )]
    [BooleanField( "Show Delete", "", true )]
    public partial class PersonList : RockBlock, ISecondaryBlock
    {
        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            gList.DataKeyNames = new string[] { "id" };
            gList.Actions.ShowAdd = true;
            gList.Actions.AddClick += gList_Add;
            gList.GridRebind += gList_GridRebind;

            // Block Security and special attributes (RockPage takes care of "View")
            bool canAddEditDelete = IsUserAuthorized( Rock.Security.Authorization.EDIT );
            gList.Actions.ShowAdd = canAddEditDelete && this.GetAttributeValue( "ShowAdd" ).AsBoolean();
            gList.IsDeleteEnabled = canAddEditDelete && this.GetAttributeValue( "ShowDelete" ).AsBoolean();
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

        #region Grid Events

        /// <summary>
        /// Handles the Add event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gList_Add( object sender, EventArgs e )
        {
            NavigateToLinkedPage( "DetailPage", "groupMemberId", 0, "groupId", hfGroupId.ValueAsInt() );
        }

        /// <summary>
        /// Handles the Edit event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gList_Edit( object sender, RowEventArgs e )
        {
            NavigateToLinkedPage( "DetailPage", "groupMemberId", e.RowKeyId, "groupId", hfGroupId.ValueAsInt() );
        }

        /// <summary>
        /// Handles the Delete event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gList_Delete( object sender, RowEventArgs e )
        {
            RockTransactionScope.WrapTransaction( () =>
            {
                var groupMemberService = new GroupMemberService();
                int groupMemberId = e.RowKeyId;

                GroupMember groupMember = groupMemberService.Get( groupMemberId );
                if ( groupMember != null )
                {
                    // check if person can be removed from the Group and also check if person can be removed from all the person assigned competencies
                    string errorMessage;
                    if ( !groupMemberService.CanDelete( groupMember, out errorMessage ) )
                    {
                        mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                        return;
                    }

                    var competencyPersonService = new ResidencyService<CompetencyPerson>();
                    var personCompetencyList = competencyPersonService.Queryable().Where( a => a.PersonId.Equals( groupMember.PersonId ) );
                    foreach ( var item in personCompetencyList )
                    {
                        if ( !competencyPersonService.CanDelete( item, out errorMessage ) )
                        {
                            mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                            return;
                        }
                    }

                    // if you made it this far, delete all person's assigned competencies, and finally delete from Group
                    foreach ( var item in personCompetencyList )
                    {
                        competencyPersonService.Delete( item, CurrentPersonId );
                        competencyPersonService.Save( item, CurrentPersonId );
                    }

                    groupMemberService.Delete( groupMember, CurrentPersonId );
                    groupMemberService.Save( groupMember, CurrentPersonId );
                }
            } );

            BindGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gList_GridRebind( object sender, EventArgs e )
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
            var residencyGroupMemberService = new ResidencyService<Rock.Model.GroupMember>();

            int residencyGroupId = PageParameter( "groupId" ).AsInteger() ?? 0;
            hfGroupId.SetValue( residencyGroupId );

            var residencyGroupMemberList = residencyGroupMemberService.Queryable( "Person" )
                .Where( a => a.GroupId.Equals( residencyGroupId ) ).ToList();

            var competencyPersonService = new ResidencyService<CompetencyPerson>();
            var competencyPersonQry = competencyPersonService.Queryable( "Competency,CompetencyPersonProjects" ).GroupBy( a => a.PersonId );

            var competencyPersonProjectQry = new ResidencyService<CompetencyPersonProject>().Queryable().GroupBy( a => a.CompetencyPerson.PersonId)
                .Select( x => new
            {
                PersonId = x.Key,
                MinAssessmentCountTotal = x.Sum(nn => nn.MinAssessmentCount ?? nn.Project.MinAssessmentCountDefault),
                CompletedProjectAssessmentsTotal = x.Sum( dd => dd.CompetencyPersonProjectAssessments.Where( nn => nn.AssessmentDateTime != null).Count())
            } ).ToList();

            var groupMemberCompetencies = from groupMember in residencyGroupMemberList
                                          join competencyList in competencyPersonQry on groupMember.PersonId
                                          equals competencyList.Key into groupJoin
                                          from qryResult in groupJoin.DefaultIfEmpty()
                                          select new
                                          {
                                              GroupMember = groupMember,
                                              ResidentCompentencies = qryResult != null ? qryResult.ToList() : null
                                          };

            var dataResult = groupMemberCompetencies.Select( a => new
            {
                Id = a.GroupMember.Id,
                FullName = a.GroupMember.Person.FullName,
                CompetencyCount = a.ResidentCompentencies == null ? 0 : a.ResidentCompentencies.Count(),
                CompletedProjectAssessmentsTotal = competencyPersonProjectQry.Where( g => g.PersonId == a.GroupMember.PersonId ).Select( g => g.CompletedProjectAssessmentsTotal ).FirstOrDefault(),
                MinAssessmentCount = competencyPersonProjectQry.Where( g => g.PersonId == a.GroupMember.PersonId ).Select( g => g.MinAssessmentCountTotal ?? 0 ).FirstOrDefault()
            } );

            SortProperty sortProperty = gList.SortProperty;

            if ( sortProperty != null )
            {
                gList.DataSource = dataResult.AsQueryable().Sort( sortProperty ).ToList();
            }
            else
            {
                gList.DataSource = dataResult.OrderBy( s => s.FullName ).ToList();
            }

            gList.DataBind();
        }

        #endregion

        #region ISecondaryBlock

        /// <summary>
        /// Hook so that other blocks can set the visibility of all ISecondaryBlocks on it's page.
        /// </summary>
        /// <param name="dimmed">if set to <c>true</c> [dimmed].</param>
        public void SetVisible( bool visible )
        {
            gList.Visible = visible;
        }

        #endregion
    }
}