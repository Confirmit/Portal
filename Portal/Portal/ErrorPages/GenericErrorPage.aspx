<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" Inherits="ErrorPages_GenericErrorPage" Title="Untitled Page" Codebehind="GenericErrorPage.aspx.cs" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
	<div class="sectionCaption">
		<asp:Label ID="lblError" runat="server" meta:resourcekey="Error" />
	</div>
	<asp:Label ID="lblErrorDescription" runat="server" meta:resourcekey="ErrorDescription" />
</asp:Content>