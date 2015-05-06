<%@ Control Language="C#" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Import Namespace="AspNetForums.Controls" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<table cellspacing="0" border="0">
    <tr>
        <td>
            <AspNetForums:WhereAmI ID="Whereami2" ShowHome="true" runat="server"></AspNetForums:WhereAmI>
        </td>
    </tr>
</table>
<center>
    <table class="tableBorder" id="TABLE1">
        <tr>
            <th class="tableHeaderText" align="left" height="25">
                <asp:Label ID="Label1" runat="server" meta:resourcekey="Header"></asp:Label>
            </th>
        </tr>
        <tr>
            <td class="forumRow">
                <table>
                    <tr>
                        <td valign="top" nowrap align="left">
                            <asp:Label ID="Label2" CssClass="normalTextSmallBold" runat="server" meta:resourcekey="SubHeader"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label3" CssClass="normalTextSmallBold" runat="server" meta:resourcekey="HasReplies"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label CssClass="normalTextSmallBold" runat="server" ID="HasReplies" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2" nowrap align="left">
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2" nowrap align="center">
                            <asp:HyperLink ID="CancelDelete" runat="server" meta:resourcekey="CancelDelete"></asp:HyperLink>
                            &nbsp;
                            <asp:LinkButton ID="DeletePost" runat="server" meta:resourcekey="DeletePost"></asp:LinkButton></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</center>
