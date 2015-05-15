<%@ Import namespace="System.ComponentModel"%>

<%@ Control Language="C#" AutoEventWireup="true" Inherits="NewsTape_Announce" Codebehind="Announce.ascx.cs" %>

<%@ Register TagName="NewsTopMenu" TagPrefix="uc1" Src="~/Controls/News/NewsTopMenu.ascx" %>
<%@ Register TagName="ImageLinkButton" TagPrefix="uc2" Src="~/Controls/ImageLinkButton.ascx" %>

<table border="0px" width="100%">
	<tr >
		<td align="left">
			<uc2:ImageLinkButton ID="list" runat="server"
				ImageUrl="~/Images/news.png"
				Href="~/NewsTape/FullNewsTape.aspx" 
				ImageWidth="16"/>
		</td>
		<td align="right">
			<uc2:ImageLinkButton ID="search" runat="server"
				ImageUrl="~/Images/announce/search.gif"
				ImageWidth="16"
				Href="~/NewsTape/SearchNews.aspx" />
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<table width="100%">
				<tr>
					<td>
						<asp:ObjectDataSource ID="ds" runat="server" SelectMethod="GetActualNews" TypeName="UlterSystems.PortalLib.NewsManager.NewsManager">
							<SelectParameters>
								<asp:Parameter Name="personID" />
							</SelectParameters>
						</asp:ObjectDataSource>
						
						<asp:UpdatePanel ID="announcePanel" runat="server">
							<ContentTemplate>
								<div class="control">
									<asp:GridView 
										ID="gvNews" runat="server" 
										EnableTheming="false" DataSourceID="ds" 
										Width="100%" AutoGenerateColumns="False"
										OnRowDataBound="gvNews_RowDataBound" 
										CellPadding="4" PageSize="4" 
										ShowHeader="False" AllowPaging="true" 
										GridLines="None" CssClass="gridview">
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
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
					</td>
				</tr>
				<tr>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td align="left">
			<uc2:ImageLinkButton ID="archive" runat="server"
				ImageUrl="~/Images/announce/archive.gif"
				ImageWidth="16"
				Href="~/NewsTape/Archive.aspx" />
		</td>
		<td align="right">
			<uc2:ImageLinkButton ID="hlAddNews" runat="server"
				ImageUrl="~/Images/announce/add_blue.gif"
				ImageWidth="16"
				Href="~/NewsTape/AddNews.aspx" />
		</td>
	</tr>
</table>
