<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SubscribeEvents.ascx.cs" Inherits="SubscribeEvents" %>

<%@ Register Assembly="Controls" Namespace="Controls.HotGridView" TagPrefix="cc1" %>

<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>
        <cc1:GridView ID="gridViewSubscribeEvents" runat="server" 
            CssClass="gridview" AutoGenerateColumns="False"
            DataSourceID="dsEventsForSubscribe"
            AllowPaging="True" AllowSorting="true"  
            DataKeyNames="ID" Width="100%"
            ShowHeader="true" ShowFooter="True" 
            EnableTheming="false"
            GridLines="None"
            PageSize="5" 
            >
                <HeaderStyle CssClass="gridview-headerrow" HorizontalAlign="Left" />
                <RowStyle CssClass="gridview-row" Height="20" />
                <SelectedRowStyle CssClass="gridview-selectedrow" />
                <AlternatingRowStyle CssClass="gridview-alternatingrow" />
                <PagerStyle CssClass="gridview-pagerrow" />
                <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                
                <EmptyDataTemplate>
                    <center>
                        No events..
                    </center>
                </EmptyDataTemplate>
                    
                <Columns>
                    <asp:BoundField SortExpression="Date" meta:resourcekey="dateHeader" DataField="DateTime" >
                        <ItemStyle Width="30%" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField SortExpression="Title" meta:resourcekey="titleHeader" DataField="Title" >
                        <ItemStyle Width="30%" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                                
                    <asp:BoundField SortExpression="Description" meta:resourcekey="descriptionHeader" DataField="Description" >
                        <ItemStyle Width="30%" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    
                    <cc1:BoundSelectionField 
                        DataField="ID" 
                        ImageUrl="~/Images/date_link.png"
                        IdPropertyName="ID" 
                        />                                                            
                </Columns>
        </cc1:GridView>
    </ContentTemplate>
</asp:UpdatePanel>    

<asp:ObjectDataSource ID="dsEventsForSubscribe" runat="server" 
    EnablePaging="True" 
    SelectMethod="Select"
    SelectCountMethod="SelectCount"
    TypeName="UlterSystems.PortalLib.BusinessObjects.UserSubscribeEventsDataSource"
    SortParameterName="SortExpression" 
    MaximumRowsParameterName="maximumRows" 
    StartRowIndexParameterName="startRowIndex"
    >                
</asp:ObjectDataSource>