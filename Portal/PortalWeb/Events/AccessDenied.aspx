<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" UICulture="auto" CodeFile="AccessDenied.aspx.cs" Inherits="Events_AccessViolation" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <center>
        <div id="divError" class="control-border" style="height: 120px; width: 480px; padding: 10px" >
            <label id="lblError" meta:resourcekey="lblError" runat="server" />
        </div>
    </center>
</asp:Content>

