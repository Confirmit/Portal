<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="True" Inherits="PersonSettingsPage" Codebehind="PersonSettingsPage.aspx.cs" %>

<%@ Register Src="~/Controls/Settings/PersonSettings.ascx" TagPrefix="uc1" TagName="PersonSettingsControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <div class="sectionCaption">
        <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitle" />
    </div>
    <br />
    <center>
        <div class="control" style="width: 600px" >
            <div class="control-body" align="left">
            <center>
            <div class="control-line-between"></div>
                    <uc1:PersonSettingsControl ID="personSettings" runat="server" />
            </center>        
            <div style="white-space: nowrap; width: 100%; text-align: center">
                <asp:Button ID="btnApply" CssClass="control-button" runat="server" 
                    meta:resourcekey="btnApply" Width="100" TabIndex="0" onclick="btnApply_Click" />
                <asp:Button ID="btnCancel" CssClass="control-button" runat="server" 
                    meta:resourcekey="btnCancel" Width="100" CausesValidation="false" 
                    onclick="btnCancel_Click" />
            </div>
            </div>
        </div>
    </center>
    
</asp:Content>
