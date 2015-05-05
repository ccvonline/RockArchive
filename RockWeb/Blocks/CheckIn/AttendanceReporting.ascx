﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttendanceReporting.ascx.cs" Inherits="RockWeb.Blocks.CheckIn.AttendanceReporting" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <div class="panel panel-block">
            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-check-square-o"></i>Attendance Analysis</h1>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-4">
                        <div class="actions margin-b-md">
                            <asp:LinkButton ID="btnApply" runat="server" CssClass="btn btn-primary" Text="Apply" ToolTip="Update the chart" OnClick="btnApply_Click" />
                        </div>

                        <Rock:SlidingDateRangePicker ID="drpSlidingDateRange" runat="server" Label="Date Range" />

                        <Rock:RockControlWrapper ID="rcwGraphBy" runat="server" Label="Graph By">
                            <div class="controls">
                                <div class="js-graph-by">
                                    <Rock:HiddenFieldWithClass ID="hfGraphBy" CssClass="js-hidden-selected" runat="server" />
                                    <div class="btn-group">
                                        <asp:HyperLink ID="btnGraphByTotal" runat="server" CssClass="btn btn-default active" Text="Total" data-val="0" />
                                        <asp:HyperLink ID="btnGraphByGroup" runat="server" CssClass="btn btn-default" Text="Group" data-val="1" />
                                        <asp:HyperLink ID="btnGraphByCampus" runat="server" CssClass="btn btn-default" Text="Campus" data-val="2" />
                                        <asp:HyperLink ID="btnGraphByTime" runat="server" CssClass="btn btn-default" Text="Schedule" data-val="3" />
                                    </div>
                                </div>
                            </div>
                        </Rock:RockControlWrapper>

                        <Rock:RockControlWrapper ID="rcwGroupBy" runat="server" Label="Group By">
                            <div class="controls">
                                <div class="js-group-by">
                                    <Rock:HiddenFieldWithClass ID="hfGroupBy" CssClass="js-hidden-selected" runat="server" />
                                    <div class="btn-group">
                                        <asp:HyperLink ID="btnGroupByWeek" runat="server" CssClass="btn btn-default active" Text="Week" data-val="0" />
                                        <asp:HyperLink ID="btnGroupByMonth" runat="server" CssClass="btn btn-default" Text="Month" data-val="1" />
                                        <asp:HyperLink ID="btnGroupByYear" runat="server" CssClass="btn btn-default" Text="Year" data-val="2" />
                                    </div>
                                </div>
                            </div>
                        </Rock:RockControlWrapper>

                        <Rock:CampusesPicker ID="cpCampuses" runat="server" Label="Campuses" />


                        <Rock:NotificationBox ID="nbGroupTypeWarning" runat="server" NotificationBoxType="Warning" Text="Please select a group type template in the block settings." Dismissable="true" />
                        <h4>Group</h4>
                        <ul class="rocktree">

                            <asp:Repeater ID="rptGroupTypes" runat="server" OnItemDataBound="rptGroupTypes_ItemDataBound">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:Repeater>

                        </ul>

                    </div>
                    <div class="col-md-8">

                        <div class="controls">
                            <div class="js-show-by">
                                <Rock:HiddenFieldWithClass ID="hfShowBy" CssClass="js-hidden-selected" runat="server" />
                                <div class="btn-group">
                                    <asp:LinkButton ID="btnShowByChart" runat="server" CssClass="btn btn-default active" data-val="0" OnClick="btnShowByChart_Click">
                                            <i class="fa fa-line-chart"></i> Chart
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnShowByAttendees" runat="server" CssClass="btn btn-default" data-val="1" OnClick="btnShowByAttendees_Click">
                                            <i class="fa fa-users"></i> Attendees
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>


                        <asp:Panel ID="pnlShowByChart" runat="server">
                            <Rock:LineChart ID="lcAttendance" runat="server" DataSourceUrl="" Title="" Subtitle="" ChartHeight="300" />
                            <div class="row margin-t-sm">
                                <div class="col-md-12">
                                    <div class="pull-right">
                                        <asp:LinkButton ID="lShowGrid" runat="server" CssClass="btn btn-default btn-xs margin-b-sm" Text="Show Data <i class='fa fa-chevron-down'></i>" ToolTip="Show Data" OnClick="lShowGrid_Click" />
                                    </div>
                                </div>
                            </div>
                            <asp:Panel ID="pnlGrid" runat="server" Visible="false">

                                <div class="grid">
                                    <Rock:Grid ID="gChartAttendance" runat="server" AllowSorting="true" DataKeyNames="DateTimeStamp,SeriesId" RowItemText="Attendance Summary">
                                        <Columns>
                                            <Rock:DateField DataField="DateTime" HeaderText="Date" SortExpression="DateTimeStamp" />
                                            <Rock:RockBoundField DataField="SeriesId" HeaderText="Series" SortExpression="SeriesId" />
                                            <Rock:RockBoundField DataField="YValue" HeaderText="Count" SortExpression="YValue" />
                                        </Columns>
                                    </Rock:Grid>
                                </div>
                            </asp:Panel>
                        </asp:Panel>

                        <asp:Panel ID="pnlShowByAttendees" runat="server">
                            <div class="panel">
                                <div class="grid-filter">
                                    <div class="controls pull-right">
                                        <div class="js-view-by">
                                            <Rock:HiddenFieldWithClass ID="hfViewBy" CssClass="js-hidden-selected" runat="server" />
                                            <div class="btn-group">
                                                <asp:HyperLink ID="btnViewAttendees" runat="server" CssClass="btn btn-default active" data-val="0">
                                                    Attendees
                                                </asp:HyperLink>
                                                <asp:HyperLink ID="btnViewParentsOfAttendees" runat="server" CssClass="btn btn-default" data-val="1">
                                                    Parents of Attendees
                                                </asp:HyperLink>
                                            </div>
                                        </div>
                                    </div>
                                    <Rock:RockControlWrapper ID="rcwAttendeesFilter" runat="server" Label="Filter">
                                        <p>
                                            <Rock:RockRadioButton ID="radAllAttendees" runat="server" GroupName="grpFilterBy" Text="All Attendees" CssClass="js-attendees-all" />
                                        </p>
                                        <p>
                                            <Rock:RockRadioButton ID="radByVisit" runat="server" GroupName="grpFilterBy" Text="By Visit" CssClass="js-attendees-by-visit" />
                                            <div class="js-attendees-by-visit-options padding-l-lg form-inline">
                                                <Rock:RockDropDownList ID="ddlNthVisit" CssClass="input-width-md" runat="server">
                                                    <asp:ListItem />
                                                    <asp:ListItem Text="1st" Value="1" />
                                                    <asp:ListItem Text="2nd" Value="2" />
                                                    <asp:ListItem Text="3rd" Value="3" />
                                                    <asp:ListItem Text="4th" Value="4" />
                                                    <asp:ListItem Text="5th" Value="5" />
                                                </Rock:RockDropDownList>
                                                <span>visit</span>
                                            </div>
                                        </p>
                                        <p>
                                            <Rock:RockRadioButton ID="radByPattern" runat="server" GroupName="grpFilterBy" Text="Pattern" CssClass="js-attendees-by-pattern" />

                                            <div class="js-attendees-by-pattern-options padding-l-lg">
                                                <div class="form-inline">
                                                    <span>Attended at least </span>
                                                    <Rock:NumberBox ID="tbPatternXTimes" runat="server" CssClass="input-width-xs" /><span> times </span>
                                                </div>
                                                <div class="padding-l-lg">
                                                    <div class="form-inline">
                                                        <Rock:RockCheckBox ID="cbPatternAndMissed" runat="server" />and missed at least                                                           
                                                                    <Rock:NumberBox ID="tbPatternMissedXTimes" runat="server" CssClass="input-width-xs" />&nbsp;times between
                                                        <Rock:DateRangePicker ID="drpPatternDateRange" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </p>
                                    </Rock:RockControlWrapper>
                                    <div class="actions margin-b-md">
                                        <asp:LinkButton ID="btnView" runat="server" CssClass="btn btn-primary" Text="Apply" ToolTip="Update the Attendees grid" OnClick="btnView_Click" />
                                    </div>

                                </div>
                            </div>

                            <Rock:Grid ID="gAttendeesAttendance" runat="server" AllowSorting="true" RowItemText="Attendee">
                                <Columns>
                                    <Rock:PersonField DataField="PersonAlias.PersonId" HeaderText="Name" SortExpression="PersonAlias.Person.NickName, PersonAlias.Person.LastName" />
                                    <Rock:RockBoundField DataField="SeriesId" HeaderText="Series" SortExpression="SeriesId" />
                                    <Rock:RockBoundField DataField="YValue" HeaderText="Count" SortExpression="YValue" />
                                </Columns>
                            </Rock:Grid>

                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>

        <script>
            function setActiveButtonGroupButton($activeBtn) {
                $activeBtn.addClass('active');
                $activeBtn.siblings('.btn').removeClass('active');
                $activeBtn.closest('.btn-group').siblings('.js-hidden-selected').val($activeBtn.data('val'));
            }

            Sys.Application.add_load(function () {
                // Graph-By button group
                $('.js-graph-by .btn').on('click', function (e) {
                    setActiveButtonGroupButton($(this));
                });

                setActiveButtonGroupButton($('.js-graph-by').find("[data-val='" + $('.js-graph-by .js-hidden-selected').val() + "']"));

                // Group-By button group
                $('.js-group-by .btn').on('click', function (e) {
                    setActiveButtonGroupButton($(this));
                });

                setActiveButtonGroupButton($('.js-group-by').find("[data-val='" + $('.js-group-by .js-hidden-selected').val() + "']"));

                // Show-By button group
                $('.js-show-by .btn').on('click', function (e) {
                    setActiveButtonGroupButton($(this));
                });

                setActiveButtonGroupButton($('.js-show-by').find("[data-val='" + $('.js-show-by .js-hidden-selected').val() + "']"));

                // View-By button group
                $('.js-view-by .btn').on('click', function (e) {
                    setActiveButtonGroupButton($(this));
                });

                setActiveButtonGroupButton($('.js-view-by').find("[data-val='" + $('.js-view-by .js-hidden-selected').val() + "']"));


                // Attendees Filter
                $('.js-attendees-all').on('click', function (e) {

                });

                $('.js-attendees-by-visit').on('click', function (e) {

                });

                $('.js-attendees-by-pattern').on('click', function (e) {

                });
            });
        </script>
    </ContentTemplate>
</asp:UpdatePanel>
