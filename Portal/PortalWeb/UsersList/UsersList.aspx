<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeFile="UsersList.aspx.cs" Inherits="UsersList_UsersList" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/Controls/Users/UsersList.ascx" TagPrefix="uc1" TagName="UsersList" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <center>
        <b>
            <asp:Localize ID="locUsersList" runat="server" 
                meta:resourcekey="locUsersList" 
            />
        </b>
        <uc1:UsersList ID="usersList" runat="server"
            Width="90%" ControlMode="Standard" 
            StandardNavigateURL="~/UsersList/UsersInfo.aspx"
            />
    </center>
</asp:Content> 
