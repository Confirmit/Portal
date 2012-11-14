<%@ Register TagPrefix="AspNetModeration" Namespace="AspNetForums.Controls.Moderation"
	Assembly="AspNetForums" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Controls" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Control Language="C#" %>
<table class="tableBorder" cellspacing="1" cellpadding="0" width="100%">
	<tr>
		<th class="tableHeaderText" align="left" height="20">
			<asp:Label runat="server" ID="lblCaption" meta:resourcekey="Caption"></asp:Label>
			<asp:Label ID="Username" runat="server"></asp:Label>
		</th>
	</tr>
	<!-- User banned-->
	<tr>
		<td valign="top" align="left">
			<asp:CheckBox ID="Banned" runat="server" CssClass="normalTextSmallBold" meta:resourcekey="Banned">
			</asp:CheckBox>
		</td>
	</tr>
</table>
<span id="Moderation" runat="server" visible="false"></span>
<table width="100%" border="0">
	<tr>
		<td valign="top" align="left">
			<asp:Button ID="Submit" runat="server" meta:resourcekey="Submit"></asp:Button>
		</td>
	</tr>
</table>
