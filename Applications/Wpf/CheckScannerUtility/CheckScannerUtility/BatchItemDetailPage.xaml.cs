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
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Rock.Model;

namespace Rock.Apps.CheckScannerUtility
{
    /// <summary>
    /// Interaction logic for BatchItemDetailPage.xaml
    /// </summary>
    public partial class BatchItemDetailPage : System.Windows.Controls.Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchItemDetailPage"/> class.
        /// </summary>
        public BatchItemDetailPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the financial transaction.
        /// </summary>
        /// <value>
        /// The financial transaction.
        /// </value>
        public FinancialTransaction FinancialTransaction { get; set; }

        /// <summary>
        /// Gets or sets the transaction image type value front.
        /// </summary>
        /// <value>
        /// The transaction image type value front.
        /// </value>
        public DefinedValue TransactionImageTypeValueFront { get; set; }

        /// <summary>
        /// Gets or sets the transaction image type value back.
        /// </summary>
        /// <value>
        /// The transaction image type value back.
        /// </value>
        public DefinedValue TransactionImageTypeValueBack { get; set; }

        /// <summary>
        /// Handles the Click event of the btnClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnClose_Click( object sender, RoutedEventArgs e )
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
            FinancialTransactionImage frontTranImage = FinancialTransaction.Images.FirstOrDefault( a => a.TransactionImageTypeValueId.Equals(TransactionImageTypeValueFront.Id));
            FinancialTransactionImage backTranImage = FinancialTransaction.Images.FirstOrDefault( a => a.TransactionImageTypeValueId.Equals(TransactionImageTypeValueBack.Id));

            if ( frontTranImage != null )
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream( frontTranImage.BinaryFile.Data.Content );
                bitmapImage.EndInit();
                imgFront.Source = bitmapImage;
            }
            else
            {
                imgFront.Source = null;
            }

            if ( backTranImage != null )
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream( backTranImage.BinaryFile.Data.Content );
                bitmapImage.EndInit();
                imgBack.Source = bitmapImage;
            }
            else
            {
                imgBack.Source = null;
            }
        }
    }
}
