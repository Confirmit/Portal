<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="True" UICulture="auto" Inherits="NewsTape_AccessDenied" Codebehind="AccessDenied.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="NewsTopMenu" Src="~/Controls/News/NewsTopMenu.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" Runat="Server">
     <uc1:NewsTopMenu ID="NewsTopMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <center>
        <div id="divError" style="height: 120px; width: 480px; border: solid 1px black; padding: 10px" >
        <label id="lblError" meta:resourcekey="lblError" runat="server"></label>
        <br />
        <a href="~//NewsTape//FullNewsTape.aspx" id="hlFullNewsTape" meta:resourcekey="hlFullNewsTape" runat="server" />
        </div>
    </center>
</asp:Content>

