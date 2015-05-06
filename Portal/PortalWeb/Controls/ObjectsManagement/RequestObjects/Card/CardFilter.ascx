<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CardFilter.ascx.cs" Inherits="CardFilter" EnableTheming="false" %>

<div class="control" >
    <asp:Panel ID="pnlFilterHeader" runat="server">
        <div class="control-header">
            <div class="control-header-title">
                <asp:Localize runat="server" ID="locFilterHeader" meta:resourcekey="FilterHeader" />
            </div>
            <div class="control-header-buttons">
                <asp:ImageButton ID="imgFilterCollapsedStatus" runat="server" ImageUrl="~/Images/expand.jpg" AlternateText="Show" />
            </div>
        </div>
    </asp:Panel>
    
    <asp:Panel ID="pnlFilter" runat="server">
        <div class="control-body">
            <div class="control-line-between"></div>
            <div class="control-line-of-controls">
                <div style="float: left; width: 100px; padding-top: 4px;">
                    <asp:Label ID="lblTitle" runat="server" CssClass="control-label" meta:resourcekey="Title" />
                </div>
                <div style="float: left; width: 300px;">
                    <asp:TextBox ID="tbxTitle" runat="server" CssClass="control-textbox" Width="100%" />
                </div>
                <div style="float: left; width: 80px; padding-left: 20px; padding-top: 4px;">
                    <asp:Label ID="lblShopName" runat="server" CssClass="control-label" meta:resourcekey="ShopName" />
                </div>
                <div style="float: left; width: 300px">
                    <asp:TextBox ID="tbxShopName" runat="server" CssClass="control-textbox" Width="100%" />
                </div>
            </div>
                                    
            <div class="control-line-between"></div>
            <div class="control-line-of-controls">
                <div style="float: left; width: 100px; padding-top: 4px;">
                    <asp:Label ID="lblValuePercent" runat="server" CssClass="control-label" meta:resourcekey="ValuePercent" />
                </div>
                <div style="float: left; width: 300px">
                    <asp:TextBox ID="tbxValuePercent" runat="server" CssClass="control-textbox" Width="100%" />
                </div>
            </div>            
                                                
            <div class="control-line-between"></div>
            <div class="control-line-of-controls">
                <div style="float: left; width: 100px; padding-top: 4px;">
                    <asp:Label ID="lblOffice" runat="server" CssClass="control-label" meta:resourcekey="Office" />
                </div>
                <div style="float: left; width: 300px">
                    <asp:DropDownList ID="ddlOffices" runat="server" 
                        CssClass="control-dropdownlist" Width="100%"
                        DataTextField="OfficeName" DataValueField="ID" AppendDataBoundItems="True">
                    </asp:DropDownList>
                </div>
            </div>   
                                                
            <div class="control-line-between"></div>
            <div class="control-line-of-controls">
                <div style="float: right">
                    <asp:Button ID="btnSearch" runat="server"  CssClass="control-button" Width="100px" meta:resourcekey="Search" />
                </div>
               <div style="float: right; padding-right: 4px;">
                    <asp:Button ID="btnReset" runat="server"  CssClass="control-button" Width="100px" meta:resourcekey="Reset" />
                </div>
            </div>
        </div>
    </asp:Panel>
</div>    

<ajaxToolkit:CollapsiblePanelExtender ID="cpeFilter" runat="server" TargetControlID="pnlFilter"
    ExpandControlID="pnlFilterHeader" CollapseControlID="pnlFilterHeader" Collapsed="true"
    ImageControlID="imgFilterCollapsedStatus" ExpandedImage="~/Images/collapse.jpg"
    CollapsedImage="~/Images/expand.jpg" SuppressPostBack="true" />