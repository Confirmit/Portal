<%@ Control Language="C#" AutoEventWireup="true" Inherits="MLStringTextBox" Codebehind="MLStringTextBox.ascx.cs" %>

<asp:Panel ID="MLStringPanel" runat="server">
	<asp:UpdatePanel ID="upMLText" runat="server" ChildrenAsTriggers="true">
		<ContentTemplate>
			<div style="width: 100%">
			    <div style="float: left; width: 100px;" >
				    <asp:DropDownList ID="DropDownListCultures" Width="100%" 
				        DataTextField="NativeName" 
				        DataValueField="Name"
					    AutoPostBack="true" 
					    runat="server" 
					    OnSelectedIndexChanged="OnCultureChanged" 
			        />
			    </div>
			    <div style="float: left; padding-left: 5px;">
				    <asp:TextBox ID="TextBoxContent" runat="server" />
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Panel>
