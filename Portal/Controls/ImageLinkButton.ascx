﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="ImageLinkButton" Codebehind="ImageLinkButton.ascx.cs" %>

<table>
    <tr>
        <td>
            <asp:Image ID="img" runat="server" />
        </td>
        <td>
            <div style="padding-bottom: 2px;">
                <a class="control-hyperlink" id="ahref" runat="server">
                    <asp:Label ID="label" runat="server" />
                </a>
            </div>
        </td>
    </tr>
</table>
