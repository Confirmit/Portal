<%@ Control Language="C#" AutoEventWireup="true" Inherits="UsersGrid" CodeBehind="UsersGrid.ascx.cs" %>

<div align="center">
    <div style="width: 100%" class="control-border">
        <asp:UpdatePanel ID="upPanel" runat="server">
            <ContentTemplate>
                <asp:GridView ID="GridViewUsers" runat="server"
                    DataKeyNames="ID" AllowCustomPaging="True"
                    AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" ShowFooter="True"
                    EnableTheming="false" GridLines="None" Width="100%"
                    OnSelectedIndexChanged="GridViewUser_SelectedIndexChanged"
                    OnPageIndexChanging="GridViewUser_PageIndexChanging"
                    OnDataBound="GridViewUser_DataBound"
                    OnRowDeleting="GridViewUsers_OnRowDeleting"
                    OnRowDataBound="OnRowDataBound"
                    OnSorting="GridViewUsers_OnSorting"
                    CssClass="gridview">
                    <HeaderStyle CssClass="gridview-headerrow" HorizontalAlign="Left" />
                    <RowStyle CssClass="gridview-row" />
                    <SelectedRowStyle CssClass="gridview-selectedrow" />
                    <AlternatingRowStyle CssClass="gridview-alternatingrow" />
                    <PagerStyle CssClass="gridview-pagerrow" />

                    <EmptyDataTemplate>
                        <%# (string) GetLocalResourceObject( "NoUsers" ) %>
                    </EmptyDataTemplate>

                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                    <PagerTemplate>
                        <div style="width: 100%">
                            <table width="100%">
                                <tr>
                                    <td align="center" style="width: 70%">
                                        <asp:LinkButton ID="lbntFirst" runat="server"
                                            CommandArgument="First" CommandName="Page"
                                            CssClass="control-hyperlink"
                                            Text="&lt;&lt;" />

                                        <asp:LinkButton ID="lbtnPrev" runat="server"
                                            CommandArgument="Prev" CommandName="Page"
                                            CssClass="control-hyperlink"
                                            Text="&lt;" />

                                        <asp:Literal ID="lblPage" runat="server" meta:resourcekey="PageIndex" />
                                        <asp:DropDownList ID="ddlPage" runat="server"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="OnPageIndexChanged"
                                            Width="100"
                                            CssClass="control-dropdownlist" />

                                        <asp:Literal ID="lblFrom" runat="server" meta:resourcekey="PageFrom" />
                                        <asp:Literal ID="lblPageCount" runat="server" />

                                        <asp:LinkButton ID="lbtnNext" runat="server"
                                            CommandArgument="Next" CommandName="Page"
                                            Text="&gt;"
                                            CssClass="control-hyperlink" />

                                        <asp:LinkButton ID="lbtnLast" runat="server"
                                            CommandArgument="Last" CommandName="Page"
                                            Text="&gt;&gt;"
                                            CssClass="control-hyperlink" />
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="lblPageSize" runat="server" meta:resourcekey="PageSize" />

                                        <asp:DropDownList ID="ddlPageSize" runat="server"
                                            AutoPostBack="true"
                                            CssClass="control-dropdownlist"
                                            OnSelectedIndexChanged="OnPageSizeChanged"
                                            Width="100">
                                            <asp:ListItem Value="5" />
                                            <asp:ListItem Selected="True" Value="10" />
                                            <asp:ListItem Value="20" />
                                            <asp:ListItem Value="50" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>

                    <Columns>
                        <asp:BoundField DataField="FirstName" HeaderText="FirstName:" SortExpression="FirstName" meta:resourcekey="FirstNamefield">
                            <ItemStyle Width="30%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MiddleName" HeaderText="MiddleName:" SortExpression="MiddleName" meta:resourcekey="MiddleName">
                            <ItemStyle Width="30%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LastName" HeaderText="LastName:" SortExpression="LastName" meta:resourcekey="LastNamefield">
                            <ItemStyle Width="30%" />
                        </asp:BoundField>
                        <asp:CommandField ButtonType="Image" meta:resourcekey="Select"
                            SelectImageUrl="~/Images/announce/add_blue.gif"
                            ShowSelectButton="True">
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                        </asp:CommandField>

                        <asp:CommandField ButtonType="Image"
                            DeleteImageUrl="~/Images/attachments/delete.gif"
                            ShowDeleteButton="true">
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>

