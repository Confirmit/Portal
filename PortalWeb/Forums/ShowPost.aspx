<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
	CodeFile="ShowPost.aspx.cs" Inherits="ShowPost" %>

<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
	<AspNetForums:NavigationMenu ID="NavigationMenu2" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<table width="100%" height="100%" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top" align="left">
			<td id="CenterColumn" width="95%" runat="server" class="CenterColumn">
				<AspNetForums:PostView runat="server" ID="PostView1" />
			</td>
		</tr>
	</table>
</asp:Content>
