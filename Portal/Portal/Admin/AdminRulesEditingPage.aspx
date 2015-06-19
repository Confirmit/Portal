<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminRulesEditingPage.aspx.cs" Inherits="Portal.Admin.AdminRulesEditingPage"
    MasterPageFile="~/MasterPages/Main.master" %>

<%@ Register Src="~/Controls/RulesControls/RulesListControl.ascx" TagPrefix="rls" TagName="RulesListControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <rls:RulesListControl ID="ControlForEditingGroups" runat="server" />
</asp:Content>

<%--<%@ Register Src="~/Controls/AdminGroupsEditingControl.ascx" TagPrefix="grp" TagName="GroupsEditingControl" %>

<asp:Content ID="ContentMain2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <grp:GroupsEditingControl ID="ControlForEditingGroups" runat="server" />
</asp:Content>--%>
