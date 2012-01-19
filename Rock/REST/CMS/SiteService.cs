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

namespace Rock.REST.CMS
{
	/// <summary>
	/// REST WCF service for Sites
	/// </summary>
    [Export(typeof(IService))]
    [ExportMetadata("RouteName", "CMS/Site")]
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    public partial class SiteService : ISiteService, IService
    {
		/// <summary>
		/// Gets a Site object
		/// </summary>
		[WebGet( UriTemplate = "{id}" )]
        public Rock.CMS.DTO.Site Get( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using (Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope())
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.CMS.SiteService SiteService = new Rock.CMS.SiteService();
				Rock.CMS.Site Site = SiteService.Get( int.Parse( id ) );
				if ( Site.Authorized( "View", currentUser ) )
					return Site.DataTransferObject;
				else
					throw new WebFaultException<string>( "Not Authorized to View this Site", System.Net.HttpStatusCode.Forbidden );
            }
        }
		
		/// <summary>
		/// Gets a Site object
		/// </summary>
		[WebGet( UriTemplate = "{id}/{apiKey}" )]
        public Rock.CMS.DTO.Site ApiGet( string id, string apiKey )
        {
            using (Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope())
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.CMS.SiteService SiteService = new Rock.CMS.SiteService();
					Rock.CMS.Site Site = SiteService.Get( int.Parse( id ) );
					if ( Site.Authorized( "View", user.Username ) )
						return Site.DataTransferObject;
					else
						throw new WebFaultException<string>( "Not Authorized to View this Site", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }
		
		/// <summary>
		/// Updates a Site object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdateSite( string id, Rock.CMS.DTO.Site Site )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.CMS.SiteService SiteService = new Rock.CMS.SiteService();
				Rock.CMS.Site existingSite = SiteService.Get( int.Parse( id ) );
				if ( existingSite.Authorized( "Edit", currentUser ) )
				{
					uow.objectContext.Entry(existingSite).CurrentValues.SetValues(Site);
					
					if (existingSite.IsValid)
						SiteService.Save( existingSite, currentUser.PersonId() );
					else
						throw new WebFaultException<string>( existingSite.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
				}
				else
					throw new WebFaultException<string>( "Not Authorized to Edit this Site", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Updates a Site object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}/{apiKey}" )]
        public void ApiUpdateSite( string id, string apiKey, Rock.CMS.DTO.Site Site )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.CMS.SiteService SiteService = new Rock.CMS.SiteService();
					Rock.CMS.Site existingSite = SiteService.Get( int.Parse( id ) );
					if ( existingSite.Authorized( "Edit", user.Username ) )
					{
						uow.objectContext.Entry(existingSite).CurrentValues.SetValues(Site);
					
						if (existingSite.IsValid)
							SiteService.Save( existingSite, user.PersonId );
						else
							throw new WebFaultException<string>( existingSite.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
					}
					else
						throw new WebFaultException<string>( "Not Authorized to Edit this Site", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Creates a new Site object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreateSite( Rock.CMS.DTO.Site Site )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.CMS.SiteService SiteService = new Rock.CMS.SiteService();
				Rock.CMS.Site existingSite = new Rock.CMS.Site();
				SiteService.Add( existingSite, currentUser.PersonId() );
				uow.objectContext.Entry(existingSite).CurrentValues.SetValues(Site);

				if (existingSite.IsValid)
					SiteService.Save( existingSite, currentUser.PersonId() );
				else
					throw new WebFaultException<string>( existingSite.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
            }
        }

		/// <summary>
		/// Creates a new Site object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "{apiKey}" )]
        public void ApiCreateSite( string apiKey, Rock.CMS.DTO.Site Site )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.CMS.SiteService SiteService = new Rock.CMS.SiteService();
					Rock.CMS.Site existingSite = new Rock.CMS.Site();
					SiteService.Add( existingSite, user.PersonId );
					uow.objectContext.Entry(existingSite).CurrentValues.SetValues(Site);

					if (existingSite.IsValid)
						SiteService.Save( existingSite, user.PersonId );
					else
						throw new WebFaultException<string>( existingSite.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Deletes a Site object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeleteSite( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.CMS.SiteService SiteService = new Rock.CMS.SiteService();
				Rock.CMS.Site Site = SiteService.Get( int.Parse( id ) );
				if ( Site.Authorized( "Edit", currentUser ) )
				{
					SiteService.Delete( Site, currentUser.PersonId() );
					SiteService.Save( Site, currentUser.PersonId() );
				}
				else
					throw new WebFaultException<string>( "Not Authorized to Edit this Site", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Deletes a Site object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}/{apiKey}" )]
        public void ApiDeleteSite( string id, string apiKey )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.CMS.SiteService SiteService = new Rock.CMS.SiteService();
					Rock.CMS.Site Site = SiteService.Get( int.Parse( id ) );
					if ( Site.Authorized( "Edit", user.Username ) )
					{
						SiteService.Delete( Site, user.PersonId );
						SiteService.Save( Site, user.PersonId );
					}
					else
						throw new WebFaultException<string>( "Not Authorized to Edit this Site", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

    }
}
