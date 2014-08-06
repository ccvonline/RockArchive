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

using Newtonsoft.Json;

using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Finance
{
    [DisplayName( "Transaction Detail" )]
    [Category( "Finance" )]
    [Description( "Displays the details of the given transaction for editing." )]

    [LinkedPage("Scheduled Transaction Detail Page", "Page used to view scheduled transaction detail")]
    public partial class TransactionDetail : Rock.Web.UI.RockBlock, IDetailBlock
    {
        #region Properties

        private List<FinancialTransactionDetail> TransactionDetailsState { get; set; }
        private List<FinancialTransactionImage> TransactionImagesState { get; set; }
        private List<int> BinaryFileIds { get; set; }

        private Dictionary<int, string> _accountNames = null;
        private Dictionary<int, string> AccountNames
        {
            get
            {
                if (_accountNames == null)
                {
                    _accountNames = new Dictionary<int,string>();
                    new FinancialAccountService( new RockContext() ).Queryable()
                        .OrderBy( a => a.Order )
                        .Select( a => new { a.Id, a.Name } )
                        .ToList()
                        .ForEach( a => _accountNames.Add( a.Id, a.Name ) );
                }
                return _accountNames;
            }
        }

        #endregion

        #region Control Methods

        /// <summary>
        /// Restores the view-state information from a previous user control request that was saved by the <see cref="M:System.Web.UI.UserControl.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object" /> that represents the user control state to be restored.</param>
        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );

            BinaryFileIds = ViewState["BinaryFileIds"] as List<int>;

            string json = ViewState["TransactionDetailsState"] as string;
            if (string.IsNullOrWhiteSpace(json))
            {
                TransactionDetailsState = new List<FinancialTransactionDetail>();
            }
            else
            {
                TransactionDetailsState = JsonConvert.DeserializeObject<List<FinancialTransactionDetail>>( json );
            }

            json = ViewState["TransactionImagesState"] as string;
            if ( string.IsNullOrWhiteSpace( json ) )
            {
                TransactionImagesState = new List<FinancialTransactionImage>();
            }
            else
            {
                TransactionImagesState = JsonConvert.DeserializeObject<List<FinancialTransactionImage>>( json );
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            gAccountsView.DataKeyNames = new string[] { "Guid" };
            gAccountsView.ShowActionRow = false;

            gAccountsEdit.DataKeyNames = new string[] { "Guid" };
            gAccountsEdit.ShowActionRow = true;
            gAccountsEdit.Actions.ShowAdd = true;
            gAccountsEdit.Actions.AddClick += gAccountsEdit_AddClick;
            gAccountsEdit.GridRebind += gAccountsEdit_GridRebind;

            
            //function toggleCheckImages() {
            //    var image1src = $('#<%=imgCheck.ClientID%>').attr("src");
            //    var image2src = $('#<%=imgCheckOtherSideThumbnail.ClientID%>').attr("src");

            //    $('#<%=imgCheck.ClientID%>').attr("src", image2src);
            //    $('#<%=imgCheckOtherSideThumbnail.ClientID%>').attr("src", image1src);
            //}

            string script = @"
    $('.transaction-image-thumbnail').click( function() {
        var $primaryImg = $('.transaction-image');
        var primarySrc = $primaryImg.attr('src');
        $primaryImg.attr('src', $(this).attr('src'));
        $(this).attr('src', primarySrc);
    });
";
            ScriptManager.RegisterStartupScript( imgPrimary, imgPrimary.GetType(), "imgPrimarySwap", script, true );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            if ( !Page.IsPostBack )
            {
                ShowDetail( PageParameter( "transactionId" ).AsInteger(), PageParameter("batchId").AsIntegerOrNull() );
            }
            else
            {
                if (pnlEditDetails.Visible)
                {
                    foreach( DataListItem item in dlImages.Items )
                    {
                        var hfImageGuid = item.FindControl( "hfImageGuid" ) as HiddenField;
                        var ddlImageType = item.FindControl( "ddlImageType" ) as RockDropDownList;
                        var imgupImage = item.FindControl( "imgupImage" ) as Rock.Web.UI.Controls.ImageUploader;

                        if ( hfImageGuid != null && ddlImageType != null && imgupImage != null )
                        {
                            var txnImage = TransactionImagesState
                                .Where( i => i.Guid.Equals( hfImageGuid.Value.AsGuid() ) )
                                .FirstOrDefault();
                            if ( txnImage != null )
                            {
                                txnImage.TransactionImageTypeValueId = ddlImageType.SelectedValueAsInt();
                                txnImage.BinaryFileId = imgupImage.BinaryFileId ?? 0;
                            }
                        }
                    }
                }

                ShowDialog();
            }
        }

        protected override object SaveViewState()
        {
            ViewState["BinaryFileIds"] = BinaryFileIds;

            var jsonSetting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new Rock.Utility.IgnoreUrlEncodedKeyContractResolver()
            };

            ViewState["TransactionDetailsState"] = JsonConvert.SerializeObject( TransactionDetailsState, Formatting.None, jsonSetting );
            ViewState["TransactionImagesState"] = JsonConvert.SerializeObject( TransactionImagesState, Formatting.None, jsonSetting );

            return base.SaveViewState();
        }

        #endregion Control Methods

        #region Events

        #region Edit Events

        /// <summary>
        /// Handles the Click event of the lbEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbEdit_Click( object sender, EventArgs e )
        {
            ShowEditDetails( GetTransaction( hfTransactionId.Value.AsInteger() ), new RockContext() );
        }

        /// <summary>
        /// Handles the Click event of the lbSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbSave_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();

            var txnService = new FinancialTransactionService( rockContext );
            var txnDetailService = new FinancialTransactionDetailService( rockContext);
            var txnImageService = new FinancialTransactionImageService( rockContext);
            var binaryFileService = new BinaryFileService( rockContext );

            FinancialTransaction txn = null;

            int? txnId = hfTransactionId.Value.AsIntegerOrNull();
            int? batchId = hfBatchId.Value.AsIntegerOrNull();

            if (txnId.HasValue)
            {
                txn = txnService.Get( txnId.Value );
            }

            if ( txn == null && batchId.HasValue )
            {
                txn = new FinancialTransaction();
                txnService.Add( txn );
                txn.BatchId = batchId.Value;
            }

            if ( txn != null  )
            {
                txn.AuthorizedPersonId = ppAuthorizedPerson.PersonId;
                txn.TransactionDateTime = dtTransactionDateTime.SelectedDateTime;
                txn.TransactionTypeValueId = ddlTransactionType.SelectedValue.AsInteger();
                txn.SourceTypeValueId = ddlSourceType.SelectedValueAsInt();

                Guid? gatewayGuid = cpPaymentGateway.SelectedValueAsGuid();
                if ( gatewayGuid.HasValue )
                {
                    var gatewayEntity = EntityTypeCache.Read( gatewayGuid.Value );
                    if ( gatewayEntity != null )
                    {
                        txn.GatewayEntityTypeId = gatewayEntity.Id;
                    }
                    else
                    {
                        txn.GatewayEntityTypeId = null;
                    }
                }
                else
                {
                    txn.GatewayEntityTypeId = null;
                }

                txn.TransactionCode = tbTransactionCode.Text;
                txn.CurrencyTypeValueId = ddlCurrencyType.SelectedValueAsInt();
                txn.CreditCardTypeValueId = ddlCreditCardType.SelectedValueAsInt();
                txn.Summary = tbSummary.Text;

                if (!Page.IsValid || !txn.IsValid)
                {
                    return;
                }

                foreach( var txnDetail in TransactionDetailsState)
                {
                    if (!txnDetail.IsValid)
                    {
                        return;
                    }
                }

                foreach( var txnImage in TransactionImagesState)
                {
                    if (!txnImage.IsValid)
                    {
                        return;
                    }
                }

                rockContext.WrapTransaction( () =>
                {
                    // Save the transaction
                    rockContext.SaveChanges();

                    // Delete any transaction details that were removed
                    var txnDetailsInDB = txnDetailService.Queryable().Where( a => a.TransactionId.Equals( txn.Id ) ).ToList();
                    var deletedDetails = from txnDetail in txnDetailsInDB
                                         where !TransactionDetailsState.Select( d => d.Guid ).Contains( txnDetail.Guid )
                                         select txnDetail;
                    deletedDetails.ToList().ForEach( txnDetail =>
                    {
                        txnDetailService.Delete( txnDetail );
                    } );
                    rockContext.SaveChanges();

                    // Save Transaction Details
                    foreach ( var editorTxnDetail in TransactionDetailsState )
                    {
                        // Add or Update the activity type
                        var txnDetail = txn.TransactionDetails.FirstOrDefault( d => d.Guid.Equals( editorTxnDetail.Guid ) );
                        if ( txnDetail == null )
                        {
                            txnDetail = new FinancialTransactionDetail();
                            txnDetail.Guid = editorTxnDetail.Guid;
                            txn.TransactionDetails.Add( txnDetail );
                        }
                        txnDetail.AccountId = editorTxnDetail.AccountId;
                        txnDetail.Amount = editorTxnDetail.Amount;
                        txnDetail.Summary = editorTxnDetail.Summary;
                    }
                    rockContext.SaveChanges();

                    // Remove any images that do not have a binary file
                    foreach( var txnImage in TransactionImagesState.Where( i => i.BinaryFileId == 0).ToList())
                    {
                        TransactionImagesState.Remove( txnImage );
                    }

                    // Delete any transaction images that were removed
                    var txnImagesInDB = txnImageService.Queryable().Where( a => a.TransactionId.Equals( txn.Id ) ).ToList();
                    var deletedImages = from txnImage in txnImagesInDB
                                         where !TransactionImagesState.Select( d => d.Guid ).Contains( txnImage.Guid )
                                         select txnImage;
                    deletedImages.ToList().ForEach( txnImage =>
                    {
                        txnImageService.Delete( txnImage );
                    } );
                    rockContext.SaveChanges();

                    // Save Transaction Images
                    foreach ( var editorTxnImage in TransactionImagesState )
                    {
                        // Add or Update the activity type
                        var txnImage = txn.Images.FirstOrDefault( d => d.Guid.Equals( editorTxnImage.Guid ) );
                        if ( txnImage == null )
                        {
                            txnImage = new FinancialTransactionImage();
                            txnImage.Guid = editorTxnImage.Guid;
                            txn.Images.Add( txnImage );
                        }
                        txnImage.BinaryFileId = editorTxnImage.BinaryFileId;
                        txnImage.TransactionImageTypeValueId = editorTxnImage.TransactionImageTypeValueId;
                    }
                    rockContext.SaveChanges();

                    // Make sure updated binary files are not temporary
                    var savedBinaryFileIds = txn.Images.Select( i => i.BinaryFileId ).ToList();
                    foreach ( var binaryFile in binaryFileService.Queryable().Where( f => savedBinaryFileIds.Contains( f.Id)))
                    {
                        binaryFile.IsTemporary = false;
                    }

                    // Delete any orphaned images
                    var orphanedBinaryFileIds = BinaryFileIds.Where( f => !savedBinaryFileIds.Contains( f ) );
                    foreach ( var binaryFile in binaryFileService.Queryable().Where( f => orphanedBinaryFileIds.Contains( f.Id ) ) )
                    {
                        binaryFileService.Delete( binaryFile );
                    }

                    rockContext.SaveChanges();

                } );

                // Requery the batch to support EF navigation properties
                var savedTxn = GetTransaction( txn.Id );
                ShowReadOnlyDetails( savedTxn );
            }
        }

        /// <summary>
        /// Handles the Click event of the lbCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbCancel_Click( object sender, EventArgs e )
        {
            int txnId = hfTransactionId.ValueAsInt();
            if ( txnId != 0 )
            {
                ShowReadOnlyDetails( GetTransaction( txnId ) );
            }
            else
            {
                NavigateToParentPage();
            }
        }

        /// <summary>
        /// Handles the BlockUpdated event of the Block control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            int txnId = hfTransactionId.ValueAsInt();
            if ( txnId != 0 )
            {
                ShowReadOnlyDetails( GetTransaction( txnId ) );
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlCurrencyType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlCurrencyType_SelectedIndexChanged( object sender, EventArgs e )
        {
            SetCreditCardVisibility();
        }

        #endregion

        #region Account Transaction Details

        /// <summary>
        /// Handles the AddClick event of the gAccountsEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void gAccountsEdit_AddClick( object sender, EventArgs e )
        {
            ShowAccountDialog( Guid.NewGuid() );
        }

        /// <summary>
        /// Handles the RowSelected event of the gTransactionDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gAccountsEdit_RowSelected( object sender, RowEventArgs e )
        {
            Guid? guid = e.RowKeyValue.ToString().AsGuidOrNull();
            if (guid.HasValue)
            {
                ShowAccountDialog( guid.Value );
            }
        }

        /// <summary>
        /// Handles the DeleteClick event of the gAccountsEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gAccountsEdit_DeleteClick( object sender, RowEventArgs e )
        {
            Guid? guid = e.RowKeyValue.ToString().AsGuidOrNull();
            if ( guid.HasValue )
            {
                var txnDetail = TransactionDetailsState.Where( t => t.Guid.Equals( guid.Value ) ).FirstOrDefault();
                if ( txnDetail != null )
                {
                    TransactionDetailsState.Remove( txnDetail );
                }

                BindAccountsEditGrid();
            }
        }

        /// <summary>
        /// Handles the GridRebind event of the gAccountsEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void gAccountsEdit_GridRebind( object sender, EventArgs e )
        {
            BindAccountsEditGrid();
        }

        /// <summary>
        /// Handles the SaveClick event of the mdAccount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void mdAccount_SaveClick( object sender, EventArgs e )
        {
            Guid? guid = hfAccountGuid.Value.AsGuidOrNull();
            if (guid.HasValue)
            {
                var txnDetail = TransactionDetailsState.Where( t => t.Guid.Equals( guid.Value ) ).FirstOrDefault();
                if (txnDetail == null)
                {
                    txnDetail = new FinancialTransactionDetail();
                    TransactionDetailsState.Add(txnDetail);
                }
                txnDetail.AccountId = ddlAccount.SelectedValue.AsInteger();
                txnDetail.Amount = tbAccountAmount.Text.AsDecimal();
                txnDetail.Summary = tbAccountSummary.Text;

                BindAccountsEditGrid();
            }

            HideDialog();
        }

        #endregion

        #region Image Details

        /// <summary>
        /// Handles the ItemDataBound event of the dlImages control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataListItemEventArgs"/> instance containing the event data.</param>
        protected void dlImages_ItemDataBound( object sender, DataListItemEventArgs e )
        {
            if ( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
            {
                var txnImage = e.Item.DataItem as FinancialTransactionImage;
                var ddlImageType = e.Item.FindControl( "ddlImageType" ) as RockDropDownList;
                var imgupImage = e.Item.FindControl( "imgupImage" ) as Rock.Web.UI.Controls.ImageUploader;
                if ( txnImage != null && ddlImageType != null )
                {
                    ddlImageType.BindToDefinedType( DefinedTypeCache.Read( Rock.SystemGuid.DefinedType.FINANCIAL_TRANSACTION_IMAGE_TYPE.AsGuid() ) );
                    ddlImageType.SetValue( txnImage.TransactionImageTypeValueId );
                    if ( txnImage.BinaryFileId != 0 )
                    {
                        imgupImage.BinaryFileId = txnImage.BinaryFileId;
                    }
                    else
                    {
                        imgupImage.BinaryFileId = null;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the FileSaved event of the imageEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void imageEditor_FileSaved( object sender, EventArgs e )
        {
            var imageEditor = sender as ImageEditor;
            if (imageEditor != null && imageEditor.BinaryFileId.HasValue)
            {
                BinaryFileIds.Add( imageEditor.BinaryFileId.Value );
            }
        }

        /// <summary>
        /// Handles the Click event of the lbAddImage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbAddImage_Click( object sender, EventArgs e )
        {
            TransactionImagesState.Add( new FinancialTransactionImage { Guid = Guid.NewGuid() } );
            BindImages();
        }

        #endregion

        #endregion Events

        #region Methods

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        private FinancialTransaction GetTransaction( int transactionId, RockContext rockContext = null )
        {
            rockContext = rockContext ?? new RockContext();
            var txn = new FinancialTransactionService( rockContext )
                .Queryable( "AuthorizedPerson,TransactionTypeValue,SourceTypeValue,GatewayEntityType,CurrencyTypeValue,TransactionDetails,Images.TransactionImageTypeValue,ScheduledTransaction,ProcessedByPersonAlias.Person" )
                .Where( t => t.Id == transactionId )
                .FirstOrDefault();
            return txn;
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        public void ShowDetail(int transactionId)
        {
            ShowDetail( transactionId, null );
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        public void ShowDetail( int transactionId, int? batchId )
        {
            FinancialTransaction txn = null;

            bool editAllowed = true;

            var rockContext = new RockContext();

            if ( !transactionId.Equals( 0 ) )
            {
                txn = GetTransaction( transactionId, rockContext );
                if (txn != null)
                {
                    editAllowed = txn.IsAuthorized(Authorization.EDIT, CurrentPerson );
                }
            }

            if ( txn == null )
            {
                txn = new FinancialTransaction { Id = 0 };
                txn.BatchId = batchId;
            }

            hfTransactionId.Value = txn.Id.ToString();
            hfBatchId.Value = batchId.HasValue ? batchId.Value.ToString() : string.Empty;

            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;
            if ( !editAllowed || !IsUserAuthorized( Authorization.EDIT ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( FinancialTransaction.FriendlyTypeName );
            }

            if ( readOnly )
            {
                lbEdit.Visible = false;
                ShowReadOnlyDetails( txn );
            }
            else
            {
                lbEdit.Visible = true;
                if ( txn.Id > 0 )
                {
                    ShowReadOnlyDetails( txn );
                }
                else
                {
                    ShowEditDetails( txn, rockContext );
                }
            }

            lbSave.Visible = !readOnly;
        }

        /// <summary>
        /// Shows the read only details.
        /// </summary>
        /// <param name="txn">The TXN.</param>
        private void ShowReadOnlyDetails( FinancialTransaction txn )
        {
            SetEditMode( false );

            if ( txn != null )
            {
                hfTransactionId.Value = txn.Id.ToString();

                var detailsLeft = new DescriptionList()
                    .Add("Person", txn.AuthorizedPerson != null ? txn.AuthorizedPerson.FullName : string.Empty)
                    .Add( "Amount", ( txn.TransactionDetails.Sum( d => (decimal?)d.Amount ) ?? 0.0M ).ToString( "C2" ) )
                    .Add( "Date/Time", txn.TransactionDateTime.HasValue ? txn.TransactionDateTime.Value.ToString( "g" ) : string.Empty )
                    .Add( "Type", txn.TransactionTypeValue != null ? txn.TransactionTypeValue.Name : string.Empty )
                    .Add("Source", txn.SourceTypeValue != null ? txn.SourceTypeValue.Name : string.Empty);

                if (txn.GatewayEntityType != null)
                {
                    detailsLeft.Add( "Payment Gateway", Rock.Financial.GatewayContainer.GetComponentName( txn.GatewayEntityType.Name ) );
                }

                detailsLeft.Add("Transaction Code", txn.TransactionCode);

                if (txn.CurrencyTypeValue != null)
                {
                    string currencyType = txn.CurrencyTypeValue.Name;
                    if (txn.CurrencyTypeValue.Guid.Equals(Rock.SystemGuid.DefinedValue.CURRENCY_TYPE_CREDIT_CARD.AsGuid()))
                    {
                        currencyType += txn.CreditCardTypeValue != null ? (" - " + txn.CreditCardTypeValue.Name) : string.Empty;
                    }
                    detailsLeft.Add( "Currency Type", currencyType);
                }

                detailsLeft.Add( "Summary", txn.Summary );
                
                lDetailsLeft.Text = detailsLeft.Html;

                gAccountsView.DataSource = txn.TransactionDetails.ToList();
                gAccountsView.DataBind();

                //if (txn.ScheduledTransaction != null)
                //{
                //    var qryParam = new Dictionary<string, string>();
                //    qryParam.Add("Txn", txn.ScheduledTransaction.Id.ToString());
                //    string url = LinkedPageUrl("ScheduledTransactionDetailPage", qryParam);
                //    detailsRight.Add( "Scheduled Transaction", !string.IsNullOrWhiteSpace( url ) ?
                //        string.Format( "<a href='{0}'>{1}</a>", url, txn.ScheduledTransaction.GatewayScheduleId ) :
                //        txn.ScheduledTransaction.GatewayScheduleId );
                //}

                //if (txn.ProcessedByPersonAlias != null && txn.ProcessedByPersonAlias.Person != null && txn.ProcessedDateTime.HasValue)
                //{
                //    detailsRight.Add( "Matched By", string.Format( "{0} ({1})", txn.ProcessedByPersonAlias.Person.FullName, txn.ProcessedDateTime.Value.ToString( "g" ) ) );
                //}

                //lDetailsRight.Text = detailsRight.Html;

                if (txn.Images.Any())
                {
                    var primaryImage = txn.Images
                        .OrderBy( i => i.TransactionImageTypeValue.Order)
                        .FirstOrDefault();
                    imgPrimary.ImageUrl = string.Format( "~/GetImage.ashx?id={0}", primaryImage.BinaryFileId );
                    imgPrimary.Visible = true;

                    rptrImages.DataSource = txn.Images
                        .Where( i => !i.Id.Equals( primaryImage.Id ) )
                        .OrderBy( i => i.TransactionImageTypeValue.Order )
                        .ToList();
                    rptrImages.DataBind();
                }
                else
                {
                    imgPrimary.Visible = false;
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="txn">The TXN.</param>
        private void ShowEditDetails( FinancialTransaction txn, RockContext rockContext )
        {
            if (txn != null)
            {
                BindDropdowns( rockContext );

                hfTransactionId.Value = txn.Id.ToString();

                SetEditMode( true );

                ppAuthorizedPerson.SetValue( txn.AuthorizedPerson );
                dtTransactionDateTime.SelectedDateTime = txn.TransactionDateTime;
                ddlTransactionType.SetValue( txn.TransactionTypeValueId );
                ddlSourceType.SetValue( txn.SourceTypeValueId );
                cpPaymentGateway.SetValue( txn.GatewayEntityType != null ? txn.GatewayEntityType.Guid.ToString().ToUpper() : string.Empty );
                tbTransactionCode.Text = txn.TransactionCode;
                ddlCurrencyType.SetValue( txn.CurrencyTypeValueId );
                ddlCreditCardType.SetValue( txn.CreditCardTypeValueId );
                SetCreditCardVisibility();

                TransactionDetailsState = txn.TransactionDetails.ToList();
                TransactionImagesState = txn.Images.ToList();

                BindAccountsEditGrid();
                tbSummary.Text = txn.Summary;

                //if ( txn.ScheduledTransaction != null )
                //{
                //    var qryParam = new Dictionary<string, string>();
                //    qryParam.Add( "Txn", txn.ScheduledTransaction.Id.ToString() );
                //    string url = LinkedPageUrl( "ScheduledTransactionDetailPage", qryParam );

                //    lScheduledTransaction.Text = !string.IsNullOrWhiteSpace( url ) ?
                //        string.Format( "<a href='{0}'>{1}</a>", url, txn.ScheduledTransaction.GatewayScheduleId ) :
                //        txn.ScheduledTransaction.GatewayScheduleId;
                //    lScheduledTransaction.Visible = true;
                //}
                //else
                //{
                //    lScheduledTransaction.Visible = false;
                //}

                //if ( txn.ProcessedByPersonAlias != null && txn.ProcessedByPersonAlias.Person != null && txn.ProcessedDateTime.HasValue )
                //{
                //    lProcessedBy.Text = string.Format( "{0} ({1})", txn.ProcessedByPersonAlias.Person.FullName, txn.ProcessedDateTime.Value.ToString( "g" ) );
                //}

                BinaryFileIds = txn.Images.Select( i => i.BinaryFileId ).ToList();

                BindImages();
            }
        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;
            valSummaryTop.Enabled = editable;
            fieldsetViewSummary.Visible = !editable;
        }

        /// <summary>
        /// Binds the dropdowns.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        private void BindDropdowns( RockContext rockContext )
        {
            ddlTransactionType.BindToDefinedType( DefinedTypeCache.Read( Rock.SystemGuid.DefinedType.FINANCIAL_TRANSACTION_TYPE.AsGuid(), rockContext ) );
            ddlSourceType.BindToDefinedType( DefinedTypeCache.Read( Rock.SystemGuid.DefinedType.FINANCIAL_SOURCE_TYPE.AsGuid(), rockContext ), true );
            ddlCurrencyType.BindToDefinedType( DefinedTypeCache.Read( Rock.SystemGuid.DefinedType.FINANCIAL_CURRENCY_TYPE.AsGuid(), rockContext ), true );
            ddlCreditCardType.BindToDefinedType( DefinedTypeCache.Read( Rock.SystemGuid.DefinedType.FINANCIAL_CREDIT_CARD_TYPE.AsGuid(), rockContext ), true );

            ddlAccount.DataSource = AccountNames;
            ddlAccount.DataBind();
        }

        /// <summary>
        /// Sets the credit card visibility.
        /// </summary>
        private void SetCreditCardVisibility()
        {
            int? currencyType = ddlCurrencyType.SelectedValueAsInt();
            var creditCardCurrencyType = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.CURRENCY_TYPE_CREDIT_CARD );
            ddlCreditCardType.Visible = currencyType.HasValue && currencyType.Value == creditCardCurrencyType.Id;
        }

        /// <summary>
        /// Binds the transaction details.
        /// </summary>
        private void BindAccountsEditGrid()
        {
            gAccountsEdit.DataSource = TransactionDetailsState;
            gAccountsEdit.DataBind();
        }

        private void BindImages()
        {
            dlImages.DataSource = TransactionImagesState;
            dlImages.DataBind();
        }

        /// <summary>
        /// Shows the account dialog.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        private void ShowAccountDialog( Guid guid )
        {
            hfAccountGuid.Value = guid.ToString();

            var txnDetail = TransactionDetailsState.Where( d => d.Guid.Equals( guid ) ).FirstOrDefault();
            if ( txnDetail != null )
            {
                ddlAccount.SetValue( txnDetail.AccountId );
                tbAccountAmount.Text = txnDetail.Amount.ToString( "N2" );
                tbAccountSummary.Text = txnDetail.Summary;
            }
            else
            {
                ddlAccount.SelectedIndex = -1;
                tbAccountSummary.Text = string.Empty;
                tbAccountSummary.Text = string.Empty;
            }

            ShowDialog( "ACCOUNT" );
        }
        
        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="dialog">The dialog.</param>
        private void ShowDialog( string dialog )
        {
            hfActiveDialog.Value = dialog.ToUpper().Trim();
            ShowDialog();
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        private void ShowDialog()
        {
            switch ( hfActiveDialog.Value )
            {
                case "ACCOUNT":
                    mdAccount.Show();
                    break;
            }
        }

        /// <summary>
        /// Hides the dialog.
        /// </summary>
        private void HideDialog()
        {
            switch ( hfActiveDialog.Value )
            {
                case "ACCOUNT":
                    mdAccount.Hide();
                    break;
            }

            hfActiveDialog.Value = string.Empty;
        }

        /// <summary>
        /// Accounts the name.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns></returns>
        protected string AccountName( int accountId )
        {
            return AccountNames.ContainsKey(accountId) ? AccountNames[accountId] : "";
        }

        /// <summary>
        /// Images the URL.
        /// </summary>
        /// <param name="binaryFileId">The binary file identifier.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <returns></returns>
        protected string ImageUrl (int binaryFileId, int? maxWidth = null, int? maxHeight = null)
        {
            string width = maxWidth.HasValue ? string.Format( "&maxWidth={0}", maxWidth.Value ) : string.Empty;
            string height = maxHeight.HasValue ? string.Format( "&maxHeight={0}", maxHeight.Value ) : string.Empty;
            return ResolveRockUrl( string.Format( "~/GetImage.ashx?id={0}{1}{2}", binaryFileId, width, height ) );
        }

        #endregion 

}
}