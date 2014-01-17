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
using Rock.Model;
using Rock;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class MergeFieldPicker : ItemPicker
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
 	         base.OnInit(e);
             base.DefaultText = "Add Merge Field";
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );
            ItemRestUrlExtraParams = "/" + MergeFields.AsDelimited( "," );
        }
        
        /// <summary>
        /// Gets or sets the merge fields.
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

            set 
            {
                ViewState["MergeFields"] = value;
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="nodePath">The node path.</param>
        public void SetValue( string nodePath )
        {
            if ( ! string.IsNullOrWhiteSpace(nodePath))
            {
                var nodes = nodePath.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                
                if ( nodes.Count > 0 )
                {
                    ItemId = nodePath;
                    ItemName = nodes[nodes.Count - 1];
                    
                    if ( nodes.Count > 1 )
                    {
                        InitialItemParentIds = nodes.Take( nodes.Count - 1 ).Select(a => a.Quoted()).ToList().AsDelimited( "," );
                    }
                }
            }
            else
            {
                ItemId = "0";
                ItemName = "Add Merge Field";
            }
        }

        /// <summary>
        /// Sets the values.
        /// </summary>
        /// <param name="nodePaths">The node paths.</param>
        public void SetValues( IEnumerable<string> nodePaths )
        {
            var nodePathsList = nodePaths.ToList();

            if ( nodePathsList.Any() )
            {
                var ids = new List<string>();
                var names = new List<string>();
                InitialItemParentIds = string.Empty;

                foreach ( string nodePath in nodePathsList )
                {
                    var nodes = nodePath.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ).ToList();

                    if ( nodes.Count > 0 )
                    {
                        ItemId = nodePath;
                        ItemName = nodes[nodes.Count - 1];

                        if ( InitialItemParentIds == string.Empty && nodes.Count > 1 )
                        {
                            InitialItemParentIds = nodes.Take( nodes.Count - 1 ).Select(a => a.Quoted()).ToList().AsDelimited( "," );
                        }
                    }
                }

                ItemIds = ids;
                ItemNames = names;

            }
            else
            {
                ItemId = "0";
                ItemName = "Add Merge Field";
            }
        }

        /// <summary>
        /// Sets the value on select.
        /// </summary>
        protected override void SetValueOnSelect()
        {
            this.SetValue( ItemId );
        }

        /// <summary>
        /// Sets the values on select.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void SetValuesOnSelect()
        {
            this.SetValues( ItemIds );
        }

        /// <summary>
        /// Gets the item rest URL.
        /// </summary>
        /// <value>
        /// The item rest URL.
        /// </value>
        public override string ItemRestUrl
        {
            get { return "~/api/MergeFields/GetChildren/"; }
        }

    }
}