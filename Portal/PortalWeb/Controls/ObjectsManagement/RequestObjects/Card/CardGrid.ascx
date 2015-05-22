<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CardGrid.ascx.cs" Inherits="CardGrid" %>

<%@ Register Src="~/controls/ActionsMenu/ActionsMenuCtl.ascx" TagName="ActionsMenu" TagPrefix="uc1" %>
<%@ Register Assembly="Controls" Namespace="Controls.HotGridView" TagPrefix="cc1" %>

<div class="control">
    <uc1:ActionsMenu ID="menuActions" 
        runat="server" 
        ViewName="req_objects_grid_action_menu"
        MenuForAnyPossibleActionsCriteria="./controls/ActionsMenu/d.xml" />   
        
    <cc1:GridView ID="gridViewReqObjects" runat="server" 
        CssClass="gridview" AutoGenerateColumns="False"
        DataSourceID="dsFilterReqObjects"
        AllowPaging="True" AllowSorting="true"  
        Width="100%" PageSize="10" 
        ShowHeader="true" ShowFooter="True" 
        EnableTheming="false"
        GridLines="None"
        UseCustomPager = "true"
        AutoGenerateCheckBoxColumn="true"
        RowIdPropertyName="Id"
        RightArrowDisableImg="~/Images/GridView/pgarrow_right_disabled.gif"
        RightArrowEnableImg="~/Images/GridView/pgarrow_right_enabled.gif"
        LeftArrowDisableImg="~/Images/GridView/pgarrow_left_disabled.gif"
        LeftArrowEnableImg="~/Images/GridView/pgarrow_left_enabled.gif"
        >
        <HeaderStyle CssClass="gridview-headerrow" HorizontalAlign="Left" />
        <RowStyle CssClass="gridview-row" Height="20" />
        <SelectedRowStyle CssClass="gridview-selectedrow" />
        <AlternatingRowStyle CssClass="gridview-alternatingrow" />
        <PagerStyle CssClass="gridview-pagerrow" />
        <PagerSettings NextPageText="Next" PreviousPageText="Prev"/>
                                    
        <EmptyDataTemplate>
            <center>
                No data...
            </center>
        </EmptyDataTemplate>
                        
        <Columns>        
            <asp:BoundField SortExpression="Title" meta:resourcekey="Title" DataField="Title" >
                <ItemStyle Width="80%" />
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
                                    
            <asp:BoundField SortExpression="ValuePercent" meta:resourcekey="ValuePercent" DataField="ValuePercent" >
                <ItemStyle Width="20%" />
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            
            <cc1:BoundSelectionField
                DataField="ID" 
                ImageUrl="~/Images/announce/add_blue.gif"
                IdPropertyName="ID">
                <ItemStyle HorizontalAlign="Right" />
            </cc1:BoundSelectionField>
        </Columns>
    </cc1:GridView>
</div>

<asp:ObjectDataSource ID="dsFilterReqObjects" runat="server" 
    TypeName="ConfirmIt.PortalLib.BusinessObjects.RequestObjects.RequestObjectDataSource"
    EnablePaging="True" 
    SelectMethod="Select"
    SelectCountMethod="SelectCount"
    SortParameterName="SortExpression" 
    MaximumRowsParameterName="maximumRows" 
    StartRowIndexParameterName="startRowIndex"
    >
</asp:ObjectDataSource>