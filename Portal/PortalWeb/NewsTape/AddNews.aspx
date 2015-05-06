<%@ Page Language="C#" ValidateRequest="false" MasterPageFile="~/MasterPages/Main.master"
    AutoEventWireup="true" CodeFile="AddNews.aspx.cs" Inherits="NewsTape_AddNews"
    EnableSessionState="true" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="uc1" TagName="NewsTopMenu" Src="~/Controls/News/NewsTopMenu.ascx" %>
<%@ Register Src="~/Controls/News/NewsAttachFile.ascx" TagName="NewsAttachFile" TagPrefix="uc2" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <uc1:NewsTopMenu ID="NewsTopMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">

    <center class="control-header" style="width: 514px;">
        <asp:Label ID="lblAddNews" runat="server" Font-Bold="true" meta:resourcekey="lblAddNews" />
    </center>
    
    <table class="control" style="width: 500px" border="0px;">
        <tr>
            <td>                
                <div style="float: left;">
                    <%--   <asp:Image ID="imgGeneralNews" runat="server" />--%>
                </div>
                <div style="float: left;">
                    <asp:RadioButtonList ID="rbOffice" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 64px">
                <asp:Label ID="lblCaptionNews" runat="server" meta:resourcekey="lblCaptionNews" />
                <asp:TextBox ID="tbCaptionNews" runat="server" Width="500px" />
                <asp:RequiredFieldValidator CssClass="control-errorlabel" ID="CaptionRequiredFieldValidator" runat="server" 
                    meta:resourcekey="CaptionRequiredFieldValidator" ControlToValidate="tbCaptionNews" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTextNews" runat="server" meta:resourcekey="lblTextNews" />
                <asp:TextBox ID="tbTextNews" runat="server" TextMode="MultiLine" Rows="10" Width="500px"
                    Style="overflow-y: auto" />
                <asp:RequiredFieldValidator CssClass="control-errorlabel" ID="TextRequiredFieldValidator" runat="server" 
                    meta:resourcekey="TextRequiredFieldValidator" ControlToValidate="tbTextNews" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblAttachFiles" runat="server" meta:resourcekey="lblAttachFiles" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView runat="server" ID="gridAttachments" Width="100%"
                    AutoGenerateColumns="false" 
                    EnableTheming="false"
                    ShowHeader="false"
                    GridLines="None"
                    HeaderStyle="height: 0px"
                    CellSpacing="0"
                    CellPadding="0"
                    AllowPaging="false" AllowSorting="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <uc2:NewsAttachFile runat="server" ID="attachFile" 
                                    IsEditMode="true" 
                                    Attachment="<%# Container.DataItem %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="control-attach-row" />
                    <AlternatingRowStyle CssClass="control-attach-row-alternating" />
                    <HeaderStyle Height="0" />
                </asp:GridView>
            
                <div id="div_newsAttachments" runat="server" >
                    <input class="attachfile-hide-server" type="file" id="uploadFile" runat="server" size="40" />
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblExpireTime" runat="server" meta:resourcekey="lblExpireTime" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Calendar ID="Calendar" runat="server" 
                    DayNameFormat="Short" 
                    Font-Name="Verdana;Arial"
                    Font-Size="14px" 
                    Height="160px" 
                    NextPrevFormat="ShortMonth" 
                    OtherMonthDayStyle-ForeColor="gray"
                    SelectMonthText="month" 
                    SelectWeekText="week" 
                    TodayDayStyle-Font-Bold="True"
                    Width="500px" align="center" 
                    OnSelectionChanged="OnSelectionChanged"
                >
                    <SelectedDayStyle BackColor="#FFCC66" Font-Bold="True" />
                    <TodayDayStyle Font-Bold="True" />
                    <SelectorStyle BackColor="#99CCFF" Font-Size="9px" ForeColor="Navy" />
                    <OtherMonthDayStyle ForeColor="Gray" />
                    <NextPrevStyle Font-Size="10px" ForeColor="White" />
                    <DayHeaderStyle Font-Bold="True" />
                    <TitleStyle BackColor="#3366FF" Font-Bold="True" ForeColor="White" />
                </asp:Calendar>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblConfirmExpDate" CssClass="control-errorlabel" runat="server" meta:resourcekey="lblConfirmExpDate" Visible="false" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID="cbSendNotification" runat="server" meta:resourcekey="cbSendNotification"/>
            </td>
        </tr>
        </table>
        
        <table align="center">
            <tr align="center">
                <td width="30%">
                    <asp:Button ID="btnAddNews" CssClass="control-button" 
                        runat="server" meta:resourcekey="btnAddNews" 
                        OnClick="btnAddNews_Click" Width="100%" />
                </td>
                <td width="40%">
                    <asp:Button ID="btnDeleteNews" CssClass="control-button" 
                        runat="server" meta:resourcekey="btnDeleteNews" Width="100%"
                        OnClick="btnDeleteNews_Click" 
                        CausesValidation="False" />
                </td>
                <td width="30%">
                    <asp:Button ID="btnPreview" CssClass="control-button" 
                        runat="server" meta:resourcekey="btnPreview" 
                        OnClick="btnPreview_Click"
                        Width="100%" />
                </td>
            </tr>
    </table>
    
</asp:Content>
