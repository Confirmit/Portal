<%@ Control Language="C#" AutoEventWireup="true" Inherits="ActionsMenuCtl" Codebehind="ActionsMenuCtl.ascx.cs" %>

<%@ Register Assembly="Controls" Namespace="Controls.jQueryMenu" TagPrefix="cc1" %>

<div id="<%=ClientID%>">
    <cc1:jQueryMenu id="menu" runat="server"/>
</div>