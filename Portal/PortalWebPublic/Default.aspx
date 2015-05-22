<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Main.master"
	CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<asp:HyperLink ID="hlSecure" NavigateUrl="~/Secure/Default.aspx" Visible="false" runat="server" />
</asp:Content>