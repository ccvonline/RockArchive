﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BlockTypes.ascx.cs" Inherits="RockWeb.Blocks.Administration.BlockTypes" %>

<asp:UpdatePanel ID="upPanel" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlList" runat="server">
            <Rock:GridFilter ID="rFilter" runat="server">
                <Rock:DataTextBox ID="tbNameFilter" runat="server" SourceTypeName="Rock.Model.BlockType, Rock" PropertyName="Name" Required="false" CausesValidation="false" LabelText="Name contains" />
                <Rock:DataTextBox ID="tbPathFilter" runat="server" SourceTypeName="Rock.Model.BlockType, Rock" PropertyName="Path" Required="false" CausesValidation="false" LabelText="Path contains" />
                <Rock:LabeledCheckBox ID="cbExcludeSystem" runat="server" LabelText="Exclude 'System' types?" />
            </Rock:GridFilter>
            <Rock:Grid ID="gBlockTypes" runat="server" AllowSorting="true">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField HeaderText="Path" DataField="Path" SortExpression="Path" />
                    <asp:BoundField HeaderText="Description" DataField="Description" SortExpression="Description" />
                    <Rock:BadgeField HeaderText="Usage" DataField="Blocks.Count" SortExpression="Blocks.Count"
                        ImportantMin="0" ImportantMax="0" InfoMin="1" InfoMax="1" SuccessMin="2" />
                    <Rock:BoolField DataField="IsSystem" HeaderText="System" SortExpression="IsSystem" />
                    <Rock:DeleteField OnClick="gBlockTypes_Delete" />
                </Columns>
            </Rock:Grid>
        </asp:Panel>

        <asp:Panel ID="pnlDetails" runat="server" Visible="false">

            <asp:HiddenField ID="hfBlockTypeId" runat="server" />

            <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-error block-message error alert" />

            <fieldset>
                <legend>
                    <asp:Literal ID="lActionTitle" runat="server" />
                </legend>
                <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="Rock.Model.BlockType, Rock" PropertyName="Name" LabelText="Name" />
                <Rock:DataTextBox ID="tbDescription" runat="server" SourceTypeName="Rock.Model.BlockType, Rock" PropertyName="Description" TextMode="MultiLine" Rows="4" />
                <Rock:DataTextBox ID="tbPath" runat="server" SourceTypeName="Rock.Model.BlockType, Rock" PropertyName="Path" CssClass="input-xlarge" />
            </fieldset>

            <div class="actions">
                <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="false" OnClick="btnCancel_Click" />
            </div>

        </asp:Panel>

        <Rock:NotificationBox ID="nbMessage" runat="server" Title="Error" NotificationBoxType="Error" Visible="false" />

    </ContentTemplate>
</asp:UpdatePanel>

