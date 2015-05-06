<%@ Control Language="C#" AutoEventWireup="true" Inherits="DiskFilter" EnableTheming="false" Codebehind="DiskFilter.ascx.cs" %>

<div class="control" >
    <asp:Panel ID="pnlFilterHeader" runat="server">
        <div class="control-header">
            <div class="control-header-title">
                <asp:Localize runat="server" ID="locFilterHeader" meta:resourcekey="FilterHeader" />
            </div>
            <div class="control-header-buttons">
                <asp:ImageButton ID="imgFilterCollapsedStatus" runat="server" ImageUrl="~/Images/expand.png" AlternateText="Show" />
            </div>
        </div>
    </asp:Panel>
    
    <asp:Panel ID="pnlFilter" runat="server">
        <div class="control-body">
            <div class="control-line-of-controls">
                <div style="float: left; width: 100px; padding-top: 4px;">
                    <asp:Label ID="lblTitle" runat="server" CssClass="control-label" meta:resourcekey="Title" />
                </div>
                <div style="float: left; width: 300px;">
                    <asp:TextBox ID="tbxTitle" runat="server" CssClass="control-textbox" Width="100%" />
                </div>
                <div style="float: left; width: 80px; padding-left: 20px; padding-top: 4px;">
                    <asp:Label ID="lblManufacturers" runat="server" CssClass="control-label" meta:resourcekey="Manufacturers" />
                </div>
                <div style="float: left; width: 300px">
                    <asp:TextBox ID="tbxManufacturers" runat="server" CssClass="control-textbox" Width="100%" />
                </div>
            </div>
                                    
            <div class="control-line-between"></div>
            <div class="control-line-of-controls">
                <div style="float: left; width: 100px; padding-top: 4px;">
                    <asp:Label ID="lblAnnotation" runat="server" CssClass="control-label" meta:resourcekey="Annotation" />
                </div>
                <div style="float: left; width: 300px">
                    <asp:TextBox ID="tbxAnnotation" runat="server" CssClass="control-textbox" Width="100%" />
                </div>
            </div>            

            <div class="control-line-between"></div>
            <div class="control-line-of-controls">
                <div style="float: left; width: 100px; padding-top: 4px;">
                    <asp:Label ID="lblPublishingYear" runat="server" CssClass="control-label" meta:resourcekey="PublishingYear" />
                </div>
                <div style="float: left; width: 30px; padding-top: 4px">
                    <asp:Label ID="lblFromPublishingYear" runat="server" meta:resourcekey="FromPublishingYear" CssClass="control-label" />
                </div>
                <div style="float: left; width: 40px;">
                    <asp:TextBox ID="tbxFromPublishingYear" runat="server" CssClass="control-textbox" Width="100%" Text="1800" MaxLength="4" />
                </div>
                <div style="float: left; width: 20px; padding-top: 4px; padding-left: 10px;">
                    <asp:Label ID="lblToPublishingYear" runat="server" meta:resourcekey="ToPublishingYear" CssClass="control-label" />
                </div> 
                <div style="float: left; width: 40px">
                    <asp:TextBox ID="tbxToPublishingYear" runat="server" CssClass="control-textbox" Width="100%" MaxLength="4" />
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
    ImageControlID="imgFilterCollapsedStatus" ExpandedImage="~/Images/collapse.png"
    CollapsedImage="~/Images/expand.png" SuppressPostBack="true" />