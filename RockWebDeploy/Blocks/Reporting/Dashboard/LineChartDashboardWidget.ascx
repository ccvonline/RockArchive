﻿<%@ control language="C#" autoeventwireup="true" inherits="RockWeb.Blocks.Reporting.Dashboard.LineChartDashboardWidget, RockWeb" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDashboardTitle" runat="server" CssClass="dashboard-title">
            <asp:Literal runat="server" ID="lDashboardTitle" />
        </asp:Panel>
        <asp:Panel ID="pnlDashboardSubtitle" runat="server" CssClass="dashboard-subtitle">
            <asp:Literal runat="server" ID="lDashboardSubtitle" />
        </asp:Panel>
        <Rock:NotificationBox ID="nbMetricWarning" runat="server" NotificationBoxType="Warning" Text="Please select a metric in the block settings." />
        <Rock:LineChart ID="lcChart" runat="server" />

    </ContentTemplate>
</asp:UpdatePanel>
