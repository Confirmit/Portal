<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RulesListControl.ascx.cs" Inherits="Portal.Controls.RulesControls.RulesListControl" %>

<asp:GridView ID="RulesListGridView" runat="server" CssClass="griduserlist"
    AutoGenerateColumns="False" Width="650px">
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
    </Columns>
    <RowStyle HorizontalAlign="Center" BorderWidth="1px" BorderStyle="Solid" />
</asp:GridView>