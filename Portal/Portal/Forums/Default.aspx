<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" Inherits="Forums_Default" Codebehind="Default.aspx.cs" %>

<%@ Import Namespace="AspNetForums.Components" %>
<%@ Register TagPrefix="AspNetForumsSearch" Namespace="AspNetForums.Controls.Search"
	Assembly="AspNetForums" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
	<AspNetForums:NavigationMenu ID="NavigationMenu1" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<table width="100%" height="100%" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top" align="left">
			<td id="CenterColumn" width="95%" runat="server" class="CenterColumn">
				<AspNetForums:ForumRepeater runat="server" ID="ForumRepeater1">
				</AspNetForums:ForumRepeater>
				<p>
				</p>
			</td>
		</tr>
	</table>
</asp:Content>
