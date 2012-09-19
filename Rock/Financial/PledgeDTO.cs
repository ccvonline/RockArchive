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

namespace Rock.Financial
{
	/// <summary>
	/// Data Transfer Object for Pledge object
	/// </summary>
	public partial class PledgeDto : IDto
	{

#pragma warning disable 1591
		public int? PersonId { get; set; }
		public int? FundId { get; set; }
		public decimal Amount { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int? FrequencyTypeId { get; set; }
		public decimal? FrequencyAmount { get; set; }
		public DateTime? ModifiedDateTime { get; set; }
		public DateTime? CreatedDateTime { get; set; }
		public int? CreatedByPersonId { get; set; }
		public int? ModifiedByPersonId { get; set; }
		public int Id { get; set; }
		public Guid Guid { get; set; }
#pragma warning restore 1591

		/// <summary>
		/// Instantiates a new DTO object
		/// </summary>
		public PledgeDto ()
		{
		}

		/// <summary>
		/// Instantiates a new DTO object from the model
		/// </summary>
		/// <param name="pledge"></param>
		public PledgeDto ( Pledge pledge )
		{
			CopyFromModel( pledge );
		}

		/// <summary>
		/// Copies the model property values to the DTO properties
		/// </summary>
		/// <param name="pledge"></param>
		public void CopyFromModel( IModel model )
		{
			if ( model is Pledge )
			{
				var pledge = (Pledge)model;
				this.PersonId = pledge.PersonId;
				this.FundId = pledge.FundId;
				this.Amount = pledge.Amount;
				this.StartDate = pledge.StartDate;
				this.EndDate = pledge.EndDate;
				this.FrequencyTypeId = pledge.FrequencyTypeId;
				this.FrequencyAmount = pledge.FrequencyAmount;
				this.ModifiedDateTime = pledge.ModifiedDateTime;
				this.CreatedDateTime = pledge.CreatedDateTime;
				this.CreatedByPersonId = pledge.CreatedByPersonId;
				this.ModifiedByPersonId = pledge.ModifiedByPersonId;
				this.Id = pledge.Id;
				this.Guid = pledge.Guid;
			}
		}

		/// <summary>
		/// Copies the DTO property values to the model properties
		/// </summary>
		/// <param name="pledge"></param>
		public void CopyToModel ( IModel model )
		{
			if ( model is Pledge )
			{
				var pledge = (Pledge)model;
				pledge.PersonId = this.PersonId;
				pledge.FundId = this.FundId;
				pledge.Amount = this.Amount;
				pledge.StartDate = this.StartDate;
				pledge.EndDate = this.EndDate;
				pledge.FrequencyTypeId = this.FrequencyTypeId;
				pledge.FrequencyAmount = this.FrequencyAmount;
				pledge.ModifiedDateTime = this.ModifiedDateTime;
				pledge.CreatedDateTime = this.CreatedDateTime;
				pledge.CreatedByPersonId = this.CreatedByPersonId;
				pledge.ModifiedByPersonId = this.ModifiedByPersonId;
				pledge.Id = this.Id;
				pledge.Guid = this.Guid;
			}
		}
	}
}
