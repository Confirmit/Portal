<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeFile="AdminUserAttributes.aspx.cs" Inherits="Admin_AdminPersonAttributes" %>

<%@ Register Src="~/Admin/AdminMenu.ascx" TagPrefix="uc1" TagName="AdminMenu" %>
<%@ Register Src="~/Controls/Users/UserAttributes/UserAttributesList.ascx" TagPrefix="uc1" TagName="UserAttributesList" %>

<asp:Content ID="AdminMenuContext" ContentPlaceHolderID="ContextMenu" runat="server">
    <uc1:AdminMenu ID="adminMenu" runat="server" />
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div style="width: 70%;">
        <uc1:UserAttributesList ID="userAttributesList" runat="server" />
    </div>
</asp:Content>