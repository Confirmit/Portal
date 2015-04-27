<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DropDownExtender.ascx.cs" Inherits="DropDownExtender" %>

<asp:Label ID="lblCaption" runat="server" Text="dropDown" Width="120" />

<asp:Panel ID="dropPanel" runat="server" 
    Style="display :none; visibility: hidden;"
    CssClass="context-menu-panel" 
>
    <asp:DataGrid ID="gridView" runat="server"
        EnableTheming="false"
        AutoGenerateColumns="false"
        GridLines="None"
        ShowHeader="false"
        DataKeyField="<%#DataValueField %>"
    >
        <Columns>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:LinkButton ID="linkItem" runat="server" 
                        CssClass="context-menu-item"
                        OnClick="OnSelectItemEvent" 
                        Text='<%# DataBinder.Eval(Container.DataItem, DataTextField) %>'
                        CommandName="ObjectID"
                        CommandArgument="<%# DataBinder.Eval(Container.DataItem, DataValueField) %>"
                    />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right" />
           </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</asp:Panel>

<ajaxToolkit:DropDownExtender runat="server" ID="DDE"
    TargetControlID="lblCaption" 
    DropDownControlID="dropPanel" 
/>
