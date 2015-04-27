<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ObjectsOnHandsGrid.ascx.cs" Inherits="ObjectsOnHandsGrid" %>

<%@ Register Assembly="Controls" Namespace="Controls.HotGridView" TagPrefix="cc1" %>

<cc1:GridView ID="gridViewOnHands" runat="server" DataSourceID="dsObjectsOnHand"
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
            <%# (string)GetLocalResourceObject("NothingOnHands") %>
        </center>
    </EmptyDataTemplate>
                    
    <Columns>
        <asp:TemplateField SortExpression="ObjType" meta:resourcekey="ObjType">
            <ItemStyle Width="15%" />
            <HeaderStyle HorizontalAlign="Left" />
            
            <ItemTemplate>
                <asp:Image ID="imgType" runat="server" Width="25" Height="25" />
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:BoundField SortExpression="Title" meta:resourcekey="Title" DataField="Title" >
            <ItemStyle Width="60%" />
            <HeaderStyle HorizontalAlign="Left" />
        </asp:BoundField>
                                
        <asp:BoundField meta:resourcekey="OwnerFirstName" >
            <ItemStyle Width="20%" />
            <HeaderStyle HorizontalAlign="Left" />
        </asp:BoundField>
    </Columns>
</cc1:GridView>
                            
<asp:SqlDataSource ID="dsObjectsOnHand" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnStr %>"
    SelectCommand="GetObjectsOnHand" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter Name="userId" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>