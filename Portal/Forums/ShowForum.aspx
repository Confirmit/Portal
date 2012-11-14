<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="True" Inherits="Forums_ShowForum" Codebehind="ShowForum.aspx.cs" %>

<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Components" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
	<AspNetForums:NavigationMenu ID="NavigationMenu2" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<table width="100%" height="100%" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top" align="left">
			<td id="CenterColumn" width="95%" runat="server" class="CenterColumn">
				<AspNetForums:ThreadView ID="ThreadView1" runat="server" />
			</td>
		</tr>
	</table>
</asp:Content>
