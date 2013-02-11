﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WorkflowTypeDetail.ascx.cs" Inherits="RockWeb.Blocks.Administration.WorkflowTypeDetail" %>

<asp:UpdatePanel ID="upDetail" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server" Visible="false">
            <asp:HiddenField ID="hfWorkflowTypeId" runat="server" />

            <asp:ValidationSummary ID="vsDetails" runat="server" CssClass="alert alert-error" />

            <div id="pnlEditDetails" runat="server" class="well">

                <fieldset>
                    <legend>
                        <asp:Literal ID="lActionTitle" runat="server" />
                    </legend>

                    <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                    <div class="row-fluid">
                        <div class="span6">
                            <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="Rock.Model.WorkflowType, Rock" PropertyName="Name" />
                            <Rock:DataTextBox ID="tbDescription" runat="server" SourceTypeName="Rock.Model.WorkflowType, Rock" PropertyName="Description" TextMode="MultiLine" Rows="4" />
                            <Rock:DataTextBox ID="tbWorkTerm" runat="server" SourceTypeName="Rock.Model.WorkflowType, Rock" PropertyName="WorkTerm" LabelText="Work Term" />
                            <Rock:LabeledDropDownList ID="ddlLoggingLevel" runat="server" LabelText="Logging Level" />
                            <Rock:LabeledCheckBox ID="cbIsActive" runat="server" LabelText="Active" />
                        </div>
                        <div class="span6">
                            <Rock:DataDropDownList ID="ddlCategory" runat="server" DataTextField="Name" DataValueField="Id" SourceTypeName="Rock.Model.Category, Rock" PropertyName="Name" LabelText="Category" />
                            <Rock:DataTextBox ID="tbProcessingInterval" runat="server" SourceTypeName="Rock.Model.WorkflowType, Rock" PropertyName="ProcessingIntervalSeconds" LabelText="Processing Interval (seconds)" />
                            <Rock:LabeledCheckBox ID="cbIsPersisted" runat="server" LabelText="Persisted" />
                            <Rock:DataTextBox ID="tbOrder" runat="server" SourceTypeName="Rock.Model.WorkflowType, Rock" PropertyName="Order" LabelText="Order" Required="true" />
                        </div>
                    </div>
                </fieldset>

                <div class="actions">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>

            </div>

            <fieldset id="fieldsetViewDetails" runat="server">
                <legend>
                    <asp:Literal ID="lReadOnlyTitle" runat="server" />
                </legend>
                <asp:Literal ID="lblActiveHtml" runat="server" />
                <div class="well">
                    <div class="row-fluid">
                        <div class="span6">
                            <Rock:NotificationBox ID="NotificationBox1" runat="server" NotificationBoxType="Info" />
                            <asp:Literal ID="lblMainDetails" runat="server" />
                        </div>
                        <div class="span6">
                            <asp:Panel ID="pnlAttributeTypes" runat="server">
                                <Rock:ModalAlert ID="mdGridWarningAttributes" runat="server" />
                                <Rock:Grid ID="gWorkflowTypeAttributes" runat="server" AllowPaging="false" DisplayType="Light">
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="Workflow Attributes" />
                                        <Rock:EditField OnClick="gWorkflowTypeAttributes_Edit" />
                                        <Rock:DeleteField OnClick="gWorkflowTypeAttributes_Delete" />
                                    </Columns>
                                </Rock:Grid>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <h4>Activities</h4>
                        <asp:Literal ID="lblWorkflowActivitiesReadonly" runat="server" />
                    </div>
                    <div class="actions">
                        <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-mini" OnClick="btnEdit_Click" />
                    </div>
                </div>
            </fieldset>

        </asp:Panel>

        <asp:Panel ID="pnlWorkflowTypeAttributes" runat="server" Visible="false">
            <RockWeb:RockAttributeEditor ID="edtWorkflowTypeAttributes" runat="server" OnSaveClick="btnSaveWorkflowTypeAttribute_Click" OnCancelClick="btnCancelWorkflowTypeAttribute_Click" />
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
