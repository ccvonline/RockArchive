﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PageProperties.ascx.cs" Inherits="RockWeb.Blocks.Administration.PageProperties" %>

<asp:UpdatePanel id="upPanel" runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="lbExport" />
        <asp:PostBackTrigger ControlID="lbImport" />
    </Triggers>
<ContentTemplate>
 
    <asp:PlaceHolder ID="phContent" runat="server">

        <ul class="nav nav-pills" >
            <asp:Repeater ID="rptProperties" runat="server" >
                <ItemTemplate >
                    <li class='<%# GetTabClass(Container.DataItem) %>'>
                        <asp:LinkButton ID="lbProperty" runat="server" Text='<%# Container.DataItem %>' OnClick="lbProperty_Click">
                        </asp:LinkButton> 
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>

        <div class="tabContent" >

            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-error block-message error"/>
            
            <asp:Panel ID="pnlBasicProperty" runat="server" Visible="true" >
                <div class="row">
                    <div class="span6">
                        <fieldset>
                            <Rock:DataDropDownList ID="ddlParentPage" runat="server" LabelText="Parent Page" SourceTypeName="Rock.Model.Page, Rock" PropertyName="ParentPageId"/>
                            <Rock:DataTextBox ID="tbPageName" runat="server" SourceTypeName="Rock.Model.Page, Rock" PropertyName="Name" />
                            <Rock:DataTextBox ID="tbPageTitle" runat="server" SourceTypeName="Rock.Model.Page, Rock" PropertyName="Title" Help="The text to be displayed in menus and page headings"/>
                            <Rock:DataDropDownList ID="ddlLayout" runat="server" SourceTypeName="Rock.Model.Page, Rock" PropertyName="Layout"/>
                        </fieldset>
                    </div>
                    <div class="span6">
                        <fieldset>
                            <Rock:DataTextBox ID="tbDescription" runat="server" TextMode="MultiLine" Rows="3" SourceTypeName="Rock.Model.Page, Rock" PropertyName="Description" />
                        </fieldset>
                    </div>
                </div>
            </asp:Panel>

             <asp:Panel ID="pnlMenuDisplay" runat="server" Visible="false" >
                <fieldset>
                    <Rock:DataDropDownList ID="ddlMenuWhen" runat="server" LabelText="Display When" SourceTypeName="Rock.Model.Page, Rock" PropertyName="DisplayInNavWhen"/>
                    <Rock:LabeledCheckBox ID="cbMenuDescription" runat="server" Text="Show Description"/>
                    <Rock:LabeledCheckBox ID="cbMenuChildPages" runat="server" Text="Show Child Pages" Help="Should child pages be displayed in a menu"/>
                    <Rock:LabeledCheckBox ID="cbMenuIcon" runat="server" Text="Show Icon"/>
                    <div class="control-group">
                        <label class="control-label">Icon Image</label>
                        <div class="controls">
                            <Rock:ImageSelector ID="imgIcon" runat="server" />
                        </div>
                    </div>
                    <Rock:DataTextBox ID="tbIconCssClass" runat="server" SourceTypeName="Rock.Model.Page, Rock" PropertyName="IconCssClass"/>
                </fieldset>
            </asp:Panel>

            <asp:Panel ID="pnlAdvancedSettings" runat="server" Visible="false" >
                <div class="row">
                    <div class="span6">
                        <fieldset>
                            <Rock:LabeledCheckBox ID="cbRequiresEncryption" runat="server" Text="Force SSL"/>
                            <Rock:LabeledCheckBox ID="cbEnableViewState" runat="server" Text="Enable ViewState"/>
                            <Rock:LabeledCheckBox ID="cbIncludeAdminFooter" runat="server" Text="Allow Configuration"/>
                            <Rock:DataTextBox ID="tbCacheDuration" runat="server" LabelText="Cache Duration" SourceTypeName="Rock.Model.Page, Rock" PropertyName="OutputCacheDuration"/>
                        </fieldset>
                    </div>
                    <div class="span6">
                        <fieldset>                
                            <Rock:DataTextBox ID="tbPageRoute" runat="server" TextMode="MultiLine" Rows="3" LabelText="Page Routes" SourceTypeName="Rock.Model.Page, Rock" PropertyName="PageRoutes"  />
                        </fieldset>
                        <asp:PlaceHolder ID="phContextPanel" runat="server">
                            <fieldset>
                                <legend>Context Parameters</legend>
                                <p>There are one or more blocks on this page that need to load objects based on a 'context' parameter.  
                                Please enter the route parameter name or query string parameter name that will contain the id for 
                                each of the objects below.</p>
                                <asp:PlaceHolder ID="phContext" runat="server"></asp:PlaceHolder>
                            </fieldset>
                        </asp:PlaceHolder>
                    </div>
                </div>
            </asp:Panel>
            
            <asp:Panel ID="pnlImportExport" runat="server" Visible="False">
                <div class="row">
                    <div class="span6">
                        <fieldset>
                            <legend>Import Pages</legend>
                            <asp:Panel runat="server" ID="pnlImportSuccess" CssClass="row-fluid" Visible="False">
                                <div class="span12 alert alert-success">
                                    <p><i class="icon-bolt"></i> <strong>Sweet!</strong> Your package was imported successfully.</p>
                                    <asp:Repeater ID="rptImportWarnings" runat="server" Visible="False">
                                        <HeaderTemplate>
                                            <p><i class="icon-warning-sign"></i> Just a quick head's up...</p>
                                            <ul>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li><%# Container.DataItem %></li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </asp:Panel>
                            <asp:Repeater runat="server" ID="rptImportErrors" Visible="False">
                                <HeaderTemplate>
                                    <div class="row-fluid">
                                        <div class="span12 alert alert-error">
                                            <p><strong>Uh oh!</strong> Looks like we ran into some trouble importing the package.</p>
                                            <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li><%# Container.DataItem %></li>
                                </ItemTemplate>
                                <FooterTemplate>
                                            </ul>
                                        </div>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:FileUpload runat="server" ID="fuImport" CssClass="input-small" />
                            <asp:LinkButton runat="server" ID="lbImport" CssClass="btn btn-small" OnClick="lbImport_Click">
                                <i class="icon-arrow-up"></i> Import
                            </asp:LinkButton>
                        </fieldset>
                    </div>
                    <div class="span6">
                        <fieldset>
                            <legend>Export Pages</legend>
                            <label class="checkbox">
                                <asp:CheckBox runat="server" ID="cbExportChildren" />
                                Export child pages?
                            </label>
                            <asp:LinkButton runat="server" ID="lbExport" OnClick="lbExport_Click" CssClass="btn btn-small">
                                <i class="icon-download-alt"></i> Export
                            </asp:LinkButton>
                        </fieldset>
                    </div>
                </div>
            </asp:Panel>

            <asp:PlaceHolder id="phAttributes" runat="server" />

        </div>

    </asp:PlaceHolder>
    
</ContentTemplate>
</asp:UpdatePanel>



