﻿// <copyright>
// Copyright by the Spark Development Network
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock.Lava
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class LegacyLavaSyntaxDetectedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyLavaSyntaxDetectedException"/> class.
        /// </summary>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <param name="fieldName">Name of the field.</param>
        public LegacyLavaSyntaxDetectedException( string entityTypeName, string fieldName )
            : base( string.Format( "Warning: Legacy Lava Syntax Detected: {0}.{1}", entityTypeName, fieldName ) )
        {
            System.Diagnostics.Debug.WriteLine( this.Message );
        }
    }
}
