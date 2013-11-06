﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BlockTypeList.ascx.cs" Inherits="RockWeb.Blocks.Administration.BlockTypeList" %>

<asp:UpdatePanel ID="upPanel" runat="server">
    <ContentTemplate>
        <Rock:ModalAlert ID="mdGridWarning" runat="server" />
        <Rock:GridFilter ID="rFilter" runat="server">
            <Rock:DataTextBox ID="tbNameFilter" runat="server" SourceTypeName="Rock.Model.BlockType, Rock" PropertyName="Name" Required="false" CausesValidation="false" Label="Name contains" />
            <Rock:DataTextBox ID="tbPathFilter" runat="server" SourceTypeName="Rock.Model.BlockType, Rock" PropertyName="Path" Required="false" CausesValidation="false" Label="Path contains" />
            <Rock:RockCheckBox ID="cbExcludeSystem" runat="server" Label="Exclude 'System' types?" />
        </Rock:GridFilter>
        <Rock:Grid ID="gBlockTypes" runat="server" AllowSorting="true" OnRowDataBound="gBlockTypes_RowDataBound" OnRowSelected="gBlockTypes_Edit" TooltipField="Description">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField HeaderText="Path" DataField="Path" SortExpression="Path" />
                <Rock:BadgeField HeaderText="Usage" DataField="BlocksCount" SortExpression="BlocksCount"
                    ImportantMin="0" ImportantMax="0" InfoMin="1" InfoMax="1" SuccessMin="2" />
                <asp:BoundField HeaderText="Status" SortExpression="Status" />
                <Rock:BoolField DataField="IsSystem" HeaderText="System" SortExpression="IsSystem" />
                <Rock:DeleteField OnClick="gBlockTypes_Delete" />
            </Columns>
        </Rock:Grid>
    </ContentTemplate>
</asp:UpdatePanel>

