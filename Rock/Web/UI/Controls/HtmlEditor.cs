﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// Control for rendering an html editor
    /// </summary>
    [ToolboxData( "<{0}:LabeledHtmlEditor runat=server></{0}:LabeledHtmlEditor>" )]
    public class HtmlEditor : TextBox, IRockControl
    {
        #region IRockControl implementation

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        /// <value>
        /// The label text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The text for the label." )
        ]
        public string Label
        {
            get { return ViewState["Label"] as string ?? string.Empty; }
            set { ViewState["Label"] = value; }
        }

        /// <summary>
        /// Gets or sets the help text.
        /// </summary>
        /// <value>
        /// The help text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The help block." )
        ]
        public string Help
        {
            get
            {
                return HelpBlock != null ? HelpBlock.Text : string.Empty;
            }
            set
            {
                if ( HelpBlock != null )
                {
                    HelpBlock.Text = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RockTextBox"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        [
        Bindable( true ),
        Category( "Behavior" ),
        DefaultValue( "false" ),
        Description( "Is the value required?" )
        ]
        public bool Required
        {
            get { return ViewState["Required"] as bool? ?? false; }
            set { ViewState["Required"] = value; }
        }

        /// <summary>
        /// Gets or sets the required error message.  If blank, the LabelName name will be used
        /// </summary>
        /// <value>
        /// The required error message.
        /// </value>
        public string RequiredErrorMessage
        {
            get
            {
                return RequiredFieldValidator != null ? RequiredFieldValidator.ErrorMessage : string.Empty;
            }
            set
            {
                if ( RequiredFieldValidator != null )
                {
                    RequiredFieldValidator.ErrorMessage = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsValid
        {
            get
            {
                return !Required || RequiredFieldValidator == null || RequiredFieldValidator.IsValid;
            }
        }

        /// <summary>
        /// Gets or sets the help block.
        /// </summary>
        /// <value>
        /// The help block.
        /// </value>
        public HelpBlock HelpBlock { get; set; }

        /// <summary>
        /// Gets or sets the required field validator.
        /// </summary>
        /// <value>
        /// The required field validator.
        /// </value>
        public RequiredFieldValidator RequiredFieldValidator { get; set; }

        /// <summary>
        /// Gets or sets the group of controls for which the <see cref="T:System.Web.UI.WebControls.TextBox" /> control causes validation when it posts back to the server.
        /// </summary>
        /// <returns>The group of controls for which the <see cref="T:System.Web.UI.WebControls.TextBox" /> control causes validation when it posts back to the server. The default value is an empty string ("").</returns>
        public override string ValidationGroup
        {
            get
            {
                return base.ValidationGroup;
            }
            set
            {
                base.ValidationGroup = value;
                RequiredFieldValidator.ValidationGroup = value;
            }
        }

        #endregion

        #region Controls

        /// <summary>
        /// The merge fields picker
        /// </summary>
        private MergeFieldPicker _mergeFieldPicker;

        #endregion

        #region Properties

        public string Toolbar
        {
            get
            {
                string toolbar = ViewState["Toolbar"] as string;
                if ( string.IsNullOrWhiteSpace( toolbar ) )
                {
                    toolbar = "RockCustomConfigLight";
                }

                return toolbar;
            }

            set
            {
                ViewState["Toolbar"] = value;
            }
        }

        public int? ResizeMaxWidth
        {
            get
            {
                return ViewState["ResizeMaxWidth"] as int?;
            }

            set
            {
                ViewState["ResizeMaxWidth"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the merge fields to make available.  This should include either a list of
        /// entity type names (full name), or other non-object string values
        /// </summary>
        /// <value>
        /// The merge fields.
        /// </value>
        public List<string> MergeFields
        {
            get
            {
                var mergeFields = ViewState["MergeFields"] as List<string>;
                if ( mergeFields == null )
                {
                    mergeFields = new List<string>();
                    ViewState["MergeFields"] = mergeFields;
                }
                return mergeFields;
            }

            set { ViewState["MergeFields"] = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlEditor"/> class.
        /// </summary>
        public HtmlEditor()
            : base()
        {
            RequiredFieldValidator = new RequiredFieldValidator();
            RequiredFieldValidator.ValidationGroup = this.ValidationGroup;
            HelpBlock = new HelpBlock();

            TextMode = TextBoxMode.MultiLine;
            Rows = 10;
            Columns = 80;

        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( System.EventArgs e )
        {
            base.OnInit( e );

        }
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();
            RockControlHelper.CreateChildControls( this, Controls );

            _mergeFieldPicker = new MergeFieldPicker();
            _mergeFieldPicker.ID = string.Format( "{0}_mfPicker", this.ID );
            _mergeFieldPicker.SetValue( string.Empty );
            Controls.Add( _mergeFieldPicker );
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( this.Visible )
            {
                RockControlHelper.RenderControl( this, writer );
            }
        }

        /// <summary>
        /// Renders the base control.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void RenderBaseControl( HtmlTextWriter writer )
        {
            string ckInitScriptFormat = @"
var toolbar_RockCustomConfigLight =
	[
        {{ name: 'document', items: ['Source'] }},
        {{ name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'NumberedList', 'BulletedList', 'Link', 'Image', 'PasteFromWord', '-', 'RemoveFormat'] }},
        {{ name: 'editing', items: ['Format'] }},
	];

var toolbar_RockCustomConfigFull =
	[
        {{ name: 'document', items: ['Source'] }},
        {{ name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] }},
        {{ name: 'editing', items: ['Find', 'Replace', '-', 'Scayt'] }},
        {{ name: 'links', items: ['Link', 'Unlink', 'Anchor'] }},
        {{ name: 'styles', items: ['Styles', 'Format'] }},
                '/',
        {{ name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] }},
        {{
            name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-',
              'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'Image', 'Table']
        }},
	];	

CKEDITOR.replace('{0}', {{ 
  toolbar: toolbar_{1},
  resize_maxWidth: '{2}'  
}} );
            ";



            string ckInitScript = string.Format( ckInitScriptFormat, this.ClientID, this.Toolbar, this.ResizeMaxWidth ?? 0 );

            ScriptManager.RegisterStartupScript( this, this.GetType(), "ckeditor_init_script_" + this.ClientID, ckInitScript, true );

            if ( MergeFields.Any() )
            {
                _mergeFieldPicker.MergeFields = this.MergeFields;
                _mergeFieldPicker.RenderControl( writer );

                AddMergeFieldScript();
            }

            base.RenderControl( writer );
        }

        /// <summary>
        /// Adds the merge field script.
        /// </summary>
        private void AddMergeFieldScript()
        {
            string scriptFormat = @"
        $('#btnSelectNone_{0}').click(function (e) {{
            clearSelection_{0}();
            return false;
        }});

        $('#btnSelect_{0}').click(function (e) {{
            var url = Rock.settings.get('baseUrl') + 'api/MergeFields/' +  $('#hfItemId_{0}').val();
            $.get(url, function(data) {{ 
                CKEDITOR.instances.{1}.insertHtml(data);
                clearSelection_{0}();
            }});
        }});

        $('#btnCancel_{0}').click(function (e) {{
            clearSelection_{0}();
        }});

        function clearSelection_{0}() {{
            
            var selectedValue = '0';
            var selectedText = 'Add Merge Field';

            var selectedItemLabel = $('#selectedItemLabel_{0}');
            selectedItemLabel.val(selectedValue);
            selectedItemLabel.text(selectedText);

            $('#hfItemId_{0}').val(selectedValue);
            $('#hfItemName_{0}').val(selectedText);
            
            $('#btnSelectNone_{0}').hide();
        }}
";
            string script = string.Format( scriptFormat, _mergeFieldPicker.ID, this.ClientID );
            ScriptManager.RegisterStartupScript( _mergeFieldPicker, _mergeFieldPicker.GetType(), "merge_field_extension-" + _mergeFieldPicker.ID.ToString(), script, true );


        }
    }
}