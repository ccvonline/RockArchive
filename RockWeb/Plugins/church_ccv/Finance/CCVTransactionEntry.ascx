﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CCVTransactionEntry.ascx.cs" Inherits="RockWeb.Plugins.church_ccv.Finance.CCVTransactionEntry" %>

<link rel="stylesheet" href="/Themes/church_ccv_External_v8/Styles/pages/home/get-involved/giving.css">

<asp:UpdatePanel ID="upPayment" runat="server">
    <ContentTemplate>

        <%-- hidden field to store...
             Transaction.Guid to use for the transaction. This is to help prevent duplicate transactions.
             PaymentType - used to know what type of transaction to process
             Saved Payment Account Name - used for displaying success info (this is encrypted)  --%>
        <asp:HiddenField ID="hfTransactionGuid" runat="server" Value="" />
        <asp:HiddenField ID="hfPaymentType" runat="server" Value="CC" ClientIDMode="Static" />
        <asp:HiddenField ID="hfSavedPaymentAccountName" runat="server" Value="" />
        <asp:HiddenField ID="hfIsScheduledTransaction" runat="server" Value="false" ClientIDMode="Static" />
        <asp:HiddenField ID="hfSuccessScheduleStartDate" runat="server" Value="" ClientIDMode="Static" />

        <div id="divTransactionCard" class="transaction-card">
        
            <div class="transaction-alerts">

                <Rock:NotificationBox ID="nbMessage" runat="server"></Rock:NotificationBox>

                <span id="nbHTMLMessage" class="alert hidden"></span>
            </div>

            <%-- Transaction Panel --%>
            <asp:Panel ID="pnlTransaction" runat="server" Visible="true">

                <div class="transaction-card-panel">
                    <%-- Progress Tracker --%>
                    <div class="transaction-progress">
                        <div  class="step">
    
                            <asp:Button runat="server" ID="btnProgressAmount" ClientIDMode="Static" OnClientClick="btnProgress_OnClick('pnlAmount'); return false;" Text="1" CssClass="step-number active" />
                            
                            <h5>Amount</h5>
                        </div>
                        <div class="step">

                            <asp:Button runat="server" ID="btnProgressPerson" ClientIDMode="Static" OnClientClick="btnProgress_OnClick('pnlPerson'); return false;" Text="2" CssClass="step-number" />

                            <h5>Personal</h5>
                        </div>
                        <div class="step">

                            <asp:Button runat="server" ID="btnProgressPayment" ClientIDMode="Static" OnClientClick="btnProgress_OnClick('pnlPayment'); return false;" Text="3" CssClass="step-number" />

                            <h5>Payment</h5>
                        </div>
                        <div class="step">

                            <%-- Even though button not used for confirm, leaving it as disabled button for consistent/easier styling --%>
                            <asp:Button runat="server" ID="btnProgressConfirm" ClientIDMode="Static" OnClientClick="return false;" Text="4" CssClass="step-number" Enabled="false" />

                            <h5>Confirm</h5>
                        </div>
                    </div>

                    <%-- Amount Form --%>
                    <asp:Panel ID="pnlAmount" runat="server" ClientIDMode="Static" CssClass="panel-amount">

                        <div class="accounts-wrapper">

                            <asp:DropDownList ID="ddlAccounts" runat="server" CssClass="accounts form-control" ClientIDMode="Static" />

                        </div>                                               
                        <div class="amount-wrapper">
                            
                            <asp:TextBox ID="nbAmount" runat="server" Placeholder="10.00" ClientIDMode="Static" CssClass="amount form-control" />

                        </div>

                        <div class="schedule-wrapper">
                            <%-- Scheduled Transaction Toggle --%>
                            <asp:Panel ID="pnlScheduledTransactionToggle" runat="server">

                                <div class="schedule-toggle">
                                    <asp:CheckBox ID="tglScheduledTransaction" runat="server" ClientIDMode="Static" CssClass="toggle-input-form" Text="<span class='slider'></span>" AutoPostBack="false" />                                          

                                    <span style="margin-left: 10px;">Schedule Recurring Transaction</span>
                           
                           
                                </div>

                            </asp:Panel>

                            <asp:Panel ID="pnlScheduledTransaction" runat="server" CssClass="collapse schedule-transaction" ClientIDMode="Static">

                                <div class="schedule-transaction-wrapper">
                                    <asp:DropDownList ID="ddlScheduleFrequency" runat="server" CssClass="schedule-frequency form-control" ClientIDMode="Static" />
                                </div>
    
                                <div class="schedule-date-wrapper">

                                    <Rock:DatePicker runat="server" ID="dpScheduledTransactionStartDate" Label="Select a start date for your recurring giving plan" ClientIDMode="Static"></Rock:DatePicker>

                                </div>
                            
                            </asp:Panel>
                        </div>

                        <div class="comment-wrapper">

                            <Rock:RockTextBox ID="tbCommentEntry" runat="server" Label="Comment" Visible="false" ClientIDMode="Static" />

                        </div>            
                        <div class="navigation">
                            <%-- Empty div is used to put Next button into correct position --%>
                            <div class="navitation-left">
                            </div>
                            <div class="navigation-right">                    
                        
                                <asp:Button runat="server" ID="btnAmountNext" ClientIDMode="Static" OnClientClick="btnNext_OnClick('pnlPerson'); return false;" Text="Next" CssClass="btn btn-primary" />
                        
                            </div>
                        </div>

                    </asp:Panel>

                    <%-- Person Information Form --%>
                    <asp:Panel ID="pnlPerson" runat="server" ClientIDMode="Static" CssClass="panel-person hidden">

                        <div class="person-info">

                            <Rock:RockTextBox ID="tbFirstName" runat="server" Label="First Name" ClientIDMode="Static" CssClass="required" />
                    
                            <Rock:RockTextBox ID="tbLastName" runat="server" Label="Last Name" ClientIDMode="Static" CssClass="required" />

                            <Rock:RockTextBox ID="tbEmail" runat="server" Label="Email" ClientIDMode="Static" />

                        </div>
                        <div class="navigation">
                            <div class="navigation-left">

                                <asp:Button runat="server" ID="btnPersonBack" ClientIDMode="Static" OnClientClick="btnBack_OnClick('pnlAmount'); return false;" Text="Back" CssClass="btn btn-primary" />

                            </div>
                            <div class="navigation-right">                    

                                <asp:Button runat="server" ID="btnPersonNext" ClientIDMode="Static" OnClientClick="btnNext_OnClick('pnlPayment'); return false;" Text="Next" CssClass="btn btn-primary" />

                            </div>
                        </div>

                    </asp:Panel>

                    <%-- Payment Information Form --%>
                    <asp:Panel ID="pnlPayment" runat="server" ClientIDMode="Static" CssClass="panel-payment hidden" >
                
                        <div class="payment-type">
                       
                                <asp:Button runat="server" ID="btnSavedPayment" ClientIDMode="Static" OnClientClick="btnSavedPayment_OnClick(); return false;" Text="Saved Account" CssClass="btn" Visible="false" />

                                <asp:Button runat="server" ID="btnBankAccount" ClientIDMode="Static" OnClientClick="btnBankAccount_OnClick(); return false;" Text="Bank Account" CssClass="btn" />
                                
                                <asp:Button runat="server" ID="btnCreditCard" ClientIDMode="Static" OnClientClick="btnCreditCard_OnClick(); return false;" Text="Credit Card" CssClass="btn btn-primary" />
                                
                        </div>

                        <%-- Saved Payment Form --%>
                        <asp:Panel ID="pnlSavedPayment" runat="server" ClientIDMode="Static" CssClass="panel-savedpayment hidden">
                            
                            <div class="savedpayment-wrapper">
    
                                <Rock:RockDropDownList ID="ddlSavedPaymentAccounts" runat="server" ClientIDMode="Static" DataValueField="Id" DataTextField="Name" />

                            </div>

                        </asp:Panel>
                
                        <%-- Credit Card Form --%>
                        <asp:Panel ID="pnlCreditCard" runat="server" ClientIDMode="Static" CssClass="panel-creditcard">
                    
                            <div class="creditcard-card">

                                <Rock:RockTextBox ID="tbName" runat="server" Label="Name on Card" ClientIDMode="Static" CssClass="required" />

                                <div class="creditcard-wrapper">
                                    <label class="control-label" for="nbCreditCard">Credit Card #</label>

                                    <asp:TextBox ID="nbCreditCard" runat="server" ClientIDMode="Static" CssClass="form-control" />

                                </div>
                                <div class="creditcard-expdate-cvv">

                                    <Rock:MonthYearPicker ID="mypExpirationDate" runat="server" Label="Exp Date" ClientIDMode="Static" MinimumYear="2018" />

                                    <Rock:NumberBox ID="nbCVV" runat="server" Label="CVV" ClientIDMode="Static" CssClass="numbers-only" MaxLength="4" />

                                </div>
                            </div>
                            <div class="creditcard-card">

                                <Rock:RockTextBox ID="tbStreet" runat="server" Label="Billing Address" ClientIDMode="Static" CssClass="required" />

                                <div class="creditcard-city">

                                    <Rock:RockTextBox ID="tbCity" runat="server" Label="City" ClientIDMode="Static" CssClass="required" />

                                </div>
                                <div class="creditcard-state-country-zip">

                                    <Rock:StateDropDownList ID="ddlState" runat="server" Label="State" ClientIDMode="Static" UseAbbreviation="true" />

                                    <Rock:RockDropDownList ID="ddlCountry" runat="server" Label="Country" ClientIDMode="Static" />

                                    <Rock:NumberBox ID="nbPostalCode" runat="server" Label="Postal Code" ClientIDMode="Static" CssClass="numbers-only" />

                                </div>
                            </div>

                        </asp:Panel>

                        <%-- ACH Form --%>
                        <asp:Panel ID="pnlBankAccount" runat="server" ClientIDMode="Static" CssClass="panel-bankaccount hidden">

                            <Rock:NumberBox ID="nbRoutingNumber" runat="server" Label="Routing #" ClientIDMode="Static" CssClass="numbers-only" />

                            <Rock:NumberBox ID="nbAccountNumber" runat="server" Label="Account #" ClientIDMode="Static" CssClass="numbers-only" />

                            <Rock:RockRadioButtonList ID="rblAccountType" runat="server" Label="Account Type" RepeatDirection="Horizontal" ClientIDMode="Static">
                                <asp:ListItem Text="Savings" Value="Savings" />
                                <asp:ListItem Text="Checking" Value="Checking" />
                            </Rock:RockRadioButtonList>

                        </asp:Panel>
                
                        <div class="navigation">
                            <div class="navigation-left">

                                <asp:Button runat="server" ID="btnPaymentBack" ClientIDMode="Static" OnClientClick="btnBack_OnClick('pnlPerson'); return false;" Text="Back" CssClass="btn btn-primary" />

                            </div>
                            <div class="navigation-right">                    

                                <asp:Button runat="server" ID="btnPaymentNext" ClientIDMode="Static" OnClientClick="btnNext_OnClick('pnlConfirm'); return false;" Text="Next" CssClass="btn btn-primary" />

                            </div>
                        </div>

                    </asp:Panel>

                    <%-- Transaction Confirmation Card --%>
                    <asp:Panel ID="pnlConfirm" runat="server" ClientIDMode="Static" CssClass="panel-confirm hidden">
            
                        <div class="confirm-card">
                            <div class="confirm-card-gift">
                                <p id="confirmGiftMessage" class="confirm-gift-summary">You are giving a gift today of</p>
                                <h3 id="confirmGiftAmount">$0</h3>
                                <p id="confirmScheduleStartMessage" class="confirm-gift-summary">using payment account</p>
                            </div>
                            <div class="confirm-paymenttype-card">
                                <span id="accountType"></span>
                                <span id="personName"></span>
                                <span id="confirmAccountNumber"></span>
                                <div class="confirm-paymenttype-card-footer">
                                    <i class="mdi mdi-information-outline"></i>
                                    <span id="savedAccountName"></span>
                                </div>
                            </div>
                            <p class="confirm-agreement">
                                By clicking "Confirm" below, I agree to allow to transer the amount above from my account. I can update the payment information at any time by returning to this website.
                            </p>
                        </div>
                        <div class="navigation">
                            <div class="navigation-left">

                                <asp:Button runat="server" ID="btnConfirmBack" ClientIDMode="Static" OnClientClick="btnBack_OnClick('pnlPayment'); return false;" Text="Back" CssClass="btn btn-primary" Visible="true" />

                            </div>
                            <div class="navigation-right">                    

                                <asp:Button runat="server" ID="btnConfirmNext" OnClick="btnConfirmNext_Click" OnClientClick="handleSubmit()" UseSubmitBehavior="false" ClientIDMode="Static" Text="Confirm Gift" CssClass="btn btn-primary" />

                            </div>
                        </div>
                        <div class="card-footer">
                            <i class="mdi mdi-lock"></i><p>This is a secure 128-byte SSL encrypted payment</p>
                        </div>

                    </asp:Panel>

                </div>

            </asp:Panel>

            <%-- Transaction Success Card --%>
            <asp:Panel ID="pnlPaymentSuccess" runat="server" Visible="false">

                <div class="transaction-card-panel panel-success">

                    <%-- Success Message --%>
                    <asp:Panel runat="server" ID="pnlSuccessCheckmark" CssClass="success-checkmark"><i class="mdi mdi-check"></i></asp:Panel>

                    <asp:Label runat="server" ID="lblSuccessMessage" CssClass="success-message">
                        Thank you for your generous contribution.<br />
                        Your support is helping to actively achieve our mission.<br />
                        We are so grateful for your commitment.
                    </asp:Label>

                    <asp:Label ID="lblSaveScheduleTransactionResult" runat="server" Visible="false" CssClass="save-success" />

                    <asp:Label ID="lblSavePaymentResult" runat="server" Visible="false" CssClass="save-success" />

                    <%-- Schedule Transaction Toggle --%>
                    <asp:Panel ID="pnlSuccessScheduleTransaction" runat="server" Visible="false">

                        <div class="schedule-toggle">
                            <span style="margin-right: 10px;">Schedule Recurring Transaction</span>
                           
                            <asp:CheckBox ID="tglSuccessScheduleTransaction" runat="server" ClientIDMode="Static" CssClass="toggle-input-form" Text="<span class='slider'></span>" AutoPostBack="false" />                                          

                        </div>

                    </asp:Panel>
                    
                    <%-- Save Payment Account Toggle --%>
                    <asp:Panel ID="pnlSavePaymentAccount" runat="server" Visible="false">

                        <div class="schedule-toggle">
                            <span style="margin-right: 10px;">Save Payment Account</span>

                            <asp:CheckBox ID="tglSavePaymentAccount" runat="server" ClientIDMode="Static"  CssClass="toggle-input-form" Text="<span class='slider'></span>" AutoPostBack="false" /> 

                        </div>

                    </asp:Panel>
                    
                    <%-- Schedule Transaction and Save Payment Form --%>
                    <asp:Panel ID="pnlSuccessInputForm" runat="server" CssClass="success-form">

                        <asp:Panel ID="pnlSavePaymentAccountInput" runat="server" CssClass="collapse save-payment-account" ClientIDMode="Static">

                            <Rock:RockTextBox ID="tbSavePaymentAccountName" runat="server" Label="Name For Saved Payment Account" ClientIDMode="Static" />

                        </asp:Panel>

                        <asp:Panel ID="pnlSuccessScheduleTransactionInput" runat="server" CssClass="collapse schedule-transaction" ClientIDMode="Static">

                            <span class="control-label">Amount</span><br />

                            <asp:Label ID="lblScheduleTransactionAmount" runat="server" CssClass="details-item">$0.00</asp:Label>

                            <Rock:RockDropDownList ID="ddlSuccessScheduleFrequency" runat="server" ClientIDMode="Static" AutoPostBack="false" DataValueField="Id" DataTextField="Name" Label="Frequency" />

                            <span class="control-label">Next Payment Date</span><br />

                            <asp:Label ID="lblSuccessScheduleStartDate" runat="server" CssClass="details-item" ClientIDMode="Static"></asp:Label><br />
                            <br />
                            
                        </asp:Panel>

                        <asp:Button runat="server" ID="btnSaveSuccessInputForm" ClientIDMode="Static" OnClick="btnSaveSuccessInputForm_Click" Text="Save" CssClass="btn btn-primary hidden" Enabled="false" />

                    </asp:Panel>

                    <%-- Manage Transaction Link --%>
                    <asp:HyperLink ID="hlSuccessLink" runat="server" Text="To fully manage your account, please login / register" NavigateUrl="/login" CssClass="success-link" />

                    <%-- Success Details --%>
                    <div class="success-details">

                        <asp:Label runat="server" ID="lblTransactionCodeTitle" CssClass="details-label"></asp:Label>

                        <asp:Label runat="server" ID="lblTransactionCode" CssClass="details-item"></asp:Label>

                        <asp:Label runat="server" ID="lblScheduleId" CssClass="details-item"></asp:Label>

                        <span class="details-label">Name</span>

                        <asp:Label runat="server" ID="lblName" CssClass="details-item"></asp:Label>

                        <span class="details-label">Email</span>

                        <asp:Label runat="server" ID="lblEmail" CssClass="details-item"></asp:Label>

                        <asp:Label runat="server" ID="lblAddressLabel" CssClass="details-label" Visible="false">Address</asp:Label>

                        <asp:Label runat="server" ID="lblAddress" CssClass="details-item" Visible="false"></asp:Label>

                        <asp:Label runat="server" ID="lblFund" CssClass="details-label"></asp:Label>

                        <asp:Label runat="server" ID="lblAmount" CssClass="details-item"></asp:Label>

                        <div class="details-payment">
                            <div class="details-payment-item">

                                <asp:Label runat="server" ID="lblPaymentMethodTitle" CssClass="details-label">Payment Method</asp:Label>

                                <asp:Label runat="server" ID="lblPaymentMethod" CssClass="details-item"></asp:Label>

                            </div>
                            <div class="details-payment-item" style="margin-left: 5px;">

                                <asp:Label runat="server" ID="lblAccountNumberTitle" CssClass="details-label">Account #</asp:Label>

                                <asp:Label runat="server" ID="lblAccountNumber" CssClass="details-item"></asp:Label>

                            </div>
                        </div>

                        <asp:Label runat="server" ID="lblWhenTitle" CssClass="details-label">When</asp:Label>

                        <asp:Label runat="server" ID="lblWhen" CssClass="details-item"></asp:Label>

                    </div>
                </div>

            </asp:Panel>

        </div>

        <asp:HiddenField runat="server" ID ="hfDecepticon" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID ="hfDecepticonMult" ClientIDMode="Static" />
        
    </ContentTemplate>

</asp:UpdatePanel>

<script src="<%= RockPage.ResolveRockUrl( "~/Plugins/church_ccv/Finance/scripts/vendor/moment.min.js", true ) %>"></script>
<script src="<%= RockPage.ResolveRockUrl( "~/Plugins/church_ccv/Finance/scripts/giving.js", true ) %>"></script>

