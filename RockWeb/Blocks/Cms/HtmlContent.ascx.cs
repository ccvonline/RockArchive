﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.CMS;

namespace RockWeb.Blocks.Cms
{
    #region Properties

    [Rock.Security.AdditionalActions( new string[] { "Approve" } )]
    [Rock.Attribute.Property( 0, "Pre-Text", "PreText", "", "HTML text to render before the blocks main content.", false, "" )]
    [Rock.Attribute.Property( 1, "Post-Text", "PostText", "", "HTML text to render after the blocks main content.", false, "" )]
    [Rock.Attribute.Property( 2, "Cache Duration", "CacheDuration", "", "Number of seconds to cache the content.", false, "0", "Rock", "Rock.FieldTypes.Integer")]
    [Rock.Attribute.Property( 3, "Context Parameter", "ContextParameter", "", "Query string parameter to use for 'personalizing' content based on unique values.", false, "" )]
    [Rock.Attribute.Property( 4, "Context Name", "ContextName", "", "Name to use to further 'personalize' content.  Blocks with the same name, and referenced with the same context parameter will share html values.", false, "" )]
    [Rock.Attribute.Property( 5, "Support Versions", "Advanced", "Support content versioning?", false, "False", "Rock", "Rock.FieldTypes.Boolean" )]
    [Rock.Attribute.Property( 6, "Require Approval", "Advanced", "Require that content be approved?", false, "False", "Rock", "Rock.FieldTypes.Boolean" )]

    #endregion

    public partial class HtmlContent : Rock.Web.UI.Block
    {
        #region Private Global Variables
 
        bool _supportVersioning = false;
        bool _requireApproval = false;

        #endregion

        #region Overriddend Block Methods

        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            _supportVersioning = bool.Parse( AttributeValue( "SupportVersions" ) ?? "false" );
            _requireApproval = bool.Parse( AttributeValue( "RequireApproval" ) ?? "false");

            this.AttributesUpdated += HtmlContent_AttributesUpdated;

            ShowView();

            rGrid.DataKeyNames = new string[] { "id" };
            rGrid.GridRebind += new Rock.Web.UI.Controls.GridRebindEventHandler( rGrid_GridRebind );
            rGrid.ShowActionRow = false;
        }

        public override List<Control> GetConfigurationControls( bool canConfig, bool canEdit )
        {
            List<Control> configControls = new List<Control>();

            // add edit icon to config controls if user has edit permission
            if ( canConfig || canEdit )
            {
                LinkButton lbEdit = new LinkButton();
                lbEdit.CssClass = "edit icon-button";
                lbEdit.ToolTip = "Edit HTML";
                lbEdit.Click += new EventHandler( lbEdit_Click );
                configControls.Add( lbEdit );
            }

            configControls.AddRange( base.GetConfigurationControls( canConfig, canEdit ) );

            return configControls;
        }

        #endregion

        #region Events

