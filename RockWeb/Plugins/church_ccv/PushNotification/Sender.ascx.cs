
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using Rock.Model;
using Rock.Web.UI;
using church.ccv.CCVCore.PushNotification.Util;
using Rock.Web.Cache;
using System.Linq;
using Rock;

namespace RockWeb.Plugins.church_ccv.PushNotification
{
    [DisplayName( "Sender" )]
    [Category( "CCV > Push Notification" )]
    [Description( "Lets you send a Push Notification." )]
    public partial class Sender: RockBlock
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
            }
        }
                
        protected void btnSendKnown_Click( object sender, EventArgs e )
        {
            var personIds = PushNotificationService.GetKnownDevicePersonIds();

            Dictionary<string, string> metaData = new Dictionary<string, string>();
            metaData.Add( tbAction.Text, tbActionData.Text );

            // send it
            PushNotificationService.SendPushNotification( tbTitle.Text, tbMessage.Text, personIds.ToList(), metaData );
        }

        protected void btnSendAll_Click( object sender, EventArgs e )
        {
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            metaData.Add( tbAction.Text, tbActionData.Text );

            // send it
            PushNotificationService.SendPushNotification( tbTitle.Text, tbMessage.Text, metaData );
        }
    }
}