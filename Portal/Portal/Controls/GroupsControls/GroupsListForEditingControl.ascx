<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupsListForEditingControl.ascx.cs" Inherits="Portal.Controls.GroupsControls.GroupsListForEditingControl" %>

<div style="margin-bottom: 15px;">
    <asp:GridView ID="GroupsEditingGridView" runat="server" CssClass="griduserlist" DataKeyNames="ID"
        AutoGenerateColumns="False" Width="650px">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="Group ID:">
                <ItemStyle BorderStyle="Solid" BorderWidth="1" />
            </asp:BoundField>
            <asp:BoundField DataField="Description" HeaderText="Description:">
                <ItemStyle BorderStyle="Solid" BorderWidth="1" />
            </asp:BoundField>
            <asp:CommandField ButtonType="Image"
                SelectImageUrl="~/Images/announce/add_blue.gif"
                ShowSelectButton="True">
                <ItemStyle HorizontalAlign="Center" Width="20px" />
            </asp:CommandField>

            <asp:CommandField ButtonType="Image" 
                DeleteImageUrl="~/Images/attachments/delete.gif"
                ShowDeleteButton="true">
                <ItemStyle HorizontalAlign="Center" Width="20px" />
            </asp:CommandField>
        </Columns>
        <RowStyle HorizontalAlign="Center" BorderWidth="1px" BorderStyle="Solid" />
    </asp:GridView>
</div>
