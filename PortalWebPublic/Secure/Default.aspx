<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Main.master"
    CodeFile="Default.aspx.cs" Inherits="Secure_Default" UICulture="auto" %>

<%@ Register Src="~/Secure/Controls/OfficeUsersList.ascx" TagPrefix="uc1" TagName="OfficeUsersList" %>
<%@ Register TagPrefix="uc1" TagName="Announce" Src="~/Controls/Announce.ascx" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <table width="80%">
        <tr>
            <td width="100%" align="center">
                <uc1:Announce ID="Announce1" runat="server" />
            </td>
        </tr>
        <tr>
            <td width="100%">
                <asp:ScriptManagerProxy ID="scriptMgrProxy" runat="server" />
                <asp:UpdateProgress ID="UpdateProgressPanel" DisplayAfter="0" runat="server">
                    <progresstemplate>
			            <div class="control-errorlabel">
				            <asp:Localize ID="locLoading" runat="server" meta:resourcekey="locLoading"/>
			            </div>
		            </progresstemplate>
                </asp:UpdateProgress>
                
                <asp:Repeater ID="repOffices" runat="server" OnItemDataBound="repOffices_OnItemDataBound">
                    <HeaderTemplate>
                        <table width="100%" border="0px;">
                    </HeaderTemplate>
                    
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <uc1:OfficeUsersList ID="officeUsersList" runat="server" OfficeName='<%# (string)DataBinder.Eval(Container.DataItem, "OfficeName") %>'
                                    ServiceURL='<%# (string)DataBinder.Eval(Container.DataItem, "ServiceURL") %>'
                                    ServiceUserName='<%# (string)DataBinder.Eval(Container.DataItem, "ServiceUserName") %>'
                                    ServicePassword='<%# (string)DataBinder.Eval(Container.DataItem, "ServicePassword") %>'
                                    MeteoInformer='<%# (string)DataBinder.Eval(Container.DataItem, "MeteoInformer") %>' 
                                    ClockInformer='<%# (string)DataBinder.Eval(Container.DataItem, "ClockInformer") %>' 
                                    DigitalClockInformer='<%# (string)DataBinder.Eval(Container.DataItem, "DigitalClockInformer") %>' />                            
                             </td>
                        </tr>
                    </ItemTemplate>
                    
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
    </table>
</asp:Content>
