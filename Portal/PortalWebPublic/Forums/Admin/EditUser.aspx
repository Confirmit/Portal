<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" 
CodeFile="EditUser.aspx.cs" Inherits="Forums_Admin_EditUser" UICulture="auto"%>

<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Components" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <AspNetForums:NavigationMenu ID="NavigationMenu1" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <AspNetForums:EditUserProfile ID="Userinfo1" runat="server" AdminMode="Admin" />
</asp:Content>
