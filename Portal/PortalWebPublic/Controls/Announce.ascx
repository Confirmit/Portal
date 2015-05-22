<%@ Import namespace="System.ComponentModel"%>

<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Announce.ascx.cs" Inherits="NewsTape_Announce" %>

<%@ Register TagName="NewsTopMenu" TagPrefix="uc1" Src="~/Controls/NewsTopMenu.ascx" %>
<%@ Register TagName="ImageLinkButton" TagPrefix="uc2" Src="~/Controls/ImageLinkButton.ascx" %>

<table border="0px">
    <tr >
        <td align="left">
            <uc2:ImageLinkButton ID="list" runat="server"
                ImageUrl="~/Images/announce/list.gif"
                Href="~/NewsTape/FullNewsTape.aspx" />
        </td>
        <td align="right">
            <uc1:NewsTopMenu ID="newsTopMenu" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%">
                <tr>
                    <td>
                        <asp:ObjectDataSource ID="ds" runat="server" 
                            SelectMethod="GetActualNews" 
                            TypeName="UlterSystems.PortalLib.NewsManager.NewsManager">
                            <SelectParameters>
                                <asp:Parameter Name="personID" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        
                        <div class="control">
                            <asp:GridView 
                                ID="gvNews" runat="server" 
                                EnableTheming="false"
                                DataSourceID="ds" 
                                Width="100%" 
                                AutoGenerateColumns="False"
                                OnRowDataBound="gvNews_RowDataBound" 
                                CellPadding="4" 
                                ShowHeader="False"
                                AllowPaging="true" 
                                PageSize="4" 
                                GridLines="None" 
                                CssClass="gridview"
                            >
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Image ID="imgNewsType" runat="Server" />
                                        </ItemTemplate>
                                 </asp:TemplateField>
                                
                                   <asp:TemplateField >
					    	            <ItemTemplate>
						    	            <asp:HyperLink ID="hlCaption" runat="server"
							                    CssClass="control-hyperlink"
							                    NavigateUrl='<%# "~//NewsTape//ViewNews.aspx?id=" + (DataBinder.Eval(Container, "DataItem.ID")) %>' 
							                    Text='<%# DataBinder.Eval(Container, "DataItem.Caption") %>'
	    						                />
    							                <br />
							                    <asp:HyperLink ID="hlAuthor" runat="server"
							                        CssClass="control-hyperlink"
							                        NavigateUrl='<%# GetNewsAuthorMail(Container) %>' 
							                        Text='<%# "(" + DataBinder.Eval(Container, "DataItem.AuthorName") + ")" %>'
							                    />
						                </ItemTemplate>
					                </asp:TemplateField>
					            
					                <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Image Width="14" ID="imgHasAttach" runat="Server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                
                                    <asp:BoundField DataField="CreateTime">
                                        <ItemStyle CssClass="control-label" 
                                            HorizontalAlign="Center" 
                                        />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle Height="0px" Wrap="False" BorderStyle="None" Width="0px" />
                                <PagerStyle CssClass="gridview-pagerrow" />
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                </tr>
            </table>
        </td>
    </tr>
</table>
