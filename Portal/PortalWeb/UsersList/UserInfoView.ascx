<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserInfoView.ascx.cs" Inherits="UserInfoView" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls" TagPrefix="asp" %>
<%@ Register Assembly="Controls" Namespace="Controls.HotGridView" TagPrefix="cc1" %> 
                            
<div style="text-align: center" class="sectionCaption">
    <asp:Label ID="lblUserInfo" runat="server" meta:resourcekey="lblUserInfo" />
</div>

<table class="control" width="100%" border="0px" align="center">	
	<tr>
        <td width="40%">
            <asp:Silverlight ID="XamlUserPhotos" runat="server" 
                Source="~/ClientBin/UserPhotos.xap" 
                MinimumVersion="2.0.31005.0" 
                InitParameters="SLService=SLService/SLService.svc"
                Width="100%" Height="150" PluginBackground="#FFECE9D8"   
                Windowless="false"
            />                 
        </td>
        <td valign="top" width="60%">
            <div class="control-line-between"></div>
            <div class="control-line-of-controls" >
                <div style="float: left; font-weight: bold; width: 100px" >
                    <asp:Label ID="locFirstName" runat="server" 
                        meta:resourcekey="locFirstName" CssClass="control-label" />
                </div>
                <div style="float: left;" >
                    <asp:Label CssClass="control-label" ID="lblFirstName" runat="server" />
                </div>
           </div>
                
           <div class="control-line-of-controls" >
                <div style="float: left; font-weight: bold; width: 100px" >
                    <asp:Label ID="locMiddleName" runat="server" 
                        meta:resourcekey="locMiddleName" CssClass="control-label"
                    />
                </div>
                <div style="float: left;" >
                    <asp:Label CssClass="control-label" ID="lblMiddleName" runat="server" />
                </div>
           </div>
                
           <div class="control-line-of-controls" >
                <div style="float: left; font-weight: bold; width: 100px" >
                    <asp:Label ID="locLastName" runat="server" 
                        CssClass="control-label" meta:resourcekey="locLastName" />
                </div>
                <div style="float: left;" >
                    <asp:Label CssClass="control-label" ID="lblLastName" runat="server" />
                </div>
            </div>
                
           <div class="control-line-of-controls" >
                <div style="float: left; font-weight: bold; width: 100px" >
                    <asp:Label ID="locSex" runat="server"
                        CssClass="control-label" meta:resourcekey="locSex" />
                </div>
                <div style="float: left;" >
                    <asp:Label CssClass="control-label" ID="lblSex" runat="server" />
                </div>
           </div>
                
           <div class="control-line-of-controls" >
                <div style="float: left; font-weight: bold; width: 100px" >
                    <asp:Label ID="locBirthday" runat="server" 
                        CssClass="control-label" meta:resourcekey="locBirthday" />
                </div>
                <div style="float: left;" >
                    <asp:Label CssClass="control-label" ID="lblBirthday" runat="server" />
                </div>
           </div>
        </td>
    </tr>
    <tr>
        <td align="left" colspan="2">
            <div class="control-line-of-controls" >
                <div id="div_photoAttachments" style="float: left;" runat="server">
                    <input class="attachfile-hide-server" type="file" 
                        id="uploadFile" runat="server" size="40" />
                </div>
                
                <div style="float: left; padding-left: 4px" >
                    <asp:Button ID="btnAddPhoto" runat="server" 
                        meta:resourcekey="btnAddPhoto" 
                        OnClick="btnAddPhoto_Click" 
                        CssClass="control-button"
                        Width="100" />
               </div>
           </div>
        </td>
    </tr>
</table>


