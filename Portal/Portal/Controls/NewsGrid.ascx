<%@ Control Language="C#" AutoEventWireup="true" Inherits="NewsTape_NewsGrid" Codebehind="NewsGrid.ascx.cs" %>

<%@ Register TagName="Grid" TagPrefix="uc1" Src="~/Controls/Grid.ascx" %>

<uc1:Grid ID="innerGrid" runat="server" 
    OnRowDataBound="OnRowDataBound" 
    OnRequestDatasource="OnRequestDataSource" >
    <Columns>
        <web:TemplateField SortExpression="ExpireTime" meta:resourcekey="StatusHeader">
            <ItemTemplate>
                <center>
                    <asp:Image ID="imgNewsLocation" runat="Server" />
                </center>
            </ItemTemplate>
        </web:TemplateField>
        
        <web:TemplateField SortExpression="OfficeID" meta:resourcekey="OfficeHeader">
            <ItemTemplate>
                <center>
                    <asp:Image ID="imgNewsType" runat="Server" />
                </center>
            </ItemTemplate>
        </web:TemplateField>
        
        <web:TemplateField SortExpression="Caption" meta:resourcekey="CaptionHeader">
            <ItemTemplate>
                <asp:LinkButton ID="lbCaption" runat="Server" PostBackUrl="~/NewsTape/ViewNews.aspx?id=" />
            </ItemTemplate>
        </web:TemplateField>
        
        <web:TemplateField meta:resourcekey="TextHeader">
            <ItemTemplate>
                <div id="divText" runat="server">
                </div>
            </ItemTemplate>
        </web:TemplateField>
        
        <web:BoundField DataField="AuthorName" SortExpression="LastName" meta:resourcekey="AuthorHeader" />
        <web:BoundField DataField="CreateTime" SortExpression="CreateTime" meta:resourcekey="DateTimeHeader" />
    </Columns>
</uc1:Grid>