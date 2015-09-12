﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddFamily.ascx.cs" Inherits="RockWeb.Blocks.Crm.PersonDetail.AddFamily" %>

<asp:UpdatePanel ID="upAddFamily" runat="server">
    <ContentTemplate>

        <div class="panel panel-block">
            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-plus-square-o"></i>
                    <asp:Literal ID="lTitle" runat="server"></asp:Literal></h1>
            </div>
            <div class="panel-body">

                <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

                <asp:Panel ID="pnlFamilyData" runat="server">

                    <div class="row">
                        <div class="col-md-12">
                            <h4>Family Members</h4>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <Rock:NewFamilyMembers ID="nfmMembers" runat="server" OnAddFamilyMemberClick="nfmMembers_AddFamilyMemberClick" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            <Rock:CampusPicker ID="cpCampus" runat="server" Required="true" />
                            <Rock:RockDropDownList ID="ddlMaritalStatus" runat="server" Label="Marital Status of Adults" 
                                Help="The marital status to use for the adults in this family." />
                        </div>

                        <div class="col-md-8">
                            <Rock:AddressControl ID="acAddress" runat="server" UseStateAbbreviation="false" UseCountryAbbreviation="false" />
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="pnlContactInfo" runat="server" Visible="false">
                    <Rock:NewFamilyContactInfo ID="nfciContactInfo" runat="server" />
                </asp:Panel>

                <asp:Panel ID="pnlAttributes" runat="server" Visible="false">
                </asp:Panel>

                <asp:Panel ID="pnlDuplicateWarning" runat="server" Visible="false">
                    <Rock:NotificationBox ID="nbDuplicateWarning" runat="server" NotificationBoxType="Warning" Title="Possible Duplicates!"
                        Text="<p>One ore more of the people you are adding may already exist! Please confirm that none of the existing people below are the same person as someone that you are adding." />
                    <asp:PlaceHolder ID="phDuplicates" runat="server" />
                </asp:Panel>

                <div class="actions">
                    <asp:LinkButton ID="btnPrevious" runat="server" Text="Previous" CssClass="btn btn-link" OnClick="btnPrevious_Click" Visible="false" CausesValidation="false" />
                    <asp:LinkButton ID="btnNext" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="btnNext_Click" />
                </div>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
