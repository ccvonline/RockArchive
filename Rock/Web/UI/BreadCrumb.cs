﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System.Web.UI;

namespace Rock.Web.UI
{
    /// <summary>
    /// Helper class to work with page navigation
    /// </summary>
    public class BreadCrumb
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BreadCrumb" /> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreadCrumb" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public BreadCrumb( string name )
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreadCrumb" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="url">The URL.</param>
        public BreadCrumb( string name, string url )
            : this( name )
        {
            Url = url;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreadCrumb" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="pageReference">The page reference.</param>
        public BreadCrumb( string name, PageReference pageReference )
            : this( name )
        {
            Url = pageReference.BuildUrl();
        }

    }
}