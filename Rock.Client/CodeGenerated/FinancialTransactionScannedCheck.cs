//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
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


namespace Rock.Client
{
    /// <summary>
    /// Special Class to use when uploading a FinancialTransaction from a Scanned Check thru the Rest API
    /// </summary>
    public partial class FinancialTransactionScannedCheckEntity
    {
        /// <summary />
        public FinancialTransaction FinancialTransaction { get; set; }

        /// <summary />
        public string ScannedCheckMicr { get; set; }

        /// <summary>
        /// Copies the base properties from a source FinancialTransactionScannedCheck object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( FinancialTransactionScannedCheck source )
        {
            this.FinancialTransaction = source.FinancialTransaction;
            this.ScannedCheckMicr = source.ScannedCheckMicr;

        }
    }

    /// <summary>
    /// Special Class to use when uploading a FinancialTransaction from a Scanned Check thru the Rest API
    /// </summary>
    public partial class FinancialTransactionScannedCheck : FinancialTransactionScannedCheckEntity
    {
    }
}
