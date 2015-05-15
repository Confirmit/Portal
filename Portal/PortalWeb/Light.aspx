<%@ Page Title="Легковесный портал" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Light.aspx.cs" Inherits="PortalWeb.Light" %>

<%@ Register TagPrefix="uc1" TagName="NewDay" Src="~/Controls/NewDay.ascx" %>

<asp:Content ID="ContentPlh" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<div align="center" style="width: 600px;">
		<uc1:NewDay ID="ucNewDay" runat="server" />
	</div>
</asp:Content>