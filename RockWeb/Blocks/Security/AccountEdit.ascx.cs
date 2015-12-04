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
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Security
{
    /// <summary>
    /// Allows a person to edit their account information. 
    /// </summary>
    [DisplayName( "Account Edit" )]
    [Category( "Security" )]
    [Description( "Allows a person to edit their account information." )]

    [BooleanField("Show Address", "Allows hiding the address field.", false, order: 0)]
    [BooleanField( "Address Required", "Whether the address is required.", false, order: 2 )]
    [GroupLocationTypeField( Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY, "Location Type",
        "The type of location that address should use.", false, Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME, "", 14 )]
    public partial class AccountEdit : RockBlock
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            ddlTitle.BindToDefinedType( DefinedTypeCache.Read( new Guid( Rock.SystemGuid.DefinedType.PERSON_TITLE ) ), true );
            ddlSuffix.BindToDefinedType( DefinedTypeCache.Read( new Guid( Rock.SystemGuid.DefinedType.PERSON_SUFFIX ) ), true );
            string smsScript = @"
    $('.js-sms-number').click(function () {
        if ($(this).is(':checked')) {
            $('.js-sms-number').not($(this)).prop('checked', false);
        }
    });
";
            ScriptManager.RegisterStartupScript( rContactInfo, rContactInfo.GetType(), "sms-number-" + BlockId.ToString(), smsScript, true );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack && CurrentPerson != null )
            {
                ShowDetails();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            rockContext.WrapTransaction( () =>
            {
                var personService = new PersonService( rockContext );

                var changes = new List<string>();

                var person = personService.Get( CurrentPersonId ?? 0 );
                if ( person != null )
                {
                    int? orphanedPhotoId = null;
                    if ( person.PhotoId != imgPhoto.BinaryFileId )
                    {
                        orphanedPhotoId = person.PhotoId;
                        person.PhotoId = imgPhoto.BinaryFileId;

                        if ( orphanedPhotoId.HasValue )
                        {
                            if ( person.PhotoId.HasValue )
                            {
                                changes.Add( "Modified the photo." );
                            }
                            else
                            {
                                changes.Add( "Deleted the photo." );
                            }
                        }
                        else if ( person.PhotoId.HasValue )
                        {
                            changes.Add( "Added a photo." );
                        }
                    }

                    int? newTitleId = ddlTitle.SelectedValueAsInt();
                    History.EvaluateChange( changes, "Title", DefinedValueCache.GetName( person.TitleValueId ), DefinedValueCache.GetName( newTitleId ) );
                    person.TitleValueId = newTitleId;

                    History.EvaluateChange( changes, "First Name", person.FirstName, tbFirstName.Text );
                    person.FirstName = tbFirstName.Text;

                    History.EvaluateChange(changes, "Nick Name", person.NickName, tbNickName.Text);
                    person.NickName = tbNickName.Text;

                    History.EvaluateChange( changes, "Last Name", person.LastName, tbLastName.Text );
                    person.LastName = tbLastName.Text;

                    int? newSuffixId = ddlSuffix.SelectedValueAsInt();
                    History.EvaluateChange( changes, "Suffix", DefinedValueCache.GetName( person.SuffixValueId ), DefinedValueCache.GetName( newSuffixId ) );
                    person.SuffixValueId = newSuffixId;

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

                    History.EvaluateChange( changes, "Birth Month", birthMonth, person.BirthMonth );
                    History.EvaluateChange( changes, "Birth Day", birthDay, person.BirthDay );
                    History.EvaluateChange( changes, "Birth Year", birthYear, person.BirthYear );

                    var newGender = rblGender.SelectedValue.ConvertToEnum<Gender>();
                    History.EvaluateChange( changes, "Gender", person.Gender, newGender );
                    person.Gender = newGender;

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

                                    History.EvaluateChange(
                                        changes,
                                        string.Format( "{0} Phone", DefinedValueCache.GetName( phoneNumberTypeId ) ),
                                        oldPhoneNumber,
                                        phoneNumber.NumberFormattedWithCountryCode );
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
                        History.EvaluateChange(
                            changes,
                            string.Format( "{0} Phone", DefinedValueCache.GetName( phoneNumber.NumberTypeValueId ) ),
                            phoneNumber.ToString(),
                            string.Empty );

                        person.PhoneNumbers.Remove( phoneNumber );
                        phoneNumberService.Delete( phoneNumber );
                    }

                    History.EvaluateChange( changes, "Email", person.Email, tbEmail.Text );
                    person.Email = tbEmail.Text.Trim();

                    if ( person.IsValid )
                    {
                        if ( rockContext.SaveChanges() > 0 )
                        {
                            if ( changes.Any() )
                            {
                                HistoryService.SaveChanges(
                                    rockContext,
                                    typeof( Person ),
                                    Rock.SystemGuid.Category.HISTORY_PERSON_DEMOGRAPHIC_CHANGES.AsGuid(),
                                    person.Id,
                                    changes );
                            }

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

                        // save address
                        if ( pnlAddress.Visible )
                        {
                            Guid? familyGroupTypeGuid = Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuidOrNull();
                            if ( familyGroupTypeGuid.HasValue )
                            {
                                var familyGroup = new GroupService( rockContext ).Queryable()
                                                .Where( f => f.GroupType.Guid == familyGroupTypeGuid.Value
                                                    && f.Members.Any( m => m.PersonId == person.Id ) )
                                                .FirstOrDefault();
                                if ( familyGroup != null )
                                {
                                    Guid? addressTypeGuid = GetAttributeValue("LocationType").AsGuidOrNull();
                                    if ( addressTypeGuid.HasValue )
                                    {
                                        var groupLocationService = new GroupLocationService( rockContext );

                                        var dvHomeAddressType = DefinedValueCache.Read( addressTypeGuid.Value );
                                        var familyHomeAddress = groupLocationService.Queryable().Where( l => l.GroupId == familyGroup.Id && l.GroupLocationTypeValueId == dvHomeAddressType.Id ).FirstOrDefault();
                                        if ( familyHomeAddress != null && string.IsNullOrWhiteSpace( acAddress.Street1 ) )
                                        {

                                            // delete the current address
                                            History.EvaluateChange( changes, familyHomeAddress.GroupLocationTypeValue.Value + " Location", familyHomeAddress.Location.ToString(), string.Empty );
                                            groupLocationService.Delete( familyHomeAddress );
                                            rockContext.SaveChanges();
                                        }
                                        else
                                        {
                                            if ( !string.IsNullOrWhiteSpace( acAddress.Street1 ) )
                                            {
                                                if ( familyHomeAddress == null )
                                                {
                                                    familyHomeAddress = new GroupLocation();
                                                    groupLocationService.Add( familyHomeAddress );
                                                    familyHomeAddress.GroupLocationTypeValueId = dvHomeAddressType.Id;
                                                    familyHomeAddress.GroupId = familyGroup.Id;
                                                    familyHomeAddress.IsMailingLocation = true;
                                                    familyHomeAddress.IsMappedLocation = true;
                                                }

                                                var updatedHomeAddress = new Location();
                                                acAddress.GetValues( updatedHomeAddress );

                                                History.EvaluateChange( changes, dvHomeAddressType.Value + " Location", familyHomeAddress.Location != null ? familyHomeAddress.Location.ToString() : string.Empty, updatedHomeAddress.ToString() );

                                                familyHomeAddress.Location = updatedHomeAddress;
                                                rockContext.SaveChanges();
                                            }
                                        }

                                        HistoryService.SaveChanges(
                                            rockContext,
                                            typeof( Person ),
                                            Rock.SystemGuid.Category.HISTORY_PERSON_DEMOGRAPHIC_CHANGES.AsGuid(),
                                            person.Id,
                                            changes );
                                    }
                                }
                            }
                        }

                        NavigateToParentPage();
                    }
                }
            } );
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            NavigateToParentPage();
        }

        /// <summary>
        /// Shows the details.
        /// </summary>
        private void ShowDetails()
        {
            RockContext rockContext = new RockContext();

            pnlAddress.Visible = GetAttributeValue( "ShowAddress" ).AsBoolean();
            acAddress.Required = GetAttributeValue( "AddressRequired" ).AsBoolean();

            var person = CurrentPerson;
            if ( person != null )
            {
                imgPhoto.BinaryFileId = person.PhotoId;
                imgPhoto.NoPictureUrl = Person.GetPhotoUrl( null, person.Age, person.Gender );
                ddlTitle.SelectedValue = person.TitleValueId.HasValue ? person.TitleValueId.Value.ToString() : string.Empty;
                tbFirstName.Text = person.FirstName;
                tbNickName.Text = person.NickName;
                tbLastName.Text = person.LastName;
                ddlSuffix.SelectedValue = person.SuffixValueId.HasValue ? person.SuffixValueId.Value.ToString() : string.Empty;
                bpBirthDay.SelectedDate = person.BirthDate;
                rblGender.SelectedValue = person.Gender.ConvertToString();
                tbEmail.Text = person.Email;


                var homeAddress = person.GetHomeLocation();
                

                Guid? locationTypeGuid = GetAttributeValue( "LocationType" ).AsGuidOrNull();
                if ( locationTypeGuid.HasValue )
                {
                    var addressTypeDv = DefinedValueCache.Read( locationTypeGuid.Value );

                    lAddressTitle.Text = addressTypeDv.Value + " Address";

                    var familyGroupTypeGuid = Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY.AsGuidOrNull();

                    if ( familyGroupTypeGuid.HasValue )
                    {
                        var familyGroupType = GroupTypeCache.Read( familyGroupTypeGuid.Value );

                        var address = new GroupLocationService( rockContext ).Queryable()
                                            .Where( l => l.Group.GroupTypeId == familyGroupType.Id
                                                 && l.GroupLocationTypeValueId == addressTypeDv.Id 
                                                 && l.Group.Members.Any( m => m.PersonId == person.Id))
                                            .Select( l => l.Location)
                                            .FirstOrDefault();
                        if ( address != null )
                        {
                            acAddress.SetValues( homeAddress );
                        }
                    }
                }

                var mobilePhoneType = DefinedValueCache.Read( new Guid( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_MOBILE ) );

                var phoneNumbers = new List<PhoneNumber>();
                var phoneNumberTypes = DefinedTypeCache.Read( new Guid( Rock.SystemGuid.DefinedType.PERSON_PHONE_TYPE ) );
                if ( phoneNumberTypes.DefinedValues.Any() )
                {
                    foreach ( var phoneNumberType in phoneNumberTypes.DefinedValues )
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