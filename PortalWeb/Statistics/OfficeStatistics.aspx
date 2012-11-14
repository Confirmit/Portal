<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Main.master"
	CodeFile="OfficeStatistics.aspx.cs" Inherits="Statistics_OfficeStatistics" %>

<%@ Register Src="~/Admin/AdminMenu.ascx" TagPrefix="uc1" TagName="AdminMenu" %>

<asp:Content ID="AdminMenuContext" ContentPlaceHolderID="ContextMenu" runat="server">
	<uc1:AdminMenu ID="adminMenu" runat="server" />
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<asp:HyperLink ID="hlMain" NavigateUrl="~/Default.aspx" Visible="false" runat="server" />
	<pwb:OfficeStatistics ID="officeStat" runat="server" Width="100%" />
</asp:Content>
