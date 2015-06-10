<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_UserStatistics" Codebehind="UserStatistics.ascx.cs" %>

<%@ Register Src="~/Controls/DayUserStatistics.ascx" TagPrefix="uc1" TagName="DayStatistics" %>

<table style="min-width: 500px; width: 100%; overflow: scroll" cellpadding="0" cellspacing="0" border="2" >
	<tr>
		<td style="width: 16%;" align="center">
			<asp:Localize ID="locDateTitle" runat="server" meta:resourcekey="locDateTitle"/>
		</td>
		<td style="width: 17%" align="center">
			<asp:Localize ID="locEventTitle" runat="server"  meta:resourcekey="locEventTitle"/>
		</td>
		<td style="width: 17%" align="center">
			<asp:Localize ID="locBeginTimeTitle" runat="server"  meta:resourcekey="locBeginTimeTitle"/>
		</td>
		<td style="width: 17%" align="center">
			<asp:Localize ID="locEndTimeTitle" runat="server"  meta:resourcekey="locEndTimeTitle"/>
		</td>
		<td style="width: 17%" align="center">
			<asp:Localize ID="locTotalTimeTitle" runat="server"  meta:resourcekey="locTotalTimeTitle"/>
		</td>
		<td  align="center">
			<asp:Localize ID="locWorkTimeTitle" runat="server"  meta:resourcekey="locWorkTimeTitle"/>
		</td>
	</tr>
	<tr>
		<td colspan="6">
			<asp:DataGrid ID="grdDaysStats" runat="server" 
			    ShowHeader="false" Width="100%"
				AutoGenerateColumns="false" 
				OnItemDataBound="OnStatisticsBound">
				<Columns>
					<asp:TemplateColumn>
						<ItemTemplate>
							<asp:Label ID="locDate" runat="server" />
						</ItemTemplate>
                        <ItemStyle Width="16%" />
					</asp:TemplateColumn>
					
					<asp:TemplateColumn>
						<ItemTemplate>
							<uc1:DayStatistics ID="dayStat" runat="server" />
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid>
		</td>
	</tr>
	<tr>
		<td colspan="6" align="right" style="font-weight:bold">
			<asp:Localize ID="locTotalTime" runat="server" meta:resourcekey="locTotalTimeTitle" />
			<asp:Label ID="lblTotalTime" runat="server" />
		</td>
	</tr>
	<tr>
		<td colspan="6" align="right" style="font-weight:bold">
			<asp:Localize ID="locWorkTime" runat="server" meta:resourcekey="locWorkTimeTitle" />
			<asp:Label ID="lblWorkTime" runat="server" />
		</td>
	</tr>
	<tr>
		<td colspan="6" align="right" style="font-weight:bold">
			<asp:Localize ID="locTimeRate" runat="server" meta:resourcekey="locTimeRate" />
			<asp:Label ID="lblTimeRate" runat="server" />
		</td>
	</tr>
	<tr>
		<td colspan="6" align="right" style="font-weight:bold">
			<asp:Localize ID="locRestTime" runat="server" meta:resourcekey="locRestTime" />
			<asp:Label ID="lblRestTime" runat="server" />
		</td>
	</tr>
</table>