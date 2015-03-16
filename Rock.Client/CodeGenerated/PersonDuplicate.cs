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
    public partial class PersonDuplicateEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public int? Capacity { get; set; }

        /// <summary />
        public int DuplicatePersonAliasId { get; set; }

        /// <summary />
        public bool IgnoreUntilScoreChanges { get; set; }

        /// <summary />
        public bool IsConfirmedAsNotDuplicate { get; set; }

        /// <summary />
        public int PersonAliasId { get; set; }

        /// <summary />
        public int? Score { get; set; }

        /// <summary />
        public string ScoreDetail { get; set; }

        /// <summary />
        public int? TotalCapacity { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public string ForeignId { get; set; }

    }

    /// <summary>
    /// Simple Client Model for PersonDuplicate
    /// </summary>
    public partial class PersonDuplicate : PersonDuplicateEntity
    {
        /// <summary />
        public double? ConfidenceScore { get; set; }

        /// <summary />
        public DateTime? CreatedDateTime { get; set; }

        /// <summary />
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary />
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary />
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public Dictionary<string, Rock.Client.Attribute> Attributes { get; set; }

        /// <summary />
        public Dictionary<string, Rock.Client.AttributeValue> AttributeValues { get; set; }
    }
}
