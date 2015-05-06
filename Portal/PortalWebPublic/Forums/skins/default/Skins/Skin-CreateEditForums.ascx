<%@ Control Language="C#" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Import Namespace="AspNetForums.Controls" %>
<table cellspacing="1" cellpadding="0" width="100%" class="tableBorder">
    <tr>
        <th height="25" class="tableHeaderText" align="left">
            &nbsp;
            <asp:Label ID="Title" runat="server" />
        </th>
    </tr>
    <tr>
        <td class="forumRow" align="left">
            <table border="0" cellpadding="3" cellspacing="0">
                <!-- Forum Name -->
                <tr>
                    <td class="forumRow" nowrap>
                        &nbsp; &nbsp;
                    </td>
                    <td align="right" nowrap>
                        <asp:Label runat="server" ID="lblForum" CssClass="normalTextSmallBold" meta:resourcekey="lblForum"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="ForumName" runat="server" Columns="45"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validationWarningSmall"
                            ErrorMessage="RequiredFieldValidator" meta:resourcekey="erForum" ControlToValidate="ForumName">Требуется заголовок.</asp:RequiredFieldValidator></td>
                </tr>
                <!-- Forum Description -->
                <tr>
                    <td class="forumRow" nowrap>
                        &nbsp; &nbsp;
                    </td>
                    <td align="right" valign="top">
                        <asp:Label runat="server" ID="Label1" CssClass="normalTextSmallBold" meta:resourcekey="lblDescr"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox Rows="10" Columns="60" TextMode="MultiLine" ID="Description" runat="server"
                            MaxLength="3500" />
                    </td>
                    <td align="left">
                    </td>
                </tr>
                <!-- Button -->
                <tr>
                    <td class="forumRow" nowrap>
                        &nbsp; &nbsp;
                    </td>
                    <td align="right" nowrap colspan="2">
                        <asp:Label ID="lblOnCreate" CssClass="normalTextSmall" runat="server" meta:resourcekey="lblOnCreate"
                            Visible="False"></asp:Label>
                        &nbsp;<asp:Button ID="Delete" runat="server" Style="left: 0px" meta:resourcekey="btnDel"
                            Text="Удалить форум" />
                        <asp:Button ID="CreateUpdate" runat="server" />
                    </td>
                    <td align="left" width="100%">
                        <span class="normalTextSmall"></span>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
