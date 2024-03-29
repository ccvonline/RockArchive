﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VolunteerScreeningDetail.ascx.cs" Inherits="RockWeb.Plugins.church_ccv.SafetySecurity.VolunteerScreeningDetail" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pNewScreening" runat="server" class="panel panel-block">
            <%-- Header with global info --%>
            <div class="panel-heading">
                <div class="row col-sm-4">
                    <h4 class="panel-title">Volunteer Screening</h4>
                    <br />
                </div>
            </div>

            <div class="panel-body">
                <h4 class="panel-title"><strong>Person</strong></h4>

                <div class="row">
                    <div class="col-md-6">
                        <asp:Literal ID="lPersonName" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>

            <%-- Application info --%>
            <div class="panel-body">
                <h4 class="panel-title"><strong>Application</strong></h4>
                
                <div class="row">
                    <div class="col-md-6">
                        <asp:Literal ID="lDateApplicationSent" runat="server"></asp:Literal>
                    </div>
                </div>
                <br>
                <div class="row">
                    <div class="col-md-6">
                        <asp:Literal ID="lDateApplicationCompleted" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <asp:Literal ID="lApplicationWorkflow" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>

             <%-- Application info --%>
            <div class="panel-body">
                <h4 class="panel-title"><strong>Initiated By</strong></h4>
                
                <div class="row">
                    <div class="col-md-6">
                        <asp:Literal ID="lInitiatedBy" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>

            <div class="panel-body"
                <h4 class="panel-title"><strong>Application Type</strong></h4>
                
                <div class="row">
                    <div class="col-md-6">
                        <asp:Literal ID="lApplicationType" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>

            <%-- Background Check info --%>
            <div class="panel-body">
                <h4 class="panel-title"><strong>Background Check</strong></h4>

                <div>
                    <asp:Literal ID="lBGCheck_Link" runat="server"></asp:Literal>
                </div>
                
                <div>
                    Date: <asp:Literal ID="lBGCheck_Date" runat="server">Pending</asp:Literal>
                </div>

                <div>
                    Doc: <asp:Literal ID="lBGCheck_Doc" runat="server">Pending</asp:Literal>
                </div>

                <div>
                    Result: <asp:Literal ID="lBGCheck_Result" runat="server">Pending</asp:Literal>
                </div>
            </div>
        </asp:Panel>
        
        <%-- Character Reference info --%>
        <asp:Panel ID="pCharReferences" runat="server" class="panel panel-block">
            <%-- Header with global info --%>
            <div class="panel-heading">
                <div class="row col-sm-4">
                    <h4 class="panel-title"><strong>Character References</strong></h4>
                    <br />
                </div>
            </div>


            <div class="panel-body">
                <Rock:Grid ID="gCharacterRefs" Title="Character References" runat="server" DisplayType="Light" AllowSorting="true" RowItemText="Result" AllowPaging="false" EmptyDataText="No Character References have been Issued">
                    <Columns>
                        <Rock:RockBoundField DataField="PersonText" HeaderText="Person" SortExpression="PersonText" />
                        <asp:HyperLinkField DataNavigateUrlFields="WorkflowId" DataTextField="WorkflowText" DataNavigateUrlFormatString="~/page/1492?CharacterReferenceWorkflowId={0}" HeaderText="Review" />
                        <Rock:RockBoundField DataField="State" HeaderText="State" SortExpression="State" />
                        <Rock:RockBoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                        <Rock:RockBoundField DataField="LastDateSent" HeaderText="Date Sent" SortExpression="Date Sent" />
                        
                        <asp:HyperLinkField DataNavigateUrlFields="CharacterReferenceWorkflowGuid, ApplicantFirstName, ApplicantLastName, SourceVolunteerScreeningId, ReferenceEmail" DataTextField="ResendReference" 
                                            DataNavigateUrlFormatString="/WorkflowEntry/212?CharacterReferenceWorkflowGuid={0}&ApplicantFirstName={1}&ApplicantLastName={2}&SourceVolunteerScreeningId={3}&ReferenceEmail={4}" 
                                            HeaderText="Resend Reference" />

                        <Rock:DeleteField OnClick="CharacterRef_Delete" />
                    </Columns>
                </Rock:Grid>
            </div>
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
