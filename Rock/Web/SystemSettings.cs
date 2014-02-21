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
using System.Linq;
using System.Runtime.Caching;

using Rock.Model;
using Rock.Web.Cache;

namespace Rock.Web
{
    /// <summary>
    /// System Settings can be used to persist a key/value 
    /// </summary>
    [Serializable]
    public class SystemSettings
    {
        #region Constructors

        private SystemSettings() { }

        #endregion

        #region Properties

        private List<AttributeCache> Attributes { get; set; }

        #endregion

        #region Static Methods

        private static string CacheKey()
        {
            return "Rock:SystemSettings";
        }

        /// <summary>
        /// Gets the Global Attribute values for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetValue( string key )
        {
            var settings = SystemSettings.Read();
            var attributeCache = settings.Attributes.FirstOrDefault( a => a.Key.Equals( key, StringComparison.OrdinalIgnoreCase ) );
            if ( attributeCache != null )
            {
                return attributeCache.DefaultValue;
            }

            return string.Empty;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="currentPersonAlias">The current person alias.</param>
        /// <param name="saveValue">if set to <c>true</c> [save value].</param>
        public static void SetValue( string key, string value, PersonAlias currentPersonAlias )
        {
            var attributeService = new AttributeService();
            var attribute = attributeService.GetSystemSetting( key );

            if ( attribute == null )
            {
                attribute = new Rock.Model.Attribute();
                attribute.FieldTypeId = FieldTypeCache.Read( new Guid( SystemGuid.FieldType.TEXT ) ).Id;
                attribute.EntityTypeQualifierColumn = Rock.Model.Attribute.SYSTEM_SETTING_QUALIFIER;
                attribute.EntityTypeQualifierValue = string.Empty;
                attribute.Key = key;
                attribute.Name = key.SplitCase();
                attribute.DefaultValue = value;
                attributeService.Add( attribute, currentPersonAlias );
                attributeService.Save( attribute, currentPersonAlias );
            }
            else
            {
                attribute.DefaultValue = value;
                attributeService.Save( attribute, currentPersonAlias );
            }

            AttributeCache.Flush( attribute.Id );

            var settings = SystemSettings.Read();
            var attributeCache = settings.Attributes.FirstOrDefault( a => a.Key.Equals( key, StringComparison.OrdinalIgnoreCase ) );
            if ( attributeCache != null )
            {
                attributeCache.DefaultValue = value;
            }
            else
            {
                settings.Attributes.Add( AttributeCache.Read( attribute.Id ) );
            }
        }

        /// <summary>
        /// Returns Global Attributes from cache.  If they are not already in cache, they
        /// will be read and added to cache
        /// </summary>
        /// <returns></returns>
        private static SystemSettings Read()
        {
            string cacheKey = SystemSettings.CacheKey();

            ObjectCache cache = MemoryCache.Default;
            SystemSettings systemSettings = cache[cacheKey] as SystemSettings;

            if ( systemSettings != null )
            {
                return systemSettings;
            }
            else
            {
                systemSettings = new SystemSettings();
                systemSettings.Attributes = new List<AttributeCache>();

                var attributeService = new Rock.Model.AttributeService();

                foreach ( Rock.Model.Attribute attribute in attributeService.GetSystemSettings() )
                {
                    var attributeCache = AttributeCache.Read( attribute );
                    systemSettings.Attributes.Add( attributeCache );
                }

                cache.Set( cacheKey, systemSettings, new CacheItemPolicy() );

                return systemSettings;
            }
        }
       
        #endregion

    }
}