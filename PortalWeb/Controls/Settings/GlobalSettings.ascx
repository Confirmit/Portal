<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GlobalSettings.ascx.cs" Inherits="Admin_GlobalSettings" %>

<%@ Register TagName="MainSettings" TagPrefix="uc1" Src="~/Controls/Settings/MainSettings.ascx" %>
<%@ Register TagName="ForumSettings" TagPrefix="uc2" Src="~/Controls/Settings/ForumSettings.ascx" %>
<%@ Register TagName="OfficesSettings" TagPrefix="uc3" Src="~/Controls/Settings/OfficesSettings.ascx" %>

<div class="sectionCaption">
    <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitle" />
</div>

<div class="control-line-between"></div>
<center>
    <div class="control" style="width: 600px" >
        <div class="control-body" align="left">
             <ajaxToolkit:TabContainer ID="SettingsTabContainer" runat="server" >
                <ajaxToolkit:TabPanel ID="mainSettings" runat="server" meta:resourcekey="MainSettings">
                    <ContentTemplate>
                        <uc1:MainSettings ID="mainCommonSettings" runat="server" />
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel ID="forumSettingsPanel" runat="server" meta:resourcekey="ForumSettings">
                    <ContentTemplate>
                        <uc2:ForumSettings ID="forumSettings" runat="server" />
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel ID="TaofficeSettingsbPanel1" runat="server" meta:resourcekey="OfficeSettings">
                    <ContentTemplate>
                        <uc3:OfficesSettings ID="officesSettings" runat="server" />
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>

            <div class="control-line-between"></div>
            <div style="white-space: nowrap; width: 100%; text-align: center">
                <asp:Button ID="btnApply" CssClass="control-button" runat="server" 
                    OnClick="btnApply_Click" meta:resourcekey="btnApply" Width="100" TabIndex="0" />
                <asp:Button ID="btnCancel" CssClass="control-button" runat="server" 
	                meta:resourcekey="btnCancel" Width="100" CausesValidation="false" />
            </div>
        </div>
    </div>
</center>
