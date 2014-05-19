﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LocationDetail.ascx.cs" Inherits="RockWeb.Blocks.Core.LocationDetail" %>

<asp:UpdatePanel ID="upnlLocationList" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlDetails" runat="server">
            <asp:HiddenField ID="hfLocationId" runat="server" />

            <div class="banner">
                <h1>
                    <asp:Literal ID="lReadOnlyTitle" runat="server" />
                </h1>

                <Rock:HighlightLabel ID="hlInactive" runat="server" LabelType="Danger" Text="Inactive" />
                <Rock:HighlightLabel ID="hlType" runat="server" LabelType="Type" />

            </div>

            <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

            <div id="pnlEditDetails" runat="server">

                <div class="row">
                    <div class="col-md-6">
                        <Rock:LocationPicker ID="gpParentLocation" runat="server" Required="false" Label="Parent Location" AllowedPickerModes="Named" />
                        <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="Rock.Model.Location, Rock" PropertyName="Name" />
                        <Rock:RockDropDownList ID="ddlLocationType" runat="server" DataTextField="Name" DataValueField="Id" Label="Location Type" AutoPostBack="true" OnSelectedIndexChanged="ddlLocationType_SelectedIndexChanged"  />
                        <Rock:RockDropDownList ID="ddlPrinter" runat="server" Label="Printer" DataTextField="Name" DataValueField="Id" 
                            Help="The printer that this location should use for printing" />
                        <asp:PlaceHolder ID="phAttributeEdits" runat="server" EnableViewState="false"></asp:PlaceHolder>
                    </div>
                    <div class="col-md-6">
                        <Rock:RockCheckBox ID="cbIsActive" runat="server" Text="Active" />
                        <Rock:LocationAddressPicker ID="locapAddress" runat="server" Label="Address" />
                        <asp:Button ID="btnStandardize" runat="server" OnClick="btnStandardize_Click" Text="Verify Address" CssClass="btn btn-action margin-b-md" />
                        <asp:Literal ID="lStandardizationUpdate" runat="server" />
                        <Rock:RockCheckBox ID="cbGeoPointLocked" runat="server" Label="Point Locked" Text="Yes" Help="Locks the geocoding to keep the location from being re-geocoding in the future." />
                        <Rock:GeoPicker ID="geopPoint" runat="server" DrawingMode="Point" Label="Point" />
                        <Rock:GeoPicker ID="geopFence" runat="server" DrawingMode="Polygon" Label="Geo-fence" />
                    </div>
                </div>

                <div class="actions">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>

            </div>

            <fieldset id="fieldsetViewDetails" runat="server">

                <div class="row">
                    <div class="col-md-6">
                        <asp:Literal ID="lblMainDetails" runat="server" />
                        <asp:PlaceHolder ID="phAttributes" runat="server"></asp:PlaceHolder>
                    </div>
                    <div class="col-md-6 location-maps">
                        <asp:PlaceHolder ID="phMaps" runat="server" />
                    </div>
                </div>

                <div class="actions">
                    <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary" OnClick="btnEdit_Click" CausesValidation="false" />
                    <Rock:ModalAlert ID="mdDeleteWarning" runat="server" />
                    <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-link" OnClick="btnDelete_Click" CausesValidation="false" />
                    <Rock:SecurityButton ID="btnSecurity" runat="server" class="btn btn-sm btn-action pull-right" />
                </div>

            </fieldset>
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
