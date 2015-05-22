<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopMenu.ascx.cs" Inherits="Controls_TopMenu" %>
<%@ Register Src="~/Controls/TopMenuItem.ascx" TagPrefix="uc1" TagName="TopMenuItem" %>

<asp:Repeater ID="repSiteMap" DataSourceID="SiteMapDS" runat="server">
	<HeaderTemplate>
		<table cellspacing="0" cellpadding="0">
		<tr>
	</HeaderTemplate>
	
	<ItemTemplate>
		<td style="padding-left:15px;">
			<uc1:TopMenuItem ID="miLink" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"url") %>'
				Tooltip='<%# DataBinder.Eval(Container.DataItem,"description") %>'
				Text='<%# DataBinder.Eval(Container.DataItem,"title") %>'
				AllowedRoles='<%# DataBinder.Eval(Container.DataItem,"roles") %>' runat="server"/>
		</td>
	</ItemTemplate>
	
	<FooterTemplate>
		</tr>
		</table>
	</FooterTemplate>
</asp:Repeater>

<asp:SiteMapDataSource ID="SiteMapDS" runat="server" ShowStartingNode="false" />