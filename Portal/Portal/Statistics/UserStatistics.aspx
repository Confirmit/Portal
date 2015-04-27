<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Main.master" Inherits="Statistics_UserStatistics" Codebehind="UserStatistics.aspx.cs" %>

<%@ Register Src="~/Controls/Users/UserStatistics.ascx" TagPrefix="uc1" TagName="UserStatistics" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<asp:HyperLink ID="hlMain" NavigateUrl="~/Default.aspx" Visible="false" runat="server" />
	<uc1:UserStatistics ID="userStat" runat="server" />
</asp:Content>