<%@ Control Language="C#" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Import Namespace="AspNetForums.Controls" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<table width="100%">
    <tr>
        <td align="center">
            <table cellspacing="1" cellpadding="0" width="60%" class="tableBorder">
                <tr>
                    <th class="tableHeaderText" align="left">
                        <asp:Label CssClass="tableHeaderText" ID="MessageTitle" runat="server" />
                    </th>
                </tr>
                <tr>
                    <td class="forumRow">
                        <table cellpadding="3" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Label CssClass="normalTextSmall" ID="MessageBody" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
