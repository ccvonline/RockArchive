﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Relationships.ascx.cs" Inherits="RockWeb.Blocks.Crm.PersonDetail.Relationships" %>
<asp:UpdatePanel ID="upEditFamily" runat="server">
    <ContentTemplate>

        <section class="panel panel-persondetails">

            <div class="panel-heading rollover-container clearfix">
                <h3 class="panel-title pull-left">
                    <asp:PlaceHolder ID="phGroupTypeIcon" runat="server"></asp:PlaceHolder>
                    <asp:Literal ID="lGroupName" runat="server"></asp:Literal></h3>
                <asp:PlaceHolder ID="phEditActions" runat="server">
                    <div class="actions rollover-item pull-right">
                        <asp:LinkButton ID="lbAdd" runat="server" CssClass="edit" Text="Add Relationship" OnClick="lbAdd_Click" CausesValidation="false"><i class="fa fa-plus"></i></asp:LinkButton>
                    </div>
                </asp:PlaceHolder>
            </div>

            <div class="panel-body">
                <ul class="personlist">
                    <asp:Repeater ID="rGroupMembers" runat="server">
                        <ItemTemplate>
                            <li>
                                <Rock:PersonLink runat="server"
                                    PersonId='<%# Eval("PersonId") %>'
                                    PersonName='<%# Eval("Person.FirstLastName") %>'
                                    Role='<%# ShowRole ? Eval("GroupRole.Name") : "" %>'
                                    PhotoId='<%# Eval("Person.PhotoId") %>' />
                                <div class="actions pull-right">
                                    <asp:LinkButton ID="lbEdit" runat="server" CssClass="edit" Text="Edit Relationship"
                                         CommandName="EditRole" CommandArgument='<%# Eval("Id") %>'><i class="fa fa-pencil"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lbRemove" runat="server" CssClass="edit remove-relationship" Text="Remove Relationship" 
                                        CommandName="RemoveRole"  CommandArgument='<%# Eval("Id") %>'><i class="fa fa-times"></i></asp:LinkButton>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>

            <Rock:ModalDialog ID="modalAddPerson" runat="server" Title="Add Relationship" Content-Height="380" ValidationGroup="NewRelationship" >
                <Content>

                    <div id="divExistingPerson" runat="server">
                        <fieldset>
                            <Rock:GroupRolePicker ID="grpRole" runat="server" Label="Relationship Type" ValidationGroup="NewRelationship"  />
                            <Rock:PersonPicker2 ID="ppPerson" runat="server" ValidationGroup="NewRelationship" />
                        </fieldset>
                    </div>

                </Content>
            </Rock:ModalDialog>

        </section>

    </ContentTemplate>
</asp:UpdatePanel>
