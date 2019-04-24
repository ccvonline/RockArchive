﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlanAVisitForm.ascx.cs" Inherits="RockWeb.Plugins.church_ccv.Cms.PlanAVisitForm" %>

<link rel="stylesheet" href="/Plugins/church_ccv/Cms/styles/plan-a-visit-form.css">

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <div class="form-alerts">

            <Rock:NotificationBox ID="nbMessage" runat="server"></Rock:NotificationBox>

            <span id="nbHTMLMessage" class="alert hidden"></span>
        </div>

        <%-- Form Panel --%>
        <asp:Panel ID="pnlForm" runat="server">

            <div class="plan-a-visit">

                <%-- Progress Tracker --%>
                <div class="form-progress">
                    <div class="step">

                        <asp:Button ID="btnProgressAdults" runat="server" ClientIDMode="Static" OnClick="btnFormBack_Click" CommandName="pnlAdults" Text="1" CssClass="step-number active" Enabled="false" />

                        <h5>Adults</h5>
                    </div>
                    <div class="step">

                        <asp:Button ID="btnProgressChildren" runat="server" ClientIDMode="Static" OnClick="btnFormBack_Click" CommandName="pnlChildren" Text="2" CssClass="step-number" Enabled="false" />

                        <h5>Children</h5>
                    </div>
                    <div class="step">

                        <%-- Even though button not used for confirm, leaving it as disabled button for consistent/easier styling --%>
                        <asp:Button ID="btnProgressSubmit" runat="server" ClientIDMode="Static" OnClientClick="return false;" Text="3" CssClass="step-number" Enabled="false" />

                        <h5>Submit</h5>
                    </div>
                </div>

                <%-- Adults Form --%>
                <asp:Panel ID="pnlAdults" runat="server" ClientIDMode="Static" CssClass="panel-adults" Visible="true">

                    <asp:Panel ID="pnlAdultsForm" runat="server">
                        <div class="form-header">
                            <h2>Let us know you're coming</h2>
                            <p>We want to make your first visit as smooth and enjoyable as possible</p>
                        </div>

                        <div class="form">
                            <div class="form-row">
                                <h4>Your name</h4>
                                <p class="required-key">required</p>
                            </div>
                            <div class="form-row">
                                <div class="form-field">
                                    <Rock:RockTextBox ID="tbAdultFirstName" runat="server" Label="First" Required="true" Text="" />
                                </div>
                                <div class="form-field">
                                    <Rock:RockTextBox ID="tbAdultLastName" runat="server" Label="Last" Required="true" Text="Workflow" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-field">
                                    <Rock:RockTextBox ID="tbAdultEmail" runat="server" Label="Email" Required="true" Text="w@safety.netz" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-field">
                                    <Rock:RockTextBox ID="tbAdultAddress" runat="server" Label="Address" Required="false" Text="123 test st" Visible="false" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-field">
                                    <Rock:RockTextBox ID="tbAdultCity" runat="server" Label="City" Required="false" Text="Test" Visible="false" />
                                </div>
                                <div class="form-field">
                                    <Rock:RockDropDownList ID="ddlAdultState" runat="server" Label="State" Required="false" Visible="false" />
                                </div>
                                <div class="form-field">
                                    <Rock:RockTextBox ID="tbAdultPostalCode" runat="server" Label="Zipcode" Required="false" Text="12345" Visible="false" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-field">
                                    <Rock:RockTextBox ID="tbAdultFormMobile" runat="server" Label="Mobile" Required="false" Text="6231234576" />
                                </div>
                                <div class="form-field">
                                     <Rock:RockDropDownList ID="ddlCampus" runat="server" Label="Campus" Required="true" ClientIDMode="Static" OnSelectedIndexChanged="CampusDropDown_SelectedIndexChanged" AutoPostBack="true" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-field">
                                    <div id="divVisitDate" runat="server" ClientIDMode="Static" class="hidden">
                                        <Rock:RockDropDownList ID="ddlVisitDate" runat="server" Label="Desired Date" Required="true" ClientIDMode="Static" OnSelectedIndexChanged="VisitDateDropDown_SelectedIndexChanged" AutoPostBack="true" />
                                    </div>
                                </div>
                                <div class="form-field">
                                    <div id="divServiceTime" runat="server" ClientIDMode="Static" class="hidden">
                                        <Rock:RockDropDownList ID="ddlServiceTime" runat="server" Label="Service Time" Required="true" ClientIDMode="Static" OnSelectedIndexChanged="ServiceTimeDropDown_SelectedIndexChanged" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                            <div id="divSpouse" runat="server" ClientIDMode="Static" class="hidden">
                                <h4>Your spouse (optional)</h4>
                                <div class="form-row">
                                    <div class="form-field">
                                        <Rock:RockTextBox ID="tbSpouseFirstName" runat="server" Label="First" />
                                    </div>
                                    <div class="form-field">
                                        <Rock:RockTextBox ID="tbSpouseLastName" runat="server" Label="Last" />
                                    </div>
                                </div>

                            </div>
                        </div>

                        <div class="form-navigation">
                            <asp:Button ID="btnAdultsNext" runat="server" ClientIDMode="Static" OnClick="btnAdultsNext_Click" Text="Next" CssClass="btn btn-primary" />                       
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlAdultsExisting" runat="server" Visible="false">

                        <div class="form-header">
                            <h2>Whoops, it looks like you may already be in our system.</h2>
                            <p>
                                The following shares your last name and email.<br />
                                Are you one of these people?
                            </p>
                        </div>

                        <div class="form">
                            <div class="form-row form-group">
                                <Rock:RockRadioButtonList ID="rblExisting" runat="server" ClientIDMode="Static" CssClass="existing-people" />
                            </div>
                        </div>

                        <div class="form-alerts">
                            <Rock:NotificationBox ID="nbAlertExisting" runat="server" />
                        </div>

                        <div class="form-navigation">
                            <asp:Button ID="btnAdultsExistingBack" runat="server" ClientIDMode="Static" OnClick="btnFormBack_Click" CommandName="pnlAdults" Text="Back" CssClass="btn btn-default" />
                            <asp:Button ID="btnAdultsExistingNext" runat="server" ClientIDMode="Static" OnClick="btnAdultsExistingNext_Click" Text="Next" CssClass="btn btn-primary" />                       
                        </div>

                    </asp:Panel>

                </asp:Panel>

                <%-- Children Form --%>
                <asp:Panel ID="pnlChildren" runat="server" ClientIDMode="Static" CssClass="panel-children" Visible="false">

                    <asp:Panel ID="pnlChildrenExisting" runat="server" Visible="false">
                        <div class="form-header">
                            <h2>The following children are currently associated with you</h2>
                            <p>Need different content text</p>
                        </div>
                        <div class="existing-children-vertical">

                            <asp:Literal ID="ltlExistingChildrenVertical" runat="server" />

                        </div>
                        <div class="form">
                            <div class="form-row row-centered">
                                <div class="form-field existing-children-buttons">
                                    <asp:Button ID="btnAddAnotherChild" runat="server" OnClick="ShowChildrenForm_Click" Text="Add another child?" CssClass="btn btn-default" />
                                    <asp:Button ID="btnNotMyChildren" runat="server" OnClick="ShowChildrenForm_Click" Text="Not my children" CssClass="btn btn-default" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>


                    <asp:Panel ID="pnlChildrenQuestion" runat="server" CssClass="children-question" Visible="false">
                        <div class="form-header">
                            <h2>Will you be bringing children?</h2>
                        </div>
                        <div class="form-navigation">
                            <asp:Button ID="btnChildrenYes" runat="server" OnClick="ShowChildrenForm_Click" Text="Yes" CssClass="btn btn-primary" />
                            <asp:Button ID="btnChildrenNo" runat="server" OnClick="btnChildrenNext_Click" Text="No" CssClass="btn btn-primary" />
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlChildrenForm" runat="server" CssClass="children-form" Visible="false">

                        <asp:HiddenField ID="hfAllergiesFormState" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfGenderFormState" runat="server" ClientIDMode="Static" />

                        <div class="form-header">
                            <h2>Child Pre-Register</h2>
                            <p>Security is very important to us. Your mobile number will be used to log into your family on our system, and to contact you for any reason during service.</p>
                        </div>

                        <div class="form">
                            <div class="form-row">
                                <div class="existing-children-horizontal">
                                    <asp:Literal ID="ltlExistingChildrenHorizontal" runat="server" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div></div>
                                <p class="required-key">required</p>
                            </div>
                            <div class="form-row">
                                <div class="form-field">
                                    <Rock:RockTextBox ID="tbChildrenFormMobile" runat="server" Label="Mobile (Parent or Guardian)" CssClass="required" />
                                </div>

                            </div>
                            <div class="form-row">
                                <div class="form-field">
                                    <Rock:RockTextBox ID="tbChildFirstName" runat="server" Label="Child's First Name" CssClass="required" />
                                </div>

                            </div>
                            <div class="form-row">
                                <div class="form-field">
                                    <Rock:RockTextBox ID="tbChildLastName" runat="server" Label="Child's Last Name" CssClass="required" />  
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-field">
                                    <Rock:RockDropDownList ID="ddlChildBdayYear" runat="server" Label="Year" ClientIDMode="Static" OnSelectedIndexChanged="ddlChildBdayYear_SelectedIndexChanged" AutoPostBack="true" />
                                </div>
                                <div class="form-field">
                                    <Rock:RockDropDownList ID="ddlChildBdayMonth" runat="server" Label="Month" OnSelectedIndexChanged="ddlChildBdayMonth_SelectedIndexChanged" AutoPostBack="true" Visible="false" />
                                </div>
                                <div class="form-field">
                                    <Rock:RockDropDownList ID="ddlChildBdayDay" runat="server" Label="Day" ClientIDMode="Static" Visible="false" />
                                 </div>
                            </div>
                            <h4 class="row-centered">Optional:</h4>
                            <div class="child-optional">
                                <div class="form-row">
                                    <div class="form-field">
                                        <Rock:RockRadioButtonList ID="rblAllergies" runat="server" Label="Allergies" RepeatDirection="Horizontal" ClientIDMode="Static">
                                            <asp:ListItem Text="Yes" Value="Yes" />
                                            <asp:ListItem Text="No" Value="No" />
                                        </Rock:RockRadioButtonList>
                                    </div>
                                    <div class="form-field">
                                        <Rock:RockRadioButtonList ID="rblGender" runat="server" Label="Gender" RepeatDirection="Horizontal" ClientIDMode="Static">
                                            <asp:ListItem Text="Boy" Value="Boy" />
                                            <asp:ListItem Text="Girl" Value="Girl" />
                                        </Rock:RockRadioButtonList>
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="form-field field-allergies">                                        
                                        <Rock:RockTextBox ID="tbAllergies" runat="server" ClientIDMode="Static" Placeholder="List Allergies:" CssClass="allergies hidden" TextMode="MultiLine" Rows="3" />
                                    </div>
                                    <div class="form-field field-grade">
                                        <Rock:GradePicker ID="gpChildGrade" runat="server" Label="Grade" UseGradeOffsetAsValue="true" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-alerts">
                            <Rock:NotificationBox ID="nbAlertChildForm" runat="server" />
                        </div>

                    </asp:Panel>

                    <div class="form-navigation">
                        <asp:Button ID="btnChildrenBack" runat="server" ClientIDMode="Static" OnClick="btnFormBack_Click" CommandName="pnlAdults" Text="Back" CssClass="btn btn-default" />                       
                        <asp:Button ID="btnChildrenAddAnother" runat="server" ClientIDMode="Static" OnClick="btnChildrenAddAnother_Click" Text="Add another child?" CssClass="btn btn-default" Visible="false" />
                        <asp:Button ID="btnChildrenNext" runat="server" ClientIDMode="Static" OnClick="btnChildrenNext_Click" Text="Next" CssClass="btn btn-primary" Visible="false" />
                    </div>

                </asp:Panel>

                <%-- Submit Panel --%>
                <asp:Panel ID="pnlSubmit" runat="server" ClientIDMode="Static" CssClass="panel-submit" Visible="false">

                     <div class="submit-form">
                        <div class="form-header">
                            <h2>Confirm Your Visit</h2>
                            <p>Make sure to visit the New to CCV table when you arrive!</p>
                        </div>
                        <div class="visit-details">
                            <div class="confirm-detail">
                                <asp:Label ID="lblSubmitCampus" runat="server" Text="Campus" />
                                <Rock:RockDropDownList ID="ddlEditCampus" runat="server" Visible="false" OnSelectedIndexChanged="CampusDropDown_SelectedIndexChanged" AutoPostBack="true" Required="true" />
                            </div>
                            <div class="confirm-detail">
                                <asp:Label ID="lblSubmitVisitDate" runat="server" Text="Visit Date" />
                                <Rock:RockDropDownList ID="ddlEditVisitDate" runat="server" Visible="false" OnSelectedIndexChanged="VisitDateDropDown_SelectedIndexChanged" AutoPostBack="true" Required="true" />
                            </div>
                             <div class="confirm-detail">
                                <asp:Label ID="lblSubmitServiceTime" runat="server" Text="Service Time" />
                                <Rock:RockDropDownList ID="ddlEditServiceTime" runat="server" Visible="false" OnSelectedIndexChanged="ServiceTimeDropDown_SelectedIndexChanged" AutoPostBack="true" Required="true" />
                            </div>

                            <asp:Button ID="btnEditVisitDetails" runat="server" Text="Edit details" OnClick="lbEditVisitDetails_Click" />
                        </div>
                        <div class="submit-survey row-centered">
                            <h4>How did you hear about CCV?</h4>
                            <Rock:RockRadioButtonList ID="rblSurvey" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ClientIDMode="Static">
                                <asp:ListItem Value="Drive-by">Drive By / New to area</asp:ListItem>
                                <asp:ListItem Value="Current Member">Current Member</asp:ListItem>
                                <asp:ListItem Value="Online / Website">Online Search</asp:ListItem>
                                <asp:ListItem Value="Advertisement">Advertising</asp:ListItem>
                                <asp:ListItem Value="STARS">Stars sports program</asp:ListItem>
                                <asp:ListItem Value="Other">Other</asp:ListItem>
                             </Rock:RockRadioButtonList>
                         </div>

                        <div class="form-alerts">
                            <Rock:NotificationBox ID="nbAlertSubmit" runat="server" />
                        </div>

                        <div class="form-navigation">
                            <asp:Button ID="btnSubmitBack" runat="server" ClientIDMode="Static" OnClick="btnFormBack_Click" CommandName="pnlChildren" Text="Back" CssClass="btn btn-default" />                       
                            <asp:Button ID="btnSubmitNext" runat="server" ClientIDMode="Static" OnClick="btnSubmitNext_Click" Text="Submit" CssClass="btn btn-primary" />
                            <asp:Button ID="btnSubmitRetry" runat="server" OnClick="btnSubmitRetry_Click" Text="Retry" CssClass="btn btn-primary" Enabled="false" Visible="false" />
                        </div>

                </asp:Panel>

            </div>


        </asp:Panel>

        <asp:Panel ID="pnlSuccess" runat="server" Visible="false">
            
            <div class="form-header">
                <h2>Thank you!</h2>
                <p>You will be receiving a confirmation email with helpful information, including a map link to make getting to <asp:Label ID="lblCampusVisit" runat="server" Text="Campus" /> a breeze</p>
            </div>
            <div class="form-navigation">
                <a href="" class="btn btn-primary">Back to home</a>
            </div>

        </asp:Panel>






    </ContentTemplate>
