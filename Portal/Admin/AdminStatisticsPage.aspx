<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
	Inherits="Admin_AdminStatisticsPage" CodeBehind="AdminStatisticsPage.aspx.cs" %>

<%@ Register Src="~/Admin/AdminMenu.ascx" TagPrefix="uc1" TagName="AdminMenu" %>

<asp:Content ID="AdminMenuContext" ContentPlaceHolderID="ContextMenu" runat="server">
	<uc1:AdminMenu ID="adminMenu" runat="server" />
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<table style="width: 100%" cellspacing="0px" align="center">
		<tr>
			<th>
				<asp:Localize ID="locReportsCaption" runat="server" meta:resourcekey="locReportsCaption" />
			</th>
		</tr>
		<tr>
			<td class="Default" align="center">
				<asp:LinkButton ID="lbtnRSCurrentWeek" runat="server" OnClick="lbtnRSCurrentWeek_Click"
					meta:resourcekey="lbtnRSCurrentWeek" />
			</td>
		</tr>
		<tr>
			<td class="Default" align="center">
				<asp:LinkButton ID="lbtnRSCurrentMonth" runat="server" OnClick="lbtnRSCurrentMonth_Click"
					meta:resourcekey="lbtnRSCurrentMonth" />
			</td>
		</tr>
		<tr>
			<td class="Default" align="center">
				<asp:LinkButton ID="lbtnRSLastWeek" runat="server" OnClick="lbtnRSLastWeek_Click"
					meta:resourcekey="lbtnRSLastWeek" />
			</td>
		</tr>
		<tr>
			<td class="Default" align="center">
				<asp:LinkButton ID="lbtnRSLastMonth" runat="server" OnClick="lbtnRSLastMonth_Click"
					meta:resourcekey="lbtnRSLastMonth" />
			</td>
		</tr>
	</table>
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
</asp:Content>
