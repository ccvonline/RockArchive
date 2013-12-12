﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Communication.ascx.cs" Inherits="RockWeb.Blocks.Communication.Communication" %>

<asp:UpdatePanel ID="upPanel" runat="server">
    <ContentTemplate>
 
        <div class="banner">
            <h1><asp:Literal ID="lTitle" runat="server"></asp:Literal></h1>
            <asp:Literal ID="lStatus" runat="server" />
            <asp:Literal ID="lRecipientStatus" runat="server" />
        </div>

        <asp:Panel ID="pnlEdit" runat="server">

        <asp:HiddenField ID="hfCommunicationId" runat="server" />
        <asp:HiddenField ID="hfChannelId" runat="server" />

        <ul class="nav nav-pills">
            <asp:Repeater ID="rptChannels" runat="server">
                <ItemTemplate>
                    <li class='<%# (int)Eval("Key") == ChannelEntityTypeId ? "active" : "" %>'>
                        <asp:LinkButton ID="lbChannel" runat="server" Text='<%# Eval("Value") %>' CommandArgument='<%# Eval("Key") %>' OnClick="lbChannel_Click" CausesValidation="false">
                        </asp:LinkButton>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
        
        <asp:ValidationSummary ID="ValidationSummary" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

        <div class="panel panel-widget recipients">
            <div class="panel-heading clearfix">
                <div class="control-label pull-left">
                    To: <asp:Literal ID="lNumRecipients" runat="server" />
                </div> 
                    
                <Rock:PersonPicker ID="ppAddPerson" runat="server" CssClass="pull-right" PersonName="Add Person" OnSelectPerson="ppAddPerson_SelectPerson" />
                <asp:CustomValidator ID="valRecipients" runat="server" OnServerValidate="valRecipients_ServerValidate" Display="None" ErrorMessage="At least one recipient is required." />
                
             </div>   
                
             <div class="panel-body">
                <div class="recipient">
                    <ul class="recipient-content">
                        <asp:Repeater ID="rptRecipients" runat="server" OnItemCommand="rptRecipients_ItemCommand" OnItemDataBound="rptRecipients_ItemDataBound">
                            <ItemTemplate>
                                <li class='<%# Eval("Status").ToString().ToLower() %>'><%# Eval("PersonName") %> <asp:LinkButton ID="lbRemoveRecipient" runat="server" CommandArgument='<%# Eval("PersonId") %>' CausesValidation="false"><i class="fa fa-times"></i></asp:LinkButton></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>

                    <div class="pull-right">
                        <asp:LinkButton ID="lbShowAllRecipients" runat="server" CssClass="btn btn-action btn-xs" Text="Show All" OnClick="lbShowAllRecipients_Click" CausesValidation="false"/>
                        <asp:LinkButton ID="lbRemoveAllRecipients" runat="server" Text="Remove All Pending Recipients" CssClass="remove-all-recipients btn btn-action btn-xs" OnClick="lbRemoveAllRecipients_Click" CausesValidation="false"/>
                    </div>
                </div>
            </div>
        </div>

        

        <asp:PlaceHolder ID="phContent" runat="server" />

        <Rock:DateTimePicker ID="dtpFutureSend" runat="server" Label="Delay Send Until" SourceTypeName="Rock.Model.Communication" PropertyName="FutureSendDateTime" />

        <div class="actions">
            <asp:LinkButton ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
            <asp:LinkButton ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-primary" OnClick="btnApprove_Click" />
            <asp:LinkButton ID="btnDeny" runat="server" Text="Deny" CssClass="btn btn-link" OnClick="btnDeny_Click" />
            <asp:LinkButton ID="btnSave" runat="server" Text="Save as Draft" CssClass="btn btn-link" OnClick="btnSave_Click" />
            <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel Send" CssClass="btn btn-link" OnClick="btnCancel_Click" />
            <asp:LinkButton ID="btnCopy" runat="server" Text="Copy Communication" CssClass="btn btn-link" OnClick="btnCopy_Click" CausesValidation="false" />
        </div>

        </asp:Panel>

        <asp:Panel ID="pnlResult" runat="server" Visible="false">
            <Rock:NotificationBox ID="nbResult" runat="server" NotificationBoxType="Success" />
            <br />
            <asp:HyperLink ID="hlViewCommunication" runat="server" Text="View Communication" />
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>


