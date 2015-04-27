<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsAttachFile.ascx.cs" Inherits="NewsAttachFile" %>

<table runat="server" id="tblAttach" width="100%" cellpadding="2">
    <tr>
        <td style="width: 110px;">
            <asp:Image runat="server" ID="imgAttachImage" Width="100px" />
        </td>
        <td>
            <asp:HyperLink CssClass="attachfile-text" runat="server" ID="hlFileName" />
        </td>
        <td runat="server" id="tblDelete" align="right" visible="false">
            <div style="float: right;" class="attachfile-text">
                <asp:LinkButton runat="server"  ID="linkDelete" Text="Delete" />
            </div>            
            <div style="float: right;" class="attachfile-right"></div>
        </td>
    </tr>
</table>