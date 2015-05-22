<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Controls" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Control Language="C#" %>
<table cellspacing="0" border="0">
    <tr>
        <td>
            <AspNetForums:WhereAmI ID="Whereami2" ShowHome="true" runat="server"></AspNetForums:WhereAmI>
        </td>
    </tr>
</table>
<center>
    <table class="tableBorder" width="75%" cellpadding="3" cellspacing="1">
        <tr>
            <th class="tableHeaderText" align="left" height="25">
                <asp:Label runat="server" meta:resourcekey="Header"></asp:Label>
            </th>
        </tr>
        <tr>
            <td class="forumRow">
                <table cellspacing="1" cellpadding="3">
                    <tr>
                        <td valign="top" nowrap align="left">
                            <asp:Label ID="Label1" CssClass="normalTextSmall" runat="server" meta:resourcekey="SubHeader"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label2" CssClass="normalTextSmall" runat="server" meta:resourcekey="Thread"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label CssClass="normalTextSmall" runat="server" ID="Subject" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="right">
                                        <asp:Label ID="Label3" CssClass="normalTextSmall" runat="server" meta:resourcekey="HasReplies"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label CssClass="normalTextSmall" runat="server" ID="HasReplies" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="right">
                                        <asp:Label ID="Label4" CssClass="normalTextSmall" runat="server" meta:resourcekey="Author"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label CssClass="normalTextSmall" runat="server" ID="PostedBy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="right">
                                        <asp:Label ID="Label5" CssClass="normalTextSmall" runat="server" meta:resourcekey="Body"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label CssClass="normalTextSmall" runat="server" ID="Body" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="right">
                                        <asp:Label ID="Label6" CssClass="normalTextSmall" runat="server" meta:resourcekey="MoveTo"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList runat="server" ID="MoveTo" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2" nowrap align="right">
                            <asp:HyperLink ID="CancelMove" runat="server" meta:resourcekey="CancelMove" />
                            &nbsp;
                            <asp:LinkButton ID="MovePost" runat="server" meta:resourcekey="MovePost" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</center>
