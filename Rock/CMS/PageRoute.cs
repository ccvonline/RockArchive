//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Rock.Data;

namespace Rock.Cms
{
    /// <summary>
    /// Page Route POCO Entity.
    /// </summary>
    [Table( "PageRoute" )]
    public partial class PageRoute : Model<PageRoute>, IExportable
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
        /// Gets or sets the Page Id.
        /// </summary>
        /// <value>
        /// Page Id.
        /// </value>
        [Required]
        public int PageId { get; set; }
        
        /// <summary>
        /// Gets or sets the Route.
        /// </summary>
        /// <value>
        /// Route.
        /// </value>
        [Route]
        [Required]
        [MaxLength( 200 )]
        public string Route { get; set; }
        
        /// <summary>
        /// Static Method to return an object based on the id
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static PageRoute Read( int id )
        {
            return Read<PageRoute>( id );
        }

        /// <summary>
        /// Gets or sets the Page.
        /// </summary>
        /// <value>
        /// A <see cref="Page"/> object.
        /// </value>
        public virtual Page Page { get; set; }

        /// <summary>
        /// Gets the dto.
        /// </summary>
        /// <returns></returns>
        public override IDto Dto
        {
            get { return this.ToDto(); }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Route;
        }

        /// <summary>
        /// Exports the object.
        /// </summary>
        /// <returns></returns>
        public object ExportObject()
        {
            return this.ToDynamic();
        }

        /// <summary>
        /// Exports the object as JSON.
        /// </summary>
        /// <returns></returns>
        public string ExportJson()
        {
            return ExportObject().ToJSON();
        }

        /// <summary>
        /// Imports the object from JSON.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ImportJson( string data )
        {
            throw new NotImplementedException();
        }

    }

    /// <summary>
    /// Page Route Configuration class.
    /// </summary>
    public partial class PageRouteConfiguration : EntityTypeConfiguration<PageRoute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageRouteConfiguration"/> class.
        /// </summary>
        public PageRouteConfiguration()
        {
            this.HasRequired( p => p.Page ).WithMany( p => p.PageRoutes ).HasForeignKey( p => p.PageId ).WillCascadeOnDelete( true );
        }
    }
}
