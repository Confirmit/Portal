<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    CodeFile="AdminUsersPage.aspx.cs" Inherits="Admin_AdminUsersPage" %>

<%@ Register Src="~/Admin/AdminMenu.ascx" TagPrefix="uc1" TagName="AdminMenu" %>
<%@ Register Src="~/Admin/UserInfo.ascx" TagPrefix="uc1" TagName="UserInfo" %>
<%@ Register Src="~/Controls/Users/UsersGrid.ascx" TagPrefix="uc1" TagName="UsersGrid" %>
<%@ Register Src="~/Controls/Users/UsersFilter.ascx" TagPrefix="uc1" TagName="UsersFilter" %>

<asp:Content ID="AdminMenuContext" ContentPlaceHolderID="ContextMenu" runat="server">
    <uc1:AdminMenu ID="adminMenu" runat="server" />
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="scriptManagerProxy" runat="server" />
    
    <div style="width: 90%">
        <uc1:UsersFilter ID="usersFilter" runat="server" />
        
        <div class="control-line-between"></div>    
        <uc1:UsersGrid ID="userGrid" runat="server" />
    </div>
    
    <asp:UpdatePanel ID="upFull" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:HyperLink ID="hlMain" runat="server" Visible="false" NavigateUrl="~/Default.aspx" />
            <table width="80%" border="0px">
                <asp:PlaceHolder ID="plhUsersList" runat="server">
                    <tr>
                        <td style="width: 40px;" />
                        <td colspan="2" align="right">
                            <div style="float: right;">
                                <asp:Button Width="150px" CssClass="control-button" ID="btnNewUser" runat="server"
                                    OnClick="createNewUser" meta:resourcekey="btnNewUser"
                                    TabIndex="1"
                                    UseSubmitBehavior="false"
                                    />
                            </div>
                        </td>
                        <td style="width: 40px;" />
                    </tr>
                </asp:PlaceHolder>
                <tr align="left">
                    <td style="width: 40px;" />
                    <td colspan="2">
                        <asp:UpdatePanel ID="upUserInfo" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <uc1:UserInfo ID="userInfo" runat="server" OnApply="applyUserInfo" OnCancel="cancelUserInfo" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 40px;" />
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="userInfo" EventName="Apply" />
            <asp:AsyncPostBackTrigger ControlID="userInfo" EventName="Cancel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>