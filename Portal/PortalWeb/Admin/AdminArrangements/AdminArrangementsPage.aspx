<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    CodeFile="AdminArrangementsPage.aspx.cs" Inherits="Admin_AdminArrangementsPage"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/Admin/AdminMenu.ascx" TagPrefix="uc1" TagName="AdminMenu" %>
<%@ Register Src="~/Admin/AdminArrangements/Controls/ArrangementsList.ascx" TagPrefix="uc2"
    TagName="ArrangementsList" %>
<asp:Content ID="AdminMenuContext" ContentPlaceHolderID="ContextMenu" runat="server">
    <uc1:AdminMenu ID="adminMenu" runat="server" />
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:HyperLink ID="hlMain" runat="server" Visible="false" NavigateUrl="~/Default.aspx" />
    <asp:ScriptManagerProxy ID="scriptMgrProxy" runat="server" />
    <asp:Label ID="lTitle" CssClass="control-label" runat="server" Font-Bold="true" Font-Size="Large"
        Font-Underline="true" Height="40px" meta:resourcekey="lTitle" />
    <asp:Calendar ID="Calendar" runat="server" DayNameFormat="Short" Font-Name="Verdana;Arial"
        Font-Size="14px" Height="160px" NextPrevFormat="ShortMonth" OtherMonthDayStyle-ForeColor="gray"
        SelectMonthText="month" SelectWeekText="week" TodayDayStyle-Font-Bold="True"
        Width="90%" OnSelectionChanged="Calendar_SelectionChanged">
        <SelectedDayStyle BackColor="#FFCC66" Font-Bold="True" />
        <TodayDayStyle Font-Bold="True" />
        <SelectorStyle BackColor="#99CCFF" Font-Size="9px" ForeColor="Navy" />
        <OtherMonthDayStyle ForeColor="Gray" />
        <NextPrevStyle Font-Size="10px" ForeColor="White" />
        <DayHeaderStyle Font-Bold="True" />
    </asp:Calendar>
    <asp:UpdateProgress ID="UpdateProgressPanel" DisplayAfter="0" runat="server">
        <ProgressTemplate>
            <div class="control-errorlabel">
                <asp:Localize ID="locLoading" runat="server" meta:resourcekey="locLoading" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:Repeater ID="repOffices" runat="server" OnItemDataBound="repOffices_OnItemDataBound">
        <HeaderTemplate>
            <table width="100%" border="0">
        </HeaderTemplate>
        <ItemTemplate>
            <div style="width: 90%;">
                <uc2:ArrangementsList ID="arrangementsList" runat="server" OfficeName='<%# (string)DataBinder.Eval(Container.DataItem, "OfficeName") %>'
                    OfficeID='<%# (int)DataBinder.Eval(Container.DataItem, "OfficeID") %>' ServiceURL='<%# (string)DataBinder.Eval(Container.DataItem, "ServiceURL") %>'
                    ServiceUserName='<%# (string)DataBinder.Eval(Container.DataItem, "ServiceUserName") %>'
                    ServicePassword='<%# (string)DataBinder.Eval(Container.DataItem, "ServicePassword") %>'
                    SelectedDate='<%# (DateTime)DataBinder.Eval(Container.DataItem, "SelectedDate") %>' />
            </div>
        </ItemTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
