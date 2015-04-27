<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BookThemesInfo.ascx.cs" Inherits="BookThemesInfo" EnableTheming="false" %>

<%@ Register Src="~/controls/ActionsMenu/ActionsMenuCtl.ascx" TagName="ActionsMenu" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MLTextBox.ascx" TagPrefix="usp" TagName="MLTextBox" %>

<div class="control" id=<%=ClientID%>>
    <uc2:ActionsMenu ID="menuActions" runat="server" 
        ViewName="book_themes_action_menu" 
        MenuForNotSelectedObjectActionsCriteria="./controls/ActionsMenu/s_s_and_new.xml"
    />

    <div class="control-body" style="padding-top: 5px; padding-bottom : 10px;">
        <div align="left" style="width: 100%" class="control-line-of-controls">
            <div style="float: left; padding-top: 4px; width: 110px;">
                <asp:Label ID="lblThemeName" runat="server" CssClass="control-label" meta:resourcekey="lblThemeName" />
            </div>
            <div style="float: left; padding-left: 5px;">
                <usp:MLTextBox ID="mltbThemeName" runat="server" Width="100%"
                    DropDownListCssClass="control-dropdownlist"
                    TextBoxCssClass="control-textbox-required" />
            </div>
        </div>
    
        <asp:Label Width="100%" ID="reqName" runat="server" CssClass="control-errorlabel" 
            Text="Missing Information - Date." Visible="false" />  
    </div>
</div>