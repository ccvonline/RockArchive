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

        <asp:ValidationSummary ID="ValidationSummary" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

        <div class="nav navbar nav-pagelist">
            <ul id="ulChannels" runat="server" class="nav nav-pills">
                <asp:Repeater ID="rptChannels" runat="server">
                    <ItemTemplate>
                        <li class='<%# (int)Eval("Key") == ChannelEntityTypeId ? "active" : "" %>'>
                            <asp:LinkButton ID="lbChannel" runat="server" Text='<%# Eval("Value") %>' CommandArgument='<%# Eval("Key") %>' OnClick="lbChannel_Click" CausesValidation="false">
                            </asp:LinkButton>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        
        <div class="panel panel-widget recipients">
            <div class="panel-heading clearfix">
                <div class="control-label pull-left">
                    To: <asp:Literal ID="lNumRecipients" runat="server" />
                </div> 
                    
                <Rock:PersonPicker ID="ppAddPerson" runat="server" CssClass="pull-right" PersonName="Add Person" OnSelectPerson="ppAddPerson_SelectPerson" />
                <asp:CustomValidator ID="valRecipients" runat="server" OnServerValidate="valRecipients_ServerValidate" Display="None" ErrorMessage="At least one recipient is required." />
                
             </div>   
                
             <div class="panel-body">

                    <ul class="recipients">
                        <asp:Repeater ID="rptRecipients" runat="server" OnItemCommand="rptRecipients_ItemCommand" OnItemDataBound="rptRecipients_ItemDataBound">
                            <ItemTemplate>
                                <li class='recipient <%# Eval("Status").ToString().ToLower() %>'><asp:Literal id="lRecipientName" runat="server"></asp:Literal> <asp:LinkButton ID="lbRemoveRecipient" runat="server" CommandArgument='<%# Eval("PersonId") %>' CausesValidation="false"><i class="fa fa-times"></i></asp:LinkButton></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>

                    <div class="pull-right">
                        <asp:LinkButton ID="lbShowAllRecipients" runat="server" CssClass="btn btn-action btn-xs" Text="Show All" OnClick="lbShowAllRecipients_Click" CausesValidation="false"/>
                        <asp:LinkButton ID="lbRemoveAllRecipients" runat="server" Text="Remove All Pending Recipients" CssClass="remove-all-recipients btn btn-action btn-xs" OnClick="lbRemoveAllRecipients_Click" CausesValidation="false"/>
                    </div>

            </div>
        </div>

        <Rock:RockDropDownList ID="ddlTemplate" runat="server" Label="Template" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged" />

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

        <script type="text/javascript">
            // change approval status to pending if any values are changed
            Sys.Application.add_load(function () {
                $('.recipient span').tooltip();
            })
        </script>


    </ContentTemplate>
</asp:UpdatePanel>


