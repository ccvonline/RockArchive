
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using church.ccv.Pastoral.Model;
using church.ccv.Pastoral.Web.UI.Controls;
using Rock;
using Rock.Data;
using Rock.Field;
using Rock.Reporting;
using Rock.Web.Cache;

namespace church.ccv.Pastoral.Field.Types
{
    /// <summary>
    /// Field Type to select a single (or null) Care Request
    /// Stored as Accounts's Guid
    /// </summary>
    public class CareRequestFieldType : FieldType, IEntityFieldType
    {

        #region Formatting

        /// <summary>
        /// Returns the field's current value(s)
        /// </summary>
        /// <param name="parentControl">The parent control.</param>
        /// <param name="value">Information about the value</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="condensed">Flag indicating if the value should be condensed (i.e. for use in a grid column)</param>
        /// <returns></returns>
        public override string FormatValue( System.Web.UI.Control parentControl, string value, Dictionary<string, ConfigurationValue> configurationValues, bool condensed )
        {
            string formattedValue = value;

            if ( !string.IsNullOrWhiteSpace( value ) )
            {
                using ( var rockContext = new RockContext() )
                {
                    var request = new Service<CareRequest>( rockContext ).Get( value.AsGuid() );
                    if ( request != null )
                    {
                        formattedValue = request.FirstName + " " + request.LastName + " (" + request.RequestDateTime.ToString( "MM/dd/yyyy" ) + ")";
                    }
                }
            }

            return base.FormatValue( parentControl, formattedValue, configurationValues, condensed );
        }

        #endregion

        #region Edit Control

        /// <summary>
        /// Creates the control(s) necessary for prompting user for a new value
        /// </summary>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="id"></param>
        /// <returns>
        /// The control
        /// </returns>
        public override System.Web.UI.Control EditControl( Dictionary<string, ConfigurationValue> configurationValues, string id )
        {
            var requestPicker = new CareRequestPicker { ID = id };

            using ( var rockContext = new RockContext() )
            {
                var requestList = new Service<CareRequest>( rockContext )
                    .Queryable().AsNoTracking()
                    .OrderBy( a => a.FirstName )
                    .ThenBy( a => a.LastName ) 
                    .ToList();

                if ( requestList.Any() )
                {
                    requestPicker.Requests = requestList;
                    return requestPicker;
                }
            }

            return null;
        }

        /// <summary>
        /// Reads new values entered by the user for the field
        /// returns CareRequest.Guid as string
        /// </summary>
        /// <param name="control">Parent control that controls were added to in the CreateEditControl() method</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <returns></returns>
        public override string GetEditValue( System.Web.UI.Control control, Dictionary<string, ConfigurationValue> configurationValues )
        {
            CareRequestPicker careRequestPicker = control as CareRequestPicker;

            if ( careRequestPicker != null )
            {
                int? requestId = careRequestPicker.SelectedRequestId;
                if ( requestId.HasValue )
                {
                    using ( var rockContext = new RockContext() )
                    {
                        var request = new Service<CareRequest>( rockContext ).Get( requestId.Value );
                        if ( request != null )
                        {
                            return request.Guid.ToString();
                        }
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Sets the value.
        /// Expects value as a CareRequest.Guid as string
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="value">The value.</param>
        public override void SetEditValue( System.Web.UI.Control control, Dictionary<string, ConfigurationValue> configurationValues, string value )
        {
            CareRequestPicker careRequestPicker = control as CareRequestPicker;

            if ( careRequestPicker != null )
            {
                Guid guid = value.AsGuid();

                // get the item (or null) and set it
                using ( var rockContext = new RockContext() )
                {
                    var request = new Service<CareRequest>( rockContext ).Get( guid );
                    if ( request != null )
                    {
                        careRequestPicker.SetValue( request == null ? "0" : request.Id.ToString() );
                    }
                }
            }
        }

        #endregion

        #region Filter Control

        /// <summary>
        /// Creates the control needed to filter (query) values using this field type.
        /// </summary>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="required">if set to <c>true</c> [required].</param>
        /// <param name="filterMode">The filter mode.</param>
        /// <returns></returns>
        public override System.Web.UI.Control FilterControl( System.Collections.Generic.Dictionary<string, ConfigurationValue> configurationValues, string id, bool required, Rock.Reporting.FilterMode filterMode )
        {
            // This field type does not support filtering
            return null;
        }

        /// <summary>
        /// Determines whether this filter has a filter control
        /// </summary>
        /// <returns></returns>
        public override bool HasFilterControl()
        {
            return false;
        }

        #endregion

        #region Entity Methods

        /// <summary>
        /// Gets the edit value as the IEntity.Id
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <returns></returns>
        public int? GetEditValueAsEntityId( System.Web.UI.Control control, Dictionary<string, ConfigurationValue> configurationValues )
        {
            Guid guid = GetEditValue( control, configurationValues ).AsGuid();

            using ( var rockContext = new RockContext() )
            {
                var request = new Service<CareRequest>( rockContext ).Get( guid );
                if ( request != null )
                {
                    return request.Id;
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the edit value from IEntity.Id value
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="id">The identifier.</param>
        public void SetEditValueFromEntityId( System.Web.UI.Control control, Dictionary<string, ConfigurationValue> configurationValues, int? id )
        {
            CareRequest item = null;
            if ( id.HasValue )
            {
                item = new Service<CareRequest>( new RockContext() ).Get( id.Value );
            }

            string guidValue = item != null ? item.Guid.ToString() : string.Empty;
            SetEditValue( control, configurationValues, guidValue );
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IEntity GetEntity( string value )
        {
            return GetEntity( value, null );
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="rockContext">The rock context.</param>
        /// <returns></returns>
        public IEntity GetEntity( string value, RockContext rockContext )
        {
            Guid? guid = value.AsGuidOrNull();
            if ( guid.HasValue )
            {
                rockContext = rockContext ?? new RockContext();
                return new Service<CareRequest>( rockContext ).Get( guid.Value );
            }

            return null;
        }

        #endregion

    }
}