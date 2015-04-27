<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UpdateProgress.ascx.cs"
	Inherits="Controls_UpdateProgress" %>
<asp:ScriptManagerProxy runat="server">
<Scripts>
	<asp:ScriptReference Path="~/scripts/progress.js" />
 </Scripts>
</asp:ScriptManagerProxy>
<asp:UpdateProgress ID="progress" runat="server" DisplayAfter="0">
	<ProgressTemplate>
		<table border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td>
					<asp:Image ImageUrl="~/Images/ani_clock.gif" runat="server" Width="16px" Height="16px" />
				</td>
				<td style="padding-left:10px;">
					<input class="button" type="button" style="vertical-align: top; width: 65px;"
						value="<%=GetLocalResourceObject("cancelButton.Text").ToString()%>" onclick="UpdateProgressStatus_CancelAsyncPostBack()" />
				</td>
			</tr>
		</table>
	</ProgressTemplate>
</asp:UpdateProgress>
<web:UpdateProgressStatus runat="server" Text="»дет обновление. ∆дите..." meta:resourcekey="updateProgressStatus" />
