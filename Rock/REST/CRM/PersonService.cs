//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace Rock.REST.CRM
{
	/// <summary>
	/// REST WCF service for People
	/// </summary>
    [Export(typeof(IService))]
    [ExportMetadata("RouteName", "CRM/Person")]
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    public partial class PersonService : IPersonService, IService
    {
		/// <summary>
		/// Gets a Person object
		/// </summary>
		[WebGet( UriTemplate = "{id}" )]
        public Rock.CRM.DTO.Person Get( string id )
        {
            var currentUser = Rock.CMS.UserRepository.GetCurrentUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using (Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope())
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.CRM.PersonRepository PersonRepository = new Rock.CRM.PersonRepository();
				Rock.CRM.Person Person = PersonRepository.Get( int.Parse( id ) );
				if ( Person.Authorized( "View", currentUser ) )
					return Person.DataTransferObject;
				else
					throw new WebFaultException<string>( "Not Authorized to View this Person", System.Net.HttpStatusCode.Forbidden );
            }
        }
		
		/// <summary>
		/// Gets a Person object
		/// </summary>
		[WebGet( UriTemplate = "{id}/{apiKey}" )]
        public Rock.CRM.DTO.Person ApiGet( string id, string apiKey )
        {
            using (Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope())
            {
				Rock.CMS.UserRepository userRepository = new Rock.CMS.UserRepository();
                Rock.CMS.User user = userRepository.AsQueryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.CRM.PersonRepository PersonRepository = new Rock.CRM.PersonRepository();
					Rock.CRM.Person Person = PersonRepository.Get( int.Parse( id ) );
					if ( Person.Authorized( "View", user ) )
						return Person.DataTransferObject;
					else
						throw new WebFaultException<string>( "Not Authorized to View this Person", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }
		
		/// <summary>
		/// Updates a Person object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdatePerson( string id, Rock.CRM.DTO.Person Person )
        {
            var currentUser = Rock.CMS.UserRepository.GetCurrentUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.CRM.PersonRepository PersonRepository = new Rock.CRM.PersonRepository();
				Rock.CRM.Person existingPerson = PersonRepository.Get( int.Parse( id ) );
				if ( existingPerson.Authorized( "Edit", currentUser ) )
				{
					uow.objectContext.Entry(existingPerson).CurrentValues.SetValues(Person);
					
					if (existingPerson.IsValid)
						PersonRepository.Save( existingPerson, currentUser.PersonId );
					else
						throw new WebFaultException<string>( existingPerson.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
				}
				else
					throw new WebFaultException<string>( "Not Authorized to Edit this Person", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Updates a Person object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}/{apiKey}" )]
        public void ApiUpdatePerson( string id, string apiKey, Rock.CRM.DTO.Person Person )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserRepository userRepository = new Rock.CMS.UserRepository();
                Rock.CMS.User user = userRepository.AsQueryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.CRM.PersonRepository PersonRepository = new Rock.CRM.PersonRepository();
					Rock.CRM.Person existingPerson = PersonRepository.Get( int.Parse( id ) );
					if ( existingPerson.Authorized( "Edit", user ) )
					{
						uow.objectContext.Entry(existingPerson).CurrentValues.SetValues(Person);
					
						if (existingPerson.IsValid)
							PersonRepository.Save( existingPerson, user.PersonId );
						else
							throw new WebFaultException<string>( existingPerson.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
					}
					else
						throw new WebFaultException<string>( "Not Authorized to Edit this Person", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Creates a new Person object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreatePerson( Rock.CRM.DTO.Person Person )
        {
            var currentUser = Rock.CMS.UserRepository.GetCurrentUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.CRM.PersonRepository PersonRepository = new Rock.CRM.PersonRepository();
				Rock.CRM.Person existingPerson = new Rock.CRM.Person();
				PersonRepository.Add( existingPerson, currentUser.PersonId );
				uow.objectContext.Entry(existingPerson).CurrentValues.SetValues(Person);

				if (existingPerson.IsValid)
					PersonRepository.Save( existingPerson, currentUser.PersonId );
				else
					throw new WebFaultException<string>( existingPerson.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
            }
        }

		/// <summary>
		/// Creates a new Person object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "{apiKey}" )]
        public void ApiCreatePerson( string apiKey, Rock.CRM.DTO.Person Person )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserRepository userRepository = new Rock.CMS.UserRepository();
                Rock.CMS.User user = userRepository.AsQueryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.CRM.PersonRepository PersonRepository = new Rock.CRM.PersonRepository();
					Rock.CRM.Person existingPerson = new Rock.CRM.Person();
					PersonRepository.Add( existingPerson, user.PersonId );
					uow.objectContext.Entry(existingPerson).CurrentValues.SetValues(Person);

					if (existingPerson.IsValid)
						PersonRepository.Save( existingPerson, user.PersonId );
					else
						throw new WebFaultException<string>( existingPerson.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Deletes a Person object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeletePerson( string id )
        {
            var currentUser = Rock.CMS.UserRepository.GetCurrentUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.CRM.PersonRepository PersonRepository = new Rock.CRM.PersonRepository();
				Rock.CRM.Person Person = PersonRepository.Get( int.Parse( id ) );
				if ( Person.Authorized( "Edit", currentUser ) )
				{
					PersonRepository.Delete( Person, currentUser.PersonId );
					PersonRepository.Save( Person, currentUser.PersonId );
				}
				else
					throw new WebFaultException<string>( "Not Authorized to Edit this Person", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Deletes a Person object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}/{apiKey}" )]
        public void ApiDeletePerson( string id, string apiKey )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserRepository userRepository = new Rock.CMS.UserRepository();
                Rock.CMS.User user = userRepository.AsQueryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.CRM.PersonRepository PersonRepository = new Rock.CRM.PersonRepository();
					Rock.CRM.Person Person = PersonRepository.Get( int.Parse( id ) );
					if ( Person.Authorized( "Edit", user ) )
					{
						PersonRepository.Delete( Person, user.PersonId );
						PersonRepository.Save( Person, user.PersonId );
					}
					else
						throw new WebFaultException<string>( "Not Authorized to Edit this Person", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

    }
}
