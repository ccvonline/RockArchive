//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// CommunicationRecipient Service class
    /// </summary>
    public partial class CommunicationRecipientService : Service<CommunicationRecipient>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationRecipientService"/> class
        /// </summary>
        public CommunicationRecipientService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationRecipientService"/> class
        /// </summary>
        public CommunicationRecipientService(IRepository<CommunicationRecipient> repository) : base(repository)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( CommunicationRecipient item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static class CommunicationRecipientExtensionMethods
    {
        /// <summary>
        /// Clones this CommunicationRecipient object to a new CommunicationRecipient object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static CommunicationRecipient Clone( this CommunicationRecipient source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as CommunicationRecipient;
            }
            else
            {
                var target = new CommunicationRecipient();
                target.PersonId = source.PersonId;
                target.CommunicationId = source.CommunicationId;
                target.Id = source.Id;
                target.Guid = source.Guid;

            
                return target;
            }
        }
    }
}
