<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminGroupsEditingControl.ascx.cs" Inherits="Portal.Controls.AdminGroupsEditingControl" %>

<asp:GridView ID="GroupsEditingGridView" runat="server" CssClass="griduserlist"
    AutoGenerateColumns="False"
    OnRowDataBound="OnGroupRowBound" Width="650px">
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="Group ID:">
            <ItemStyle BorderStyle="Solid" BorderWidth="1" />
        </asp:BoundField>
        <asp:BoundField DataField="Description" HeaderText="Description:">
            <ItemStyle BorderStyle="Solid" BorderWidth="1" />
        </asp:BoundField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:ImageButton ID="EditGroupImageButton" runat="server"
                    OnClick="EditGroupImageButton_OnClick"
                    ImageUrl="~/Images/menu/save.gif"
                    Height="17" Width="17" />
                <asp:ImageButton ID="RemoveGroupImageButton" runat="server"
                    OnClick="RemoveGroupImageButton_OnClick"
                    ImageUrl="~/Images/menu/delete.gif"
                    Height="17" Width="17" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <RowStyle HorizontalAlign="Center" BorderWidth="1px" BorderStyle="Solid" />
</asp:GridView>