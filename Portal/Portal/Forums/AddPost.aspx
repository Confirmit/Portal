<%@ Page Language="C#" Inherits="Forums_AddPost" MasterPageFile="~/MasterPages/Main.master" Codebehind="AddPost.aspx.cs" %>

<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Components" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
	<AspNetForums:NavigationMenu ID="NavigationMenu1" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<div align="left">
		<AspNetForums:CreateEditPost runat="server" ID="Createeditpost1" />
	</div>
</asp:Content>