<div class="control-line-between"></div>
<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate> 
        <table border="0px" width="100%" align="center" cellspacing="0" cellpadding="4" class="control" > 
            <tr valign="top" >
                <td align="left" width="40%">
                    <div>
                         <div class="control-line-of-controls">
                            <div style="float: left; padding-top: 3px; width: 55px;">
                                <asp:Label ID="lSelectType" runat="server" CssClass="control-label" meta:resourcekey="lSelectType" Width="100%" />
                            </div>   
                            <div style="float: left; width: 157px;">
                                <asp:DropDownList ID="ddlTypeSelect" runat="server" 
                                    AutoPostBack="true" CssClass="control-dropdownlist" Width="100%" 
                                    onselectedindexchanged="OnFilterSelectedIndexChanged" />
                            </div>
                         </div>
                        
                        <div class="control-line-between"></div>
                        <div ID="dvAddInfo" runat="server">
                            <div class="control-line-of-controls">
                                <div style="float: left; width: 55px; padding-top: 3px;" >
                                    <asp:Label ID="lblValue" runat="server" CssClass="control-label" meta:resourcekey="lblValue" Width="100%" />
                                </div>
                                <div style="float: left; width: 150px">
                                    <asp:TextBox ID="tbContInfo" runat="server"
                                        Width="100%" CssClass="control-textbox-required" 
                                        EnableTheming="false" />
                                </div>
                            </div>
                            
                            <div class="control-line-between"></div>
                            <div align="center">
                                <asp:Button ID="btnAddContact" runat="server" 
                                    meta:resourcekey="btnAddContact" 
                                    onclick="btnAddContact_Click" Width="150"
                                    CssClass="control-button" />
                            </div>
                      </div>
                   </div>
                </td>
                <td width="60%">
                    <cc1:GridView ID="gvContact" runat="server" 
                        Width="100%" AutoGenerateColumns="False" 
                        DataKeyNames="ID"
                        CssClass="gridview" GridLines="None" 
                        CellPadding="3" UseCustomPager = "true"
                        AllowPaging="True" AllowSorting="True" PageSize="5"
                        RightArrowDisableImg="~/Images/GridView/pgarrow_right_disabled.gif"
                        RightArrowEnableImg="~/Images/GridView/pgarrow_right_enabled.gif"
                        LeftArrowDisableImg="~/Images/GridView/pgarrow_left_disabled.gif"
                        LeftArrowEnableImg="~/Images/GridView/pgarrow_left_enabled.gif"
                        >
                        <HeaderStyle CssClass="gridview-headerrow" HorizontalAlign="Center" />
                        <PagerSettings NextPageText="Next" PreviousPageText="Prev"/>

                        <EmptyDataTemplate>
                            <center>
			                    <%# (string)GetLocalResourceObject("NoContact")%>
			                </center>
		                 </EmptyDataTemplate>
                            
                         <Columns> 
                            <asp:TemplateField meta:resourcekey="dataHeader" >                                
                                <ItemTemplate>
                                    <asp:Panel CssClass="popup-menu" ID="PopupMenu" runat="server">
                                        <div style="border: 1px outset white; padding: 2px;">
                                            <div>
                                                <asp:LinkButton ID="linkEdit" runat="server" 
                                                    CommandName="Edit" Text="Edit" 
                                                    CssClass="control-hyperlink-big"/>
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
                                        <asp:Label runat="server" ID="lContact"  Mode="Encode"  Width = "100%"
                                                Text='<%# AllUserInfoPrint((string)Eval("StringField"), (int)Eval("AttributeID")) %>'/>
                                    </asp:Panel>
	                                    
                                    <ajaxToolkit:HoverMenuExtender ID="hoverMenuView" runat="Server"
                                        HoverCssClass="popup-hover"
                                        PopupControlID="PopupMenu"
                                        PopupPosition="Left"
                                        TargetControlID="dataPanel"
                                        PopDelay="25" 
                                        />
                                 </ItemTemplate>
                                    
                                 <EditItemTemplate>
		                            <ajaxToolkit:HoverMenuExtender ID="hoverMenuEdit" runat="Server"
                                        TargetControlID="dataPanelEdit"
                                        PopupControlID="PopupMenuEdit"
                                        HoverCssClass="popup-hover"
                                        PopupPosition="Right" />
                                       
                                    <asp:Panel ID="PopupMenuEdit" runat="server" CssClass="popup-menu" Width="80" >
                                        <div style="border:1px outset white">
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
                                        <asp:Label runat="server" ID="lUserKeyWord"  Mode="Encode"  Width = "30%"
                                            CssClass = "control-label" Visible = '<%# printIsAllTypeAtr() %>'
                                            Text='<%# UserKeyWordPrint((int)Eval("AttributeID")) %>'/>
                                        <asp:TextBox ID="tbUserData" runat="server" CssClass="control-textbox" Width = "40%"
                                            Text='<%# Eval("StringField") %>' />
		                            </asp:Panel>
		                        </EditItemTemplate>
		                    </asp:TemplateField>
                        </Columns>
                            
                            <RowStyle CssClass="gridview-row"/>
                            <EditRowStyle BackColor="#EEEEEE" Height="20"/>
                            <SelectedRowStyle CssClass="gridview-selectedrow"/>
                            <PagerStyle CssClass="gridview-pagerrow"/>
                            <AlternatingRowStyle CssClass="gridview-alternatingrow"/>
                            <FooterStyle CssClass="gridview-footer"  />
                        </cc1:GridView>
                    </td>
                </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

<div align="center" class="control-line-of-controls">
    <asp:HyperLink ID="hlEdit" runat="server"
        CssClass="control-hyperlink" meta:resourcekey="hlEdit" 
		NavigateUrl = "~/Admin/AdminUsersPage.aspx" 
    />
</div>    