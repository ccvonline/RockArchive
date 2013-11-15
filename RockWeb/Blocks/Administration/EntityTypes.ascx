﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EntityTypes.ascx.cs" Inherits="EntityTypes" %>

<asp:UpdatePanel ID="upMarketingCampaigns" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlList" runat="server">
            <Rock:ModalAlert ID="mdGridWarning" runat="server" />
            <Rock:Grid ID="gEntityTypes" runat="server" AllowSorting="true">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Entity Type" SortExpression="Name" />
                    <asp:BoundField DataField="FriendlyName" HeaderText="Friendly Name" SortExpression="FriendlyName" />
                    <Rock:BoolField DataField="IsCommon" HeaderText="Common" SortExpression="IsCommon" />
                    <asp:TemplateField>
                        <HeaderStyle CssClass="span1" />
                        <ItemStyle HorizontalAlign="Center"/>
                        <ItemTemplate>
                            <a id="aSecure" runat="server" class="btn btn-security btn-sm" height="500px"><i class="icon-lock"></i></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Rock:Grid>
        </asp:Panel>

        <asp:Panel ID="pnlDetails" runat="server" Visible="false" CssClass="panel panel-default">

            <div class="panel-body">
                <asp:HiddenField ID="hfEntityTypeId" runat="server" />
                
                <div class="banner"><h1><asp:Literal ID="lActionTitle" runat="server" /></h1></div>
                
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

                <fieldset id="fieldsetEditDetails" runat="server">
                    <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="Rock.Model.EntityType, Rock" PropertyName="Name" Label="Entity Type Name" />
                    <Rock:DataTextBox ID="tbFriendlyName" runat="server" SourceTypeName="Rock.Model.EntityType, Rock" PropertyName="FriendlyName" Label="Friendly Name" />
                    <Rock:RockCheckBox ID="cbCommon" runat="server" Text="Common" Help="There are various places that a user is prompted for an entity type.  'Common' entities will be listed first for the user to easily find them" />
                </fieldset>

                <div class="actions" id="pnlEditDetailsActions" runat="server">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>
            </div>
        </asp:Panel>

        <Rock:NotificationBox ID="nbWarning" runat="server" Title="Warning" NotificationBoxType="Warning" Visible="false" />

    </ContentTemplate>
</asp:UpdatePanel>