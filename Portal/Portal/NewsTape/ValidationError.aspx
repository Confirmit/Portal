<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="True" Inherits="Secure_ValidationError" Codebehind="ValidationError.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <center>
        <div id="divError" style="height: 120px; width: 480px; border: solid 1px black; padding: 10px" >
        <asp:Label id="lblError" meta:resourcekey="lblError" runat="server" />
        <br />
        <asp:LinkButton ID="hlBack" OnClientClick="javascript:history.go(-1);" meta:resourcekey="hlBack" runat="server" />
        </div>
    </center>
</asp:Content>

