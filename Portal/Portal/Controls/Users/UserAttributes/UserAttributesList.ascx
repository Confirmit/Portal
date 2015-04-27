<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_AdminPersonAttributesView" Codebehind="UserAttributesList.ascx.cs" %>

<%@ Register Src="~/Controls/Users/UserAttributes/UserAttributeInfo.ascx" TagPrefix="uc1" TagName="UserAttributeInfo" %>

<div class="control">
    <div class="control-body" style="padding:0;">
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate> 
                <div style="height: 200px;">
                    <div style="float: left; width: 30%">
                        <uc1:UserAttributeInfo runat="server" ID="userAttributeInfo" />
                    </div>
                    <div style="width: 68%; float: left; padding-top: 5px;">
                        <asp:GridView 
                            ID="gridViewAttributes" runat="server" Width="100%" 
                            AutoGenerateColumns="False" 
                            DataKeyNames="ID" 
                            EnableTheming="False"
                            CssClass="gridview" 
                            GridLines="None" AllowPaging="True" PageSize="5" 
                            >
                            <RowStyle CssClass="gridview-row" HorizontalAlign="Center" />
                            <EditRowStyle BackColor="#EEEEEE" Height="20"/>
                            <SelectedRowStyle CssClass="gridview-selectedrow"/>
                            <PagerStyle CssClass="gridview-pagerrow"/>
                            <AlternatingRowStyle CssClass="gridview-alternatingrow"/>
                            <FooterStyle CssClass="gridview-footer"  />
                            <HeaderStyle CssClass="gridview-headerrow" />                            
                            
                            <EmptyDataTemplate>
                                <center>
                                    <%# (string)GetLocalResourceObject("NoAttributes")%>
                                </center>
                            </EmptyDataTemplate>
                            
                            <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                            <PagerTemplate>
                                <div style="width: 100%">
                                    <table width="100%">
                                        <tr>
                                            <td align="center" style="width: 70%">
                                                <asp:LinkButton ID="lbntFirst" runat="server" Text="<<" CommandName="Page" CommandArgument="First" />
                                                <asp:LinkButton ID="lbtnPrev" runat="server" Text="<" CommandName="Page" CommandArgument="Prev" />
                                                <asp:Literal ID="lblPage" runat="server" meta:resourcekey="PageIndex" />
                                                <asp:DropDownList CssClass="control-dropdownlist" ID="ddlPage" runat="server" Width="100" AutoPostBack="true" OnSelectedIndexChanged="OnPageIndexChanged"/>
                                                <asp:Literal ID="lblFrom" runat="server" meta:resourcekey="PageFrom" />
                                                <asp:Literal ID="lblPageCount" runat="server" />
                                                <asp:LinkButton ID="lbtnNext" runat="server" Text=">" CommandName="Page" CommandArgument="Next" />
                                                <asp:LinkButton ID="lbtnLast" runat="server" Text=">>" CommandName="Page" CommandArgument="Last" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                            
                            <Columns>
                                <asp:BoundField meta:resourcekey="HeaderNameAttr" 
                                    DataField="AttributeName" />
                                    
                                <asp:CheckBoxField meta:resourcekey="HeaderIsShowToUsers" 
                                    DataField="ShowToUsers" />
                                                                
                                <asp:CommandField ButtonType="Image" 
                                    EditImageUrl="~/Images/Edit.gif"
                                    ShowEditButton="True" meta:resourcekey="Edit">
                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                </asp:CommandField>
                                
                                <asp:CommandField ButtonType="Image" 
                                    DeleteImageUrl="~/Images/Delete.gif"
                                    ShowDeleteButton="True" meta:resourcekey="Delete"
                                    >
                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                </asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate> 
        </asp:UpdatePanel>
    </div>
</div>    