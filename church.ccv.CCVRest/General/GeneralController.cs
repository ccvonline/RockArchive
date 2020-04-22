using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Rock;
using Rock.Web.Cache;

namespace church.ccv.CCVRest.General
{
    /// <summary>
    /// A spot to put one-off REST APIs that are needed, but don't necessarily fall into a project specific area.
    /// </summary>
    public partial class GeneralController : Rock.Rest.ApiControllerBase
    {
        [HttpPost]
        [System.Web.Http.Route( "api/General/FormPostToWorkflow" )]
        public System.Net.Http.HttpResponseMessage FormPostToWorkflow( FormDataCollection formData )
        {
            var workflowTypeGuid = formData["WorkflowTypeGuid"].AsGuidOrNull();
            var successRedirectUrl = formData["success-redirect"];
            var errorRedirectUrl = formData["error-redirect"];

            var rockContext = new Rock.Data.RockContext();

            if ( workflowTypeGuid != null )
            {
                var workflowTypeCache = WorkflowTypeCache.Read( workflowTypeGuid.Value );
                var workflow = Rock.Model.Workflow.Activate( workflowTypeCache, "Workflow From REST" );

                // set workflow attributes from querystring
                foreach ( var parm in formData )
                {
                    workflow.SetAttributeValue( parm.Key, parm.Value );
                }

                // save -> run workflow
                List<string> workflowErrors;
                new Rock.Model.WorkflowService( rockContext ).Process( workflow, out workflowErrors );

                if ( !workflowErrors.Any() )
                {
                    if ( !string.IsNullOrWhiteSpace( successRedirectUrl ) )
                    {
                        var response = ControllerContext.Request.CreateResponse( HttpStatusCode.Redirect );
                        response.Headers.Location = new Uri( successRedirectUrl );
                        return response;
                    }
                }
                else
                {
                    if ( !string.IsNullOrWhiteSpace( errorRedirectUrl ) )
                    {
                        var response = ControllerContext.Request.CreateResponse( HttpStatusCode.Redirect );
                        response.Headers.Location = new Uri( errorRedirectUrl );
                        return response;
                    }
                }
            }
            else
            {
                if ( !string.IsNullOrWhiteSpace( errorRedirectUrl ) )
                {
                    var response = ControllerContext.Request.CreateResponse( HttpStatusCode.Redirect );
                    response.Headers.Location = new Uri( errorRedirectUrl );
                    return response;
                }
            }

            return ControllerContext.Request.CreateResponse( HttpStatusCode.NotFound );
        }
    }
}
