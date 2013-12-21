//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web.UI.WebControls;
using Rock.Data;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace Rock.Model
{
    /// <summary>
    /// Represents a filterable dataview in Rock.
    /// </summary>
    [Table( "DataView" )]
    [DataContract]
    public partial class DataView : Model<DataView>, ICategorized
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets a flag indicating if this DataView is part of the Rock core system/framework.
        /// </summary>
        /// <value>
        /// A <see cref="System.Boolean"/> value that is <c>true</c> if it is part of the Rock core system/framework; otherwise <c>false</c>.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the Name of the DataView.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the Name of the DataView.
        /// </value>
        [Required]
        [MaxLength( 100 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user defined description of the DataView
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the description of the DataView.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the CategoryId of the <see cref="Rock.Model.Category"/> that this DataView belongs to. If there is no Category, this value will be null.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the CategoryId of the <see cref="Rock.Model.Category"/> that this DataView belongs to. If it is not part of a Category this value will be null.
        /// </value>
        [DataMember]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the EntityTypeId of the <see cref="Rock.Model.EntityType"/> that this DataView reports on.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the EntityTypeId of the <see cref="Rock.Model.EntityType"/> that this DataView reports on.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int? EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the DataViewFilterId of the root/base <see cref="Rock.Model.DataViewFilter"/> that is used to generate this DataView. 
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> that represents the DataViewFilterId of the root/base <see cref="Rock.Model.DataViewFilter"/> that is used to generate this DataView. If there is 
        /// not a filter on this DataView, this value will be null.
        /// </value>
        [DataMember]
        public int? DataViewFilterId { get; set; }

        /// <summary>
        /// Gets or sets the EntityTypeId of the <see cref="Rock.Model.EntityType"/> that is used for an optional transformation on this DataView.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the EntityTypeId of the <see cref="Rock.Model.EntityType"/> that is used for an optional transformation on this DataView. If there
        /// is not a transformation on this DataView, this value will be null.
        /// </value>
        [DataMember]
        public int? TransformEntityTypeId { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.Category"/> that this DataView belongs to
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.Category"/> that this DataView belongs to.
        /// </value>
        [DataMember]
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.EntityType"/> that this DataView reports on.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.EntityType"/> that this DataView reports on.
        /// </value>
        [DataMember]
        public virtual EntityType EntityType { get; set; }

        /// <summary>
        /// Gets or sets the base <see cref="Rock.Model.DataViewFilter"/> that is used to generate this DataView.
        /// </summary>
        /// <value>
        /// The base <see cref="Rock.Model.DataViewFilter"/>.
        /// </value>
        [DataMember]
        public virtual DataViewFilter DataViewFilter { get; set; }

        /// <summary>
        /// Gets the parent security authority for the DataView
        /// </summary>
        /// <value>
        /// The parent authority of the DataView.
        /// </value>
        public override Security.ISecured ParentAuthority
        {
            get
            {
                if ( this.Category != null )
                {
                    return this.Category;
                }

                return base.ParentAuthority;
            }
        }

        /// <summary>
        /// Gets or sets the type of the entity used for an optional transformation
        /// </summary>
        /// <value>
        /// The transformation type of entity.
        /// </value>
        [DataMember]
        public virtual EntityType TransformEntityType { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Binds the grid.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <param name="createColumns">if set to <c>true</c> [create columns].</param>
        /// <returns></returns>
        public List<IEntity> BindGrid( Grid grid, out List<string> errorMessages, bool createColumns = false )
        {
            errorMessages = new List<string>();

            if ( EntityTypeId.HasValue )
            {
                var cachedEntityType = EntityTypeCache.Read( EntityTypeId.Value );
                if ( cachedEntityType != null && cachedEntityType.AssemblyName != null )
                {
                    Type entityType = cachedEntityType.GetEntityType();

                    if ( entityType != null )
                    {
                        if ( createColumns )
                        {
                            grid.CreatePreviewColumns( entityType );
                        }

                        using ( new Rock.Data.UnitOfWorkScope() )
                        {
                            var qry = this.GetQuery( out errorMessages );
                            if ( grid.SortProperty != null )
                            {
                                qry = qry.Sort( grid.SortProperty );
                            }

                            return qry.AsNoTracking().ToList();
                        };
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        public IQueryable<IEntity> GetQuery( out List<string> errorMessages )
        {
            errorMessages = new List<string>();

            if ( EntityTypeId.HasValue )
            {
                var cachedEntityType = EntityTypeCache.Read( EntityTypeId.Value );
                if ( cachedEntityType != null && cachedEntityType.AssemblyName != null )
                {
                    Type entityType = cachedEntityType.GetEntityType();

                    if ( entityType != null )
                    {
                        Type[] modelType = { entityType };
                        Type genericServiceType = typeof( Rock.Data.Service<> );
                        Type modelServiceType = genericServiceType.MakeGenericType( modelType );

                        object serviceInstance = Activator.CreateInstance( modelServiceType );

                        if ( serviceInstance != null )
                        {
                            ParameterExpression paramExpression = serviceInstance.GetType().GetProperty( "ParameterExpression" ).GetValue( serviceInstance ) as ParameterExpression;
                            Expression whereExpression = GetExpression( serviceInstance, paramExpression, out errorMessages );

                            MethodInfo getMethod = serviceInstance.GetType().GetMethod( "Get", new Type[] { typeof( ParameterExpression ), typeof( Expression ) } );
                            if ( getMethod != null )
                            {
                                var getResult = getMethod.Invoke( serviceInstance, new object[] { paramExpression, whereExpression } );
                                var qry = getResult as IQueryable<IEntity>;

                                return qry;
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <param name="serviceInstance">The service instance.</param>
        /// <param name="paramExpression">The param expression.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        public Expression GetExpression( object serviceInstance, ParameterExpression paramExpression, out List<string> errorMessages )
        {
            errorMessages = new List<string>();

            var cachedEntityType = EntityTypeCache.Read( EntityTypeId.Value );
            if ( cachedEntityType != null && cachedEntityType.AssemblyName != null )
            {
                Type filteredEntityType = cachedEntityType.GetEntityType();

                if ( filteredEntityType != null )
                {
                    Expression filterExpression = DataViewFilter != null ? DataViewFilter.GetExpression( filteredEntityType, serviceInstance, paramExpression, errorMessages ) : null;

                    Expression transformedExpression = GetTransformExpression( serviceInstance, paramExpression, filterExpression, errorMessages );
                    if ( transformedExpression != null )
                    {
                        return transformedExpression;
                    }

                    return filterExpression;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the transform expression.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <param name="whereExpression">The where expression.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        private Expression GetTransformExpression( object service, Expression parameterExpression, Expression whereExpression, List<string> errorMessages )
        {
            if ( this.TransformEntityTypeId.HasValue )
            {
                var entityType = Rock.Web.Cache.EntityTypeCache.Read( this.TransformEntityTypeId.Value );
                if ( entityType != null )
                {
                    var component = Rock.Reporting.DataTransformContainer.GetComponent( entityType.Name );
                    if ( component != null )
                    {
                        try
                        {
                            return component.GetExpression( service, parameterExpression, whereExpression );
                        }
                        catch ( SystemException ex )
                        {
                            errorMessages.Add( string.Format( "{0}: {1}", component.Title, ex.Message ) );
                        }
                    }
                }
            }

            return null;
        }

        #endregion

    }

    #region Entity Configuration

    /// <summary>
    /// Campus Configuration class.
    /// </summary>
    public partial class DataViewConfiguration : EntityTypeConfiguration<DataView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportConfiguration"/> class.
        /// </summary>
        public DataViewConfiguration()
        {
            this.HasOptional( v => v.Category ).WithMany().HasForeignKey( v => v.CategoryId ).WillCascadeOnDelete( false );
            this.HasOptional( v => v.DataViewFilter ).WithMany().HasForeignKey( v => v.DataViewFilterId ).WillCascadeOnDelete( true );
            this.HasRequired( v => v.EntityType ).WithMany().HasForeignKey( v => v.EntityTypeId ).WillCascadeOnDelete( false );
            this.HasOptional( e => e.TransformEntityType ).WithMany().HasForeignKey( e => e.TransformEntityTypeId ).WillCascadeOnDelete( false );
        }
    }

    #endregion

}
