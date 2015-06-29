<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotReportToMoscowRuleConfigurationControl.ascx.cs" Inherits="Portal.Controls.RulesControls.RuleConfigurationControls.NotReportToMoscowRuleConfigurationControl" %>

<%@ Register TagPrefix="asp" Namespace="Controls.DatePicker" Assembly="Controls" %>

<div>
    <table>
        <tr>
            <td>
                <asp:Label ID="BeginTimeLabel" runat="server" Text="BeginTime" /></td>
            <td>
                <asp:DatePicker ID="BeginTimeDatePicker" runat="server" /></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="EndTimeLabel" runat="server" Text="EndTime" /></td>
            <td>
                <asp:DatePicker ID="EndTimeDatePicker" runat="server" /></td>
        </tr>
    </table>
    <asp:Button ID="SaveChangesButton" runat="server" Text="Save Changes" />
</div>
