<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Main.master" Inherits="Statistics_UserStatistics" Codebehind="UserStatistics.aspx.cs" %>

<%@ Register Src="~/Controls/Users/UserStatistics.ascx" TagPrefix="uc1" TagName="UserStatistics" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<asp:HyperLink ID="hlMain" NavigateUrl="~/Default.aspx" Visible="false" runat="server" />
    <div style="width: 100%">
		<asp:Label ID="lblReportFromDate" runat="server" CssClass="control-label" Text="Генерировать отчет с "
			meta:resourcekey="lblReportFromDate" />
		<asp:TextBox ID="tbReportFromDate" runat="server" />
		<asp:Label ID="lblReportToDate" runat="server" CssClass="control-label" Text=" по "
			meta:resourcekey="lblReportToDate" />
		<asp:TextBox ID="tbReportToDate" runat="server" />
		<asp:Button ID="btnGenerateReport" runat="server" CssClass="control-button" Text="Ок"
			OnClick="GenerateReport" meta:resourcekey="btnGenerateReport" Width="40" />
	</div>
    <div class="control-line-between"></div>
	<uc1:UserStatistics ID="UserStatisticsControl" runat="server" />
</asp:Content>