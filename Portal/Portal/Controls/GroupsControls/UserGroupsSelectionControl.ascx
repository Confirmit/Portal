<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserGroupsSelectionControl.ascx.cs" Inherits="Portal.Controls.GroupsControls.UserGroupsSelectionControl" %>


<asp:Label ID="SelectedGroupLabel" runat="server"/>
<asp:GridView ID="UserGroupsSelectionGridView" runat="server" CssClass="griduserlist"
    AutoGenerateColumns="False" Width="650px">
    <Columns>
        <asp:BoundField DataField="UserID" HeaderText="User ID:">
            <ItemStyle BorderStyle="Solid" BorderWidth="1" />
        </asp:BoundField>
        <asp:BoundField DataField="UserName" HeaderText="User Name:">
            <ItemStyle BorderStyle="Solid" BorderWidth="1" />
        </asp:BoundField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:CheckBox ID="UserContainsInGroupCheckBox" runat="server"  /> 
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <RowStyle HorizontalAlign="Center" BorderWidth="1px" BorderStyle="Solid" />
</asp:GridView>