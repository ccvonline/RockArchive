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
    /// Gateways REST API
    /// </summary>
    public partial class GatewaysController : Rock.Rest.ApiController<Rock.Financial.Gateway, Rock.Financial.GatewayDto>
    {
        public GatewaysController() : base( new Rock.Financial.GatewayService() ) { } 
    }
}
