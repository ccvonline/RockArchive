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
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using SharpKml.Engine;
using SharpKml.Base;
using SharpKml.Dom;

namespace RockWeb.Blocks.Groups
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Groups KML" )]
    [Category( "Groups" )]
    [Description( "Block that allows the import and export of group geographies to a KML/KMZ format." )]
    public partial class GroupsKml : Rock.Web.UI.RockBlock
    {
        #region Fields

        // used for private variables
        enum ExportType
        {
            Point,
            GeoFence
        };

        int colorIndex = 0;
        
        Color32[] MapColors =   new Color32[] { 
                                    new Color32( 150, 199, 211, 141 ),
                                    new Color32( 150, 179, 255, 255 ),
                                    new Color32( 150, 218, 186, 190 ),
                                    new Color32( 150, 114, 128, 251 ),
                                    new Color32( 150, 211, 177, 128 ),
                                    new Color32( 150, 98, 180, 253 ),
                                    new Color32( 150, 105, 222, 179 ),
                                    new Color32( 150, 229, 205, 252 ),
                                    new Color32( 150, 217, 217, 217 ),
                                    new Color32( 150, 189, 128, 188 ),
                                    new Color32( 150, 197, 235, 204 ),
                                    new Color32( 150, 111, 237, 255 )
                                };

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
                //gtpExportGroupType.GroupTypes =  new GroupTypeService().Queryable().ToList();
                /*ddlExportGroupTypes.DataValueField = "Id";
                ddlExportGroupTypes.DataTextField = "Name";
                ddlExportGroupTypes.DataSource = new GroupTypeService().Queryable().Select( g => new { g.Id, g.Name } ).ToList();
                ddlExportGroupTypes.DataBind();
                ddlExportGroupTypes.Items.Insert( 0, "" );

                ddlExportGroupLocationType.DataValueField = "Id";
                ddlExportGroupLocationType.DataTextField = "Name";*/
            }
        }

        #endregion

        #region Events

        // handlers called by the controls on your block


        /*protected void gtpExportGroupType_SelectedIndexChanged( object sender, EventArgs e )
        {
            string test = "";
        }*/

        /*protected void ddlExportGroupTypes_SelectedIndexChanged( object sender, EventArgs e )
        {
            int selectedGroupTypeId = Int32.Parse( ddlExportGroupTypes.SelectedValue );

            GroupTypeService groupTypeService = new GroupTypeService();
            var selectedGroupType = groupTypeService.Get( selectedGroupTypeId );

            ddlExportGroupLocationType.DataSource = selectedGroupType.LocationTypes.ToList();
        }*/

        protected void btnExport_Click( object sender, EventArgs e )
        {
            int groupTypeId = 28;
            int locationTypeValueId = 212;
            bool exportAsKmz = true;

            ExportType exportType = ExportType.GeoFence;


            GroupService groupService = new GroupService();
            var groups = groupService.Queryable()
                            .Where( g => g.GroupTypeId == groupTypeId )
                            .Select( g => new { 
                                g.Id,
                                g.Guid,
                                g.Name,
                                g.Description,
                                GroupLocation = g.GroupLocations.Where( l => l.GroupLocationTypeValueId == locationTypeValueId ).FirstOrDefault()
                            } ).ToList();

            // create kml document
            Document document = new Document();
            document.Name = String.Format("Rock Group Export ({0})", RockDateTime.Now.ToShortDateString());
            document.Open = false;
            

            foreach ( var group in groups )
            {
                Placemark placemark = new Placemark();
                placemark.Name = group.Name;
                placemark.Id = group.Guid.ToString();

                Description description = new Description();
                description.Text = group.Description ;
                placemark.Description = description;

                switch ( exportType )
                {
                    case ExportType.GeoFence:
                        if ( group.GroupLocation.Location != null && group.GroupLocation.Location.GeoFence != null )
                        {

                            //group.GroupLocation.Location.GeoFence.
                            OuterBoundary outerBoundary = new OuterBoundary();
                            outerBoundary.LinearRing = new LinearRing();
                            outerBoundary.LinearRing.Coordinates = new CoordinateCollection();

                            // get points
                            string groupFence = group.GroupLocation.Location.GeoFence.ProviderValue.ToString();
                            string match = @"POLYGON \(\(([0-9\.\-\,\s]+)\)\)";
                            string longSpaceLatComma = Regex.Replace( groupFence, match, "$1", RegexOptions.IgnoreCase );
                            string[] points = longSpaceLatComma.Split( ',' );

                            foreach ( string point in points )
                            {
                                string[] longLat;
                                longLat = point.Trim().Split( ' ' );
                                outerBoundary.LinearRing.Coordinates.Add( new Vector( Convert.ToDouble(longLat[1]), Convert.ToDouble(longLat[0]), 0 ) );
                            }

                            Polygon polygon = new Polygon();
                            polygon.OuterBoundary = outerBoundary;

                            // set the color of the polygon
                            var style = new SharpKml.Dom.Style();

                            style.Polygon = new PolygonStyle();
                            style.Polygon.ColorMode = SharpKml.Dom.ColorMode.Normal;
                            style.Polygon.Color = GetColor();

                            placemark.Geometry = polygon;
                            placemark.AddStyle( style);
                            document.AddFeature( placemark );
                        }
                        break;
                }
               
            }

            Kml root = new Kml();
            root.Feature = document;
            KmlFile kmlFile = KmlFile.Create(root, false);

            if ( exportAsKmz == false )
            {
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearHeaders();
                response.ClearContent();
                response.Clear();
                response.ContentType = "application/vnd.google-earth.kml+xml";
                response.AddHeader( "Content-Disposition", "attachment; filename=\"rock-groups.kml\"" ); 
                response.Charset = "";

                var stream = new MemoryStream();
                kmlFile.Save( stream );
                response.BinaryWrite( stream.ToArray() );

                response.Flush();
                response.End();
            }
            else
            {
                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                response.ClearHeaders();
                response.ClearContent();
                response.Clear();
                response.ContentType = "application/vnd.google-earth.kmz";
                response.AddHeader( "Content-Disposition", "attachment; filename=\"rock-groups.kmz\"" ); 
                response.Charset = "";

                var stream = new MemoryStream();
                KmzFile kmzFile = KmzFile.Create( kmlFile );
                kmzFile.Save( stream );
                response.BinaryWrite( stream.ToArray() );

                response.Flush();
                response.End();
            }
            
            
        }

        #endregion

        #region Methods

        private Color32 GetColor(  )
        {
            if ( colorIndex == MapColors.Length )
            {
                colorIndex = 0;
            }

            Color32 returnColor = MapColors[colorIndex];
            colorIndex++;

            return returnColor;
        }

       

        #endregion

        
}
}