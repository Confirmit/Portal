<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Main.master" Inherits="Arrangements_AddEditArrangement"
    UICulture="auto" Codebehind="AddEditArrangement.aspx.cs" %>

<%@ Register Src="~/controls/ActionsMenu/ActionsMenuCtl.ascx" TagName="ActionsMenu"
    TagPrefix="uc1" %>

<%@ Register Src="~/Controls/AjaxControls/Calendar/Calendar.ascx" TagName="Calendar"
    TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <div class="control" id="<%=ClientID%>" style="width: 510px;">
        <div align="center" style="width: 100%">
            <asp:Label ID="lTitle" CssClass="control-label" runat="server" Font-Bold="true" Font-Size="Large"
                Font-Underline="true" Height="40px" />
        </div>
        <div class="control-body">
            <div align="left" style="width: 100%" class="control-line-of-controls">
                <div style="float: left; width: 110px;">
                    <asp:Label ID="lOfficeName" CssClass="control-label" runat="server" meta:resourcekey="lOfficeName" />
                </div>
                <div style="float: left;">
                    <asp:TextBox ID="tbOffice" CssClass="control-textbox" runat="server" Width="370"></asp:TextBox>
                </div>
            </div>
            <div class="control-line-between">
            </div>
            <div align="left" class="control-line-of-controls">
                <div style="float: left; width: 110px;">
                    <asp:Label ID="lConferenceHallName" runat="server" CssClass="control-label" meta:resourcekey="lConferenceHallName" />
                </div>
                <div style="float: left; width: 377px;">
                    <asp:DropDownList ID="ddlConferenceHalls" CssClass="control-dropdownlist" DataTextField="Name"
                        DataValueField="ConferenceHallID" runat="server" Width="100%" AutoPostBack="True"
                        Font-Size="Small">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="control-errorlabel"
                        runat="server" ControlToValidate="ddlConferenceHalls" ErrorMessage="<%$ Resources:rfvConferenceHallError %>"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div align="left">
                <div style="float: left; padding-top: 4px; width: 110px;">
                    <asp:Label ID="lArrName" runat="server" CssClass="control-label" meta:resourcekey="lArrName" />
                </div>
                <div style="float: left;">
                    <asp:TextBox ID="tbArrName" CssClass="control-textbox-required" runat="server" Width="370"
                        Font-Size="Small" /><br />
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbArrName"
                        ErrorMessage="<%$ Resources:rfvArrName %>"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div align="left" style="height: 130px;">
                <div style="float: left; width: 110px;">
                    <asp:Label ID="lDescription" runat="server" CssClass="control-label" meta:resourcekey="lDescription" />
                </div>
                <div style="float: left;">
                    <asp:TextBox ID="tbDescription" CssClass="control-textbox" runat="server" Width="373"
                        Height="60" TextMode="MultiLine" Font-Size="Small" />
                </div>
            </div>
            <div align="left" style="height: 180px;">
                <div style="float: left; width: 110px;">
                    <asp:Label ID="lDate" runat="server" CssClass="control-label" meta:resourcekey="lDate" />
                </div>
                <div style="float: left;">
                    <asp:Calendar ID="Calendar" runat="server" DayNameFormat="Short" Font-Name="Verdana;Arial"
                        Font-Size="14px" Height="80px" NextPrevFormat="ShortMonth" OtherMonthDayStyle-ForeColor="gray"
                        SelectMonthText="month" SelectWeekText="week" TodayDayStyle-Font-Bold="True"
                        Width="377" align="left">
                        <SelectedDayStyle BackColor="#FFCC66" Font-Bold="True" />
                        <TodayDayStyle Font-Bold="True" />
                        <SelectorStyle BackColor="#99CCFF" Font-Size="9px" ForeColor="Navy" />
                        <OtherMonthDayStyle ForeColor="Gray" />
                        <NextPrevStyle Font-Size="10px" ForeColor="White" />
                        <DayHeaderStyle Font-Bold="True" />
                        <TitleStyle BackColor="#3366FF" Font-Bold="True" ForeColor="White" />
                    </asp:Calendar>
                </div>
            </div>
            <div align="left">
                <div style="float: left; width: 110px;">
                    <asp:Label ID="lTimeBegin" runat="server" CssClass="control-label" meta:resourcekey="lTimeBegin" />
                </div>
                <div style="float: left; width: 370px">
                    <asp:TextBox ID="tbTimeBegin" CssClass="control-textbox-required" runat="server"
                        Width="100%" Font-Size="Small" />&nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="control-errorlabel"
                        runat="server" ControlToValidate="tbTimeBegin" ErrorMessage="<%$ Resources:rfvTimeBegin %>">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvTimeBegin" runat="server" ControlToValidate="tbTimeBegin"
                        CssClass="control-errorlabel" ErrorMessage="<%$ Resources:cvTimeError %>" OnServerValidate="cvTimeBegin_ServerValidate"
                        EnableClientScript="False"></asp:CustomValidator>
                </div>
            </div>
            <div align="left" style="height: 90px;">
                <div style="float: left; width: 110px;">
                    <asp:Label ID="lTimeEnd" runat="server" CssClass="control-label" meta:resourcekey="lTimeEnd" />
                </div>
                <div style="float: left; width: 370px">
                    <asp:TextBox ID="tbTimeEnd" CssClass="control-textbox-required" runat="server" Width="100%"
                        Font-Size="Small" />&nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="control-errorlabel"
                        runat="server" ControlToValidate="tbTimeEnd" ErrorMessage="<%$ Resources:rfvTimeEnd %>">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvTimeEnd" runat="server" ControlToValidate="tbTimeEnd"
                        CssClass="control-errorlabel" ErrorMessage="<%$ Resources:cvTimeError %>" OnServerValidate="cvTimeEnd_ServerValidate"></asp:CustomValidator>
                </div>
            </div>
            <div align="left" style="height: 160px;">
                <div style="width: 110px; height: 70px; float: left">
                    <asp:CheckBox ID="cbCyclicArrangement" runat="server" CssClass="control-label" meta:resourcekey="cbCyclicArrangement" />
                </div>
                <br />
                <div align="left" style="float: left; width: 375px; display: block;" id="cyclicArrDiv"
                    runat="server">
                    <div style="width: 30%; float: left;">
                        <asp:RadioButtonList ID="rbListDailyWeekly" CssClass="control-label" runat="server">
                            <asp:ListItem meta:resourcekey="rbDaily" Selected="True"></asp:ListItem>
                            <asp:ListItem meta:resourcekey="rbWeekly"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div style="width: 70%;">
                        <div style="display: block;" id="daysDiv">
                            <asp:RadioButtonList ID="rbDaily" CssClass="control-label" runat="server">
                                <asp:ListItem meta:resourcekey="rbEveryDay" Selected="True" />
                                <asp:ListItem meta:resourcekey="rbEveryNDays" />
                            </asp:RadioButtonList>
                            <div style="position: relative; top: -23px; left: 70%;">
                                <asp:TextBox runat="Server" ID="tbDayRepeatEvery" Width="20" />
                                <asp:Label ID="Label4" CssClass="control-label" runat="server" meta:resourcekey="rbEveryNDays2" />
                                <asp:CustomValidator ID="cvDayRepeatEvery" runat="server" ControlToValidate="tbDayRepeatEvery"
                                    ValidateEmptyText="true" ErrorMessage="<%$ Resources:rfvRepeatEvery %>" OnServerValidate="tbDayRepeatEvery_ServerValidate"
                                    CssClass="control-errorlabel" EnableClientScript="False"></asp:CustomValidator>
                            </div>
                        </div>
                        <div style="display: none; width: 330px;" id="weeksDiv">
                            <asp:Label ID="Label1" CssClass="control-label" runat="server" meta:resourcekey="lRepeatEvery" />
                            &nbsp;<asp:TextBox ID="tbWeekRepeatEvery" runat="server" Width="30px"></asp:TextBox>
                            <asp:Label ID="Label2" CssClass="control-label" runat="server" meta:resourcekey="lWeeksOn" />
                            <asp:CustomValidator ID="cvWeekRepeatEvery" runat="server" ControlToValidate="tbWeekRepeatEvery"
                                ValidateEmptyText="true" ErrorMessage="<%$ Resources:rfvRepeatEvery %>" OnServerValidate="tbWeedRepeatEvery_ServerValidate"
                                CssClass="control-errorlabel" EnableClientScript="False"></asp:CustomValidator>
                            <asp:CheckBoxList ID="cbDaysOfWeek" CssClass="control-label" runat="server">
                                <asp:ListItem Value="Mo" meta:resourcekey="cbMonday" />
                                <asp:ListItem Value="Tu" meta:resourcekey="cbTuesday" />
                                <asp:ListItem Value="We" meta:resourcekey="cbWednesday" />
                                <asp:ListItem Value="Th" meta:resourcekey="cbThursday" />
                                <asp:ListItem Value="Fr" meta:resourcekey="cbFriday" />
                                <asp:ListItem Value="Sa" meta:resourcekey="cbSaturday" />
                                <asp:ListItem Value="Su" meta:resourcekey="cbSunday" />
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <hr />
                    <div style="width: 70%;">
                        <asp:RadioButtonList ID="rbEnd" CssClass="control-label" runat="server">
                            <asp:ListItem meta:resourcekey="rbEndAfter" />
                            <asp:ListItem meta:resourcekey="rbEndBy" Selected="True" />
                        </asp:RadioButtonList>
                        <div style="position: relative; top: -46px; left: 50%;">
                            <asp:TextBox runat="Server" ID="tbCount" Width="20" />
                            <asp:Label ID="Label5" CssClass="control-label" runat="server" meta:resourcekey="rbEveryNDays2" />
                            <asp:CustomValidator ID="cvEndCount" runat="server" ControlToValidate="tbCount" ValidateEmptyText="true"
                                ErrorMessage="<%$ Resources:rfvEndCount %>" OnServerValidate="tbCount_ServerValidate"
                                CssClass="control-errorlabel" EnableClientScript="False"></asp:CustomValidator>
                        </div>
                        <div style="position: relative; top: -44px; left: 30%;">
                            <div style="float: left; padding-left: 5px;">
                                <asp:TextBox ID="tbEndDate" runat="server" CssClass="control-textbox" EnableTheming="false"
                                    Width="70px" />
                            </div>
                            <div style="float: left; padding-left: 5px;">
                                <uc:Calendar ID="calendarEndDate" runat="server" TargetControlID="tbEndDate" />
                            </div>
                            <asp:CustomValidator ID="cvEndDate" runat="server" ControlToValidate="tbEndDate"
                                ValidateEmptyText="true" ErrorMessage="<%$ Resources:rfvEndDate %>" OnServerValidate="tbEndDate_ServerValidate"
                                CssClass="control-errorlabel" EnableClientScript="False"></asp:CustomValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div align="left" style="height: 70px;">
                <div style="float: left; width: 110px;">
                    <asp:Label ID="lListOfGuests" runat="server" CssClass="control-label" meta:resourcekey="lListOfGuests" />
                </div>
                <div style="float: left;">
                    <asp:TextBox ID="tbListOfGuests" CssClass="control-textbox" runat="server" Width="373"
                        Height="60px" TextMode="MultiLine" Font-Size="Small" />
                </div>
            </div>
            <div align="left" style="height: 70px;">
                <div style="float: left; width: 110px;">
                    <asp:Label ID="lEquipment" runat="server" CssClass="control-label" meta:resourcekey="lEquipment" />
                </div>
                <div style="float: left;">
                    <asp:TextBox ID="tbEquipment" CssClass="control-textbox" runat="server" Width="373"
                        Height="60px" TextMode="MultiLine" Font-Size="Small" />
                </div>
            </div>
            <div style="top">
                <asp:CustomValidator ID="cvCheckAdding" runat="server" ErrorMessage="<%$ Resources:cvCheckAdding %>"
                    OnServerValidate="cvCheckAdding_ServerValidate"></asp:CustomValidator>
                <asp:CustomValidator ID="cvCheckOffice" runat="server" ErrorMessage="<%$ Resources:cvCheckOffice %>"
                    OnServerValidate="cvCheckOffice_ServerValidate"></asp:CustomValidator>
            </div>
            <asp:Button ID="btnApply" CssClass="control-button" runat="server" OnClick="btnApply_Click"
                meta:resourcekey="btnApply" Width="100" />
            <asp:Button ID="btnDelete" CssClass="control-button" runat="server" OnClick="btnDelete_Click"
                meta:resourcekey="btnDelete" Width="138px" />
        </div>
    </div>
</asp:Content>
