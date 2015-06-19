<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminRulesEditingPage.aspx.cs" Inherits="Portal.Admin.AdminRulesEditingPage"
    MasterPageFile="~/MasterPages/Main.master" %>

<%@ Register Src="~/Controls/RulesControls/RulesListControl.ascx" TagPrefix="rls" TagName="RulesListControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <rls:RulesListControl ID="ControlForEditingGroups" runat="server" />
    <asp:PlaceHolder ID="RuleEditingControlPlaceHolder" runat="server">
        
    </asp:PlaceHolder>
</asp:Content>
