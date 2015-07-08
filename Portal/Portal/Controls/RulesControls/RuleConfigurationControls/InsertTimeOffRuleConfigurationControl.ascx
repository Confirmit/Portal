<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InsertTimeOffRuleConfigurationControl.ascx.cs" Inherits="Portal.Controls.RulesControls.RuleConfigurationControls.InsertTimeOffRuleConfigurationControl" %>

<%@ Register Src="~/Controls/RulesControls/TimeSelectorControl.ascx" TagPrefix="uc" TagName="TimeSelectorControl" %>

<table>
    <tr>
        <td>
            <asp:Label ID="TimeIntervalLabel" runat="server" Text="Time Interval:"/>
        </td>
        <td>
            <uc:TimeSelectorControl ID="TimeIntervalSelectorControl" runat="server" />
        </td>
    </tr>
</table>