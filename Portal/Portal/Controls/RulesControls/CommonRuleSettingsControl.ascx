<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommonRuleSettingsControl.ascx.cs" Inherits="Portal.Controls.RulesControls.CommonRuleSettingsControl" %>

<%@ Register TagPrefix="asp" Namespace="Controls.DatePicker" Assembly="Controls" %>

<div id="RuleConfigurationPanel" runat="server">
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
        <input type="checkbox" runat="server" ID="SelectAllDayCheckBox"/>
        <asp:Label ID="DaysOfWeekLabel" runat="server" Text="Days Of Week:" />
        <asp:CheckBoxList ID="DaysOfWeekCheckBoxList" runat="server" />
    </div>
</div>

<script type="text/javascript">
    function CheckBoxListSelect(daysOfWeekCheckBoxListId, selectAllDaysCheckBoxClientId) {
        var daysOfWeekCheckBoxList = document.getElementById(daysOfWeekCheckBoxListId);
        var selectAllDaysCheckBox = document.getElementById(selectAllDaysCheckBoxClientId);
        var state = selectAllDaysCheckBox.checked;
        var checkBoxes = daysOfWeekCheckBoxList.getElementsByTagName("input");
        for (var i = 0; i < checkBoxes.length; i++)
            checkBoxes[i].checked = state;

        return false;
    }
</script>