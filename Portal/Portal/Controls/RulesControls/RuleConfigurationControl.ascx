<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RuleConfigurationControl.ascx.cs" Inherits="Portal.Controls.RulesControls.RuleConfigurationControl" %>

<%@ Register Src="~/Controls/RulesControls/CommonRuleSettingsControl.ascx" TagPrefix="uc" TagName="CommonRuleSettingsControl" %>

<div id="RuleConfigurationPanel" runat="server" style="border: 1px solid black; width: 350px; padding: 5px; margin: 5px;">
    <uc:CommonRuleSettingsControl ID="CommonRuleSettingsControl" runat="server"/>
    <asp:Button ID="SaveRuleCongigurationButton" runat="server" Text="Save" />
</div>