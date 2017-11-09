// <copyright>
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
using System.Web.Security;
using System.Linq;
using Rock;
using Rock.Attribute;
using Rock.Security;
using System.Text;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.church_ccv.Core
{
    /// <summary>
    /// Displays currently logged in user's name along with options to Login, Logout, or manage account.
    /// </summary>
    [DisplayName( "Login Modal" )]
    [Category( "CCV > Core" )]
    [Description( "Javascript Modal window used to manage login, account creation and forgot password functionality." )]
   
    [CodeEditorField( "Locked Out Caption", "The text (HTML) to display when a user's account has been locked.", CodeEditorMode.Html, CodeEditorTheme.Rock, 100, false, 
        @"Sorry, your account has been locked.  Please contact our office at {{ 'Global' | Attribute:'OrganizationPhone' }} or email {{ 'Global' | Attribute:'OrganizationEmail' }} to resolve this.  Thank-you.", "", 1 )]
    [CodeEditorField( "Confirm Caption", "The text (HTML) to display when a user's account needs to be confirmed.", CodeEditorMode.Html, CodeEditorTheme.Rock, 100, false, 
        @"Thank-you for logging in, however, we need to confirm the email associated with this account belongs to you. We've sent you an email that contains a link for confirming.  Please click the link in your email to continue.", "", 2 )]
    [CodeEditorField( "Forgot Password Caption", "The text (HTML) to display on the forgot password panel.", CodeEditorMode.Html, CodeEditorTheme.Rock, 100, false, 
        @"Enter the email address associated with your account. If you do not receive an email containing reset instructions, please call us at: {{ 'Global' | Attribute:'OrganizationPhone' }}", "", 3 )]

    [SystemEmailField( "Confirm Account Email Template Guid", "Used when the Confirmed option is UNCHECKED on a username.", false, Rock.SystemGuid.SystemEmail.SECURITY_CONFIRM_ACCOUNT, "Email Templates", 4 )]
    [SystemEmailField( "Forgot Password Email Template Guid", "Used when they should receive a list of usernames with Reset Password links next to each.", false, Rock.SystemGuid.SystemEmail.SECURITY_FORGOT_USERNAME, "Email Templates", 5 )]
    [SystemEmailField( "Account Created Email Template Guid", "Used when a new account is created.", false, Rock.SystemGuid.SystemEmail.SECURITY_ACCOUNT_CREATED, "Email Templates", 6 )]
    public partial class LoginModal : Rock.Web.UI.RockBlock
    {
        #region LoginModal
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
        }

        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            LoginModal_OnLoad( e );
        }
        #endregion
        
        #region LoginModal
        protected void LoginModal_OnLoad( EventArgs e )
        {
            // see if the login class needs to handle this
            if ( Page.IsPostBack )
            {
                // parse the event args so we know what we need to handle
                string postbackArgs = Request.Params["__EVENTARGUMENT"];
                if ( !string.IsNullOrWhiteSpace( postbackArgs ) )
                {
                    string[] splitArgs = postbackArgs.Split( new char[] { ':' } );

                    // the first should always be the action
                    switch ( splitArgs [ 0 ] )
                    {
                        case "__LOGIN_SUCCEEDED":
                        {
                            string returnUrl = Request.QueryString["returnurl"];
                            LoginModal_TryRedirectUser( returnUrl );

                            break;
                        }

                        // for now, do the same as when a login succeeds
                        case "__FORGOT_PASSWORD_SUCCEEDED":
                        case "__REGISTRATION_SUCCEEDED":
                        {
                            string returnUrl = Request.QueryString["returnurl"];
                            LoginModal_TryRedirectUser( returnUrl );

                            break;
                        }

                        default:
                        {
                            // ignore unknown actions
                            break;
                        }
                    }
                }
            }
        }

        protected void LoginModal_TryRedirectUser( string returnUrl )
        {
            if ( !string.IsNullOrWhiteSpace( returnUrl ) )
            {
                string redirectUrl = Server.UrlDecode( returnUrl );
                Response.Redirect( redirectUrl );
                ApplicationInstance.CompleteRequest( );
            }
            else
            {
                // without a return URL, just reload the existing page.
                Response.Redirect(Request.RawUrl);
            }
        }
                
        public string LoginModal_GetThemeUrl( )
        {
            return ResolveRockUrl( "~~/" );
        }

        public string LoginModal_GetAppUrl( )
        {
            return ResolveRockUrl( "~/" );
        }

        public string LoginModal_GetThemeUrlIncludeRoot( )
        {
            return ResolveRockUrlIncludeRoot( "~~/" );
        }

        public string LoginModal_GetAppUrlIncludeRoot( )
        {
            return ResolveRockUrlIncludeRoot( "~/" );
        }

        public string LoginModal_GetConfirmAccountEmailTemplateGuid( )
        {
            return GetAttributeValue( "ConfirmAccountEmailTemplateGuid" );
        }

        public string LoginModal_GetForgotPasswordEmailTemplateGuid( )
        {
            return GetAttributeValue( "ForgotPasswordEmailTemplateGuid" );
        }

        public string LoginModal_GetAccountCreatedEmailTemplateGuid( )
        {
            return GetAttributeValue( "AccountCreatedEmailTemplateGuid" );
        }

        public string LoginModal_GetConfirmAccountUrl( )
        {
            string url = ResolveRockUrl( "~/ConfirmAccount" );
            return RootPath + url;
        }

        public string LoginModal_GetConfirmCaption( )
        {
            var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( RockPage, CurrentPerson );
            return GetAttributeValue( "ConfirmCaption" ).ResolveMergeFields( mergeFields );
        }

        public string LoginModal_GetLockedOutCaption( )
        {
            var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( RockPage, CurrentPerson );
            return GetAttributeValue( "LockedOutCaption" ).ResolveMergeFields( mergeFields );
        }

        public string LoginModal_GetForgotPasswordCaption( )
        {
            var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( RockPage, CurrentPerson );
            return GetAttributeValue( "ForgotPasswordCaption" ).ResolveMergeFields( mergeFields );
        }
        #endregion
    }
}
