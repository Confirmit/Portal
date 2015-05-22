<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsTopMenu.ascx.cs"
	Inherits="NewsTape_NewsTopMenu" %>
	
<%@ Register TagName="ImageLinkButton" TagPrefix="uc2" Src="~/Controls/ImageLinkButton.ascx" %>

<div style="float: right;">
    <div style="float: left;">
        <uc2:ImageLinkButton ID="search" runat="server"
            ImageUrl="~/Images/announce/search.gif"
            Href="~/NewsTape/SearchNews.aspx" />
    </div>
    
    <div style="float: left;">
        <uc2:ImageLinkButton ID="archive" runat="server"
            ImageUrl="~/Images/announce/archive.gif"
            Href="~/NewsTape/Archive.aspx" />
    </div>
	
	<div style="float: left;">
        <uc2:ImageLinkButton ID="hlAddNews" runat="server"
            ImageUrl="~/Images/announce/add_blue.gif"
            Href="~/NewsTape/AddNews.aspx" />
   </div>
</div>        

