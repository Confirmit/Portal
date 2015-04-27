<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="True" Inherits="NewsTape_ViewNews" Codebehind="ViewNews.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="FullNews" Src="~/Controls/News/FullNews.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewsTopMenu" Src="~/Controls/News/NewsTopMenu.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <uc1:NewsTopMenu ID="NewsTopMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <br />
    <uc1:FullNews ID="fullNews" runat="server" align="center" />
    
    <div style="width: 150px; padding-top: 5px;">
        <input id="btnBack" type="button" class="control-button" runat="server" meta:resourcekey="btnBack" onclick="javascript:history.go(-1);" />
    </div>
</asp:Content>
