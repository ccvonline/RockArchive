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

namespace Rock.Core
    
    /// <summary>
    /// Data Transfer Object for TaggedItem object
    /// </summary>
    public partial class TaggedItemDto : IDto
        

#pragma warning disable 1591
        public bool IsSystem      get; set; }
        public int TagId      get; set; }
        public int? EntityId      get; set; }
        public int Id      get; set; }
        public Guid Guid      get; set; }
#pragma warning restore 1591

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public TaggedItemDto ()
            
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="taggedItem"></param>
        public TaggedItemDto ( TaggedItem taggedItem )
            
            CopyFromModel( taggedItem );
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyFromModel( IEntity model )
            
            if ( model is TaggedItem )
                
                var taggedItem = (TaggedItem)model;
                this.IsSystem = taggedItem.IsSystem;
                this.TagId = taggedItem.TagId;
                this.EntityId = taggedItem.EntityId;
                this.Id = taggedItem.Id;
                this.Guid = taggedItem.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
            
            if ( model is TaggedItem )
                
                var taggedItem = (TaggedItem)model;
                taggedItem.IsSystem = this.IsSystem;
                taggedItem.TagId = this.TagId;
                taggedItem.EntityId = this.EntityId;
                taggedItem.Id = this.Id;
                taggedItem.Guid = this.Guid;
            }
        }
    }
}
