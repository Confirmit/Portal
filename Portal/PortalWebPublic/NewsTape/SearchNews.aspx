<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    CodeFile="SearchNews.aspx.cs" Inherits="NewsTape_SearchNews" UICulture="auto"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="uc1" TagName="NewsTopMenu" Src="~/Controls/NewsTopMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Grid" Src="~/Controls/Grid.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewsGrid" Src="~/Controls/NewsGrid.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <uc1:NewsTopMenu ID="NewsTopMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <center>
        <asp:Label ID="lblSearchNews" Style="font-weight: bold;" meta:resourcekey="lblSearchNews"
            runat="server" />
        <br />
        <br />
    </center>
    <table width="100%">
        <tr>
            <td align="center">
                <table cols="2" width="75%">
                    <thead bgcolor="#005EB2">
                        <tr>
                            <td width="50%" align="center">
                                <asp:Label ID="hKeyWord" meta:resourcekey="hKeyWord" runat="server" Font-Bold="true"
                                    ForeColor="#DEEEF3" />
                            </td>
                            <td width="50%" align="center">
                                <asp:Label ID="hAuthor" meta:resourcekey="hAuthor" runat="server" Font-Bold="true"
                                    ForeColor="#DEEEF3" />
                            </td>
                        </tr>
                    </thead>
                    <tr bgcolor="#D9E2EC">
                        <td>
                            <center>
                                <asp:TextBox ID="tbSearchText" meta:resourcekey="tbSearchText" runat="server" Width="90%" />
                            </center>
                        </td>
                        <td>
                            <center>
                                <asp:DropDownList ID="ddlAuthors" runat="server">
                                    <asp:ListItem meta:resourcekey="liAll">
                                    </asp:ListItem>
                                </asp:DropDownList>
                            </center>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table align="center" cols="3" width="75%">
                    <thead bgcolor="#005EB2">
                        <tr>
                            <td width="30%" align="center">
                                <asp:Label ID="hLocation" meta:resourcekey="hLocation" runat="server" Font-Bold="true"
                                    ForeColor="#DEEEF3" />
                            </td>
                            <td width="40%" align="center">
                                <asp:Label ID="hOffice" meta:resourcekey="hOffice" runat="server" Font-Bold="true"
                                    ForeColor="#DEEEF3" />
                            </td>
                            <td width="30%" align="center">
                                <asp:Label ID="hDate" meta:resourcekey="hDate" runat="server" Font-Bold="true" ForeColor="#DEEEF3" />
                            </td>
                        </tr>
                    </thead>
                    <tr bgcolor="#D9E2EC">
                        <td>
                            <table>
                                <tr>
                                    <td align="center" width="30%">
                                        <asp:DropDownList ID="ddlNewsStatus" runat="server">
                                            <asp:ListItem meta:resourcekey="liAll">
                                            </asp:ListItem>
                                            <asp:ListItem meta:resourcekey="liActualNews">
                                            </asp:ListItem>
                                            <asp:ListItem meta:resourcekey="liArchiveNews">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="center" width="40%">
                            <asp:DropDownList ID="ddlOffices" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td valign="middle" align="center" width="30%">
                            <asp:DropDownList ID="ddlPeriod" runat="server">
                                <asp:ListItem meta:resourcekey="liAll">
                                </asp:ListItem>
                                <asp:ListItem meta:resourcekey="li1day">
                                </asp:ListItem>
                                <asp:ListItem meta:resourcekey="li3days">
                                </asp:ListItem>
                                <asp:ListItem meta:resourcekey="li7days">
                                </asp:ListItem>
                                <asp:ListItem meta:resourcekey="li30days">
                                </asp:ListItem>
                                <asp:ListItem meta:resourcekey="li6months">
                                </asp:ListItem>
                                <asp:ListItem meta:resourcekey="li1year">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnSearch" meta:resourcekey="btnSearch" runat="server" OnClick="btnSearch_Click" />&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <uc1:NewsGrid ID="gvNews" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
