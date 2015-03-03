<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" Inherits="ErrorPages_AccessDenied" Title="Untitled Page"CodeBehind="AccessDenied.aspx.cs" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
	<div class="sectionCaption">
		<asp:Label ID="lblError" runat="server"  meta:resourcekey="Error" />
	</div>
	<asp:Label ID="lblErrorDescription" runat="server"  meta:resourcekey="ErrorDescription" />
</asp:Content>