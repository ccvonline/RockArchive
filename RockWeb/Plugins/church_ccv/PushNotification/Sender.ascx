
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Sender.ascx.cs" Inherits="RockWeb.Plugins.church_ccv.PushNotification.Sender"%>

<asp:UpdatePanel ID="upnlSettings" runat="server">
    <ContentTemplate>
        <div ID="error-panel" class="alert alert-danger" style="visibility: hidden; position: fixed; z-index:9999; left: 50%; top: 50%; transform: translate( -50%, -50%);">
            <p><i class="fa fa-times"></i> Fill in the fields colored red and try again.</p>
        </div>

        <div class ="panel panel-block">
            <div class="panel-heading">
                <div class="row col-sm-4">
                    <h4 class="panel-title">Sender</h4>
                </div>
            </div>

            <div class="panel-body">
                <div class="persona-editable-item">
                    <asp:Literal runat="server"><p class="title">Title</p></asp:Literal>
                    <Rock:RockTextBox runat="server" ClientIDMode="Static" ID="tbTitle"></Rock:RockTextBox>
                </div>
                <br/>
                <div class="persona-editable-item">
                    <asp:Literal runat="server"><p class="title">Message</p></asp:Literal>
                    <Rock:RockTextBox runat="server" ClientIDMode="Static" ID="tbMessage"></Rock:RockTextBox>
                </div>
                <div class="persona-editable-item">
                    <asp:Literal runat="server"><p class="title">Action</p></asp:Literal>
                    <Rock:RockTextBox runat="server" ClientIDMode="Static" ID="tbAction"></Rock:RockTextBox>
                </div>
                <br/>
                <div class="persona-editable-item">
                    <asp:Literal runat="server"><p class="title">Action Data</p></asp:Literal>
                    <Rock:RockTextBox runat="server" ClientIDMode="Static" ID="tbActionData"></Rock:RockTextBox>
                </div>
                <br/>
                <asp:LinkButton ID="btnSend" ClientIDMode="Static" runat="server" Text="Send To Known People" CssClass="btn btn-primary" OnClientClick="return validateFields();" OnClick="btnSendKnown_Click" />
                <asp:LinkButton ID="LinkButton1" ClientIDMode="Static" runat="server" Text="Send To All" CssClass="btn btn-primary" OnClientClick="return validateFields();" OnClick="btnSendAll_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<style>
    .persona-editable-item{
        display: flex;
        justify-content: flex-start;
    }
    
    .persona-editable-item .title {
        min-width: 150px;
    }

    .persona-types {
    }

    .persona-type {
        display: flex;
        flex-direction: column;
        margin-top: 25px;

        border-width: 1px;
        border-color: #b8b2aa;
        border-style: solid;
        border-radius: 4px;
        padding: 5px;
    }

    .persona-type-header {
    }

    .persona-type-template-item {
        display: flex;
        flex-direction: row;
    }

    .form-control {
        width: 500px;
    }

    .error {
        border-width: 3px;
        border-color: #9d3f3d;
        border-style: solid;
    }
</style>

<script>
    function pageLoad() {

        // setup event handlers for input that clear the red error box around fields.
        $("#tbTitle").on('input', function () {
            $(this).removeClass("error");
        });

        $("#tbMessage").on('input', function () {
            $(this).removeClass("error");
        });
    }

    function validateFields() {
        var success = true;

        // validate the title field
        if (validateField("#tbTitle") == false) {
            success = false;
        }

        // validate the message field
        if (validateField("#tbMessage") == false) {
            success = false;
        }

        if (success == false) {
            $("#error-panel").css("visibility", "visible");
            setTimeout(hideAlert, 5000);
        }

        return success;
    }

    function validateField(fieldId) {
        var fieldVal = $(fieldId).val();

        if (fieldVal) {
            $(fieldId).removeClass("error");
            return true;
        }
        else {
            $(fieldId).addClass("error");
            return false;
        }
    }

    function hideAlert() {
        $("#error-panel").css("visibility", "hidden");
    }
</script>