﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FamilySelect.ascx.cs" Inherits="RockWeb.Blocks.CheckIn.Attended.FamilySelect" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<script type="text/javascript">

    function cvBirthDateValidator_ClientValidate(sender, args) {
        args.IsValid = false;
        var isValidDate = args.Value.match(/^(\d{1,2})[- /.](\d{1,2})[- /.](\d{2,4})$/);
        if ( isValidDate ) {
            args.IsValid = true;
            return;
        }
    };

    //function parseDate(str) {
    //    return str.match(/^(\d{1,2})[- /.](\d{1,2})[- /.](\d{2,4})$/);
    //};
       
    function setControlEvents() {

        // give user instant feedback, server side object needs to set class though
        $('.family').unbind('click').on('click', function () {
            $('.family').removeClass('active');
            $(this).toggleClass('active');
        });

        $('.person').unbind('click').on('click', function () {
            $(this).toggleClass('active');
            var selectedIds = $('#hfSelectedPerson').val();
            var buttonId = this.getAttribute('data-id') + ',';
            if (typeof selectedIds == "string" && (selectedIds.indexOf(buttonId) >= 0)) {
                $('#hfSelectedPerson').val(selectedIds.replace(buttonId, ''));
            } else {
                $('#hfSelectedPerson').val(buttonId + selectedIds);
            }
            return false;
        });

        $('.visitor').unbind('click').on('click', function () {
            $(this).toggleClass('active');
            var selectedIds = $('#hfSelectedVisitor').val();
            var buttonId = this.getAttribute('data-id') + ',';
            if (typeof selectedIds == "string" && (selectedIds.indexOf(buttonId) >= 0)) {
                $('#hfSelectedVisitor').val(selectedIds.replace(buttonId, ''));
            } else {
                $('#hfSelectedVisitor').val(buttonId + selectedIds);
            }
            return false;
        });

    };
    $(document).ready(function () { setControlEvents(); });
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(setControlEvents);

</script>

