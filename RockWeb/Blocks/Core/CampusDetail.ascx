﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CampusDetail.ascx.cs" Inherits="RockWeb.Blocks.Core.CampusDetail" %>

<asp:UpdatePanel ID="upCampusDetail" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server">
            
            <asp:HiddenField ID="hfCampusId" runat="server" />

            <div class="banner">
                <h1><asp:Literal ID="lActionTitle" runat="server" /></h1>
            </div>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />
            <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />

            <fieldset>

                <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                <div class="row">
                    <div class="col-md-6">
                        <Rock:DataTextBox ID="tbCampusName" runat="server" SourceTypeName="Rock.Model.Campus, Rock" PropertyName="Name" />
                        <Rock:DataTextBox ID="tbCampusCode" runat="server" SourceTypeName="Rock.Model.Campus, Rock" PropertyName="ShortCode" />
                        <Rock:PersonPicker ID="ppCampusLeader" runat="server" Label="Campus Leader" />
                    </div>
                    <div class="col-md-6">
                        <Rock:DataTextBox ID="tbPhoneNumber" runat="server" SourceTypeName="Rock.Model.Campus, Rock" PropertyName="PhoneNumber" />
                        <Rock:LocationPicker ID="lpAddress" runat="server" AllowedPickerModes="Address" Label="Campus Address" />
                        <Rock:KeyValueList ID="kvlServiceTimes" runat="server" label="Service Times" KeyPrompt="Day" ValuePrompt="Time" Help="A list of days and times that this campus has services." />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="attributes">
                            <asp:PlaceHolder ID="phAttributes" runat="server" EnableViewState="false"></asp:PlaceHolder>
                        </div>
                    </div>
                </div>

            </fieldset>

            <div class="actions">
                <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
            </div>

        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
