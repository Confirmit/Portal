<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" UICulture="auto" Inherits="Events_AccessViolation" Codebehind="AccessDenied.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <center>
        <div id="divError" class="control-border" style="height: 120px; width: 480px; padding: 10px" >
            <label id="lblError" meta:resourcekey="lblError" runat="server" />
        </div>
    </center>
</asp:Content>

