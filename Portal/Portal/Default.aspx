<%@ Page Language="c#" MasterPageFile="~/MasterPages/Main.master" Inherits="Main"
    MaintainScrollPositionOnPostback="true" meta:resourcekey="Page" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="NewDay" Src="~/Controls/NewDay.ascx" %>

<asp:Content ID="ContentPlaceHolder" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <table style="width:40%">
        <tr>
            <td align="center">
                <asp:Localize ID="localizeNotRegistered" runat="server" Visible="false" meta:resourcekey="NotRegistered" />
                <asp:Localize ID="localizzeNotEmployee" runat="server" Visible="false" meta:resourcekey="NotEmployee" />
                <uc1:NewDay ID="NewDayControl" OnWorkFinished="OnWorkFinish" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
