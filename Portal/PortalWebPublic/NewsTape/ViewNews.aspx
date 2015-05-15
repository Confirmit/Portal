<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    UICulture="auto" CodeFile="ViewNews.aspx.cs" Inherits="NewsTape_ViewNews" %>

<%@ Register TagPrefix="uc1" TagName="FullNews" Src="~/Controls/FullNews.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewsTopMenu" Src="~/Controls/NewsTopMenu.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <uc1:NewsTopMenu ID="NewsTopMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <uc1:FullNews ID="fullNews" runat="server" align="center" />
    
    
    <center style="width: 500px; padding-top: 5px;">
    <div style="float: left; width: 30%;">
        <input class="control-button" id="btnBack" type="button" runat="server" 
            meta:resourcekey="btnBack" 
            style="width: 100%;"
            onclick="javascript:history.go(-1);" />
    </div>
    </center>
    
</asp:Content>
