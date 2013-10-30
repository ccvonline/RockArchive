﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActivitySelect.ascx.cs" Inherits="RockWeb.Blocks.CheckIn.Attended.ActivitySelect" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Panel ID="pnlContent" runat="server" >

    <Rock:ModalAlert ID="maWarning" runat="server" />
    <asp:HiddenField ID="hfAllergyAttributeId" runat="server" />

    <asp:Panel ID="pnlActivitySelect" runat="server" CssClass="attended" >
        <div class="row checkin-header">
            <div class="col-sm-3 checkin-actions">
                <Rock:BootstrapButton ID="lbBack" CssClass="btn btn-primary btn-lg" runat="server" OnClick="lbBack_Click" Text="Back" EnableViewState="false" />
            </div>

            <div class="col-sm-6">
                <h1><asp:Literal ID="lblPersonName" runat="server" EnableViewState="false" /></h1>
            </div>

            <div class="col-sm-3 checkin-actions">
                <Rock:BootstrapButton ID="lbNext" CssClass="btn btn-primary btn-lg" runat="server" OnClick="lbNext_Click" Text="Next"
                    DataLoadingText="Processing... &lt;i class='icon-spinner icon-spin icon-large'&gt;&lt;/i&gt;" EnableViewState="false" />
            </div>
        </div>
                
        <div class="row checkin-body">

            <asp:UpdatePanel ID="pnlSelectGroupType" runat="server" UpdateMode="Conditional" class="col-md-3">
            <ContentTemplate>        
                
                    <h3>GroupType</h3>
                    <asp:Repeater ID="rGroupType" runat="server" OnItemCommand="rGroupType_ItemCommand" OnItemDataBound="rGroupType_ItemDataBound">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbSelectGroupType" runat="server" CssClass="btn btn-primary btn-lg btn-block btn-checkin-select" CausesValidation="false" />
                        </ItemTemplate>
                    </asp:Repeater>
                
            </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="pnlSelectLocation" runat="server" UpdateMode="Conditional" class="col-md-3">
            <ContentTemplate>        
                
                    <h3>Location</h3>
                    <asp:ListView ID="lvLocation" runat="server" OnPagePropertiesChanging="lvLocation_PagePropertiesChanging" OnItemCommand="lvLocation_ItemCommand" OnItemDataBound="lvLocation_ItemDataBound">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbSelectLocation" runat="server" CssClass="btn btn-primary btn-lg btn-block btn-checkin-select" ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:ListView>
                    <asp:DataPager ID="Pager" runat="server" PageSize="5" PagedControlID="lvLocation">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn btn-primary btn-checkin-select" />
                        </Fields>
                    </asp:DataPager>
                
            </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="pnlSelectSchedule" runat="server" UpdateMode="Conditional" class="col-md-3">
            <ContentTemplate>        
                
                    <h3>Schedule</h3>
                    <asp:Repeater ID="rSchedule" runat="server" OnItemCommand="rSchedule_ItemCommand" OnItemDataBound="rSchedule_ItemDataBound">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbSelectSchedule" runat="server" CssClass="btn btn-primary btn-lg btn-block btn-checkin-select" CausesValidation="false" />
                        </ItemTemplate>
                    </asp:Repeater>
                
            </ContentTemplate>
            </asp:UpdatePanel>

            <div class="col-md-3 selected-grid">
                <h3>Selected</h3>
                <asp:UpdatePanel ID="pnlSelectedGrid" runat="server" UpdateMode="Conditional">
                <ContentTemplate> 
                    <Rock:Grid ID="gSelectedList" runat="server" ShowHeader="false" DataKeyNames="LocationId, ScheduleId" DisplayType="Light">
                    <Columns>
                        <asp:BoundField DataField="Schedule" />
                        <asp:BoundField DataField="ScheduleId" Visible="false" />
                        <asp:BoundField DataField="Location" />
                        <asp:BoundField DataField="LocationId" Visible="false" />                        
                        <Rock:DeleteField OnClick="gSelectedList_Delete" ControlStyle-CssClass="btn btn-lg btn-primary" />
                    </Columns>
                    </Rock:Grid>
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row checkin-footer">
            <div class="col-md-9"></div>
            <div class="col-md-3">
                <asp:LinkButton ID="lbAddNote" runat="server" Text="Add a Note" CssClass="btn btn-primary btn-lg btn-checkin-select" OnClick="lbAddNote_Click" CausesValidation="false" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlAddNote" runat="server" CssClass="attended modal-foreground small-modal" DefaultButton="lbAddNoteSave">
        <div class="checkin-header row">
            <div class="col-sm-3 checkin-actions">
                <asp:LinkButton ID="lbAddNoteCancel" CssClass="btn btn-lg btn-primary" runat="server" OnClick="lbAddNoteCancel_Click" Text="Cancel" CausesValidation="false" EnableViewState="false" />
            </div>
            <div class="col-sm-6">
                <h3>Add Note</h3>
            </div>
            <div class="col-sm-3 checkin-actions">
                <asp:LinkButton ID="lbAddNoteSave" CssClass="btn btn-lg btn-primary" runat="server" OnClick="lbAddNoteSave_Click" Text="Save" EnableViewState="false" />
            </div>
        </div>
		
        <div class="checkin-body">
            <div class="row">
                <fieldset id="fsNotes" runat="server"/>
            </div>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="mpeAddNote" runat="server" BehaviorID="mpeAddNote" TargetControlID="hfOpenNotePanel" PopupControlID="pnlAddNote" 
        CancelControlID="lbAddNoteCancel" BackgroundCssClass="attended modal-background" />
    <asp:HiddenField ID="hfOpenNotePanel" runat="server" />    

</asp:Panel>

<script type="text/javascript">

    function setControlEvents() {

        $find("mpeAddNote").add_shown(function () {
            $find("mpeAddNote")._backgroundElement.onclick = function () {
                $find("mpeAddNote").hide();
            }
        });

    };

    $(document).ready(function () { setControlEvents(); });
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(setControlEvents);

</script>
