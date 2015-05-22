<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" Inherits="AdminGlobalSettings" Codebehind="AdminGlobalSettingsPage.aspx.cs" %>

<%@ Register Src="~/Controls/Settings/GlobalSettings.ascx" TagPrefix="uc1" TagName="GlobalSettings" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <uc1:GlobalSettings ID="globalSettings" runat="server" />
</asp:Content>

