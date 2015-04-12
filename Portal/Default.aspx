<%@ Page Language="c#" MasterPageFile="~/MasterPages/Main.master" Inherits="Main"
    MaintainScrollPositionOnPostback="true" meta:resourcekey="Page" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="NewDay" Src="~/Controls/NewDay.ascx" %>

<asp:Content ID="ContentPlh" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <table width="95%">
        <tr>
            <td align="center">
                <asp:Localize ID="locNotRegistered" runat="server" Visible="false" meta:resourcekey="NotRegistered" />
                <asp:Localize ID="locNotEmployee" runat="server" Visible="false" meta:resourcekey="NotEmployee" />
                <uc1:NewDay ID="NewDay1" OnWorkFinished="OnWorkFinish" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
