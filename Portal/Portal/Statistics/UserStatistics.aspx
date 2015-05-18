<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Main.master" Inherits="Statistics_UserStatistics" Codebehind="UserStatistics.aspx.cs" %>

<%@ Register Src="~/Controls/Users/UserStatistics.ascx" TagPrefix="uc1" TagName="UserStatistics" %>
<%@ Register TagPrefix="asp" Namespace="Controls.DatePicker" Assembly="Controls" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<asp:HyperLink ID="hlMain" NavigateUrl="~/Default.aspx" Visible="false" runat="server" />
    <div style="width: 100%">
		<asp:Label ID="UserStatisticsFromCurrentDateLabel" runat="server" CssClass="control-label" Text="Получить статистику пользователя за период с"
			meta:resourcekey="UserStatisticsFromCurrentDateLabel" />
        <asp:DatePicker ID="UserStatisticsFromCurrentDateTextBox" runat="server" />
		<asp:Label ID="UserStatisticsToCurrentDateLabel" runat="server" CssClass="control-label" Text=" по "
			meta:resourcekey="UserStatisticsToCurrentDateLabel" />
        <asp:DatePicker ID="UserStatisticsToCurrentDateTextBox" runat="server" />
		<asp:Button ID="GetUserStatisticsButton" runat="server" CssClass="control-button" Text="Ок"
			OnClick="GetUserStatisticsButtonOnClick" meta:resourcekey="GetUserStatisticsButton" />
	</div>
    <div class="control-line-between"></div>
	<uc1:UserStatistics ID="UserStatisticsControl" runat="server" />
</asp:Content>