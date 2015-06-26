<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminRulesEditingPage.aspx.cs" Inherits="Portal.Admin.AdminRulesEditingPage"
    MasterPageFile="~/MasterPages/Main.master" %>

<%@ Register Src="~/Controls/RulesControls/GroupsMinipulationControl.ascx" TagPrefix="uc" TagName="GroupsMinipulationControl" %>
<%@ Register Src="~/Controls/RulesControls/RuleCreatorControl.ascx" TagPrefix="uc" TagName="RuleCreatorControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <a href="/Admin/AdminRulesListPage.aspx" style="color: #000000;">Link for redirection to 'AdminRulesListPage'</a>
    <div style="margin: 5px;">
        <uc:RuleCreatorControl ID="RuleCreatorControl" runat="server" />
    </div>
    <asp:PlaceHolder ID="RuleEditingControlPlaceHolder" runat="server" />
    <div style="margin: 5px;">
        <uc:GroupsMinipulationControl ID="GroupsMinipulationControl" runat="server" />
    </div>
</asp:Content>
