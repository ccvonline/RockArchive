using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Runtime.Serialization;
using church.ccv.Datamart.Data;

namespace church.ccv.Datamart.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table( "_church_ccv_Datamart_Family" )]
    [DataContract]
    public partial class DatamartFamily : Rock.Data.Entity<DatamartFamily>, Rock.Data.IRockEntity
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the family identifier.
        /// </summary>
        /// <value>
        /// The family identifier.
        /// </value>
        [DataMember]
        [DatabaseGenerated( DatabaseGeneratedOption.None )]
        public int FamilyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the family.
        /// </summary>
        /// <value>
        /// The name of the family.
        /// </value>
        [DataMember]
        [MaxLength( 100 )]
        public string FamilyName { get; set; }

        /// <summary>
        /// Gets or sets the Head of Household person identifier.
        /// </summary>
        /// <value>
        /// The Head of Household person identifier.
        /// </value>
        [DataMember]
        public int? HHPersonId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the Head of Household.
        /// </summary>
        /// <value>
        /// The first name of the Head of Household.
        /// </value>
        [DataMember]
        [MaxLength( 50 )]
        public string HHFirstName { get; set; }

        /// <summary>
        /// Gets or sets the name of the Head of Household nick.
        /// </summary>
        /// <value>
        /// The name of the Head of Household nick.
        /// </value>
        [DataMember]
        [MaxLength( 50 )]
        public string HHNickName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the Head of Household.
        /// </summary>
        /// <value>
        /// The last name of the Head of Household.
        /// </value>
        [DataMember]
        [MaxLength( 50 )]
        public string HHLastName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the Head of Household.
        /// </summary>
        /// <value>
        /// The full name of the Head of Household.
        /// </value>
        [DataMember]
        [MaxLength( 100 )]
        public string HHFullName { get; set; }

        /// <summary>
        /// Gets or sets the Head of Household gender.
        /// </summary>
        /// <value>
        /// The Head of Household gender.
        /// </value>
        [DataMember]
        [MaxLength( 50 )]
        public string HHGender { get; set; }

        /// <summary>
        /// Gets or sets the Head of Household member status.
        /// </summary>
        /// <value>
        /// The Head of Household member status.
        /// </value>
        [DataMember]
        [MaxLength( 50 )]
        public string HHMemberStatus { get; set; }

        /// <summary>
        /// Gets or sets the Head of Household marital status.
        /// </summary>
        /// <value>
        /// The Head of Household marital status.
        /// </value>
        [DataMember]
        [MaxLength( 50 )]
        public string HHMaritalStatus { get; set; }

        /// <summary>
        /// Gets or sets the Head of Household first visit.
        /// </summary>
        /// <value>
        /// The Head of Household first visit.
        /// </value>
        [DataMember]
        public DateTime? HHFirstVisit { get; set; }

        /// <summary>
        /// Gets or sets the Head of Household first activity.
        /// </summary>
        /// <value>
        /// The Head of Household first activity.
        /// </value>
        [DataMember]
        public DateTime? HHFirstActivity { get; set; }

        /// <summary>
        /// Gets or sets the Head of Household age.
        /// </summary>
        /// <value>
        /// The Head of Household age.
        /// </value>
        [DataMember]
        public int? HHAge { get; set; }

        /// <summary>
        /// Gets or sets the neighborhood identifier.
        /// </summary>
        /// <value>
        /// The neighborhood identifier.
        /// </value>
        [DataMember]
        public int? NeighborhoodId { get; set; }

        /// <summary>
        /// Gets or sets the name of the neighborhood.
        /// </summary>
        /// <value>
        /// The name of the neighborhood.
        /// </value>
        [DataMember]
        public string NeighborhoodName { get; set; }

        /// <summary>
        /// Gets or sets the in neighborhood group.
        /// </summary>
        /// <value>
        /// The in neighborhood group.
        /// </value>
        [DataMember]
        public bool? InNeighborhoodGroup { get; set; }

        /// <summary>
        /// Gets or sets the is era.
        /// </summary>
        /// <value>
        /// The is era.
        /// </value>
        [DataMember]
        public bool? IsEra { get; set; }

        /// <summary>
        /// Gets or sets the name of the nearest neighborhood group.
        /// </summary>
        /// <value>
        /// The name of the nearest neighborhood group.
        /// </value>
        [DataMember]
        [MaxLength( 100 )]
        public string NearestNeighborhoodGroupName { get; set; }

        /// <summary>
        /// Gets or sets the nearest neighborhood group identifier.
        /// </summary>
        /// <value>
        /// The nearest neighborhood group identifier.
        /// </value>
        [DataMember]
        public int? NearestNeighborhoodGroupId { get; set; }

        /// <summary>
        /// Gets or sets the is serving.
        /// </summary>
        /// <value>
        /// The is serving.
        /// </value>
        [DataMember]
        public bool? IsServing { get; set; }

        /// <summary>
        /// Gets or sets the attendance16 week.
        /// </summary>
        /// <value>
        /// The attendance16 week.
        /// </value>
        [DataMember]
        public int? Attendance16Week { get; set; }

        /// <summary>
        /// Gets or sets the connection status.
        /// </summary>
        /// <value>
        /// The connection status.
        /// </value>
        [DataMember]
        [MaxLength( 50 )]
        public string ConnectionStatus { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [DataMember]
        [MaxLength( 100 )]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the home phone.
        /// </summary>
        /// <value>
        /// The home phone.
        /// </value>
        [DataMember]
        [MaxLength( 50 )]
        public string HomePhone { get; set; }

        /// <summary>
        /// Gets or sets the adult count.
        /// </summary>
        /// <value>
        /// The adult count.
        /// </value>
        [DataMember]
        public int? AdultCount { get; set; }

        /// <summary>
        /// Gets or sets the child count.
        /// </summary>
        /// <value>
        /// The child count.
        /// </value>
        [DataMember]
        public int? ChildCount { get; set; }

        /// <summary>
        /// Gets or sets the location identifier.
        /// </summary>
        /// <value>
        /// The location identifier.
        /// </value>
        [DataMember]
        public int? LocationId { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [DataMember]
        [MaxLength( 200 )]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [DataMember]
        [MaxLength( 50 )]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        [DataMember]
        [MaxLength( 50 )]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        [DataMember]
        [MaxLength( 100 )]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        [DataMember]
        [MaxLength( 50 )]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the geo point.
        /// </summary>
        /// <value>
        /// The geo point.
        /// </value>
        [DataMember]
        public DbGeography GeoPoint { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        [DataMember]
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        [DataMember]
        public double? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the campus.
        /// </summary>
        /// <value>
        /// The campus.
        /// </value>
        [DataMember]
        [MaxLength( 50 )]
        [Column( "Campus" )]
        public string CampusName { get; set; }

        /// <summary>
        /// Gets or sets the campus identifier.
        /// </summary>
        /// <value>
        /// The campus identifier.
        /// </value>
        [DataMember]
        [Rock.Data.FieldType( Rock.SystemGuid.FieldType.CAMPUS )]
        public int? CampusId { get; set; }

        /// <summary>
        /// Gets or sets the adult names.
        /// </summary>
        /// <value>
        /// The adult names.
        /// </value>
        [DataMember]
        [MaxLength( 200 )]
        public string AdultNames { get; set; }

        /// <summary>
        /// Gets or sets the child names.
        /// </summary>
        /// <value>
        /// The child names.
        /// </value>
        [DataMember]
        [MaxLength( 200 )]
        public string ChildNames { get; set; }

        /// <summary>
        /// Gets or sets the giving2015.
        /// </summary>
        /// <value>
        /// The giving2015.
        /// </value>
        [DataMember]
        public decimal? Giving2015 { get; set; }

        /// <summary>
        /// Gets or sets the giving2014.
        /// </summary>
        /// <value>
        /// The giving2014.
        /// </value>
        [DataMember]
        public decimal? Giving2014 { get; set; }

        /// <summary>
        /// Gets or sets the giving2013.
        /// </summary>
        /// <value>
        /// The giving2013.
        /// </value>
        [DataMember]
        public decimal? Giving2013 { get; set; }

        /// <summary>
        /// Gets or sets the giving2012.
        /// </summary>
        /// <value>
        /// The giving2012.
        /// </value>
        [DataMember]
        public decimal? Giving2012 { get; set; }

        /// <summary>
        /// Gets or sets the giving2011.
        /// </summary>
        /// <value>
        /// The giving2011.
        /// </value>
        [DataMember]
        public decimal? Giving2011 { get; set; }

        /// <summary>
        /// Gets or sets the giving2010.
        /// </summary>
        /// <value>
        /// The giving2010.
        /// </value>
        [DataMember]
        public decimal? Giving2010 { get; set; }

        /// <summary>
        /// Gets or sets the giving2009.
        /// </summary>
        /// <value>
        /// The giving2009.
        /// </value>
        [DataMember]
        public decimal? Giving2009 { get; set; }

        /// <summary>
        /// Gets or sets the giving2008.
        /// </summary>
        /// <value>
        /// The giving2008.
        /// </value>
        [DataMember]
        public decimal? Giving2008 { get; set; }

        /// <summary>
        /// Gets or sets the giving2007.
        /// </summary>
        /// <value>
        /// The giving2007.
        /// </value>
        [DataMember]
        public decimal? Giving2007 { get; set; }

        /// <summary>
        /// Gets or sets the last attended date.
        /// </summary>
        /// <value>
        /// The last attended date.
        /// </value>
        [DataMember]
        public DateTime? LastAttendedDate { get; set; }

        #endregion
    }
}
