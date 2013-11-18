﻿ <%@ Control Language="C#" AutoEventWireup="true" CodeFile="Components.ascx.cs" Inherits="RockWeb.Blocks.Core.Components" %>

<asp:UpdatePanel runat="server">
<ContentTemplate>

    <Rock:ModalAlert ID="mdAlert" runat="server" />

    <asp:Panel ID="pnlList" runat="server" Visible="true" >
        
        <Rock:GridFilter ID="rFilter" runat="server">
            <Rock:RockTextBox ID="tbName" runat="server" Label="Name" />
            <Rock:RockTextBox ID="tbDescription" runat="server" Label="Description" />
            <Rock:RockRadioButtonList ID="rblActive" runat="server" Label="Active" RepeatDirection="Horizontal">
                <asp:ListItem Value="" Text="All" />
                <asp:ListItem Value="Yes" Text="Yes" />
                <asp:ListItem Value="No" Text="No" />
            </Rock:RockRadioButtonList>
        </Rock:GridFilter>

        <Rock:Grid ID="rGrid" runat="server" EmptyDataText="No Components Found" OnRowSelected="rGrid_Edit">
            <Columns>
                <Rock:ReorderField />
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <Rock:BoolField DataField="IsActive" HeaderText="Active" />
                <asp:TemplateField>
                    <HeaderStyle CssClass="span1" />
                    <ItemStyle HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <a id="aSecure" runat="server" class="btn btn-sm btn-security" height="500px"><i class="fa fa-lock"></i></a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Rock:Grid>

    </asp:Panel>

    <asp:Panel ID="pnlDetails" runat="server" Visible="false" CssClass="panel panel-default">
        <div class="panel-body">
            <asp:ValidationSummary runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

            <div class="banner">
                <h1><asp:Literal ID="lProperties" runat="server"></asp:Literal></h1>
            </div>

            <fieldset>
                <asp:PlaceHolder ID="phProperties" runat="server"></asp:PlaceHolder>
            </fieldset>

            <div class="actions">
                <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" onclick="btnSave_Click" />
                <asp:LinkButton id="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
            </div>
            </div>
    </asp:Panel>

</ContentTemplate>
</asp:UpdatePanel>
