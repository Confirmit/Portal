<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="True" Inherits="UsersList_UsersList" MaintainScrollPositionOnPostback="true" Codebehind="UsersList.aspx.cs" %>

<%@ Register Src="~/Controls/Users/UsersList.ascx" TagPrefix="uc1" TagName="UsersList" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div style="margin-bottom: 15px;">
         <b>
            <asp:Localize ID="locUsersList" runat="server" 
                meta:resourcekey="locUsersList" 
            />
        </b>
        <uc1:UsersList ID="usersList" runat="server"
            Width="90%" ControlMode="Standard" 
            StandardNavigateURL="~/UsersList/UsersInfo.aspx"
            />
    </div>
</asp:Content> 
