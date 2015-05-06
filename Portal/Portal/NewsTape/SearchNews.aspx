<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="True" Inherits="NewsTape_SearchNews"
    MaintainScrollPositionOnPostback="true" Codebehind="SearchNews.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="NewsGrid" Src="~/Controls/NewsGrid.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewsTopMenu" Src="~/Controls/News/NewsTopMenu.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <uc1:NewsTopMenu ID="NewsTopMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <center>
        <asp:Label ID="lblSearchNews" Font-Bold="true" meta:resourcekey="lblSearchNews" runat="server" />
        <br />
        <br />
    </center>
    <table width="100%" align="center" >
        <tr>
            <td>
                <table align="center" cols="2" width="75%">
                    <thead bgcolor="#005EB2">
                        <tr>
                            <td width="50%" align="center">
                                <asp:Label ID="hKeyWord" meta:resourcekey="hKeyWord" runat="server" Font-Bold="true"
                                    ForeColor="#DEEEF3" />
                            </td>
                            <td align="center">
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
                                <%--<asp:TextBox ID="tbSearchAuthor" meta:resourcekey="tbSearchAuthor" runat="server"
                                    Width="90%" />--%>
                                <asp:DropDownList ID="ddlAuthors" runat="server">
                                    <asp:ListItem meta:resourcekey="liAll">
                                    </asp:ListItem>
                                </asp:DropDownList>
                            </center>
                        </td>
                    </tr>
                </table>
                <table align="center" cols="3" width="75%">
                    <thead bgcolor="#005EB2">
                        <tr>
                            <td align="center">
                                <asp:Label ID="hLocation" meta:resourcekey="hLocation" runat="server" Font-Bold="true"
                                    ForeColor="#DEEEF3" />
                            </td>
                            <td align="center">
                                <asp:Label ID="hType" meta:resourcekey="hType" runat="server" Font-Bold="true" ForeColor="#DEEEF3" />
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
                                    <td align="center" width="35%">
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
                        <td>
                            <table>
                                <tr>
                                    <td align="center" width="35%">
                                        <asp:DropDownList ID="ddlOffices" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
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
                <center>
                    <br />
                    <asp:Button ID="btnSearch" meta:resourcekey="btnSearch" runat="server" OnClick="btnSearch_Click" />&nbsp;
                </center>
            </td>
        </tr>
        <tr>
            <td align="center">
                <uc1:NewsGrid ID="gvNews" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
