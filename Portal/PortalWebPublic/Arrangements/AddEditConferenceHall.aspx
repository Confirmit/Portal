<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    CodeFile="AddEditConferenceHall.aspx.cs" Inherits="Arrangements_AddEditConferenceHall" UICulture="auto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <table style="width: 100%">
        <tr>
	        <td colspan="2" align="center">
	            <asp:Label ID="lTitle" CssClass="control-label" runat="server" Font-Bold="true" Font-Size="Large" Font-Underline="true" Height="40px" />
	        </td>
	    </tr>
	    <tr>
		    <td style="width: 20%" align="left">
			    <asp:Label ID="lOfficeName" CssClass="control-label" runat="server" meta:resourcekey="lOfficeName" />
			</td>    
		    <td style="width: 80%" align="left">
		        <asp:DropDownList ID="tbOffice" CssClass="control-dropdownlist" DataTextField="Name" runat="server" Width="207px" Font-Size="Small">
                </asp:DropDownList>
            </td>
	    </tr>
	    <tr>
		    <td style="width: 20%" align="left">
			    <asp:Label ID="lCHName" CssClass="control-label" runat="server" meta:resourcekey="lCHName" />
		    </td>
		    <td style="width: 80%" align="left">
			    <asp:TextBox ID="tbCHName" CssClass="control-textbox" runat="server" Width="200px" Font-Size="Small" /><br />
                &nbsp;<asp:RequiredFieldValidator ID="rfvCHName" CssClass="control-errorlabel" runat="server" ControlToValidate="tbCHName"
                    ErrorMessage="<%$ Resources:rfvCHName %>"></asp:RequiredFieldValidator></td>
	    </tr>
	    <tr>
		    <td style="width: 20%" align="left">
			    <asp:Label ID="lDescription" CssClass="control-label" runat="server" meta:resourcekey="lDescription" />
		    </td>
		    <td style="width: 80%" align="left">
			    <asp:TextBox ID="tbDescription" CssClass="control-textbox" runat="server" Width="80%" Height="120px" TextMode="MultiLine" Font-Size="Small"/>
		    </td>
	    </tr>
	    <tr>
		    <td class="Default" colspan="2" align="center">
		        <asp:Button ID="btnDelete" CssClass="control-button"  runat="server" OnClick="btnDelete_Click" meta:resourcekey="btnDelete" Width="160px" />
		        <asp:Button ID="btnApply" CssClass="control-button"  runat="server" OnClick="btnApply_Click" meta:resourcekey="btnApply" Width="100" />
		    </td>
	    </tr>
    </table>
</asp:Content>

