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
    /// Base client model for GroupType that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class GroupTypeEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public Rock.Client.Enums.ScheduleType AllowedScheduleTypes { get; set; }

        /// <summary />
        public bool AllowMultipleLocations { get; set; }

        /// <summary />
        public Rock.Client.Enums.PrintTo AttendancePrintTo { get; set; }

        /// <summary />
        public Rock.Client.Enums.AttendanceRule AttendanceRule { get; set; }

        /// <summary />
        public int? DefaultGroupRoleId { get; set; }

        /// <summary />
        public string Description { get; set; }

        /// <summary />
        public bool? EnableLocationSchedules { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public string GroupMemberTerm { get; set; }

        /// <summary />
        public string GroupTerm { get; set; }

        /// <summary />
        public int? GroupTypePurposeValueId { get; set; }

        /// <summary />
        public string IconCssClass { get; set; }

        /// <summary />
        public bool IgnorePersonInactivated { get; set; }

        /// <summary />
        public int? InheritedGroupTypeId { get; set; }

        /// <summary />
        public bool IsSystem { get; set; }

        /// <summary />
        public Rock.Client.Enums.GroupLocationPickerMode LocationSelectionMode { get; set; }

        /// <summary>
        /// If the ModifiedByPersonAliasId and ModifiedDateTime properties are being set manually and should not be overwritten with current time/user when saved, set this value to true
        /// </summary>
        public bool ModifiedAuditValuesAlreadyUpdated { get; set; }

        /// <summary />
        public string Name { get; set; }

        /// <summary />
        public int Order { get; set; }

        /// <summary />
        public bool SendAttendanceReminder { get; set; }

        /// <summary />
        public bool ShowInGroupList { get; set; }

        /// <summary />
        public bool ShowInNavigation { get; set; }

        /// <summary />
        public bool TakesAttendance { get; set; }

        /// <summary>
        /// Leave this as NULL to let Rock set this
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// If you need to set this manually, set ModifiedAuditValuesAlreadyUpdated=True to prevent Rock from setting it
        /// </summary>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Leave this as NULL to let Rock set this
        /// </summary>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// If you need to set this manually, set ModifiedAuditValuesAlreadyUpdated=True to prevent Rock from setting it
        /// </summary>
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source GroupType object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( GroupType source )
        {
            this.Id = source.Id;
            this.AllowedScheduleTypes = source.AllowedScheduleTypes;
            this.AllowMultipleLocations = source.AllowMultipleLocations;
            this.AttendancePrintTo = source.AttendancePrintTo;
            this.AttendanceRule = source.AttendanceRule;
            this.DefaultGroupRoleId = source.DefaultGroupRoleId;
            this.Description = source.Description;
            this.EnableLocationSchedules = source.EnableLocationSchedules;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.GroupMemberTerm = source.GroupMemberTerm;
            this.GroupTerm = source.GroupTerm;
            this.GroupTypePurposeValueId = source.GroupTypePurposeValueId;
            this.IconCssClass = source.IconCssClass;
            this.IgnorePersonInactivated = source.IgnorePersonInactivated;
            this.InheritedGroupTypeId = source.InheritedGroupTypeId;
            this.IsSystem = source.IsSystem;
            this.LocationSelectionMode = source.LocationSelectionMode;
            this.ModifiedAuditValuesAlreadyUpdated = source.ModifiedAuditValuesAlreadyUpdated;
            this.Name = source.Name;
            this.Order = source.Order;
            this.SendAttendanceReminder = source.SendAttendanceReminder;
            this.ShowInGroupList = source.ShowInGroupList;
            this.ShowInNavigation = source.ShowInNavigation;
            this.TakesAttendance = source.TakesAttendance;
            this.CreatedDateTime = source.CreatedDateTime;
            this.ModifiedDateTime = source.ModifiedDateTime;
            this.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            this.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for GroupType that includes all the fields that are available for GETs. Use this for GETs (use GroupTypeEntity for POST/PUTs)
    /// </summary>
    public partial class GroupType : GroupTypeEntity
    {
        /// <summary />
        public ICollection<GroupType> ChildGroupTypes { get; set; }

        /// <summary />
        public GroupTypeRole DefaultGroupRole { get; set; }

        /// <summary />
        public DefinedValue GroupTypePurposeValue { get; set; }

        /// <summary />
        public ICollection<GroupTypeLocationType> LocationTypes { get; set; }

        /// <summary />
        public ICollection<GroupTypeRole> Roles { get; set; }

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
