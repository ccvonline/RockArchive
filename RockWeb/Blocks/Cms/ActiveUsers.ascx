﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActiveUsers.ascx.cs" Inherits="RockWeb.Blocks.Cms.ActiveUsers" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <h4><asp:Literal ID="lSiteName" runat="server" /></h4>
        <asp:Literal ID="lMessages" runat="server" />

        <asp:Literal ID="lUsers" runat="server" />

    </ContentTemplate>
</asp:UpdatePanel>
