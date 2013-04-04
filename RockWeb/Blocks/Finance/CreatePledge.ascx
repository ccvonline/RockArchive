﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CreatePledge.ascx.cs" Inherits="RockWeb.Blocks.Finance.CreatePledge" %>

<asp:UpdatePanel ID="upCreatePledge" runat="server">
    <ContentTemplate>
        <fieldset>
            <legend><asp:Literal ID="lLegendText" runat="server"/></legend>
            <Rock:DataTextBox ID="tbFirstName" runat="server" LabelText="First Name" SourceTypeName="Rock.Model.Person, Rock" PropertyName="NickName"/>
            <Rock:DataTextBox ID="tbLastName" runat="server" LabelText="Last Name" SourceTypeName="Rock.Model.Person, Rock" PropertyName="LastName"/>
            <Rock:DataTextBox ID="tbAmount" runat="server" PrependText="$" LabelText="Total Amount" SourceTypeName="Rock.Model.Pledge, Rock" PropertyName="Amount"/>
            <Rock:DataTextBox ID="tbEmail" runat="server" LabelText="Email" TextMode="Email" SourceTypeName="Rock.Model.Person, Rock" PropertyName="Email"/>
            <Rock:DateTimePicker ID="dtpStartDate" runat="server" LabelText="Start Date" SourceTypeName="Rock.Model.Pledge, Rock" PropertyName="StartDate"/>
            <Rock:DateTimePicker ID="dtpEndDate" runat="server" LabelText="End Date" SourceTypeName="Rock.Model.Pledge, Rock" PropertyName="EndDate"/>
            <Rock:DataTextBox ID="tbFrequencyAmount" runat="server" PrependText="$" LabelText="Amount" SourceTypeName="Rock.Model.Pledge, Rock" PropertyName="FrequencyAmount"/>
            <Rock:DataDropDownList ID="ddlFrequencyType" runat="server" SourceTypeName="Rock.Model.Pledge, Rock" PropertyName="FrequencyTypeValue"/>
            <asp:Panel ID="pnlConfirm" runat="server" CssClass="alert alert-info" Visible="False">
                <p><strong>Hey!</strong> You currently have a pledge in the system. Do you want to replace it with this one?</p>
                <div class="actions">
                    <asp:LinkButton ID="btnConfirmYes" runat="server" CssClass="btn btn-success"><i class="icon-ok"></i> Yes</asp:LinkButton>
                    <asp:LinkButton ID="btnConfirmNo" runat="server" CssClass="btn"><i class="icon-remove"></i> No</asp:LinkButton>
                </div>
            </asp:Panel>
        </fieldset>
        <div class="actions">
            <asp:Button ID="btnSave" runat="server" Text="Save Pledge" OnClick="btnSave_Click" CssClass="btn"/>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>