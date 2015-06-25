<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupsMinipulationControl.ascx.cs" Inherits="Portal.Controls.RulesControls.GroupsMinipulationControl" %>

<%@ Register Src="~/Controls/RulesControls/GroupsListInRuleControl.ascx" TagPrefix="uc" TagName="GroupsListInRuleControl" %>

<table>
    <tr>
        <td style="vertical-align: auto;">
            <div id="GroupsListContainingInCurrentInRule">
                <uc:GroupsListInRuleControl ID="GroupsListContainingInCurrentInRuleControl" runat="server" />
            </div>
        </td>
        <td>
            <div style="height: 200px;">
                <div style="margin-top: 40px; margin-bottom: 15px;">
                    <asp:Button ID="RemoveGroupsFromRuleButton" runat="server" Text=" >> " />
                </div>
                <div style="height: 100px;">
                    <asp:Button ID="AddGroupsFromRuleButton" runat="server" Text=" << " />
                </div>
            </div>
        </td>
        <td>
            <div id="GroupsListNotContainingInCurrentInRule">
                <uc:GroupsListInRuleControl ID="GroupsListNotContainingInCurrentInRuleControl" runat="server" />
            </div>
        </td>
    </tr>
</table>