        protected void lbEdit_Click( object sender, EventArgs e )
        {
            phEditContent.Visible = true;

            PageInstance.AddScriptLink( this.Page, "~/scripts/ckeditor/ckeditor.js" );
            PageInstance.AddScriptLink( this.Page, "~/scripts/ckeditor/adapters/jquery.js" );
            PageInstance.AddScriptLink( this.Page, "~/Scripts/Rock/htmlContentOptions.js" );

            HtmlContentService service = new HtmlContentService();
            Rock.CMS.HtmlContent content = service.GetActiveContent( BlockInstance.Id, EntityValue() );
            if ( content == null )
                content = new Rock.CMS.HtmlContent();

            string script = string.Format( @"

    $(document).ready(function () {{

        $('#html-content-editor-{0}').modal({{
            show: true,
            backdrop: true,
            keyboard: true
        }});

        $('#html-content-editor-{0}').bind('shown', function () {{
            $(this).appendTo($('form'));
            $('#html-content-editor-{0} textarea.html-content-editor').ckeditor(ckoptionsAdv).end();
        }});

        $('#html-content-editor-{0}').bind('hide', function () {{
            $('#html-content-editor-{0}').find('textarea.html-content-editor').ckeditorGet().destroy();
        }});

    }});

    Sys.Application.add_load(function () {{

        $('#html-content-edit-{0} .btn').click(function () {{
            $('#html-content-editor-{0}').modal('hide');
        }});

    }});
",
                BlockInstance.Id);
            this.Page.ClientScript.RegisterStartupScript( this.GetType(), string.Format( "edit-html-content-{0}", BlockInstance.Id ), script, true );

            if ( _supportVersioning )
            {
                PageInstance.AddScriptLink( this.Page, "~/scripts/Kendo/kendo.core.min.js" );
                PageInstance.AddScriptLink( this.Page, "~/scripts/Kendo/kendo.fx.min.js" );
                PageInstance.AddScriptLink( this.Page, "~/scripts/Kendo/kendo.popup.min.js" );

                PageInstance.AddScriptLink( this.Page, "~/scripts/Kendo/kendo.calendar.min.js" );
                PageInstance.AddScriptLink( this.Page, "~/scripts/Kendo/kendo.datepicker.min.js" );

                PageInstance.AddCSSLink( this.Page, "~/CSS/Kendo/kendo.common.min.css" );
                PageInstance.AddCSSLink( this.Page, "~/CSS/Kendo/kendo.rock.min.css" );


                script = string.Format( @"

    $(document).ready(function () {{

        $('#{2}').kendoDatePicker({{ open:function(e){{
            window.setTimeout(function(){{ $('.k-calendar-container').parent('.k-animation-container').css('zIndex', '11000'); }}, 1);
        }} }});

        $('#{3}').kendoDatePicker({{ open:function(e){{
            window.setTimeout(function(){{ $('.k-calendar-container').parent('.k-animation-container').css('zIndex', '11000'); }}, 1);
        }} }});

        $('#html-content-version-{0}').click(function () {{
            $('#html-content-versions-{0}').show();
            $('#html-content-edit-{0}').hide();
        }});

        $('#html-content-versions-cancel-{0}').click(function () {{
            $('#html-content-edit-{0}').show();
            $('#html-content-versions-{0}').hide();
            return false;
        }});

        $('a.html-content-show-version-{0}').click(function () {{

            if (CKEDITOR.instances['{5}'].checkDirty() == false ||
                confirm('Loading a previous version will cause any changes you\'ve made to the existing text to be lost.  Are you sure you want to continue?'))
            {{
                $.ajax({{
                    type: 'GET',
                    contentType: 'application/json',
                    dataType: 'json',
                    url: rock.baseUrl + 'REST/CMS/HtmlContent/' + $(this).attr('html-id'),
                    success: function (getData, status, xhr) {{

                        htmlContent = getData;
                        
                        $('#html-content-version-{0}').text('Version ' + htmlContent.Version);
                        $('#{1}').val(htmlContent.Version);
                        $('#{2}').val(htmlContent.StartDateTime);
                        $('#{3}').val(htmlContent.ExpireDateTime);
                        $('#{4}').attr('checked', htmlContent.Approved);

                        CKEDITOR.instances['{5}'].setData(htmlContent.Content, function() {{
                            CKEDITOR.instances['{5}'].resetDirty();
                            $('#html-content-edit-{0}').show();
                            $('#html-content-versions-{0}').hide();
                        }});

                    }},
                    error: function (xhr, status, error) {{
                        alert(status + ' [' + error + ']: ' + xhr.responseText);
                    }}
                }});
            }}
        }});
        

    }});
",
                    BlockInstance.Id,
                    hfVersion.ClientID,
                    tbStartDate.ClientID,
                    tbExpireDate.ClientID,
                    cbApprove.ClientID,
                    txtHtmlContentEditor.ClientID );
                this.Page.ClientScript.RegisterStartupScript( this.GetType(), string.Format( "edit-html-content-version-{0}", BlockInstance.Id ), script, true );

                pnlVersioningHeader.Visible = true;
                cbOverwriteVersion.Visible = true;

                hfVersion.Value = content.Version.ToString();
                lVersion.Text = content.Version.ToString();
                tbStartDate.Text = content.StartDateTime.HasValue ? content.StartDateTime.Value.ToShortDateString() : string.Empty;
                tbExpireDate.Text = content.ExpireDateTime.HasValue ? content.ExpireDateTime.Value.ToShortDateString() : string.Empty;

                if ( _requireApproval )
                {
                    cbApprove.Checked = content.Approved;
                    cbApprove.Enabled = UserAuthorized( "Approve" );
                    cbApprove.Visible = true;
                }
                else
                    cbApprove.Visible = false;
            }
            else
            {
                pnlVersioningHeader.Visible = false;
                cbOverwriteVersion.Visible = false;
            }

            txtHtmlContentEditor.Text = content.Content;

            BindGrid();
        }

