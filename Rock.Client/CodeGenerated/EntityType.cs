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
using System.Collections.Generic;


namespace Rock.Client
{
    /// <summary>
    /// Base client model for EntityType that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class EntityTypeEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public string AssemblyName { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public string FriendlyName { get; set; }

        /// <summary />
        public bool IsCommon { get; set; }

        /// <summary />
        public bool IsEntity { get; set; }

        /// <summary />
        public bool IsSecured { get; set; }

        /// <summary />
        public int? MultiValueFieldTypeId { get; set; }

        /// <summary />
        public string Name { get; set; }

        /// <summary />
        public int? SingleValueFieldTypeId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source EntityType object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( EntityType source )
        {
            this.Id = source.Id;
            this.AssemblyName = source.AssemblyName;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.FriendlyName = source.FriendlyName;
            this.IsCommon = source.IsCommon;
            this.IsEntity = source.IsEntity;
            this.IsSecured = source.IsSecured;
            this.MultiValueFieldTypeId = source.MultiValueFieldTypeId;
            this.Name = source.Name;
            this.SingleValueFieldTypeId = source.SingleValueFieldTypeId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for EntityType that includes all the fields that are available for GETs. Use this for GETs (use EntityTypeEntity for POST/PUTs)
    /// </summary>
    public partial class EntityType : EntityTypeEntity
    {
    }
}
