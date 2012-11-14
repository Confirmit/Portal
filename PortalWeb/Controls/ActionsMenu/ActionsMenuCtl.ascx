<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActionsMenuCtl.ascx.cs" Inherits="ActionsMenuCtl" %>

<%@ Register Assembly="Controls" Namespace="Controls.jQueryMenu" TagPrefix="cc1" %>

<div id="<%=ClientID%>">
    <cc1:jQueryMenu id="menu" runat="server"/>
</div>
