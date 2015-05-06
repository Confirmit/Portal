<%@ Control Language="C#" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Import Namespace="AspNetForums.Controls" %>
<table class="tableBorder" width="100%" cellpadding="3" cellspacing="0">
    <tr>
        <td class="forumRowHighlight" align="center">
            <span class="normalTextSmallBold">
                <asp:Label runat="server" meta:resourcekey="ModerCap"></asp:Label>
                <asp:Label runat="server" ID="PostID" />
                [ </span>
            <asp:HyperLink runat="server" ID="DeletePost" meta:resourcekey="DeletePost" CssClass="menuTextLink"></asp:HyperLink>
            |
            <asp:HyperLink runat="server" ID="EditPost" meta:resourcekey="EditPost" CssClass="menuTextLink"></asp:HyperLink>
            <asp:Label runat="server" ID="Sep" Text="|"></asp:Label>
            <asp:HyperLink runat="server" ID="MovePost" meta:resourcekey="MovePost" CssClass="menuTextLink"></asp:HyperLink>
            <span class="normalTextSmallBold">] </span>
        </td>
    </tr>
</table>
