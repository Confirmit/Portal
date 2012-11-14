<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Grid.ascx.cs" Inherits="Grid" %>
<%@ Register Src="~/Controls/UpdateProgress.ascx" TagName="MyUpdateProgress" TagPrefix="web" %>
<%@ Register Src="~/Controls/ShadowLine.ascx" TagName="ShadowLine" TagPrefix="web" %>
<asp:ScriptManagerProxy ID="scriptManagerProxy" runat="server" />
<asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
    <ContentTemplate>
        <asp:PlaceHolder ID="plhGrid" runat="server">
            <asp:UpdatePanel ID="upHiddenTotalSelected" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <input id="totalSelected" runat="server" type="hidden" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="margin-bottom: 10px;">
                <asp:PlaceHolder ID="phPager" runat="server">
                    <tr>
                        <td style="padding-top: 3px; padding-bottom: 5px;">
                            <web:ShadowLine ID="ucShadowLine1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table align="center" id="tblControl" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td valign="middle" style="padding-right: 10px;">
                                        <asp:Localize ID="locTotalRecs" runat="server" Text="Всего записей" meta:resourcekey="locTotalRecs" />:</td>
                                    <td valign="middle">
                                        <asp:Label ID="lbTotalRecords" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" style="width: 20px;">
                                        <img id="Img1" src="~/Images/razd.gif" runat="server" alt="" />
                                    </td>
                                    <asp:PlaceHolder ID="plhTotalCount" runat="server" Visible="false">
                                        <td valign="middle" style="padding-right: 10px;">
                                            <asp:Localize ID="locTotalSelected" runat="server" Text="Всего выбрано" meta:resourcekey="locTotalSelected" />:</td>
                                        <td valign="middle" style="padding-right: 10px;">
                                            <asp:UpdatePanel ID="upTotalSelected" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTotalSelected" runat="server" EnableViewState="false" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td valign="middle">
                                            <asp:LinkButton ID="lbClearSelection" runat="server" OnClick="lbClearSelection_Click"
                                                Text="Сбросить" CausesValidation="false" meta:resourcekey="lbClearSelection" />
                                        </td>
                                        <td valign="middle" align="center" style="width: 20px;">
                                            <img src="~/images/razd.gif" runat="server" alt="" />
                                        </td>
                                    </asp:PlaceHolder>
                                    <td valign="middle" style="padding-right: 14px;">
                                        <asp:Localize ID="locRecordsOnPage" runat="server" Text="Записей на странице" meta:resourcekey="locRecordsOnPage" />:
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" CssClass="input"
                                            CausesValidation="false" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                            <asp:ListItem Value="5" />
                                            <asp:ListItem Value="10" Selected="True" />
                                            <asp:ListItem Value="25" />
                                            <asp:ListItem Value="50" />
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" align="center" style="width: 20px;">
                                        <img id="Img2" src="~/Images/razd.gif" runat="server" alt="" />
                                    </td>
                                    <td valign="middle" style="padding-right: 10px;">
                                        <asp:Localize ID="locPages" runat="server" Text="Страницы" meta:resourcekey="locPages" />:</td>
                                    <td class="pager">
                                        <asp:LinkButton ID="lbPrev" runat="server" CommandName="pager" OnCommand="PagerCommand"
                                            Text="Пред." meta:resourcekey="lbPrev" CausesValidation="false"></asp:LinkButton>
                                        <asp:LinkButton ID="lbFirst" runat="server" CommandName="pager" OnCommand="PagerCommand"
                                            CausesValidation="false"></asp:LinkButton>
                                        <asp:LinkButton ID="lbPrevWindow" runat="server" CommandName="pager" OnCommand="PagerCommand"
                                            CausesValidation="false">...</asp:LinkButton>
                                        <asp:Repeater ID="repPager" runat="server" OnItemCommand="repPager_ItemCommand" OnItemDataBound="repPager_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbPage" runat="server" CommandName="pager" CausesValidation="false"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:LinkButton ID="lbNextWindow" runat="server" CommandName="pager" OnCommand="PagerCommand"
                                            CausesValidation="false">...</asp:LinkButton>
                                        <asp:LinkButton ID="lbLast" runat="server" CommandName="pager" OnCommand="PagerCommand"
                                            CausesValidation="false"></asp:LinkButton>
                                        <asp:LinkButton ID="lbNext" runat="server" CommandName="pager" OnCommand="PagerCommand"
                                            CausesValidation="false" Text="След." meta:resourcekey="lbNext"></asp:LinkButton>
                                    </td>
                                    <td valign="middle" align="center" style="width: 20px;">
                                        <img id="Img4" src="~/Images/razd.gif" runat="server" alt="" />
                                    </td>
                                    <td>
                                        <web:MyUpdateProgress ID="updateProgress" runat="server" meta:resourcekey="updateProgress" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 5px; padding-bottom: 5px;">
                            <web:ShadowLine ID="ucShadowLine2" runat="server" />
                        </td>
                    </tr>
                </asp:PlaceHolder>
                <tr>
                    <td align="center">
                        <asp:GridView ID="innerGrid" runat="server" BorderWidth="0" GridLines="None" CssClass="grid"
                            AutoGenerateColumns="False" OnSorting="OnSorting" AllowSorting="True" PagerSettings-Visible="false"
                            OnRowCreated="OnRowCreated" OnRowDataBound="OnRowDataBound" OnRowCommand="OnRowCommand">
                            <HeaderStyle CssClass="header" HorizontalAlign="Center" VerticalAlign="middle" />
                            <RowStyle VerticalAlign="middle" />
                            <%-- CssStyle строки (row) устанавливается (дополняется) в коде в OnRowDataBound(), 
					т.к. он может быть изменен в процессе формирования строк--%>
                            <Columns>
                                <web:TemplateField>
                                    <headertemplate>
							<web:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
						</headertemplate>
                                    <headerstyle horizontalalign="Left" />
                                    <itemtemplate>
							<web:CheckBox ID="chkSelected" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelected_CheckedChanged"/>
						</itemtemplate>
                                    <itemstyle width="20px" horizontalalign="Left" />
                                </web:TemplateField>
                                <web:TemplateField>
                                    <itemtemplate>
							<asp:ImageButton ID="ibToggle" runat="server" 
								Width="16" Height="16" AlternateText=""
								CommandName="toggle" CausesValidation="false" />
						</itemtemplate>
                                    <itemstyle width="16px" verticalalign="Middle" horizontalalign="Left" />
                                </web:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <asp:PlaceHolder ID="phButtonContainer" runat="server">
                <%-- Имена кнопок специально усложнены, чтобы исключить их повторение на более высоком уровне --%>
                <asp:PlaceHolder ID="plhAddButton" runat="server" Visible="false">
                    <asp:Button ID="btnAdd_AutoGenerated" runat="server" Text="Добавить" CssClass="button"
                        CausesValidation="false" meta:resourcekey="btnAdd" OnClick="btnAdd_Click" />&nbsp;
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="plhDeleteButton" runat="server" Visible="false">
                    <asp:Button ID="btnDelete_AutoGenerated" runat="server" Text="Удалить выбранные"
                        CssClass="button" CausesValidation="false" meta:resourcekey="btnDelete" OnClick="btnDelete_Click" />
                    <web:GridSelectionController runat="server" ID="gscDelete_AutoGenerated" TargetControlID="btnDelete_AutoGenerated" />
                    &nbsp; </asp:PlaceHolder>
            </asp:PlaceHolder>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plhNothingFound" runat="server" Visible="false">
            <div class="bad_search">
                <%=Resources.NewsTape.EmptySearchResults%>
            </div>
        </asp:PlaceHolder>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="innerGrid" EventName="RowCommand" />
        <asp:AsyncPostBackTrigger ControlID="btnAdd_AutoGenerated" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnDelete_AutoGenerated" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="ddlPageSize" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="lbPrev" EventName="Command" />
        <asp:AsyncPostBackTrigger ControlID="lbFirst" EventName="Command" />
        <asp:AsyncPostBackTrigger ControlID="lbPrevWindow" EventName="Command" />
        <asp:AsyncPostBackTrigger ControlID="repPager" EventName="ItemCommand" />
        <asp:AsyncPostBackTrigger ControlID="lbNextWindow" EventName="Command" />
        <asp:AsyncPostBackTrigger ControlID="lbLast" EventName="Command" />
        <asp:AsyncPostBackTrigger ControlID="lbNext" EventName="Command" />
        <asp:AsyncPostBackTrigger ControlID="lbClearSelection" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
