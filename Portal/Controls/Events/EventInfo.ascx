<%@ Control Language="C#" AutoEventWireup="true" Inherits="EventInfo" Codebehind="EventInfo.ascx.cs" %>

<%@ Register TagPrefix="uc1" TagName="Calendar" Src="~/Controls/AjaxControls/Calendar/Calendar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SubscribeEvents" Src="~/Controls/Events/SubscribeEvents.ascx" %>

<%@ Register Src="~/controls/ActionsMenu/ActionsMenuCtl.ascx" TagName="ActionsMenu" TagPrefix="uc2" %>

<div class="control" id=<%=ClientID%>>
	<uc2:ActionsMenu ID="menuActions" runat="server" 
		ViewName="user_events_action_menu" 
		MenuForNotSelectedObjectActionsCriteria="./controls/ActionsMenu/s_s_and_new.xml" />

	<div class="control-body" style="padding-top: 3px;">
		<div align="left" style="width: 100%" class="control-line-of-controls">
			<div style="float: left; padding-top: 4px; width: 110px;">
				<asp:Label ID="lblEventDate" runat="server" CssClass="control-label" meta:resourcekey="lblEventDate" />
			</div>
			<div style="float: left; padding-left: 5px;">
				<asp:TextBox ID="tbEventDate" runat="server" CssClass="control-textbox-required" EnableTheming="false" />
			</div>
			<div style="float: left; padding-left: 4px; padding-top: 3px;">
				<uc1:Calendar ID="calendar" runat="server" TargetControlID="tbEventDate" />
			</div>
		</div>
	
		<asp:Label Width="100%" ID="reqDate" runat="server" CssClass="control-errorlabel" 
			Text="Missing Information - Date." Visible="false" />    
		
		<div align="left" style="padding-top: 8px;" class="control-line-of-controls">
			<div style="float: left; padding-top: 4px; width: 110px;">
				<asp:Label ID="lblEventName" runat="server" CssClass="control-label" meta:resourcekey="lblEventName" />
			</div>
			<div style="float: left; padding-left: 5px;">
				<asp:TextBox ID="tbEventName" Width="300" runat="server" CssClass="control-textbox-required" EnableTheming="false" />
			</div>
		</div>
		
		<asp:Label Width="100%" ID="reqEventName" runat="server" CssClass="control-errorlabel" 
			Text="Missing Information - Event Name." Visible="false" />
		
		<div align="left" style="height: 90px; padding-top: 8px;">
			<div style="float: left; padding-top: 4px; width: 110px;">
				<asp:Label ID="lblDescription" runat="server" CssClass="control-label" meta:resourcekey="lblDescription" />
			</div>
			<div style="float: left; padding-left: 5px;">
				<textarea id="tbDescription" runat="server" style="width: 305px; height: 80px;" />
			</div>
		</div>
		
		<div align="left" class="control-line-of-controls" style="height: 25px;">
			<div style="float: left; padding-top: 4px; width: 110px;">
				<asp:Label ID="lblFormatDate" runat="server" CssClass="control-label" meta:resourcekey="lblFormatDate" />
			</div>
			<div style="float: left; padding-left: 5px; width: 200px">
				<asp:DropDownList ID="dropDownFormat" runat="server" Width="100%" CssClass="control-dropdownlist" EnableTheming="false" />
			</div>
		</div>
		
		<div align="left" class="control-line-of-controls">
			<div style="float: left; padding-top: 4px; width: 110px;">
				<asp:Label ID="lblAuthorTitle" runat="server" CssClass="control-label" meta:resourcekey="lblAuthor" />
			</div>
			<div style="float: left; padding-left: 5px; padding-top: 4px; width: 200px">
				<asp:Label ID="lblAuthorName" runat="server" CssClass="control-label" Width="100%" />
			</div>
		</div>
		
		<div class="control-line-between"></div>
		<div align="left" class="control-line-of-controls">
			<asp:CheckBox ID="checkBoxPublicEvent" runat="server" CssClass="control-label" meta:resourcekey="lblPublicEvent"/>
		</div>

		<div align="left" class="control-line-of-controls">
			<asp:CheckBox ID="cbPersonalEvent" runat="server" 
				CssClass="control-label" Checked="true" meta:resourcekey="cbPersonal" />
		</div>

		<div align="left" class="control-line-of-controls">
			<asp:CheckBox ID="cbGroupEvent" runat="server" 
				CssClass="control-label" Checked="true" meta:resourcekey="cbGroup" />
		</div>
		
		<div class="control-line-between"></div>
		<div align="left">
			<ajaxToolkit:TabContainer ID="TabContainer1" runat="server">
				<ajaxToolkit:TabPanel ID="personalTab" runat="server" meta:resourcekey="tabPersonal">
					<ContentTemplate>
						<div class="control-border" style="overflow: scroll; height: 200px;">
							<asp:CheckBoxList ID="cblUsers" runat="server" CssClass="control-label" />
						</div>
					</ContentTemplate>
				</ajaxToolkit:TabPanel>
			
				<ajaxToolkit:TabPanel ID="groupTab" runat="server" meta:resourcekey="tabGroup" >
					<ContentTemplate>                
						<div class="control-border" align="left" >
							<asp:CheckBoxList ID="checkBoxListGroup" runat="server" CssClass="control-label" />
						</div>
					</ContentTemplate>
				</ajaxToolkit:TabPanel>
				
				<ajaxToolkit:TabPanel ID="tabSubscribe" runat="server" meta:resourcekey="tabSubscribe">
					<ContentTemplate>
						<div class="control-border" style="overflow: scroll; height: 200px; width: 100%">
							<uc1:SubscribeEvents ID="subscribeEventsGrid" runat="server" />
						</div>
					</ContentTemplate>
				</ajaxToolkit:TabPanel>
			</ajaxToolkit:TabContainer>
		</div>
	</div>
</div>