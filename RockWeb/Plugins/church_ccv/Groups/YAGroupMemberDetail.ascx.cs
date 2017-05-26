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
using Rock.Web.UI;

using church.ccv.Utility.SystemGuids;
using church.ccv.Utility.Groups;

namespace RockWeb.Plugins.church_ccv.Groups
{
    [DisplayName( "YA Group Member Detail" )]
    [Category( "CCV > Groups" )]
    [Description( "Displays the details of the given Young Adult group member " )]
    [WorkflowTypeField( "OptOut Needs Next Steps Coach", "The workflow to use when opting out a person due to them needing a Next Steps Coach. The Person will be set as the workflow 'Entity' attribute when processing is started.", false, false, "", "" )]
    [WorkflowTypeField( "OptOut Not Attending Group", "The workflow to use when opting out a person due to them not attending group. The Person will be set as the workflow 'Entity' attribute when processing is started.", false, false, "", "" )]
    [WorkflowTypeField( "OptOut No Longer Attends Workflow", "The workflow to use when opting out a person due to them no longer attending CCV. The Person will be set as the workflow 'Entity' attribute when processing is started.", false, false, "", "" )]
    public partial class YAGroupMemberDetail : ToolboxGroupMemberDetail, IDetailBlock
    {
        #region Control Methods

        DefinedTypeCache OptOutReasonDefinedType { get; set; }

        public override Literal MemberName { get { return lPersonName; } }
        public override Literal GroupRole { get { return lGroupRole; } }

        public override Rock.Web.UI.Controls.RockRadioButtonList ActivePendingStatus { get { return rblActivePendingStatus; } }
        public override Rock.Web.UI.Controls.EmailBox EmailAddress { get { return ebEmailAddress; } }
        public override Rock.Web.UI.Controls.PhoneNumberBox MobileNumber { get { return pnMobile; } }
        public override CheckBox SMSEnabled { get { return cbSms; } }
        public override Rock.Web.UI.Controls.RockDropDownList OptOutReason { get { return ddlOptOutReason; } }
        public override Rock.Web.UI.Controls.DatePicker FollowUpDatePicker { get { return dpFollowUpDate; } }
        public override Rock.Web.UI.Controls.RockTextBox ReassignReason { get { return null; } }
        public override Rock.Web.UI.Controls.ImageEditor ProfilePicEditor { get { return imgPhoto; } }

