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
    /// Base client model for RegistrationTemplate that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class RegistrationTemplateEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public bool AllowMultipleRegistrants { get; set; }

        /// <summary />
        public int? CategoryId { get; set; }

        /// <summary />
        public string ConfirmationEmailTemplate { get; set; }

        /// <summary />
        public decimal Cost { get; set; }

        /// <summary />
        public string DiscountCodeTerm { get; set; }

        /// <summary />
        public string FeeTerm { get; set; }

        /// <summary />
        public int? FinancialGatewayId { get; set; }

        /// <summary />
        public int? GroupMemberRoleId { get; set; }

        /// <summary />
        public int /* GroupMemberStatus*/ GroupMemberStatus { get; set; }

        /// <summary />
        public int? GroupTypeId { get; set; }

        /// <summary />
        public bool IsActive { get; set; }

        /// <summary />
        public bool LoginRequired { get; set; }

        /// <summary />
        public int MaxRegistrants { get; set; }

        /// <summary />
        public decimal MinimumInitialPayment { get; set; }

        /// <summary />
        public string Name { get; set; }

        /// <summary />
        public bool NotifyGroupLeaders { get; set; }

        /// <summary />
        public int /* RegistrantsSameFamily*/ RegistrantsSameFamily { get; set; }

        /// <summary />
        public string RegistrantTerm { get; set; }

        /// <summary />
        public string RegistrationTerm { get; set; }

        /// <summary />
        public string ReminderEmailTemplate { get; set; }

        /// <summary />
        public string RequestEntryName { get; set; }

        /// <summary />
        public string SuccessText { get; set; }

        /// <summary />
        public string SuccessTitle { get; set; }

        /// <summary />
        public bool UseDefaultConfirmationEmail { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public string ForeignId { get; set; }

    }

    /// <summary>
    /// Client model for RegistrationTemplate that includes all the fields that are available for GETs. Use this for GETs (use RegistrationTemplateEntity for POST/PUTs)
    /// </summary>
    public partial class RegistrationTemplate : RegistrationTemplateEntity
    {
        /// <summary />
        public ICollection<RegistrationTemplateDiscount> Discounts { get; set; }

        /// <summary />
        public ICollection<RegistrationTemplateFee> Fees { get; set; }

        /// <summary />
        public ICollection<RegistrationTemplateForm> Forms { get; set; }

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
