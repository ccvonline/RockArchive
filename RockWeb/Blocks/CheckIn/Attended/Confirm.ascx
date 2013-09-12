﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Confirm.ascx.cs" Inherits="RockWeb.Blocks.CheckIn.Attended.Confirm" %>

<asp:UpdatePanel ID="upContent" runat="server">
<ContentTemplate>

    <Rock:ModalAlert ID="maWarning" runat="server" />
    <asp:Panel ID="pnlConfirm" runat="server" CssClass="attended">
        <div class="row-fluid checkin-header">
            <div class="span3 checkin-actions">
                <asp:LinkButton ID="lbBack" CssClass="btn btn-large btn-primary" runat="server" OnClick="lbBack_Click" Text="Back"/>
            </div>

            <div class="span6">
                <h1>Confirm</h1>
            </div>

            <div class="span3 checkin-actions">
                <asp:LinkButton ID="lbDone" CssClass="btn btn-large btn-primary" runat="server" OnClick="lbDone_Click" Text="Done"/>
            </div>
        </div>

        <div class="row-fluid checkin-body">

            <div class="span12">
                <div>
                    <Rock:Grid ID="gPersonList" runat="server" AllowSorting="true" AllowPaging="false" ShowActionRow="false" OnRowCommand="gPersonList_RowCommand" DataKeyNames="Id,AssignedTo,LocationId" >
                        <Columns>
                            <asp:BoundField DataField="Id" Visible="false" />
                            <asp:BoundField DataField="Name" HeaderText="Name" />
                            <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" />
                            <asp:BoundField DataField="Time" HeaderText="Time" />
                            <asp:BoundField DataField="LocationId" Visible="false" />
                            <Rock:EditValueField OnClick="gPersonList_Edit" HeaderText="Edit" ControlStyle-CssClass="btn btn-large btn-primary btn-checkin-select" />
                            <Rock:DeleteField OnClick="gPersonList_Delete" HeaderText="Delete" ControlStyle-CssClass="btn btn-large btn-primary btn-checkin-select" />
                            <asp:TemplateField HeaderText="Print">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnPrint" runat="server" CssClass="btn btn-large btn-primary btn-checkin-select" CommandName="Print" Text="Print" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"><i class="icon-print"></i></asp:LinkButton>
                                </ItemTemplate> 
                            </asp:TemplateField>
                        </Columns>
                    </Rock:Grid>
                </div>
            </div>

        </div>
        <div class="row-fluid checkin-body">
            <div class="span9"></div>
            <div class="span3">
                <asp:LinkButton ID="lbPrintAll" CssClass="btn btn-primary btn-large btn-block btn-checkin-select" runat="server" OnClick="lbPrintAll_Click" Text="Print All" />
            </div>
        </div>
    </asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>