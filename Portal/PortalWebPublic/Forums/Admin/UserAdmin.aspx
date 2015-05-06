<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" 
CodeFile="UserAdmin.aspx.cs" Inherits="Forums_Admin_UserAdmin" UICulture="auto"%>

<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Components" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <AspNetForums:NavigationMenu ID="NavigationMenu1" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <p>
        <AspNetForums:ShowAllUsers ID="Useradmin1" runat="server" SkinFilename="Skin-AdminShowAllUsers.ascx" />
    </p>
</asp:Content>
