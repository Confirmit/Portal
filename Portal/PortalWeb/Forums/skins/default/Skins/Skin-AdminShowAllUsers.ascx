<%@ Import Namespace="AspNetForums.Controls" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Control Language="C#" %>
<table cellpadding="0" width="90%" align="center">
	<tr>
		<td align="left" valign="top">
			<asp:Label runat="server" CssClass="title" ID="lblCap" meta:resourcekey="Caption"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="top" align="right" colspan="2">
			<AspNetForums:UserList UrlUserNameRedirect='<%# Globals.UrlAdminEditUser %>' Width="100%"
				ID="UserList" CssClass="tableBorder" CellPadding="3" CellSpacing="1" runat="server" />
			<AspNetForums:Paging ID="Pager" runat="server" />
		</td>
	</tr>
	<tr>
		<td>
			&nbsp;
		</td>
	</tr>
</table>
