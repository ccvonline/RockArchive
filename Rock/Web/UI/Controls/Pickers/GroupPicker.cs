//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock.Model;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupPicker : ItemPicker, ILabeledControl
    {
        private Label label;

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        /// <value>
        /// The label text.
        /// </value>
        public string LabelText
        {
            get { return label.Text; }
            set 
            { 
                label.Text = value;
                base.RequiredErrorMessage = string.IsNullOrWhiteSpace( value ) ? "Group value is required" : value + " is required";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountPicker" /> class.
        /// </summary>
        public GroupPicker()
            : base()
        {
            label = new Label();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            ItemRestUrlExtraParams = "/0/false/0";
        }
        
        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="group">The group.</param>
        public void SetValue( Group group )
        {
            if ( group != null )
            {
                ItemId = group.Id.ToString();
                
                string parentGroupIds = string.Empty;
                var parentGroup = group.ParentGroup;
                while ( parentGroup != null )
                {
                    parentGroupIds = parentGroup.Id + "," + parentGroupIds;
                    parentGroup = parentGroup.ParentGroup;
                }

                InitialItemParentIds = parentGroupIds.TrimEnd( new[] { ',' } );
                ItemName = group.Name;
            }
            else
            {
                ItemId = Constants.None.IdValue;
                ItemName = Constants.None.TextHtml;
            }
        }

        /// <summary>
        /// Sets the values.
        /// </summary>
        /// <param name="groups">The groups.</param>
        public void SetValues( IEnumerable<Group> groups )
        {
            var theGroups = groups.ToList();

            if ( theGroups.Any() )
            {
                var ids = new List<string>();
                var names = new List<string>();
                var parentGroupIds = string.Empty;

                foreach ( var group in theGroups )
                {
                    if ( group != null )
                    {
                        ids.Add( group.Id.ToString() );
                        names.Add( group.Name );
                        var parentGroup = group.ParentGroup;

                        while ( parentGroup != null )
                        {
                            parentGroupIds += parentGroup.Id.ToString() + ",";
                            parentGroup = parentGroup.ParentGroup;
                        }
                    }
                }

                InitialItemParentIds = parentGroupIds.TrimEnd( new[] { ',' } );
                ItemIds = ids;
                ItemNames = names;
            }
            else
            {
                ItemId = Constants.None.IdValue;
                ItemName = Constants.None.TextHtml;
            }
        }

        /// <summary>
        /// Sets the value on select.
        /// </summary>
        protected override void SetValueOnSelect()
        {
            var group = new GroupService().Get( int.Parse( ItemId ) );
            SetValue( group );
        }

        /// <summary>
        /// Sets the values on select.
        /// </summary>
        protected override void SetValuesOnSelect()
        {
            var groups = new GroupService().Queryable().Where( g => ItemIds.Contains( g.Id.ToString() ) );
            this.SetValues( groups );
        }

        /// <summary>
        /// Gets the item rest URL.
        /// </summary>
        /// <value>
        /// The item rest URL.
        /// </value>
        public override string ItemRestUrl
        {
            get { return "~/api/groups/getchildren/"; }
        }
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Add( label );
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( string.IsNullOrEmpty( LabelText ) )
            {
                base.RenderControl( writer );
            }
            else
            {
                writer.AddAttribute( "class", "form-group" );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );

                label.AddCssClass( "control-label" );

                label.RenderControl( writer );

                writer.AddAttribute( "class", "controls" );

                writer.RenderBeginTag( HtmlTextWriterTag.Div );

                base.Render( writer );

                writer.RenderEndTag();

                writer.RenderEndTag();
            }
        }
    }
}