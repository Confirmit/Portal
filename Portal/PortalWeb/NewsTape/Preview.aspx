<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    CodeFile="Preview.aspx.cs" Inherits="NewsTape_Preview" %>

<%@ Register TagPrefix="uc1" TagName="NewsTopMenu" Src="~/Controls/News/NewsTopMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FullNews" Src="~/Controls/News/FullNews.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <uc1:NewsTopMenu ID="NewsTopMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <br />
    <uc1:FullNews ID="fullNews" runat="server" align="center" />
    
    <table align="center">
        <tr>
            <td width="180px">
                <input id="btnBackToEdit" type="button" runat="server" 
                    meta:resourcekey="btnBackToEdit" class="control-button"
                    style="width:100%;" onclick="javascript:history.go(-1);" />
            </td>
            <td width="130px">
                <asp:Button ID="btnDelete" runat="server" 
                    meta:resourcekey="btnDelete" CssClass="control-button"
                    OnClick="btnDelete_Click" width="100%" />
            </td>
            <td width="130px">
                <asp:Button ID="btnPublish" runat="server" 
                    meta:resourcekey="btnPublish" CssClass="control-button"
                    OnClick="btnPublish_Click" width="100%" />
            </td>
        </tr>
    </table>
</asp:Content>
