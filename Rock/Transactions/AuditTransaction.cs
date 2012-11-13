﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.Collections.Generic;

using Rock.Core;

namespace Rock.Transactions
{
    /// <summary>
    /// Writes entity audits 
    /// </summary>
    public class AuditTransaction : ITransaction
    {
        /// <summary>
        /// Gets or sets the audits.
        /// </summary>
        /// <value>
        /// The audits.
        /// </value>
        public List<AuditDto> Audits { get; set; }

        /// <summary>
        /// Execute method to write transaction to the database.
        /// </summary>
        public void Execute()
        {
            if ( Audits != null && Audits.Count > 0 )
            {
                var auditService = new AuditService();

                foreach ( var auditDto in Audits )
                {
                    var audit = new Audit();
                    auditDto.CopyToModel( audit );

                    auditService.Add( audit, audit.PersonId );
                    auditService.Save( audit, audit.PersonId );
                }
            }
        }
    }
}