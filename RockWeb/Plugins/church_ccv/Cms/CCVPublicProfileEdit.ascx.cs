﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
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
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.church_ccv.Cms
{
    /// <summary>
    /// The main Person Profile block the main information about a peron 
    /// </summary>
    [DisplayName( "CCV Public Profile Edit" )]
    [Category( "CMS" )]
    [Description( "Public block for users to manage their accounts" )]

    [BooleanField( "Disable Name Edit", "Whether the First and Last Names can be edited.", false, order: 0 )]
    [BooleanField( "View Only", "Should people be prevented from editing thier profile or family records?", false, "", 1 )]
    [BooleanField( "Show Family Members", "Whether family members are shown or not.", true, order: 2 )]
    [GroupLocationTypeField( Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY, "Address Type",
        "The type of address to be displayed / edited.", false, Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME, "", order: 3 )]
    [DefinedValueField( Rock.SystemGuid.DefinedType.PERSON_PHONE_TYPE, "Phone Numbers", "The types of phone numbers to display / edit.", true, true, Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_HOME, order: 4 )]
    [BooleanField( "Show Communication Preference", "Show the communication preference and allow it to be edited", true, order: 5 )]
    [LinkedPage( "Workflow Launch Page", "Page used to launch the workflow to make a profile change request", false, order: 6 )]
    [TextField( "Request Changes Text", "The text to use for the request changes button (only displayed if there is a 'Workflow Launch Page' configured).", false, "Request Additional Changes", "", 7 )]
    [AttributeField( Rock.SystemGuid.EntityType.GROUP, "GroupTypeId", Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY, "Family Attributes", "The family attributes that should be displayed / edited.", false, true, order: 8 )]
    [AttributeField( Rock.SystemGuid.EntityType.PERSON, "Person Attributes (adults)", "The person attributes that should be displayed / edited for adults.", false, true, order: 9 )]
    [AttributeField( Rock.SystemGuid.EntityType.PERSON, "Person Attributes (children)", "The person attributes that should be displayed / edited for children.", false, true, order: 10 )]
    [DefinedValueField( Rock.SystemGuid.DefinedType.PERSON_CONNECTION_STATUS, "Default Connection Status", "The default connection status that is given to new family members.", true, false, Rock.SystemGuid.DefinedValue.PERSON_CONNECTION_STATUS_WEB_PROSPECT, "", 11 )]
    [DefinedValueField( Rock.SystemGuid.DefinedType.PERSON_RECORD_STATUS, "Default Record Status", "The default record status that is given to new family members.", true, false, Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_PENDING, "", 12 )]

    [DataViewField( "Photo Edit Blacklist", "A data view of people that are not allowed to edit their photo.", false, "", "Rock.Model.Person", "Photo Editing", 0 )]
    [TextField( "Blacklist Message", "This photo cannot be edited.", true, "This photo cannot be edited.", "Photo Editing", 1 )]
    public partial class CCVPublicProfileEdit : RockBlock
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Role Type. Used to help in loading Attribute panel
        /// </summary>
        protected int? RoleType
        {
            get { return ViewState["RoleType"] as int? ?? null; }
            set { ViewState["RoleType"] = value; }
        }

        bool _canEdit = false;

        List<int> _photoBlacklistPersonIds;

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            ScriptManager.RegisterStartupScript( ddlGradePicker, ddlGradePicker.GetType(), "grade-selection-" + BlockId.ToString(), ddlGradePicker.GetJavascriptForYearPicker( ypGraduation ), true );
            ddlTitle.BindToDefinedType( DefinedTypeCache.Read( new Guid( Rock.SystemGuid.DefinedType.PERSON_TITLE ) ), true );
            ddlSuffix.BindToDefinedType( DefinedTypeCache.Read( new Guid( Rock.SystemGuid.DefinedType.PERSON_SUFFIX ) ), true );
            RockPage.AddCSSLink( ResolveRockUrl( "~/Styles/fluidbox.css" ) );
            RockPage.AddScriptLink( ResolveRockUrl( "~/Scripts/imagesloaded.min.js" ) );
            RockPage.AddScriptLink( ResolveRockUrl( "~/Scripts/jquery.fluidbox.min.js" ) );

            _canEdit = !GetAttributeValue( "ViewOnly" ).AsBoolean();
            lbEditPerson.Visible = _canEdit;
            lbAddGroupMember.Visible = _canEdit;

            lbRequestChanges.Text = GetAttributeValue( "RequestChangesText" );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );
            if ( CurrentPerson != null )
            {
                Guid? dataViewGuid = GetAttributeValue( "PhotoEditBlacklist" ).AsGuidOrNull();
                if ( dataViewGuid.HasValue )
                {
                    var dataView = new DataViewService( new RockContext() ).Get( dataViewGuid.Value );
                    if ( dataView != null )
                    {
                        var errors = new List<string>();
                        var qry = dataView.GetQuery( null, 30, out errors );
                        if ( qry != null )
                        {
                            _photoBlacklistPersonIds = qry.Select( q => q.Id ).ToList();
                        }
                    }
                }

                if ( !Page.IsPostBack )
                {
                    BindFamilies();
                }
                else
                {
                    var rockContext = new RockContext();
                    var group = new GroupService( rockContext ).Get( ddlGroup.SelectedValueAsId().Value );
                    var person = new PersonService( rockContext ).Get( hfPersonId.ValueAsInt() );
                    if ( person != null && group != null )
                    {
                        // Person Attributes
                        var displayedAttributeGuids = GetPersonAttributeGuids( person.Id );
                        if ( !displayedAttributeGuids.Any() || person.Id == 0 )
                        {
                            pnlPersonAttributes.Visible = false;
                        }
                        else
                        {
                            pnlPersonAttributes.Visible = true;
                            DisplayEditAttributes( person, displayedAttributeGuids, phPersonAttributes, pnlPersonAttributes, false );
                        }

                        // Family Attributes
                        if ( person.Id == CurrentPerson.Id )
                        {
                            List<Guid> familyAttributeGuidList = GetAttributeValue( "FamilyAttributes" ).SplitDelimitedValues().AsGuidList();
                            if ( familyAttributeGuidList.Any() )
                            {
                                pnlFamilyAttributes.Visible = true;
                                DisplayEditAttributes( group, familyAttributeGuidList, phFamilyAttributes, pnlFamilyAttributes, false );
                            }
                            else
                            {
                                pnlFamilyAttributes.Visible = false;
                            }
                        }
                    }
                    if ( person == null && RoleType != null )
                    {
                        DisplayPersonAttributeOnRoleType( RoleType );
                    }
                }
            }
            else
            {
                pnlView.Visible = false;
                pnlEdit.Visible = false;
                nbNotAuthorized.Visible = true;
            }
        }

        private void BindFamilies()
        {
            ddlGroup.DataSource = CurrentPerson.GetFamilies().ToList();
            ddlGroup.DataBind();
            ShowDetail();
        }

        #endregion

        #region Events

        #region View Events

        /// <summary>
        /// Handles the Click event of the lbEditPerson control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbEditPerson_Click( object sender, EventArgs e )
        {
            ShowEditPersonDetails( CurrentPerson.Id );
        }

        /// <summary>
        /// Handles the Click event of the lbMoved control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbMoved_Click( object sender, EventArgs e )
        {
            if ( !string.IsNullOrWhiteSpace( acAddress.Street1 ) )
            {
                hfStreet1.Value = acAddress.Street1;
                hfStreet2.Value = acAddress.Street2;
                hfCity.Value = acAddress.City;
                hfState.Value = acAddress.State;
                hfPostalCode.Value = acAddress.PostalCode;
                hfCountry.Value = acAddress.Country;

                Location currentAddress = new Location();
                acAddress.GetValues( currentAddress );
                lPreviousAddress.Text = string.Format( "<strong>Previous Address</strong><br />{0}", currentAddress.FormattedHtmlAddress );

                acAddress.Street1 = string.Empty;
                acAddress.Street2 = string.Empty;
                acAddress.PostalCode = string.Empty;
                acAddress.City = string.Empty;

                cbIsMailingAddress.Checked = true;
                cbIsPhysicalAddress.Checked = true;
            }
        }

        /// <summary>
        /// Handles the Click event of the lbRequestChanges control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbRequestChanges_Click( object sender, EventArgs e )
        {
            NavigateToLinkedPage( "WorkflowLaunchPage" );
        }

        /// <summary>
        /// Handles the ItemCommand event of the rptGroupMembers control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="RepeaterCommandEventArgs"/> instance containing the event data.</param>
        protected void rptGroupMembers_ItemCommand( object source, RepeaterCommandEventArgs e )
        {
            int personId = e.CommandArgument.ToString().AsInteger();
            ShowEditPersonDetails( personId );
        }

        /// <summary>
        /// Handles the Click event of the lbAddGroupMember control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbAddGroupMember_Click( object sender, EventArgs e )
        {
            if ( ddlGroup.SelectedValueAsId().HasValue )
            {
                ShowEditPersonDetails( 0 );
            }
        }

        /// <summary>
        /// Handles the ItemDataBound event of the rptGroupMembers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void rptGroupMembers_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            var rockContext = new RockContext();
            var groupMemberService = new GroupMemberService( rockContext );
            var attributeValueService = new AttributeValueService( rockContext );

            var groupMember = e.Item.DataItem as GroupMember;
            var person = groupMember.Person;
            var lGroupMemberImage = e.Item.FindControl( "lGroupMemberImage" ) as Literal;
            var lGroupMemberName = e.Item.FindControl( "lGroupMemberName" ) as Literal;
            var lGroupMemberEmail = e.Item.FindControl( "lGroupMemberEmail" ) as Literal;
            var lAge = e.Item.FindControl( "lAge" ) as Literal;
            var lGender = e.Item.FindControl( "lGender" ) as Literal;
            var lMaritalStatus = e.Item.FindControl( "lMaritalStatus" ) as Literal;
            var lGrade = e.Item.FindControl( "lGrade" ) as Literal;
            var rptGroupMemberPhones = e.Item.FindControl( "rptGroupMemberPhones" ) as Repeater;
            var rptGroupMemberAttributes = e.Item.FindControl( "rptGroupMemberAttributes" ) as Repeater;
            var lbEditGroupMember = e.Item.FindControl( "lbEditGroupMember" ) as LinkButton;

            var lBaptismPhoto = e.Item.FindControl( "lBaptismPhoto" ) as Literal;
            var lCertificate = e.Item.FindControl( "lCertificate" ) as Literal;

            var lLeaderBaptismPhoto = e.Item.FindControl( "lLeaderBaptismPhoto" ) as Literal;
            var lLeaderCertificate = e.Item.FindControl( "lLeaderCertificate" ) as Literal;

            // Load persons Attributes
            person.LoadAttributes();

            if ( lbEditGroupMember != null )
            {
                lbEditGroupMember.Visible = _canEdit;
            }

            // Get Current Person Details (baptism photo, baptized at CCV, baptism date)
            var hasBaptismPhoto = person.GetAttributeValues( "BaptismPhoto" );
            var isBaptizedHere = person.AttributeValues["BaptizedHere"];

            // Assign Head of House baptism photo
            if ( hasBaptismPhoto != null && hasBaptismPhoto.Count > 0 )
            {
                lBaptismPhoto.Text = string.Format( "<a href='/baptismdashboard?display=photo&paguid={0}'><div class='fa fa-picture-o nextstep-modal-baptism-icon baptism-profile-icons'></div></a>", person.PrimaryAlias.Guid );
            }
            else
            {
                lBaptismPhoto.Text = "";
            }

            // Assign Head of House baptism certificate 
            if ( isBaptizedHere != null && isBaptizedHere.ToString() == "Yes" )
            {
                lCertificate.Text = string.Format( "<a href='/baptismdashboard?display=certificate&paguid={0}'><div class='mdi mdi-certificate nextstep-modal-baptism-icon baptism-profile-icons'></div></a>", person.PrimaryAlias.Guid );
            }
            else
            {
                lCertificate.Text = "";
            }

            // Setup Image
            string imgTag = Rock.Model.Person.GetPersonPhotoImageTag( person );
            if ( person.PhotoId.HasValue )
            {
                lGroupMemberImage.Text = string.Format( "<a href='{0}'>{1}</a>", person.PhotoUrl, imgTag );
            }
            else
            {
                lGroupMemberImage.Text = imgTag;
            }

            // Person Info
            lGroupMemberName.Text = person.FullName;
            lGroupMemberEmail.Text = person.Email;
            if ( person.BirthDate.HasValue )
            {
                var formattedAge = person.FormatAge();
                if ( formattedAge.IsNotNullOrWhitespace() )
                {
                    formattedAge += " old";
                }

                lAge.Text = string.Format( "{0} <small>({1})</small><br/>", formattedAge, ( person.BirthYear.HasValue && person.BirthYear != DateTime.MinValue.Year ) ? person.BirthDate.Value.ToShortDateString() : person.BirthDate.Value.ToMonthDayString() );
            }

            lGender.Text = person.Gender != Gender.Unknown ? person.Gender.ToString() : string.Empty;
            lGrade.Text = person.GradeFormatted;
            lMaritalStatus.Text = person.MaritalStatusValueId.DefinedValue();
            if ( person.AnniversaryDate.HasValue )
            {
                lMaritalStatus.Text += string.Format( " {0} yrs <small>({1})</small>", person.AnniversaryDate.Value.Age(), person.AnniversaryDate.Value.ToMonthDayString() );
            }

            // Contact Info
            if ( person.PhoneNumbers != null )
            {
                var selectedPhoneTypeGuids = GetAttributeValue( "PhoneNumbers" ).Split( ',' ).AsGuidList();
                rptGroupMemberPhones.DataSource = person.PhoneNumbers.Where( pn => selectedPhoneTypeGuids.Contains( pn.NumberTypeValue.Guid ) ).ToList();
                rptGroupMemberPhones.DataBind();
            }

            // Person Attributes
            List<Guid> attributeGuidList = new List<Guid>();
            var adultGuid = Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT.AsGuid();
            var childGuid = Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_CHILD.AsGuid();

            if ( groupMember.GroupRole.Guid == adultGuid )
            {
                attributeGuidList = GetAttributeValue( "PersonAttributes(adults)" ).SplitDelimitedValues().AsGuidList();
            }
            else
            {
                attributeGuidList = GetAttributeValue( "PersonAttributes(children)" ).SplitDelimitedValues().AsGuidList();
            }

            person.LoadAttributes();
            rptGroupMemberAttributes.DataSource = person.Attributes.Where( a =>
             attributeGuidList.Contains( a.Value.Guid ) )
            .Select( a => new
            {
                Name = a.Value.Name,
                Value = a.Value.FieldType.Field.FormatValue( null, a.Value.EntityTypeId, person.Id, person.GetAttributeValue( a.Key ), a.Value.QualifierValues, a.Value.FieldType.Class == typeof( Rock.Field.Types.ImageFieldType ).FullName )
            } )
            .OrderBy( av => av.Name )
            .ToList()
            .Where( av => !String.IsNullOrWhiteSpace( av.Value ) );
            rptGroupMemberAttributes.DataBind();
        }

        #endregion

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            if ( ddlGroup.SelectedValueAsId().HasValue )
            {
                var group = new GroupService( rockContext ).Get( ddlGroup.SelectedValueAsId().Value );
                if ( group != null )
                {
                    rockContext.WrapTransaction( () =>
                    {
                        var personService = new PersonService( rockContext );

                        var personId = hfPersonId.Value.AsInteger();
                        if ( personId == 0 )
                        {
                            var groupMemberService = new GroupMemberService( rockContext );
                            var groupMember = new GroupMember() { Person = new Person(), Group = group, GroupId = group.Id };
                            groupMember.Person.TitleValueId = ddlTitle.SelectedValueAsId();
                            groupMember.Person.FirstName = tbFirstName.Text;
                            groupMember.Person.NickName = tbNickName.Text;
                            groupMember.Person.LastName = tbLastName.Text;
                            groupMember.Person.SuffixValueId = ddlSuffix.SelectedValueAsId();
                            groupMember.Person.Gender = rblGender.SelectedValueAsEnum<Gender>();
                            DateTime? birthdate = bpBirthDay.SelectedDate;
                            if ( birthdate.HasValue )
                            {
                                // If setting a future birthdate, subtract a century until birthdate is not greater than today.
                                var today = RockDateTime.Today;
                                while ( birthdate.Value.CompareTo( today ) > 0 )
                                {
                                    birthdate = birthdate.Value.AddYears( -100 );
                                }
                            }

                            groupMember.Person.SetBirthDate( birthdate );
                            if ( ddlGradePicker.Visible )
                            {
                                groupMember.Person.GradeOffset = ddlGradePicker.SelectedValueAsInt();
                            }

                            var role = group.GroupType.Roles.Where( r => r.Id == ( rblRole.SelectedValueAsInt() ?? 0 ) ).FirstOrDefault();
                            if ( role != null )
                            {
                                groupMember.GroupRole = role;
                                groupMember.GroupRoleId = role.Id;
                            }

                            groupMember.Person.ConnectionStatusValueId = DefinedValueCache.Read( GetAttributeValue( "DefaultConnectionStatus" ).AsGuid() ).Id;
                            groupMember.Person.RecordStatusValueId = DefinedValueCache.Read( GetAttributeValue( "DefaultRecordStatus" ).AsGuid() ).Id;

                            if ( groupMember.GroupRole.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT.AsGuid() )
                            {
                                groupMember.Person.GivingGroupId = group.Id;
                            }

                            groupMember.Person.IsEmailActive = true;
                            groupMember.Person.EmailPreference = EmailPreference.EmailAllowed;
                            groupMember.Person.RecordTypeValueId = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_TYPE_PERSON.AsGuid() ).Id;

                            groupMemberService.Add( groupMember );
                            rockContext.SaveChanges();
                            personId = groupMember.PersonId;
                        }

                        var person = personService.Get( personId );
                        if ( person != null )
                        {
                            int? orphanedPhotoId = null;
                            if ( person.PhotoId != imgPhoto.BinaryFileId )
                            {
                                orphanedPhotoId = person.PhotoId;
                                person.PhotoId = imgPhoto.BinaryFileId;
                            }

                            person.TitleValueId = ddlTitle.SelectedValueAsInt();
                            person.FirstName = tbFirstName.Text;
                            person.NickName = tbNickName.Text;
                            person.LastName = tbLastName.Text;
                            person.SuffixValueId = ddlSuffix.SelectedValueAsInt();

                            var birthMonth = person.BirthMonth;
                            var birthDay = person.BirthDay;
                            var birthYear = person.BirthYear;

                            var birthday = bpBirthDay.SelectedDate;
                            if ( birthday.HasValue )
                            {
                                // If setting a future birthdate, subtract a century until birthdate is not greater than today.
                                var today = RockDateTime.Today;
                                while ( birthday.Value.CompareTo( today ) > 0 )
                                {
                                    birthday = birthday.Value.AddYears( -100 );
                                }

                                person.BirthMonth = birthday.Value.Month;
                                person.BirthDay = birthday.Value.Day;
                                if ( birthday.Value.Year != DateTime.MinValue.Year )
                                {
                                    person.BirthYear = birthday.Value.Year;
                                }
                                else
                                {
                                    person.BirthYear = null;
                                }
                            }
                            else
                            {
                                person.SetBirthDate( null );
                            }

                            if ( ddlGradePicker.Visible )
                            {
                                int? graduationYear = null;
                                if ( ypGraduation.SelectedYear.HasValue )
                                {
                                    graduationYear = ypGraduation.SelectedYear.Value;
                                }
                                person.GraduationYear = graduationYear;
                            }

                            person.Gender = rblGender.SelectedValue.ConvertToEnum<Gender>();

                            var phoneNumberTypeIds = new List<int>();

                            bool smsSelected = false;

                            foreach ( RepeaterItem item in rContactInfo.Items )
                            {
                                HiddenField hfPhoneType = item.FindControl( "hfPhoneType" ) as HiddenField;
                                PhoneNumberBox pnbPhone = item.FindControl( "pnbPhone" ) as PhoneNumberBox;
                                CheckBox cbUnlisted = item.FindControl( "cbUnlisted" ) as CheckBox;
                                CheckBox cbSms = item.FindControl( "cbSms" ) as CheckBox;

                                if ( hfPhoneType != null &&
                                    pnbPhone != null &&
                                    cbSms != null &&
                                    cbUnlisted != null )
                                {
                                    if ( !string.IsNullOrWhiteSpace( PhoneNumber.CleanNumber( pnbPhone.Number ) ) )
                                    {
                                        int phoneNumberTypeId;
                                        if ( int.TryParse( hfPhoneType.Value, out phoneNumberTypeId ) )
                                        {
                                            var phoneNumber = person.PhoneNumbers.FirstOrDefault( n => n.NumberTypeValueId == phoneNumberTypeId );
                                            string oldPhoneNumber = string.Empty;
                                            if ( phoneNumber == null )
                                            {
                                                phoneNumber = new PhoneNumber { NumberTypeValueId = phoneNumberTypeId };
                                                person.PhoneNumbers.Add( phoneNumber );
                                            }
                                            else
                                            {
                                                oldPhoneNumber = phoneNumber.NumberFormattedWithCountryCode;
                                            }

                                            phoneNumber.CountryCode = PhoneNumber.CleanNumber( pnbPhone.CountryCode );
                                            phoneNumber.Number = PhoneNumber.CleanNumber( pnbPhone.Number );

                                            // Only allow one number to have SMS selected
                                            if ( smsSelected )
                                            {
                                                phoneNumber.IsMessagingEnabled = false;
                                            }
                                            else
                                            {
                                                phoneNumber.IsMessagingEnabled = cbSms.Checked;
                                                smsSelected = cbSms.Checked;
                                            }

                                            phoneNumber.IsUnlisted = cbUnlisted.Checked;
                                            phoneNumberTypeIds.Add( phoneNumberTypeId );
                                        }
                                    }
                                }
                            }

                            // Remove any blank numbers
                            var phoneNumberService = new PhoneNumberService( rockContext );
                            foreach ( var phoneNumber in person.PhoneNumbers
                                .Where( n => n.NumberTypeValueId.HasValue && !phoneNumberTypeIds.Contains( n.NumberTypeValueId.Value ) )
                                .ToList() )
                            {
                                person.PhoneNumbers.Remove( phoneNumber );
                                phoneNumberService.Delete( phoneNumber );
                            }

                            person.Email = tbEmail.Text.Trim();
                            person.EmailPreference = rblEmailPreference.SelectedValue.ConvertToEnum<EmailPreference>();
                            person.CommunicationPreference = rblCommunicationPreference.SelectedValueAsEnum<CommunicationType>();

                            person.LoadAttributes();
                            Rock.Attribute.Helper.GetEditValues( phPersonAttributes, person );

                            if ( person.IsValid )
                            {
                                if ( rockContext.SaveChanges() > 0 )
                                {
                                    if ( orphanedPhotoId.HasValue )
                                    {
                                        BinaryFileService binaryFileService = new BinaryFileService( rockContext );
                                        var binaryFile = binaryFileService.Get( orphanedPhotoId.Value );
                                        if ( binaryFile != null )
                                        {
                                            // marked the old images as IsTemporary so they will get cleaned up later
                                            binaryFile.IsTemporary = true;
                                            rockContext.SaveChanges();
                                        }
                                    }

                                    // if they used the ImageEditor, and cropped it, the uncropped file is still in BinaryFile. So clean it up
                                    if ( imgPhoto.CropBinaryFileId.HasValue )
                                    {
                                        if ( imgPhoto.CropBinaryFileId != person.PhotoId )
                                        {
                                            BinaryFileService binaryFileService = new BinaryFileService( rockContext );
                                            var binaryFile = binaryFileService.Get( imgPhoto.CropBinaryFileId.Value );
                                            if ( binaryFile != null && binaryFile.IsTemporary )
                                            {
                                                string errorMessage;
                                                if ( binaryFileService.CanDelete( binaryFile, out errorMessage ) )
                                                {
                                                    binaryFileService.Delete( binaryFile );
                                                    rockContext.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                                person.SaveAttributeValues();

                                // save family information
                                if ( pnlAddress.Visible )
                                {
                                    Guid? familyGroupTypeGuid = Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuidOrNull();
                                    if ( familyGroupTypeGuid.HasValue )
                                    {
                                        var familyGroup = new GroupService( rockContext )
                                            .Queryable()
                                            .Where( f =>
                                                f.GroupType.Guid == familyGroupTypeGuid.Value &&
                                                f.Members.Any( m => m.PersonId == person.Id ) )
                                            .FirstOrDefault();
                                        if ( familyGroup != null )
                                        {
                                            Guid? addressTypeGuid = GetAttributeValue( "AddressType" ).AsGuidOrNull();
                                            if ( addressTypeGuid.HasValue )
                                            {
                                                var groupLocationService = new GroupLocationService( rockContext );

                                                var dvHomeAddressType = DefinedValueCache.Read( addressTypeGuid.Value );
                                                var familyAddress = groupLocationService.Queryable().Where( l => l.GroupId == familyGroup.Id && l.GroupLocationTypeValueId == dvHomeAddressType.Id ).FirstOrDefault();
                                                if ( familyAddress != null && string.IsNullOrWhiteSpace( acAddress.Street1 ) )
                                                {
                                                    // delete the current address
                                                    groupLocationService.Delete( familyAddress );
                                                    rockContext.SaveChanges();
                                                }
                                                else
                                                {
                                                    if ( !string.IsNullOrWhiteSpace( acAddress.Street1 ) )
                                                    {
                                                        if ( familyAddress == null )
                                                        {
                                                            familyAddress = new GroupLocation();
                                                            groupLocationService.Add( familyAddress );
                                                            familyAddress.GroupLocationTypeValueId = dvHomeAddressType.Id;
                                                            familyAddress.GroupId = familyGroup.Id;
                                                            familyAddress.IsMailingLocation = true;
                                                            familyAddress.IsMappedLocation = true;
                                                        }
                                                        else if ( hfStreet1.Value != string.Empty )
                                                        {
                                                            // user clicked move so create a previous address
                                                            var previousAddress = new GroupLocation();
                                                            groupLocationService.Add( previousAddress );

                                                            var previousAddressValue = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_PREVIOUS.AsGuid() );
                                                            if ( previousAddressValue != null )
                                                            {
                                                                previousAddress.GroupLocationTypeValueId = previousAddressValue.Id;
                                                                previousAddress.GroupId = familyGroup.Id;

                                                                Location previousAddressLocation = new Location();
                                                                previousAddressLocation.Street1 = hfStreet1.Value;
                                                                previousAddressLocation.Street2 = hfStreet2.Value;
                                                                previousAddressLocation.City = hfCity.Value;
                                                                previousAddressLocation.State = hfState.Value;
                                                                previousAddressLocation.PostalCode = hfPostalCode.Value;
                                                                previousAddressLocation.Country = hfCountry.Value;

                                                                previousAddress.Location = previousAddressLocation;
                                                            }
                                                        }

                                                        familyAddress.IsMailingLocation = cbIsMailingAddress.Checked;
                                                        familyAddress.IsMappedLocation = cbIsPhysicalAddress.Checked;

                                                        var loc = new Location();
                                                        acAddress.GetValues( loc );

                                                        familyAddress.Location = new LocationService( rockContext ).Get(
                                                            loc.Street1, loc.Street2, loc.City, loc.State, loc.PostalCode, loc.Country, familyGroup, true );

                                                        // since there can only be one mapped location, set the other locations to not mapped
                                                        if ( familyAddress.IsMappedLocation )
                                                        {
                                                            var groupLocations = groupLocationService.Queryable()
                                                                .Where( l => l.GroupId == familyGroup.Id && l.Id != familyAddress.Id ).ToList();

                                                            foreach ( var grouplocation in groupLocations )
                                                            {
                                                                grouplocation.IsMappedLocation = false;
                                                            }
                                                        }

                                                        rockContext.SaveChanges();
                                                    }
                                                }
                                            }

                                            familyGroup.LoadAttributes();
                                            Rock.Attribute.Helper.GetEditValues( phFamilyAttributes, familyGroup );
                                            familyGroup.SaveAttributeValues();
                                        }
                                    }
                                }
                            }
                        }
                    } );

                    NavigateToCurrentPage();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            ShowDetail();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlGroup control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlGroup_SelectedIndexChanged( object sender, EventArgs e )
        {
            ShowDetail();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the rblRole control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void rblRole_SelectedIndexChanged( object sender, EventArgs e )
        {
            var selectedId = rblRole.SelectedValueAsId();
            DisplayPersonAttributeOnRoleType( selectedId );
            RoleType = selectedId;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shows the detail.
        /// </summary>
        private void ShowDetail()
        {
            var rockContext = new RockContext();
            var groupMemberService = new GroupMemberService( rockContext );
            var attributeValueService = new AttributeValueService( rockContext );

            if ( CurrentPerson != null )
            {
                var personId = CurrentPerson.Id;

                // Setup Image
                string imgTag = Rock.Model.Person.GetPersonPhotoImageTag( CurrentPerson );
                if ( CurrentPerson.PhotoId.HasValue )
                {
                    lImage.Text = string.Format( "<a href='{0}'>{1}</a>", CurrentPerson.PhotoUrl, imgTag );
                }
                else
                {
                    lImage.Text = imgTag;
                }

                // Person Info
                lName.Text = CurrentPerson.FullName;
                if ( CurrentPerson.BirthDate.HasValue )
                {
                    lAge.Text = string.Format( "{0} old <small>({1})</small><br/>", CurrentPerson.FormatAge(), CurrentPerson.BirthYear != DateTime.MinValue.Year ? CurrentPerson.BirthDate.Value.ToShortDateString() : CurrentPerson.BirthDate.Value.ToMonthDayString() );
                }

                lGender.Text = CurrentPerson.Gender != Gender.Unknown ? CurrentPerson.Gender.ToString() : string.Empty;
                lGrade.Text = CurrentPerson.GradeFormatted;
                lMaritalStatus.Text = CurrentPerson.MaritalStatusValueId.DefinedValue();
                if ( CurrentPerson.AnniversaryDate.HasValue )
                {
                    lMaritalStatus.Text += string.Format( " {0} yrs <small>({1})</small>", CurrentPerson.AnniversaryDate.Value.Age(), CurrentPerson.AnniversaryDate.Value.ToMonthDayString() );
                }

                if ( CurrentPerson.GetFamilies().Count() > 1 )
                {
                    ddlGroup.Visible = true;
                }

                // Contact Info
                if ( CurrentPerson.PhoneNumbers != null )
                {
                    var selectedPhoneTypeGuids = GetAttributeValue( "PhoneNumbers" ).Split( ',' ).AsGuidList();
                    rptPhones.DataSource = CurrentPerson.PhoneNumbers.Where( pn => selectedPhoneTypeGuids.Contains( pn.NumberTypeValue.Guid ) ).ToList();
                    rptPhones.DataBind();
                }

                lEmail.Text = CurrentPerson.Email;

                // Person Attributes
                List<Guid> attributeGuidList = GetPersonAttributeGuids( personId );
                CurrentPerson.LoadAttributes();
                rptPersonAttributes.DataSource = CurrentPerson.Attributes.Where( a =>
                     attributeGuidList.Contains( a.Value.Guid ) )
                    .Select( a => new
                    {
                        Name = a.Value.Name,
                        Value = a.Value.FieldType.Field.FormatValue( null, a.Value.EntityTypeId, CurrentPerson.Id, CurrentPerson.GetAttributeValue( a.Key ), a.Value.QualifierValues, a.Value.FieldType.Class == typeof( Rock.Field.Types.ImageFieldType ).FullName )
                    } )
                    .OrderBy( av => av.Name )
                    .ToList()
                    .Where( av => !String.IsNullOrWhiteSpace( av.Value ) );
                rptPersonAttributes.DataBind();

                // Families
                if ( GetAttributeValue( "ShowFamilyMembers" ).AsBoolean() )
                {
                    if ( ddlGroup.SelectedValueAsId().HasValue )
                    {
                        var group = new GroupService( rockContext ).Get( ddlGroup.SelectedValueAsId().Value );
                        if ( group != null )
                        {

                            // Family Name
                            lGroupName.Text = group.Name;

                            // Family Address
                            Guid? locationTypeGuid = GetAttributeValue( "AddressType" ).AsGuidOrNull();
                            if ( locationTypeGuid.HasValue )
                            {
                                var addressTypeDv = DefinedValueCache.Read( locationTypeGuid.Value );

                                var familyGroupTypeGuid = Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuidOrNull();

                                if ( familyGroupTypeGuid.HasValue )
                                {
                                    var familyGroupType = GroupTypeCache.Read( familyGroupTypeGuid.Value );

                                    var address = new GroupLocationService( rockContext ).Queryable()
                                                        .Where( l => l.Group.GroupTypeId == familyGroupType.Id
                                                             && l.GroupLocationTypeValueId == addressTypeDv.Id
                                                             && l.Group.Members.Any( m => m.PersonId == CurrentPerson.Id )
                                                             && l.Group.Id == group.Id )
                                                        .Select( l => l.Location )
                                                        .FirstOrDefault();
                                    if ( address != null )
                                    {
                                        lAddress.Text = string.Format( "<b>{0} Address</b><br />{1}", addressTypeDv.Value, address.FormattedHtmlAddress );
                                    }
                                }
                            }

                            // Family Attributes
                            group.LoadAttributes();
                            List<Guid> familyAttributeGuidList = GetAttributeValue( "FamilyAttributes" ).SplitDelimitedValues().AsGuidList();
                            var familyAttributes = group.Attributes.Where( a =>
                                 familyAttributeGuidList.Contains( a.Value.Guid ) )
                                .Select( a => new
                                {
                                    Name = a.Value.Name,
                                    Value = a.Value.FieldType.Field.FormatValue( null, a.Value.EntityTypeId, group.Id, group.GetAttributeValue( a.Key ), a.Value.QualifierValues, a.Value.FieldType.Class == typeof( Rock.Field.Types.ImageFieldType ).FullName )
                                } )
                                .OrderBy( av => av.Name )
                                .ToList()
                                .Where( av => !String.IsNullOrWhiteSpace( av.Value ) );
                            if ( familyAttributes.Count() > 0 )
                            {
                                lFamilyHeader.Visible = true;
                                rptGroupAttributes.DataSource = familyAttributes;
                                rptGroupAttributes.DataBind();
                            }

                            rptGroupMembers.DataSource = group.Members.Where( gm =>
                                gm.PersonId != CurrentPerson.Id &&
                                gm.Person.IsDeceased == false )
                                .OrderBy( m => m.GroupRole.Order )
                                .ToList();
                            rptGroupMembers.DataBind();
                        }
                    }
                }

                if ( String.IsNullOrWhiteSpace( GetAttributeValue( "WorkflowLaunchPage" ) ) )
                {
                    lbRequestChanges.Visible = false;
                }

                // Get next family members details
                var leaderBaptismPhoto = CurrentPerson.GetAttributeValues( "BaptismPhoto" );
                var isLeaderBaptizedHere = CurrentPerson.AttributeValues["BaptizedHere"];

                // Assign baptism photo
                if ( leaderBaptismPhoto != null && leaderBaptismPhoto.Count > 0 )
                {
                    lLeaderBaptismPhoto.Text = string.Format( "<a href='/baptismdashboard?display=photo&paguid={0}'><div class='fa fa-picture-o nextstep-modal-baptism-icon baptism-profile-icons'></div></a>", CurrentPerson.PrimaryAlias.Guid );
                }
                else
                {
                    lLeaderBaptismPhoto.Text = "";
                }

                // Assign baptism certificate 
                if ( isLeaderBaptizedHere != null && isLeaderBaptizedHere.ToString() == "Yes" )
                {
                    lLeaderCertificate.Text = string.Format( "<a href='/baptismdashboard?display=certificate&paguid={0}'><div class='mdi mdi-certificate nextstep-modal-baptism-icon baptism-profile-icons'></div></a>", CurrentPerson.PrimaryAlias.Guid );
                }
                else
                {
                    lLeaderCertificate.Text = "";
                }
            }

            hfPersonId.Value = string.Empty;
            pnlEdit.Visible = false;
            pnlView.Visible = true;
        }

        /// <summary>
        /// Shows the edit person details.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="groupId">The group identifier.</param>
        private void ShowEditPersonDetails( int personId )
        {
            var childGuid = Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_CHILD.AsGuid();

            RockContext rockContext = new RockContext();
            if ( ddlGroup.SelectedValueAsId().HasValue )
            {
                var group = new GroupService( rockContext ).Get( ddlGroup.SelectedValueAsId().Value );
                if ( group != null )
                {
                    RoleType = null;
                    hfPersonId.Value = personId.ToString();
                    var person = new Person();
                    if ( personId == 0 )
                    {
                        rblRole.DataSource = group.GroupType.Roles.OrderBy( r => r.Order ).ToList();
                        rblRole.DataBind();
                        rblRole.Visible = true;
                        rblRole.Required = true;
                    }
                    else
                    {
                        person = new PersonService( rockContext ).Get( personId );
                    }

                    if ( ddlGroup.SelectedValueAsId().HasValue )
                    {

                        if ( person != null )
                        {
                            if ( GetAttributeValue( "DisableNameEdit" ).AsBoolean() )
                            {
                                tbFirstName.Enabled = false;
                                tbLastName.Enabled = false;
                            }

                            bool photoRestricted = _photoBlacklistPersonIds != null ? _photoBlacklistPersonIds.Contains( personId ) : false;
                            if ( photoRestricted )
                            {
                                nbPhotoWarning.Text = GetAttributeValue( "BlacklistMessage" );
                                nbPhotoWarning.Visible = true;
                                imgPhoto.Visible = false;
                            }
                            else
                            {
                                imgPhoto.BinaryFileId = person.PhotoId;
                                imgPhoto.NoPictureUrl = Person.GetPersonNoPictureUrl( person, 200, 200 );
                                imgPhoto.Visible = true;
                                nbPhotoWarning.Visible = false;
                            }

                            ddlTitle.SelectedValue = person.TitleValueId.HasValue ? person.TitleValueId.Value.ToString() : string.Empty;
                            tbFirstName.Text = person.FirstName;
                            tbNickName.Text = person.NickName;
                            tbLastName.Text = person.LastName;
                            ddlSuffix.SelectedValue = person.SuffixValueId.HasValue ? person.SuffixValueId.Value.ToString() : string.Empty;
                            bpBirthDay.SelectedDate = person.BirthDate;
                            rblGender.SelectedValue = person.Gender.ConvertToString();
                            if ( group.Members.Where( gm => gm.PersonId == person.Id && gm.GroupRole.Guid == childGuid ).Any() )
                            {
                                if ( person.GraduationYear.HasValue )
                                {
                                    ypGraduation.SelectedYear = person.GraduationYear.Value;
                                }
                                else
                                {
                                    ypGraduation.SelectedYear = null;
                                }

                                ddlGradePicker.Visible = true;
                                if ( !person.HasGraduated ?? false )
                                {
                                    int gradeOffset = person.GradeOffset.Value;
                                    var maxGradeOffset = ddlGradePicker.MaxGradeOffset;

                                    // keep trying until we find a Grade that has a gradeOffset that that includes the Person's gradeOffset (for example, there might be combined grades)
                                    while ( !ddlGradePicker.Items.OfType<ListItem>().Any( a => a.Value.AsInteger() == gradeOffset ) && gradeOffset <= maxGradeOffset )
                                    {
                                        gradeOffset++;
                                    }

                                    ddlGradePicker.SetValue( gradeOffset );
                                }
                                else
                                {
                                    ddlGradePicker.SelectedIndex = 0;
                                }
                            }

                            tbEmail.Text = person.Email;
                            rblEmailPreference.SelectedValue = person.EmailPreference.ConvertToString( false );

                            rblCommunicationPreference.Visible = this.GetAttributeValue( "ShowCommunicationPreference" ).AsBoolean();
                            rblCommunicationPreference.SetValue( person.CommunicationPreference == CommunicationType.SMS ? "2" : "1" );

                            // Person Attributes
                            var displayedAttributeGuids = GetPersonAttributeGuids( person.Id );
                            if ( !displayedAttributeGuids.Any() || personId == 0 )
                            {
                                pnlPersonAttributes.Visible = false;
                            }
                            else
                            {
                                pnlPersonAttributes.Visible = true;
                                DisplayEditAttributes( person, displayedAttributeGuids, phPersonAttributes, pnlPersonAttributes, true );
                            }

                            // Family Attributes
                            if ( person.Id == CurrentPerson.Id )
                            {
                                List<Guid> familyAttributeGuidList = GetAttributeValue( "FamilyAttributes" ).SplitDelimitedValues().AsGuidList();
                                if ( familyAttributeGuidList.Any() )
                                {
                                    pnlFamilyAttributes.Visible = true;
                                    DisplayEditAttributes( group, familyAttributeGuidList, phFamilyAttributes, pnlFamilyAttributes, true );
                                }
                                else
                                {
                                    pnlFamilyAttributes.Visible = false;
                                }

                                Guid? locationTypeGuid = GetAttributeValue( "AddressType" ).AsGuidOrNull();
                                if ( locationTypeGuid.HasValue )
                                {
                                    pnlAddress.Visible = true;
                                    var addressTypeDv = DefinedValueCache.Read( locationTypeGuid.Value );

                                    // if address type is home enable the move and is mailing/physical
                                    if ( addressTypeDv.Guid == Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME.AsGuid() )
                                    {
                                        lbMoved.Visible = true;
                                        cbIsMailingAddress.Visible = true;
                                        cbIsPhysicalAddress.Visible = true;
                                    }
                                    else
                                    {
                                        lbMoved.Visible = false;
                                        cbIsMailingAddress.Visible = false;
                                        cbIsPhysicalAddress.Visible = false;
                                    }

                                    lAddressTitle.Text = addressTypeDv.Value + " Address";

                                    var familyGroupTypeGuid = Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuidOrNull();

                                    if ( familyGroupTypeGuid.HasValue )
                                    {
                                        var familyGroupType = GroupTypeCache.Read( familyGroupTypeGuid.Value );

                                        var familyAddress = new GroupLocationService( rockContext ).Queryable()
                                                            .Where( l => l.Group.GroupTypeId == familyGroupType.Id
                                                                 && l.GroupLocationTypeValueId == addressTypeDv.Id
                                                                 && l.Group.Members.Any( m => m.PersonId == person.Id ) )
                                                            .FirstOrDefault();
                                        if ( familyAddress != null )
                                        {
                                            acAddress.SetValues( familyAddress.Location );

                                            cbIsMailingAddress.Checked = familyAddress.IsMailingLocation;
                                            cbIsPhysicalAddress.Checked = familyAddress.IsMappedLocation;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                pnlFamilyAttributes.Visible = false;
                                pnlAddress.Visible = false;
                            }

                            var mobilePhoneType = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_MOBILE ) );

                            var phoneNumbers = new List<PhoneNumber>();
                            var phoneNumberTypes = DefinedTypeCache.Read( new Guid( Rock.SystemGuid.DefinedType.PERSON_PHONE_TYPE ) );
                            var selectedPhoneTypeGuids = GetAttributeValue( "PhoneNumbers" ).Split( ',' ).AsGuidList();
                            if ( phoneNumberTypes.DefinedValues.Where( pnt => selectedPhoneTypeGuids.Contains( pnt.Guid ) ).Any() )
                            {
                                foreach ( var phoneNumberType in phoneNumberTypes.DefinedValues.Where( pnt => selectedPhoneTypeGuids.Contains( pnt.Guid ) ) )
                                {
                                    var phoneNumber = person.PhoneNumbers.FirstOrDefault( n => n.NumberTypeValueId == phoneNumberType.Id );
                                    if ( phoneNumber == null )
                                    {
                                        var numberType = new DefinedValue();
                                        numberType.Id = phoneNumberType.Id;
                                        numberType.Value = phoneNumberType.Value;

                                        phoneNumber = new PhoneNumber { NumberTypeValueId = numberType.Id, NumberTypeValue = numberType };
                                        phoneNumber.IsMessagingEnabled = mobilePhoneType != null && phoneNumberType.Id == mobilePhoneType.Id;
                                    }
                                    else
                                    {
                                        // Update number format, just in case it wasn't saved correctly
                                        phoneNumber.NumberFormatted = PhoneNumber.FormattedNumber( phoneNumber.CountryCode, phoneNumber.Number );
                                    }

                                    phoneNumbers.Add( phoneNumber );
                                }

                                rContactInfo.DataSource = phoneNumbers;
                                rContactInfo.DataBind();
                            }
                        }
                    }
                }
            }

            pnlView.Visible = false;
            pnlEdit.Visible = true;
        }

        /// <summary>
        /// Gets the person attribute guids.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        private List<Guid> GetPersonAttributeGuids( int personId )
        {
            GroupMemberService groupMemberService = new GroupMemberService( new RockContext() );
            List<Guid> attributeGuidList = new List<Guid>();
            var adultGuid = Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT.AsGuid();
            var childGuid = Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_CHILD.AsGuid();
            var groupTypeGuid = Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuid();

            if ( groupMemberService.Queryable().Where( gm =>
               gm.PersonId == personId &&
               gm.Group.GroupType.Guid == groupTypeGuid &&
               gm.GroupRole.Guid == adultGuid ).Any() )
            {
                attributeGuidList = GetAttributeValue( "PersonAttributes(adults)" ).SplitDelimitedValues().AsGuidList();
            }
            else
            {
                attributeGuidList = GetAttributeValue( "PersonAttributes(children)" ).SplitDelimitedValues().AsGuidList();
            }

            return attributeGuidList;
        }

        /// <summary>
        /// Display Person Attribute on the Basis of Role
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="groupId">The group identifier.</param>
        private void DisplayPersonAttributeOnRoleType( int? selectedId )
        {
            GroupTypeRoleService groupTypeRoleService = new GroupTypeRoleService( new RockContext() );
            List<Guid> attributeGuidList = new List<Guid>();
            var adultGuid = Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT.AsGuid();
            var groupTypeGuid = Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuid();

            if ( selectedId.HasValue )
            {
                if ( groupTypeRoleService.Queryable().Where( gr =>
                               gr.GroupType.Guid == groupTypeGuid &&
                               gr.Guid == adultGuid &&
                               gr.Id == selectedId ).Any() )
                {
                    attributeGuidList = GetAttributeValue( "PersonAttributes(adults)" ).SplitDelimitedValues().AsGuidList();
                    ddlGradePicker.Visible = false;
                }
                else
                {
                    attributeGuidList = GetAttributeValue( "PersonAttributes(children)" ).SplitDelimitedValues().AsGuidList();
                    ddlGradePicker.Visible = true;
                }

                if ( attributeGuidList.Any() )
                {
                    pnlPersonAttributes.Visible = true;
                    DisplayEditAttributes( new Person(), attributeGuidList, phPersonAttributes, pnlPersonAttributes, true );
                }
                else
                {
                    pnlPersonAttributes.Visible = false;
                }
            }
        }

        /// <summary>
        /// Displays the edit attributes.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="displayedAttributeGuids">The displayed attribute guids.</param>
        /// <param name="phAttributes">The ph attributes.</param>
        /// <param name="pnlAttributes">The PNL attributes.</param>
        private void DisplayEditAttributes( IHasAttributes item, List<Guid> displayedAttributeGuids, PlaceHolder phAttributes, Panel pnlAttributes, bool setValue )
        {
            phAttributes.Controls.Clear();
            item.LoadAttributes();
            var excludedAttributeList = item.Attributes.Where( a => !displayedAttributeGuids.Contains( a.Value.Guid ) ).Select( a => a.Value.Key ).ToList();
            if ( item.Attributes != null && item.Attributes.Any() && displayedAttributeGuids.Any() )
            {
                pnlAttributes.Visible = true;
                Rock.Attribute.Helper.AddEditControls( item, phAttributes, setValue, BlockValidationGroup, excludedAttributeList, false, 2 );
            }
            else
            {
                pnlAttributes.Visible = false;
            }
        }

        /// <summary>
        /// Formats the phone number.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        protected string FormatPhoneNumber( object countryCode, object number )
        {
            string cc = countryCode as string ?? string.Empty;
            string n = number as string ?? string.Empty;
            return PhoneNumber.FormattedNumber( cc, n );
        }

        #endregion                          
    }
}
