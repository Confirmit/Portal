<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Control %>
<table cellspacing="0" border="0">
    <tr>
        <td>
            <AspNetForums:WhereAmI ID="Whereami1" ShowHome="true" runat="server"></AspNetForums:WhereAmI>
        </td>
    </tr>
</table>
<table class="tableBorder" cellspacing="1" cellpadding="3" width="100%">
    <tr>
        <th class="tableHeaderText" align="left" height="25">
            &nbsp;<asp:Label ID="PostTitle" runat="server"></asp:Label>
        </th>
    </tr>
    <span id="ReplyTo" runat="server" visible="false">
        <tr>
            <td class="forumRow">
                <table cellspacing="1" cellpadding="3">
                    <tr>
                        <td colspan="2">
                            <asp:Label runat="server" CssClass="normalTextSmallBold" meta:resourcekey="ReplyTo"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" nowrap align="right">
                            <asp:Label ID="Label1" runat="server" CssClass="normalTextSmallBold" meta:resourcekey="Author"></asp:Label>
                        </td>
                        <td valign="top" align="left">
                            <asp:Label ID="ReplyPostedBy" runat="server" CssClass="normalTextSmall"></asp:Label>
                            <asp:Label ID="ReplyPostedByDate" runat="server" CssClass="normalTextSmall"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" align="right">
                            <asp:Label CssClass="normalTextSmallBold" ID="Label2" runat="server" meta:resourcekey="Caption"></asp:Label>
                        </td>
                        <td valign="top" align="left">
                            <asp:HyperLink ID="ReplySubject" runat="server" CssClass="normalTextSmall"></asp:HyperLink></td>
                    </tr>
                    <tr>
                        <td valign="top" align="right">
                            <asp:Label ID="Label3" runat="server" CssClass="normalTextSmallBold" meta:resourcekey="Post"></asp:Label>
                        </td>
                        <td valign="top" align="left">
                            <span class="normalTextSmall">
                                <asp:Label ID="ReplyBody" runat="server"></asp:Label>
                            </span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </span><span id="Preview" runat="server" visible="false">
        <tr>
            <td class="forumRow">
                <table cellspacing="1" cellpadding="3" width="75%" border="0">
                    <tr>
                        <td valign="top" align="left">
                            <span class="normalTextSmallBold">
                                <asp:Label ID="PreviewSubject" runat="server"></asp:Label>
                            </span>
                            <br>
                            <span class="normalTextSmall">
                                <asp:Label ID="PreviewBody" runat="server"></asp:Label>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left">
                            <asp:Button ID="BackButton" runat="server" meta:resourcekey="BackButton"></asp:Button>&nbsp;
                            <asp:Button ID="PreviewPostButton" runat="server" meta:resourcekey="PreviewPostButton">
                            </asp:Button></td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </span>
    <span id="Post" runat="server" visible="true">
        <tr>
            <td class="forumRow">
                <table cellspacing="1" cellpadding="3">
                    <tr>
                        <td valign="top" nowrap align="right">
                            <asp:Label CssClass="normalTextSmallBold" ID="Label4" runat="server" meta:resourcekey="Author"></asp:Label>
                        </td>
                        <td valign="top" align="left" colspan="2">
                            <span class="normalTextSmall">
                                <asp:Label ID="PostAuthor" runat="server"></asp:Label>
                            </span>
                        </td>
                    </tr>
                    <span id="Edit" runat="server" visible="false">
                        <tr>
                            <td valign="top" nowrap align="right">
                                <asp:Label CssClass="normalTextSmallBold" ID="Label5" runat="server" meta:resourcekey="EditAs"></asp:Label>
                            </td>
                            <td valign="top" align="left" colspan="2">
                                <span class="normalTextSmall">
                                    <asp:Label ID="PostEditor" runat="server"></asp:Label>
                                </span>
                            </td>
                        </tr>
                    </span>
                    <tr>
                        <td nowrap valign="middle" align="right">
                            <asp:Label CssClass="normalTextSmallBold" ID="Label6" runat="server" meta:resourcekey="Caption"></asp:Label>
                        </td>
                        <td valign="top" align="left">
                            <asp:TextBox AutoCompleteType="none" ID="PostSubject" runat="server" Columns="55"></asp:TextBox></td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validationWarningSmall"
                                ErrorMessage="RequiredFieldValidator" meta:resourcekey="RequiredFieldValidator1"
                                ControlToValidate="PostSubject"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" nowrap align="right">
                            <asp:Label CssClass="normalTextSmallBold" ID="Label7" runat="server" meta:resourcekey="Post"></asp:Label>
                        </td>
                        <td valign="top" align="left">
                            <asp:TextBox ID="PostBody" runat="server" Columns="65" TextMode="MultiLine" Rows="15"></asp:TextBox></td>
                        <td valign="top">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validationWarningSmall"
                                ErrorMessage="RequiredFieldValidator" ControlToValidate="PostBody" meta:resourcekey="RequiredFieldValidator2"
                                EnableClientScript="False"></asp:RequiredFieldValidator></td>
                    </tr>
                    <span id="AllowPinnedPosts" runat="server" visible="false">
                        <tr>
                            <td valign="middle" align="right" width="91">
                                <asp:Label CssClass="normalTextSmallBold" ID="Label8" runat="server" meta:resourcekey="Pinned"></asp:Label>
                            </td>
                            <td valign="top" align="left">
                                <span class="normalTextSmall">
                                    <asp:DropDownList ID="PinnedPost" runat="server">
                                    </asp:DropDownList>
                                </span>
                            </td>
                        </tr>
                    </span>
                    <tr>
                        <td valign="middle" align="right" width="93">
                            <span class="normalTextSmallBold">&nbsp;</span></td>
                        <td valign="top" align="left">
                            <span class="normalTextSmall">
                                <asp:CheckBox ID="AllowReplies" runat="server" meta:resourcekey="AllowReplies"></asp:CheckBox>
                                <br />
                                <asp:CheckBox ID="chkPinnedPost" runat="server" meta:resourcekey="chkPinnedPost"></asp:CheckBox>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="right" colspan="2">
                            <asp:Button CausesValidation="false" ID="Cancel" runat="server" meta:resourcekey="Cancel">
                            </asp:Button>&nbsp;
                            <asp:Button ID="PreviewButton" runat="server" meta:resourcekey="Preview"></asp:Button></td>
                    </tr>
                    <tr>
                        <td valign="top" align="right" colspan="2">
                            <asp:Button ID="PostButton" runat="server" meta:resourcekey="PreviewPostButton"></asp:Button></td>
                    </tr>
                </table>
            </td>
        </tr>
    </span>
</table>
<table cellspacing="0" border="0">
    <tr>
        <td>
            <AspNetForums:WhereAmI ShowHome="true" ID="Whereami2" runat="server"></AspNetForums:WhereAmI>
        </td>
    </tr>
</table>
