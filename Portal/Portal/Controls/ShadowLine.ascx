<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_ShadowLine" Codebehind="ShadowLine.ascx.cs" %>
<table cellspacing="0" cellpadding="0" width="100%" border="0">
	<tr>
		<td style="background-color: #a0a0a0;" colspan="3">
			<img id="Img1" height="1" alt="" src="~/images/b.gif" width="1" runat="server" /></td>
	</tr>
	<tr>
		<td>
			<img id="Img2" height="1" alt="" src="~/images/b.gif" width="1" runat="server" /></td>
		<td style='width: 100%; background-image: url(<%=ResolveClientUrl("~/images/bkg_gray_grad.gif")%>);'>
			<img id="Img3" height="3" alt="" src="~/images/b.gif" width="1" runat="server" /></td>
		<td>
			<img id="Img4" height="1" alt="" src="~/images/b.gif" width="1" runat="server" /></td>
	</tr>
</table>
