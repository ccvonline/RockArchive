﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BatchList.ascx.cs" Inherits="RockWeb.Blocks.Finance.Administration.BatchList" %>

<asp:UpdatePanel ID="upFinancialBatch" runat="server">
    <ContentTemplate>

        <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger block-message error alert" />
        <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />

        <asp:Panel ID="pnlBatchList" runat="server">

            <Rock:GridFilter ID="rFBFilter" runat="server">
                <Rock:DatePicker ID="dtBatchDate" runat="server" SourceTypeName="Rock.Model.FinancialBatch, Rock" PropertyName="BatchDate" Label="Date" />
                <Rock:RockTextBox ID="txtTitle" runat="server" Label="Title"></Rock:RockTextBox>
                <Rock:RockDropDownList ID="ddlStatus" runat="server" Label="Status" />
                <Rock:CampusPicker ID="ddlCampus" runat="server" />
            </Rock:GridFilter>

            <Rock:Grid ID="rGridBatch" runat="server" OnRowDataBound="rGridBatch_RowDataBound" ShowConfirmDeleteDialog="true" OnRowSelected="rGridBatch_Edit">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                    <asp:BoundField DataField="Name" HeaderText="Title" SortExpression="Name" />
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <span><%# Eval("BatchStartDateTime") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <Rock:EnumField DataField="Status" HeaderText="Status" SortExpression="Status" />
                    <asp:BoundField DataField="ControlAmount" HeaderText="Control Amount" />
                    <asp:TemplateField HeaderText="Transaction Total">
                        <ItemTemplate>
                            <asp:Literal ID="TransactionTotal" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Variance">
                        <ItemTemplate>
                            <asp:Literal ID="Variance" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Transaction Count">
                        <ItemTemplate>
                            <asp:Literal ID="TransactionCount" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="Campus" HeaderText="Campus" />
                    <Rock:DeleteField OnClick="rGridBatch_Delete" />
                </Columns>
            </Rock:Grid>

        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>





