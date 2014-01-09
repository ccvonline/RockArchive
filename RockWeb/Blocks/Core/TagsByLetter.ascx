﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TagsByLetter.ascx.cs" Inherits="RockWeb.Blocks.Core.TagsByLetter" %>

<asp:UpdatePanel ID="upTagCloud" runat="server">
    <ContentTemplate>

        <div class="nav nav-pills">
            <li class='<%= personalTagsCss %>'><asp:LinkButton id="lbPersonalTags" runat="server" OnClick="lbPersonalTags_Click" Text="Personal Tags" CssClass="active"></asp:LinkButton></li>
            <li class="<%= publicTagsCss %>"><asp:LinkButton id="lbPublicTags" runat="server" OnClick="lbPublicTags_Click" Text="Public Tags"></asp:LinkButton></li>
        </div>

        <asp:Literal ID="lLetters" runat="server"></asp:Literal>

        <asp:Literal ID="lTagList" runat="server"></asp:Literal>

    </ContentTemplate>
</asp:UpdatePanel>

<script>
    // fade-in effect for the panel
    function FadePanelIn() {
        $("[id$='upTagCloud']").rockFadeIn();
    }
    $(document).ready(function () { FadePanelIn(); });
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(FadePanelIn);

</script>
