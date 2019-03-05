﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CCVMinistrySafe.ascx.cs" Inherits="RockWeb.Plugins.church_ccv.SafetySecurity.CCVMinistrySafe" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlView" runat="server" CssClass="panel panel-block">

            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-cloud-upload"></i>&nbsp;Ministry Safe Import</h1>
            </div>
            <div class="panel-body">

                <%-- Start Panel --%>
                <asp:Panel ID="pnlStart" runat="server">
                    <Rock:FileUploader ID="fuImport" runat="server" Label="Import File" OnFileUploaded="fuImport_FileUploaded" />
                </asp:Panel>

                <%-- Done Panel --%>
                <asp:Panel ID="pnlDone" runat="server" Visible="false">
                    <Rock:NotificationBox ID="nbSuccess" runat="server" Title="Success" Text="n records imported" />
                </asp:Panel>

            </div>

        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>