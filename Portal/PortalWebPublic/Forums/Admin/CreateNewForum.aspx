<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
	CodeFile="CreateNewForum.aspx.cs" Inherits="Forums_Admin_CreateNewForum" UICulture="auto"%>

<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Register TagPrefix="Admin" Namespace="AspNetForums.Controls.Admin" Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Components" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
	<AspNetForums:NavigationMenu ID="NavigationMenu1" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<Admin:CreateEditForum ID="Createeditforum1" runat="server" Mode="CreateForum" RedirectUrl="CreateNewForum.aspx" />
</asp:Content>
