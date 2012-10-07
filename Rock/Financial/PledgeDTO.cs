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
		/// Instantiates a new DTO object from the entity
		/// </summary>
		/// <param name="pledge"></param>
		public PledgeDto ( Pledge pledge )
		{
			CopyFromModel( pledge );
		}

		/// <summary>
		/// Copies the model property values to the DTO properties
		/// </summary>
		/// <param name="model">The model</param>
		public void CopyFromModel( IEntity model )
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
				this.Id = pledge.Id;
				this.Guid = pledge.Guid;
			}
		}

		/// <summary>
		/// Copies the DTO property values to the entity properties
		/// </summary>
		/// <param name="model">The model</param>
		public void CopyToModel ( IEntity model )
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
				pledge.Id = this.Id;
				pledge.Guid = this.Guid;
			}
		}
	}
}
