<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    CodeFile="Archive.aspx.cs" Inherits="NewsTape_Archive" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="uc1" TagName="NewsTopMenu" Src="~/Controls/News/NewsTopMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewsGrid" Src="~/Controls/NewsGrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <uc1:NewsTopMenu ID="NewsTopMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <center>
        <asp:Label ID="lblCaptionArchive" Font-Bold="true" runat="server" meta:resourcekey="lblCaptionArchive" />
    </center>
    <br />
    <uc1:NewsGrid ID="gvArchive" runat="server" />
</asp:Content>
