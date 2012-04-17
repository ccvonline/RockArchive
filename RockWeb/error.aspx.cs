﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock.Groups;

public partial class error : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        // get error level
        int errorLevel = 0;

        if (Request["error"] != null)
            errorLevel = Int32.Parse( Request["error"].ToString() );

        if ( errorLevel == 1 )
        {
            // check to see if the user is an admin, if so allow them to view the error details
            Rock.CMS.User user = Rock.CMS.UserRepository.GetCurrentUser();

            GroupRepository service = new GroupRepository();
            Group adminGroup = service.GetByGuid( Rock.SystemGuid.Group.GROUP_ADMINISTRATORS );

            if ( user != null && adminGroup.Members.Where( m => m.PersonId == user.PersonId ).Count() > 0 )
            {
                // is an admin
                lErrorInfo.Text = "<h4>Exception Log:</h4>";

                // get exception from Session
                if ( Session["Exception"] != null )
                    ProcessException( ( Exception )Session["Exception"], " " );
            }
        }

        // clear session object
        Session.Remove( "Exception" );
    }

    private void ProcessException(Exception ex, string exLevel) {

        lErrorInfo.Text += "<a href=\"#\" class=\"exception-type label\">" + exLevel + ex.GetType().Name + " in " + ex.Source + "</a>";
        lErrorInfo.Text += "<div class=\"stack-trace\"><strong>Stack Trace</strong><br>" + ex.StackTrace + "</div>";

        // check for inner exception
        if ( ex.InnerException != null )
        {
            //lErrorInfo.Text += "<p /><p />";
            ProcessException( ex.InnerException, "-" + exLevel  );
        }
    }
}