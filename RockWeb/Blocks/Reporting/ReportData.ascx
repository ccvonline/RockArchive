﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReportData.ascx.cs" Inherits="RockWeb.Blocks.Reporting.ReportData" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <Rock:NotificationBox ID="nbConfigurationWarning" runat="server" NotificationBoxType="Warning" Visible="false" />

        <%-- View Panel --%>
        <asp:Panel ID="pnlView" runat="server">
            <Rock:NotificationBox ID="nbError" runat="server" NotificationBoxType="Danger" Visible="false" />

            <asp:PlaceHolder ID="phFilters" runat="server" />

            <div class="actions">
                <asp:LinkButton ID="btnRun" runat="server" AccessKey="m" Text="Run" CssClass="btn btn-primary" OnClick="btnRun_Click" />
            </div>

            <h4>Results</h4>
            <Rock:NotificationBox ID="nbReportErrors" runat="server" NotificationBoxType="Info" />
            <div class="grid">
                <Rock:Grid ID="gReport" runat="server" AllowSorting="true" EmptyDataText="No Results" />
            </div>


        </asp:Panel>

        <%-- Configuration Panel --%>
        <asp:Panel ID="pnlConfigure" runat="server" Visible="false">
            <Rock:ModalDialog ID="mdConfigure" runat="server" ValidationGroup="vgConfigure" OnSaveClick="mdConfigure_SaveClick">
                <Content>
                    <Rock:RockDropDownList ID="ddlReport" runat="server" Label="Report" Help="Select the report to present to the user. Then set which of the report's dataview's filters to show." Required="false" ValidationGroup="vgConfigure" OnSelectedIndexChanged="ddlReport_SelectedIndexChanged" AutoPostBack="true" />
                    <Rock:NotificationBox ID="nbMultipleFilterGroupsWarning" runat="server" NotificationBoxType="Warning" Text="This report has multiple filter groups. This block currently only supports non-grouped filters" Dismissable="true" Visible="false" />
                    <Rock:HelpBlock ID="hbDataFilters" runat="server" Text="Select which filters that will be visible to the user.  If Configurable is selected for a visible filter, the user will be able to change the filter, otherwise, the filter will presented as checkbox where user can choose to use the filter or not." />
                    <Rock:Grid ID="grdDataFilters" runat="server" DisplayType="Light" DataKeyNames="Guid">
                        <Columns>
                            <Rock:SelectField HeaderText="Show as a Filter" DataSelectedField="ShowAsFilter" ShowSelectAll="false" ItemStyle-CssClass="js-select-show-filter" />
                            <Rock:SelectField HeaderText="Configurable" DataSelectedField="IsConfigurable" ShowSelectAll="false" ItemStyle-CssClass="js-select-configure-filter" />
                            <asp:BoundField DataField="Title" HeaderText="Title" />
                            <asp:BoundField DataField="Summary" HeaderText="Summary" />
                        </Columns>
                    </Rock:Grid>
                </Content>
            </Rock:ModalDialog>
        </asp:Panel>

        <script>
            var updateConfigureCheckbox = function (t) {
                var $cbConfigurable = $(t).closest('tr').find('.js-select-configure-filter input');
                if ($(t).is(':checked')) {
                    $cbConfigurable.removeAttr('disabled');
                }
                else {
                    $cbConfigurable.removeAttr('checked');
                    $cbConfigurable.attr('disabled', 'disabled');
                }
            }

            Sys.Application.add_load(function () {
                $('.js-select-show-filter input').each(function (i, cb) {
                    updateConfigureCheckbox(cb)
                });

                $('.js-select-show-filter input').on('click', function () {
                    updateConfigureCheckbox(this)
                });
            })
        </script>


    </ContentTemplate>
</asp:UpdatePanel>
