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
using System.ComponentModel;
using System.ComponentModel.Composition;
using Rock.Attribute;
using Rock.StrikeIron.USAddressVerification;
using Rock.Web.UI;

namespace Rock.Address.Geocode
{
    /// <summary>
    /// The USAddressVerification service from <a href="http://www.strikeiron.com/Home.aspx">StrikeIron</a>
    /// </summary>
    [Description( "Address Standardization and Geocoding service from StrikeIron" )]
    [Export( typeof( GeocodeComponent ) )]
    [ExportMetadata( "ComponentName", "StrikeIron" )]
    [TextField( "User ID", "The Strike Iron User ID", true, "", "Security", 0 )]
    [TextField( "Password", "The Strike Iron Password", true, "", "Security", 1 )]
    public class StrikeIron : GeocodeComponent
    {
        /// <summary>
        /// Geocodes the specified address.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="result">The result.</param>
        /// <returns>
        /// True/False value of whether the address was standardized was succesfully
        /// </returns>
        public override bool Geocode( Rock.Model.Location location, out string result )
        {
            if ( location != null )
            {
                var registeredUser = new RegisteredUser();
                registeredUser.UserID = GetAttributeValue("UserID");
                registeredUser.Password = GetAttributeValue("Password");

                var licenseInfo = new LicenseInfo();
                licenseInfo.RegisteredUser = registeredUser;

                var client = new USAddressVerificationSoapClient();

                SIWsOutputOfUSAddress verifyResult;
                SubscriptionInfo info = client.VerifyAddressUSA(
                    licenseInfo,
                    location.Street1,
                    location.Street2,
                    string.Format("{0} {1} {2}", 
                        location.City,
                        location.State,
                        location.Zip),
                    string.Empty,
                    string.Empty,
                    CasingEnum.PROPER,
                    out verifyResult );

                if (verifyResult != null)
                {
                    result = verifyResult.ServiceStatus.StatusNbr.ToString();

                    if ( verifyResult.ServiceStatus.StatusNbr == 200 )
                    {
                        USAddress usAddress = verifyResult.ServiceResult;

                        if ( usAddress != null && usAddress.GeoCode != null )
                        {
                            location.SetLocationPointFromLatLong( usAddress.GeoCode.Latitude, usAddress.GeoCode.Longitude );

                            return true;
                        }
                    }
                }
                else
                    result = "Null Result";
            }
            else
                result = "Null Address";

            return false;
        }
    }
}