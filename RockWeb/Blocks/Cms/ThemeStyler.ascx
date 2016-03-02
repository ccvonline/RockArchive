﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ThemeStyler.ascx.cs" Inherits="RockWeb.Blocks.Cms.ThemeStyler" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlView" runat="server" CssClass="panel panel-block">
        
            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-paint-brush"></i> Theme Editor</h1>
            </div>
            <div class="panel-body">

                <Rock:NotificationBox ID="nbMessages" runat="server" />

                <div class="row">
                    <div class="col-md-6">
                        <asp:PlaceHolder ID="phThemeControls" runat="server" EnableViewState="true" />
                    </div>

                    <div class="col-md-6">
                        <div class="clearfix">
                            <div class="pull-right">
                                <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click"><i class="fa fa fa-refresh"></i> Save</asp:LinkButton>
                                <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-default" Text="Back" OnClick="btnBack_Click"  />
                            </div>
                        </div>
                        <Rock:CodeEditor ID="ceOverrides" runat="server" Label="CSS Overrides" EditorHeight="600"  />
                    </div>
                </div>

            </div>
        
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    // change approval status to pending if any values are changed
    Sys.Application.add_load( function () {
        $(".js-color-override").on("click", function () {
            var controlKey = $(this).attr("data-control");
            var originalValue = $(this).attr("data-original-value");

            $("input[id$='" + controlKey + "']").parent().colorpicker('setValue', originalValue);
            
            $(this).hide();
        });

        $(".js-text-override").on("click", function () {
            var controlKey = $(this).attr("data-control");
            var originalValue = $(this).attr("data-original-value");

            $("input[id$='" + controlKey + "']").val(originalValue);

            $(this).hide();
        });
    });
</script>
