<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RequestObjectEditControl.ascx.cs" Inherits="RequestObjectEditControl" %>

<%@ Register Assembly="Controls" Namespace="Controls" TagPrefix="cc1" %>

<%@ Register Src="~/Controls/ObjectsManagement/RequestObjects/Book/BookFilter.ascx" TagName="BookFilter" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ObjectsManagement/RequestObjects/Book/BookGrid.ascx" TagName="BookGrid" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ObjectsManagement/RequestObjects/Book/BookInfo.ascx" TagName="BookInfo" TagPrefix="uc1" %>

<%@ Register Src="~/Controls/ObjectsManagement/RequestObjects/Card/CardFilter.ascx" TagName="CardFilter" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/ObjectsManagement/RequestObjects/Card/CardGrid.ascx" TagName="CardGrid" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/ObjectsManagement/RequestObjects/Card/CardInfo.ascx" TagName="CardInfo" TagPrefix="uc2" %>

<%@ Register Src="~/Controls/ObjectsManagement/RequestObjects/Disk/DiskFilter.ascx" TagName="DiskFilter" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/ObjectsManagement/RequestObjects/Disk/DiskGrid.ascx" TagName="DiskGrid" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/ObjectsManagement/RequestObjects/Disk/DiskInfo.ascx" TagName="DiskInfo" TagPrefix="uc3" %>


<div>
    <cc1:SimpleTabContainer ID="simpleTabContainer" runat="server" AutoPostBack="true">
        <ContentTemplate>
            <asp:UpdatePanel ID="upObjects" runat="server">
                <ContentTemplate>
                    <div class="control-line-of-controls">
                        <div style="float: right; padding-top: 4px; padding-right: 10px;">
                            <asp:UpdateProgress ID="upObjectsProgress" AssociatedUpdatePanelID="upObjects" runat="server" DisplayAfter="0">
                                <ProgressTemplate>
                                    <asp:Image ID="Image2" ImageUrl="~/Images/progress.gif" runat="server" Width="16px" Height="16px" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                    </div>
                    
                    <uc1:BookFilter ID="bookFilter" runat="server" ViewName="book_filter_ctl" />
                    <uc2:CardFilter ID="cardFilter" runat="server" ViewName="card_filter_ctl" />
                    <uc3:DiskFilter ID="diskFilter" runat="server" ViewName="disk_filter_ctl" />
                    
                    <div class="control-line-between"></div>
                    <uc1:BookGrid ID="bookGrid" runat="server" />
                    <uc2:CardGrid ID="cardGrid" runat="server" />
                    <uc3:DiskGrid ID="diskGrid" runat="server" />
                    
                    <div class="control-line-between"></div>
                    <uc1:BookInfo ID="bookInfo" runat="server" />
                    <uc2:CardInfo ID="cardInfo" runat="server" />
                    <uc3:DiskInfo ID="diskInfo" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </cc1:SimpleTabContainer>
</div>