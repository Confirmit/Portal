<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    CodeFile="AddEditConferenceHall.aspx.cs" Inherits="Arrangements_AddEditConferenceHall"
    UICulture="auto" %>

<%@ Register Src="~/controls/ActionsMenu/ActionsMenuCtl.ascx" TagName="ActionsMenu"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <div class="control" id="<%=ClientID%>" style="width: 510px;">
        <div align="center" style="width: 100%">
            <asp:Label ID="lTitle" CssClass="control-label" runat="server" Font-Bold="true" Font-Size="Large"
                Font-Underline="true" Height="40px" />
        </div>
        <div class="control-body">
            <div align="left" style="width: 100%" class="control-line-of-controls">
                <div style="float: left; padding-top: 4px; width: 110px;">
                    <asp:Label ID="lOfficeName" CssClass="control-label" runat="server" meta:resourcekey="lOfficeName" />
                </div>
                <div style="float: left;">
                    <asp:DropDownList ID="tbOffice" CssClass="control-dropdownlist" DataTextField="Name"
                        runat="server" Width="377" Font-Size="Small">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="control-line-between">
            </div>
            <div align="left" class="control-line-of-controls">
                <div style="float: left; padding-top: 4px; width: 110px;">
                    <asp:Label ID="lCHName" runat="server" CssClass="control-label" meta:resourcekey="lCHName" />
                </div>
                <div style="float: left;">
                    <asp:TextBox ID="tbCHName" CssClass="control-textbox-required" runat="server" Width="370"
                        Font-Size="Small" /><br />
                    &nbsp;<asp:RequiredFieldValidator ID="rfvCHName" CssClass="control-errorlabel" runat="server"
                        ControlToValidate="tbCHName" ErrorMessage="<%$ Resources:rfvCHName %>"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="control-line-between">
            </div>
            <div align="left" style="height: 90px;">
                <div style="float: left; padding-top: 4px; width: 110px;">
                    <asp:Label ID="lDescription" runat="server" CssClass="control-label" meta:resourcekey="lDescription" />
                </div>
                <div style="float: left;">
                    <asp:TextBox ID="tbDescription" CssClass="control-textbox" runat="server" Width="373"
                        Height="60" TextMode="MultiLine" Font-Size="Small" />
                </div>
            </div>
            <div class="control-line-between">
            </div>
            <div align="center"">
                <asp:Button ID="btnApply" CssClass="control-button" runat="server" OnClick="btnApply_Click"
                    meta:resourcekey="btnApply" Width="100" />
                <asp:Button ID="btnDelete" CssClass="control-button" runat="server" OnClick="btnDelete_Click"
                    meta:resourcekey="btnDelete" Width="160px" />
            </div>
        </div>
    </div>
</asp:Content>
