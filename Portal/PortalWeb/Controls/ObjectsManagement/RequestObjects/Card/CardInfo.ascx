<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CardInfo.ascx.cs" Inherits="CardInfo" EnableTheming="false" %>

<%@ Register Src="~/controls/ActionsMenu/ActionsMenuCtl.ascx" TagName="ActionsMenu" TagPrefix="uc1" %>

<div class="control">
    <uc1:ActionsMenu ID="menuActions" runat="server" 
        ViewName="req_objects_info_action_menu" 
        MenuForNotSelectedObjectActionsCriteria="./controls/ActionsMenu/s_s_and_new.xml" />   

    <div class="control-body">
        <div class="control-line-of-controls">
            <div style="float: left; width: 100px; padding-top: 4px;">
                <asp:Label ID="lblTitle" runat="server" CssClass="control-label" meta:resourcekey="Title" />
            </div>
            <div style="float: left; width: 300px;">
                <asp:TextBox ID="tbxTitle" runat="server" CssClass="control-textbox-required" Width="100%" />
            </div>
            <div class="control-errorlabel" id="reqTitle" runat="server" visible="false" style="float: left; padding-left: 6px; padding-top: 8px">*</div>
        </div>
        
        <div class="control-line-between"></div>
        <div class="control-line-of-controls">
            <div style="float: left; width: 100px; padding-top: 4px;">
                <asp:Label ID="lblShopName" runat="server" CssClass="control-label" meta:resourcekey="ShopName" />
            </div>
            <div style="float: left; width: 300px;">
                <asp:TextBox ID="tbxShopName" runat="server" CssClass="control-textbox-required" Width="100%" />
            </div>
            <div class="control-errorlabel" id="reqShopName" runat="server" visible="false" style="float: left; padding-left: 6px; padding-top: 8px">*</div>
            <div style="float: left; width: 80px; padding-top: 4px; padding-left: 15px;">
                <asp:Label ID="lblShopSite" runat="server" CssClass="control-label" meta:resourcekey="ShopSite" />
            </div>
            <div style="float: left; width: 300px">
                <asp:TextBox ID="tbxShopSite" runat="server" CssClass="control-textbox" Width="100%" />
            </div>
        </div>
                        
        <div class="control-line-between"></div>
        <div class="control-line-of-controls">
            <div style="float: left; width: 100px; padding-top: 4px;">
                <asp:Label ID="lblValuePercent" runat="server" CssClass="control-label" meta:resourcekey="ValuePercent" />
            </div>
            <div style="float: left; width: 40px;">
                <asp:TextBox ID="tbxValuePercent" runat="server" CssClass="control-textbox-required" Width="100%" MaxLength="3" />
            </div>
            <div class="control-errorlabel" id="reqValuePercent" runat="server" visible="false" style="float: left; padding-left: 6px; padding-top: 8px">*</div>
        </div>
                                
        <div class="control-line-between"></div>
        <div class="control-line-of-controls">
            <div style="float: left; width: 100px; padding-top: 4px;">
                <asp:Label ID="lblOwner" runat="server" CssClass="control-label" meta:resourcekey="Owner" />
            </div>
            <div style="float: left; width: 300px">
                <asp:DropDownList ID="ddlOwner" runat="server" Width="100%" CssClass="control-dropdownlist" />
            </div>
            <div style="float: left; width: 80px; padding-top: 4px; padding-left: 20px;">
                <asp:Label ID="lblOffice" runat="server" CssClass="control-label" meta:resourcekey="Office" />
            </div>
            <div style="float: left; width: 300px">
                <asp:DropDownList ID="ddlOffices" runat="server" CssClass="control-dropdownlist" Width="100%" />
            </div>
        </div>   
    </div>
</div>