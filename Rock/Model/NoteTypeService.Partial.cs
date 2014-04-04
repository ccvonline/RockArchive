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
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Data access/Service class for entities of the <see cref="Rock.Model.NoteType"/>
    /// </summary>
    public partial class NoteTypeService : Service<NoteType>
    {
        /// <summary>
        /// Gets the first <see cref="Rock.Model.NoteType"/> by Name and EntityType
        /// </summary>
        /// <param name="entityTypeId">A <see cref="System.Int32"/> representing the Id of the <see cref="Rock.Model.EntityType"/> to search for.</param>
        /// <param name="name">A <see cref="System.String"/> representing the Name of the </param>
        /// <returns>The first <see cref="Rock.Model.NoteType"/> matching the provided values. If a match is not found, this value will be null.</returns>
        public NoteType Get( int entityTypeId, string name )
        {
            return Queryable().FirstOrDefault( n => n.EntityTypeId == entityTypeId && n.Name == name );
        }

    }
}
