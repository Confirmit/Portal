<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" Inherits="Admin_AdminUserOperations" Codebehind="AdminUserOperations.aspx.cs" %>

<%@ Register TagName="ImageLinkButton" TagPrefix="uc1" Src="~/Controls/ImageLinkButton.ascx" %>
<%@ Register TagName="DescriptionExtender" TagPrefix="uc2" Src="~/Controls/AjaxControls/DescriptionExtender.ascx" %>

<asp:Content ID="cntntMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<div class="sectionCaption">
		<asp:Label ID="lblCaption" runat="server" meta:resourcekey="Caption" />
	</div>
	
	<table cellspacing="0">
	    <tr>
	        <td align="left">
                <uc1:ImageLinkButton ID="hlUsers" runat="server"
                    ImageUrl="~/Images/admin/users.gif"
                    ImageWidth="40"
                    LinkCssClass="control-hyperlink-big"
                    Href="~/Admin/AdminUsersPage.aspx" 
                    meta:resourcekey="Users" />
            </td>
            <td>
                <asp:Image ID="imgUsers" runat="server" 
                    ImageUrl="~/Images/admin/info.gif" 
                    Width="20" 
                    ToolTip="Description"
                    Style="cursor: help" />
                <uc2:DescriptionExtender ID="desciptorExtenderUsers" runat="server"
                    TargetControlID="imgUsers"
                    MoveHorizontal="20"
                    MoveVertical="-50"
                    meta:resourcekey="UsersDescription"  
                />
            </td>
       </tr>
       <tr>
            <td align="left">
                <uc1:ImageLinkButton ID="hlRoles" runat="server"
                    ImageUrl="~/Images/admin/keys.gif"
                    ImageWidth="40"
                    LinkCssClass="control-hyperlink-big"
                    Href="~/Admin/AdminRolesPage.aspx" 
                    meta:resourcekey="Roles" />
            </td>
            <td>
                <asp:Image ID="imgRoles" runat="server" 
                    ImageUrl="~/Images/admin/info.gif" 
                    Width="20" 
                    ToolTip="Description"
                    Style="cursor: help" />
                <uc2:DescriptionExtender ID="desciptorExtenderRoles" runat="server"
                    TargetControlID="imgRoles"
                    MoveHorizontal="30"
                    MoveVertical="-40" 
                    meta:resourcekey="RolesDescription" 
                />
           </td>
      </tr>
</table>
</asp:Content>

