<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CultureChanger.ascx.cs" Inherits="Controls_CultureChanger" %>

<div class="control-label" style="float: left; padding-top: 2px;">
    <asp:Localize ID="locChangeText" runat="server" meta:resourcekey="changeText" />
</div>
<div style="float: left; padding-left: 5px;">
    <asp:DropDownList ID="ddlCultures" Width="100" 
        CssClass="control-dropdownlist"
	    DataTextField="NativeName" DataValueField="Name" 
	    AutoPostBack="true" runat="server" 
	    OnSelectedIndexChanged="OnCultureChanged"
	/>
</div>