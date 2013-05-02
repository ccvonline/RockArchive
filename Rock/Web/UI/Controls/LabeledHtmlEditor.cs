﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using CKEditor.NET;

namespace Rock.Web.UI.Controls
{
    [ToolboxData( "<{0}:LabeledHtmlEditor runat=server></{0}:LabeledHtmlEditor>" )]
    public class LabeledHtmlEditor : CKEditorControl, ILabeledControl
    {
        /// <summary>
        /// The label
        /// </summary>
        protected Literal label;

        /// <summary>
        /// The help block
        /// </summary>
        protected HelpBlock helpBlock;

        /// <summary>
        /// The merge fields picker
        /// </summary>
        protected MergeFieldPicker mergeFieldPicker;

        /// <summary>
        /// Gets or sets the help block.
        /// </summary>
        /// <value>
        /// The help block.
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
                EnsureChildControls();
                return helpBlock.Text;
            }
            set
            {
                EnsureChildControls();
                helpBlock.Text = value;
            }
        }

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
        public string LabelText
        {
            get
            {
                EnsureChildControls();
                return label.Text;
            }
            set
            {
                EnsureChildControls();
                label.Text = value;
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

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            
            Controls.Clear();

            label = new Literal();
            label.ID = string.Format( "{0}_lbl", this.ID );
            Controls.Add( label );

            mergeFieldPicker = new MergeFieldPicker();
            mergeFieldPicker.ID = string.Format( "{0}_mfPicker", this.ID );
            mergeFieldPicker.SetValue( string.Empty );
            Controls.Add( mergeFieldPicker );

            helpBlock = new HelpBlock();
            helpBlock.ID = string.Format( "{0}_help", this.ID );
            Controls.Add( helpBlock );
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            writer.AddAttribute( "class", "control-group" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.AddAttribute( "class", "control-label" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            label.Visible = this.Visible;
            label.RenderControl( writer );
            writer.Write( " " );

            if ( MergeFields.Any() )
            {
                mergeFieldPicker.MergeFields = this.MergeFields;
                mergeFieldPicker.RenderControl( writer );
                writer.Write( " " );

                string scriptFormat = @"
        $('#btnSelectNone_{0}').click(function (e) {{

            var selectedText = 'Add Merge Field';

            var selectedItemLabel = $('#selectedItemLabel_{0}');
            var hiddenItemName = $('#hfItemName_{0}');
            
            hiddenItemName.val(selectedText);
            selectedItemLabel.text(selectedText);

            return false;

        }});

        $('#btnSelect_{0}').click(function (e) {{

            var url = rock.baseUrl + 'api/MergeFields/' +  $('#hfItemId_{0}').val();
            
            $.get(url, function(data) {{ 

                CKEDITOR.instances.{1}.insertHtml(data);

                var selectedValue = '0';
                var selectedText = 'Add Merge Field';

                var selectedItemLabel = $('#selectedItemLabel_{0}');
                var hiddenItemId = $('#hfItemId_{0}');
                var hiddenItemName = $('#hfItemName_{0}');

                hiddenItemId.val(selectedValue);
                hiddenItemName.val(selectedText);
                selectedItemLabel.val(selectedValue);
                selectedItemLabel.text(selectedText);

            }});
        }});
";
                string script = string.Format( scriptFormat, mergeFieldPicker.ID, this.ClientID );
                ScriptManager.RegisterStartupScript( mergeFieldPicker, mergeFieldPicker.GetType(), "merge_field_extension-" + mergeFieldPicker.ID.ToString(), script, true );
            }

            helpBlock.RenderControl( writer );

            writer.RenderEndTag();

            var wrapperClassName = "controls";

            writer.AddAttribute( "class", wrapperClassName );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            base.RenderControl( writer );

            writer.RenderEndTag();

            writer.RenderEndTag();
        }
    }
}