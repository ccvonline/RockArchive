﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PageRouteDetail.ascx.cs" Inherits="RockWeb.Blocks.Administration.PageRouteDetail" %>

<asp:UpdatePanel ID="upPageRoutes" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server" Visible="false">

            <asp:HiddenField ID="hfPageRouteId" runat="server" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-error" />

            <fieldset>
                <legend>
                    <asp:Literal ID="lActionTitle" runat="server" />
                </legend>

                <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                <Rock:DataDropDownList ID="ddlPageName" runat="server" DataTextField="DropDownListText" DataValueField="Id" SourceTypeName="Rock.Model.Page, Rock" PropertyName="Title" LabelText="Page Title" />
                <Rock:DataTextBox ID="tbRoute" runat="server" SourceTypeName="Rock.Model.PageRoute, Rock" PropertyName="Route" />
            </fieldset>

            <div class="actions">
                <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="false" OnClick="btnCancel_Click" />
            </div>

        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>


