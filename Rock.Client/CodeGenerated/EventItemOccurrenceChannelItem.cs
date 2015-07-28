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
    /// Base client model for EventItemOccurrenceChannelItem that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class EventItemOccurrenceChannelItemEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public int ContentChannelItemId { get; set; }

        /// <summary />
        public int EventItemOccurrenceId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public string ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source EventItemOccurrenceChannelItem object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( EventItemOccurrenceChannelItem source )
        {
            this.Id = source.Id;
            this.ContentChannelItemId = source.ContentChannelItemId;
            this.EventItemOccurrenceId = source.EventItemOccurrenceId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for EventItemOccurrenceChannelItem that includes all the fields that are available for GETs. Use this for GETs (use EventItemOccurrenceChannelItemEntity for POST/PUTs)
    /// </summary>
    public partial class EventItemOccurrenceChannelItem : EventItemOccurrenceChannelItemEntity
    {
        /// <summary />
        public ContentChannelItem ContentChannelItem { get; set; }

        /// <summary />
        public DateTime? CreatedDateTime { get; set; }

        /// <summary />
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary />
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary />
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary>
        /// NOTE: Attributes are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.Attribute> Attributes { get; set; }

        /// <summary>
        /// NOTE: AttributeValues are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.AttributeValue> AttributeValues { get; set; }
    }
}
