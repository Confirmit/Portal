<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master"
	AutoEventWireup="true" CodeFile="AdminRolesPage.aspx.cs" Inherits="Admin_AdminRolesPage"
	meta:resourcekey="Page" %>

<%@ Register Src="~/Controls/MLTextBox.ascx" TagPrefix="usp"
	TagName="MLTextBox" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder"
	runat="Server">
	<asp:ObjectDataSource ID="dsRoles" runat="server" TypeName="ConfirmIt.PortalLib.BAL.Role"
		SelectMethod="GetAllRoles" DeleteMethod="DeleteRoleByRoleID">
		<DeleteParameters>
			<asp:Parameter Name="roleID" Type="String" />
		</DeleteParameters>
	</asp:ObjectDataSource>
	<asp:GridView ID="gvRoles" runat="server" AutoGenerateColumns="False"
		DataSourceID="dsRoles" Width="80%" DataKeyNames="RoleID" OnRowCreated="gvRoles_RowCreated"
		OnSelectedIndexChanged="gvRoles_SelectedIndexChanged" OnRowDeleted="gvRoles_RowDeleted">
		<EmptyDataTemplate>
		</EmptyDataTemplate>
		<Columns>
			<asp:TemplateField meta:resourcekey="RoleID">
				<ItemTemplate>
					<div style="text-align: left">
						<asp:Label ID="lblRoleID" runat="server" Text='<%# Eval( "RoleID" ) %>' />
					</div>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="RoleName">
				<ItemTemplate>
					<div style="text-align: left">
						<asp:Label ID="lblName" runat="server" Text='<%# Eval( "Name" ) %>' />
					</div>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="RoleDescription">
				<ItemTemplate>
					<div style="text-align: left">
						<asp:Label ID="lblDescription" runat="server" Text='<%# Eval( "Description" ) %>' />
					</div>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/Edit.gif"
				ShowSelectButton="True" meta:resourcekey="Select">
				<ItemStyle HorizontalAlign="Center" Width="20px" />
			</asp:CommandField>
			<asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif"
				ShowDeleteButton="True" meta:resourcekey="Delete">
				<ItemStyle HorizontalAlign="Center" Width="20px" />
			</asp:CommandField>
		</Columns>
	</asp:GridView>
	<asp:ObjectDataSource ID="dsSelectedRole" runat="server" SelectMethod="GetRole"
		TypeName="ConfirmIt.PortalLib.BAL.Role" InsertMethod="CreateRole"
		UpdateMethod="UpdateRole">
		<SelectParameters>
			<asp:ControlParameter ControlID="gvRoles" Name="roleID" PropertyName="SelectedValue"
				Type="String" />
		</SelectParameters>
		<InsertParameters>
			<asp:Parameter Name="roleID" Type="String" />
			<asp:Parameter Name="name" Type="Object" />
			<asp:Parameter Name="description" Type="Object" />
		</InsertParameters>
		<UpdateParameters>
			<asp:Parameter Name="id" Type="Int32" />
			<asp:Parameter Name="roleID" Type="String" />
			<asp:Parameter Name="name" Type="Object" />
			<asp:Parameter Name="description" Type="Object" />
		</UpdateParameters>
	</asp:ObjectDataSource>
	<asp:DetailsView ID="dvSelectedRole" runat="server" AutoGenerateRows="False"
		DataSourceID="dsSelectedRole" DataKeyNames="ID" Width="80%"
		Height="50px" DefaultMode="Insert" meta:resourcekey="RoleDetails"
		OnItemInserted="dvSelectedRole_ItemInserted" OnItemCommand="dvSelectedRole_ItemCommand"
		OnDataBound="dvSelectedRole_DataBound" OnItemInserting="dvSelectedRole_ItemInserting"
		OnItemUpdating="dvSelectedRole_ItemUpdating" OnItemUpdated="dvSelectedRole_ItemUpdated">
		<Fields>
			<asp:TemplateField meta:resourcekey="RoleID">
				<ItemTemplate>
					<asp:Label ID="lblRoleID" runat="server" Text='<%# Eval( "RoleID" ) %>' />
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox ID="tbRoleID" runat="server" Text='<%# Bind( "RoleID" ) %>'
						Width="90%" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="RoleName">
				<ItemTemplate>
					<asp:Label ID="lblRoleName" runat="server" Text='<%# Eval( "Name" ) %>' />
				</ItemTemplate>
				<EditItemTemplate>
					<usp:MLTextBox ID="mltbRoleName" runat="server" Width="90%" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="RoleDescription">
				<ItemTemplate>
					<asp:Label ID="lblRoleDescription" runat="server" Text='<%# Eval( "Description" ) %>' />
				</ItemTemplate>
				<EditItemTemplate>
					<usp:MLTextBox ID="mltbRoleDescription" runat="server" Width="90%" />
				</EditItemTemplate>
			</asp:TemplateField>
			<asp:CommandField ButtonType="Link" ShowCancelButton="true" ShowEditButton="true"
				ShowInsertButton="true" meta:resourcekey="Command" />
		</Fields>
	</asp:DetailsView>
	<asp:Panel ID="pnlErrorDescription" runat="server" Width="80%">
		<asp:Label ID="lblErrorDescription" runat="server" ForeColor="Red" />
	</asp:Panel>
</asp:Content>
