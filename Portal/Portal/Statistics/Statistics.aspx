<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="True" Inherits="Statistics_Statistics" CodeBehind="Statistics.aspx.cs" %>

<asp:Content ID="ContentPlh" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:HyperLink ID="hlStatPage" NavigateUrl="UserStatistics.aspx" runat="server" Visible="false" />
    <div id="statisticsspage-reports-by-time">
        <table style="width: 600px; margin-top: 20px;" class="Default" cellspacing="0px" align="center">
            <tr>
                <th>
                    <asp:Localize ID="ReportsCaptionLocalize" runat="server" meta:resourcekey="ReportsCaptionLocalize" />
                </th>
            </tr>
            <tr>
                <td class="Default" align="center">
                    <asp:LinkButton ID="GetReportByCurrentWeekLinkButton" runat="server" OnClick="GetReportByCurrentWeekLinkButton_Click" meta:resourcekey="GetReportByCurrentWeekLinkButton" />
                </td>
            </tr>
            <tr>
                <td class="Default" align="center">
                    <asp:LinkButton ID="GetReportByCurrentMonthLinkButton" runat="server" OnClick="GetReportByCurrentMonthLinkButton_Click" meta:resourcekey="GetReportByCurrentMonthLinkButton" />
                </td>
            </tr>
            <tr>
                <td class="Default" align="center">
                    <asp:LinkButton ID="GetReportByCurrentMonthToNowLinkButton" runat="server" OnClick="GetReportByCurrentMonthToNowLinkButton_Click" meta:resourcekey="GetReportByCurrentMonthToNowLinkButton" />
                </td>
            </tr>
            <tr>
                <td class="Default" align="center">
                    <asp:LinkButton ID="GetReportByLastWeekLinkButton" runat="server" OnClick="GetReportByLastWeekLinkButton_Click" meta:resourcekey="GetReportByLastWeekLinkButton" />
                </td>
            </tr>
            <tr>
                <td class="Default" align="center">
                    <asp:LinkButton ID="GetReportByLastMonthLinkButton" runat="server" OnClick="GetReportByLastMonthLinkButton_Click" meta:resourcekey="GetReportByLastMonthLinkButton" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
