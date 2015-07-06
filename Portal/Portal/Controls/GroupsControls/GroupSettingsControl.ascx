<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupSettingsControl.ascx.cs" Inherits="Portal.Controls.GroupsControls.GroupSettingsControl" %>

<div id="GroupConfigurationPanel" runat="server">
    <table style="margin-bottom: 10px;">
        <tr>
            <td>
                <asp:Label ID="GroupNameLabel" runat="server" Text="Name:" />
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
</div>