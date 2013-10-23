﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GroupRoleList.ascx.cs" Inherits="RockWeb.Blocks.Crm.GroupRoleList" %>

<asp:UpdatePanel ID="upGroupRoles" runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfGroupTypeId" />
        <div id="pnlGroupRoles" runat="server">
            <h4>Roles</h4>
            <Rock:ModalAlert ID="mdGridWarning" runat="server" />
            <Rock:Grid ID="gGroupRoles" runat="server" OnRowSelected="gGroupRoles_Edit">
                <Columns>
                    <Rock:ReorderField />
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="MinCount" HeaderText="Minumum" />
                    <asp:BoundField DataField="MaxCount" HeaderText="Maximum" />
                    <Rock:BoolField DataField="IsSystem" HeaderText="System" />
                    <Rock:DeleteField OnClick="gGroupRoles_Delete" />
                </Columns>
            </Rock:Grid>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
