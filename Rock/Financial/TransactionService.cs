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

namespace Rock.Financial
    
    /// <summary>
    /// Transaction Service class
    /// </summary>
    public partial class TransactionService : Service<Transaction, TransactionDto>
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionService"/> class
        /// </summary>
        public TransactionService()
            : base()
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionService"/> class
        /// </summary>
        public TransactionService(IRepository<Transaction> repository) : base(repository)
            
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override Transaction CreateNew()
            
            return new Transaction();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<TransactionDto> QueryableDto( )
            
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<TransactionDto> QueryableDto( IQueryable<Transaction> items )
            
            return items.Select( m => new TransactionDto()
                    
                    Description = m.Description,
                    TransactionDate = m.TransactionDate,
                    Entity = m.Entity,
                    EntityId = m.EntityId,
                    BatchId = m.BatchId,
                    CurrencyTypeId = m.CurrencyTypeId,
                    CreditCardTypeId = m.CreditCardTypeId,
                    Amount = m.Amount,
                    RefundTransactionId = m.RefundTransactionId,
                    TransactionImageId = m.TransactionImageId,
                    TransactionCode = m.TransactionCode,
                    GatewayId = m.GatewayId,
                    SourceTypeId = m.SourceTypeId,
                    Summary = m.Summary,
                    Id = m.Id,
                    Guid = m.Guid,
                });
        }
    }
}
