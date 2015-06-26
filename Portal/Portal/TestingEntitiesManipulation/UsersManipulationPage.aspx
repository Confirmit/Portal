<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsersManipulationPage.aspx.cs" Inherits="Portal.TestingEntitiesManipulation.UsersManipulationPage" %>

<%@ Register Src="~/TestingEntitiesManipulation/EntitiesManipulationControl.ascx" TagPrefix="uc" TagName="EntitiesManipulationControl" %>

<div style="margin: 5px;">
    <uc:EntitiesManipulationControl ID="EntitiesManipulationControl" runat="server" />
</div>
