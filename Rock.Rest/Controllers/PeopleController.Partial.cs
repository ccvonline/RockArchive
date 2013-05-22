//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;

using Rock.Search.Person;
using Rock.Model;

namespace Rock.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PeopleController : IHasCustomRoutes
    {
        /// <summary>
        /// Adds the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public void AddRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapHttpRoute(
                name: "PeopleSearch",
                routeTemplate: "api/People/Search/{name}",
                defaults: new
                {
                    controller = "People",
                    action = "Search"
                } );

            routes.MapHttpRoute(
                name: "PeopleGetByUserName",
                routeTemplate: "api/People/GetByUserName/{username}",
                defaults: new
                {
                    controller = "People",
                    action = "GetByUserName"
                } );

            routes.MapHttpRoute(
                name: "PeoplePopupHtml",
                routeTemplate: "api/People/PopupHtml/{personId}",
                defaults: new
                {
                    controller = "People",
                    action = "GetPopupHtml"
                } );

        }

        /// <summary>
        /// Searches the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<PersonSearchResult> Search( string name)
        {
            int count = 20;
            bool lastFirst;
            IOrderedQueryable<Person> sortedPersonQry = new PersonService().Queryable().QueryByName( name, out lastFirst );

            var topQry = sortedPersonQry.Take( count );
            List<Person> sortedPersonList = topQry.ToList();

            var appPath = System.Web.VirtualPathUtility.ToAbsolute( "~" );
            string imageUrlFormat = Path.Combine( appPath, "Image.ashx?id={0}&width=25&height=25" );
            string imageNoPhoto = string.Format("<image src='{0}' />", Path.Combine( appPath, "/Assets/images/person-no-photo.jpg" ));
            string itemDetailFormat = @"
<div class='rock-picker-select-item-details clearfix' style='display: none;'>
	{0}
	<div class='rock-picker-select-person-details'>
        {1}
	</div>
</div>
";

            // figure out Family, Address, Spouse
            GroupMemberService groupMemberService = new GroupMemberService();

            List<PersonSearchResult> searchResult = new List<PersonSearchResult>();
            foreach ( var person in sortedPersonList)
            {
                string imageHtml = null;
                if ( person.PhotoId != null )
                {
                    imageHtml = string.Format( imageUrlFormat, person.PhotoId );
                }
                else
                {
                    imageHtml = imageNoPhoto;
                }

                string personInfo = string.Empty;

                var groupMemberQry = groupMemberService.Queryable().Where( a => a.PersonId.Equals( person.Id ) );
                List<GroupMember> personGroupMember = groupMemberQry.ToList();

                GroupMember familyGroupMember = personGroupMember.Where( a => a.Group.GroupType.Guid.Equals( Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY ) ).FirstOrDefault();
                if ( familyGroupMember != null )
                {
                    personInfo += familyGroupMember.GroupRole.Name;
                    if ( person.Age != null )
                    {
                        personInfo += " - " + person.Age.ToString() + "yrs";
                    }

                    // Figure out spouse (Implied by "the other GROUPROLE_FAMILY_MEMBER_ADULT that is of the opposite gender")
                    if ( familyGroupMember.GroupRole.Guid.Equals( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT ) )
                    {
                        GroupMember spouseMember = familyGroupMember.Group.Members.Where( a => !a.PersonId.Equals( person.Id ) && a.GroupRole.Guid.Equals( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT ) ).FirstOrDefault();
                        if ( spouseMember != null )
                        {
                            if ( !familyGroupMember.Person.Gender.Equals( spouseMember.Person.Gender ) )
                            {
                                personInfo += "<h5>Spouse</h5>" + spouseMember.Person.FullName;
                            }
                        }
                    }
                }
                else
                {
                    personInfo += person.Age.ToString() + "yrs";
                }

                if ( familyGroupMember != null )
                {
                    var groupLocation = familyGroupMember.Group.GroupLocations.FirstOrDefault();
                    if ( groupLocation != null )
                    {
                        var location = groupLocation.Location;
                        if ( location != null )
                        {
                            string streetInfo;
                            if ( !string.IsNullOrWhiteSpace( location.Street1 ) )
                            {
                                streetInfo = location.Street1 + " " + location.Street2;
                            }
                            else
                            {
                                streetInfo = location.Street2;
                            }

                            string addressHtml = string.Format( "<h5>Address</h5>{0} {1}, {2}, {3}", streetInfo, location.City, location.State, location.Zip );
                            personInfo += addressHtml;
                        }
                    }
                }

                PersonSearchResult personSearchResult = new PersonSearchResult();
                personSearchResult.Name = lastFirst ? person.FullNameLastFirst : person.FullName;
                if ( person.PersonStatusValueId != null )
                {
                    personSearchResult.IsActive = person.PersonStatusValue.Guid.Equals( SystemGuid.DefinedValue.PERSON_RECORD_STATUS_ACTIVE );
                }
                else
                {
                    personSearchResult.IsActive = false;
                }

                personSearchResult.Id = person.Id;
                personSearchResult.PickerItemDetailsHtml = string.Format( itemDetailFormat, imageHtml, personInfo );
                searchResult.Add( personSearchResult );
            }

            return searchResult.AsQueryable();
        }

        /// <summary>
        /// Gets the name of the by user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        [HttpGet]
        public Person GetByUserName( string username )
        {
            int? personId = new UserLoginService().Queryable().Where( u => u.UserName.Equals( username ) ).Select( a => a.Id ).FirstOrDefault();
            if ( personId != null )
            {
                return this.Get(personId.Value);
            }

            throw new HttpResponseException( System.Net.HttpStatusCode.NotFound );
        }

        /// <summary>
        /// Gets the popup html for the selected person
        /// </summary>
        /// <param name="personId">The person id.</param>
        /// <returns></returns>
        [HttpGet]
        public PersonSearchResult GetPopupHtml( int personId )
        {
            var result = new PersonSearchResult();
            result.Id = personId;
            result.PickerItemDetailsHtml = "No Details Available";

            var html = new StringBuilder();
            var person = new PersonService().Get( personId );
            if ( person != null )
            {
                var appPath = System.Web.VirtualPathUtility.ToAbsolute( "~" );
                string imageUrlFormat = Path.Combine( appPath, "Image.ashx?id={0}&width=37&height=37" );
                string imageNoPhoto = Path.Combine( appPath, "Assets/images/person-no-photo.jpg" );
                html.AppendFormat( "<header><img src='{0}'/> <div>{1}<small>{2}</small></div></header>",
                    person.PhotoId.HasValue ? string.Format( imageUrlFormat, person.PhotoId.Value ) : imageNoPhoto,
                    person.FullName,
                    person.PersonStatusValueId.HasValue ? person.PersonStatusValue.Name : string.Empty );

                var spouse = person.GetSpouse();
                if (spouse != null)
                {
                    html.AppendFormat("<br/><strong>Spouse</strong> {0}", 
                        spouse.LastName == person.LastName ? spouse.FirstName : spouse.FullName);
                }

                int? age = person.Age;
                if (age.HasValue)
                {
                    html.AppendFormat("<br/><strong>Age</strong> {0}", age); 
                }

                if (!string.IsNullOrWhiteSpace(person.Email))
                {
                    html.AppendFormat("<br/><strong>Email</strong> <a href='mailto:{0}'>{0}</a>", person.Email); 
                }

                foreach(var phoneNumber in person.PhoneNumbers.Where( n => n.IsUnlisted == false).OrderBy( n => n.NumberTypeValue.Order))
                {
                    html.AppendFormat("<br/><strong>{0}</strong> {1}", phoneNumber.NumberTypeValue.Name, phoneNumber.NumberFormatted);
                }

                // TODO: Should also show area: <br /><strong>Area</strong> Westwing

                result.PickerItemDetailsHtml = html.ToString();
            }

            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PersonSearchResult
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name last first.
        /// </summary>
        /// <value>
        /// The full name last first.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the picker item details HTML.
        /// </summary>
        /// <value>
        /// The picker item details HTML.
        /// </value>
        public string PickerItemDetailsHtml { get; set; }
    }
}
