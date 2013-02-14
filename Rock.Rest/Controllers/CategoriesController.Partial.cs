//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http;

using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

namespace Rock.Rest.Controllers
{
    public partial class CategoriesController : IHasCustomRoutes
    {
        /// <summary>
        /// Adds the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public void AddRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "CategoriesGetChildren",
                routeTemplate: "api/Categories/GetChildren/{id}/{entityTypeName}",
                defaults: new
                {
                    controller = "Categories",
                    action = "GetChildren"
                } );
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <returns></returns>
        public IQueryable<CategoryItem> GetChildren( int id, string entityTypeName )
        {
            IQueryable<Category> qry;
            qry = Get().Where( a => ( a.ParentCategoryId ?? 0 ) == id );

            int? entityTypeId = null;

            object serviceInstance = null;

            if ( !string.IsNullOrWhiteSpace( entityTypeName ) )
            {
                var entityType = EntityTypeCache.Read( entityTypeName );
                if ( entityType != null )
                {
                    entityTypeId = entityType.Id;
                    qry = qry.Where( a => a.EntityTypeId == entityTypeId );

                    // Get the GetByCategory method
                    if ( entityType.AssemblyName != null )
                    {
                        Type type = Type.GetType( entityType.AssemblyName );
                        if ( type != null )
                        {
                            Type[] modelType = { type };
                            Type genericServiceType = typeof( Rock.Data.Service<> );
                            Type modelServiceType = genericServiceType.MakeGenericType( modelType );

                            serviceInstance = Activator.CreateInstance( modelServiceType );
                        }
                    }
                }
            }

            List<Category> categoryList = qry.ToList();
            List<CategoryItem> categoryItemList = new List<CategoryItem>();

            var appPath = System.Web.VirtualPathUtility.ToAbsolute( "~" );
            string imageUrlFormat = Path.Combine( appPath, "Image.ashx?id={0}&width=25&height=25" );

            foreach ( var category in categoryList )
            {
                var categoryItem = new CategoryItem();
                categoryItem.Id = category.Id;
                categoryItem.Name = System.Web.HttpUtility.HtmlEncode( category.Name );
                categoryItem.EntityTypeName = typeof( Category ).Name;

                // if there a IconCssClass is assigned, use that as the Icon.  Otherwise, use the SmallIcon (if assigned)
                if ( !string.IsNullOrWhiteSpace( category.IconCssClass ) )
                {
                    categoryItem.IconCssClass = category.IconCssClass;
                }
                else
                {
                    categoryItem.IconSmallUrl = category.IconSmallFileId != null ? string.Format( imageUrlFormat, category.IconSmallFileId ) : string.Empty;
                }

                categoryItemList.Add( categoryItem );
            }

            MethodInfo getMethod = serviceInstance.GetType().GetMethod( "Get", new Type[] { typeof(ParameterExpression), typeof(Expression) } );
            IQueryable items = GetCategorizedItems( serviceInstance, getMethod, id ) as IQueryable;
            if (items != null)
            {
                foreach(var item in items)
                {
                    ICategorized categorizedItem = item as ICategorized;
                    if ( categorizedItem != null )
                    {
                        var categoryItem = new CategoryItem();
                        categoryItem.Id = categorizedItem.Id;
                        categoryItem.Name = categorizedItem.Name;
                        categoryItem.EntityTypeName = typeof( WorkflowType ).Name;
                        categoryItem.IconCssClass = "icon-list-ol";
                        categoryItem.IconSmallUrl = string.Empty;
                        categoryItemList.Add( categoryItem );
                    }
                }
            }

            // try to quickly figure out which items have Children
            List<int> resultIds = categoryItemList.Select( a => a.Id ).ToList();

            var qryHasChildrenCategory = from x in Get().Select( a => a.ParentCategoryId )
                                         where resultIds.Contains( x.Value )
                                         select x.Value;

            List<int> qryHasChildrenList = qryHasChildrenCategory.ToList();

            MethodInfo anyMethod = serviceInstance.GetType().GetMethod( "Any", new Type[] { typeof(ParameterExpression), typeof(Expression) } );
            foreach ( var g in categoryItemList )
            {
                if ( g.EntityTypeName.Equals( typeof( Category ).Name ) )
                {
                    g.HasChildren = qryHasChildrenList.Any( a => a == g.Id );
                    if ( !g.HasChildren && serviceInstance != null && getMethod != null )
                    {
                        g.HasChildren = (bool)GetCategorizedItems( serviceInstance, anyMethod, g.Id );
                    }
                }
            }

            return categoryItemList.AsQueryable();
        }

        private object GetCategorizedItems( object serviceInstance, System.Reflection.MethodInfo queryMethod, int categoryId )
        {
            if ( serviceInstance != null && queryMethod != null )
            {
                var paramExpression = serviceInstance.GetType().GetProperty( "ParameterExpression" ).GetValue( serviceInstance ) as ParameterExpression;
                var propertyExpreesion = Expression.Property( paramExpression, "CategoryId" );
                var zeroExpression = Expression.Constant( 0 );
                var coalesceExpression = Expression.Coalesce( propertyExpreesion, zeroExpression );
                var constantExpression = Expression.Constant( categoryId );
                var compareExpression = Expression.Equal( coalesceExpression, constantExpression );

                return queryMethod.Invoke( serviceInstance, new object[] { paramExpression, compareExpression } );
            }

            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CategoryItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity type.
        /// </summary>
        /// <value>
        /// The name of the entity type.
        /// </value>
        public string EntityTypeName { get; set; }

        /// <summary>
        /// Gets or sets the group type icon CSS class.
        /// </summary>
        /// <value>
        /// The group type icon CSS class.
        /// </value>
        public string IconCssClass { get; set; }

        /// <summary>
        /// Gets or sets the group type icon small id.
        /// </summary>
        /// <value>
        /// The group type icon small id.
        /// </value>
        public string IconSmallUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has children.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has children; otherwise, <c>false</c>.
        /// </value>
        public bool HasChildren { get; set; }
    }
}
