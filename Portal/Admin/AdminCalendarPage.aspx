<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" Inherits="Admin_AdminCalendarPage" Title=":: Portal :: Administration :: Calendar" Codebehind="AdminCalendarPage.aspx.cs" %>

<%@ Register Src="~/Admin/AdminMenu.ascx" TagPrefix="uc1" TagName="AdminMenu" %>
<asp:Content ID="AdminMenuContext" ContentPlaceHolderID="ContextMenu" runat="server">
    <uc1:AdminMenu ID="adminMenu" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="scriptManagerProxy" runat="server" />
    <asp:UpdatePanel ID="upCalendar" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <table style="width: 100%">
                <tr align="center">
                    <td colspan="2">
                        <asp:Calendar ID="calendar" Width="90%" runat="server" OnSelectionChanged="ShowCalendarItem" />
                    </td>
                </tr>
                <tr align="center">
                    <td style="width: 10%">
                        <asp:Label ID="lblWorkTime" CssClass="control-label" runat="server" meta:resourcekey="WorkTime" />
                    </td>
                    <td>
                        <asp:TextBox ID="tbxWorkTime" runat="server" Width="90%" />
                        <asp:RequiredFieldValidator ID="valReqWorkTime" runat="server" ControlToValidate="tbxWorkTime"
                            ValidationGroup="Calendar" ErrorMessage="*" Display="Dynamic" CssClass="control-errorlabel" />
                    </td>
                </tr>
                <tr align="center">
                    <td style="width: 10%">
                        <asp:Label ID="lblComment" CssClass="control-label" runat="server" meta:resourcekey="Comment" />
                    </td>
                    <td>
                        <asp:TextBox ID="tbxComment" runat="server" Width="90%" />
                        <asp:RequiredFieldValidator ID="valReqComment" runat="server" ControlToValidate="tbxComment"
                            ValidationGroup="Calendar" ErrorMessage="*" Display="Dynamic" CssClass="control-errorlabel" />
                    </td>
                </tr>
                <tr align="center">
                    <td colspan="2">
                        <asp:Button ID="btnApply" CssClass="control-button" runat="server" OnClick="ApplyCalendarItem"
                            ValidationGroup="Calendar" meta:resourcekey="Apply" Width="100" />
                        <asp:Button ID="btnDelete" CssClass="control-button" runat="server" OnClick="DeleteCalendarItem"
                            meta:resourcekey="Delete" Width="100" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
