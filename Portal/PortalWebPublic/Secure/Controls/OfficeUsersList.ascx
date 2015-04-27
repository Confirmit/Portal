<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OfficeUsersList.ascx.cs"
	Inherits="Secure_Controls_OfficeUsersList" %>
	
<asp:PlaceHolder ID="plh" runat="server" Visible="true" >
<asp:ScriptManagerProxy ID="scriptMgrProxy" runat="server" />

<table width="100%" border="0px;">
	<tr>
		<td align="left" Width="100px;">
			<asp:Label ID="lblOfficeName" runat="server" Width="100%" />
		</td>
	</tr>
	<tr>
		<td align="left">
			<asp:Button ID="btnUpdate" runat="server" 
			    meta:resourcekey="btnUpdate" 
			    CssClass="control-button"
			    OnClick="OnUpdate" />
		</td>
		<td align="left" Width="100px;">
			<%# GetClockInformer() %>
		</td>
		<td align="center">
			<%# GetDigitalClockInformer() %>
		</td>
		<td align="right">
			<%# GetMeteoInformer() %>
		</td>
	</tr>
	<tr>
		<td colspan="4" align="center">
			<asp:UpdatePanel ID="upOfficeUsersList" runat="server">
				<ContentTemplate>
					<asp:DataGrid ID="grdUsersList" runat="server" 
					    DataKeyField="UserID" 
					    AutoGenerateColumns="false"
						OnItemDataBound="OnUserInfoBound"
					>
						<SelectedItemStyle 
						    Height="50px" 
						    HorizontalAlign="Right" 
						    Font-Bold="False" 
						    Font-Italic="False"
							Font-Overline="False" 
							Font-Strikeout="False" 
							Font-Underline="False" />
						<ItemStyle Width="5px" />
						<Columns>
							<asp:TemplateColumn HeaderText="USL name" ItemStyle-HorizontalAlign="Center" meta:resourcekey="hUSLname">
								<ItemTemplate>
									<asp:HyperLink ID="hlUSLName" runat="server" Text='<%# (string)DataBinder.Eval(Container.DataItem, "USLName") %>' />
								</ItemTemplate>
								<HeaderStyle Width="30" HorizontalAlign="Center" Font-Bold="true" />
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="User" ItemStyle-HorizontalAlign="Left" meta:resourcekey="hUser">
								<ItemTemplate>
									<asp:HyperLink ID="hlUserName" runat="server" Text='<%# (string)DataBinder.Eval(Container.DataItem, "UserName") %>' />
								</ItemTemplate>
								<HeaderStyle Width="40%" HorizontalAlign="Center" Font-Bold="true" />
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="Begin" ItemStyle-HorizontalAlign="Center" meta:resourcekey="hBeginTime">
								<ItemTemplate>
									<asp:Label ID="lBeginTime" runat="server" Text='<%# GetTimePresentation((DateTime)DataBinder.Eval(Container, "DataItem.BeginWork")) %>' />
								</ItemTemplate>
								<HeaderStyle Width="70" HorizontalAlign="Center" Font-Bold="true" />
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="Status" ItemStyle-HorizontalAlign="Center" meta:resourcekey="hStatus">
								<ItemTemplate>
									<asp:Label ID="lUserStatus" runat="server" Text='<%# (string)DataBinder.Eval(Container.DataItem, "Status") %>' />
								</ItemTemplate>
								<HeaderStyle Width="30%" HorizontalAlign="Center" Font-Bold="true" />
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="End" ItemStyle-HorizontalAlign="Center" meta:resourcekey="hEndTime">
								<ItemTemplate>
									<asp:Label ID="lEndTime" runat="server" Text='<%# GetEndTime(Container.DataItem) %>' />
								</ItemTemplate>
								<HeaderStyle Width="70" HorizontalAlign="Center" Font-Bold="true" />
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="Duration" ItemStyle-HorizontalAlign="Center" meta:resourcekey="hDuration">
							    <ItemTemplate>
							        <asp:Label ID="lblDuration" runat="server" Text='<%# GetEventDuration(Container.DataItem) %>' />
							    </ItemTemplate>
							    <HeaderStyle Width="90" HorizontalAlign="Center" Font-Bold="true" />
							</asp:TemplateColumn>
						</Columns>
					</asp:DataGrid>
					
				</ContentTemplate>
				
				<Triggers>
					<asp:AsyncPostBackTrigger 
					    ControlID="btnUpdate" 
					    EventName="Click" 
					/>
				</Triggers>
				
			</asp:UpdatePanel>
		</td>
	</tr>
</table>
</asp:PlaceHolder>	
