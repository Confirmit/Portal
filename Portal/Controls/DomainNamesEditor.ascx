<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_DomainNamesEditor" Codebehind="DomainNamesEditor.ascx.cs" %>

<asp:Panel ID="pnlDomainNames" runat="server">
	<asp:ScriptManagerProxy ID="smProxy" runat="server" />
	<asp:UpdatePanel ID="upDomainNames" ChildrenAsTriggers="true" runat="server">
		<ContentTemplate>
			<table width="100%">
				<tr>
					<td>
						<div style="white-space: nowrap; width: 100%">
							<asp:TextBox ID="tbDomainName" runat="server" />
							<asp:Button ID="btnAdd" Width="100" runat="server" CssClass="control-button" OnClick="OnAdd" meta:resourcekey="Add" />
							<asp:Button ID="btnRemove" Width="100" runat="server" CssClass="control-button" OnClick="OnRemove" meta:resourcekey="Remove" />
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<asp:ListBox ID="lbDomainNames" Width="100%" SelectionMode="Multiple"
							Rows="5" DataTextField="Value" DataValueField="ID" runat="server" />
					</td>
				</tr>
			</table>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Panel>
