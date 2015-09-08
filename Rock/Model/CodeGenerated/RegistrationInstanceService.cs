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
    /// RegistrationInstance Service class
    /// </summary>
    public partial class RegistrationInstanceService : Service<RegistrationInstance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationInstanceService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public RegistrationInstanceService(RockContext context) : base(context)
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
        public bool CanDelete( RegistrationInstance item, out string errorMessage )
        {
            errorMessage = string.Empty;
            
            // ignoring Registration,RegistrationInstanceId 
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class RegistrationInstanceExtensionMethods
    {
        /// <summary>
        /// Clones this RegistrationInstance object to a new RegistrationInstance object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static RegistrationInstance Clone( this RegistrationInstance source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as RegistrationInstance;
            }
            else
            {
                var target = new RegistrationInstance();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another RegistrationInstance object to this RegistrationInstance object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this RegistrationInstance target, RegistrationInstance source )
        {
            target.Id = source.Id;
            target.AccountId = source.AccountId;
            target.AdditionalConfirmationDetails = source.AdditionalConfirmationDetails;
            target.AdditionalReminderDetails = source.AdditionalReminderDetails;
            target.ContactEmail = source.ContactEmail;
            target.ContactPersonAliasId = source.ContactPersonAliasId;
            target.ContactPhone = source.ContactPhone;
            target.Details = source.Details;
            target.EndDateTime = source.EndDateTime;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.IsActive = source.IsActive;
            target.MaxAttendees = source.MaxAttendees;
            target.Name = source.Name;
            target.RegistrationTemplateId = source.RegistrationTemplateId;
            target.ReminderSent = source.ReminderSent;
            target.SendReminderDateTime = source.SendReminderDateTime;
            target.StartDateTime = source.StartDateTime;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}
