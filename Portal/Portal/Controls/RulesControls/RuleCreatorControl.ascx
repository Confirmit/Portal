<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RuleCreatorControl.ascx.cs" Inherits="Portal.Controls.RulesControls.RuleCreatorControl" %>

<%@ Register Src="~/Controls/RulesControls/CommonRuleSettingsControl.ascx" TagPrefix="uc" TagName="CommonRuleSettingsControl" %>

<div id="RuleConfigurationPanel" runat="server" style="border: 1px solid black; width: 350px; padding: 5px; margin: 5px;">
    <asp:Label ID="RuleTypeLabel" runat="server" Text="Rule Type:" />
    <asp:DropDownList ID="RuleTypesDropDownList" runat="server" AutoPostBack="True" />
    
    <uc:CommonRuleSettingsControl ID="CommonRuleSettingsControl" runat="server"/>
    <asp:Button ID="CreateRuleButton" runat="server" Text="Create Rule" />
</div>



