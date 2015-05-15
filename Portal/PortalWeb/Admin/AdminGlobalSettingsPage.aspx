<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeFile="AdminGlobalSettingsPage.aspx.cs" Inherits="AdminGlobalSettings" %>

<%@ Register Src="~/Controls/Settings/GlobalSettings.ascx" TagPrefix="uc1" TagName="GlobalSettings" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <uc1:GlobalSettings ID="globalSettings" runat="server" />
</asp:Content>

