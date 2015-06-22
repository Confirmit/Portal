<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserListInGroupControl.ascx.cs" Inherits="Portal.Controls.GroupsControls.UserListInGroupControl" %>

<div style="margin-left: 10px;">
    <asp:GridView ID="UserGroupsSelectionGridView" runat="server"
        AutoGenerateColumns="False" Width="350px">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="User ID:">
                <ItemStyle BorderStyle="Solid" BorderWidth="1" />
            </asp:BoundField>
            <asp:BoundField DataField="FirstName" HeaderText="User Name:">
                <ItemStyle BorderStyle="Solid" BorderWidth="1" />
            </asp:BoundField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="UserContainsInGroupCheckBox" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <RowStyle HorizontalAlign="Center" BorderWidth="1px" BorderStyle="Solid" />
    </asp:GridView>
</div>
