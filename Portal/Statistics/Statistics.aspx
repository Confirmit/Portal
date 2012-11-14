<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="True" Inherits="Statistics_Statistics" Codebehind="Statistics.aspx.cs" %>

<asp:Content ID="ContentPlh" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<asp:HyperLink ID="hlStatPage" NavigateUrl="UserStatistics.aspx" runat="server" Visible="false" />
	<table style="width:600px;margin-top:20px;" class="Default" cellspacing="0px" align="center">
		<tr>
			<th>
				<asp:Localize ID="locReportsCaption" runat="server" meta:resourcekey="locReportsCaption" />
			</th>
		</tr>
		<tr>
			 <td class="Default" align="center">
				  <asp:LinkButton ID="lbtnRSCurrentWeek" runat="server" OnClick="lbtnRSCurrentWeek_Click" meta:resourcekey="lbtnRSCurrentWeek" />
			 </td>
		</tr>
		<tr>
			 <td class="Default" align="center">
				  <asp:LinkButton ID="lbtnRSCurrentMonth" runat="server" OnClick="lbtnRSCurrentMonth_Click" meta:resourcekey="lbtnRSCurrentMonth" />
			 </td>
		</tr>
		<tr>
			 <td class="Default" align="center">
				  <asp:LinkButton ID="lbtnRSCurrentMonthToNow" runat="server" OnClick="lbtnRSCurrentMonthToNow_Click" meta:resourcekey="lbtnRSCurrentMonthToNow" />
			 </td>
		</tr>
		<tr>
			 <td class="Default" align="center">
				  <asp:LinkButton ID="lbtnRSLastWeek" runat="server" OnClick="lbtnRSLastWeek_Click" meta:resourcekey="lbtnRSLastWeek" />
			 </td>
		</tr>
		<tr>
			 <td class="Default" align="center">
				  <asp:LinkButton ID="lbtnRSLastMonth" runat="server" OnClick="lbtnRSLastMonth_Click" meta:resourcekey="lbtnRSLastMonth" />
			 </td>
		</tr>
	</table>                                                  
</asp:Content>
