﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UsersGrid.ascx.cs" Inherits="UsersGrid" %>
        
<div align="center" >
    <div style="width: 80%" class="control-border" >
        <asp:UpdatePanel ID="upPanel" runat="server">
            <ContentTemplate>            
                <asp:GridView ID="gridViewUsers" runat="server" 
                    DataSourceID="objectDataSourcePersons" 
                    DataKeyNames="ID"
                    AllowPaging="True" AllowSorting="True" 
                    AutoGenerateColumns="False" ShowFooter="True" 
                    EnableTheming="false" GridLines="None" Width="100%" 
                    onselectedindexchanged="gvGridUser_SelectedIndexChanged"
                    OnPageIndexChanging="gvGridUser_PageIndexChanging" 
                    ondatabound="gvGridUser_DataBound" 
                    OnRowDataBound="OnRowDataBound"
                    CssClass="gridview"
                    >
                    <HeaderStyle CssClass="gridview-headerrow" HorizontalAlign="Left" />
                    <RowStyle CssClass="gridview-row" />
                    <SelectedRowStyle CssClass="gridview-selectedrow" />
                    <AlternatingRowStyle CssClass="gridview-alternatingrow" />
                    <PagerStyle CssClass="gridview-pagerrow" />
                    
                    <EmptyDataTemplate>
                        <%# (string) GetLocalResourceObject( "NoUsers" ) %>
                    </EmptyDataTemplate>
                
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                    <PagerTemplate>
                        <div style="width: 100%">
                            <table width="100%">
                                <tr>
                                    <td align="center" style="width: 70%">
                                        <asp:LinkButton ID="lbntFirst" runat="server" 
                                            CommandArgument="First" CommandName="Page"
                                            CssClass="control-hyperlink" 
                                            Text="&lt;&lt;" />
                                        
                                        <asp:LinkButton ID="lbtnPrev" runat="server" 
                                            CommandArgument="Prev" CommandName="Page"
                                            CssClass="control-hyperlink"  
                                            Text="&lt;" />
                                    
                                        <asp:Literal ID="lblPage" runat="server" meta:resourcekey="PageIndex" />
                                        <asp:DropDownList ID="ddlPage" runat="server" 
                                            AutoPostBack="true" 
                                            OnSelectedIndexChanged="OnPageIndexChanged" 
                                            Width="100"
                                            CssClass="control-dropdownlist"  />
                                    
                                        <asp:Literal ID="lblFrom" runat="server" meta:resourcekey="PageFrom" />
                                        <asp:Literal ID="lblPageCount" runat="server" />
                                        
                                        <asp:LinkButton ID="lbtnNext" runat="server" 
                                            CommandArgument="Next" CommandName="Page" 
                                            Text="&gt;" 
                                            CssClass="control-hyperlink" />
                                            
                                        <asp:LinkButton ID="lbtnLast" runat="server" 
                                            CommandArgument="Last" CommandName="Page" 
                                            Text="&gt;&gt;" 
                                            CssClass="control-hyperlink" />
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="lblPageSize" runat="server" meta:resourcekey="PageSize" />
                                        
                                        <asp:DropDownList ID="ddlPageSize" runat="server" 
                                            AutoPostBack="true"
                                            CssClass="control-dropdownlist"  
                                            OnSelectedIndexChanged="OnPageSizeChanged" 
                                            Width="100">
                                            <asp:ListItem Value="5" />
                                            <asp:ListItem Selected="True" Value="10" />
                                            <asp:ListItem Value="20" />
                                            <asp:ListItem Value="50" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                
                    <Columns>                
                        <asp:TemplateField  SortExpression="FirstName" meta:resourcekey="FirstNamefield">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbFirstName" runat="server" Text='<%# Bind("FirstName") %>' />
                            </EditItemTemplate>
                        
                            <ItemTemplate>
                                <asp:Label ID="lblGridFirstName" runat="server" Text='<%# Bind("FirstName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    
                        <asp:TemplateField  SortExpression="MiddleName" meta:resourcekey="MiddleName">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbMiddleName" runat="server" Text='<%# Bind("MiddleName") %>'></asp:TextBox>
                            </EditItemTemplate>
                        
                            <ItemTemplate>
                                <asp:Label ID="lblMiddleName" runat="server" Text='<%# Bind("MiddleName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    
                        <asp:TemplateField meta:resourcekey="LastNamefield" SortExpression="LastName">
                            <EditItemTemplate>
                                <asp:TextBox ID="tbLastName" runat="server" Text='<%# Bind("LastName") %>'></asp:TextBox>
                            </EditItemTemplate>
                        
                            <ItemTemplate>
                                <asp:Label ID="lblGridLastName" runat="server" Text='<%# Bind("LastName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    
                        <asp:CommandField ButtonType="Image" meta:resourcekey="Select" 
                            SelectImageUrl="~/Images/announce/add_blue.gif" 
                            ShowSelectButton="True"
                            >
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                        </asp:CommandField>
                        
                        <asp:CommandField ButtonType="Image"
                            DeleteImageUrl="~/Images/attachments/delete.gif" 
                            ShowDeleteButton="true"                             
                            >
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>   
</div>  

<asp:ObjectDataSource ID="objectDataSourcePersons" runat="server" 
    EnablePaging="True"
    SelectMethod="Select" 
    SelectCountMethod="SelectCount"
    DeleteMethod="DeleteEntity"
    TypeName="ConfirmIt.PortalLib.BusinessObjects.Persons.PersonDataSource" 
    SortParameterName="SortExpression"
    OnObjectCreated="OnObjectDataSourceCreated"
    >    
    <DeleteParameters>
        <asp:Parameter Name="id" Type="Int32" />
    </DeleteParameters>
</asp:ObjectDataSource>

