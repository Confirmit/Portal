<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RuleCreatorControl.ascx.cs" Inherits="Portal.Controls.RulesControls.RuleCreatorControl" %>

<%@ Register TagPrefix="asp" Namespace="Controls.DatePicker" Assembly="Controls" %>

<asp:Button ID="AddNewRuleButton" runat="server" Text="Add Group" />

<div ID="RuleConfigurationPanel" runat="server">
    <div>
        <asp:Label ID="RuleNameLabel" runat="server" />
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
        <asp:Label ID="ExpirationTimeLabel" runat="server" />
    </div>
    <div>
        <asp:Label ID="LaunchTimeLabel" runat="server" />
    </div>
    <div>
        <asp:Label ID="DaysOfWeekLabel" runat="server" />
    </div>
    <asp:Button ID="CreateRuleButton" runat="server" Text="Create Group" />
</div>



