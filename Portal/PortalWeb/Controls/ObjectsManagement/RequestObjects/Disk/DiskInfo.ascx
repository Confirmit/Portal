<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiskInfo.ascx.cs" Inherits="DiskInfo" EnableTheming="false" %>

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
            <div style="float: left; width: 80px; padding-top: 4px; padding-left: 15px;">
                <asp:Label ID="lblManufacturers" runat="server" CssClass="control-label" meta:resourcekey="Manufacturers" />
            </div>
            <div style="float: left; width: 300px">
                <asp:TextBox ID="tbxManufacturers" runat="server" CssClass="control-textbox-required" Width="100%" />
            </div>
            <div class="control-errorlabel" id="reqManufacturers" runat="server" visible="false" style="float: left; padding-left: 6px; padding-top: 8px">*</div>
        </div>
                        
        <div class="control-line-between"></div>
        <div class="control-line-of-controls">
            <div style="float: left; width: 100px; padding-top: 4px;">
                <asp:Label ID="lblPublishingYear" runat="server" CssClass="control-label" meta:resourcekey="PublishingYear" />
            </div>
            <div style="float: left; width: 40px;">
                <asp:TextBox ID="tbxPublishingYear" runat="server" CssClass="control-textbox-required" Width="100%" MaxLength="4" />
            </div>
            <div class="control-errorlabel" id="reqPublishingDate" runat="server" visible="false" style="float: left; padding-left: 6px; padding-top: 8px">*</div>
        </div>
                
        <div class="control-line-between"></div>
        <div style="height: 100px" class="control-line-of-controls">
            <div style="float: left; width: 100px; padding-top: 4px;">
                <asp:Label ID="lblAnnotation" runat="server" CssClass="control-label" meta:resourcekey="Annotation" />
            </div>
            <div style="float: left; width: 700px; height: 100px">
                <textarea id="tbxAnnotation" runat="server" class="control-texbox" style="width: 100%; height: 100%;" />
            </div>
            <div class="control-errorlabel" id="reqAnnotation" runat="server" visible="false" style="float: left; padding-left: 6px; padding-top: 8px">*</div>
        </div>            
                
        <div class="control-line-between"></div>
        <div class="control-line-of-controls">
            <div style="float: left; width: 100px; padding-top: 4px;">
                <asp:Label ID="lblOffice" runat="server" CssClass="control-label" meta:resourcekey="Office" />
            </div>
            <div style="float: left; width: 300px">
                <asp:DropDownList ID="ddlOffices" runat="server" CssClass="control-dropdownlist" Width="100%" />
            </div>
        </div>   
    </div>
</div>