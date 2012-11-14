<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master"
	AutoEventWireup="true" CodeFile="FullNewsTape.aspx.cs" Inherits="NewsTape_FullNewsTape"
	 %>

<%@ Register TagPrefix="uc1" TagName="NewsTopMenu" Src="~/Controls/News/NewsTopMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FullNews" Src="~/Controls/News/FullNews.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
    <uc1:NewsTopMenu ID="NewsTopMenu1" runat="server" />
    <br />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <center>
        <asp:Label ID="lblOfficeNews" Font-Bold="true" meta:resourcekey="lblOfficeNews" runat="server" />
    </center>
        
    <asp:GridView EnableTheming="false"
        ID="gvFullNewsTape" runat="server" 
        AutoGenerateColumns="false" 
        DataSourceID="dsNews"
        AllowPaging="True" 
        BorderStyle="None"
        PageSize="3" 
        HorizontalAlign="Center" 
        BorderWidth="0px" 
        CellPadding="10" CellSpacing="0"
        ShowHeader="false"
    >
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <a id="newsID" runat="server"></a>
                    <uc1:FullNews ID="fullNews" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <asp:ObjectDataSource ID="dsNews" runat="server" SelectMethod="GetActualNews" TypeName="UlterSystems.PortalLib.NewsManager.NewsManager">
        <SelectParameters>
            <asp:Parameter Name="personID" />
        </SelectParameters>
    </asp:ObjectDataSource>
    
</asp:Content>
