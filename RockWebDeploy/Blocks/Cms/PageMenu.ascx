﻿<%@ control language="C#" autoeventwireup="true" inherits="RockWeb.Blocks.Cms.PageMenu, RockWeb" %>
<asp:UpdatePanel ID="upContent" runat="server">
<ContentTemplate>
    <asp:PlaceHolder ID="phContent" runat="server"></asp:PlaceHolder>
</ContentTemplate>
</asp:UpdatePanel>
