﻿// <copyright>
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
using System.Linq;
using System.Web;

namespace Rock.Attribute
{
    /// <summary>
    /// Field Attribute to select 0 or more ContentChannelTypes
    /// Stored as comma-delimited list of ContentChannelType.Guids
    /// </summary>
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = true, Inherited = true )]
    public class ContentChannelTypesFieldAttribute : FieldAttribute
    {
        public ContentChannelTypesFieldAttribute( string name, string description = "", bool required = true, string defaultGroupTypeGuids = "", string category = "", int order = 0, string key = null )
            : base( name, description, required, defaultGroupTypeGuids, category, order, key, typeof( Rock.Field.Types.ContentChannelTypesFieldType ).FullName )
        {
        }
    }
}