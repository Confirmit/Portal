<%@ Control Language="C#" AutoEventWireup="True" Inherits="UserInfoView" CodeBehind="UserInfoView.ascx.cs" %>

<%@ Register Assembly="Controls" Namespace="Controls.HotGridView" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/Users/UserStatistics.ascx" TagPrefix="uc1" TagName="UserStatistics" %>

<div style="width: 50%;">
    <div style="text-align: center" class="sectionCaption">
        <asp:Label ID="lblUserInfo" runat="server" meta:resourcekey="lblUserInfo" />
    </div>
    <div style="text-align: center" class="sectionCaption">
        <asp:Label ID="FullUserName" runat="server" />
    </div>

    <div class="control-line-between"></div>
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <table border="0px" width="100%" align="center" cellspacing="0" cellpadding="4" class="control">
                <tr valign="top">
                    <td align="left" width="50%">
                        

                        <div>
                            <cc1:GridView ID="gvContact" runat="server"
                                Width="100%" AutoGenerateColumns="False"
                                DataKeyNames="ID"
                                CssClass="gridview" GridLines="None"
                                CellPadding="3" UseCustomPager="true"
                                AllowPaging="True" AllowSorting="True" PageSize="5"
                                RightArrowDisableImg="~/Images/GridView/pgarrow_right_disabled.gif"
                                RightArrowEnableImg="~/Images/GridView/pgarrow_right_enabled.gif"
                                LeftArrowDisableImg="~/Images/GridView/pgarrow_left_disabled.gif"
                                LeftArrowEnableImg="~/Images/GridView/pgarrow_left_enabled.gif">
                                <HeaderStyle CssClass="gridview-headerrow" HorizontalAlign="Center" />
                                <PagerSettings NextPageText="Next" PreviousPageText="Prev" />

                                <EmptyDataTemplate>
                                    <center>
								<%# (string)GetLocalResourceObject("NoContact")%>
							</center>
                                </EmptyDataTemplate>

                                <Columns>
                                    <asp:TemplateField meta:resourcekey="dataHeader">
                                        <ItemTemplate>
                                            <asp:Panel CssClass="popup-menu" ID="PopupMenu" runat="server">
                                                <div style="border: 1px outset white; padding: 2px;">
                                                    <div>
                                                        <asp:LinkButton ID="linkEdit" runat="server"
                                                            CommandName="Edit" Text="Edit"
                                                            CssClass="control-hyperlink-big" />
                                                    </div>
                                                    <div>
                                                        <asp:LinkButton ID="linkDelete" runat="server"
                                                            CommandName="Delete" Text="Delete"
                                                            CommandArgument="true"
                                                            CssClass="control-hyperlink-big" />
                                                    </div>
                                                </div>
                                            </asp:Panel>

                                            <asp:Panel ID="dataPanel" runat="server">
                                                <asp:Label runat="server" ID="lContact" Mode="Encode" Width="100%"
                                                    Text='<%# AllUserInfoPrint((string)Eval("StringField"), (int)Eval("AttributeID")) %>' />
                                            </asp:Panel>

                                            <ajaxToolkit:HoverMenuExtender ID="hoverMenuView" runat="Server"
                                                HoverCssClass="popup-hover"
                                                PopupControlID="PopupMenu"
                                                PopupPosition="Left"
                                                TargetControlID="dataPanel"
                                                PopDelay="25" />
                                        </ItemTemplate>

                                        <EditItemTemplate>
                                            <ajaxToolkit:HoverMenuExtender ID="hoverMenuEdit" runat="Server"
                                                TargetControlID="dataPanelEdit"
                                                PopupControlID="PopupMenuEdit"
                                                HoverCssClass="popup-hover"
                                                PopupPosition="Right" />

                                            <asp:Panel ID="PopupMenuEdit" runat="server" CssClass="popup-menu" Width="80">
                                                <div style="border: 1px outset white">
                                                    <asp:LinkButton ID="linkUpdate" runat="server"
                                                        CausesValidation="True" CommandName="Update"
                                                        Text="Update" CssClass="control-hyperlink-big" />
                                                    <br />
                                                    <asp:LinkButton ID="linkCancel" runat="server"
                                                        CausesValidation="False" CommandName="Cancel"
                                                        Text="Cancel" CssClass="control-hyperlink-big" />
                                                </div>
                                            </asp:Panel>

                                            <asp:Panel ID="dataPanelEdit" runat="server">
                                                <asp:Label runat="server" ID="lUserKeyWord" Mode="Encode" Width="30%"
                                                    CssClass="control-label" Visible='<%# printIsAllTypeAtr() %>'
                                                    Text='<%# UserKeyWordPrint((int)Eval("AttributeID")) %>' />
                                                <asp:TextBox ID="tbUserData" runat="server" CssClass="control-textbox" Width="40%"
                                                    Text='<%# Eval("StringField") %>' />
                                            </asp:Panel>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                                <RowStyle CssClass="gridview-row" />
                                <EditRowStyle BackColor="#EEEEEE" Height="20" />
                                <SelectedRowStyle CssClass="gridview-selectedrow" />
                                <PagerStyle CssClass="gridview-pagerrow" />
                                <AlternatingRowStyle CssClass="gridview-alternatingrow" />
                                <FooterStyle CssClass="gridview-footer" />
                            </cc1:GridView>
                        </div>

                        <div>
                            <div style="float: left;">
                                <asp:Label ID="lSelectType" runat="server" CssClass="control-label" meta:resourcekey="lSelectType" Width="100%" />
                                <div style="float: left; width: 157px;">
                                    <asp:DropDownList ID="ddlTypeSelect" runat="server"
                                        AutoPostBack="true" CssClass="control-dropdownlist" Width="100%"
                                        OnSelectedIndexChanged="OnFilterSelectedIndexChanged" />
                                </div>
                            </div>
                        </div>

                        <div id="dvAddInfo" runat="server">
                            <div style="float: left;">
                                <asp:Label ID="lblValue" runat="server" CssClass="control-label" meta:resourcekey="lblValue" Width="100%" />
                                <div style="float: left;">
                                    <asp:TextBox ID="tbContInfo" runat="server"
                                        Width="100%" CssClass="control-textbox-required"
                                        EnableTheming="false" />
                                </div>
                            </div>

                            <div align="center">
                                <asp:Button ID="btnAddContact" runat="server"
                                    meta:resourcekey="btnAddContact"
                                    OnClick="btnAddContact_Click" Width="150"
                                    CssClass="control-button" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div align="center" class="control-line-of-controls">
        <asp:HyperLink ID="hlEdit" runat="server"
            CssClass="control-hyperlink" meta:resourcekey="hlEdit"
            NavigateUrl="~/Admin/AdminUsersPage.aspx" />
    </div>
</div>

<div style="width: 50%;">
    <uc1:UserStatistics ID="UserStatisticsControl" runat="server" />
</div>

