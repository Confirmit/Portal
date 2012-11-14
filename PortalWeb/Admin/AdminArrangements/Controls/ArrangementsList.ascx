<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ArrangementsList.ascx.cs"
    Inherits="Arrangements_Controls_ArrangementsList" %>
<asp:PlaceHolder ID="plh" runat="server" Visible="true">
    <asp:ScriptManagerProxy ID="scriptMgrProxy" runat="server" />
    <div style="width: 100%; text-align: left">
        <div>
            <asp:Label ID="Label1" CssClass="control-label" runat="server" />
        </div>
        <div>
            <asp:Label ID="lblOfficeName" CssClass="control-label" runat="server" />
        </div>
        <div>
            <asp:Button ID="btnUpdate" runat="server" meta:resourcekey="btnUpdate" CssClass="control-button" />
        </div>
    </div>
    <div style="overflow: auto; width: 100%;">
        <asp:UpdatePanel ID="upArrangementsList" runat="server">
            <ContentTemplate>
                <div style="overflow: auto; width: 100%;">
                    <asp:DataGrid ID="grdConferenceHallsList" runat="server" DataKeyField="ConferenceHallID"
                        AutoGenerateColumns="false" AllowCustomPaging="true" OnItemDataBound="OnCHBound"
                        EnableViewState="true" OnItemCreated="OnCHItemCreated">
                        <SelectedItemStyle Height="50px" HorizontalAlign="Right" Font-Bold="False" Font-Italic="False"
                            Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="#DEEEF3" />
                        <ItemStyle Width="5px" />
                        <Columns>
                            <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Small"
                                ItemStyle-HorizontalAlign="Center" meta:resourcekey="hConferenceHallName">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlConferenceHallName" ForeColor="Black" runat="server" Text='<%# (string)DataBinder.Eval(Container.DataItem, "Name") %>' />
                                </ItemTemplate>
                                
                                <HeaderStyle Width="28%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="09:00" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl1" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="09:15" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl2" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="09:30" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl3" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="09:45" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl4" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="10:00" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl5" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="10:15" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl6" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="10:30" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl7" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="10:45" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl8" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="11:00" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl9" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="11:15" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl10" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="11:30" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl11" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="11:45" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl12" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="12:00" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl13" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="12:15" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl14" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="12:30" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl15" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="12:45" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl16" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="13:00" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl17" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="13:15" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl18" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="13:30" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl19" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="13:45" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl20" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="14:00" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl21" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="14:15" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl22" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="14:30" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl23" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="14:45" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl24" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="15:00" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl25" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="15:15" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl26" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="15:30" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl27" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="15:45" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl28" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="16:00" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl29" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="16:15" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl30" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="16:30" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl31" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="16:45" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl32" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="17:00" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl33" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="17:15" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl34" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="17:30" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl35" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="17:45" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl36" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="18:00" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl37" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="18:15" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl38" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="18:30" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl39" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="18:45" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl40" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="19:00" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl41" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="19:15" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl42" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="19:30" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl43" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="19:45" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl44" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="20:00" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hl45" runat="server" ForeColor="Black" />
                                </ItemTemplate>
                                <HeaderStyle Width="2%" HorizontalAlign="Center" Font-Bold="true" />
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <div style="width: 100%; text-align: left">
            <asp:HyperLink ID="hlAddCH" CssClass="control-hyperlink" runat="server" meta:resourcekey="hlAddCH" />
            &nbsp&nbsp
            <asp:HyperLink ID="hlAddArr" CssClass="control-hyperlink" runat="server" meta:resourcekey="hlAddArr" />
        </div>
        <br />
    </div>
</asp:PlaceHolder>
