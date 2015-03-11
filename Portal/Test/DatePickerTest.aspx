<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master"  AutoEventWireup="true" CodeBehind="DatePickerTest.aspx.cs" Inherits="Portal.Test.DatePickerTest" %>

<%@ Register Src="~/Controls/DatePicker/DatePicker.ascx" TagPrefix="uc" TagName="DatePicker1" %>
<%@ Register TagPrefix="usp" Namespace="Controls.DatePicker" Assembly="Controls" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    
    <div style="width: 100%">
    <%--<uc:DatePicker1 ID="datepicker1" Runat="Server"/>--%>
    <asp:Calendar ID="calendar1" runat="server" SelectionMode="Day" ShowGridLines="True"/>
   <%-- <usp:DatePicker ID="datepicker3" Runat="Server"/>--%>
        </div>
    
    <div style="width: 100%">
		<asp:Label ID="lblReportFromDate" runat="server" CssClass="control-label" Text="Ãåíåðèðîâàòü îò÷åò ñ "/>
		 <uc:DatePicker1 ID="tbReportFromDate" runat="server" />
		<asp:Label ID="lblReportToDate" runat="server" CssClass="control-label" Text=" ïî "/>
		<usp:DatePicker ID="tbReportToDate" runat="server" />
		<asp:Button ID="btnGenerateReport" runat="server" CssClass="control-button" Text="Îê" Width="40" />
	</div>
	
</asp:Content>
