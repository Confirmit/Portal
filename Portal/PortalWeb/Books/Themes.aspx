<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    CodeFile="Themes.aspx.cs" Inherits="ConfirmIt.Portal.Books.ThemesPage" meta:resourcekey="Page" %>

<%@ Register Src="~/Controls/MLTextBox.ascx" TagPrefix="usp" TagName="MLTextBox" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <asp:ScriptManagerProxy ID="scriptManagerProxy" runat="server" />
    <asp:UpdatePanel ID="upThemes" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:ObjectDataSource ID="dsThemes" runat="server" DeleteMethod="DeleteBookTheme"
                SelectMethod="GetAllBookThemesSorted" TypeName="ConfirmIt.PortalLib.BAL.BookTheme"
                SortParameterName="sortExpr">
                <DeleteParameters>
                    <asp:Parameter Name="id" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="sortExpr" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gvThemes" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                Width="80%" DataSourceID="dsThemes" DataKeyNames="ID" OnRowCreated="gvThemes_RowCreated"
                OnRowDeleted="gvThemes_RowDeleted" OnSelectedIndexChanged="gvThemes_SelectedIndexChanged">
                <EmptyDataTemplate>
                    <%# (string) this.GetLocalResourceObject( "NoThemes" ) %>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField SortExpression="Name" meta:resourcekey="ThemeName">
                        <ItemTemplate>
                            <div style="text-align: left">
                                <asp:Label ID="lblThemeName" runat="server" Text='<%# Eval( "Name" ) %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/Edit.gif" ShowSelectButton="True"
                        meta:resourcekey="Select">
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:CommandField>
                    <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" ShowDeleteButton="True"
                        meta:resourcekey="Delete">
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:CommandField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="dsSelectedTheme" runat="server" InsertMethod="CreateBookTheme"
                SelectMethod="GetThemeByID" TypeName="ConfirmIt.PortalLib.BAL.BookTheme" UpdateMethod="UpdateBookTheme">
                <InsertParameters>
                    <asp:Parameter Name="name" Type="Object" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="id" Type="Int32" />
                    <asp:Parameter Name="name" Type="Object" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="gvThemes" Name="id" PropertyName="SelectedValue"
                        Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:DetailsView ID="dvBookTheme" runat="server" AutoGenerateRows="False" Height="50px"
                Width="80%" DefaultMode="Insert" DataSourceID="dsSelectedTheme" DataKeyNames="ID"
                meta:resourcekey="ThemeDetails" OnItemCommand="dvBookTheme_ItemCommand" OnItemInserted="dvBookTheme_ItemInserted"
                OnItemInserting="dvBookTheme_ItemInserting" OnDataBound="dvBookTheme_DataBound"
                OnItemUpdated="dvBookTheme_ItemUpdated" OnItemUpdating="dvBookTheme_ItemUpdating">
                <Fields>
                    <asp:TemplateField meta:resourcekey="ThemeName">
                        <ItemTemplate>
                            <asp:Label ID="lblThemeName" runat="server" Text='<%# Eval( "Name" ) %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <usp:MLTextBox ID="mltbThemeName" runat="server" Width="90%" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="True" ShowInsertButton="True" meta:resourcekey="Command" />
                </Fields>
            </asp:DetailsView>
            <asp:Panel ID="pnlErrorDescription" runat="server" Width="80%">
                <asp:Label ID="lblErrorDescription" runat="server" ForeColor="Red" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
