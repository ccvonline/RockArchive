﻿using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using church.ccv.Datamart.Model;
using Rock.Data;
using Rock.Model;
using Rock.Reporting;

namespace church.ccv.Datamart.Reporting.DataSelect.Person
{
    /// <summary>
    /// 
    /// </summary>
    [Description( "Select the Neighborhood Pastor of the Person" )]
    [Export( typeof( DataSelectComponent ) )]
    [ExportMetadata( "ComponentName", "Select Person's Neighborhood Pastor" )]
    public class NeighborhoodPastorSelect : DataSelectComponent
    {
        /// <summary>
        /// Gets the name of the entity type. Filter should be an empty string
        /// if it applies to all entities
        /// </summary>
        /// <value>
        /// The name of the entity type.
        /// </value>
        public override string AppliesToEntityType
        {
            get
            {
                return typeof( Rock.Model.Person ).FullName;
            }
        }

        /// <summary>
        /// The PropertyName of the property in the anonymous class returned by the SelectExpression
        /// </summary>
        /// <value>
        /// The name of the column property.
        /// </value>
        public override string ColumnPropertyName
        {
            get
            {
                return "Neighborhood Pastor";
            }
        }

        /// <summary>
        /// Gets the type of the column field.
        /// </summary>
        /// <value>
        /// The type of the column field.
        /// </value>
        public override Type ColumnFieldType
        {
            get { return typeof( string ); }
        }

        /// <summary>
        /// Gets the default column header text.
        /// </summary>
        /// <value>
        /// The default column header text.
        /// </value>
        public override string ColumnHeaderText
        {
            get
            {
                return "Neighborhood Pastor";
            }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        /// <value>
        /// The title.
        /// </value>
        public override string GetTitle( Type entityType )
        {
            return "Neighborhood Pastor";
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="entityIdProperty">The entity identifier property.</param>
        /// <param name="selection">The selection.</param>
        /// <returns></returns>
        public override System.Linq.Expressions.Expression GetExpression( Rock.Data.RockContext context, System.Linq.Expressions.MemberExpression entityIdProperty, string selection )
        {
            var datamartNeighborhoodsService = new Service<DatamartNeighborhoods>( context );
            var datamartPersonService = new Service<DatamartPerson>( context );
            var personService = new PersonService( context );

            var qryDatamartNeighborhoods = datamartNeighborhoodsService.Queryable();
            var qryDatamartPerson = datamartPersonService.Queryable();
            var qryPerson = personService.Queryable();

            var qryResult = qryPerson
                .Select( p => qryDatamartPerson.Where( d => d.PersonId == p.Id )
                  .Select( s => qryDatamartNeighborhoods.Where( n => n.NeighborhoodId == s.NeighborhoodId ).Select( ss => ss.NeighborhoodPastorName ).FirstOrDefault() ).FirstOrDefault() );

            var resultExpresssion = SelectExpressionExtractor.Extract<Rock.Model.Person>( qryResult, entityIdProperty, "p" );

            return resultExpresssion;
        }
    }
}
