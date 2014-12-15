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
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using Rock.Store;
using NuGet;
using Rock.VersionInfo;

namespace RockWeb.Blocks.Store
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Package Detail" )]
    [Category( "Store" )]
    [Description( "Manages the details of a package." )]
    public partial class PackageDetail : Rock.Web.UI.RockBlock
    {
        #region Fields

        // used for private variables

        #endregion

        #region Properties

        // used for public / protected properties

        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                ShowPackage();
            }
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            ShowPackage();
        }

        #endregion

        #region Methods

        private void ShowPackage()
        {
            // get package id
            int packageId = -1;

            if ( !string.IsNullOrWhiteSpace( PageParameter( "PackageId" ) ) )
            {
                packageId = Convert.ToInt32( PageParameter( "PackageId" ) );
            }

            PackageService packageService = new PackageService();
            var package = packageService.GetPackage( packageId );

            lPackageName.Text = package.Name;
            lPackageDescription.Text = package.Description;
            lVendorName.Text = package.Vendor.Name;
            imgPackageImage.ImageUrl = package.PackageIconBinaryFile.ImageUrl;
            lbPackageLink.PostBackUrl = package.SupportUrl;
            lRatingSummary.Text = string.Format( "<div class='rating rating-{0}'><small>{1}</small></div>", package.Rating.ToString().Replace( ".", "" ) );

            lAuthorInfo.Text = string.Format( "<a href='{0}'>{1}</a>", package.Vendor.Url, package.Vendor.Name );

            if ( package.IsFree )
            {
                lCost.Text = "<div class='pricelabel free'><h4>Free</h4></div>";
            }
            else
            {
                lCost.Text = string.Format( "<div class='pricelabel cost'><h4>${0}</h4></div>", package.Price );
            }

            // get latest version
            PackageVersion latestVersion = null;
            if ( package.Versions.Count > 0 )
            {
                SemanticVersion rockVersion = new SemanticVersion( VersionInfo.GetRockProductVersionNumber() );
                latestVersion = package.Versions.Where( v => v.RequiredRockSemanticVersion <= rockVersion ).OrderByDescending(v => v.Id).FirstOrDefault();
            }

            if ( latestVersion != null )
            {
                lLatestVersionLabel.Text = latestVersion.VersionLabel;
                lLatestVersionDescription.Text = latestVersion.Description;

                // alert the user if a newer version exists but requires a rock update
                if ( package.Versions.Where( v => v.Id > latestVersion.Id ).Count() > 0 )
                {
                    var lastVersion = package.Versions.OrderByDescending( v => v.RequiredRockSemanticVersion ).FirstOrDefault();
                    lVersionWarning.Text = string.Format( "<div class='alert alert-info'>A newer version of this item is available but requires Rock v{0}.{1}.</div>",
                                                    lastVersion.RequiredRockSemanticVersion.Version.Minor.ToString(),
                                                    lastVersion.RequiredRockSemanticVersion.Version.MinorRevision.ToString() );
                }

                lLastUpdate.Text = latestVersion.DateAdded.ToShortDateString();
                lRequiredRockVersion.Text = string.Format("v{0}.{1}", 
                                                latestVersion.RequiredRockSemanticVersion.Version.Minor.ToString(),
                                                latestVersion.RequiredRockSemanticVersion.Version.MinorRevision.ToString());
                lDocumenationLink.Text = string.Format( "<a href='{0}'>Support Link</a>", latestVersion.DocumentationUrl );

                // fill in previous version info
                rptAdditionalVersions.DataSource = package.Versions.Where( v => v.Id < latestVersion.Id );
                rptAdditionalVersions.DataBind();

                // get the details for the latest version
                PackageVersion = new PackageVersionService().GetPackageVersion( latestVersion.Id );
            }
            else
            {
                // display info on what Rock version you need to be on to run this package
                if ( package.Versions.Count > 0 )
                {
                    var firstVersion = package.Versions.OrderBy( v => v.RequiredRockSemanticVersion ).FirstOrDefault();
                    var lastVersion = package.Versions.OrderByDescending( v => v.RequiredRockSemanticVersion ).FirstOrDefault();

                    if ( firstVersion == lastVersion )
                    {
                        lVersionWarning.Text = string.Format( "<div class='alert alert-warning'>This item requires Rock version v{0}.{1}.</div>",
                                                    lastVersion.RequiredRockSemanticVersion.Version.Minor.ToString(),
                                                    lastVersion.RequiredRockSemanticVersion.Version.MinorRevision.ToString() );
                    }
                    else
                    {
                        lVersionWarning.Text = string.Format( "<div class='alert alert-warning'>This item requires at least Rock version v{0}.{1} but the latest version requires v{2}.{3}.</div>",
                                                    firstVersion.RequiredRockSemanticVersion.Version.Minor.ToString(),
                                                    firstVersion.RequiredRockSemanticVersion.Version.MinorRevision.ToString(),
                                                    lastVersion.RequiredRockSemanticVersion.Version.Minor.ToString(),
                                                    lastVersion.RequiredRockSemanticVersion.Version.MinorRevision.ToString() );
                    }
                }
                
            }

        }

        #endregion
    }
}