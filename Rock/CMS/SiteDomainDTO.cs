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

using Rock.Data;

namespace Rock.Cms
    
    /// <summary>
    /// Data Transfer Object for SiteDomain object
    /// </summary>
    public partial class SiteDomainDto : IDto
        

#pragma warning disable 1591
        public bool IsSystem      get; set; }
        public int SiteId      get; set; }
        public string Domain      get; set; }
        public int Id      get; set; }
        public Guid Guid      get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public SiteDomainDto ()
            
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="siteDomain"></param>
        public SiteDomainDto ( SiteDomain siteDomain )
            
            CopyFromModel( siteDomain );
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyFromModel( IEntity model )
            
            if ( model is SiteDomain )
                
                var siteDomain = (SiteDomain)model;
                this.IsSystem = siteDomain.IsSystem;
                this.SiteId = siteDomain.SiteId;
                this.Domain = siteDomain.Domain;
                this.Id = siteDomain.Id;
                this.Guid = siteDomain.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
            
            if ( model is SiteDomain )
                
                var siteDomain = (SiteDomain)model;
                siteDomain.IsSystem = this.IsSystem;
                siteDomain.SiteId = this.SiteId;
                siteDomain.Domain = this.Domain;
                siteDomain.Id = this.Id;
                siteDomain.Guid = this.Guid;
            }
        }
    }
}
