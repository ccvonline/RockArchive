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
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Crm.PersonDetail
{
    /// <summary>
    /// Block for adding new families
    /// </summary>
    [DisplayName( "Add Family" )]
    [Category( "CRM > Person Detail" )]
    [Description( "Allows for adding new families." )]

    [GroupLocationTypeField( Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY, "Location Type",
        "The type of location that address should use", false, Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME, "", 0 )]
    [BooleanField( "Nick Name", "Show Nick Name column", "Hide Nick Name column", "Should the Nick Name field be displayed?", false, "", 1 )]
    [BooleanField( "Gender", "Require a gender for each person", "Don't require", "Should Gender be required for each person added?", false, "", 2 )]
    [BooleanField( "Grade", "Require a grade for each child", "Don't require", "Should Grade be required for each child added?", false, "", 3 )]
    [AttributeCategoryField( "Attribute Categories", "The Attribute Categories to display attributes from", true, "Rock.Model.Person", false, "", "", 4 )]
    public partial class AddFamily : Rock.Web.UI.RockBlock
    {
        private bool _requireGender = false;
        private bool _requireGrade = false;
        private int _childRoleId = 0;


        /// <summary>
        /// Gets or sets the index of the current category.
        /// </summary>
        /// <value>
        /// The index of the current category.
        /// </value>
        protected int CurrentCategoryIndex
        {
            get { return ViewState["CurrentCategoryIndex"] as int? ?? 0; }
            set { ViewState["CurrentCategoryIndex"] = value; }
        }

        private List<NewFamilyAttributes> attributeControls = new List<NewFamilyAttributes>();

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            var campusi = new CampusService().Queryable().OrderBy( a => a.Name ).ToList();
            cpCampus.Campuses = campusi;
            cpCampus.Visible = campusi.Any();

            var childRole = new GroupTypeRoleService().Get( new Guid( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_CHILD ) );
            if ( childRole != null )
            {
                _childRoleId = childRole.Id;
            }

            bool.TryParse( GetAttributeValue( "Gender" ), out _requireGender );
            bool.TryParse( GetAttributeValue( "Grade" ), out _requireGrade );

            bool showNickName = false;
            nfmMembers.ShowNickName = bool.TryParse( GetAttributeValue( "NickName" ), out showNickName ) && showNickName;

            lTitle.Text = ("Add Family").FormatAsHtmlTitle(); 
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
                CreateControls( new List<GroupMember>(), false );
                AddFamilyMember();
            }
            else
            {
                // Update the name on attribute panels
                if ( CurrentCategoryIndex == 0 )
                {
                    foreach ( var familyMemberRow in nfmMembers.FamilyMemberRows )
                    {
                        foreach ( var attributeControl in attributeControls )
                        {
                            var attributeRow = attributeControl.AttributesRows.FirstOrDefault( r => r.PersonGuid == familyMemberRow.PersonGuid );
                            if ( attributeRow != null )
                            {
                                attributeRow.PersonName = string.Format( "{0} {1}", familyMemberRow.FirstName, familyMemberRow.LastName );
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Restores the view-state information from a previous user control request that was saved by the <see cref="M:System.Web.UI.UserControl.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object" /> that represents the user control state to be restored.</param>
        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );

            var familyMembers = new List<GroupMember>();
            List<string> jsonStrings = ViewState["FamilyMembers"] as List<string>;
            jsonStrings.ForEach( j => familyMembers.Add( GroupMember.FromJson( j ) ) );
            CreateControls( familyMembers, false );
        }

        /// <summary>
        /// Saves any user control view-state changes that have occurred since the last page postback.
        /// </summary>
        /// <returns>
        /// Returns the user control's current view state. If there is no view state associated with the control, it returns null.
        /// </returns>
        protected override object SaveViewState()
        {
            var groupMembers = new List<string>();
            GetControlData().ForEach( m => groupMembers.Add( m.ToJson() ) );

            ViewState["FamilyMembers"] = groupMembers;
            return base.SaveViewState();
        }

        /// <summary>
        /// Handles the AddFamilyMemberClick event of the nfmMembers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void nfmMembers_AddFamilyMemberClick( object sender, EventArgs e )
        {
            AddFamilyMember();
        }

        /// <summary>
        /// Handles the RoleUpdated event of the familyMemberRow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void familyMemberRow_RoleUpdated( object sender, EventArgs e )
        {
            NewFamilyMembersRow row = sender as NewFamilyMembersRow;
            row.ShowGrade = row.RoleId == _childRoleId;
        }

        /// <summary>
        /// Handles the DeleteClick event of the familyMemberRow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void familyMemberRow_DeleteClick( object sender, EventArgs e )
        {
            NewFamilyMembersRow row = sender as NewFamilyMembersRow;

            foreach ( var attributeControl in attributeControls )
            {
                var attributeRow = attributeControl.AttributesRows.FirstOrDefault( r => r.PersonGuid == row.PersonGuid );
                if ( attributeRow != null )
                {
                    attributeControl.Controls.Remove( attributeRow );
                }
            }

            nfmMembers.Controls.Remove( row );
        }

        private void CreateControls( List<GroupMember> familyMembers, bool setSelection )
        {
            // Load all the attribute controls
            attributeControls.Clear();
            pnlAttributes.Controls.Clear();

            foreach ( string categoryGuid in GetAttributeValue( "AttributeCategories" ).SplitDelimitedValues( false ) )
            {
                Guid guid = Guid.Empty;
                if ( Guid.TryParse( categoryGuid, out guid ) )
                {
                    var category = CategoryCache.Read( guid );
                    if ( category != null )
                    {
                        var attributeControl = new NewFamilyAttributes();
                        attributeControl.ClearRows();
                        pnlAttributes.Controls.Add( attributeControl );
                        attributeControls.Add( attributeControl );
                        attributeControl.ID = "familyAttributes_" + category.Id.ToString();
                        attributeControl.CategoryId = category.Id;

                        foreach ( var attribute in new AttributeService().GetByCategoryId( category.Id ) )
                        {
                            if ( attribute.IsAuthorized( "Edit", CurrentPerson ) )
                            {
                                attributeControl.AttributeList.Add( AttributeCache.Read( attribute ) );
                            }
                        }
                    }
                }
            }

            nfmMembers.ClearRows();

            foreach ( var familyMember in familyMembers )
            {
                var familyMemberRow = new NewFamilyMembersRow();
                nfmMembers.Controls.Add( familyMemberRow );
                familyMemberRow.ID = string.Format( "row_{0}", familyMember.Person.Guid.ToString().Replace( "-", "_" ) );
                familyMemberRow.RoleUpdated += familyMemberRow_RoleUpdated;
                familyMemberRow.DeleteClick += familyMemberRow_DeleteClick;
                familyMemberRow.PersonGuid = familyMember.Person.Guid;
                familyMemberRow.RequireGender = _requireGender;
                familyMemberRow.RequireGrade = _requireGrade;
                familyMemberRow.RoleId = familyMember.GroupRoleId;
                familyMemberRow.ShowGrade = familyMember.GroupRoleId == _childRoleId;

                if ( setSelection )
                {
                    if ( familyMember.Person != null )
                    {
                        familyMemberRow.TitleValueId = familyMember.Person.TitleValueId;
                        familyMemberRow.FirstName = familyMember.Person.FirstName;
                        familyMemberRow.NickName = familyMember.Person.NickName;
                        familyMemberRow.LastName = familyMember.Person.LastName;
                        familyMemberRow.Gender = familyMember.Person.Gender;
                        familyMemberRow.BirthDate = familyMember.Person.BirthDate;
                        familyMemberRow.ConnectionStatusValueId = familyMember.Person.ConnectionStatusValueId;
                    }
                }

                foreach ( var attributeControl in attributeControls )
                {
                    var attributeRow = new NewFamilyAttributesRow();
                    attributeControl.Controls.Add( attributeRow );
                    attributeRow.ID = string.Format( "{0}_{1}", attributeControl.ID, familyMember.Person.Guid );
                    attributeRow.AttributeList = attributeControl.AttributeList;
                    attributeRow.PersonGuid = familyMember.Person.Guid;

                    if ( setSelection )
                    {
                        attributeRow.SetEditValues( familyMember.Person );
                    }
                }
            }

            ShowAttributeCategory( CurrentCategoryIndex );
        }

        private List<GroupMember> GetControlData()
        {
            var familyMembers = new List<GroupMember>();

            foreach ( NewFamilyMembersRow row in nfmMembers.FamilyMemberRows )
            {
                var groupMember = new GroupMember();
                groupMember.Person = new Person();
                groupMember.Person.Guid = row.PersonGuid.Value;
                groupMember.Person.RecordStatusValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_ACTIVE.AsGuid() ).Id;

                if ( row.RoleId.HasValue )
                {
                    groupMember.GroupRoleId = row.RoleId.Value;
                }

                groupMember.Person.TitleValueId = row.TitleValueId;
                groupMember.Person.FirstName = row.FirstName;
                if ( nfmMembers.ShowNickName )
                {
                    groupMember.Person.NickName = row.NickName;
                }

                if (string.IsNullOrWhiteSpace(groupMember.Person.NickName))
                {
                    groupMember.Person.NickName = groupMember.Person.FirstName;
                }

                groupMember.Person.LastName = row.LastName;
                groupMember.Person.Gender = row.Gender;
                groupMember.Person.BirthDate = row.BirthDate;
                groupMember.Person.ConnectionStatusValueId = row.ConnectionStatusValueId;
                //groupMember.Person.GraduationDate = 

                groupMember.Person.Attributes = new Dictionary<string, AttributeCache>();
                groupMember.Person.AttributeValues = new Dictionary<string, List<AttributeValue>>();

                foreach ( var attributeControl in attributeControls )
                {
                    attributeControl.AttributeList.ForEach( a =>
                    {
                        groupMember.Person.Attributes.Add( a.Key, a );
                        groupMember.Person.AttributeValues.Add( a.Key, new List<AttributeValue>() );
                        groupMember.Person.AttributeValues[a.Key].Add( new AttributeValue { AttributeId = a.Id } );
                    } );

                    NewFamilyAttributesRow attributeRow = attributeControl.AttributesRows.FirstOrDefault( r => r.PersonGuid == row.PersonGuid );
                    if ( attributeRow != null )
                    {
                        attributeRow.GetEditValues( groupMember.Person );
                    }
                }

                familyMembers.Add( groupMember );
            }

            return familyMembers;
        }

        private void AddFamilyMember()
        {
            var rows = nfmMembers.FamilyMemberRows;
            var familyMemberGuid = Guid.NewGuid();

            var familyMemberRow = new NewFamilyMembersRow();
            nfmMembers.Controls.Add( familyMemberRow );
            familyMemberRow.ID = string.Format( "row_{0}", familyMemberGuid.ToString().Replace( "-", "_" ) );
            familyMemberRow.RoleUpdated += familyMemberRow_RoleUpdated;
            familyMemberRow.DeleteClick += familyMemberRow_DeleteClick;
            familyMemberRow.PersonGuid = familyMemberGuid;
            familyMemberRow.Gender = Gender.Unknown;
            familyMemberRow.RequireGender = _requireGender;
            familyMemberRow.RequireGrade = _requireGrade;
            familyMemberRow.ValidationGroup = BlockValidationGroup;

            var familyGroupType = GroupTypeCache.GetFamilyGroupType();
            if ( familyGroupType != null && familyGroupType.DefaultGroupRoleId.HasValue )
            {
                familyMemberRow.RoleId = familyGroupType.DefaultGroupRoleId;
                familyMemberRow.ShowGrade = familyGroupType.DefaultGroupRoleId == _childRoleId;
            }
            else
            {
                familyMemberRow.ShowGrade = false;
            }

            if ( rows.Count > 0 )
            {
                familyMemberRow.LastName = rows[0].LastName;
            }

            foreach ( var attributeControl in attributeControls )
            {
                var attributeRow = new NewFamilyAttributesRow();
                attributeControl.Controls.Add( attributeRow );
                attributeRow.ID = string.Format( "{0}_{1}", attributeControl.ID, familyMemberGuid );
                attributeRow.AttributeList = attributeControl.AttributeList;
                attributeRow.PersonGuid = familyMemberGuid;
            }
        }

        protected void btnPrevious_Click( object sender, EventArgs e )
        {
            if ( CurrentCategoryIndex > 0 )
            {
                CurrentCategoryIndex--;
                ShowAttributeCategory( CurrentCategoryIndex );
            }
        }

        protected void btnNext_Click( object sender, EventArgs e )
        {
            if ( Page.IsValid )
            {
                if ( CurrentCategoryIndex < attributeControls.Count )
                {
                    CurrentCategoryIndex++;
                    ShowAttributeCategory( CurrentCategoryIndex );
                }
                else
                {
                    var familyMembers = GetControlData();
                    if ( familyMembers.Any() )
                    {

                        RockTransactionScope.WrapTransaction( () =>
                        {
                            using ( new UnitOfWorkScope() )
                            {
                                var familyGroupType = GroupTypeCache.GetFamilyGroupType();

                                var familyChanges = new List<string>();
                                var familyMemberChanges = new Dictionary<Guid, List<string>>();
                                var familyDemographicChanges = new Dictionary<Guid, List<string>>();

                                if ( familyGroupType != null )
                                {
                                    var groupService = new GroupService();

                                    var groupTypeRoleService = new GroupTypeRoleService();

                                    var familyGroup = new Group();
                                    familyGroup.GroupTypeId = familyGroupType.Id;
                                    
                                    familyChanges.Add("Created");
                                    
                                    familyGroup.Name = familyMembers.FirstOrDefault().Person.LastName + " Family";
                                    History.EvaluateChange( familyChanges, "Name", string.Empty, familyGroup.Name );

                                    int? campusId = cpCampus.SelectedValueAsInt();
                                    if (campusId.HasValue)
                                    {
                                        History.EvaluateChange( familyChanges, "Campus", string.Empty, CampusCache.Read( campusId.Value ).Name );
                                    }
                                    familyGroup.CampusId = campusId;

                                    foreach(var familyMember in familyMembers)
                                    {
                                        var person = familyMember.Person;
                                        if ( person != null )
                                        {
                                            familyGroup.Members.Add( familyMember );

                                            var demographicChanges = new List<string>();
                                            demographicChanges.Add( "Created" );
                                            History.EvaluateChange( demographicChanges, "Record Status", string.Empty,
                                                person.RecordStatusValueId.HasValue ? DefinedValueCache.GetName( person.RecordStatusValueId.Value ) : string.Empty );
                                            History.EvaluateChange( demographicChanges, "Title", string.Empty,
                                                person.TitleValueId.HasValue ? DefinedValueCache.GetName( person.TitleValueId ) : string.Empty );
                                            History.EvaluateChange( demographicChanges, "First Name", string.Empty, person.FirstName);
                                            History.EvaluateChange( demographicChanges, "Nick Name", string.Empty, person.NickName );
                                            History.EvaluateChange( demographicChanges, "Middle Name", string.Empty, person.MiddleName );
                                            History.EvaluateChange( demographicChanges, "Last Name", string.Empty, person.LastName );
                                            History.EvaluateChange( demographicChanges, "Gender", null, person.Gender );
                                            History.EvaluateChange( demographicChanges, "Birth Date", null, person.BirthDate );
                                            History.EvaluateChange( demographicChanges, "Connection Status", string.Empty,
                                                person.ConnectionStatusValueId.HasValue ? DefinedValueCache.GetName( person.ConnectionStatusValueId ) : string.Empty );
                                            History.EvaluateChange( demographicChanges, "Graduation Date", null, person.GraduationDate );
                                            familyDemographicChanges.Add( person.Guid, demographicChanges );

                                            var memberChanges = new List<string>();
                                            string roleName = familyGroupType.Roles[familyMember.GroupRoleId] ?? string.Empty;
                                            History.EvaluateChange( memberChanges, "Role", string.Empty, roleName );
                                            familyMemberChanges.Add( person.Guid, memberChanges );
                                        }
                                    }

                                    if ( !String.IsNullOrWhiteSpace( tbStreet1.Text ) ||
                                         !String.IsNullOrWhiteSpace( tbStreet2.Text ) ||
                                         !String.IsNullOrWhiteSpace( tbCity.Text ) ||
                                         !String.IsNullOrWhiteSpace( tbZip.Text ) )
                                    {
                                        string addressChangeField = "Address";

                                        var groupLocation = new GroupLocation();
                                        var location = new LocationService().Get(
                                            tbStreet1.Text, tbStreet2.Text, tbCity.Text, ddlState.SelectedValue, tbZip.Text );
                                        groupLocation.Location = location;

                                        Guid locationTypeGuid = Guid.Empty;
                                        if ( Guid.TryParse( GetAttributeValue( "LocationType" ), out locationTypeGuid ) )
                                        {
                                            var locationType = Rock.Web.Cache.DefinedValueCache.Read( locationTypeGuid );
                                            if ( locationType != null )
                                            {
                                                addressChangeField = string.Format("{0} Address", locationType.Name);
                                                groupLocation.GroupLocationTypeValueId = locationType.Id;
                                            }
                                        }

                                        familyGroup.GroupLocations.Add( groupLocation );

                                        History.EvaluateChange( familyChanges, addressChangeField, string.Empty, groupLocation.Location.ToString() );
                                    }


                                    groupService.Add( familyGroup, CurrentPersonId );
                                    groupService.Save( familyGroup, CurrentPersonId );

                                    var historyService = new HistoryService();
                                    historyService.SaveChanges( typeof( Group ), Rock.SystemGuid.Category.HISTORY_PERSON_FAMILY_CHANGES.AsGuid(),
                                        familyGroup.Id, familyChanges, CurrentPersonId );

                                    var personService = new PersonService();

                                    foreach ( var groupMember in familyMembers )
                                    {
                                        var person = personService.Get( groupMember.PersonId );
                                        if ( person != null )
                                        {
                                            bool updateRequired = false;
                                            if (!person.Aliases.Any( a => a.AliasPersonId == person.Id))
                                            {
                                                person.Aliases.Add( new PersonAlias { AliasPersonId = person.Id, AliasPersonGuid = person.Guid } );
                                                updateRequired = true;
                                            }
                                            var changes = familyDemographicChanges[person.Guid];
                                            if ( groupMember.GroupRoleId != _childRoleId )
                                            {
                                                person.GivingGroupId = familyGroup.Id;
                                                updateRequired = true;
                                                History.EvaluateChange( changes, "Giving Group", string.Empty, familyGroup.Name );
                                            }

                                            if ( updateRequired )
                                            {
                                                personService.Save( person, CurrentPersonId );
                                            }

                                            foreach ( var attributeControl in attributeControls )
                                            {
                                                foreach ( var attribute in attributeControl.AttributeList )
                                                {
                                                    string attributeValue = person.GetAttributeValue( attribute.Key );
                                                    if ( !string.IsNullOrWhiteSpace( attributeValue ) )
                                                    {
                                                        Rock.Attribute.Helper.SaveAttributeValue( person, attribute, attributeValue, CurrentPersonId );
                                                        attributeValue = attribute.FieldType.Field.FormatValue( null, attributeValue, attribute.QualifierValues, false );
                                                        History.EvaluateChange( changes, attribute.Name, string.Empty, attributeValue );
                                                    }
                                                }
                                            }

                                            historyService.SaveChanges( typeof( Person ), Rock.SystemGuid.Category.HISTORY_PERSON_DEMOGRAPHIC_CHANGES.AsGuid(),
                                                person.Id, changes, CurrentPersonId );

                                            historyService.SaveChanges( typeof( Person ), Rock.SystemGuid.Category.HISTORY_PERSON_FAMILY_CHANGES.AsGuid(),
                                                person.Id, familyMemberChanges[person.Guid], familyGroup.Name, typeof( Group), familyGroup.Id, CurrentPersonId );
                                        }
                                    }
                                }
                            }
                        } );

                        Response.Redirect( string.Format( "~/Person/{0}", familyMembers[0].Person.Id ), false );
                    }

                }
            }

        }

        private void ShowAttributeCategory( int index )
        {
            pnlFamilyData.Visible = ( index == 0 );

            attributeControls.ForEach( c => c.Visible = false );
            if ( index > 0 && attributeControls.Count >= index )
            {
                attributeControls[index - 1].Visible = true;
            }

            btnPrevious.Visible = index > 0;
            btnNext.Text = index < attributeControls.Count ? "Next" : "Finish";
        }
    }
}