﻿<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" Inherits="Rock.Web.UI.RockPage" %>

<asp:Content ID="ctFeature" ContentPlaceHolderID="feature" runat="server">

    <section class="main-feature">
        <div id="section-feature-bg">

            <Rock:Zone Name="Feature" runat="server" />

        </div>
    </section>

</asp:Content>

<asp:Content ID="ctMain" ContentPlaceHolderID="main" runat="server">

	<main>
                <div class="alert alert-danger ajax-error" style="display:none">
                        <p><strong>Error</strong></p>
                        <span class="ajax-error-message"></span>
                </div>

                <div id="section-a-bg">
                        <Rock:Zone Name="Section A" runat="server" />
                </div>

                <div id="section-b-bg">
                        <Rock:Zone Name="Section B" runat="server" />
                </div>

                <div id="section-c-bg">
                        <Rock:Zone Name="Section C" runat="server" />
                </div>

                <div id="section-d-bg">
                        <Rock:Zone Name="Section D" runat="server" />
                </div>

                <div id="section-e-bg">
                        <Rock:Zone Name="Section E" runat="server" />
                </div>

	</main>

</asp:Content>

