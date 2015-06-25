<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupsListInRuleControl.ascx.cs" Inherits="Portal.Controls.RulesControls.GroupsListInRuleControl" %>

<div style="padding: 5px;">
    <div style="margin: 5px; text-align: center;">
       <input id="UncheckAllCheckoboxesButton" runat="server" type="button" value="Uncheck All" />
       <input id="CheckAllCheckoboxesButton" runat="server" type="button" value="Check All" />
    </div>
    <asp:GridView ID="GroupsRuleSelectionGridView" runat="server"
        AutoGenerateColumns="False" Width="350px">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="Group ID:">
                <ItemStyle BorderStyle="Solid" BorderWidth="1" />
            </asp:BoundField>
            <asp:BoundField DataField="Description" HeaderText="Description:">
                <ItemStyle BorderStyle="Solid" BorderWidth="1" />
            </asp:BoundField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="GroupContainingInRuleCheckBox" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <RowStyle HorizontalAlign="Center" BorderWidth="1px" BorderStyle="Solid" />
    </asp:GridView>
</div>
