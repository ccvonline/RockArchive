﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TransactionList.ascx.cs" Inherits="RockWeb.Blocks.Finance.TransactionList" %>
<asp:UpdatePanel ID="upTransactions" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlContent" runat="server">

            <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

            <div class="panel panel-block">
                <div class="panel-heading">
                    <h1 class="panel-title"><i class="fa fa-credit-card"></i> <asp:Literal ID="lTitle" runat="server"></asp:Literal></h1>
                </div>
                <div class="panel-body">

                    <Rock:NotificationBox ID="nbClosedWarning" runat="server" NotificationBoxType="Info" Title="Note"
                        Text="This batch has been closed and transactions cannot be edited." Visible="false" Dismissable="true" />

                    <div class="grid grid-panel">
                        <Rock:GridFilter ID="gfTransactions" runat="server">
                            <Rock:DateRangePicker ID="drpDates" runat="server" Label="Date Range" />
                            <Rock:NumberRangeEditor ID="nreAmount" runat="server" Label="Amount Range" NumberType="Double" />
                            <Rock:RockTextBox ID="tbTransactionCode" runat="server" Label="Transaction Code"></Rock:RockTextBox>
                            <Rock:RockDropDownList ID="ddlAccount" runat="server" Label="Account" />
                            <Rock:RockDropDownList ID="ddlTransactionType" runat="server" Label="Transaction Type" />
                            <Rock:RockDropDownList ID="ddlCurrencyType" runat="server" Label="Currency Type" />
                            <Rock:RockDropDownList ID="ddlCreditCardType" runat="server" Label="Credit Card Type" />
                            <Rock:RockDropDownList ID="ddlSourceType" runat="server" Label="Source Type" />
                        </Rock:GridFilter>

                        <Rock:ModalAlert ID="mdGridWarning" runat="server" />

                        <Rock:Grid ID="gTransactions" runat="server" EmptyDataText="No Transactions Found" 
                            RowItemText="Transaction" OnRowSelected="gTransactions_Edit" AllowSorting="true" >
                            <Columns>
                                <Rock:SelectField></Rock:SelectField>
                                <asp:BoundField DataField="AuthorizedPersonAlias.Person.FullName" HeaderText="Person" 
                                    SortExpression="AuthorizedPersonAlias.Person.LastName,AuthorizedPersonAlias.Person.NickName" />
                                <asp:BoundField DataField="TransactionDateTime" HeaderText="Date / Time" SortExpression="TransactionDateTime" />                
                                <asp:BoundField DataField="TotalAmount" HeaderText="Amount" DataFormatString="{0:C}" SortExpression="TotalAmount" />
                                <asp:TemplateField HeaderText="Currency Type" >
                                    <ItemTemplate>
                                        <asp:Literal ID="lCurrencyType" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TransactionCode" HeaderText="Transaction Code" SortExpression="TransactionCode" />                
                                <asp:BoundField DataField="Summary" HeaderText="Summary" SortExpression="Summary" />
                                <Rock:DeleteField OnClick="gTransactions_Delete" Visible="false"/>
                            </Columns>
                        </Rock:Grid>

                        <Rock:NotificationBox ID="nbResult" runat="server" Visible="false" Dismissable="true"></Rock:NotificationBox>

                    </div>

                </div>
            </div>

        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
