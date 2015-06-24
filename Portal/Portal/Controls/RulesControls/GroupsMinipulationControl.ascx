<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupsMinipulationControl.ascx.cs" Inherits="Portal.Controls.RulesControls.GroupsMinipulationControl" %>

<%@ Register Src="~/Controls/RulesControls/GroupsListInRuleControl.ascx" TagPrefix="uc" TagName="GroupsListInRuleControl" %>

<div style="background: rgb(221, 221, 221); width: 60%; display: inline-flex; margin-bottom: 10px;">
    <div style="width: 33%;">
        <uc:GroupsListInRuleControl ID="GroupsListContainingInCurrentInRuleControl" runat="server" />
    </div>
    <div style="width: 33%; height: 200px;">
       <div style="height: 100px;">
            <asp:Button ID="RemoveGroupsFromRuleButton" runat="server" Text=" >> "/>
       </div>
        <div style="height: 100px;">
            <asp:Button ID="AddGroupsFromRuleButton" runat="server" Text=" << "/>
        </div>
    </div>
    <div style="width: 33%;">
        <uc:GroupsListInRuleControl ID="GroupsListNotContainingInCurrentInRuleControl" runat="server" />
    </div>
</div>
