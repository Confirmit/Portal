<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsersGroupManipulationControl.ascx.cs" Inherits="Portal.Controls.GroupsControls.UsersGroupManipulationControl" %>

<%@ Register Src="~/Controls/GroupsControls/UserListInGroupControl.ascx" TagPrefix="uc" TagName="UserGroupsSelectionControl" %>

<div style="background: rgb(221, 221, 221); width: 60%; display: inline-flex; margin-bottom: 10px;">
    <div style="width: 33%;">
        <uc:UserGroupsSelectionControl ID="UsersListContainingInCurrentGroupControl" runat="server" />
    </div>
    <div style="width: 33%; height: 200px;">
       <div style="margin-top: 45px;">
            <asp:Button ID="RemoveUsersFromGroupButton" runat="server" Text=" >> "/>
       </div>
        <div style="height: 100px;">
            <asp:Button ID="AddUsersInGroupButton" runat="server" Text=" << "/>
        </div>
    </div>
    <div style="width: 33%;">
        <uc:UserGroupsSelectionControl ID="UsersListNotContainingInCurrentGroupControl" runat="server" />
    </div>
</div>
