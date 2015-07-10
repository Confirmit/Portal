<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimeSelectorControl.ascx.cs" Inherits="Portal.Controls.RulesControls.TimeSelectorControl" %>

<%@ Register TagPrefix="uc" Namespace="MKB.TimePicker" Assembly="TimePicker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d25e9f59e49c4d2f" %>

<uc:TimeSelector ID="TimeSelector" runat="server" CssClass="time-selector" SelectedTimeFormat="TwentyFour" MinuteIncrement="1" AllowSecondEditing="True"/>
