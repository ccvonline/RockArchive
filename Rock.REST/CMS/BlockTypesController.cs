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

using Rock.Cms;

namespace Rock.Rest.Cms
{
    /// <summary>
    /// BlockTypes REST API
    /// </summary>
    public partial class BlockTypesController : Rock.Rest.ApiController<Rock.Cms.BlockType, Rock.Cms.BlockTypeDto>
    {
        public BlockTypesController() : base( new Rock.Cms.BlockTypeService() ) { } 
    }
}
