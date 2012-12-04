//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Email Template POCO Entity.
    /// </summary>
    [Table( "EmailTemplate" )]
    public partial class EmailTemplate : Model<EmailTemplate>
    {
        /// <summary>
        /// Gets or sets the System.
        /// </summary>
        /// <value>
        /// System.
        /// </value>
        [Required]
        public bool IsSystem { get; set; }
        
        /// <summary>
        /// Gets or sets the Person Id.
        /// </summary>
        /// <value>
        /// Person Id.
        /// </value>
        public int? PersonId { get; set; }
        
        /// <summary>
        /// Gets or sets the Category.
        /// </summary>
        /// <value>
        /// Category.
        /// </value>
        [MaxLength( 100 )]
        public string Category { get; set; }
        
        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        /// <value>
        /// Title.
        /// </value>
        [Required]
        [MaxLength( 100 )]
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the From.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        [MaxLength( 200 )]
        public string From { get; set; }
        
        /// <summary>
        /// Gets or sets the To.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public string To { get; set; }
        
        /// <summary>
        /// Gets or sets the Cc.
        /// </summary>
        /// <value>
        /// Cc.
        /// </value>
        public string Cc { get; set; }
        
        /// <summary>
        /// Gets or sets the Bcc.
        /// </summary>
        /// <value>
        /// Bcc.
        /// </value>
        public string Bcc { get; set; }
        
        /// <summary>
        /// Gets or sets the Subject.
        /// </summary>
        /// <value>
        /// Subject.
        /// </value>
        [Required]
        [MaxLength( 200 )]
        public string Subject { get; set; }
        
        /// <summary>
        /// Gets or sets the Body.
        /// </summary>
        /// <value>
        /// Body.
        /// </value>
        [Required]
        public string Body { get; set; }

        /// <summary>
        /// Gets the dto.
        /// </summary>
        /// <returns></returns>
        public override IDto Dto
        {
            get { return this.ToDto(); }
        }

        /// <summary>
        /// Static Method to return an object based on the id
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static EmailTemplate Read( int id )
        {
            return Read<EmailTemplate>( id );
        }

        /// <summary>
        /// Gets or sets the Person.
        /// </summary>
        /// <value>
        /// A <see cref="Person"/> object.
        /// </value>
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
    public partial class EmailTemplateConfiguration : EntityTypeConfiguration<EmailTemplate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplateConfiguration"/> class.
        /// </summary>
        public EmailTemplateConfiguration()
        {
            this.HasOptional( p => p.Person ).WithMany( p => p.EmailTemplates ).HasForeignKey( p => p.PersonId ).WillCascadeOnDelete(true);
        }
    }
}
