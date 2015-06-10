<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_CultureChanger" Codebehind="CultureChanger.ascx.cs" %>

<table>
	<tr>
		<td valign="middle" class="control-label">
			<asp:Localize ID="locChangeText" runat="server" meta:resourcekey="changeText" />
		</td>
		<td>
			<asp:DropDownList ID="DropDownListCultures" runat="server" 
				Width="100" CssClass="control-dropdownlist"
				DataTextField="NativeName" DataValueField="Name" 
				AutoPostBack="true" />
		</td>
	</tr>
</table>