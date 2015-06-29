<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RuleCreatorControl.ascx.cs" Inherits="Portal.Controls.RulesControls.RuleCreatorControl" %>

<%@ Register TagPrefix="asp" Namespace="Controls.DatePicker" Assembly="Controls" %>

<div id="RuleConfigurationPanel" runat="server" style="border: 1px solid black; width: 350px; padding: 5px; margin: 5px;">

    <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="RuleDiscriptionLabel" runat="server" Text="Rule Discription:" />

                </td>
                <td>
                    <asp:TextBox ID="RuleDiscriptionTextBox" runat="server" />

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="RuleTypeLabel" runat="server" Text="Rule Type:" />

                </td>
                <td>
                    <asp:DropDownList ID="RuleTypesDropDownList" runat="server" AutoPostBack="True" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="BeginTimeLabel" runat="server" Text="Begin Time:" />

                </td>
                <td>
                    <asp:DatePicker ID="BeginTimeDatePicker" runat="server" />

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="EndTimeLabel" runat="server" Text="End Time:" />

                </td>
                <td>
                    <asp:DatePicker ID="EndTimeDatePicker" runat="server" />

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="ExpirationTimeLabel" runat="server" Text="Expiration Time [HH]:" />

                </td>
                <td>
                    <asp:TextBox ID="ExpirationTimeTextBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LaunchTimeLabel" runat="server" Text="Launch Time [HH:MM]:" />

                </td>
                <td>
                    <asp:TextBox ID="LaunchTimeTextBox" runat="server" />
                </td>
            </tr>
        </table>

        <div>
            <asp:PlaceHolder ID="RuleConfigurationControlPlaceHolder" runat="server" />
        </div>
    </div>

    <div style="border: 1px solid black; margin: 5px;">
        <asp:Label ID="DaysOfWeekLabel" runat="server" Text="Days Of Week:" />
        <asp:CheckBoxList ID="DaysOfWeekCheckBoxList" runat="server" />
    </div>
    <asp:Button ID="CreateRuleButton" runat="server" Text="Create Rule" />
</div>



