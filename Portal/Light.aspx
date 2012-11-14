<%@ Page Title="Легковесный портал" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="True" Inherits="PortalWeb.Light" Codebehind="Light.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="NewDay" Src="~/Controls/NewDay.ascx" %>

<asp:Content ID="ContentPlh" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<div align="center" style="width: 600px;">
		<uc1:NewDay ID="ucNewDay" runat="server" />
	</div>
</asp:Content>