</asp:UpdatePanel>

<script>
    function pageLoad() {
        restoreFormState();

        $('#rblAllergies').on('change', function () {
            // get selected value
            var value = $('#rblAllergies input:checked').val();

            // hide/show text box depending on selected val
            if (value == "Yes") {
                $('#tbAllergies').removeClass('hidden');
                $('#hfAllergiesFormState').attr('value', 'Yes');
            }
            else {
                $('#tbAllergies').addClass('hidden');
                $('#hfAllergiesFormState').attr('value', 'No');

            }

            // toggle active class to change color of selected item
            $('#rblAllergies input[type="radio"]:checked').parents('label').addClass('active');
            $('#rblAllergies input[type="radio"]:not(:checked)').parents('label').removeClass('active');
        });

        $('#rblGender').on('change', function () {
            // toggle active class to change color of selected item
            $('#rblGender input[type="radio"]:checked').parents('label').addClass('active');
            $('#rblGender input[type="radio"]:not(:checked)').parents('label').removeClass('active');
        });

        $('#rblSurvey').on('change', function () {
            // toggle active class to change color of selected item
            $('#rblSurvey input[type="radio"]:checked').parents('label').addClass('active');
            $('#rblSurvey input[type="radio"]:not(:checked)').parents('label').removeClass('active');
        });

        // error checking
        $('.has-error').on('change', function () {
            var value = "";

            // error check for text boxes
            if ($(this).hasClass('rock-text-box')) {
                // always reset value to ""
                value = "";
                value = $(this).children('div').children('input').val();

                if (value != "") {
                    // has value
                    $(this).removeClass('has-error');
                }
                else {
                    // no value
                    $(this).addClass('has-error');
                }
            }

            // error check for dropdowns (that dont use postbacks)
            if ($(this).hasClass('rock-drop-down-list')) {
                // always reset value to ""
                value = "";
                value = $(this).find(':selected').val();

                if (value != "") {
                    // has value
                    $(this).removeClass('has-error');
                }
                else {
                    // no value
                    $(this).addClass('has-error');
                }
            }

        });
    }

    function restoreFormState() {
        var allergiesValue = $('#hfAllergiesFormState').val();

        if (allergiesValue == 'Yes') {
            $('#tbAllergies').removeClass('hidden');
        }
        else if (allergiesValue == 'No') {
            $('#tbAllergies').addClass('hidden');
        }

        $('#rblAllergies input[type="radio"]:checked').parents('label').addClass('active');
        $('#rblAllergies input[type="radio"]:not(:checked)').parents('label').removeClass('active');

        $('#rblGender input[type="radio"]:checked').parents('label').addClass('active');
        $('#rblGender input[type="radio"]:not(:checked)').parents('label').removeClass('active');

        $('#rblSurvey input[type="radio"]:checked').parents('label').addClass('active');
        $('#rblSurvey input[type="radio"]:not(:checked)').parents('label').removeClass('active');
    }
</script>

