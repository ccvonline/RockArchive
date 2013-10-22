﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BlockTypeDetail.ascx.cs" Inherits="RockWeb.Blocks.Administration.BlockTypeDetail" %>

<asp:UpdatePanel ID="upPanel" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server" Visible="false">

            <asp:HiddenField ID="hfBlockTypeId" runat="server" />

            <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger block-message error alert" />
            <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />

            <fieldset>
                <legend>
                    <asp:Literal ID="lActionTitle" runat="server" />
                </legend>

                <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="Rock.Model.BlockType, Rock" PropertyName="Name" Label="Name" />
                <Rock:DataTextBox ID="tbDescription" runat="server" SourceTypeName="Rock.Model.BlockType, Rock" PropertyName="Description" TextMode="MultiLine" Rows="4" />
                <Rock:DataTextBox ID="tbPath" runat="server" SourceTypeName="Rock.Model.BlockType, Rock" PropertyName="Path" CssClass="input-xlarge" />
                <Rock:RockLiteral ID="lblStatus" runat="server" Label="Status" />
                <Rock:RockBulletedList ID="lstPages" runat="server" Label="Pages that use this block type"/>
            </fieldset>

            <div class="actions">
                <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-default" CausesValidation="false" OnClick="btnCancel_Click" />
            </div>

        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>

