<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminRulesListPage.aspx.cs" Inherits="Portal.Admin.AdminRulesListPage" 
MasterPageFile="~/MasterPages/Main.master" %>

<%@ Register Src="~/Controls/RulesControls/RulesListControl.ascx" TagPrefix="rls" TagName="RulesListControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div style="margin: 5px;">
        <asp:Button ID="AddNewRuleButton" runat="server" Text="Add Rule"/>
    </div>
    <rls:RulesListControl ID="ControlForEditingRules" runat="server" />
</asp:Content>