<%@ Page Language="c#" MasterPageFile="~/MasterPages/Main.master" Inherits="Main" MaintainScrollPositionOnPostback="true" meta:resourcekey="Page" AutoEventWireup="true" Codebehind="Default.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="NewDay" Src="~/Controls/NewDay.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Announce" Src="~/Controls/Announce.ascx" %>
<%@ Register TagPrefix="uc2" TagName="ObjectManager" Src="~/Controls/ObjectsManagement/ObjectManagerControl.ascx" %>

<asp:Content ID="ContentPlh" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<table border="0" width="100%">        
		<tr>
			<td width="10%" align="center" valign="bottom"> 
				<script type="text/jscript">
					document.write('<a href="http://www.informer.ru/cgi-bin/redirect.cgi?id=177_1_1_52_40_1-0&url=http://www.rbc.ru/cash/&src_url=usd/eur_cb_forex_cf320e_88x90.gif" target="_blank"><img src="http://pics.rbc.ru/img/grinf/usd/eur_cb_forex_cf320e_88x90.gif?' + Math.floor(100000 * Math.random()) + '" WIDTH=88 HEIGHT="90" border=0></a>');
				</script>
			</td>
			<td align="center" valign="top" width="55%" >
				<table align="center" border="0" width="95%">
					<tr>
						<td>
							<div style="float: left;">
								<asp:ImageButton ID="btnChangeSkin" runat="server" 
									AlternateText="Change day info present" ToolTip="Change day info present" 
									ImageUrl="~/Images/reload.png" />
							</div>
						</td>
					</tr>
					<tr>
						<td align="center">
							<asp:Localize ID="locNotRegistered" runat="server" Visible="false" meta:resourcekey="NotRegistered" />
							<asp:Localize ID="locNotEmployee" runat="server" Visible="false" meta:resourcekey="NotEmployee" />
							<uc1:NewDay ID="NewDay1" OnWorkFinished="OnWorkFinish" runat="server" />
							
						</td>
					</tr>
					<tr>
						<td>
							<uc2:ObjectManager ID="objectManager" runat="server" />
						</td>
					</tr>
				</table>
			</td>
		   
			<td align="center" valign="top" width="40%" >
				<uc1:Announce ID="Announce1" AllowOfficeNewsEditors="true" runat="server" />
				<object width="100%" height="118"
					type="application/x-silverlight-2" 
					data="data:application/x-silverlight-2,">
					<param name="source" value="ClientBin/Events.xap" />
					<param name="background" value="white" />
					<param name="minRuntimeVersion" value="4.0.0.0" />
					<param name="autoUpgrade" value="true" />
					<param id="slEventsHostParameter" runat="server" name="initParams" 
						value="SLService=SLService/SLService.svc,MenuSource=xml/EventsMenu.xml" />

					Silverlight 4 not installed.
				</object>
			</td>
		</tr>
	</table>
</asp:Content>