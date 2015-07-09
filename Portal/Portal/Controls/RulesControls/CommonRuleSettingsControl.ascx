<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommonRuleSettingsControl.ascx.cs" Inherits="Portal.Controls.RulesControls.CommonRuleSettingsControl" %>

<%@ Register TagPrefix="asp" Namespace="Controls.DatePicker" Assembly="Controls" %>
<%@ Register Src="~/Controls/RulesControls/TimeSelectorControl.ascx" TagPrefix="uc" TagName="TimeSelectorControl" %>
<script src="../../Scripts/commonRuleSettings.js" type="text/javascript"></script>

<style>
    #rules-property-panel {
        width: 300px;
    }

     #rules-property-panel table {
         width: 100%;
     }

    #rules-property-panel td {
        width: 50%;
    }
</style>

<div id="RuleConfigurationPanel" runat="server">
    <div id="rules-property-panel">
        <table>
            <tr>
                <td>
                    <asp:Label ID="RuleDiscriptionLabel" runat="server" Text="Rule Discription:" />

                </td>
                <td>
                    <asp:TextBox ID="RuleDiscriptionTextBox" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="BeginTimeLabel" runat="server" Text="Begin Time:" />

                </td>
                <td>
                    <asp:DatePicker ID="BeginTimeDatePicker" runat="server" />

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="EndTimeLabel" runat="server" Text="End Time:" />

                </td>
                <td>
                    <asp:DatePicker ID="EndTimeDatePicker" runat="server" />

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="ExpirationTimeLabel" runat="server" Text="Expiration Time:" />

                </td>
                <td>
                    <uc:TimeSelectorControl ID="ExpirationTimeSelectorControl" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LaunchTimeLabel" runat="server" Text="Launch Time:" />

                </td>
                <td>
                    <uc:TimeSelectorControl ID="LaunchTimeSelectorControl" runat="server" />
                </td>
            </tr>
        </table>

        <div>
            <asp:PlaceHolder ID="RuleConfigurationControlPlaceHolder" runat="server" />
        </div>
    </div>

    <div style="border: 1px solid black; margin: 5px;">
        <input type="checkbox" runat="server" ID="SelectAllDayCheckBox"/>
        <asp:Label ID="DaysOfWeekLabel" runat="server" Text="Days Of Week:" />
        <asp:CheckBoxList ID="DaysOfWeekCheckBoxList" runat="server" OnDataBound="DaysOfWeekCheckBoxList_OnDataBound"/>
    </div>
</div>