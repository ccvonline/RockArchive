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

using Rock.Model;

namespace Rock.Rest.Financial
{
    /// <summary>
    /// FinancialTransactionDetails REST API
    /// </summary>
    public partial class FinancialTransactionDetailsController : Rock.Rest.ApiController<Rock.Model.FinancialTransactionDetail, Rock.Model.FinancialTransactionDetailDto>
    {
        public FinancialTransactionDetailsController() : base( new Rock.Model.FinancialTransactionDetailService() ) { } 
    }
}
