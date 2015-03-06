<%@ Control Language="C#" AutoEventWireup="true" Inherits="ObjectManagerControl" Codebehind="ObjectManagerControl.ascx.cs" %>

<%@ Register Src="~/Controls/ObjectsManagement/ObjectsOnHandsGrid.ascx" TagName="ObjectsOnHandsGrid" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ObjectsManagement/ObjectHistoryGrid.ascx" TagName="ObjectHistoryGrid" TagPrefix="uc2" %>

<%@ Register Assembly="Controls" Namespace="Controls" TagPrefix="cc2" %>

<div align="left" class="control">
	<asp:UpdatePanel ID="upObjects" runat="server">
		<ContentTemplate>
			<asp:Panel ID="pnlObjectManagment" runat="server" Width="100%">
				<div class="control-header">
					<div class="control-header-title">
						<asp:Localize runat="server" ID="locObjectManagmentTitle" Text="Report" meta:resourcekey="cbToggleObjectManagment" />
					</div>
					<div class="control-header-buttons">
						<asp:ImageButton ID="imgObjectManagmenCollapsedStatus" runat="server" ImageUrl="~/Images/expand.png" AlternateText="Show" />
					</div>
				</div>
			</asp:Panel>
			
			<asp:Panel ID="pnlObjectManagmentContent" runat="server" style="padding: 5px;">
				<cc2:SimpleTabContainer ID="simpleTabContainer" runat="server" AutoPostBack="true">
					<ContentTemplate>
						<div style="float: right; padding-top: 4px; padding-right: 10px;">
							<asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="upObjects" runat="server" DisplayAfter="0">
								<ProgressTemplate>
									<asp:Image ID="Image2" ImageUrl="~/Images/progress.gif" runat="server" Width="16px" Height="16px" />
								</ProgressTemplate>
							</asp:UpdateProgress>
						</div>
							
						<div class="control-line-of-controls">
							<div style="float: left; padding-top: 4px; width: 80px;">
								<asp:Label ID="lblObject" runat="server" CssClass="control-label" meta:resourcekey="lblObject" />
							</div>
							<div style="float: left; width: 200px;">
								<asp:DropDownList ID="ddlObjects" runat="server" 
									AutoPostBack="True" 
									CssClass="control-dropdownlist"
									Width="100%" />
							</div>
						</div>
						
						<div class="control-line-of-controls">
							<div style="float: left; padding-top: 4px; width: 80px;">
								<asp:Label ID="lblOwner" runat="server" CssClass="control-label" meta:resourcekey="lblOwner" />
							</div>
							<div style="float: left; padding-top: 4px;">
								<asp:Label ID="lblOwnerName" runat="server" CssClass="control-label" />
							</div>
						</div>
					   
						<div class="control-line-of-controls">
							<div style="float: left; padding-top: 4px; width: 80px;">
								<asp:Label ID="lblHolder" runat="server" CssClass="control-label" meta:resourcekey="lblHolder" />
							</div>
							<div style="float: left; padding-top: 4px;">
								<asp:Label ID="lblHolderName" runat="server" CssClass="control-label" />
							</div>
						</div>
						
						<ajaxToolkit:TabContainer ID="tcInfo" runat="server" Width="100%" ActiveTabIndex="0">
							<ajaxToolkit:TabPanel ID="tpnObjectHistory" runat="server" meta:resourcekey="ObjectHistory">
								<ContentTemplate>
									<uc2:ObjectHistoryGrid ID="objectHistoryGrid" runat="server" />
								</ContentTemplate>
							</ajaxToolkit:TabPanel>
									
							<ajaxToolkit:TabPanel ID="tpnPersonsInfo" runat="server" meta:resourcekey="PersonsInfo">
								<ContentTemplate>
									<div class="control-line-of-controls" >
										<div style="float: left; padding-top: 4px; width: 130px;">
											<asp:Label ID="lblChooseUser" runat="server" CssClass="control-label" meta:resourcekey="ChooseUser"></asp:Label>
										</div>
										<div style="float: left; width: 200px;">
											<asp:DropDownList ID="ddlUsers" runat="server" 
												AutoPostBack="True" Width="100%"
												CssClass="control-dropdownlist" />
										</div>
									</div>
																				
									<uc1:ObjectsOnHandsGrid ID="objectsOnHandsGrid" runat="server" ControlID="ddlUsers" />
								</ContentTemplate>
							</ajaxToolkit:TabPanel>
						</ajaxToolkit:TabContainer>
						
						<div id="divTake" runat="server" visible="false" >
							<div class="control-line-between"></div>
							<asp:Button ID="btnTake" runat="server" meta:resourcekey="btnTake" CssClass="control-button" />
						</div>
						
						<div id="divGrant" runat="server" visible="false">
							<div class="control-line-between"></div>
							<div class="control-line-of-controls">
								<div style="float: left; padding-top: 4px; width: 80px;">
									<asp:Label ID="lblUser" runat="server" CssClass="control-label" meta:resourcekey="lblUser" />
								</div>
								<div style="float: left; width: 200px;">
									<asp:DropDownList ID="ddlGrantTo" runat="server" CssClass="control-dropdownlist"
											Width="100%" />
								</div>
								<div style="float: left; padding-left: 4px; width: 10px;">
									<asp:RequiredFieldValidator ID="valGrantTo" runat="server" ControlToValidate="ddlGrantTo"
											ErrorMessage="*" Display="Dynamic" ValidationGroup="vgGrantTo" />
								</div>
							</div>
							
							<asp:Button ID="btnGrant" runat="server" CssClass="control-button" meta:resourcekey="btnGrant" ValidationGroup="vgGrantTo" />
						</div>
					</ContentTemplate>
				</cc2:SimpleTabContainer>
			</asp:Panel>
			
			<ajaxToolkit:CollapsiblePanelExtender ID="cpeObjectManagment" runat="server" 
				TargetControlID="pnlObjectManagmentContent"
				ExpandControlID="pnlObjectManagment" 
				CollapseControlID="pnlObjectManagment" 
				Collapsed="true"
				ImageControlID="imgObjectManagmenCollapsedStatus" 
				ExpandedImage="~/Images/collapse.png"
				CollapsedImage="~/Images/expand.png" 
				SuppressPostBack="true" />
		</ContentTemplate>
	</asp:UpdatePanel>
</div>