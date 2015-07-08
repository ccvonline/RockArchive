﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RegistrationEntry.ascx.cs" Inherits="RockWeb.Blocks.Event.RegistrationEntry" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
<ContentTemplate>

    <asp:HiddenField ID="hfTriggerScroll" runat="server" Value="" />

    <asp:ValidationSummary ID="vsSummary" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

    <Rock:NotificationBox ID="nbMain" runat="server" Visible="false" NotificationBoxType="Warning"
        Heading="Sorry" Text="<p>The selected registration could not be found or is no longer active.</p>"></Rock:NotificationBox>

    <asp:Panel ID="pnlHowMany" runat="server" Visible="false" CssClass="registrationentry-intro">

        <h1>How many people will you be registering?</h1>
        <Rock:NumberUpDown ID="numHowMany" NumberDisplayCssClass="input-lg form-control input-width-xs" ButtonCssClass="btn btn-lg btn-default margin-l-sm" runat="server" CssClass="text-center" />

        <div class="actions">
            <asp:LinkButton ID="lbHowManyNext" runat="server" AccessKey="n" Text="Next" CssClass="btn btn-primary pull-right" CausesValidation="true" OnClick="lbHowManyNext_Click" />
        </div>

    </asp:Panel>

    <asp:Panel ID="pnlRegistrant" runat="server" Visible="false" CssClass="registrationentry-registrant">

        <h1><asp:Literal ID="lRegistrantTitle" runat="server" /></h1>
        
        <div class="js-registration-same-family">
            <Rock:RockRadioButtonList ID="rblFamilyOptions" runat="server" Label="Individual is in the same family as" RepeatDirection="Vertical" Required="true" DataTextField="Value" DataValueField="Key" />
        </div>
        
        <asp:PlaceHolder ID="phRegistrantControls" runat="server" />
        
        <div id="divFees" runat="server" class="well registration-additional-options">
            <h4>Additional Options</h4>
            <asp:PlaceHolder ID="phFees" runat="server" />
        </div>

        <div class="actions">
            <asp:LinkButton ID="lbRegistrantPrev" runat="server" AccessKey="p" Text="Previous" CssClass="btn btn-default" CausesValidation="false" OnClick="lbRegistrantPrev_Click"  />
            <asp:LinkButton ID="lbRegistrantNext" runat="server" AccessKey="n" Text="Next" CssClass="btn btn-primary pull-right" CausesValidation="true" OnClick="lbRegistrantNext_Click" />
        </div>

    </asp:Panel>

    <asp:Panel ID="pnlSummaryAndPayment" runat="server" Visible="false" CssClass="registrationentry-summary">
        
        <h1>Summary</h1>
        
        <div class="well">
            <h4>Your Information</h4>
            <div class="row">
                <div class="col-md-6">
                    <Rock:RockTextBox ID="tbYourFirstName" runat="server" Label="First Name" />
                    <Rock:EmailBox ID="tbConfirmationEmail" runat="server" Label="Send Confirmation Emails To" />
                </div>
                <div class="col-md-6">
                    <Rock:RockTextBox ID="tbYourLastName" runat="server" Label="First Name" />
                </div>
            </div>
        </div>
        
        
        
        <asp:Panel ID="pnlMoney" runat="server">

            <div class="well">
                <h4>Summary of Fees</h4>
                
                <Rock:NotificationBox ID="nbDiscountCode" runat="server" Visible="false" NotificationBoxType="Warning"></Rock:NotificationBox>
                <div id="divDiscountCode" runat="server" class="form-group pull-right">
                    <label class="control-label">Discount Code</label>
                    <div class="input-group">
                        <asp:TextBox ID="tbDiscountCode" runat="server" CssClass="form-control input-width-md input-sm"></asp:TextBox>
                        <asp:LinkButton ID="lbDiscountApply" runat="server" CssClass="btn btn-default btn-sm margin-l-sm" Text="Apply" OnClick="lbDiscountApply_Click" CausesValidation="false"></asp:LinkButton>
                    </div>
                </div>

                <div class="fee-table">
                    <asp:Repeater ID="rptFeeSummary" runat="server">
                        <HeaderTemplate>
                            <div class="row fee-header">
                                <div class="col-md-6">
                                    <strong>Description</strong>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div runat="server" class="col-sm-6 pull-right text-right" visible='<%# (RegistrationState.DiscountPercentage > 0.0m) %>'>
                                            <strong>Discounted Amount</strong>
                                        </div>
                                        <div class="col-sm-6 pull-right text-right">
                                            <strong>Amount</strong>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="row fee-row-<%# Eval("Type").ToString().ToLower() %>">
                                <div class="col-md-6">
                                    <%# Eval("Description") %>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div runat="server" class="col-sm-6 pull-right text-right" visible='<%# (RegistrationState.DiscountPercentage > 0.0m) %>'>
                                            <%# Rock.Web.Cache.GlobalAttributesCache.Value( "CurrencySymbol" )%> <%# string.Format("{0:N}", Eval("DiscountedCost")) %> 
                                        </div>
                                        <div class="col-sm-6 pull-right text-right">
                                            <%# Rock.Web.Cache.GlobalAttributesCache.Value( "CurrencySymbol" )%> <%# string.Format("{0:N}", Eval("Cost")) %> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <asp:HiddenField ID="hfTotalCost" runat="server" />
                <Rock:RockLiteral ID="lTotalCost" runat="server" Label="Total Cost" />

                <asp:HiddenField ID="hfPreviouslyPaid" runat="server" />
                <Rock:RockLiteral ID="lPreviouslyPaid" runat="server" Label="Previously Paid" />

                <asp:HiddenField ID="hfMinimumDue" runat="server" />
                <Rock:RockLiteral ID="lMinimumDue" runat="server" Label="Minimum Due Today" />

                <Rock:NumberBox ID="nbAmountPaid" runat="server" NumberType="Currency" Label="Amount To Pay Today" Required="true" />
            
                <Rock:RockLiteral ID="lRemainingDue" runat="server" Label="Amount Remaining" />
            </div>

            <div id="divPaymentInfo" runat="server" class="well">

                <h4>Payment Information</h4>

                <Rock:RockTextBox ID="txtCardFirstName" runat="server" Label="First Name on Card" Visible="false" Required="true"></Rock:RockTextBox>
                <Rock:RockTextBox ID="txtCardLastName" runat="server" Label="Last Name on Card" Visible="false" Required="true"></Rock:RockTextBox>
                <Rock:RockTextBox ID="txtCardName" runat="server" Label="Name on Card" Visible="false" Required="true"></Rock:RockTextBox>
                <Rock:RockTextBox ID="txtCreditCard" runat="server" Label="Credit Card #" MaxLength="19" CssClass="credit-card" Required="true" />

                <ul class="card-logos list-unstyled">
                    <li class="card-visa"></li>
                    <li class="card-mastercard"></li>
                    <li class="card-amex"></li>
                    <li class="card-discover"></li>
                </ul>
                                        
                <div class="row">
                    <div class="col-sm-6">
                        <Rock:MonthYearPicker ID="mypExpiration" runat="server" Label="Expiration Date" Required="true" />
                    </div>
                    <div class="col-sm-6">
                        <Rock:NumberBox ID="txtCVV" Label="Card Security Code" CssClass="input-width-xs" runat="server" MaxLength="4" Required="true"/>
                    </div>
                </div>

                <Rock:AddressControl ID="acBillingAddress" runat="server" UseStateAbbreviation="true" UseCountryAbbreviation="false" ShowAddressLine2="false" 
                    Required="true" RequiredErrorMessage="Billing Address is required" />

            </div>

        </asp:Panel>

        <div class="actions">
            <asp:LinkButton ID="lbSummaryPrev" runat="server" AccessKey="p" Text="Previous" CssClass="btn btn-default" CausesValidation="false" OnClick="lbSummaryPrev_Click" />
            <asp:LinkButton ID="lbSummaryNext" runat="server" AccessKey="n" Text="Next" CssClass="btn btn-primary pull-right" CausesValidation="true" OnClick="lbSummaryNext_Click" />
        </div>

        <asp:Panel ID="pnlDupWarning" runat="server" CssClass="alert alert-block" Visible="true">
            <h4>Warning!</h4>
            <p>
                You have already submitted a similar transaction that has been processed.  Are you sure you want
            to submit another possible duplicate transaction?
            </p>
            <asp:LinkButton ID="btnConfirm" runat="server" Text="Yes, submit another transaction" CssClass="btn btn-danger margin-t-sm" OnClick="lbConfirm_Click"/>
        </asp:Panel>

    </asp:Panel>

    <asp:Panel ID="pnlSuccess" runat="server" Visible="false" >
        
        <h1>Success</h1>
        
        <asp:PlaceHolder ID="phSuccessControls" runat="server" />

    </asp:Panel>

</ContentTemplate>
</asp:UpdatePanel>
