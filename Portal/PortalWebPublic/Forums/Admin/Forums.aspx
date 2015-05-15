<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" 
CodeFile="Forums.aspx.cs" Inherits="Forums_Admin_Forums" UICulture="auto"%>

<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Components" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <AspNetForums:NavigationMenu ID="NavigationMenu1" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <AspNetForums:ForumRepeater ShowAllForums="true" runat="server" ID="Forumrepeater1">
        <HeaderTemplate>
            <table cellpadding="3" cellspacing="1" class="tableBorder" width="100%">
                <tr>
                    <th height="25" class="tableHeaderText" align="left">
                        <asp:Label runat="server" ID="Caption" meta:resourcekey="Caption"></asp:Label>
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="forumRow">
                    <span class="forumTitle"><a href="EditForum.aspx?ForumId=<%# DataBinder.Eval(Container.DataItem, "ForumID") %>">
                        <%# DataBinder.Eval(Container.DataItem, "Name") %>
                    </a></span>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </AspNetForums:ForumRepeater>
</asp:Content>