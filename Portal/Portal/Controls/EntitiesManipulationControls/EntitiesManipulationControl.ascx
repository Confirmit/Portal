<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntitiesManipulationControl.ascx.cs" Inherits="Portal.Controls.EntitiesManipulationControls.EntitiesManipulationControl" %>

<%@ Register Src="~/Controls/EntitiesManipulationControls/EntitiesListControl.ascx" TagPrefix="uc" TagName="EntitiesListControl" %>

<table>
    <tr>
        <td style="vertical-align: auto; width: 380px;">
            <div id="EntitiesListIncluded">
                <uc:EntitiesListControl ID="EntitiesListIncludedControl" runat="server" />
            </div>
        </td>
        <td style="vertical-align: auto;">
            <div style="height: 200px;">
                <div style="margin-top: 40px; margin-bottom: 15px;">
                    <asp:Button ID="RemoveEntitiesButton" runat="server" Text=" >> " />
                </div>
                <div style="height: 100px;">
                    <asp:Button ID="AddEntitiesButton" runat="server" Text=" << " />
                </div>
            </div>
        </td>
        <td style="vertical-align: auto; width: 380px;">
            <div id="EntitiesListNotIncluded">
                <uc:EntitiesListControl ID="EntitiesListNotIncludedControl" runat="server" />
            </div>
        </td>
    </tr>
</table>