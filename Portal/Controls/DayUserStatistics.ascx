<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_DayUserStatistics" Codebehind="DayUserStatistics.ascx.cs" %>

<table width="100%" cellpadding="0" cellspacing="0" border="0">
	<asp:PlaceHolder ID="phAbsence" runat="server">
		<tr>
			<td align="center">
				<asp:Label ID="lblAbsenceReason" runat="server" />
			</td>
		</tr>
	</asp:PlaceHolder>
	<asp:PlaceHolder ID="phWorkTimes" runat="server">
		<tr>
			<td>
				<table width="100%" cellpadding="0" cellspacing="0" border="0">
					<tr><!-- Строка рабочего времени -->
						<td style="width:20%">
							<asp:Localize ID="locWork" runat="server" meta:resourcekey="locWork" />
						</td>
						<td style="width:20%">
							<asp:Localize ID="locBeginTime" runat="server" meta:resourcekey="locBeginTime" />
						</td>
						<td style="width:20%">
							<asp:Localize ID="locEndTime" runat="server" meta:resourcekey="locEndTime" />
						</td>
						<td style="width:20%">
							<asp:Localize ID="locTotalTime" runat="server" meta:resourcekey="locTotalTime" />
						</td>
						<td style="width:20%">
							<asp:Localize ID="locWorkTime" runat="server" meta:resourcekey="locWorkTime" />
						</td>
					</tr>
					<asp:PlaceHolder ID="phDinner" runat="server">
						<tr><!-- Строка обеденного времени времени -->
							<td>
								<asp:Localize ID="locDinner" runat="server" meta:resourcekey="locDinner" />
							</td>
							<td>
							</td>
							<td>
							</td>
							<td>
								<asp:Localize ID="locDinnerTime" runat="server" meta:resourcekey="locDinnerTime" />
							</td>
							<td>
							</td>
						</tr>
					</asp:PlaceHolder>
					<asp:PlaceHolder ID="phTimeOff" runat="server">
						<tr><!-- Строка нерабочего времени времени -->
							<td>
								<asp:Localize ID="locTimeOff" runat="server" meta:resourcekey="locTimeOff" />
							</td>
							<td>
							</td>
							<td>
							</td>
							<td>
								<asp:Localize ID="locTimeOffTime" runat="server" meta:resourcekey="locTimeOffTime" />
							</td>
							<td>
							</td>
						</tr>
					</asp:PlaceHolder>
				</table>
			</td>
		</tr>
	</asp:PlaceHolder>
</table>