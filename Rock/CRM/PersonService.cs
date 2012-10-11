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

namespace Rock.Crm
    
    /// <summary>
    /// Person Service class
    /// </summary>
    public partial class PersonService : Service<Person, PersonDto>
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonService"/> class
        /// </summary>
        public PersonService()
            : base()
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonService"/> class
        /// </summary>
        public PersonService(IRepository<Person> repository) : base(repository)
            
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override Person CreateNew()
            
            return new Person();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<PersonDto> QueryableDto( )
            
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<PersonDto> QueryableDto( IQueryable<Person> items )
            
            return items.Select( m => new PersonDto()
                    
                    IsSystem = m.IsSystem,
                    RecordTypeId = m.RecordTypeId,
                    RecordStatusId = m.RecordStatusId,
                    RecordStatusReasonId = m.RecordStatusReasonId,
                    PersonStatusId = m.PersonStatusId,
                    TitleId = m.TitleId,
                    GivenName = m.GivenName,
                    NickName = m.NickName,
                    LastName = m.LastName,
                    SuffixId = m.SuffixId,
                    PhotoId = m.PhotoId,
                    BirthDay = m.BirthDay,
                    BirthMonth = m.BirthMonth,
                    BirthYear = m.BirthYear,
                    Gender = m.Gender,
                    MaritalStatusId = m.MaritalStatusId,
                    AnniversaryDate = m.AnniversaryDate,
                    GraduationDate = m.GraduationDate,
                    Email = m.Email,
                    IsEmailActive = m.IsEmailActive,
                    EmailNote = m.EmailNote,
                    DoNotEmail = m.DoNotEmail,
                    SystemNote = m.SystemNote,
                    ViewedCount = m.ViewedCount,
                    Id = m.Id,
                    Guid = m.Guid,
                });
        }
    }
}
