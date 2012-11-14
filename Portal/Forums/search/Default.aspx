<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="True" Inherits="Forums_search_Default" Codebehind="Default.aspx.cs" %>

<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Register TagPrefix="AspNetForumsSearch" Namespace="AspNetForums.Controls.Search"
	Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Components" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
	<AspNetForums:NavigationMenu ID="NavigationMenu1" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<div align="left">
		<AspNetForumsSearch:Search runat="server" ID="Search1" />
	</div>
</asp:Content>
