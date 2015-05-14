<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_UserStatistics" Codebehind="UserStatistics.ascx.cs" %>

<%@ Register Src="~/Controls/DayUserStatistics.ascx" TagPrefix="uc1" TagName="DayStatistics" %>

<div style="width: 100%">
		<asp:Label ID="lblReportFromDate" runat="server" CssClass="control-label" Text="Генерировать отчет с "
			meta:resourcekey="lblReportFromDate" />
		<asp:TextBox ID="tbReportFromDate" runat="server" />
		<asp:Label ID="lblReportToDate" runat="server" CssClass="control-label" Text=" по "
			meta:resourcekey="lblReportToDate" />
		<asp:TextBox ID="tbReportToDate" runat="server" />
		<asp:Button ID="btnGenerateReport" runat="server" CssClass="control-button" Text="Ок"
			OnClick="GenerateReport" meta:resourcekey="btnGenerateReport" Width="40" />
	</div>

<table width="100%" cellpadding="0" cellspacing="0" border="2">
	<tr>
		<th style="width:15%" align="center">
			<asp:Localize ID="locDateTitle" runat="server" meta:resourcekey="locDateTitle"/>
		</th>
		<th style="width:17%" align="center">
			<asp:Localize ID="locEventTitle" runat="server"  meta:resourcekey="locEventTitle"/>
		</th>
		<th style="width:17%" align="center">
			<asp:Localize ID="locBeginTimeTitle" runat="server"  meta:resourcekey="locBeginTimeTitle"/>
		</th>
		<th style="width:17%" align="center">
			<asp:Localize ID="locEndTimeTitle" runat="server"  meta:resourcekey="locEndTimeTitle"/>
		</th>
		<th style="width:17%" align="center">
			<asp:Localize ID="locTotalTimeTitle" runat="server"  meta:resourcekey="locTotalTimeTitle"/>
		</th>
		<th style="width:17%" align="center">
			<asp:Localize ID="locWorkTimeTitle" runat="server"  meta:resourcekey="locWorkTimeTitle"/>
		</th>
	</tr>
	<tr>
		<td colspan="6">
			<asp:DataGrid ID="grdDaysStats" runat="server" 
			    ShowHeader="false" Width="100%"
				AutoGenerateColumns="false" 
				OnItemDataBound="OnStatisticsBound">
				<Columns>
					<asp:TemplateColumn ItemStyle-Width="15%">
						<ItemTemplate>
							<asp:Label ID="locDate" runat="server" />
						</ItemTemplate>
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