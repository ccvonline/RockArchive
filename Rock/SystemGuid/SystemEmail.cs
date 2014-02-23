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
using System.Web;

namespace Rock.SystemGuid
{
    /// <summary>
    /// System Email Templates
    /// </summary>
    public static class SystemEmail
    {
        /// <summary>
        /// Gets the template guid for the Forgot Username email
        /// </summary>
        public const string  SECURITY_FORGOT_USERNAME= "113593ff-620e-4870-86b1-7a0ec0409208";

        /// <summary>
        /// Gets the template guid for the Account Created email
        /// </summary>
        public const string  SECURITY_ACCOUNT_CREATED= "84e373e9-3aaf-4a31-b3fb-a8e3f0666710";

        /// <summary>
        /// Gets the template guid for the Confirm Account email
        /// </summary>
        public const string  SECURITY_CONFIRM_ACCOUNT= "17aaceef-15ca-4c30-9a3a-11e6cf7e6411";

        /// <summary>
        /// Gets the template guid for the Exception Notification email
        /// </summary>
        public const string  CONFIG_EXCEPTION_NOTIFICATION= "75CB0A4A-B1C5-4958-ADEB-8621BD231520";
    }
}