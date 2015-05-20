<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    Inherits="Admin_AdminStatisticsPage" CodeBehind="AdminStatisticsPage.aspx.cs" %>

<%@ Register Src="~/Admin/AdminMenu.ascx" TagPrefix="uc1" TagName="AdminMenu" %>
<%@ Register TagPrefix="asp" Namespace="Controls.DatePicker" Assembly="Controls" %>

<asp:Content ID="AdminMenuContext" ContentPlaceHolderID="ContextMenu" runat="server">
    <uc1:AdminMenu ID="adminMenu" runat="server" />
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div id="adminstatisticspage-table-reports-by-time">
        <table style="width: 100%" cellspacing="0px" align="center">
            <tr>
                <th>
                    <asp:Localize ID="AdminReportsCaptionLocalize" runat="server" meta:resourcekey="AdminReportsCaptionLocalize" />
                </th>
            </tr>
            <tr>
                <td class="Default" align="center">
                    <asp:LinkButton ID="AdminReportByCurrentWeekLinkButton" runat="server" OnClick="AdminReportByCurrentWeekLinkButton_Click"
                        meta:resourcekey="AdminReportByCurrentWeekLinkButton" />
                </td>
            </tr>
            <tr>
                <td class="Default" align="center">
                    <asp:LinkButton ID="AdminReportByCurrentMonthLinkButton" runat="server" OnClick="AdminReportByCurrentMonthLinkButton_Click"
                        meta:resourcekey="AdminReportByCurrentMonthLinkButton" />
                </td>
            </tr>
            <tr>
                <td class="Default" align="center">
                    <asp:LinkButton ID="AdminReportByLastWeekLinkButton" runat="server" OnClick="AdminReportByLastWeekLinkButton_Click"
                        meta:resourcekey="AdminReportByLastWeekLinkButton" />
                </td>
            </tr>
            <tr>
                <td class="Default" align="center">
                    <asp:LinkButton ID="AdminReportByLastMonthLinkButton" runat="server" OnClick="AdminReportByLastMonthLinkButton_Click"
                        meta:resourcekey="AdminReportByLastMonthLinkButton" />
                </td>
            </tr>
        </table>
    </div>
    <div id="report-generator" style="width: 100%;">
        <asp:Label ID="ReportFromDateLabel" runat="server" CssClass="control-label" Text="Генерировать отчет с "
            meta:resourcekey="ReportFromDateLabel" />
        <asp:DatePicker ID="ReportFromDateDatePicker" runat="server" />
        <asp:Label ID="ReportToDateLabel" runat="server" CssClass="control-label" Text=" по "
            meta:resourcekey="ReportToDateLabel" />
        <asp:DatePicker ID="ReportToDateDatePicker" runat="server" />
        <asp:Button ID="GenerateReportButton" runat="server" CssClass="control-button" Text="Ок"
            OnClick="GenerateReport" meta:resourcekey="GenerateReportButton" Width="40" />
    </div>
</asp:Content>
