﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Rock.Net;

namespace Rock.Apps.CheckScannerUtility
{
    /// <summary>
    /// Interaction logic for OptionsPage.xaml
    /// </summary>
    public partial class OptionsPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsPage"/> class.
        /// </summary>
        public OptionsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the batch page.
        /// </summary>
        /// <value>
        /// The batch page.
        /// </value>
        public BatchPage BatchPage { get; set; }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        private void ShowDetail()
        {
            LoadDropDowns();
            
            lblAlert.Visibility = Visibility.Collapsed;
            
            var rockConfig = RockConfig.Load();

            txtRockUrl.Text = rockConfig.RockBaseUrl;

            if ( rockConfig.ScannerInterfaceType == RockConfig.InterfaceType.MICRImageRS232 )
            {
                cboScannerInterfaceType.SelectedItem = "MagTek";
                lblMakeModel.Content = "MagTek";

                string version = null;
                try
                {
                    this.Cursor = Cursors.Wait;
                    version = BatchPage.micrImage.Version();
                }
                finally
                {
                    this.Cursor = null;
                }

                if ( !version.Equals( "-1" ) )
                {
                    lblInterfaceVersion.Content = version;
                }
                else
                {
                    lblInterfaceVersion.Content = "error";
                }
            }
            else
            {
                cboScannerInterfaceType.SelectedItem = "Ranger";
                lblMakeModel.Content = string.Format( "Scanner Type: {0} {1}", BatchPage.rangerScanner.GetTransportInfo( "General", "Make" ), BatchPage.rangerScanner.GetTransportInfo( "General", "Model" ) );
                lblInterfaceVersion.Content = string.Format( "Interface Version: {0}", BatchPage.rangerScanner.GetVersion() );
            }

            string feederFriendlyNameType = BatchPage.ScannerFeederType.Equals( FeederType.MultipleItems ) ? "Multiple Items" : "Single Item";
            lblFeederType.Content = string.Format( "Feeder Type: {0}", feederFriendlyNameType );

            switch ( (ImageColorType)rockConfig.ImageColorType )
            {
                case ImageColorType.ImageColorTypeGrayscale:
                    cboImageOption.SelectedValue = "Grayscale";
                    break;
                case ImageColorType.ImageColorTypeColor:
                    cboImageOption.SelectedValue = "Color";
                    break;
                default:
                    cboImageOption.SelectedIndex = 0;
                    break;
            }


            if ( cboMagTekCommPort.Items.Count > 0 )
            {
                cboMagTekCommPort.SelectedItem = string.Format( "COM{0}", rockConfig.MICRImageComPort );
            }
        }

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns()
        {
            cboImageOption.Items.Clear();
            cboImageOption.Items.Add( "Bitonal" );
            cboImageOption.Items.Add( "Grayscale" );
            cboImageOption.Items.Add( "Color" );

            cboScannerInterfaceType.Items.Clear();
            cboScannerInterfaceType.Items.Add( "Ranger" );
            cboScannerInterfaceType.Items.Add( "MagTek" );

            cboMagTekCommPort.ItemsSource = System.IO.Ports.SerialPort.GetPortNames();
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSave_Click( object sender, RoutedEventArgs e )
        {
            RockConfig rockConfig = RockConfig.Load();

            try
            {
                txtRockUrl.Text = txtRockUrl.Text.Trim();
                RockRestClient client = new RockRestClient( txtRockUrl.Text );
                client.Login( rockConfig.Username, rockConfig.Password );
            }
            catch ( Exception ex )
            {
                lblAlert.Content = ex.Message;
                lblAlert.Visibility = Visibility.Visible;
                return;
            }

            rockConfig.RockBaseUrl = txtRockUrl.Text;

            if ( cboScannerInterfaceType.SelectedItem.Equals( "MagTek" ) )
            {
                rockConfig.ScannerInterfaceType = RockConfig.InterfaceType.MICRImageRS232;
            }
            else
            {
                rockConfig.ScannerInterfaceType = RockConfig.InterfaceType.RangerApi;
            }
            
            string imageOption = cboImageOption.SelectedValue as string;

            switch ( imageOption )
            {
                case "Grayscale":
                    rockConfig.ImageColorType = ImageColorType.ImageColorTypeGrayscale;
                    break;
                case "Color":
                    rockConfig.ImageColorType = ImageColorType.ImageColorTypeColor;
                    break;
                default:
                    rockConfig.ImageColorType = ImageColorType.ImageColorTypeBitonal;
                    break;
            }

            string comPortName = cboMagTekCommPort.SelectedItem as string;
            
            if (!string.IsNullOrWhiteSpace(comPortName))
            {
                rockConfig.MICRImageComPort = short.Parse( comPortName.Replace( "COM", string.Empty ) );
            }

            rockConfig.Save();

            this.NavigationService.GoBack();
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click( object sender, RoutedEventArgs e )
        {
            this.NavigationService.GoBack();
        }

        /// <summary>
        /// Handles the Loaded event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Page_Loaded( object sender, RoutedEventArgs e )
        {
            ShowDetail();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the cboScannerInterfaceType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void cboScannerInterfaceType_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            bool magTekSelected = cboScannerInterfaceType.SelectedItem.Equals( "MagTek" );
            
            // show COM port option only for Mag Tek
            lblMagTekCommPort.Visibility = magTekSelected ? Visibility.Visible : Visibility.Collapsed;
            cboMagTekCommPort.Visibility = magTekSelected ? Visibility.Visible : Visibility.Collapsed;

            // show Image Option only for Ranger
            lblImageOption.Visibility = magTekSelected ? Visibility.Collapsed : Visibility.Visible;
            cboImageOption.Visibility = magTekSelected ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
