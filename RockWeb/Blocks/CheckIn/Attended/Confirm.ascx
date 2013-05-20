﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Confirm.ascx.cs" Inherits="RockWeb.Blocks.CheckIn.Attended.Confirm" %>

<asp:UpdatePanel ID="upContent" runat="server">
<ContentTemplate>

    <Rock:ModalAlert ID="maWarning" runat="server" />

    <div class="row-fluid attended-checkin-header">
        <div class="span3 attended-checkin-actions">
            <asp:LinkButton ID="lbBack" CssClass="btn btn-primary btn-large" runat="server" OnClick="lbBack_Click" Text="Back"/>
        </div>

        <div class="span6">
            <h1>Confirm</h1>
        </div>

        <div class="span3 attended-checkin-actions">
            <asp:LinkButton ID="lbDone" CssClass="btn btn-primary btn-large last" runat="server" OnClick="lbDone_Click" Text="Done"/>
        </div>
    </div>

    <div class="row-fluid attended-checkin-body">

        <div class="span9">
            <div class="attended-checkin-body-container">
                <Rock:Grid ID="gPersonList" runat="server" AllowSorting="true" OnRowSelected="gPerson_Edit" OnRowDataBound="gPersonList_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Text" />
                        <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" SortExpression="AssignedTo" />
                        <asp:BoundField DataField="Room" HeaderText="Room" SortExpression="Room" />
                        <asp:BoundField DataField="Time" HeaderText="Time" SortExpression="Time" />
                        <Rock:EditValueField OnClick="gPerson_Edit" />
                        <Rock:DeleteField OnClick="gPerson_Delete" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnPrint" runat="server" CommandName="Print" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CssClass="icon-print" />
                            </ItemTemplate> 
                        </asp:TemplateField>
                    </Columns>
                </Rock:Grid>
            </div>
        </div>

        <div class="span3">
            <div class="attended-checkin-body-container">
                <asp:LinkButton ID="lbPrintAll" CssClass="btn btn-primary btn-large btn-block btn-checkin-select" runat="server" OnClick="lbPrintAll_Click" Text="Print All"/>
            </div>
        </div>

    </div>

</ContentTemplate>
</asp:UpdatePanel>