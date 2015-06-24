<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RuleCreatorControl.ascx.cs" Inherits="Portal.Controls.RulesControls.RuleCreatorControl" %>

<%@ Register TagPrefix="asp" Namespace="Controls.DatePicker" Assembly="Controls" %>

<asp:Button ID="AddNewRuleButton" runat="server" Text="Add Group" />

<div id="RuleConfigurationPanel" runat="server" style="border: 1px solid black; width: 350px; padding: 5px; margin: 5px;">
    <div>
        <asp:Label ID="RuleNameLabel" runat="server" Text="Rule Name:" />
        <asp:TextBox ID="RuleNameTextBox" runat="server" />
    </div>

    <div>
        <asp:Label ID="RuleTypeLabel" runat="server" Text="Rule Type:" />
        <asp:DropDownList ID="RuleTypesDropDownList" runat="server" />
    </div>
    <div>
        <asp:Label ID="BeginTimeLabel" runat="server" Text="Begin Time:" />
        <asp:DatePicker ID="BeginTimeDatePicker" runat="server" />
    </div>
    <div>
        <asp:Label ID="EndTimeLabel" runat="server" Text="End Time:" />
        <asp:DatePicker ID="EndTimeDatePicker" runat="server" />
    </div>
    <div>
        <asp:Label ID="ExpirationTimeLabel" runat="server" Text="Expiration Time:" />
    </div>
    <div>
        <asp:Label ID="LaunchTimeLabel" runat="server" Text="Launch Time:" />
    </div>
    <div style="border: 1px solid black; margin: 5px;">
        <asp:Label ID="DaysOfWeekLabel" runat="server" Text="Days Of Week:" />
        <asp:CheckBoxList ID="DaysOfWeekCheckBoxList" runat="server" />
    </div>
    <asp:Button ID="CreateRuleButton" runat="server" Text="Create Group" />
</div>