        protected void btnSaveContent_Click( object sender, EventArgs e )
        {
            if ( UserAuthorized( "Edit" ) || UserAuthorized( "Configure" ) )
            {
                Rock.CMS.HtmlContent content = null;
                HtmlContentService service = new HtmlContentService();

                // get settings
                string entityValue = EntityValue();

                // get current  content
                int version = 0;
                if ( !Int32.TryParse( hfVersion.Value, out version ) )
                    version = 0;
                content = service.GetByBlockIdAndEntityValueAndVersion( BlockInstance.Id, entityValue, version );

                // if the existing content changed, and the overwrite option was not checked, create a new version
                if ( content != null && 
                    _supportVersioning && 
                    content.Content != txtHtmlContentEditor.Text && 
                    !cbOverwriteVersion.Checked )
                    content = null;

                // if a record doesn't exist then  create one
                if ( content == null )
                {
                    content = new Rock.CMS.HtmlContent();
                    content.BlockId = BlockInstance.Id;
                    content.EntityValue = entityValue;

                    if ( _supportVersioning )
                    {
                        int? maxVersion = service.Queryable().
                            Where( c => c.BlockId == BlockInstance.Id &&
                                c.EntityValue == entityValue ).
                            Select( c => ( int? )c.Version ).Max();

                        content.Version = maxVersion.HasValue ? maxVersion.Value + 1 : 1;
                    }
                    else
                        content.Version = 0;

                    service.Add( content, CurrentPersonId );
                }

                if ( _supportVersioning )
                {
                    DateTime startDate;
                    if ( DateTime.TryParse( tbStartDate.Text, out startDate ) )
                        content.StartDateTime = startDate;
                    else
                        content.StartDateTime = null;

                    DateTime expireDate;
                    if ( DateTime.TryParse( tbExpireDate.Text, out expireDate ) )
                        content.ExpireDateTime = expireDate;
                    else
                        content.ExpireDateTime = null;
                }
                else
                {
                    content.StartDateTime = null;
                    content.ExpireDateTime = null;
                }

                if ( !_requireApproval || UserAuthorized( "Approve" ) )
                {
                    content.Approved = !_requireApproval || cbApprove.Checked;
                    if ( content.Approved )
                    {
                        content.ApprovedByPersonId = CurrentPersonId;
                        content.ApprovedDateTime = DateTime.Now;
                    }
                }

                content.Content = txtHtmlContentEditor.Text;

                service.Save( content, CurrentPersonId );

                // flush cache content 
                this.FlushCacheItem( entityValue );

            }

            ShowView();
        }

        void rGrid_GridRebind( object sender, EventArgs e )
        {
            throw new NotImplementedException();
        }

        void HtmlContent_AttributesUpdated( object sender, EventArgs e )
        {
            lPreText.Text = AttributeValue( "PreText" );
            lPostText.Text = AttributeValue( "PostText" );
        }

        #endregion

        #region Private Methods

        private void ShowView()
        {
            string entityValue = EntityValue();
            string html = "";

            int cacheDuration = Int32.Parse( AttributeValue( "CacheDuration" ) );
            string cachedContent = GetCacheItem( entityValue ) as string;

            // if content not cached load it from DB
            if ( cachedContent == null )
            {
                Rock.CMS.HtmlContent content = new HtmlContentService().GetActiveContent( BlockInstance.Id, entityValue );

                if ( content != null )
                {
                    html = content.Content;

                    // cache content
                    if ( cacheDuration > 0 )
                        AddCacheItem( entityValue, html, cacheDuration );
                }
            }
            else
                html = cachedContent;

            // add content to the content window
            lPreText.Text = AttributeValue( "PreText" );
            lHtmlContent.Text = html;
            lPostText.Text = AttributeValue( "PostText" );
        }

        private void BindGrid()
        {
            HtmlContentService service = new HtmlContentService();

            var versions = service.GetContent(BlockInstance.Id, EntityValue()).
                Select( v => new {
                    v.Id,
                    v.Version,
                    v.Content,
                    ModifiedDateTime = v.ModifiedDateTime.ToElapsedString(),
                    ModifiedByPerson = v.ModifiedByPerson != null ? v.ModifiedByPerson.FullName : "",
                    v.Approved,
                    ApprovedByPerson = v.ApprovedByPerson != null ? v.ApprovedByPerson.FullName : "",
                    v.StartDateTime,
                    v.ExpireDateTime
                }).ToList();

            rGrid.DataSource = versions;
            rGrid.DataBind();
        }

        private string EntityValue()
        {
            string entityValue = string.Empty;

            string contextParameter = AttributeValue( "ContextParameter" );
            if ( !string.IsNullOrEmpty( contextParameter ) )
                entityValue = string.Format( "{0}={1}", contextParameter, PageParameter( contextParameter ) ?? string.Empty );

            string contextParameterValue = PageParameter( contextParameter );
            if ( !string.IsNullOrEmpty( contextParameterValue ) )
                entityValue += "&ContextName=" + contextParameterValue;

            return entityValue;
        }

        #endregion
    }
}

/*
CKEditor Notes:
 * 
 * Toolbar Options: http://docs.cksource.com/CKEditor_3.x/Developers_Guide/Toolbar
 * Config Settings: http://docs.cksource.com/ckeditor_api/index.html

*/