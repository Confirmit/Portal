<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FullNews.ascx.cs" Inherits="NewsTape_FullNews" %>

<%@ Register Src="~/Controls/News/NewsAttachFile.ascx" TagName="NewsAttachFile" TagPrefix="uc1" %> 

<table class="control" width="500px">
    <tr>
        <td align="center" class="control-header">
            <div style="float: left;">
                <asp:Image ID="imgNewsType" runat="server" />
            </div>
            <div style="float: none;">
                <asp:Label ID="lblCaption" runat="server" />
                <asp:HyperLink ForeColor="white" ID="hlCaption" runat="server" NavigateUrl="~//NewsTape//ViewNews.aspx?id=" />
            </div>
        </td>
    </tr>
    <tr>
        <td align="left" >
            <div id="newsText" runat="server" 
                style="border: solid 1px; border-color: Gray; padding: 1px; overflow: auto" />
            
            <asp:GridView runat="server" ID="gridAttachments" Width="100%"
                    AutoGenerateColumns="false" 
                    EnableTheming="false"
                    ShowHeader="false"
                    GridLines="None"
                    HeaderStyle="height: 0px"
                    CellSpacing="0"
                    CellPadding="0"
                    AllowPaging="false" AllowSorting="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <uc1:NewsAttachFile runat="server" ID="attachFile" 
                                    IsEditMode="false" 
                                    Attachment="<%# Container.DataItem %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="control-attach-row" />
                    <AlternatingRowStyle CssClass="control-attach-row-alternating" />
                    <HeaderStyle Height="0" />
                </asp:GridView>
                
            <div id="newsAttachments" runat="server" style="padding-top: 5px;"/>
        </td>
    </tr>
    <tr>
        <td>
            <div style="float: left;">
                <asp:Label ID="lblAuthorName" CssClass="control-label" runat="server" />
            </div>
            <div style="float: left;">    
                <asp:HyperLink runat="server" ID="hlAuthorName" CssClass="control-hyperlink" />
            </div>
            <div style="float: right;">
                <asp:Label ID="lblDateTime" CssClass="control-label" runat="server" />
            </div>
        </td>
    </tr>
    <tr runat="server" id="trLinks">
        <td>
            <div style="float: left;padding-left: 20%">
                <asp:LinkButton CssClass="control-hyperlink" ID="hlDiscussOnForum" runat="server" meta:resourcekey="hlDiscussOnForum" OnClick="hlDiscussOnForum_Click" />
            </div>                        
            <div style="float: left; padding-left: 6px;">
                <asp:LinkButton CssClass="control-hyperlink" ID="hlEditNews" PostBackUrl="~//NewsTape//AddNews.aspx?id=" runat="server" meta:resourcekey="hlEditNews" />
            </div>                        
            <div style="float: left; padding-left: 6px;">
                <asp:LinkButton CssClass="control-hyperlink" ID="hlSendToArchive" runat="server" meta:resourcekey="hlSendToArchive" OnClick="hlSendToArchive_Click" />
            </div>                        
            <div style="float: left; padding-left: 6px;">
                <asp:LinkButton CssClass="control-hyperlink" ID="hlDeleteNews" runat="server" meta:resourcekey="hlDeleteNews" OnClick="hlDeleteNews_Click" />
            </div>
        </td>
    </tr>
</table>
