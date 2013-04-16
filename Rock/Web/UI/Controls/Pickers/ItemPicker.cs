//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ItemPicker : CompositeControl, ILabeledControl
    {
        private Label _label;
        private Literal _literal;
        private HiddenField _hfItemId;
        private HiddenField _hfInitialItemParentIds;
        private HiddenField _hfItemName;
        private HiddenField _hfItemRestUrlExtraParams;
        private LinkButton _btnSelect;
        private LinkButton _btnSelectNone;

        /// <summary>
        /// The required validator
        /// </summary>
        protected HiddenFieldValidator RequiredValidator;

        public abstract string ItemRestUrl { get; }

        /// <summary>
        /// Gets or sets the item rest URL extra params.
        /// </summary>
        /// <value>
        /// The item rest URL extra params.
        /// </value>
        public string ItemRestUrlExtraParams
        {
            get
            {
                EnsureChildControls();
                return _hfItemRestUrlExtraParams.Value;
            }

            set
            {
                EnsureChildControls();
                _hfItemRestUrlExtraParams.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the _label text.
        /// </summary>
        /// <value>
        /// The _label text.
        /// </value>
        public string LabelText
        {
            get
            {
                EnsureChildControls();
                return _label.Text;
            }

            set
            {
                EnsureChildControls();
                _label.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the item id.
        /// </summary>
        /// <value>
        /// The item id.
        /// </value>
        public string ItemId
        {
            get
            {
                EnsureChildControls();
                if ( string.IsNullOrWhiteSpace( _hfItemId.Value ) )
                {
                    _hfItemId.Value = Constants.None.IdValue;
                }

                return _hfItemId.Value;
            }

            set
            {
                EnsureChildControls();
                _hfItemId.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial item parent ids.
        /// </summary>
        /// <value>
        /// The initial item parent ids.
        /// </value>
        public string InitialItemParentIds
        {
            get
            {
                EnsureChildControls();
                if ( string.IsNullOrWhiteSpace( _hfInitialItemParentIds.Value ) )
                {
                    _hfInitialItemParentIds.Value = Constants.None.IdValue;
                }

                return _hfInitialItemParentIds.Value;
            }

            set
            {
                EnsureChildControls();
                _hfInitialItemParentIds.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected value.
        /// </summary>
        /// <value>
        /// The selected value.  NOTE: If nothing was previously set, it will return <see cref="Rock.Constants.None.IdValue"/>.
        /// </value>
        public string SelectedValue
        {
            get
            {
                return ItemId;
            }

            private set
            {
                ItemId = value;
            }
        }

        /// <summary>
        /// Gets the selected value as int.
        /// </summary>
        /// <param name="noneAsNull">if set to <c>true</c> [none as null].</param>
        /// <returns></returns>
        /// <value>
        /// The selected value as int.
        ///   </value>
        public int? SelectedValueAsInt( bool noneAsNull = true )
        {
            if ( string.IsNullOrWhiteSpace( ItemId ) )
            {
                return null;
            }
            
            int result = int.Parse( ItemId );
            if ( noneAsNull )
            {
                if ( result == Constants.None.Id )
                {
                    return null;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>
        /// The name of the item.
        /// </value>
        public string ItemName
        {
            get
            {
                EnsureChildControls();
                if ( string.IsNullOrWhiteSpace( _hfItemName.Value ) )
                {
                    _hfItemName.Value = Constants.None.Text;
                }

                return _hfItemName.Value;
            }

            set
            {
                EnsureChildControls();
                _hfItemName.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ItemPicker"/> is required.
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
            get
            {
                if ( ViewState["Required"] != null )
                    return (bool)ViewState["Required"];
                
                return false;
            }
            set
            {
                ViewState["Required"] = value;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            const string treeViewScriptFormat = "Rock.controls.itemPicker.initialize({{ controlId: '{0}', restUrl: '{1}' }});";
            string treeViewScript = string.Format( treeViewScriptFormat, this.ID, this.ResolveUrl( ItemRestUrl ) );
            ScriptManager.RegisterStartupScript( this, this.GetType(), "item_picker-treeviewscript_" + this.ID, treeViewScript, true );
            var sm = ScriptManager.GetCurrent( this.Page );
            EnsureChildControls();

            if ( sm != null )
            {
                sm.RegisterAsyncPostBackControl( _btnSelect );
                sm.RegisterAsyncPostBackControl( _btnSelectNone );
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
                return !Required || RequiredValidator.IsValid;
            }
        }

        /// <summary>
        /// Handles the Click event of the _btnSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSelect_Click( object sender, EventArgs e )
        {
            SetValueOnSelect();
            if ( SelectItem != null )
            {
                SelectItem( sender, e );
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="id">The id.</param>
        public void SetValue( int? id )
        {
            ItemId = id.HasValue ? id.Value.ToString() : Constants.None.IdValue;
            SetValueOnSelect();
        }

        /// <summary>
        /// Sets the value on select.
        /// </summary>
        protected abstract void SetValueOnSelect();

        /// <summary>
        /// Occurs when [select item].
        /// </summary>
        public event EventHandler SelectItem;

        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public void ShowErrorMessage( string errorMessage )
        {
            RequiredValidator.ErrorMessage = errorMessage;
            RequiredValidator.IsValid = false;
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Controls.Clear();

            _label = new Label();
            _literal = new Literal();
            _hfItemId = new HiddenField();
            _hfItemId.ClientIDMode = ClientIDMode.Static;
            _hfItemId.ID = string.Format( "hfItemId_{0}", this.ID );
            _hfInitialItemParentIds = new HiddenField();
            _hfInitialItemParentIds.ClientIDMode = ClientIDMode.Static;
            _hfInitialItemParentIds.ID = string.Format( "hfInitialItemParentIds_{0}", this.ID );
            _hfItemName = new HiddenField();
            _hfItemName.ClientIDMode = ClientIDMode.Static;
            _hfItemName.ID = string.Format( "hfItemName_{0}", this.ID );
            _hfItemRestUrlExtraParams = new HiddenField();
            _hfItemRestUrlExtraParams.ClientIDMode = ClientIDMode.Static;
            _hfItemRestUrlExtraParams.ID = string.Format( "hfItemRestUrlExtraParams_{0}", this.ID );

            _btnSelect = new LinkButton();
            _btnSelect.ClientIDMode = ClientIDMode.Static;
            _btnSelect.CssClass = "btn btn-mini btn-primary";
            _btnSelect.ID = string.Format( "btnSelect_{0}", this.ID );
            _btnSelect.Text = "Select Item";
            _btnSelect.CausesValidation = false;
            _btnSelect.Click += btnSelect_Click;

            _btnSelectNone = new LinkButton();
            _btnSelectNone.ClientIDMode = ClientIDMode.Static;
            _btnSelectNone.CssClass = "rock-picker-select-none";
            _btnSelectNone.ID = string.Format( "btnSelectNone_{0}", this.ID );
            _btnSelectNone.Text = "<i class='icon-remove'></i>";
            _btnSelectNone.CausesValidation = false;
            _btnSelectNone.Visible = false;
            _btnSelectNone.Click += btnSelect_Click;


            Controls.Add( _label );
            Controls.Add( _literal );
            Controls.Add( _hfItemId );
            Controls.Add( _hfInitialItemParentIds );
            Controls.Add( _hfItemName );
            Controls.Add( _hfItemRestUrlExtraParams );
            Controls.Add( _btnSelect );
            Controls.Add( _btnSelectNone );

            RequiredValidator = new HiddenFieldValidator();
            RequiredValidator.ID = this.ID + "_rfv";
            RequiredValidator.InitialValue = "0";
            RequiredValidator.ControlToValidate = _hfItemId.ID;
            RequiredValidator.Display = ValidatorDisplay.Dynamic;
            RequiredValidator.CssClass = "validation-error help-inline";
            RequiredValidator.Enabled = false;

            Controls.Add( RequiredValidator );
        }

        /// <summary>
        /// Renders the <see cref="T:System.Web.UI.WebControls.TextBox" /> control to the specified <see cref="T:System.Web.UI.HtmlTextWriter" /> object.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> that receives the rendered output.</param>
        protected override void Render( HtmlTextWriter writer )
        {
            writer.AddAttribute( "class", "control-group" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            _label.AddCssClass( "control-_label" );

            _label.RenderControl( writer );

            writer.AddAttribute( "class", "controls" );

            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            if ( Required )
            {
                RequiredValidator.Enabled = true;
                RequiredValidator.ErrorMessage = LabelText + " is Required.";
                RequiredValidator.RenderControl( writer );
            }

            _hfItemId.RenderControl( writer );
            _hfInitialItemParentIds.RenderControl( writer );
            _hfItemName.RenderControl( writer );
            _hfItemRestUrlExtraParams.RenderControl( writer );

            if ( this.Enabled )
            {
                string controlHtmlFormatStart = @"
<div class='control-group' id='{0}'>
    <div class='controls'>
        <div class='rock-picker rock-picker-select'> 
            <a class='rock-picker' href='#'>
                <i class='icon-folder-open'></i>
                <span id='selectedItemLabel_{0}'>{1}</span>
                <b class='caret'></b>
            </a>
";
                writer.Write( controlHtmlFormatStart, this.ID, this.ItemName );

                // if there is a PostBack registered, create a real LinkButton, otherwise just spit out HTML (to prevent the autopostback)
                if ( SelectItem != null )
                {
                    _btnSelectNone.RenderControl( writer );
                }
                else
                {
                    writer.Write( "<a class='rock-picker-select-none' id='btnSelectNone_{0}' href='#' style='display:none'><i class='icon-remove'></i></a>", this.ID );
                }

                string controlHtmlFormatMiddle = @"
        </div>
        <div class='dropdown-menu rock-picker rock-picker-item'>

            <div id='treeview-scroll-container_{0}' class='scroll-container scroll-container-picker'>
                <div class='scrollbar'>
                    <div class='track'>
                        <div class='thumb'>
                            <div class='end'></div>
                        </div>
                    </div>
                </div>
                <div class='viewport'>
                    <div class='overview'>
                        <div id='treeviewItems_{0}' class='tree-view tree-view-items'></div>        
                    </div>
                </div>
            </div>

            <hr />
";
                writer.Write( controlHtmlFormatMiddle, this.ID, this.ItemName );

                // if there is a PostBack registered, create a real LinkButton, otherwise just spit out HTML (to prevent the autopostback)
                if ( SelectItem != null )
                {
                    _btnSelect.RenderControl( writer );
                }
                else
                {
                    writer.Write( "<a class='btn btn-mini btn-primary' id='btnSelect_{0}'>Select</a>", this.ID );
                }

                string controlHtmlFormatEnd = @"
            <a class='btn btn-mini' id='btnCancel_{0}'>Cancel</a>
            
        </div>
    </div>
</div>
";
                writer.Write( controlHtmlFormatEnd, this.ID, this.ItemName );
            }
            else
            {
                string controlHtmlFormatDisabled = @"
<div class='control-group' id='{0}'>
    <div class='controls'>

        <i class='icon-file-alt'></i>
        <span id='selectedItemLabel_{0}'>{1}</span>

    </div>
</div>
"; 
                writer.Write( controlHtmlFormatDisabled, this.ID, this.ItemName );
            }

            writer.RenderEndTag();

            writer.RenderEndTag();
        }
    }
}