<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Main.master" 
    Inherits="RequestObjects" meta:resourcekey="Page" Codebehind="RequestObjects.aspx.cs" %>

<%@ Register Src="~/Controls/ObjectsManagement/RequestObjects/RequestObjectEditControl.ascx" TagName="RequestObjectEditControl" TagPrefix="uc1" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <div style="width: 850px;" align="left" >
        <uc1:RequestObjectEditControl ID="reqObjEditControl" runat="server" />
    </div>
</asp:Content>