<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupsManipulationPage.aspx.cs" Inherits="Portal.TestingEntitiesManipulation.GroupsManipulationPage"
    MasterPageFile="~/MasterPages/Main.master" %>

<%@ Register Src="~/TestingEntitiesManipulation/EntitiesManipulationControl.ascx" TagPrefix="uc" TagName="EntitiesManipulationControl" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div style="margin: 5px;">
        <uc:EntitiesManipulationControl ID="EntitiesManipulationControl" runat="server" />
    </div>
</asp:Content>

