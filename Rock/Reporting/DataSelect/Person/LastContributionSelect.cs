﻿using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI.Controls;
using Rock;
using System;

namespace Rock.Reporting.DataSelect.Person
{
    /// <summary>
    /// 
    /// </summary>
    [Description( "Selects Last Contribution Date for a Person" )]
    [Export( typeof( DataSelectComponent ) )]
    [ExportMetadata( "ComponentName", "Select Person Last Contribution" )]
    public class LastContributionSelect : DataSelectComponent<Rock.Model.Person>
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get
            {
                return "Last Contribution";
            }
        }

        /// <summary>
        /// Gets the name of the entity type.
        /// </summary>
        /// <value>
        /// The name of the entity type.
        /// </value>
        public override string EntityTypeName
        {
            get
            {
                return typeof( Rock.Model.Person ).FullName;
            }
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
                return "Last Contribution Date";
            }
        }

        /*
         -- Example1: turn something like this into Linq 
         select 
            p.FirstName, 
           (select max(TransactionDateTime) from FinancialTransaction where AuthorizedPersonId = p.Id and AccountID in (3,4,5)) [LastDateTime]
         from Person p
         * 
         
         -- Example2: turn something like this into linq
        select 
            p.FirstName, 
            g.Name [FamilyName]
        from Person p
            left outer join GroupMember gm on gm.PersonId = p.Id
            left outer join Group g on gm.GroupId = g.Id
            where g.GroupTypeId = :familyGroupTypeId
         
         */

        #region Query

        /// <summary>
        /// Returns an IQueryable that subquery of this DataSelectComponent
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <returns></returns>
        public override IQueryable<IEntity> SubQuery( string selection )
        {
            IQueryable<FinancialTransaction> lastTransactionQry = new FinancialTransactionService().Queryable().OrderByDescending( o => o.TransactionDateTime );

            // split the selection into parts of Control Values (AccountIds will be the first one)
            string[] controlValues = selection.Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries );

            if ( controlValues.Count() > 0 )
            {
                // get the selected AccountId(s).  If there are any, limit to transactions that for that Account
                var selectedAccountIds = controlValues[0].Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ).Select( a => a.AsInteger() ?? 0 ).ToList();
                if ( selectedAccountIds.Count() > 0 )
                {
                    lastTransactionQry = lastTransactionQry
                        .Where(a => selectedAccountIds
                            .Contains( a.TransactionDetails.Select( s => s.AccountId ).FirstOrDefault() )
                            );
                }
            }


            return lastTransactionQry;

        }

        /// <summary>
        /// The Linq Expression for the Select portion of the SubQuery
        /// </summary>
        /// <returns></returns>
        public override Expression<System.Func<IEntity, DataSelectData>> SelectExpression
        {
            get
            {
                Expression<Func<IEntity, DataSelectData>> lastTranSelect = a => new DataSelectData
                {
                    EntityId = ( a as FinancialTransaction ).AuthorizedPersonId ?? 0,
                    Data = new
                    {
                        // this should be the same as ColumnPropertyName
                        LastTransactionDateTime = ( a as FinancialTransaction ).TransactionDateTime
                    }
                };

                return lastTranSelect;
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
                return "LastTransactionDateTime";
            }
        }

        #endregion

        #region Controls methods

        /// <summary>
        /// Creates the child controls.
        /// </summary>
        /// <param name="parentControl"></param>
        /// <returns></returns>
        public override System.Web.UI.Control[] CreateChildControls( System.Web.UI.Control parentControl )
        {
            AccountPicker accountPicker = new AccountPicker();
            accountPicker.AllowMultiSelect = true;
            accountPicker.ID = parentControl.ID + "_accountPicker";
            accountPicker.Label = "Account";
            accountPicker.Help = "Pick accounts to show the last time the person made a contribution into any of those accounts. Leave blank if you don't want to limit it to specific accounts.";
            parentControl.Controls.Add( accountPicker );

            return new System.Web.UI.Control[] { accountPicker };
        }

        /// <summary>
        /// Formats the selection.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <returns></returns>
        public override string FormatSelection( string selection )
        {
            // TODO: 
            return base.FormatSelection( selection );
        }

        /// <summary>
        /// Formats the selection on the client-side.  When the widget is collapsed by the user, the Filterfield control
        /// will set the description of the filter to whatever is returned by this property.  If including script, the
        /// controls parent container can be referenced through a '$content' variable that is set by the control before
        /// referencing this property.
        /// </summary>
        /// <returns></returns>
        /// <value>
        /// The client format script.
        ///   </value>
        public override string GetClientFormatSelection()
        {
            // TODO: 
            return base.GetClientFormatSelection();
        }

        /// <summary>
        /// Renders the controls.
        /// </summary>
        /// <param name="parentControl">The parent control.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="controls">The controls.</param>
        public override void RenderControls( System.Web.UI.Control parentControl, System.Web.UI.HtmlTextWriter writer, System.Web.UI.Control[] controls )
        {
            // TODO: 
            base.RenderControls( parentControl, writer, controls );
        }

        /// <summary>
        /// Gets the selection.
        /// </summary>
        /// <param name="controls">The controls.</param>
        /// <returns></returns>
        public override string GetSelection( System.Web.UI.Control[] controls )
        {
            if ( controls.Count() == 1 )
            {
                AccountPicker accountPicker = controls[0] as AccountPicker;
                if ( accountPicker != null )
                {
                    return accountPicker.SelectedValueAsId().ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the selection.
        /// </summary>
        /// <param name="controls">The controls.</param>
        /// <param name="selection">The selection.</param>
        public override void SetSelection( System.Web.UI.Control[] controls, string selection )
        {
            if ( controls.Count() == 1 )
            {
                AccountPicker accountPicker = controls[0] as AccountPicker;
                if ( accountPicker != null )
                {
                    var account = new FinancialAccountService().Get( selection.AsInteger() ?? 0 );
                    accountPicker.SetValue( account );
                }
            }
        }

        #endregion
    }
}
