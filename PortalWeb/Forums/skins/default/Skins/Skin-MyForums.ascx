<%@ Control Language="C#" %>
<%@ Import Namespace="AspNetForums.Controls" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Register TagPrefix="AspNetForumsModeration" Namespace="AspNetForums.Controls.Moderation" Assembly="AspNetForums" %>


<table cellPadding="0" width="100%">
	<tr>
		<td align="left" colSpan="2"><ASPNETFORUMS:WHEREAMI id="Whereami1" runat="server" NAME="Whereami1"></ASPNETFORUMS:WHEREAMI></td>
	</tr>

	<tr>
		<td colSpan="2">
			<%--<span class="menuTitle">Темы которые Вы отслеживаете:</span>--%>
			<asp:Label CssClass="menuTitle" runat ="server" meta:resourcekey = "lblTrack"></asp:Label>
			<AspNetForums:ThreadList id="ThreadTracking" CssClass="tableBorder" runat="server" CellSpacing="1" CellPadding="0" Width="100%"></AspNetForums:ThreadList>
			<br>
			<asp:Label visible="false" id="NoTrackedThreads" runat="server" meta:resourcekey = "lblNoTrack" CssClass="normalTextSmallBold"></asp:Label>
		</td>
	</tr>

	<tr>
		<td colSpan="2">
			<%--<span class="menuTitle">Последние темы, в обсуждении которых Вы участвовали:</span>--%>
			<asp:Label ID="Label1" CssClass="menuTitle" runat ="server" meta:resourcekey = "lblLast"></asp:Label>
			<AspNetForums:ThreadList id="ParticipatedThreads" CssClass="tableBorder" runat="server" CellSpacing="1" CellPadding="0" Width="100%"></AspNetForums:ThreadList>
			<br>
			<asp:Label visible="false" id="NoParticipatedThreads" runat="server" CssClass="normalTextSmallBold" meta:resourcekey = "lblNoLast"></asp:Label>
            <br />
            <asp:Label runat="server" CssClass="normalTextSmallBold" meta:resourcekey = "lblLastDDBox"></asp:Label>
            
            <asp:DropDownList ID="LastCount" runat="server" AutoPostBack="True">
                <asp:ListItem Value="5"></asp:ListItem>
                <asp:ListItem Value="10"></asp:ListItem>
                <asp:ListItem Value="15"></asp:ListItem>
                <asp:ListItem Value="20"></asp:ListItem>
                <asp:ListItem Selected="True" Value="25"></asp:ListItem>
                <asp:ListItem Value="30"></asp:ListItem>
                <asp:ListItem Value="35"></asp:ListItem>
                <asp:ListItem Value="40"></asp:ListItem>
                <asp:ListItem Value="45"></asp:ListItem>
                <asp:ListItem Value="50"></asp:ListItem>
            </asp:DropDownList></td>
	</tr>

</table>
