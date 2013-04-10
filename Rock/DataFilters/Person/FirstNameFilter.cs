﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq.Expressions;

using Rock.Model;

namespace Rock.DataFilters.Person
{
    /// <summary>
    /// 
    /// </summary>
    [Description( "Filter people on First Name" )]
    [Export( typeof( DataFilterComponent ) )]
    [ExportMetadata( "ComponentName", "Person First Name Filter" )]
    public class FirstNameFilter : DataFilterComponent
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get { return "First Name"; }
        }

        /// <summary>
        /// Gets the name of the filtered entity type.
        /// </summary>
        /// <value>
        /// The name of the filtered entity type.
        /// </value>
        public override string FilteredEntityTypeName
        {
            get { return "Rock.Model.Person"; }
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <value>
        /// The section.
        /// </value>
        public override string Section
        {
            get { return "Additional Filters"; }
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <param name="serviceInstance">The service instance.</param>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <param name="selection">The selection.</param>
        /// <returns></returns>
        public override Expression GetExpression( object serviceInstance, Expression parameterExpression, string selection )
        {
            ComparisonType comparisonType = ComparisonType.StartsWith;
            string value = string.Empty;

            string[] options = selection.Split( '|' );
            if ( options.Length > 0 )
            {
                try { comparisonType = options[0].ConvertToEnum<ComparisonType>(); }
                catch { }
            }
            if ( options.Length > 1 )
            {
                value = options[1];
            }

            MemberExpression gnProperty = Expression.Property( parameterExpression, "GivenName" );
            MemberExpression nnProperty = Expression.Property( parameterExpression, "NickName" );
            Expression property = Expression.Coalesce( nnProperty, gnProperty );
            Expression constant = Expression.Constant( value );
            return ComparisonExpression( comparisonType, property, constant );
        }
    }
}