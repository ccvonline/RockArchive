//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
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


namespace Rock.Client
{
    /// <summary>
    /// Base client model for BinaryFile that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class BinaryFileEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public int? BinaryFileTypeId { get; set; }

        /// <summary />
        public DateTime? ContentLastModified { get; set; }

        /// <summary />
        public string Description { get; set; }

        /// <summary />
        public string FileName { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public bool IsSystem { get; set; }

        /// <summary />
        public bool IsTemporary { get; set; }

        /// <summary />
        public string MimeType { get; set; }

        /// <summary />
        public string Path { get; set; }

        /// <summary />
        public string StorageEntitySettings { get; set; }

        /// <summary />
        public DateTime? CreatedDateTime { get; set; }

        /// <summary />
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary />
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary />
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source BinaryFile object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( BinaryFile source )
        {
            this.Id = source.Id;
            this.BinaryFileTypeId = source.BinaryFileTypeId;
            this.ContentLastModified = source.ContentLastModified;
            this.Description = source.Description;
            this.FileName = source.FileName;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.IsSystem = source.IsSystem;
            this.IsTemporary = source.IsTemporary;
            this.MimeType = source.MimeType;
            this.Path = source.Path;
            this.StorageEntitySettings = source.StorageEntitySettings;
            this.CreatedDateTime = source.CreatedDateTime;
            this.ModifiedDateTime = source.ModifiedDateTime;
            this.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            this.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for BinaryFile that includes all the fields that are available for GETs. Use this for GETs (use BinaryFileEntity for POST/PUTs)
    /// </summary>
    public partial class BinaryFile : BinaryFileEntity
    {
        /// <summary />
        public BinaryFileType BinaryFileType { get; set; }

        /// <summary />
        public int? StorageEntityTypeId { get; set; }

    }
}
