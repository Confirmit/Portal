<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    CodeFile="Themes.aspx.cs" Inherits="ConfirmIt.Portal.RequestObjects.ThemesPage" meta:resourcekey="Page" %>

<%@ Register Src="~/Controls/ObjectsManagement/BookThemes/BookThemesGrid.ascx" TagName="BookThemesGrid" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ObjectsManagement/BookThemes/BookThemesInfo.ascx" TagName="BookThemesInfo" TagPrefix="uc1" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <asp:ScriptManagerProxy ID="scriptManagerProxy" runat="server" />
    
    <asp:UpdatePanel ID="upThemes" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div style="width: 510px;">
                <uc1:BookThemesGrid ID="bookThemesGrid" runat="server" />
            </div>
            
            <div class="control-line-between"></div>
            <div style="width: 510px;">
                <uc1:BookThemesInfo ID="bookThemesInfo" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>