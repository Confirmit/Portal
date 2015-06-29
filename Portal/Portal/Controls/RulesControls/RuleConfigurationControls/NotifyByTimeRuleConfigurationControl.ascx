<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotifyByTimeRuleConfigurationControl.ascx.cs" Inherits="Portal.Controls.RulesControls.RuleConfigurationControls.NotifyByTimeRuleConfigurationControl" %>

<table>
    <tr>
        <td>
            <asp:Label ID="SubjectLabel" runat="server" Text="Subject:"/>
        </td>
        <td>
            <asp:TextBox ID="SubjectTextBox" runat="server"/>
        </td>
    </tr>
    <tr>
       <td>
            <asp:Label ID="InformationLabel" runat="server" Text="Information:"/>
        </td>
        <td>
            <asp:TextBox ID="InformationTextBox" runat="server"/>
        </td>
    </tr>
</table>