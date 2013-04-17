﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Rock.Model;
using Rock.Web.UI.Controls;

namespace Rock.Field.Types
{
    public class AccountsFieldType : FieldType
    {
        /// <summary>
        /// Returns the field's current value(s)
        /// </summary>
        /// <param name="parentControl">The parent control.</param>
        /// <param name="value">Information about the value</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="condensed">Flag indicating if the value should be condensed (i.e. for use in a grid column)</param>
        /// <returns></returns>
        public override string FormatValue( Control parentControl, string value, Dictionary<string, ConfigurationValue> configurationValues, bool condensed )
        {
            if ( !string.IsNullOrWhiteSpace( value ) )
            {
                var guids = value.SplitDelimitedValues();
                var service = new FinancialAccountService();
                var accounts = service.Queryable().Where( a => guids.Contains( a.Guid.ToString() ) );

                if ( accounts.Any() )
                {
                    return string.Join( ", ", ( from account in accounts select account.PublicName ).ToArray() );
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Creates the control(s) neccessary for prompting user for a new value
        /// </summary>
        /// <param name="configurationValues">The configuration values.</param>
        /// <returns>
        /// The control
        /// </returns>
        public override Control EditControl( Dictionary<string, ConfigurationValue> configurationValues )
        {
            var picker = new AccountPicker { AllowMultiSelect = true };
            return picker;
        }

        /// <summary>
        /// Reads new values entered by the user for the field
        /// </summary>
        /// <param name="control">Parent control that controls were added to in the CreateEditControl() method</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <returns></returns>
        public override string GetEditValue( Control control, Dictionary<string, ConfigurationValue> configurationValues )
        {
            var picker = control as AccountPicker;
            string result = null;

            if ( picker != null )
            {
                var guid = Guid.Empty;
                var id = picker.ItemId.AsInteger();
                var account = new FinancialAccountService().Get( id ?? 0 );

                if ( account != null )
                {
                    guid = account.Guid;
                }

                result = guid.ToString();
            }

            return result;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="configurationValues">The configuration values.</param>
        /// <param name="value">The value.</param>
        public override void SetEditValue( Control control, Dictionary<string, ConfigurationValue> configurationValues, string value )
        {
            if ( value != null )
            {
                var picker = control as AccountPicker;

                if ( picker != null )
                {
                    Guid guid;
                    Guid.TryParse( value, out guid );
                    var account = new FinancialAccountService().Get( guid );
                    picker.SetValue( account );
                }
            }
        }
    }
}
