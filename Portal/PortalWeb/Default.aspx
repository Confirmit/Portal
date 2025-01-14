<%@ Page Language="c#" MasterPageFile="~/MasterPages/Main.master" Inherits="Main" 
    CodeFile="Default.aspx.cs" MaintainScrollPositionOnPostback="true" meta:resourcekey="Page" AutoEventWireup="true" %>

<%@ Register TagPrefix="uc1" TagName="NewDay" Src="~/Controls/NewDay.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Announce" Src="~/Controls/Announce.ascx" %>
<%@ Register TagPrefix="uc2" TagName="ObjectManager" Src="~/Controls/ObjectsManagement/ObjectManagerControl.ascx" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls" TagPrefix="asp" %>

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
                                                        
                            <% if (IsUsingSilverlightControl())
                               { %>
                                    <asp:Silverlight ID="slDayInfo" runat="server" 
                                        Source="~/ClientBin/DayInfoPresenter.xap"
                                        MinimumVersion="3.0.40624.0" 
                                        InitParameters="SLService=SLService/SLService.svc"
                                        Width="100%" Height="250"
                                    >
                                        <PluginNotInstalledTemplate>
                                            <uc1:NewDay ID="ucNewDayPluginNI" OnWorkFinished="OnWorkFinish" runat="server" />                                    
                                        </PluginNotInstalledTemplate>
                                    </asp:Silverlight>
                            <% } else { %>
                                <uc1:NewDay ID="NewDay1" OnWorkFinished="OnWorkFinish" runat="server" />
                            <% } %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                                <uc2:ObjectManager ID="objectManager" runat="server" />
                          
                                <div class="control-line-between"></div>
                                <asp:Panel ID="pnlReportHeader" runat="server" Width="100%"> 
                                    <div class="control-header">
                                        <div class="control-header-title">
                                            <asp:Localize runat="server" ID="locReportTitle" Text="Report" meta:resourcekey="cbToggleReport" />      
                                        </div>
                                        <div class="control-header-buttons">
                                            <asp:ImageButton ID="imgCollapsedStatus" runat="server" ImageUrl="~/Images/expand.jpg" AlternateText="Show"/>
                                        </div>            
                                    </div>
                                </asp:Panel>
                                                                
                                <asp:Panel ID="pnlReportContent" Width="100%" runat="server" style="overflow: hidden" Height="0">
                                 <div class="control">
                                    <div class="control-body">
                                        <div style="float: right;">
                                            <asp:Button ID="btnSend" CssClass="control-button" runat="server" ValidationGroup="Report" CausesValidation="true"
                                                OnClick="OnSendReport" meta:resourcekey="btnSend" />
                                        </div>
                                    </div>
                                    
                                    <table width="100%" border="0px">
                                        <tr>
                                            <td align="center">
                                                <div id="divPMReport">
                                                    <asp:PlaceHolder ID="phReportForm" runat="server" Visible="true">
                                                        <table width="100%" border="0px">
                                                            <tr>
                                                                <th class="control-label" style="width: 20%">
                                                                    <asp:Localize ID="locTo" runat="server" meta:resourcekey="locTo" />
                                                                </th>
                                                                <td>
                                                                    <asp:TextBox ID="tbTo" runat="server" Width="97%" />
                                                                    <asp:RequiredFieldValidator CssClass="control-errorlabel" ValidationGroup="Report" ID="valToReq" ControlToValidate="tbTo"
                                                                        SetFocusOnError="true" Display="Dynamic" runat="server" meta:resourcekey="valToReq" />
                                                                    <asp:RegularExpressionValidator CssClass="control-errorlabel" ValidationGroup="Report" ID="valToRegex" ControlToValidate="tbTo"
                                                                        SetFocusOnError="true" ValidationExpression="^(\w+\.)*\w+@(\w+\.)*[A-Za-z]+$"
                                                                        Display="Dynamic" runat="server" meta:resourcekey="valToRegex" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th class="control-label" style="width: 20%">
                                                                    <asp:Localize ID="locSubject" runat="server" meta:resourcekey="locSubject" />
                                                                </th>
                                                                <td>
                                                                    <asp:TextBox ID="tbSubject" runat="server" Width="97%" />
                                                                    <asp:RequiredFieldValidator CssClass="control-errorlabel" ValidationGroup="Report" ID="valSubjectReq" ControlToValidate="tbSubject"
                                                                        ErrorMessage="Subject can't be empty" Display="Dynamic" runat="server" meta:resourcekey="valSubjectReq" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th class="control-label" colspan="2" align="center">
                                                                    <asp:Localize ID="locMessage" runat="server" meta:resourcekey="locMessage" />
                                                                </th>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="tbMessage" runat="server" TextMode="MultiLine" Rows="10" Width="99%" />
                                                                    <asp:RequiredFieldValidator CssClass="control-errorlabel" ValidationGroup="Report" ID="valMessageReq" ControlToValidate="tbMessage"
                                                                        ErrorMessage="Message can't be empty" Display="Dynamic" runat="server" meta:resourcekey="valMessageReq" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:PlaceHolder>
                                                    
                                                    <asp:PlaceHolder ID="phReportUnavaliable" runat="server" Visible="false">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label ID="locReportUnavaliable" runat="server" meta:resourcekey="locReportUnavaliable" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:PlaceHolder>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
                                </asp:Panel>
                                
                                <ajaxToolkit:CollapsiblePanelExtender ID="cpeReport" runat="server" TargetControlID="pnlReportContent"
                                    ExpandControlID="pnlReportHeader" CollapseControlID="pnlReportHeader" Collapsed="true"
                                    ImageControlID="imgCollapsedStatus" ExpandedImage="Images/collapse.jpg" CollapsedImage="Images/expand.jpg"
                                    SuppressPostBack="true" 
                                />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
           
            <td align="center" valign="top" width="40%" >
                <uc1:Announce ID="Announce1" AllowOfficeNewsEditors="true" runat="server" />
                <asp:Silverlight ID="slEventsControl" runat="server" 
                    Source="~/ClientBin/Events.xap" 
                    MinimumVersion="2.0.31005.0" 
                    InitParameters="MenuSource=xml/EventsMenu.xml,SLService=SLService/SLService.svc"
                    Width="100%" Height="118"
                />
            </td>
        </tr>
    </table>
</asp:Content>