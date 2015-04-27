<%@ Control Language="C#" %>
<%@ Import Namespace="AspNetForums.Controls" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Register TagPrefix="AspNetForumsModeration" Namespace="AspNetForums.Controls.Moderation"
    Assembly="AspNetForums" %>
<table cellpadding="0" width="100%">
    <tr>
        <td align="left" colspan="2">
            <AspNetForums:WhereAmI ID="Whereami1" ShowHome="true" runat="server"></AspNetForums:WhereAmI>
        </td>
    </tr>
    <tr>
        <td align="left" colspan="2">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td valign="top" align="left">
            <span class="normalTextSmallBold">
                <asp:CheckBox runat="server" ID="TrackThread" meta:resourcekey="TrackThread" Checked="false" />
            </span>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <AspNetForums:PostList ID="PostList" CssClass="tableBorder" runat="server" CellSpacing="1"
                CellPadding="0" Width="100%">
            </AspNetForums:PostList>
            <AspNetForums:Paging ID="Pager" runat="server"></AspNetForums:Paging>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td align="left" colspan="2">
            <AspNetForums:WhereAmI ShowHome="true" ID="Whereami2" runat="server" />
        </td>
    </tr>
</table>
