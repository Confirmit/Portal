<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntitiesListControl.ascx.cs" Inherits="Portal.Controls.EntitiesManipulationControls.EntitiesListControl" %>

<script src="../../Scripts/entitiesManipulations.js" type="text/javascript"></script>

<div style="padding: 5px;">
    <div style="margin: 5px; text-align: center;">
       <input id="UncheckAllCheckoboxesButton" runat="server" type="button" value="Uncheck All" />
       <input id="CheckAllCheckoboxesButton" runat="server" type="button" value="Check All" />
    </div>
    <asp:GridView ID="EntitiesListGridView" runat="server"
        AutoGenerateColumns="True" Width="350px">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="EntitySelectionCheckBox" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <RowStyle HorizontalAlign="Center" BorderWidth="1px" BorderStyle="Solid" />
    </asp:GridView>
</div>