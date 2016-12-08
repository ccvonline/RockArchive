﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CalendarDimensionSettings.ascx.cs" Inherits="RockWeb.Blocks.Reporting.CalendarDimensionSettings" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlView" runat="server" CssClass="panel panel-block">

            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-calendar"></i>&nbsp;Calendar Dimension Settings</h1>
            </div>
            <div class="panel-body">

                <div class="row">
                    <div class="col-md-6">
                        <Rock:DatePicker ID="dpStartDate" runat="server" Label="Start Date" Required="true" />
                        <Rock:RockDropDownList ID="monthDropDownList" CssClass="input-width-md" runat="server" Label="Fiscal Start Month" Required="true" />
                        <Rock:RockCheckBox ID="cbGivingMonthUseSundayDate" runat="server" Label="Use Sunday Date for Giving Month" />
                    </div>
                    <div class="col-md-6">
                        <Rock:DatePicker ID="dpEndDate" runat="server" Label="End Date" Required="true" />
                    </div>
                </div>

                <div class="actions">
                    <asp:LinkButton ID="btnGenerate" runat="server" AccessKey="s" Text="Generate Dimension" CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                </div>

            </div>

        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
