<%@ Page Language="C#" CodeFile="EditPost.aspx.cs" Inherits="EditPost" MasterPageFile="~/MasterPages/Main.master"%>

<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Register TagPrefix="AspNetForumsModeration" Namespace="AspNetForums.Controls.Moderation"
    Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Components" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <AspNetForums:NavigationMenu ID="NavigationMenu1" runat="server" />
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <AspNetForums:CreateEditPost ID="CreateEditPost1" Mode="EditPost" runat="server" />
</asp:Content>
