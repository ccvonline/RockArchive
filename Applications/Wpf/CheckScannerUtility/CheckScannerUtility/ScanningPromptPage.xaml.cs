﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rock.Model;
using Rock.Net;

namespace Rock.Apps.CheckScannerUtility
{
    /// <summary>
    /// Interaction logic for ScanningPromptPage.xaml
    /// </summary>
    public partial class ScanningPromptPage : System.Windows.Controls.Page
    {
        private BatchPage BatchPage;

        /// <summary>
        /// The currency list
        /// </summary>
        private List<DefinedValue> CurrencyList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanningPromptPage"/> class.
        /// </summary>
        /// <param name="scanningPage">The scanning page.</param>
        public ScanningPromptPage( BatchPage batchPage )
        {
            InitializeComponent();
            this.BatchPage = batchPage;

            RockConfig rockConfig = RockConfig.Load();
            RockRestClient client = new RockRestClient( rockConfig.RockBaseUrl );
            client.Login( rockConfig.Username, rockConfig.Password );

            var currencyTypeDefinedType = client.GetDataByGuid<DefinedType>( "api/DefinedTypes", Rock.SystemGuid.DefinedType.FINANCIAL_CURRENCY_TYPE.AsGuid() );
            this.CurrencyList = client.GetData<List<DefinedValue>>( "api/DefinedValues", "DefinedTypeId eq " + currencyTypeDefinedType.Id.ToString() );

            spTenderButtons.Children.Clear();
            foreach (var currency in this.CurrencyList.OrderBy(a => a.Order).ThenBy(a => a.Value))
            {
                ToggleButton btnToggle = new ToggleButton();
                btnToggle.Margin = new Thickness( 0, 12, 0, 0 );
                btnToggle.Padding = new Thickness( 0, 12, 0, 8 );
                btnToggle.Style = this.FindResource( "toggleButtonStyle" ) as Style;
                btnToggle.Content = currency.Value;
                btnToggle.Tag = currency.Guid;
                btnToggle.IsChecked = rockConfig.TenderTypeValueGuid.AsGuid() == currency.Guid;
                btnToggle.Click += btnToggle_Click;
                
                spTenderButtons.Children.Add( btnToggle );
            }
        }

        /// <summary>
        /// Handles the Click event of the btnToggle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnToggle_Click( object sender, RoutedEventArgs e )
        {
            ToggleButton btnToggleSelected = sender as ToggleButton;
            
            // ensure only one toggle button is selected at a time
            foreach ( ToggleButton btnToggle in spTenderButtons.Children.OfType<ToggleButton>() )
            {
                btnToggle.IsChecked = btnToggle == btnToggleSelected;
                
            }

            chkDoubleDocDetection.IsChecked = (Guid)btnToggleSelected.Tag == Rock.SystemGuid.DefinedValue.CURRENCY_TYPE_CHECK.AsGuid();
        }

        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnNext_Click( object sender, RoutedEventArgs e )
        {
            var rockConfig = RockConfig.Load();

            var selectedTenderButton = spTenderButtons.Children.OfType<ToggleButton>().Where( a => a.IsChecked == true ).FirstOrDefault();
            if (selectedTenderButton != null)
            {
                rockConfig.TenderTypeValueGuid = selectedTenderButton.Tag.ToString();
            }

            rockConfig.EnableRearImage = radDoubleSided.IsChecked == true;
            rockConfig.EnableDoubleDocDetection = chkDoubleDocDetection.IsChecked == true;

            // restart the scanner so that options will be reloaded
            this.BatchPage.rangerScanner.ShutDown();
            this.BatchPage.rangerScanner.StartUp();

            this.NavigationService.Navigate( this.BatchPage.ScanningPage );
        }

        /// <summary>
        /// Handles the Click event of the btnBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnBack_Click( object sender, RoutedEventArgs e )
        {
            this.NavigationService.GoBack();
        }
    }
}
