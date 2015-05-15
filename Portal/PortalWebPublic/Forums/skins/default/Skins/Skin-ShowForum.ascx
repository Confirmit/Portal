<%@ Control Language="C#" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Import Namespace="AspNetForums.Controls" %>
<table cellpadding="0" width="100%">
    <tr>
        <td colspan="2" align="left">
            <AspNetForums:WhereAmI ID="Whereami1" ShowHome="true" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td valign="bottom" align="left">
            <asp:HyperLink ID="NewThreadLinkTop" runat="server">
                <asp:Image runat="server" ID="NewThreadImageTop"></asp:Image>
            </asp:HyperLink></td>
        <td align="right">
            <asp:Label runat="server" CssClass="normalTextSmallBold" meta:resourcekey="lblSearch"></asp:Label>
            <asp:TextBox ID="Search" runat="server"></asp:TextBox>
            <asp:Button ID="SearchButton" runat="server" meta:resourcekey="btnSearch"></asp:Button></td>
    </tr>
    <tr>
        <td valign="top" colspan="2">
            <AspNetForums:ThreadList ID="ThreadList" CssClass="tableBorder" Width="100%" CellPadding="3"
                CellSpacing="1" runat="server" /><AspNetForums:Paging ID="Pager" runat="server" />
            <asp:Label CssClass="normalTextSmall" ID="NoThreads" runat="server" meta:resourcekey="lblNoThreads"
                Visible="False"></asp:Label>
            <asp:Label CssClass="normalTextSmall" ID="NoPostsDueToFilter" runat="server" meta:resourcekey="lblNoPostsDueToFilter"
                Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td colspan="2" align="right">
            <asp:Label runat="server" CssClass="normalTextSmallBold" meta:resourcekey="lblSortBy"></asp:Label>
            <asp:DropDownList ID="OrderBy" runat="server" AutoPostBack="True">
                <asp:ListItem Value="1"></asp:ListItem>
                <asp:ListItem Value="2"></asp:ListItem>
                <asp:ListItem Value="3"></asp:ListItem>
                <asp:ListItem Value="4"></asp:ListItem>
                <asp:ListItem Selected="True" Value="5"></asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="OrderTyp" runat="server" AutoPostBack="True">
                <asp:ListItem Value="ACS">A..Z</asp:ListItem>
                <asp:ListItem Selected="True" Value="DESC">Z..A</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="left" valign="top" style="height: 62px">
            <AspNetForums:WhereAmI ShowHome="true" ID="Whereami2" runat="server" />
        </td>
        <td align="right" style="height: 62px">
            <asp:Label ID="Label1" runat="server" CssClass="normalTextSmallBold" meta:resourcekey="lblFilter"></asp:Label>
            <asp:DropDownList ID="DisplayByDays" runat="server">
            </asp:DropDownList>
            <br>
            <asp:LinkButton ID="MarkAllRead" meta:resourcekey="MarkRead"
                runat="server"></asp:LinkButton>
            <br>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;
        </td>
    </tr>
</table>
