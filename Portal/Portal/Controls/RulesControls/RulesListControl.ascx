<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RulesListControl.ascx.cs" Inherits="Portal.Controls.RulesControls.RulesListControl" %>

<asp:GridView ID="RulesListGridView" runat="server" CssClass="griduserlist"
    AutoGenerateColumns="False" Width="650px" OnRowDataBound="RulesListGridView_OnRowDataBound">
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="Rule ID:">
            <ItemStyle BorderStyle="Solid" BorderWidth="1" />
        </asp:BoundField>
        <asp:BoundField DataField="BeginTime" HeaderText="BeginTime:">
            <ItemStyle BorderStyle="Solid" BorderWidth="1" />
        </asp:BoundField>
        <asp:BoundField DataField="EndTime" HeaderText="EndTime:">
            <ItemStyle BorderStyle="Solid" BorderWidth="1" />
        </asp:BoundField>
        <asp:BoundField DataField="TypeId" HeaderText="TypeId:">
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
