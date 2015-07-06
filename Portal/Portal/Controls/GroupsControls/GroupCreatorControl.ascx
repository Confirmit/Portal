<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupCreatorControl.ascx.cs"
    Inherits="Portal.Controls.GroupsControls.GroupCreatorControl" %>

<%@ Register Src="~/Controls/GroupsControls/GroupSettingsControl.ascx" TagPrefix="uc" TagName="GroupSettingsControl" %>

<uc:GroupSettingsControl ID="GroupSettingsControl" runat="server" />
<asp:Button ID="CreateGroupButton" runat="server" Text="Create Group" />


