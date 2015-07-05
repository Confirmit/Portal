<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminRulesEditingPage.aspx.cs" Inherits="Portal.Admin.AdminRulesEditingPage"
    MasterPageFile="~/MasterPages/Main.master" %>

<%@ Register Src="~/Controls/EntitiesManipulationControls/EntitiesManipulationControl.ascx" TagPrefix="uc" TagName="EntitiesManipulationControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <a href="/Admin/AdminRulesListPage.aspx" style="color: #000000;">Link for redirection to 'AdminRulesListPage'</a>
    
    <asp:PlaceHolder ID="RuleConfigurationPlaceHolder" runat="server" />

    <div style="margin: 5px;">
        <uc:EntitiesManipulationControl ID="GroupsManipulationControlID" runat="server" />
    </div>
</asp:Content>
