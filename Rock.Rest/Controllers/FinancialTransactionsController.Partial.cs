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
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rock.Data;
using Rock.Model;
using Rock.Rest.Filters;
using Rock.Security;

namespace Rock.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FinancialTransactionsController : IHasCustomRoutes
    {
        /// <summary>
        /// Adds the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public void AddRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "FinancialTransactionGetMicr",
                routeTemplate: "api/FinancialTransactions/PostScanned",
                defaults: new
                {
                    controller = "FinancialTransactions",
                    action = "PostScanned"
                } );

            routes.MapHttpRoute(
                name: "GetContributionPersonGroupAddress",
                routeTemplate: "api/FinancialTransactions/GetContributionPersonGroupAddress",
                defaults: new
                {
                    controller = "FinancialTransactions",
                    action = "GetContributionPersonGroupAddress"
                } );

            routes.MapHttpRoute(
                name: "GetContributionTransactionsGroup",
                routeTemplate: "api/FinancialTransactions/GetContributionTransactions/{groupId}",
                defaults: new
                {
                    controller = "FinancialTransactions",
                    action = "GetContributionTransactions"
                } );

            routes.MapHttpRoute(
                name: "GetContributionTransactionsPerson",
                routeTemplate: "api/FinancialTransactions/GetContributionTransactions/{groupId}/{personId}",
                defaults: new
                {
                    controller = "FinancialTransactions",
                    action = "GetContributionTransactions"
                } );
        }

        /// <summary>
        /// Posts the scanned.
        /// </summary>
        /// <param name="financialTransaction">The financial transaction.</param>
        /// <param name="checkMicr">The check micr.</param>
        /// <returns></returns>
        [Authenticate, Secured]
        [HttpPost]
        public HttpResponseMessage PostScanned( [FromBody]FinancialTransactionScannedCheck financialTransactionScannedCheck )
        {
            financialTransactionScannedCheck.CheckMicrEncrypted = Encryption.EncryptString( financialTransactionScannedCheck.ScannedCheckMicr );
            FinancialTransaction financialTransaction = FinancialTransaction.FromJson( financialTransactionScannedCheck.ToJson() );
            return this.Post( financialTransaction );
        }

        /// <summary>
        /// Gets the contribution person group address.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <exception cref="System.Web.Http.HttpResponseException">
        /// </exception>
        [Authenticate, Secured]
        [HttpPost]
        public DataSet GetContributionPersonGroupAddress( [FromBody]Rock.Net.RestParameters.ContributionStatementOptions options )
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add( "startDate", options.StartDate );
            if ( options.EndDate.HasValue )
            {
                parameters.Add( "endDate", options.EndDate.Value );
            }
            else
            {
                parameters.Add( "endDate", DateTime.MaxValue );
            }

            if ( options.AccountIds != null )
            {
                parameters.Add( "accountIds", options.AccountIds.AsDelimited( "," ) );
            }
            else
            {
                parameters.Add( "accountIds", DBNull.Value );
            }

            if ( options.PersonId.HasValue )
            {
                parameters.Add( "personId", options.PersonId );
            }
            else
            {
                parameters.Add( "personId", DBNull.Value );
            }

            parameters.Add( "orderByZipCode", options.OrderByZipCode );
            var result = new Service( Service.Context).GetDataSet( "spFinance_ContributionStatementQuery", System.Data.CommandType.StoredProcedure, parameters );

            if ( result.Tables.Count > 0 )
            {
                result.Tables[0].TableName = "contribution_person_group_address";
            }

            return result;
        }

        /// <summary>
        /// Gets the contribution transactions.
        /// </summary>
        /// <param name="groupId">The group unique identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        [Authenticate, Secured]
        [HttpPost]
        public DataSet GetContributionTransactions( int groupId, [FromBody]Rock.Net.RestParameters.ContributionStatementOptions options )
        {
            return GetContributionTransactions( groupId, null, options );
        }

        /// <summary>
        /// Gets the contribution transactions.
        /// </summary>
        /// <param name="personId">The person unique identifier.</param>
        /// <param name="groupId">The group unique identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        [Authenticate, Secured]
        [HttpPost]
        public DataSet GetContributionTransactions( int groupId, int? personId, [FromBody]Rock.Net.RestParameters.ContributionStatementOptions options )
        {
            var qry = Get()
                .Where( a => a.TransactionDateTime >= options.StartDate )
                .Where( a => a.TransactionDateTime < ( options.EndDate ?? DateTime.MaxValue ) );

            if ( personId.HasValue )
            {
                // get transactions for a specific person
                qry = qry.Where( a => a.AuthorizedPersonId == personId.Value );
            }
            else
            {
                // get transactions for all the persons in the specified group that have specified that group as their GivingGroup
                GroupMemberService groupMemberService = new GroupMemberService( (RockContext)Service.Context );
                var personIdList = groupMemberService.GetByGroupId( groupId ).Where( a => a.Person.GivingGroupId == groupId ).Select( s => s.PersonId ).ToList();

                qry = qry.Where( a => personIdList.Contains( a.AuthorizedPersonId.Value ) );
            }

            if ( options.AccountIds != null )
            {
                qry = qry.Where( a => options.AccountIds.Contains( a.TransactionDetails.FirstOrDefault().AccountId ) );
            }

            var selectQry = qry.Select( a => new
            {
                a.TransactionDateTime,
                CurrencyTypeValueName = a.CurrencyTypeValue.Name,
                a.Summary,
                Account = a.TransactionDetails.FirstOrDefault().Account,
                TotalAmount = a.TransactionDetails.Sum( d=> d.Amount)
            } ).OrderBy( a => a.TransactionDateTime );

            DataTable dataTable = new DataTable( "contribution_transactions" );
            dataTable.Columns.Add( "TransactionDateTime", typeof(DateTime) );
            dataTable.Columns.Add( "CurrencyTypeValueName" );
            dataTable.Columns.Add( "Summary" );
            dataTable.Columns.Add( "AccountId", typeof(int) );
            dataTable.Columns.Add( "AccountName" );
            dataTable.Columns.Add( "Amount", typeof(decimal) );

            var list = selectQry.ToList();

            dataTable.BeginLoadData();
            foreach ( var fieldItems in list )
            {
                var itemArray = new object[] {
                    fieldItems.TransactionDateTime,
                    fieldItems.CurrencyTypeValueName,
                    fieldItems.Summary,
                    fieldItems.Account.Id,
                    fieldItems.Account.Name,
                    fieldItems.TotalAmount
                };

                dataTable.Rows.Add( itemArray );
            }

            dataTable.EndLoadData();

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add( dataTable );

            return dataSet;
        }
    }
}
