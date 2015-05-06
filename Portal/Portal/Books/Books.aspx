<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" Inherits="ConfirmIt.Portal.Books.BooksPage" MaintainScrollPositionOnPostback="true"
    meta:resourcekey="Page" Codebehind="Books.aspx.cs" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <asp:ScriptManagerProxy ID="scriptManagerProxy" runat="server" />
    <asp:Panel ID="pnlAdminOperations" runat="server" CssClass="control" BorderWidth="1"
        Width="90%">
        <div class="sectionCaption">
            <asp:Label ID="lblAdminOperationsCaption" runat="server" meta:resourcekey="AdminOperations" />
        </div>
        <div style="text-align: left">
            <ul>
                <li>
                    <asp:HyperLink ID="hlThemes" runat="server" NavigateUrl="~/Books/Themes.aspx" meta:resourcekey="Themes" />
                    <asp:Label ID="lblThemes" runat="server" meta:resourcekey="ThemesDescription" />
                </li>
            </ul>
        </div>
    </asp:Panel>
    <br />
    <asp:ObjectDataSource ID="dsThemes" runat="server" SelectMethod="GetAllBookThemes"
        TypeName="ConfirmIt.PortalLib.BAL.BookTheme" />
    <asp:ObjectDataSource ID="dsOffices" runat="server" SelectMethod="GetAllOffices"
        TypeName="ConfirmIt.PortalLib.BAL.Office" />
    <div style="width: 90%">
        <asp:Panel ID="pnlFilterHeader" runat="server">
            <div class="control-header">
                <div class="control-header-title">
                    <asp:Label ID="lblBookSearch" runat="server" meta:resourcekey="BookSearch" />
                </div>
                <div class="control-header-buttons">
                    <asp:ImageButton ID="imgFilterCollapsedStatus" runat="server" ImageUrl="~/Images/expand.jpg"
                        AlternateText="Show" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlFilter" runat="server" style="overflow: hidden" Height="0">
            <div class="control">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 30%" class="header">
                            <asp:Label ID="lblAuthors" runat="server" meta:resourcekey="Authors" />
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbxAuthors" runat="server" Width="98%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%" class="header">
                            <asp:Label ID="lblTitle" runat="server" meta:resourcekey="Title" />
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbxTitle" runat="server" Width="98%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%" class="header">
                            <asp:Label ID="lblAnnotation" runat="server" meta:resourcekey="Annotation" />
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="tbxAnnotation" runat="server" Width="98%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%" class="header">
                            <asp:Label ID="lblPublishingYear" runat="server" meta:resourcekey="PublishingYear" />
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="lblFromPublishingYear" runat="server" meta:resourcekey="From" />
                            <asp:TextBox ID="tbxFromPublishingYear" runat="server" MaxLength="4" Text="1800" />
                            <asp:CompareValidator ID="valFromPublishingYear" runat="server" ControlToValidate="tbxFromPublishingYear"
                                ErrorMessage="*" Display="Dynamic" Type="Integer" Operator="DataTypeCheck" ValidationGroup="Filter" />
                            <asp:Label ID="lblToPublishingYear" runat="server" meta:resourcekey="To" />
                            <asp:TextBox ID="tbxToPublishingYear" runat="server" MaxLength="4" />
                            <asp:CompareValidator ID="valToPublishingYear" runat="server" ControlToValidate="tbxToPublishingYear"
                                ErrorMessage="*" Display="Dynamic" Type="Integer" Operator="DataTypeCheck" ValidationGroup="Filter" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%" class="header">
                            <asp:Label ID="lblBookThemes" runat="server" meta:resourcekey="Themes" />
                        </td>
                        <td style="text-align: left">
                            <asp:Panel ID="pnlThemes" runat="server" Height="100" Width="100%" ScrollBars="Auto">
                                <asp:CheckBoxList ID="cblThemes" runat="server" DataSourceID="dsThemes" DataTextField="Name"
                                    DataValueField="ID" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%" class="header">
                            <asp:Label ID="lblLanguage" runat="server" meta:resourcekey="Language" />
                        </td>
                        <td style="text-align: left">
                            <asp:ObjectDataSource ID="dsLanguages" runat="server" SelectMethod="GetLanguages"
                                TypeName="ConfirmIt.PortalLib.BAL.Book" />
                            <asp:RadioButtonList ID="rblLanguages" runat="server" DataSourceID="dsLanguages"
                                RepeatDirection="Horizontal" AppendDataBoundItems="true">
                                <asp:ListItem Selected="True" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%" class="header">
                            <asp:Label ID="lblOffice" runat="server" meta:resourcekey="Office" />
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlOffices" runat="server" DataSourceID="dsOffices" Width="100%"
                                DataTextField="OfficeName" DataValueField="ID" AppendDataBoundItems="true">
                                <asp:ListItem Selected="True" Value="-1" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%" class="header">
                            <asp:Label ID="lblIsElectronic" runat="server" meta:resourcekey="Electronic" />
                        </td>
                        <td style="text-align: left">
                            <asp:CheckBox ID="cbIsElectronic" runat="server" Checked="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%" class="header">
                            <asp:Label ID="lblIsPaper" runat="server" meta:resourcekey="Paper" />
                        </td>
                        <td style="text-align: left">
                            <asp:CheckBox ID="cbIsPaper" runat="server" Checked="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right">
                            <asp:Button ID="btnSearch" runat="server" CssClass="control-button" ValidationGroup="Filter"
                                Width="100" OnClick="btnSearch_Click" meta:resourcekey="Search" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilter" runat="server" TargetControlID="pnlFilter"
            ExpandControlID="pnlFilterHeader" CollapseControlID="pnlFilterHeader" Collapsed="true"
            ImageControlID="imgFilterCollapsedStatus" ExpandedImage="~/Images/collapse.jpg"
            CollapsedImage="~/Images/expand.jpg" SuppressPostBack="true" />
    </div>
    <br />
    <asp:UpdatePanel ID="upBooks" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:ObjectDataSource ID="dsBooks" runat="server" EnablePaging="True" DeleteMethod="DeleteBook"
                SelectMethod="GetBooks" SelectCountMethod="GetBooksCount" TypeName="ConfirmIt.PortalLib.BAL.Book"
                SortParameterName="sortExpr" MaximumRowsParameterName="pageSize" StartRowIndexParameterName="rowIndex"
                OnSelecting="dsBooks_Selecting">
                <DeleteParameters>
                    <asp:Parameter Name="id" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="tbxAuthors" Name="authors" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="tbxTitle" Name="title" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="tbxAnnotation" Name="annotation" PropertyName="Text"
                        Type="String" />
                    <asp:Parameter Name="themes" Type="Object" />
                    <asp:Parameter Name="fromYear" Type="Int32" />
                    <asp:Parameter Name="toYear" Type="Int32" />
                    <asp:Parameter Name="language" Type="String" />
                    <asp:Parameter Name="officeID" Type="Int32" />
                    <asp:ControlParameter ControlID="cbIsElectronic" Name="isElectronic" PropertyName="Checked"
                        Type="Boolean" />
                    <asp:ControlParameter ControlID="cbIsPaper" Name="isPaper" PropertyName="Checked"
                        Type="Boolean" />
                    <asp:Parameter Name="sortExpr" Type="String" />
                    <asp:Parameter Name="rowIndex" Type="Int32" />
                    <asp:Parameter Name="pageSize" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gvBooks" runat="server" CssClass="control" AutoGenerateColumns="False"
                AllowSorting="True" AllowPaging="True" DataSourceID="dsBooks" PageSize="10" DataKeyNames="ID"
                ShowFooter="True" Width="90%" OnRowCreated="gvBooks_RowCreated" OnRowDeleted="gvBooks_RowDeleted"
                OnSelectedIndexChanged="gvBooks_SelectedIndexChanged" OnPageIndexChanging="gvBooks_PageIndexChanging"
                OnDataBound="gvBooks_DataBound">
                <EmptyDataTemplate>
                    <%# (string) GetLocalResourceObject( "NoBooks" ) %>
                </EmptyDataTemplate>
                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                <PagerTemplate>
                    <div style="width: 100%">
                        <table width="100%">
                            <tr>
                                <td align="center" style="width: 70%">
                                    <asp:LinkButton ID="lbntFirst" runat="server" Text="<<" CommandName="Page" CommandArgument="First" />
                                    <asp:LinkButton ID="lbtnPrev" runat="server" Text="<" CommandName="Page" CommandArgument="Prev" />
                                    <asp:Literal ID="lblPage" runat="server" meta:resourcekey="PageIndex" />
                                    <asp:DropDownList ID="ddlPage" runat="server" Width="100" AutoPostBack="true" OnSelectedIndexChanged="OnPageIndexChanged" />
                                    <asp:Literal ID="lblFrom" runat="server" meta:resourcekey="PageFrom" />
                                    <asp:Literal ID="lblPageCount" runat="server" />
                                    <asp:LinkButton ID="lbtnNext" runat="server" Text=">" CommandName="Page" CommandArgument="Next" />
                                    <asp:LinkButton ID="lbtnLast" runat="server" Text=">>" CommandName="Page" CommandArgument="Last" />
                                </td>
                                <td align="right">
                                    <asp:Literal ID="lblPageSize" runat="server" meta:resourcekey="PageSize" />
                                    <asp:DropDownList ID="ddlPageSize" runat="server" Width="100" AutoPostBack="true"
                                        OnSelectedIndexChanged="OnPageSizeChanged">
                                        <asp:ListItem Value="5" />
                                        <asp:ListItem Value="10" Selected="True" />
                                        <asp:ListItem Value="20" />
                                        <asp:ListItem Value="50" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </PagerTemplate>
                <Columns>
                    <asp:TemplateField SortExpression="Authors" meta:resourcekey="AuthorsField">
                        <ItemTemplate>
                            <asp:Literal ID="lblGVAuthors" runat="server" Mode="Encode" Text='<%# Eval( "Authors" ) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Title" meta:resourcekey="TitleField">
                        <ItemTemplate>
                            <asp:Literal ID="lblGVTitle" runat="server" Mode="Encode" Text='<%# Eval( "Title" ) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="PublishingYear" meta:resourcekey="PublishingYearField">
                        <ItemTemplate>
                            <asp:Literal ID="lblGVPublishingYear" runat="server" Mode="Encode" Text='<%# Eval( "PublishingYear" ) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField ButtonType="Image" CommandName="Download" ImageUrl="~/Images/View.gif"
                        meta:resourcekey="Download">
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:ButtonField>
                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/Edit.gif" ShowSelectButton="True"
                        meta:resourcekey="Select">
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:CommandField>
                    <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" ShowDeleteButton="True"
                        meta:resourcekey="Delete">
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:CommandField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:ObjectDataSource ID="dsSelectedBook" runat="server" InsertMethod="CreateBook"
                SelectMethod="GetBookByID" TypeName="ConfirmIt.PortalLib.BAL.Book" UpdateMethod="UpdateBook">
                <UpdateParameters>
                    <asp:Parameter Name="id" Type="Int32" />
                    <asp:Parameter Name="authors" Type="String" />
                    <asp:Parameter Name="title" Type="String" />
                    <asp:Parameter Name="publishingYear" Type="Int32" />
                    <asp:Parameter Name="annotation" Type="String" />
                    <asp:Parameter Name="language" Type="String" />
                    <asp:Parameter Name="officeID" Type="Int32" />
                    <asp:Parameter Name="downloadLink" Type="String" />
                    <asp:Parameter Name="isElectronic" Type="Boolean" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="gvBooks" Name="id" PropertyName="SelectedValue"
                        Type="Int32" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="authors" Type="String" />
                    <asp:Parameter Name="title" Type="String" />
                    <asp:Parameter Name="publishingYear" Type="Int32" />
                    <asp:Parameter Name="annotation" Type="String" />
                    <asp:Parameter Name="language" Type="String" />
                    <asp:Parameter Name="officeID" Type="Int32" />
                    <asp:Parameter Name="downloadLink" Type="String" />
                    <asp:Parameter Name="isElectronic" Type="Boolean" />
                    <asp:Parameter Name="themes" Type="Object" />
                </InsertParameters>
            </asp:ObjectDataSource>
            <div style="width: 90%">
                <asp:Panel ID="pnlDetailsHeader" runat="server">
                    <div class="control-header">
                        <div class="control-header-title">
                            <asp:Label ID="lblBookDetails" runat="server" meta:resourcekey="BookDetails" />
                        </div>
                        <div class="control-header-buttons">
                            <asp:ImageButton ID="imgDetailsCollapsedStatus" runat="server" ImageUrl="~/Images/expand.jpg"
                                AlternateText="Show" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlDetails" runat="server" style="overflow: hidden" Height="0">
                    <div class="control">
                        <asp:DetailsView ID="dvSelectedBook" runat="server" AutoGenerateRows="False" Height="50px"
                            DefaultMode="Insert" DataSourceID="dsSelectedBook" DataKeyNames="ID" Width="100%"
                            OnDataBound="dvSelectedBook_DataBound" OnItemCommand="dvSelectedBook_ItemCommand"
                            OnItemInserted="dvSelectedBook_ItemInserted" OnItemUpdated="dvSelectedBook_ItemUpdated"
                            OnItemCreated="dvSelectedBook_ItemCreated" OnItemInserting="dvSelectedBook_ItemInserting"
                            OnItemUpdating="dvSelectedBook_ItemUpdating">
                            <Fields>
                                <asp:TemplateField meta:resourcekey="AuthorsField">
                                    <ItemStyle CssClass="gridview-alternatingrow" />
                                    <ItemTemplate>
                                        <asp:Literal ID="lblDVAuthors" runat="server" Mode="Encode" Text='<%# Eval( "Authors" ) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbxDVAuthors" runat="server" Width="95%" Text='<%# Bind( "Authors" ) %>' />
                                        <asp:RequiredFieldValidator ID="valDVAuthors" runat="server" ControlToValidate="tbxDVAuthors"
                                            Display="Dynamic" ErrorMessage="*" ValidationGroup="Details" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="TitleField">
                                    <ItemStyle CssClass="gridview-alternatingrow" />
                                    <ItemTemplate>
                                        <asp:Literal ID="lblDVTitle" runat="server" Mode="Encode" Text='<%# Eval( "Title" ) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbxDVTitle" runat="server" Width="95%" Text='<%# Bind( "Title" ) %>' />
                                        <asp:RequiredFieldValidator ID="valTitle" runat="server" ControlToValidate="tbxDVTitle"
                                            Display="Dynamic" ErrorMessage="*" ValidationGroup="Details" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="PublishingYearField">
                                    <ItemStyle CssClass="gridview-alternatingrow" />
                                    <ItemTemplate>
                                        <asp:Literal ID="lblDVPublishingYear" runat="server" Mode="Encode" Text='<%# Eval( "PublishingYear" ) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbxDVPublishingYear" Width="95%" runat="server" Text='<%# Bind( "PublishingYear" ) %>' />
                                        <asp:RequiredFieldValidator ID="valDVPublishingYear" runat="server" ControlToValidate="tbxDVPublishingYear"
                                            Display="Dynamic" ErrorMessage="*" ValidationGroup="Details" />
                                        <asp:CompareValidator ID="valDVPublishingYearInt" runat="server" ControlToValidate="tbxDVPublishingYear"
                                            Operator="DataTypeCheck" Type="Integer" Display="Dynamic" ErrorMessage="*" ValidationGroup="Details" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="AnnotationField">
                                    <ItemStyle CssClass="gridview-alternatingrow" />
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlDVAnnotation" runat="server" Width="100%" Height="300" HorizontalAlign="Justify"
                                            ScrollBars="Auto">
                                            <asp:Literal ID="lblDVAnnotation" runat="server" Mode="Encode" Text='<%# Eval( "Annotation" ) %>' />
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbxDVAnnotation" runat="server" Width="95%" Height="150" TextMode="MultiLine"
                                            Text='<%# Bind( "Annotation" ) %>' />
                                        <asp:RequiredFieldValidator ID="valDVAnnotation" runat="server" ControlToValidate="tbxDVAnnotation"
                                            Display="Dynamic" ErrorMessage="*" ValidationGroup="Details" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="ThemesField">
                                    <ItemStyle CssClass="gridview-alternatingrow" />
                                    <ItemTemplate>
                                        <asp:Literal ID="lblDVThemes" runat="server" Mode="Encode" Text='<%# Eval( "ThemesText" ) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Panel ID="pnlDVThemes" runat="server" Height="100" ScrollBars="Auto" HorizontalAlign="Left"
                                                Width="95%">
                                                <asp:CheckBoxList ID="cblDVBookThemes" runat="server" DataSourceID="dsThemes" DataValueField="ID"
                                                    DataTextField="Name" />
                                            </asp:Panel>
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="LanguageField">
                                    <ItemStyle CssClass="gridview-alternatingrow" />
                                    <ItemTemplate>
                                        <asp:Literal ID="lblDVLanguage" runat="server" Mode="Encode" Text='<%# Eval( "Language" ) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlDVLanguage" runat="server" Width="95%" DataSourceID="dsLanguages" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="OfficeField">
                                    <ItemStyle CssClass="gridview-alternatingrow" />
                                    <ItemTemplate>
                                        <asp:Literal ID="lblDVOfficeName" runat="server" Mode="Encode" Text='<%# Eval( "OfficeName" ) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlDVOffices" runat="server" DataSourceID="dsOffices" DataTextField="OfficeName"
                                            DataValueField="ID" Width="95%" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="DownloadLinkField">
                                    <ItemStyle CssClass="gridview-alternatingrow" />
                                    <ItemTemplate>
                                        <asp:Literal ID="lblDVDownloadLink" runat="server" Mode="Encode" Text='<%# Eval( "DownloadLink" ) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbxDVDownloadLink" runat="server" Width="94%" Text='<%# Bind( "DownloadLink" ) %>' />
                                        <asp:RequiredFieldValidator ID="valDVDownloadLink" runat="server" ControlToValidate="tbxDVDownloadLink"
                                            Display="Dynamic" ErrorMessage="*" ValidationGroup="Details" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField meta:resourcekey="IsElectronicField">
                                    <ItemStyle CssClass="gridview-alternatingrow" />
                                    <ItemTemplate>
                                        <div style="text-align: left">
                                            <asp:CheckBox ID="cbDVIsElectronic" runat="server" Enabled="false" Checked='<%# Eval( "IsElectronic" ) %>' />
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="text-align: left">
                                            <asp:CheckBox ID="cbDVIsElectronicEdit" runat="server" Checked='<%# Bind( "IsElectronic" ) %>' />
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="True" ShowInsertButton="True" NewText="" EditText=""
                                    ValidationGroup="Details" meta:resourcekey="Command" />
                            </Fields>
                        </asp:DetailsView>
                        <asp:Panel ID="pnlErrorDescription" runat="server" Width="90%">
                            <asp:Label ID="lblErrorDescription" runat="server" ForeColor="Red" />
                        </asp:Panel>
                    </div>
                </asp:Panel>
                <ajaxToolkit:CollapsiblePanelExtender ID="cpeDetails" runat="server" TargetControlID="pnlDetails"
                    ExpandControlID="pnlDetailsHeader" CollapseControlID="pnlDetailsHeader" Collapsed="true"
                    ImageControlID="imgDetailsCollapsedStatus" ExpandedImage="~/Images/collapse.jpg"
                    CollapsedImage="~/Images/expand.jpg" SuppressPostBack="true" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
