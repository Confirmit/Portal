<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" 
CodeFile="EditForum.aspx.cs" Inherits="Forums_Admin_EditForum" UICulture="auto"%>

<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Register TagPrefix="Admin" Namespace="AspNetForums.Controls.Admin" Assembly="AspNetForums" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <AspNetForums:NavigationMenu ID="NavigationMenu1" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <br />
    <Admin:EditForum ID="objEditForum" runat="server" RedirectUrl="Forums.aspx" />
</asp:Content>