<asp:UpdatePanel ID="upContent" runat="server" UpdateMode="Conditional">
<ContentTemplate>

    <Rock:ModalAlert ID="maWarning" runat="server" />
    <asp:HiddenField ID="personVisitorType" runat="server" />
    <asp:HiddenField ID="hfSelectedPerson" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfSelectedVisitor" runat="server" ClientIDMode="Static" />
        
    <asp:Panel ID="pnlFamilySelect" runat="server">

        <div class="row-fluid attended-checkin-header">
            <div class="span3 attended-checkin-actions">
                <asp:LinkButton ID="lbBack" CssClass="btn btn-large btn-primary" runat="server" OnClick="lbBack_Click" Text="Back" CausesValidation="false"/>
            </div>

            <div class="span6">
                <h1 id="familyTitle" runat="server">Search Results</h1>
            </div>

            <div class="span3 attended-checkin-actions">
                <asp:LinkButton ID="lbNext" CssClass="btn btn-large last btn-primary" runat="server" OnClick="lbNext_Click" Text="Next"/>
            </div>
        </div>
                
        <div class="row-fluid attended-checkin-body">
            
            <asp:UpdatePanel ID="pnlUpdateFamily" runat="server" UpdateMode="Conditional" class="span3 family-div">
            <ContentTemplate>
                
                <div class="attended-checkin-body-container">
                    <h3>Families</h3>
                    <asp:ListView ID="lvFamily" runat="server" OnPagePropertiesChanging="lvFamily_PagePropertiesChanging" OnItemCommand="lvFamily_ItemCommand" OnItemDataBound="lvFamily_ItemDataBound" >
                    <ItemTemplate>                            
                            <asp:LinkButton ID="lbSelectFamily" runat="server" data-selected='<%# Eval("Selected") %>' CommandArgument='<%# Eval("Group.Id") %>'
                                CssClass="btn btn-primary btn-large btn-block btn-checkin-select family" CausesValidation="false">
                                <%# Eval("Caption") %><br /><span class='checkin-sub-title'><%# Eval("SubCaption") %></span>
                            </asp:LinkButton>
                    </ItemTemplate>
                    </asp:ListView>
                    <asp:DataPager ID="dpPager" runat="server" PageSize="5" PagedControlID="lvFamily">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn btn-primary" />
                        </Fields>
                    </asp:DataPager>
                </div>
                
            </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="pnlUpdatePerson" runat="server" UpdateMode="Conditional" class="span3 person-div">
            <ContentTemplate>
                
                <div class="attended-checkin-body-container">
                    <h3>People</h3>
                    <asp:Repeater ID="repPerson" runat="server" OnItemDataBound="repPerson_ItemDataBound">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbSelectPerson" runat="server" Text='<%# Container.DataItem.ToString() %>' data-id='<%# Eval("Person.Id") %>' CommandArgument='<%# Eval("Person.Id") %>' CssClass="btn btn-primary btn-large btn-block btn-checkin-select person" CausesValidation="false" />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                
            </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="pnlUpdateVisitor" runat="server" UpdateMode="Conditional" class="span3 visitor-div">
            <ContentTemplate>
                
                <div class="attended-checkin-body-container">
                    <h3>Visitors</h3>
                    <asp:Repeater ID="repVisitors" runat="server" OnItemDataBound="repVisitors_ItemDataBound">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbSelectVisitor" runat="server" Text='<%# Container.DataItem.ToString() %>' data-id='<%# Eval("Person.Id") %>' CommandArgument='<%# Eval("Person.Id") %>' CssClass="btn btn-primary btn-large btn-block btn-checkin-select visitor" CausesValidation="false" />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                
            </ContentTemplate>
            </asp:UpdatePanel>

            <div ID="divNothingFound" runat="server" class="span9 nothing-found-message" visible="false">
                <asp:Label ID="lblNothingFound" runat="server" Text="Please add them using one of the buttons on the right:" />
            </div>
            
            <div class="span3 add-someone">
                <div class="attended-checkin-body-container last">
                    <h3 id="actions" runat="server">Actions</h3>
                    <asp:LinkButton ID="lbAddPerson" runat="server" CssClass="btn btn-primary btn-large btn-block btn-checkin-select" OnClick="lbAddPerson_Click" Text="Add Person" CausesValidation="false"></asp:LinkButton>
                    <asp:LinkButton ID="lbAddVisitor" runat="server" CssClass="btn btn-primary btn-large btn-block btn-checkin-select" OnClick="lbAddVisitor_Click" Text="Add Visitor" CausesValidation="false"></asp:LinkButton>                
                    <asp:LinkButton ID="lbAddFamily" runat="server" CssClass="btn btn-primary btn-large btn-block btn-checkin-select" OnClick="lbAddFamily_Click" Text="Add Family" CausesValidation="false" />
                </div>
            </div>
        </div>

    </asp:Panel>

    <asp:Panel ID="AddPersonPanel" runat="server" CssClass="add-person">

        <Rock:ModalAlert ID="maAddPerson" runat="server" />
        <div class="row-fluid attended-checkin-header">
            <div class="span3 attended-checkin-actions">
                <asp:LinkButton ID="lbAddPersonCancel" CssClass="btn btn-large btn-primary" runat="server" OnClick="lbAddPersonCancel_Click" Text="Cancel" CausesValidation="false"/>
            </div>

            <div class="span6">
                <h1><asp:Label ID="lblAddPersonHeader" runat="server"></asp:Label></h1>
            </div>

            <div class="span3 attended-checkin-actions">
                <asp:LinkButton ID="lbAddPersonSearch" CssClass="btn btn-large last btn-primary" runat="server" OnClick="lbAddPersonSearch_Click" Text="Search" CausesValidation="false" />
            </div>
        </div>
        <div class="row-fluid attended-checkin-body">
            <div class="span3">
                <h3>First Name</h3>
            </div>
            <div class="span3">
                <h3>Last Name</h3>
            </div>
            <div class="span2">
                <h3>DOB</h3>
            </div>
            <div class="span2">
                <h3>Gender</h3>
            </div>
            <div class="span2">
                <h3>Ability/Grade</h3>
            </div>
        </div>
        <div class="row-fluid attended-checkin-body searchperson">
            <div class="span3">
                <Rock:DataTextBox ID="tbFirstNameSearch" runat="server" CssClass="fullBlock"></Rock:DataTextBox>
            </div>
            <div class="span3">
                <Rock:DataTextBox ID="tbLastNameSearch" runat="server" CssClass="fullBlock"></Rock:DataTextBox>
            </div>
            <div class="span2">
                <Rock:DatePicker ID="dtpDOBSearch" runat="server" CssClass="datePickerClass"></Rock:DatePicker>
                <asp:CustomValidator ID="cvDOBSearch" runat="server" ErrorMessage="The first DOB/Age field is incorrect."
                    CssClass="align-middle" EnableClientScript="true" Display="None" 
                    ClientValidationFunction="cvBirthDateValidator_ClientValidate" 
                    OnServerValidate="cvBirthDateValidator_ServerValidate" ControlToValidate="dtpDOBSearch" />
            </div>
            <div class="span2">
                <Rock:DataDropDownList ID="ddlGenderSearch" runat="server" CssClass="fullBlock"></Rock:DataDropDownList>
            </div>
            <div class="span2">
                <Rock:DataDropDownList ID="ddlAttributeSearch" runat="server" CssClass="fullBlock"></Rock:DataDropDownList>
            </div>
        </div>
        <br />
        <div class="row-fluid attended-checkin-body searchperson">
            <Rock:Grid ID="grdPersonSearchResults" runat="server" AllowPaging="true" OnRowCommand="grdPersonSearchResults_RowCommand" ShowActionRow="false" PageSize="3" DataKeyNames="personId">
                <Columns>
                    <asp:BoundField DataField="personId" Visible="false" />
                    <asp:BoundField DataField="personFirstName" HeaderText="First Name" />
                    <asp:BoundField DataField="personLastName" HeaderText="Last Name" />
                    <asp:BoundField DataField="personDOB" HeaderText="DOB" />
                    <asp:BoundField DataField="personGender" HeaderText="Gender" />
                    <asp:BoundField DataField="personAttribute" HeaderText="Ability/Grade" />
                    <asp:TemplateField HeaderText="Add">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbAdd" runat="server" CssClass="btn btn-large btn-primary" CommandName="Add" Text="Add" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"><i class="icon-plus"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Rock:Grid>
        </div>
        <div class="row-fluid attended-checkin-body person">
            <asp:LinkButton ID="lbAddSearchedForPerson" runat="server" Text="None of these, add me as a new [person/visitor]." Visible="false" OnClick="lbAddSearchedForPerson_Click" CausesValidation="false"></asp:LinkButton>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="mpePerson" runat="server" TargetControlID="lbOpenPersonPanel" PopupControlID="AddPersonPanel" CancelControlID="lbAddPersonCancel" BackgroundCssClass="modalBackground"></asp:ModalPopupExtender>
    <asp:LinkButton ID="lbOpenPersonPanel" runat="server" CausesValidation="false"></asp:LinkButton>

    <asp:Panel ID="pnlAddFamily" runat="server" CssClass="add-family">

        <div class="row-fluid attended-checkin-header">
            <div class="span3 attended-checkin-actions">
                <asp:LinkButton ID="lbAddFamilyCancel" CssClass="btn btn-large btn-primary" runat="server" OnClick="lbAddFamilyCancel_Click" Text="Cancel" CausesValidation="false" />
            </div>

            <div class="span6">
                <h1>Add Family</h1>
            </div>

            <div class="span3 attended-checkin-actions">
                <asp:LinkButton ID="lbAddFamilySave" CssClass="btn btn-large last btn-primary" runat="server" OnClick="lbAddFamilySave_Click" Text="Save" />
            </div>
        </div>

        <div class="row-fluid attended-checkin-body">
            <div class="span2">
                <h3>First Name</h3>
            </div>
            <div class="span3">
                <h3>Last Name</h3>
            </div>
            <div class="span2">
                <h3>DOB</h3>
            </div>
            <div class="span2">
                <h3>Ability/Grade</h3>
            </div>
            <div class="span3">
                <h3>Gender</h3>
            </div>
        </div>
    
        <div class="row-fluid attended-checkin-body person">

            <asp:ListView ID="lvAddFamily" runat="server" OnPagePropertiesChanging="lvAddFamily_PagePropertiesChanging"  OnItemDataBound="lvAddFamily_ItemDataBound">
            <ItemTemplate>
                <div class="row-fluid">
                    <div class="span2">
                        <asp:TextBox ID="tbFirstName" runat="server" CssClass="fullBlock" />
                    </div>
                    <div class="span3">
                        <asp:TextBox ID="tbLastName" runat="server" CssClass="fullBlock" />
                    </div>
                    <div class="span2">
                        <Rock:DatePicker ID="dpBirthDate" runat="server" />
                        <asp:CustomValidator ID="cvBirthDateValidator" runat="server" 
                            ErrorMessage="Please enter a valid birth date."
                            CssClass="align-middle" EnableClientScript="true" Display="Dynamic"
                            ClientValidationFunction="cvBirthDateValidator_ClientValidate"
                            OnServerValidate="cvBirthDateValidator_ServerValidate" 
                            ControlToValidate="dpBirthDate" />
                    </div>
                    <div class="span2">
                        <Rock:RockDropDownList ID="ddlAbilityGrade" runat="server" CssClass="fullBlock"  />
                    </div>
                    <div class="span3">
                        <Rock:RockDropDownList ID="ddlGender" runat="server" CssClass="fullBlock" />
                    </div>      
                </div>
            </ItemTemplate>
            </asp:ListView>
            <asp:DataPager ID="dpAddFamily" runat="server" PageSize="5" PagedControlID="lvAddFamily">
                <Fields>
                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn btn-primary" />
                </Fields>
            </asp:DataPager>
                                            
        </div>
        
        <asp:ValidationSummary ID="valSummaryBottom" runat="server" HeaderText="Please Correct the Following:" CssClass="alert alert-error block-message error alert error-modal" />

    </asp:Panel>
    <asp:ModalPopupExtender ID="mpeAddFamily" runat="server" TargetControlID="hfOpenFamilyPanel" PopupControlID="pnlAddFamily"
        CancelControlID="lbAddFamilyCancel" BackgroundCssClass="modalBackground" />
    <asp:HiddenField ID="hfOpenFamilyPanel" runat="server" />

</ContentTemplate>
</asp:UpdatePanel>
