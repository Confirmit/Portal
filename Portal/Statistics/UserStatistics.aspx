<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Main.master" Inherits="Statistics_UserStatistics" Codebehind="UserStatistics.aspx.cs" %>

<%@ Register Src="~/Controls/Users/UserStatistics.ascx" TagPrefix="uc1" TagName="UserStatistics" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<asp:HyperLink ID="hlMain" NavigateUrl="~/Default.aspx" Visible="false" runat="server" />
    <div style="width: 100%">
		<asp:Label ID="UserStatisticsFromCurrentDateLabel" runat="server" CssClass="control-label" Text="�������� ���������� ������������ �� ������ �"
			meta:resourcekey="UserStatisticsFromCurrentDateLabel" />
		<asp:TextBox ID="UserStatisticsFromCurrentDateTextBox" runat="server" />
		<asp:Label ID="UserStatisticsToCurrentDateLabel" runat="server" CssClass="control-label" Text=" �� "
			meta:resourcekey="UserStatisticsToCurrentDateLabel" />
		<asp:TextBox ID="UserStatisticsToCurrentDateTextBox" runat="server" />
		<asp:Button ID="GetUserStatisticsButton" runat="server" CssClass="control-button" Text="��"
			OnClick="GenerateReport" meta:resourcekey="GetUserStatisticsButton" />
	</div>
    <div class="control-line-between"></div>
	<uc1:UserStatistics ID="UserStatisticsControl" runat="server" />
</asp:Content>