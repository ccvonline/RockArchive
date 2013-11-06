﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SiteDetail.ascx.cs" Inherits="RockWeb.Blocks.Cms.SiteDetail" %>

<asp:UpdatePanel ID="upSites" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlDetails" runat="server" >

            <asp:HiddenField ID="hfSiteId" runat="server" />

            <div class="banner">
                <h1><asp:Literal ID="lReadOnlyTitle" runat="server" /></h1>
            </div>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" />

            <div id="pnlEditDetails" runat="server">

                <fieldset>

                    <div class="row">
                        <div class="col-md-6">
                            <Rock:DataTextBox ID="tbSiteName" runat="server" SourceTypeName="Rock.Model.Site, Rock" PropertyName="Name" />
                        </div>
                        <div class="col-md-6">
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <Rock:DataTextBox ID="tbDescription" runat="server" SourceTypeName="Rock.Model.Site, Rock" PropertyName="Description" TextMode="MultiLine" Rows="4" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <Rock:DataDropDownList ID="ddlTheme" runat="server" SourceTypeName="Rock.Model.Site, Rock" PropertyName="Theme" Help="The theme that should be used for the site.  Themes contain specific layouts and css styling that controls how a site and it's pages will look" />
                            <Rock:PagePicker ID="ppDefaultPage" runat="server" Label="Default Page" Required="true" PromptForPageRoute="true" Help="The page and route that will be used whenever a specific page or page route is not provided."/>
                            <Rock:PagePicker ID="ppLoginPage" runat="server" Label="Login Page" Required="true" PromptForPageRoute="true" Help="The page that user will be redirected to when they request a page that requires them to login."/>
                            <Rock:PagePicker ID="ppRegistrationPage" runat="server" Label="Registration Page" Required="true" PromptForPageRoute="true" Help="The page that user will be redirected to when they request to register for a group."/>
                            <Rock:DataTextBox ID="tbErrorPage" runat="server" SourceTypeName="Rock.Model.Site, Rock" PropertyName="ErrorPage" Help="The url that user will be redirected to if an error occurs on site" />
                        </div>
                        <div class="col-md-6">
                            <Rock:DataTextBox ID="tbSiteDomains" runat="server" SourceTypeName="Rock.Model.Site, Rock" PropertyName="SiteDomains" TextMode="MultiLine" LabelTextFromPropertyName="false" Label="Domain(s)" Help="A delimited list of domain values that are associated with this site.  These values are used by Rock to load the correct site whenever a specific page or route is not provided in the url. Rock will determine the site to use by finding the first site with a domain value that is contained by the current request's hostname in the url.  It will then display that site's default page" />
                            <Rock:DataTextBox ID="tbFaviconUrl" runat="server" SourceTypeName="Rock.Model.Site, Rock" PropertyName="FaviconUrl" />
                            <Rock:DataTextBox ID="tbAppleTouchIconUrl" runat="server" SourceTypeName="Rock.Model.Site, Rock" PropertyName="AppleTouchIconUrl" />
                            <Rock:DataTextBox ID="tbFacebookAppId" runat="server" SourceTypeName="Rock.Model.Site, Rock" PropertyName="FacebookAppId" />
                            <Rock:DataTextBox ID="tbFacebookAppSecret" runat="server" SourceTypeName="Rock.Model.Site, Rock" PropertyName="FacebookAppSecret" />
                        </div>
                    </div>

                </fieldset>

                <div class="actions">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>

            </div>

            <fieldset id="fieldsetViewDetails" runat="server">

                <p class="description"><asp:Literal ID="lSiteDescription" runat="server"></asp:Literal></p>

                <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                <div class="row">
                    <asp:Literal ID="lblMainDetails" runat="server" />
                </div>

                <div class="actions">
                    <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-sm" CausesValidation="false" OnClick="btnEdit_Click" />
                    <Rock:ModalAlert ID="mdDeleteWarning" runat="server" />
                    <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-action btn-sm" CausesValidation="false" OnClick="btnDelete_Click" />
                </div>

            </fieldset>

        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>

