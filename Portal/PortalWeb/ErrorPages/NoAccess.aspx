<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeFile="NoAccess.aspx.cs" Inherits="ErrorPages_NoAccess" Title="Untitled Page" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
	<div class="sectionCaption">
		<asp:Label ID="lblError" runat="server" meta:resourcekey="Error" />
	</div>
	<asp:Label ID="lblErrorDescription" runat="server" meta:resourcekey="ErrorDescription" />
</asp:Content>

