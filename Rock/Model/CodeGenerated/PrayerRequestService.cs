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
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// PrayerRequest Service class
    /// </summary>
    public partial class PrayerRequestService : Service<PrayerRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrayerRequestService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public PrayerRequestService(RockContext context) : base(context)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( PrayerRequest item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class PrayerRequestExtensionMethods
    {
        /// <summary>
        /// Clones this PrayerRequest object to a new PrayerRequest object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static PrayerRequest Clone( this PrayerRequest source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as PrayerRequest;
            }
            else
            {
                var target = new PrayerRequest();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another PrayerRequest object to this PrayerRequest object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this PrayerRequest target, PrayerRequest source )
        {
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.Email = source.Email;
            target.RequestedByPersonAliasId = source.RequestedByPersonAliasId;
            target.CategoryId = source.CategoryId;
            target.Text = source.Text;
            target.Answer = source.Answer;
            target.EnteredDateTime = source.EnteredDateTime;
            target.ExpirationDate = source.ExpirationDate;
            target.GroupId = source.GroupId;
            target.AllowComments = source.AllowComments;
            target.IsUrgent = source.IsUrgent;
            target.IsPublic = source.IsPublic;
            target.IsActive = source.IsActive;
            target.IsApproved = source.IsApproved;
            target.FlagCount = source.FlagCount;
            target.PrayerCount = source.PrayerCount;
            target.ApprovedByPersonAliasId = source.ApprovedByPersonAliasId;
            target.ApprovedOnDateTime = source.ApprovedOnDateTime;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Id = source.Id;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}
