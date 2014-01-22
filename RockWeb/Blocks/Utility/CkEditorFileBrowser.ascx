﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CkEditorFileBrowser.ascx.cs" Inherits="RockWeb.Blocks.Utility.CkEditorFileBrowser" %>

<table>
    <tr>
        <td>
            <%-- Folders - Separate UpdatePanel so that Tree doesn't get rebuilt on postbacks --%>
            <asp:UpdatePanel ID="upnlFolders" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <asp:LinkButton ID="lbCreateFolder" runat="server" CssClass="btn btn-xs btn-action" Text="Create Folder" OnClick="lbCreateFolder_Click" CausesValidation="false" />
                    <asp:LinkButton ID="lbRenameFolder" runat="server" CssClass="btn btn-xs  btn-action" Text="Rename Folder" OnClick="lbRenameFolder_Click" CausesValidation="false" />
                    <asp:LinkButton ID="lbDeleteFolder" runat="server" CssClass="btn btn-xs btn-action" Text="Delete Folder" OnClientClick="Rock.dialogs.confirmDelete(event, 'folder and all its contents');" OnClick="lbDeleteFolder_Click" CausesValidation="false" />
                    <asp:LinkButton ID="lbRefresh" runat="server" CssClass="btn btn-xs  btn-action" Text="Refresh" OnClick="lbRefresh_Click" CausesValidation="false" />

                    <Rock:NotificationBox ID="nbWarning" runat="server" NotificationBoxType="Warning" Text="Folder not found" Visible="false" />

                    <div style="height: 100%;">
                        <div class="scroll-container scroll-container-vertical scroll-container-picker js-folder-treeview">
                            <div class="scrollbar">
                                <div class="track">
                                    <div class="thumb">
                                        <div class="end"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="viewport">
                                <div class="overview">
                                    <asp:Label ID="lblFolders" CssClass="treeview treeview-items" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    </div>
                    <script type="text/javascript">

                        Sys.Application.add_load(function () {
                            var folderTreeData = $('.js-folder-treeview .treeview').data('rockTree');

                            // init the folder list treeview only if it hasn't been created already
                            if (!folderTreeData) {
                                var selectedFolders = $('#<%=hfSelectedFolder.ClientID%>').val().split(',');
                                // init rockTree on folder (no url option since we are generating off static html)
                                $('.js-folder-treeview .treeview').rockTree({
                                    selectedIds: selectedFolders
                                });
                            }

                            // init scroll bars for folder and file list divs
                            $('.js-folder-treeview').tinyscrollbar({ size: 120, sizethumb: 20 });

                            $('.js-folder-treeview .treeview').on('rockTree:expand rockTree:collapse rockTree:dataBound rockTree:rendered', function (evt) {
                                // update the folder treeview scroll bar
                                $('.js-folder-treeview').tinyscrollbar_update('relative');
                            });

                            $('.js-folder-treeview .treeview').on('rockTree:selected ', function (e, data) {
                                var relativeFolderPath = data;
                                $('#<%=hfSelectedFolder.ClientID%>').val(data);
                                __doPostBack('<%=upnlFiles.ClientID %>', 'folder-selected:' + relativeFolderPath + '');
                            });

                            // init the file list treeview on every load
                            $('.js-file-list .treeview').rockTree({});
                            $('.js-file-list').tinyscrollbar({ size: 120, sizethumb: 20 });

                            // disable/hide actions depending on if a folder is selected
                            var selectedFolderPath = $('#<%=hfSelectedFolder.ClientID%>').val();
                            if (selectedFolderPath && selectedFolderPath != '') {
                                $('#<%=lbRenameFolder.ClientID%>').removeAttr('disabled');
                                $('#<%=lbDeleteFolder.ClientID%>').removeAttr('disabled');
                                $('#<%=iupFileBrowser.ClientID%>').css('visibility', 'visible');
                            }
                            else {
                                $('#<%=lbRenameFolder.ClientID%>').attr('disabled', 'disabled');
                                $('#<%=lbDeleteFolder.ClientID%>').attr('disabled', 'disabled');
                                $('#<%=iupFileBrowser.ClientID%>').css('visibility', 'hidden');
                            }
                        });

                    </script>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>

        <td>
            <%-- Files and Modals --%>
            <asp:UpdatePanel ID="upnlFiles" runat="server">
                <ContentTemplate>
                    <Rock:ModalDialog runat="server" Title="Rename Folder" ID="mdRenameFolder" OnSaveClick="mdRenameFolder_SaveClick" ValidationGroup="vgRenameFolder">
                        <Content>
                            <Rock:RockTextBox runat="server" ID="tbOrigFolderName" Label="Folder Name" ReadOnly="true" />
                            <Rock:RockTextBox runat="server" ID="tbRenameFolderName" Label="New Folder Name" Required="true" ValidationGroup="vgRenameFolder" />
                        </Content>
                    </Rock:ModalDialog>

                    <Rock:ModalDialog runat="server" Title="Create Folder" ID="mdCreateFolder" OnSaveClick="mdCreateFolder_SaveClick" ValidationGroup="vgCreateFolder">
                        <Content>
                            <Rock:RockTextBox runat="server" ID="tbNewFolderName" Label="New Folder Name" Required="true" ValidationGroup="vgCreateFolder" />
                        </Content>
                    </Rock:ModalDialog>

                    <asp:HiddenField ID="hfSelectedFolder" runat="server" />
                    <Rock:ImageUploader ID="iupFileBrowser" runat="server" />

                    <div>
                        <div class="scroll-container scroll-container-vertical scroll-container-picker js-file-list">
                            <div class="scrollbar">
                                <div class="track">
                                    <div class="thumb">
                                        <div class="end"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="viewport">
                                <div class="overview">
                                    <asp:Label ID="lblFiles" CssClass="treeview treeview-items" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="js-filebrowser-result">
                        <asp:HiddenField ID="hfResultValue" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>

