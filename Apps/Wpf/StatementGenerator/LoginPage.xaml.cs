﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Net;
using System.Windows;
using Rock.Model;
using Rock.Net;

namespace Rock.Apps.StatementGenerator
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : System.Windows.Controls.Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class.
        /// </summary>
        public LoginPage()
            : this(false)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class.
        /// </summary>
        public LoginPage(bool forceRockURLVisible)
        {
            InitializeComponent();
            ForceRockURLVisible = forceRockURLVisible;
        }

        /// <summary>
        /// Handles the Click event of the btnLogin control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnLogin_Click( object sender, RoutedEventArgs e )
        {
            StartPage startPage = new StartPage();
            try
            {
                txtUsername.Text = txtUsername.Text.Trim();
                txtRockUrl.Text = txtRockUrl.Text.Trim();
                RockRestClient rockRestClient = new RockRestClient( txtRockUrl.Text );
                rockRestClient.Login( txtUsername.Text, txtPassword.Password );
                Person person = rockRestClient.GetData<Person>( string.Format( "api/People/GetByUserName/{0}", txtUsername.Text ) );
            }
            catch ( Exception ex )
            {
                if ( ex is WebException )
                {
                    WebException wex = ex as WebException;
                    HttpWebResponse response = wex.Response as HttpWebResponse;
                    if ( response != null )
                    {
                        if ( response.StatusCode.Equals( HttpStatusCode.Unauthorized ) )
                        {
                            lblLoginWarning.Content = "Invalid Login";
                            lblLoginWarning.Visibility = Visibility.Visible;
                            return;
                        }
                    }
                }

                lblRockUrl.Visibility = Visibility.Visible;
                txtRockUrl.Visibility = Visibility.Visible;
                lblLoginWarning.Content = ex.Message;
                lblLoginWarning.Visibility = Visibility.Visible;
                return;
            }

            RockConfig rockConfig = RockConfig.Load();
            rockConfig.RockBaseUrl = txtRockUrl.Text;
            rockConfig.Username = txtUsername.Text;
            rockConfig.Password = txtPassword.Password;
            rockConfig.Save();
            
            this.NavigationService.Navigate( startPage);
        }

        /// <summary>
        /// Handles the Loaded event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Page_Loaded( object sender, RoutedEventArgs e )
        {
            HideLoginWarning( null, null );
            RockConfig rockConfig = RockConfig.Load();

            bool promptForUrl = string.IsNullOrWhiteSpace( rockConfig.RockBaseUrl ) || ForceRockURLVisible;

            lblRockUrl.Visibility = promptForUrl ? Visibility.Visible : Visibility.Collapsed;
            txtRockUrl.Visibility = promptForUrl ? Visibility.Visible : Visibility.Collapsed;
            
            txtRockUrl.Text = rockConfig.RockBaseUrl;
            txtUsername.Text = rockConfig.Username;
            txtPassword.Password = rockConfig.Password;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [force rock URL visible].
        /// </summary>
        /// <value>
        /// <c>true</c> if [force rock URL visible]; otherwise, <c>false</c>.
        /// </value>
        private bool ForceRockURLVisible { get; set; }

        /// <summary>
        /// Hides the login warning.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void HideLoginWarning( object sender, System.Windows.Input.KeyEventArgs e )
        {
            lblLoginWarning.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Handles the Click event of the btnRunReport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnRunReport_Click( object sender, RoutedEventArgs e )
        {
            ProgressPage progressPage = new ProgressPage();
            this.NavigationService.Navigate( progressPage );
        }
    }
}
