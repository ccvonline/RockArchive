﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GatewayTest.ascx.cs" Inherits="RockWeb.Blocks.Finance.Administration.GatewayTest" %>

<asp:UpdatePanel ID="upPayment" runat="server">
    <ContentTemplate>

        <% if (FluidLayout) { %>
            <div class="row-fluid">
                <div class="span6">
        <% } %>

                    <div class="well">
                        <legend>Contribution Information</legend>
                        <div class="form-horizontal">
                            <fieldset>

                            </fieldset>
                        </div>
                    </div>

        <% if (FluidLayout) { %>
                </div>
                <div class="span6">
        <% } %>

                    <div class="well">
                        <legend>Personal Information</legend>
                        <div class="form-horizontal">
                            <fieldset>
                                <Rock:LabeledTextBox ID="txtFirstName" runat="server" LabelText="First Name" CssClass="input-small"></Rock:LabeledTextBox>
                                <Rock:LabeledTextBox ID="txtLastName" runat="server" LabelText="Last Name" CssClass="input-small"></Rock:LabeledTextBox>
                                <Rock:LabeledTextBox ID="txtPhone" runat="server" LabelText="Phone" CssClass="input-medium"></Rock:LabeledTextBox>
                                <Rock:LabeledTextBox ID="txtEmail" runat="server" LabelText="Email" CssClass="input-large"></Rock:LabeledTextBox>
                                <Rock:LabeledTextBox ID="txtStreet" runat="server" LabelText="Address" CssClass="input-large"></Rock:LabeledTextBox>
                                <div class="control-group">
                                    <div class="control-label">&nbsp;</div>
                                    <div class="controls">
                                        <asp:TextBox ID="txtCity" runat="server" CssClass="input-small" /> ,&nbsp;
                                        <Rock:StateDropDownList ID="ddlState" runat="server" UseAbbreviation="true" CssClass="input-mini" />&nbsp;
                                        <asp:TextBox ID="txtZip" runat="server" CssClass="input-small" />
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>

        <% if (FluidLayout) { %>
                </div>
            </div>
        <% } %>


        <asp:Panel ID="pnlPaymentInfo" runat="server" class="well">

            <asp:HiddenField ID="hfPaymentTab" runat="server" />

            <asp:panel ID="pnlPaymentHeading" runat="server" CssClass="tabHeader">
                <legend>Payment Information</legend>
                <asp:PlaceHolder ID="phPills" runat="server" Visible="false">
                    <ul class="nav nav-pills remove-margin">
                        <li id="liCreditCard" runat="server"><a href='#<%=divCCPaymentInfo.ClientID%>' data-toggle="pill">Credit Card</a></li>
                        <li id="liACH" runat="server"><a href='#<%=divACHPaymentInfo.ClientID%>' data-toggle="pill">Bank Account</a></li>
                    </ul>
                </asp:PlaceHolder>
            </asp:panel>

            <div class="tab-content">

                <div id="divCCPaymentInfo" runat="server" visible="false" class="form-horizontal">
                    <fieldset>
                        <Rock:LabeledRadioButtonList ID="rblSavedCC" runat="server" LabelText=" " CssClass="radio-list" RepeatDirection="Vertical" DataValueField="Id" DataTextField="Name" />
                        <div id="divNewCard" runat="server" class="radio-content">
                            <Rock:LabeledTextBox ID="txtCardFirstName" runat="server" LabelText="First Name on Card" CssClass="input-small" Visible="false"></Rock:LabeledTextBox>
                            <Rock:LabeledTextBox ID="txtCardLastName" runat="server" LabelText="Last Name on Card" CssClass="input-small" Visible="false"></Rock:LabeledTextBox>
                            <Rock:LabeledTextBox ID="txtCardName" runat="server" LabelText="Name on Card" CssClass="input-large" Visible="false"></Rock:LabeledTextBox>
                            <Rock:LabeledTextBox ID="txtCreditCard" runat="server" LabelText="Credit Card #" MaxLength="19" CssClass="credit-card input-large" />
                            <ul class="card-logos" >
                                <li class="card-visa"></li>
                                <li class="card-mastercard"></li>
                                <li class="card-amex"></li>
                                <li class="card-discover"></li>
                            </ul>                                        
                            <Rock:MonthYearPicker ID="mypExpiration" runat="server" LabelText="Expiration Date" />
                            <Rock:NumberBox ID="txtCVV" LabelText="Card Security Code" runat="server" MaxLength="3" CssClass="input-mini" />
                            <Rock:LabeledCheckBox ID="cbBillingAddress" runat="server" LabelText=" " Text="Enter a different billing address" CssClass="toggle-input" />
                            <div id="divBillingAddress" runat="server" class="toggle-content">
                                <Rock:LabeledTextBox ID="txtBillingStreet" runat="server" LabelText="Billing Address" CssClass="input-large"></Rock:LabeledTextBox>
                                <div class="control-group">
                                    <div class="control-label">&nbsp;</div>
                                    <div class="controls">
                                        <asp:TextBox ID="txtBillingCity" runat="server" CssClass="input-small" /> ,&nbsp;
                                        <Rock:StateDropDownList ID="txtBillingState" runat="server" UseAbbreviation="true" CssClass="input-mini" />&nbsp;
                                        <asp:TextBox ID="txtBillingZip" runat="server" CssClass="input-small" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>

                <div id="divACHPaymentInfo" runat="server" visible="false" class="form-horizontal">
                    <fieldset>
                        <Rock:LabeledRadioButtonList ID="rblSavedAch" runat="server" RepeatDirection="Vertical" DataValueField="Id" DataTextField="Name" />
                        <div id="divNewBank" runat="server" class="radio-content row-fluid">
                            <div class="span7">
                                <Rock:LabeledTextBox ID="txtBankName" runat="server" LabelText="Bank Name" CssClass="input-medium" />
                                <Rock:LabeledTextBox ID="txtRoutingNumber" runat="server" LabelText="Routing #" CssClass="input-large" />
                                <Rock:LabeledTextBox ID="txtAccountNumber" runat="server" LabelText="Account #" CssClass="input-large" />
                                <Rock:LabeledRadioButtonList ID="rblAccountType" runat="server" RepeatDirection="Horizontal" LabelText="Account Type">
                                    <asp:ListItem Text="Checking" Selected="true"  />
                                    <asp:ListItem Text="Savings" />
                                </Rock:LabeledRadioButtonList>
                            </div>
                            <div class="span5">
                                <asp:Image ID="imgCheck" runat="server" ImageUrl="~/Assets/Images/check-image.png" />
                            </div>
                        </div>
                    </fieldset>
                </div>

            </div>
        </asp:Panel>

        <Rock:NotificationBox ID="errorBox" runat="server" Visible="false"></Rock:NotificationBox>

        <div class="actions">
            <asp:LinkButton ID="btnNext" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="btnNext_Click" />
        </div>

    </ContentTemplate>
</asp:UpdatePanel>


