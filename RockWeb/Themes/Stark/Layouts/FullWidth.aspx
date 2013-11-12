﻿<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="Site.Master" 
    AutoEventWireup="true" Inherits="Rock.Web.UI.RockPage" %>

<asp:Content ID="ctMain" ContentPlaceHolderID="main" runat="server">
    
    <!-- Page Header -->
    <header>
        
        <!-- Brand Bar -->
        <nav class="navbar navbar-default navbar-static-top">
            <div class="container">
			    <div class="navbar-header">
                    <button class="navbar-toggle" type="button" data-toggle="collapse" data-target=".pageheader-collapse">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="#">Stark Theme</a>
			    </div>	
                <div class="navbar-collapse collapse">   
                    <!-- Main Navigation -->
                    <Rock:Zone ID="Navigation" runat="server" />
			    </div>	
            </div>
        </nav>

    </header>
		
	<main>
        
        <!-- Start Content Area -->
        
        <!-- Page Title -->
        <Rock:PageIcon ID="PageIcon" runat="server" /> <h1><Rock:PageTitle ID="PageTitle" runat="server" /></h1>
        
        <!-- Breadcrumbs -->    
        <Rock:PageBreadCrumbs ID="PageBreadCrumbs" runat="server" />

        <!-- Ajax Error -->
        <div class="alert alert-danger ajax-error" style="display:none">
            <p><strong>Error</strong></p>
            <span class="ajax-error-message"></span>
        </div>

        <div class="row">
            <div class="col-md-12">
                <Rock:Zone ID="Feature" runat="server" />
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <Rock:Zone ID="Main" runat="server" />
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <Rock:Zone ID="SectionA" runat="server" />
            </div>
        </div>

        <div class="row">
            <div class="col-md-4">
                <Rock:Zone ID="SectionB" runat="server" />
            </div>
            <div class="col-md-4">
                <Rock:Zone ID="SectionC" runat="server" />
            </div>
            <div class="col-md-4">
                <Rock:Zone ID="SectionD" runat="server" />
            </div>
        </div>

        <!-- End Content Area -->

	</main>
		
	<footer class="page-footer">
		<div class="row">
			<div class="col-md-12">
				<Rock:Zone ID="Footer" runat="server" />
			</div>
		</div>
	</footer>
        
</asp:Content>

