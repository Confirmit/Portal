<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_GroupsMembershipControl" Codebehind="GroupsMembershipControl.ascx.cs" %>

<asp:Panel ID="pnlGroups" runat="server">
	<asp:ScriptManagerProxy ID="smProxy" runat="server" />
	
	<asp:UpdatePanel ID="upGroups" ChildrenAsTriggers="true" runat="server">
		<ContentTemplate>
			<table width="100%">
				<tr>
				    <td></td>
					<td>
						<div style="white-space: nowrap; width: 100%">
							<asp:DropDownList ID="ddlGroups" runat="server"
							    DataTextField="Description" 
							    DataValueField="RoleID" 
							    Width="300" />
							<asp:Button ID="btnAdd" Width="100" runat="server" 
							    OnClick="OnAdd"
							    CssClass="control-button"
								meta:resourcekey="Add"
								CausesValidation="false" 
							/>
							<asp:Button ID="btnRemove" Width="100" 
							    runat="server" 
							    OnClick="OnRemove"
							    CssClass="control-button"
								meta:resourcekey="Remove" 
								CausesValidation="false"
						    />
						</div>
					</td>
				</tr>
				<tr>
				    <td>
				        <asp:Localize ID="locGroupsMembership" runat="server" meta:resourcekey="locGroupsMembership" />
				    </td>
					<td>
						<asp:ListBox ID="lbPersonGroups" Width="100%" 
						    SelectionMode="Multiple" Rows="5" 
						    DataTextField="Description" 
						    DataValueField="RoleID" runat="server" 
						/>
					</td>
				</tr>
				<tr id="trPassword" runat="server" visible="false">
				    <td>
				        <asp:Localize ID="locPublicPassword" runat="server" meta:resourcekey="locPublicPassword" />				    
				    </td>
				    <td>
				        <asp:TextBox ID="tbPassword" runat="server" 
				            EnableTheming="false"
				        />
				    </td>
				</tr>
			</table>
			<center>
			    <asp:RequiredFieldValidator ID="requiredPassword" runat="server"
			         ControlToValidate="tbPassword" 
			         CssClass="control-errorlabel" Display="Dynamic" Text="Missing iformation - public password." 
			         ErrorMessage="Missing iformation - public password."
			         Enabled="true"
			    />
			</center>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Panel>
