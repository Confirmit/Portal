<%@ Control Language="C#" AutoEventWireup="true" Inherits="BookThemesGrid" Codebehind="BookThemesGrid.ascx.cs" %>

<%@ Register Assembly="Controls" Namespace="Controls.HotGridView" TagPrefix="cc1" %> 
<%@ Register Src="~/controls/ActionsMenu/ActionsMenuCtl.ascx" TagName="ActionsMenu" TagPrefix="uc1" %>

<div class="control">
    <uc1:ActionsMenu ID="menuActions" 
        runat="server" 
        ViewName="book_themes_grid_action_menu"
        MenuForAnyPossibleActionsCriteria="./controls/ActionsMenu/d.xml" />    

    <div class="control-body" style="padding:0;">
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <cc1:GridView ID="gridViewBookThemes" runat="server" 
                    Width="100%" CssClass="gridview" PageSize="5" 
                    AllowPaging="True" AllowSorting="True" 
                    AutoGenerateColumns="False"
                    AutoGenerateCheckBoxColumn="True"
                    CellPadding="3" GridLines="None"
                    UseCustomPager = "true"
                    RightArrowDisableImg="~/Images/GridView/pgarrow_right_disabled.gif"
                    RightArrowEnableImg="~/Images/GridView/pgarrow_right_enabled.gif"
                    LeftArrowDisableImg="~/Images/GridView/pgarrow_left_disabled.gif"
                    LeftArrowEnableImg="~/Images/GridView/pgarrow_left_enabled.gif"                        
                    RowIdPropertyName="ID"
                    DataKeyNames="ID"
                    DataSourceID="dsBookThemes" 
                >
                    <HeaderStyle CssClass="gridview-headerrow" />
                    <PagerSettings NextPageText="Next" PreviousPageText="Prev"/>
                    
                    <EmptyDataTemplate>
                        <center>
                            No themes..
                        </center>
                    </EmptyDataTemplate>
                    
                    <Columns>
                        <asp:BoundField SortExpression="Name" DataField="Name" meta:resourcekey="headerName">
                            <ItemStyle Width="90%" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <cc1:BoundSelectionField
                            DataField="ID" 
                            ImageUrl="~/Images/announce/add_blue.gif"
                            IdPropertyName="ID"  
                            >
                            <ItemStyle HorizontalAlign="Right" />
                        </cc1:BoundSelectionField>
                    </Columns>
                        <RowStyle CssClass="gridview-row"/>
                        <EditRowStyle BackColor="#EEEEEE" Height="20"/>
                        <SelectedRowStyle CssClass="gridview-selectedrow"/>
                        <PagerStyle CssClass="gridview-pagerrow"/>
                        <AlternatingRowStyle CssClass="gridview-alternatingrow"/>
                        <FooterStyle CssClass="gridview-footer"  />
                </cc1:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>

<asp:ObjectDataSource ID="dsBookThemes" runat="server" 
    EnablePaging="True" 
    SelectMethod="Select"
    SelectCountMethod="SelectCount"
    TypeName="ConfirmIt.PortalLib.BusinessObjects.RequestObjects.BookThemesDataSource"
    SortParameterName="SortExpression" 
    MaximumRowsParameterName="maximumRows" 
    StartRowIndexParameterName="startRowIndex"
    >
</asp:ObjectDataSource>