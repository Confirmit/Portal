<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopMenu.ascx.cs" Inherits="Controls_TopMenu" %>

<asp:HyperLink ID="hlHome" NavigateUrl="~/Default.aspx" 
    runat="server" 
    meta:resourcekey="hlHome" 
    CssClass="control-hyperlink-big"
/>

&nbsp;&nbsp;
<asp:HyperLink ID="hlArrangements" NavigateUrl="~/Arrangements/Default.aspx"
    runat="server"
    meta:resourcekey="hlArrangements" 
    CssClass="control-hyperlink-big"
/>

&nbsp;&nbsp;
<asp:HyperLink ID="hlNews" NavigateUrl="~/NewsTape/FullNewsTape.aspx" 
    meta:resourcekey="hlNews" 
    runat="server"
    CssClass="control-hyperlink-big"
/>

&nbsp;&nbsp;
<asp:HyperLink ID="hlForum" NavigateUrl="~/Forums/Default.aspx" 
    runat="server" 
    meta:resourcekey="hlForum" 
    CssClass="control-hyperlink-big"
/>

&nbsp;&nbsp;
<asp:LinkButton ID="btnLogOut" OnClick="OnLogOut" 
    runat="server" meta:resourcekey="btnLogOut" 
    CausesValidation="False" 
    CssClass="control-hyperlink-big"
/>

