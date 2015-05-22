<%@ Control Language="C#" %>
<table cellPadding="0" cellSpacing="0" width="100%">
 <tr>
        <td valign="top" align="left" width="1px">
            <nobr>
                <asp:HyperLink runat="server" id="LinkHome" />
            </nobr>
        </td>
        <td  valign="top" align="left" runat="server" id="ForumMenu" width="1px">
            <nobr>
                <span runat="server" class="normalTextSmallBold" id="ForumSeparator">&nbsp;&gt;</span>
                <asp:HyperLink runat="server" id="LinkForum" />
            </nobr>
        </td>
        <td  valign="top" align="left" runat="server" id="PostMenu" width="1px">
            <nobr>
                <span runat="server" class="normalTextSmallBold" id="PostSeparator">&nbsp;&gt;</span>
                <asp:HyperLink  runat="server" id="LinkPost" />
            </nobr>
        </td>
        <td valign="top" align="left" width="*">&nbsp;</td>
    </tr></table>