﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PageRoutes.ascx.cs" Inherits="RockWeb.Blocks.Administration.PageRoutes" %>

<asp:UpdatePanel ID="upPageRoutes" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlList" runat="server">
            <Rock:Grid ID="gPageRoutes" runat="server" EmptyDataText="No Page Routes Found" AllowSorting="false">
                <Columns>
                    <asp:BoundField DataField="Route" HeaderText="Route" />
                    <asp:BoundField DataField="Page.Name" HeaderText="Page Name" />
                    <Rock:BoolField DataField="IsSystem" HeaderText="System" />
                    <Rock:EditField OnClick="gPageRoutes_Edit" />
                    <Rock:DeleteField OnClick="gPageRoutes_Delete" />
                </Columns>
            </Rock:Grid>
        </asp:Panel>

        <asp:Panel ID="pnlDetails" runat="server" Visible="false">

            <asp:HiddenField ID="hfPageRouteId" runat="server" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="failureNotification" />

            <div class="row-fluid">

                <div class="span6">

                    <fieldset>
                        <legend>Page Route</legend>
                        <Rock:DataDropDownList ID="ddlPageName" runat="server" DataTextField="Name" DataValueField="Id" />
                        <Rock:DataTextBox ID="tbRoute" runat="server" SourceTypeName="Rock.Cms.PageRoute, Rock" PropertyName="Route" />
                    </fieldset>

                </div>

            </div>

            <div class="actions">
                <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn primary" OnClick="btnSave_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn secondary" CausesValidation="false" OnClick="btnCancel_Click" />
            </div>

        </asp:Panel>

        <Rock:NotificationBox ID="nbMessage" runat="server" Title="Error" NotificationBoxType="Error" Visible="false" />

    </ContentTemplate>
</asp:UpdatePanel>


