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
using Humanizer;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using System.Text;
using dotless.Core;

namespace RockWeb.Blocks.Cms
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Theme Styler" )]
    [Category( "CMS" )]
    [Description( "Allows you to change the LESS variables of a theme." )]

    public partial class ThemeStyler : Rock.Web.UI.RockBlock
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

            string script = @"
$('.js-panel-toggle').on('click', function (e) {
    e.stopImmediatePropagation();
    e.preventDefault();
    var $header = $(this).closest('.panel-heading');
    $header.siblings('.panel-body').slideToggle();
    $header.find('i').toggleClass('fa-chevron-up fa-chevron-down');
});
";
            ScriptManager.RegisterStartupScript( this.Page, this.Page.GetType(), "DefinedValueChecklistScript", script, true );
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
                // load themes
                DirectoryInfo dInfo = new DirectoryInfo( Request.PhysicalApplicationPath + "Themes" );
                DirectoryInfo[] subdirs = dInfo.GetDirectories();

                ddlTheme.DataSource = subdirs;
                ddlTheme.DataTextField = "Name";
                ddlTheme.DataValueField = "Name";
                ddlTheme.DataBind();

                ddlTheme.Items.Insert( 0, "" );
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

        }


        protected void ddlTheme_SelectedIndexChanged( object sender, EventArgs e )
        {
            // load the theme
            if ( !string.IsNullOrWhiteSpace( ddlTheme.SelectedValue )){

                btnSave.Visible = true;

                BuildControls();
            } else
            {
                btnSave.Visible = false;
            }
        }

        protected void btnSave_Click( object sender, EventArgs e )
        {
            string variableFile = string.Format( @"{0}Themes/{1}/Styles/_variables.less", Request.PhysicalApplicationPath, ddlTheme.SelectedValue );
            string variableOverrideFile = string.Format( @"{0}Themes/{1}/Styles/_variable-overrides.less", Request.PhysicalApplicationPath, ddlTheme.SelectedValue );

            // get list of original values
            Dictionary<string, string> overrides = GetVariables( variableFile );

            StringBuilder overrideFile = new StringBuilder();

            foreach (var control in phThemeControls.Controls )
            {
                var variableName = "";
            }

            overrideFile.Append( "/* Custom CSS */");

            //System.IO.StreamWriter file = new System.IO.StreamWriter( variableOverrideFile );
            //file.WriteLine( overrideFile );
        }
        #endregion

        #region Methods

        private void BuildControls()
        {
            string variableFile = string.Format( @"{0}Themes/{1}/Styles/_variables.less", Request.PhysicalApplicationPath, ddlTheme.SelectedValue );
            string variableOverrideFile = string.Format( @"{0}Themes/{1}/Styles/_variable-overrides.less", Request.PhysicalApplicationPath, ddlTheme.SelectedValue );

            if ( !File.Exists( variableFile ) || !File.Exists( variableOverrideFile ) )
            {
                nbMessages.NotificationBoxType = NotificationBoxType.Warning;
                nbMessages.Text = "This theme does not have a variables file(s) to allow overriding.";
                return;
            }



            // get list of current overrides
            Dictionary<string, string> overrides = GetVariables( variableOverrideFile );

            bool inPanel = false;

            foreach ( string line in File.ReadLines( variableFile ) )
            {
                if ( line.Left( 4 ) == @"//--" )
                {
                    Literal spacing = new Literal();
                    spacing.Text = "<br/><br/>";
                    phThemeControls.Controls.Add( spacing );
                }
                else if ( line.Left( 4 ) == @"////" )
                {
                    Literal header = new Literal();
                    header.Text = string.Format( "<h4>{0}</h4>", line.Replace( @"/", "" ) );
                    phThemeControls.Controls.Add( header );
                }
                else if ( line.Left( 2 ) == @"//" )
                {
                    string title = line.Substring( 2 );

                    StringBuilder content = new StringBuilder();

                    // panel
                    if ( inPanel )
                    {
                        content.Append( "</div></div>" );
                    }

                    bool isOpen = false;
                    if ( title.Contains( "*open*" ) )
                    {
                        isOpen = true;
                        title = title.Replace( "*open*", "" );
                    }


                    content.Append( "<div class='panel panel-widget'>" );
                    content.Append( "<div class='panel-heading'>" );

                    if ( isOpen )
                    {
                        content.Append( string.Format( "<h1 class='panel-title'>{0} <div class='pull-right'><a class='btn btn-link btn-xs js-panel-toggle'><i class='fa fa-chevron-up'></i></a></div></h1>", title ) );
                        content.Append( "</div>" );
                        content.Append( "<div class='panel-body'>" );
                    }
                    else
                    {
                        content.Append( string.Format( "<h1 class='panel-title'>{0} <div class='pull-right'><a class='btn btn-link btn-xs js-panel-toggle'><i class='fa fa-chevron-down'></i></a></div></h1>", title ) );
                        content.Append( "</div>" );
                        content.Append( "<div class='panel-body' style='display: none;'>" );
                    }

                    Literal header = new Literal();
                    header.Text = content.ToString();
                    phThemeControls.Controls.Add( header );
                    inPanel = true;
                }
                else if ( line.Left( 1 ) == "@" )
                {
                    // variable
                    char[] delimiters = new char[] { ':', ';' };
                    string[] variableParts = line.Split( delimiters );

                    string helpText = string.Empty;

                    // determine type
                    VariableType variableType = VariableType.Text;
                    if ( variableParts.Length >= 3 )
                    {
                        if ( variableParts[2].Contains( "#color" ) )
                        {
                            variableType = VariableType.Color;
                        }

                        // get help
                        helpText = variableParts[2].Replace( "#color", "" ).Replace( "//", "" ).Trim();
                    }

                    // get variable name
                    string variableName = variableParts[0].Replace( "@", "" ).Replace( "-", " " ).Titleize();

                    // get variable value
                    string variableValue = string.Empty;
                    if ( variableParts.Length > 1 )
                    {
                        variableValue = variableParts[1].Trim();
                    }

                    switch ( variableType )
                    {
                        case VariableType.Color:
                            {
                                if ( phThemeControls.FindControl( variableName ) == null )
                                {
                                    ColorPicker colorPicker = new ColorPicker();
                                    colorPicker.ID = variableName;
                                    colorPicker.Label = variableName;
                                    //colorPicker.CssClass = "input-width-lg";

                                    // check if override of the variable exists
                                    if ( overrides.ContainsKey( variableName ) )
                                    {
                                        colorPicker.Value = overrides[variableName];
                                    }
                                    else
                                    {
                                        colorPicker.Value = variableValue;
                                    }

                                    if ( !string.IsNullOrWhiteSpace( helpText ) )
                                    {
                                        colorPicker.Help = helpText;
                                    }

                                    colorPicker.OriginalValue = variableValue;
                                    colorPicker.RequiredFieldValidator = null;
                                    phThemeControls.Controls.Add( colorPicker );
                                }
                                break;
                            }
                        default:
                            {
                                if ( phThemeControls.FindControl( variableName ) == null )
                                {
                                    RockTextBox textbox = new RockTextBox();
                                    textbox.Label = variableName;
                                    textbox.CssClass = "input-width-xxl";

                                    // check if override of the variable exists
                                    if ( overrides.ContainsKey( variableName ) )
                                    {
                                        textbox.Text = overrides[variableName];
                                    }
                                    else
                                    {
                                        textbox.Text = variableValue;
                                    }

                                    textbox.RequiredFieldValidator = null;
                                    textbox.ID = variableName;
                                    phThemeControls.Controls.Add( textbox );
                                }
                                break;
                            }
                    }
                }
            }

            if ( inPanel )
            {
                Literal header = new Literal();
                header.Text = "</div></div>";
                phThemeControls.Controls.Add( header );
            }
        }

        private Dictionary<string,string> GetVariables(string filePath )
        {
            Dictionary<string, string> overrides = new Dictionary<string, string>();

            if ( File.Exists( filePath ) )
            {
                foreach ( string line in File.ReadLines( filePath ) )
                {
                    if ( line.Replace(" ", "").Contains( "/*Custom CSS*/" ) )
                    {
                        break;
                    }

                    char[] delimiters = new char[] { ':', ';' };
                    string[] variableParts = line.Split( delimiters );

                    if (variableParts.Length > 2 )
                    {
                        overrides.Add( variableParts[0].Replace( "@", "" ), variableParts[1].Trim() );
                    }
                }
            }

            return overrides;
        }

        #endregion

        enum VariableType
        {
            Text,
            Color,
            Number
        }
    }
}