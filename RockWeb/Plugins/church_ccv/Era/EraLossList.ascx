﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EraLossList.ascx.cs" Inherits="RockWeb.Plugins.church_ccv.Era.EraLossList" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlView" runat="server" CssClass="panel panel-block">

            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-user"></i>&nbsp;eRA Loss Report</h1>
            </div>
            <div class="panel-body">

                <div class="grid grid-panel">
                    <Rock:GridFilter ID="gfList" runat="server">
                        <Rock:DateRangePicker ID="drpDates" runat="server" Label="Date Range" />
                        <Rock:CampusPicker ID="cpCampus" runat="server" Label="Campus" />
                        <Rock:PersonPicker ID="ppPastor" runat="server" Label="Pastor"/>
                        <Rock:RockCheckBox ID="cbShowProcessed" runat="server" Text="Show Processed" />
                    </Rock:GridFilter>

                    <Rock:ModalAlert ID="mdGridWarning" runat="server" />

                    <Rock:Grid ID="gList" runat="server" AllowSorting="true" DataKeyNames="Id">
                        <Columns>
                            <Rock:SelectField DataSelectedField="Processed" HeaderText="Processed" SortExpression="Processed" />
                            <Rock:SelectField DataSelectedField="SendEmail" HeaderText="Send Email" SortExpression="SendEmail" />
                            <Rock:CallbackField DataField="HHPerson" HeaderText="Family Head" SortExpression="HHPerson.FullName" OnOnFormatDataValue="HHPerson_OnFormatDataValue" />
                            <Rock:RockBoundField DataField="AdultNames" HeaderText="Adults" />
                            <Rock:RockBoundField DataField="ChildNames" HeaderText="Children" />
                            <Rock:DateField DataField="LossDate" HeaderText="Loss Date" SortExpression="LossDate" />
                            <Rock:DateField DataField="FirstAttended" HeaderText="First Attended" SortExpression="FirstAttended" />
                            <Rock:DateField DataField="LastAttended" HeaderText="Last Attended" SortExpression="LastAttended" />
                            <Rock:DateField DataField="LastGave" HeaderText="Last Gave" SortExpression="LastGave" />
                            <Rock:RockBoundField DataField="TimesGaveLastYear" HeaderText="Times Gave Last Year" SortExpression="TimesGaveLastYear" />
                            <Rock:DateField DataField="StartingPointDate" HeaderText="Starting Point Date" SortExpression="StartingPointDate" />
                            <Rock:DateField DataField="BaptismDate" HeaderText="Baptism Date" SortExpression="BaptismDate" />
                            <Rock:CampusField DataField="CampusId" HeaderText="Campus" SortExpression="CampusName" />
                            <Rock:CallbackField DataField="NeighborhoodPastor" HeaderText="Associate Pastor" SortExpression="NeighborhoodPastor.FullName" OnOnFormatDataValue="NeighborhoodPastor_OnFormatDataValue" />
                            <Rock:BoolField DataField="InNeighborhoodGroup" HeaderText="In Neighborhood Group" SortExpression="InNeighborhoodGroup" />
                        </Columns>
                    </Rock:Grid>
                </div>

            </div>

        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
