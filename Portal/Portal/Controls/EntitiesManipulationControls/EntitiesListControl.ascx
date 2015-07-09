<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntitiesListControl.ascx.cs" Inherits="Portal.Controls.EntitiesManipulationControls.EntitiesListControl" %>

<script src="../../Scripts/entitiesManipulations.js" type="text/javascript"></script>

<div style="padding: 5px;">
    <asp:GridView ID="EntitiesListGridView" runat="server" AutoGenerateColumns="False"
        ShowHeaderWhenEmpty="True" Width="350px" OnRowDataBound="EntitiesGridViewOnRowDataBound">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:CheckBox ID="SelectAllCheckboxes" runat="server"/>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="EntitySelectionCheckBox" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <RowStyle HorizontalAlign="Center" BorderWidth="1px" BorderStyle="Solid" />
    </asp:GridView>
</div>
