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
using System.IO;
using System.Linq;

using Rock;
using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// EntitySetItem POCO Service class
    /// </summary>
    public partial class EntitySetItemService
    {
        /// <summary>
        /// Gets the by entity set identifier.
        /// </summary>
        /// <param name="entitySetId">The entity set identifier.</param>
        /// <returns></returns>
        public IQueryable<EntitySetItem> GetByEntitySetId( int entitySetId, bool overrideExpiration = false )
        {
            var maxExpiration = overrideExpiration ? DateTime.MaxValue : RockDateTime.Now;

            return Queryable()
                .Where( s =>
                    s.EntitySetId == entitySetId && ( 
                        !s.EntitySet.ExpireDateTime.HasValue || 
                        s.EntitySet.ExpireDateTime.Value > maxExpiration 
                    ) 
                )
                .OrderBy( s => s.Order );
        }
    }
}
