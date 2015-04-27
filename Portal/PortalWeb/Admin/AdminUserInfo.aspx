<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeFile="AdminUserInfo.aspx.cs" Inherits="Admin_AdminUserInfo" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/Admin/AdminMenu.ascx" TagPrefix="uc1" TagName="AdminMenu" %>
<%@ Register Src="~/Controls/AjaxControls/DropDownExtender.ascx" TagPrefix="uc2" TagName="DropDownExtender" %>

<asp:Content ID="AdminMenuContext" ContentPlaceHolderID="ContextMenu" runat="server">
	<uc1:AdminMenu ID="adminMenu" runat="server" />
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
   <asp:HyperLink ID="hlMain" runat="server" Visible="false" NavigateUrl="~/Default.aspx" />
   <table width="100%" cellspacing="10">
		<tr>
			<th>
				<asp:Label ID="lblUserName" Font-Bold="true" runat="server"/>
			</th>
		</tr>
		<tr>
			<td align="center">
				<asp:Calendar ID="Calendar" runat="server" Width="90%" OnSelectionChanged="Calendar_SelectionChanged" />
			</td>
		</tr>
		<tr>
			<td>
                <table align="center" width="680">
				    <tr>
				        <td width="5%">
				        </td>
					    <td width="10%">
					        <asp:Label ID="lblBeginTimeHeader" runat="server" meta:resourcekey="hBeginTime" Font-Bold="true" />
                        </td>
					    <td width="10%">
					        <asp:Label ID="lblEndTimeHeader" runat="server" meta:resourcekey="hEndTime" Font-Bold="true" />
                        </td>
					    <td width="10%">
					        <asp:Label ID="lblDurationHeader" runat="server" meta:resourcekey="hPeriod" Font-Bold="true" />
                        </td>
					    <td width="10%">
					        <asp:Label ID="lblEventTypeHeader" runat="server" meta:resourcekey="hEventType" Font-Bold="true" />
                        </td>
				   </tr>
				</table>
			    
			    <asp:UpdatePanel ID="updatePanel" runat="server">
			        <ContentTemplate>
				        <asp:DataGrid Id="dgUserEventsDataGrid" runat="server" 
				            Height="33" Width="680" PageSize="20" 
				            AutoGenerateColumns="False" 
					        HorizontalAlign="Center" 
					        ShowHeader="false"
					        DataKeyField="ID"
					        CssClass="control"
				        >
					        <ItemStyle Width="5px" />
					        <Columns>
					        <asp:TemplateColumn>
					            <ItemTemplate>
					                <asp:Panel CssClass="popup-menu" ID="PopupMenu" runat="server">
                                        <div style="border: 1px outset white; padding: 2px;">
                                            <div>
                                                <asp:LinkButton ID="linkEdit" runat="server" 
                                                    CommandName="Edit" Text="Edit" 
                                                    CssClass="control-hyperlink-big"/>
                                            </div>
                                            <div>
                                                <asp:LinkButton ID="linkDelete" runat="server" 
                                                    CommandName="Delete" Text="Delete"
                                                    CommandArgument="true" 
                                                    CssClass="control-hyperlink-big" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                
                                    <asp:Panel ID="dataPanel" runat="server">
					                    <table width="100%">
					                        <tr>
					                            <td width="5%">
					                                <asp:CheckBox ID="chbDelete" Checked="false" runat="server" />
					                            </td>
					                            <td width="10%">
					                                <asp:Label ID="lblBeginTime2" runat="server" Text='<%# ((DateTime)DataBinder.Eval(Container.DataItem, "BeginTime")).ToShortTimeString()%>' />
					                            </td>
					                            <td width="10%">
    					                            <asp:Label ID="lblEndTime2" runat="server" Text='<%# ((DateTime)DataBinder.Eval(Container.DataItem, "EndTime")).ToShortTimeString() %>' />
	    				                        </td>
					                            <td width="10%">
					                                <asp:Label ID="lblDuration2" runat="server" Text='<%# ConvertTimeSpanToString((TimeSpan)DataBinder.Eval(Container.DataItem, "Duration")) %>'/>
					                            </td>
					                            <td width="10%">
					                                <asp:Label ID="lblEventType2" runat="server" Text='<%# ConvertWorkTypeToString(DataBinder.Eval(Container.DataItem, "EventTypeId")) %>'/>
					                            </td>
					                        </tr>
					                    </table>
					                </asp:Panel>
					            
					                <ajaxToolkit:HoverMenuExtender ID="hme2" runat="Server"
                                        HoverCssClass="popup-hover"
                                        PopupControlID="PopupMenu"
                                        PopupPosition="Left"
                                        TargetControlID="dataPanel"
                                        PopDelay="25" 
                                    />
					            </ItemTemplate>
					        
					            <EditItemTemplate>
					                <ajaxToolkit:HoverMenuExtender ID="hme1" runat="Server"
                                        TargetControlID="dataPanelEdit"
                                        PopupControlID="PopupMenuEdit"
                                        HoverCssClass="popup-hover"
                                        PopupPosition="Right" />
                                   
                                    <asp:Panel ID="PopupMenuEdit" runat="server" 
                                        CssClass="popup-menu" Width="80"
                                    >
                                        <div style="border:1px outset white">
                                            <asp:LinkButton ID="linkUpdate" runat="server"
                                                CausesValidation="True" CommandName="Update" 
                                                Text="Update" 
                                                CssClass="control-hyperlink-big" />
                                            <br />
                                            <asp:LinkButton ID="linkCancel" runat="server"
                                                CausesValidation="False" CommandName="Cancel" 
                                                Text="Cancel" 
                                                CssClass="control-hyperlink-big" />
                                        </div>
                                    </asp:Panel>
                                    
                                    <asp:Panel ID="dataPanelEdit" runat="server">
					                    <table width="100%">
					                        <tr>
					                            <td width="5%">
				                                </td>
					                            <td width="10%">
					                                <asp:TextBox ID="tbBeginTime" runat="server" Text='<%# ((DateTime)DataBinder.Eval(Container.DataItem, "BeginTime")).ToShortTimeString()  %>' />
					                            </td>
					                            <td width="10%">
					                                <asp:TextBox ID="tbEndTime" runat="server" Text='<%# ((DateTime)DataBinder.Eval(Container.DataItem, "EndTime")).ToShortTimeString() %>' />
	    				                        </td>
					                            <td width="10%">
					                                <asp:Label ID="tbDuration" runat="server" Text='<%# ConvertTimeSpanToString((TimeSpan)DataBinder.Eval(Container.DataItem, "Duration")) %>'/>
					                            </td>
					                            <td width="10%">
					                                <asp:DropDownList id="EventTypeList" runat="server" Visible="false"
					                                    DataSource="<%# GetEventTypeList() %>" DataValueField="ID" DataTextField="Name" Width="190px" />
					                                    
					                                <uc2:DropDownExtender ID="dropDownExtender" runat="server"
					                                    DataSource="<%# GetEventTypeList() %>"
					                                    DataTextField="Name"
					                                    DataValueField="ID"
					                                />					                                    
					                            </td>
					                        </tr>
					                    </table>
					                </asp:Panel>
					            </EditItemTemplate>
					        </asp:TemplateColumn>
					        </Columns>
				        </asp:DataGrid>
				        <center>
				            <asp:Label runat="server" ID="lblException" Visible="false" CssClass="control-errorlabel" />
				        </center>
				    </ContentTemplate>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr align="center">
			<td>
				<asp:Button ID="bt_NewUptimeEvent" runat="server" Width="150" CssClass="control-button" TabIndex="11" Text="New " OnClick="bt_NewUptimeEvent_Click" meta:resourcekey="btnNew"/>
				<asp:Button ID="bt_DeleteUptimeEvent" runat="server" Width="150" CssClass="control-button" TabIndex="13" Text="Delete " OnClick="bt_DeleteUptimeEvent_Click" meta:resourcekey="btnDelete"/>
			</td>
		</tr>
   </table>
   
</asp:Content>