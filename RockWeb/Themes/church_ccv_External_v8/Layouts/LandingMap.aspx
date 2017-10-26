<%@ Page Language="C#" AutoEventWireup="true" Inherits="Rock.Web.UI.RockPage" %>

<!DOCTYPE html>

<script runat="server">

    protected override void OnLoad( EventArgs e )
    {
        base.OnLoad( e );

        var campusEntityType = Rock.Web.Cache.EntityTypeCache.Read("Rock.Model.Campus");
        var currentCampus = GetCurrentContext(campusEntityType) as Rock.Model.Campus;

        var homePageRoute = "home";

        if (currentCampus != null)
        {
            Response.Redirect("/"+homePageRoute);
        }
        else
        {
            AddScriptToHead(this.Page, "var CCV = CCV || {}; CCV.homePageRoute ='"+homePageRoute+"';", true);
        }
    }

</script>

<html class="no-js">
<head runat="server">

    <script type="text/javascript">var _sf_startpt=(new Date()).getTime()</script>

    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta charset="utf-8">
    <title></title>

    <script src="<%# ResolveRockUrl("~/Scripts/modernizr.js", true) %>" ></script>
    <script src="<%# ResolveRockUrl("~/Scripts/jquery-1.12.4.min.js", true) %>"></script>

    <script src="//maps.google.com/maps/api/js?v=3"></script>

    <!-- Set the viewport width to device width for mobile -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=no">

    <!-- Included CSS Files -->
    <link rel="stylesheet" href="<%# ResolveRockUrl("~~/Styles/bootstrap.css", true) %>"/>
    <link rel="stylesheet" href="<%# ResolveRockUrl("~~/Styles/landing-map.css", true) %>"/>

    <!-- Icons -->
    <link rel="shortcut icon" href="<%# ResolveRockUrl("~~/Assets/Icons/favicon.ico", true) %>">
    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="<%# ResolveRockUrl("~~/Assets/Icons/touch-icon-ipad-retina.png", true) %>">
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="<%# ResolveRockUrl("~~/Assets/Icons/touch-icon-iphone-retina.png", true) %>">
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="<%# ResolveRockUrl("~~/Assets/Icons/touch-icon-ipad.png", true) %>">
    <link rel="apple-touch-icon-precomposed" href="<%# ResolveRockUrl("~~/Assets/Icons/touch-icon-iphone.png", true) %>">

</head>

<body class="rock-blank">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sManager" runat="server" />

        <asp:UpdateProgress ID="updateProgress" runat="server">
            <ProgressTemplate>
                <div class="updateprogress-status">
                    <div class="spinner">
                        <div class="rect1"></div>
                        <div class="rect2"></div>
                        <div class="rect3"></div>
                        <div class="rect4"></div>
                        <div class="rect5"></div>
                    </div>
                </div>
                <div class="updateprogress-bg modal-backdrop">
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <!-- Start Content Area -->
        <Rock:Zone Name="Main" runat="server" />

    </form>

    <script type="text/javascript">
        var _sf_async_config = { uid: 23413, domain: 'ccv.church', useCanonical: true };
        (function() {
            function loadChartbeat() {
                window._sf_endpt = (new Date()).getTime();
                var e = document.createElement('script');
                e.setAttribute('language', 'javascript');
                e.setAttribute('type', 'text/javascript');
                e.setAttribute('src','//static.chartbeat.com/js/chartbeat.js');
                document.body.appendChild(e);
            };
            var oldonload = window.onload;
            window.onload = (typeof window.onload != 'function') ?
                loadChartbeat : function() { oldonload(); loadChartbeat(); };
        })();
    </script>

</body>
</html>
