<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ObjectHistoryGrid.ascx.cs" Inherits="ObjectHistoryGrid" %>

<%@ Register Assembly="Controls" Namespace="Controls.HotGridView" TagPrefix="cc1" %>

<cc1:GridView ID="gridViewObjectHistory" runat="server" DataSourceID="dsObjectHistory"
    CssClass="gridview" AutoGenerateColumns="False"
    AllowPaging="True" AllowSorting="true"  
    Width="100%" PageSize="5" 
    ShowHeader="true" ShowFooter="True" 
    EnableTheming="false"
    GridLines="None"
    UseCustomPager = "true"
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
            <%# (string)GetLocalResourceObject("NoHistory")%>
        </center>
    </EmptyDataTemplate>
                    
    <Columns>
        <asp:BoundField SortExpression="LastName" meta:resourcekey="UserName" >
            <ItemStyle Width="60%" />
            <HeaderStyle HorizontalAlign="Left" />
        </asp:BoundField>

        <asp:BoundField SortExpression="Date" meta:resourcekey="Date" DataField="Date" >
            <ItemStyle Width="20%" />
            <HeaderStyle HorizontalAlign="Left" />
        </asp:BoundField>
                                
        <asp:BoundField meta:resourcekey="TakenOrGranted" >
            <ItemStyle Width="20%" />
            <HeaderStyle HorizontalAlign="Left" />
        </asp:BoundField>
    </Columns>
</cc1:GridView>

<asp:ObjectDataSource ID="dsObjectHistory" runat="server" 
    EnablePaging="True" 
    SelectMethod="Select"
    SelectCountMethod="SelectCount"
    TypeName="ConfirmIt.PortalLib.BusinessObjects.RequestObjects.RequestObjectHistoryDataSource"
    SortParameterName="SortExpression" 
    MaximumRowsParameterName="maximumRows" 
    StartRowIndexParameterName="startRowIndex"
    >                
</asp:ObjectDataSource>
