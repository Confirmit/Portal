<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupEditorControl.ascx.cs" Inherits="Portal.Controls.GroupsControls.GroupEditorControl" %>

<%@ Register Src="~/Controls/GroupsControls/GroupSettingsControl.ascx" TagPrefix="uc" TagName="GroupSettingsControl" %>

<uc:GroupSettingsControl ID="GroupSettingsControl" runat="server"/>
<asp:Button ID="SaveGroupChangesButton" runat="server" Text="Save Group Changes"/>