<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" Inherits="Forums_Admin_Default" Codebehind="Default.aspx.cs" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Import Namespace="AspNetForums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <AspNetForums:NavigationMenu ID="NavigationMenu1" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <table class="tableBorder" cellspacing="1" cellpadding="3" width="100%" border="0">
        <tbody>
            <tr>
                <th class="tableHeaderText" align="left" colspan="2" height="25">
                    <asp:Label runat="server" ID="lblHeader" meta:resourcekey="Header"></asp:Label>
                </th>
            </tr>
            <tr>
                <td class="forumHeaderBackgroundAlternate" colspan="2" height="25">
                    <asp:Label ID="Label1" CssClass="forumTitle" runat="server" meta:resourcekey="UserAdminHeader"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="forumRow" width="20" height="25">
                    <img src="<% =imagePathForUserIcon%>" />
                </td>
                <td class="forumRow" align="left">
                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="forumTitle" NavigateUrl="UserAdmin.aspx"
                        meta:resourcekey="UserAdmin"></asp:HyperLink>
                    <br />
                    <asp:Label ID="Label2" CssClass="normalTextSmall" runat="server" meta:resourcekey="UserAdminSub"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="forumHeaderBackgroundAlternate" colspan="2" height="25">
                    <asp:Label ID="Label5" CssClass="forumTitle" runat="server" meta:resourcekey="ForumAdminHeader"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="forumRow" width="20" height="45">
                    <img src="<% =imagePathForAdminIcon%>" />
                </td>
                <td class="forumRow" align="left">
                    <asp:HyperLink ID="HyperLink2" runat="server" CssClass="forumTitle" NavigateUrl="Forums.aspx"
                        meta:resourcekey="EditForum"></asp:HyperLink>
                    <br />
                    <asp:Label ID="Label3" CssClass="normalTextSmall" runat="server" meta:resourcekey="EditForumSub"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="forumRow" width="20" height="45">
                    <img src="<% =imagePathForAdminIcon%>" />
                </td>
                <td class="forumRow" align="left">
                    <asp:HyperLink ID="HyperLink3" runat="server" CssClass="forumTitle" NavigateUrl="CreateNewForum.aspx"
                        meta:resourcekey="CreateForum"></asp:HyperLink>
                    <br />
                    <asp:Label ID="Label4" CssClass="normalTextSmall" runat="server" meta:resourcekey="CreateForumSub"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
