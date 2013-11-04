﻿<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" Inherits="Rock.Web.UI.RockPage" %>
<!DOCTYPE html> 
<html>
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta charset="utf-8">
    <title></title>
    
    <!--[if lt IE 9]>
        <script src="<%# ResolveUrl("~/Themes/RockChMS/Scripts/html5.js") %>" ></script>
    <![endif]-->

    <!-- Set the viewport width to device width for mobile -->
    <meta name="viewport" content="width=device-width" />

    <!-- Included CSS Files -->
	<link rel="stylesheet" href="<%# ResolveUrl("~/Themes/RockChMS/Styles/theme.css") %>"/>
	<link rel="stylesheet" href="<%# ResolveUrl("~/Styles/developer.css") %>"/>

    <script src="<%# ResolveUrl("~/Scripts/jquery.js") %>" ></script>
    <script src="<%# ResolveUrl("~/Scripts/bootstrap.min.js") %>" ></script>

</head>
<body id="splash">

    <form id="form1" runat="server">

            <div id="content">
                <asp:Image ID="Image1" runat="server" AlternateText="Rock ChMS" ImageUrl="~/Assets/Images/rock-logo.svg" CssClass="pageheader-logo" />
                
                <div id="content-box" class="clearfix">
                    <Rock:Zone ID="Main" runat="server" />
                </div>
            </div>


    </form>
</body>
</html>