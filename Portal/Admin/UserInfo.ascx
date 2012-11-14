<%@ Control Language="C#" AutoEventWireup="true"
	Inherits="Admin_UserInfol" Codebehind="UserInfo.ascx.cs" %>

<%@ Register Src="~/Controls/MLTextBox.ascx" TagPrefix="uc1" TagName="MLTextBox" %>
<%@ Register Src="~/Controls/DomainNamesEditor.ascx" TagPrefix="uc1" TagName="DNEditor" %>
<%@ Register Src="~/Controls/GroupsMembershipControl.ascx" TagPrefix="uc1" TagName="GroupsEditor" %>
	
<asp:Wizard ID="wzrdUserInfo" runat="server" Width="100%" DisplaySideBar="False"
	StartNextButtonType="Link" 
	FinishPreviousButtonType="Link"
	FinishCompleteButtonText="" 
	FinishCompleteButtonType="Link"
	OnActiveStepChanged="wzrdUserInfo_ActiveStepChanged" 
	meta:resourcekey="Wizard"
>
	<WizardSteps>
		<asp:WizardStep runat="server" meta:resourcekey="Profile">
			<table style="width: 100%" border="0px">
				<tr>
					<td style="width: 15%">
						<asp:Localize ID="locFirstName" runat="server" meta:resourcekey="locFirstName" />
					</td>
					<td style="width: 100%">
						<uc1:MLTextBox ID="tbFirstName" runat="server" Width="100%" />
					</td>
					<td style="width: 60px;" />
				</tr>
				<tr>
					<td>
						<asp:Localize ID="locMiddleName" runat="server" meta:resourcekey="locMiddleName" />
					</td>
					<td>
						<uc1:MLTextBox ID="tbMiddleName" runat="server" Width="100%" />
					</td>
					<td></td>
				</tr>
				<tr>
					<td>
						<asp:Localize ID="locLastName" runat="server" meta:resourcekey="locLastName" />
					</td>
					<td>
						<uc1:MLTextBox ID="tbLastName" runat="server" Width="100%" />
					</td>
					<td></td>
				</tr>
				<tr>
					<td>
						<asp:Localize ID="locSex" runat="server" meta:resourcekey="locSex" />
					</td>
					<td>
						<asp:DropDownList ID="ddlSex" Width="100%" runat="server" />
					</td>
					<td></td>
				</tr>
				<tr>
					<td>
						<asp:Localize ID="locBirthday" runat="server" meta:resourcekey="locBirthday" />
					</td>
					<td>
						<asp:TextBox ID="tbBirthday" Width="99%" runat="server" />
					</td>
					<td></td>
				</tr>
				<tr>
					<td>
						<asp:Localize ID="locDomainName" runat="server" meta:resourcekey="locDomainName" />
					</td>
					<td>
						<uc1:DNEditor ID="dnEditor" runat="server" />
					</td>
					<td></td>
				</tr>
				<tr>
					<td>
						<asp:Localize ID="locPrimaryEMail" runat="server" meta:resourcekey="locPrimaryEMail" />
					</td>
					<td>
						<asp:TextBox ID="tbPrimaryEMail" Width="99%" runat="server" />
					</td>
					<td></td>
				</tr>
				<tr>
					<td>
						<asp:Localize ID="locProject" runat="server" meta:resourcekey="locProject" />
					</td>
					<td>
						<asp:TextBox ID="tbProject" Width="99%" runat="server" />
					</td>
					<td></td>
				</tr>
				<tr>
					<td>
						<asp:Localize ID="locRoom" runat="server" meta:resourcekey="locRoom" />
					</td>
					<td>
						<asp:TextBox ID="tbRoom" Width="99%" runat="server" />
					</td>
					<td></td>
				</tr>
				<tr>
					<td>
						<asp:Localize ID="locPrimaryIP" runat="server" meta:resourcekey="locPrimaryIP" />
					</td>
					<td>
						<asp:TextBox ID="tbPrimaryIP" Width="99%" runat="server" />
					</td>
					<td></td>
				</tr>
			</table>
		</asp:WizardStep>
		
		<asp:WizardStep runat="server" meta:resourcekey="Roles">
            <uc1:GroupsEditor ID="gmEditor" runat="server" />
		</asp:WizardStep>
		
	</WizardSteps>
</asp:Wizard>

<div class="control-line-between"></div>
<div style="white-space: nowrap; width: 100%; text-align: center">
	<asp:Button ID="btnApply" CssClass="control-button" runat="server" 
	    OnClick="btnApply_Click" 
	    meta:resourcekey="btnApply" Width="100" 
	    TabIndex="0"
	/>
	<asp:Button ID="btnCancel" CssClass="control-button" runat="server" 
	    OnClick="btnCancel_Click"
		meta:resourcekey="btnCancel" Width="100"
		CausesValidation="false" 
	/>
</div>
