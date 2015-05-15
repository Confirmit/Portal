<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserAttributeInfo" Codebehind="UserAttributeInfo.ascx.cs" %>

<div style="padding-top: 5px;" class="control-line-of-controls">
    <div style="float: left; width: 60px; padding-top: 3px;">
        <asp:Label runat="server" ID="lAttrName" CssClass = "control-label" meta:resourcekey="AttrName" />
	</div>
	<div style="float: left; width: 140px;">	    
	    <asp:TextBox ID="tbAttrName" runat="server" EnableTheming="false" CssClass="control-textbox-required" Width="100%" />
	</div>
</div>	

<div class="control-line-between"></div>
<asp:Label runat="server" ID="lbErrorText" CssClass="control-errorlabel" Visible="true" />

<div class="control-line-of-controls" style="float: left;">
    <asp:CheckBox ID="cbShowToUsers" runat="server" CssClass = "control-label" meta:resourcekey="AttrShowToUsers" />
</div>    

<div class="control-line-of-controls">
    <div style="float: left; padding-left: 5px;">
        <asp:Button ID="btnCancelEditAttribute" runat="server" 
	        meta:resourcekey="CancelEditAttr"
		    onclick="btnCancelEditAttribute_Click"
		    CssClass="control-button" 
		    />
    </div>
    <div style="float: left; padding-left: 5px;">
        <asp:Button ID="btnAddAttribute" runat="server" 
	        meta:resourcekey="SaveAttr"
		    onclick="btnAddAttribute_Click"
		    CssClass="control-button" 
		    />
    </div>
</div>