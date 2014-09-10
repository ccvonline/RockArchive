//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
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


namespace Rock.Client
{
    /// <summary>
    /// Simple Client Model for Page
    /// </summary>
    public partial class Page
    {
        /// <summary />
        public string InternalName { get; set; }

        /// <summary />
        public string PageTitle { get; set; }

        /// <summary />
        public string BrowserTitle { get; set; }

        /// <summary />
        public int? ParentPageId { get; set; }

        /// <summary />
        public bool IsSystem { get; set; }

        /// <summary />
        public int LayoutId { get; set; }

        /// <summary />
        public bool RequiresEncryption { get; set; }

        /// <summary />
        public bool EnableViewState { get; set; }

        /// <summary />
        public bool PageDisplayTitle { get; set; }

        /// <summary />
        public bool PageDisplayBreadCrumb { get; set; }

        /// <summary />
        public bool PageDisplayIcon { get; set; }

        /// <summary />
        public bool PageDisplayDescription { get; set; }

        /// <summary />
        public int /* DisplayInNavWhen*/ DisplayInNavWhen { get; set; }

        /// <summary />
        public bool MenuDisplayDescription { get; set; }

        /// <summary />
        public bool MenuDisplayIcon { get; set; }

        /// <summary />
        public bool MenuDisplayChildPages { get; set; }

        /// <summary />
        public bool BreadCrumbDisplayName { get; set; }

        /// <summary />
        public bool BreadCrumbDisplayIcon { get; set; }

        /// <summary />
        public int Order { get; set; }

        /// <summary />
        public int OutputCacheDuration { get; set; }

        /// <summary />
        public string Description { get; set; }

        /// <summary />
        public string KeyWords { get; set; }

        /// <summary />
        public string HeaderContent { get; set; }

        /// <summary />
        public string IconCssClass { get; set; }

        /// <summary />
        public bool IncludeAdminFooter { get; set; }

        /// <summary />
        public DateTime? CreatedDateTime { get; set; }

        /// <summary />
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary />
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary />
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public string ForeignId { get; set; }

    }
}
