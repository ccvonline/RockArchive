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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Represents a Rock email template.
    /// </summary>
    [Table( "SystemEmail" )]
    [DataContract]
    public partial class SystemEmail : Model<SystemEmail>
    {
        /// <summary>
        /// Gets or sets a flag indicating if the email template is part of the Rock core system/framework.
        /// </summary>
        /// <value>
        /// A <see cref="System.Boolean"/> value that is <c>true</c> if the EmailTemplate is part of the Rock core system/framework otherwise <c>false</c>.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public bool IsSystem { get; set; }
        
        /// <summary>
        /// Gets or sets the PersonId of the <see cref="Rock.Model.Person"/> who is the owner of this email template. This property is only populated for private EmailTemplates
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the PersonId of the <see cref="Rock.Model.Person"/> who is the owner of the email template. If the email template is a public template 
        /// this value will be null.
        /// </value>
        [DataMember]
        public int? PersonId { get; set; }
        
        /// <summary>
        /// Gets or sets a string to identify the category that the EmailTemplate belongs to.
        /// </summary>
        /// <value>
        /// A <see cref="System.String" /> representing the category that this EmailTemplate belongs to.
        /// </value>
        /// <remarks>
        /// There are plans to update this to implement ICategorized and <see cref="Rock.Model.Category"/>. See https://github.com/SparkDevNetwork/Rock/issues/142
        /// </remarks>
        [MaxLength( 100 )]
        [DataMember]
        public string Category { get; set; }
        
        /// <summary>
        /// Gets or sets the Title of the EmailTemplate 
        /// </summary>
        /// <value>
        /// A <see cref="System.String" /> representing the Title of the EmailTemplate.
        /// </value>
        [Required]
        [MaxLength( 100 )]
        [DataMember( IsRequired = true )]
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the From email address.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the from email address.
        /// </value>
        [MaxLength( 200 )]
        [DataMember]
        public string From { get; set; }
        
        /// <summary>
        /// Gets or sets the To email addresses that emails using this template should be delivered to.  If there is not a predetermined distribution list, this property can 
        /// remain empty.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing a list of email addresses that the message should be delivered to. If there is not a predetermined email list, this property will 
        /// be null.
        /// </value>
        [DataMember]
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the email addresses that should be sent a CC or carbon copy of an email using this template. If there is not a predetermined distribution list, this property
        /// can remain empty.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing a list of email addresses that should be sent a CC or carbon copy of an email that uses this template. If there is not a predetermined
        /// distribution list, this property will be null.
        /// </value>
        [DataMember]
        public string Cc { get; set; }
        
        /// <summary>
        /// Gets or sets the email addresses that should be sent a BCC or blind carbon copy of an email using this template. If there is not a predetermined distribution list; this property 
        /// can remain empty.
        /// </summary>
        /// <value>
        /// A <see cref="System.String" /> representing a list of email addresses that should be sent a BCC or blind carbon copy of an email that uses this template. If there is not a predetermined
        /// distribution list this property will remain null.
        /// </value>
        [DataMember]
        public string Bcc { get; set; }
        
        /// <summary>
        /// Gets or sets the subject of an email that uses this template.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the subject of an email that uses this template.
        /// </value>
        [Required]
        [MaxLength( 200 )]
        [DataMember( IsRequired = true )]
        public string Subject { get; set; }
        
        /// <summary>
        /// Gets or sets the Body template that is used for emails that use this template.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the body template for emails that use this template.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.Person"/> who is the owner of the email template. This property is only populated for private email templates.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.Person"/> who is the owner of the email template. If the template is a public template, this property will be null.
        /// </value>
        [DataMember]
        public virtual Person Person { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Title;
        }
    }
    
    /// <summary>
    /// Email Template Configuration class.
    /// </summary>
    public partial class SystemEmailConfiguration : EntityTypeConfiguration<SystemEmail>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemEmailConfiguration"/> class.
        /// </summary>
        public SystemEmailConfiguration()
        {
            this.HasOptional( p => p.Person ).WithMany( p => p.EmailTemplates ).HasForeignKey( p => p.PersonId ).WillCascadeOnDelete(true);
        }
    }
}
