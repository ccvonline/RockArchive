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

using Rock.Financial;

namespace Rock.Rest.Financial
{
    /// <summary>
    /// Pledges REST API
    /// </summary>
    public partial class PledgesController : Rock.Rest.ApiController<Rock.Financial.Pledge, Rock.Financial.PledgeDto>
    {
        public PledgesController() : base( new Rock.Financial.PledgeService() ) { } 
    }
}
