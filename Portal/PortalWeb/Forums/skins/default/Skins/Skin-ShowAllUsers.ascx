<%@ Import Namespace="AspNetForums.Controls" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Control Language="C#" %>
<table cellPadding="0" width="100%">
	<tr>
		<td vAlign="top" align="right" colspan="2">
			<AspNetForums:UserList width="100%" id="UserList" CssClass="tableBorder" CellPadding="3" CellSpacing="1" runat="server" />
			<AspNetForums:Paging id="Pager" runat="server" />
		</td>
	</tr>
</table>
