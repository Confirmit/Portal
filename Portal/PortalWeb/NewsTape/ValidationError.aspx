<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeFile="ValidationError.aspx.cs" Inherits="Secure_ValidationError"%>
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