        public override Literal EmailReceipt { get { return lReceipt; } }

        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            OptOutReasonDefinedType = DefinedTypeCache.Read( new Guid( church.ccv.Utility.SystemGuids.DefinedType.NEIGHBORHOOD_OPT_OUT_REASON ) );
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
                ShowDetail( PageParameter( "AdminPersonId" ).AsInteger(), PageParameter( "GroupMemberId" ).AsInteger(), null );
            }
        }

        #endregion

        #region Edit Events

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            SaveGroupMember();
            
            Dictionary<string, string> qryString = new Dictionary<string, string>();
            qryString["GroupId"] = hfGroupId.Value;
            NavigateToParentPage( qryString );
        }

        /// <summary>
        /// Saves the group member.
        /// </summary>
        private void SaveGroupMember()
        {
            if ( Page.IsValid )
            {
                var rockContext = new RockContext();
                
                // load the existing group member
                GroupMemberService groupMemberService = new GroupMemberService( rockContext );

                int groupMemberId = int.Parse( hfGroupMemberId.Value );
                GroupMember groupMember = groupMemberService.Get( groupMemberId );

                // let the base save its stuff
                base.SaveGroupMember( groupMember, rockContext );
                

                // now we only need to save stuff specific to this toolbox

                // set their home phone
                var homeNumberType = Rock.Web.Cache.DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_HOME ) );
                groupMember.Person.UpdatePhoneNumber( homeNumberType.Id, pnHome.CountryCode, pnHome.Text, null, null, rockContext );

                // set their anniversary date
                groupMember.Person.AnniversaryDate = dpAnniversaryDate.SelectedDate;
                
                // using WrapTransaction because there are three Saves
                rockContext.WrapTransaction( () =>
                {
                    if ( groupMember.Id.Equals( 0 ) )
                    {
                        groupMemberService.Add( groupMember );
                    }

                    rockContext.SaveChanges();
                    groupMember.SaveAttributeValues( rockContext );
                } );

                // pass the group to the workflow
                var workflowAttribs = new Dictionary<string, string>();
                workflowAttribs.Add( "Group", groupMember.Group.Guid.ToString() );

                // now handle any opt-out specific behavior
                switch ( ddlOptOutReason.SelectedValue )
                {
                    case church.ccv.Utility.SystemGuids.DefinedValue.NEIGHBORHOOD_OPT_OUT_NOT_ATTENDING_GROUP:
                    {
                        StartWorkflow( "OptOutNotAttendingGroup", groupMember.Person, workflowAttribs, rockContext );
                        break;
                    }

                    case church.ccv.Utility.SystemGuids.DefinedValue.NEIGHBORHOOD_OPT_OUT_NEEDS_NEXT_STEPS_COACH:
                    {
                        StartWorkflow( "OptOutNeedsNextStepsCoach", groupMember.Person, workflowAttribs, rockContext );
                        break;
                    }

                    case church.ccv.Utility.SystemGuids.DefinedValue.NEIGHBORHOOD_OPT_OUT_NO_LONGER_ATTENDING_CCV:
                    {
                        StartWorkflow( "OptOutNoLongerAttendsWorkflow", groupMember.Person, workflowAttribs, rockContext );
                        break;
                    }
                }
            }
        }
        
        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            if ( hfGroupMemberId.Value.Equals( "0" ) )
            {
                // Cancelling on Add.  
                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString["GroupId"] = hfGroupId.Value;
                NavigateToParentPage( qryString );
            }
            else
            {
                // Cancelling on Edit.  Return to Details
                GroupMemberService groupMemberService = new GroupMemberService( new RockContext() );
                GroupMember groupMember = groupMemberService.Get( int.Parse( hfGroupMemberId.Value ) );

                Dictionary<string, string> qryString = new Dictionary<string, string>();
                qryString["GroupId"] = groupMember.GroupId.ToString();
                NavigateToParentPage( qryString );
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="groupMemberId">The group member identifier.</param>
        public void ShowDetail( int groupMemberId )
        {
            // unused
        }

        private bool IsMarried { get; set; }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="adminPersonId">The admin's person identifier.</param>
        /// <param name="groupMemberId">The group member identifier.</param>
        /// <param name="groupId">The group id.</param>
        public void ShowDetail( int adminPersonId, int groupMemberId, int? groupId )
        {
            var rockContext = new RockContext();
            Person adminPerson = new PersonService( rockContext ).Get( adminPersonId );
            GroupMember groupMember = new GroupMemberService( rockContext ).Get( groupMemberId );

            // make sure both exist, otherwise warn one is wrong.
            if ( groupMember == null || adminPerson == null )
            {
                if ( groupMemberId > 0 && adminPersonId > 0 )
                {
                    nbErrorMessage.NotificationBoxType = Rock.Web.UI.Controls.NotificationBoxType.Warning;
                    nbErrorMessage.Title = "Warning";
                    nbErrorMessage.Text = "Group Member or Admin Person not found. Group Member may have been moved to another group or deleted.";
                }
                else
                {
                    nbErrorMessage.NotificationBoxType = Rock.Web.UI.Controls.NotificationBoxType.Danger;
                    nbErrorMessage.Title = "Invalid Request";
                    nbErrorMessage.Text = "An incorrect querystring parameter was used.  Valid GroupMemberId and AdminPersonId parameters are required.";
                }

                pnlEditDetails.Visible = false;
            }
            else
            {
                // call the base show detail (which is an overloaded, not overridden, version)
                ShowDetail( adminPerson, groupMember, OptOutReasonDefinedType, rockContext );

                pnlEditDetails.Visible = true;

                hfGroupId.Value = groupMember.GroupId.ToString();
                hfGroupMemberId.Value = groupMember.Id.ToString();
                hfAdminPersonId.Value = adminPerson.Id.ToString();
                
                // get their home phone number
                var homeNumberType = Rock.Web.Cache.DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_HOME ) );
                var homephone = groupMember.Person.PhoneNumbers.Where( p => p.NumberTypeValueId == homeNumberType.Id ).FirstOrDefault();
                if ( homephone != null )
                {
                    pnHome.Text = homephone.NumberFormatted;
                }
                
                // get their anniversary date
                dpAnniversaryDate.SelectedDate = groupMember.Person.AnniversaryDate;
                
                // see if they're married so we can show / hide the anniversary picker
                IsMarried = false;

                Person spouse = new PersonService( rockContext ).GetSpouse( groupMember.Person );
                if ( spouse != null )
                {
                    IsMarried = true;
                }

                SetControlVisibilities();
            }
        }
        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlOptOutReason control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlOptOutReason_SelectedIndexChanged( object sender, EventArgs e )
        {
            SetControlVisibilities();
        }

        /// <summary>
        /// Sets the control visibilities.
        /// </summary>
        private void SetControlVisibilities()
        {
            // toggle the follow-up date on or off depending on the reason.
            dpFollowUpDate.Visible = ddlOptOutReason.SelectedValue == church.ccv.Utility.SystemGuids.DefinedValue.NEIGHBORHOOD_OPT_OUT_FOLLOW_UP_LATER;

            // show the active / pending status picker if there's NO opt out reason.
            rblActivePendingStatus.Visible = ddlOptOutReason.SelectedValue == string.Empty;
            
            dpAnniversaryDate.Visible = IsMarried == true;
        }
        #endregion
    }
}