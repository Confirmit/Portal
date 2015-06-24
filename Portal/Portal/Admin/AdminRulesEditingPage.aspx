<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminRulesEditingPage.aspx.cs" Inherits="Portal.Admin.AdminRulesEditingPage"
    MasterPageFile="~/MasterPages/Main.master" %>

<%@ Register Src="~/Controls/RulesControls/RulesListControl.ascx" TagPrefix="rls" TagName="RulesListControl" %>
<%@ Register Src="~/Controls/RulesControls/GroupsMinipulationControl.ascx" TagPrefix="uc" TagName="GroupsMinipulationControl" %>
<%@ Register Src="~/Controls/RulesControls/RuleCreatorControl.ascx" TagPrefix="uc" TagName="RuleCreatorControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <uc:RuleCreatorControl ID="RuleCreatorControl" runat="server" />
    <rls:RulesListControl ID="ControlForEditingRules" runat="server" />
    <asp:PlaceHolder ID="RuleEditingControlPlaceHolder" runat="server"></asp:PlaceHolder>
    <div style="width: 100%; margin: 5px;">
        <uc:GroupsMinipulationControl ID="GroupsMinipulationControl" runat="server" />
    </div>
</asp:Content>
