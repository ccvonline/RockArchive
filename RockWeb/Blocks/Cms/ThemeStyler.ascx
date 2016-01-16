﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ThemeStyler.ascx.cs" Inherits="RockWeb.Blocks.Cms.ThemeStyler" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlView" runat="server" CssClass="panel panel-block">
        
            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-star"></i> Blank Detail Block</h1>

                <div class="panel-labels">
                    <Rock:HighlightLabel ID="hlblTest" runat="server" LabelType="Info" Text="Label" />
                </div>
            </div>
            <div class="panel-body">

                <Rock:ColorPicker ID="cpColor" runat="server" Value="#000" Label="My Color" />

            </div>
        
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
