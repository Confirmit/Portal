<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupCreatorControl.ascx.cs"
    Inherits="Portal.Controls.GroupsControls.GroupCreatorControl" %>

<div id="GroupConfigurationPanel" runat="server">
    <table style="margin-bottom: 10px;">
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

