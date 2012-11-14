<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Main.master" CodeFile="UserStatistics.aspx.cs" Inherits="Statistics_UserStatistics" %>

<%@ Register Src="~/Controls/Users/UserStatistics.ascx" TagPrefix="uc1" TagName="UserStatistics" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<asp:HyperLink ID="hlMain" NavigateUrl="~/Default.aspx" Visible="false" runat="server" />
	<uc1:UserStatistics ID="userStat" runat="server" />
</asp:Content>