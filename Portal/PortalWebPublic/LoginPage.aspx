<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Main.master"
	CodeFile="LoginPage.aspx.cs" Inherits="LoginPage" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<table border="0px" width="100%">
		<tr>
			<td align="center">
				<asp:Login ID="logIn" runat="server" 
				    OnAuthenticate="OnLogin" 
				    meta:resourcekey="logIn"
				>
					<TitleTextStyle CssClass="control-label" Font-Bold="true" />
					<LabelStyle CssClass="control-label" Font-Bold="true" />
					<TextBoxStyle CssClass="control-textbox-required" />
					<LoginButtonStyle CssClass="control-button" />
					<CheckBoxStyle CssClass="control-label" />
					<FailureTextStyle CssClass="control-errorlabel" />
				</asp:Login>
			</td>
		</tr>
	</table>
</asp:Content>