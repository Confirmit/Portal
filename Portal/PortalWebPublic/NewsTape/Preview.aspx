<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    UICulture="auto" CodeFile="Preview.aspx.cs" Inherits="NewsTape_Preview" %>

<%@ Register TagPrefix="uc1" TagName="NewsTopMenu" Src="~/Controls/NewsTopMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FullNews" Src="~/Controls/FullNews.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <uc1:NewsTopMenu ID="NewsTopMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <uc1:FullNews ID="fullNews" runat="server" align="center" />
    
    <center style="width: 500px; padding-top: 5px;">     
        <div style="float: left; padding-left: 10px; width: 33%;">
            <input id="btnBackToEdit" type="button" runat="server" 
                meta:resourcekey="btnBackToEdit"
                onclick="javascript:history.go(-1);" 
                style="width:100%;"                  
                class="control-button"
            />                 
        </div>
        <div style="float: left; padding-left: 10px; width: 30%;">
            <asp:Button ID="btnDelete" runat="server" 
                meta:resourcekey="btnDelete" 
                OnClick="btnDelete_Click"  
                Width="100%" 
                CssClass="control-button"
            />
        </div>
        <div style="float: left; padding-left: 10px; width: 30%;">
            <asp:Button ID="btnPublish" runat="server" 
                meta:resourcekey="btnPublish" 
                OnClick="btnPublish_Click" 
                Width="100%" 
                CssClass="control-button"
            />
        </div>
    </center>
</asp:Content>
