<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupCreatorControl.ascx.cs"
    Inherits="Portal.Controls.GroupsControls.GroupCreatorControl" ViewStateMode="Disabled" %>

<div style="margin-bottom: 10px;">
    <asp:Button ID="AddNewGroupButton" runat="server" Text="Add Group" />
    <div ID="GroupConfigurationPanel" runat="server">
        <table style="margin-bottom: 10px;">
            <tr>
                <td>
                    <asp:Label ID="GroupNameLabel" runat="server" Text="Group Name:" />
                </td>
                <td>
                    <asp:TextBox ID="GroupNameTextBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="GroupDescriptionLabel" runat="server" Text="Description:" />
                </td>
                <td>
                    <asp:TextBox ID="GroupDescriptionTextBox" runat="server" />
                </td>
            </tr>
        </table>
        <asp:Button ID="CreateGroupButton" runat="server" Text="Create Group" />
    </div>
</div>

