<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Main.master"
    CodeFile="AddEditArrangement.aspx.cs" Inherits="Arrangements_AddEditArrangement" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContextMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <table style="width: 100%">
        <tr>
	        <td colspan="2" align="center">
	            <asp:Label ID="lTitle" CssClass="control-label" runat="server" Font-Bold="true" Font-Size="Large" Font-Underline="true" Height="40px" />
	        </td>
	    </tr>
	    <tr>
		    <td style="width: 20%" align="left">
			    <asp:Label ID="lOfficeName" CssClass="control-label" runat="server" meta:resourcekey="lOfficeName" />
			</td>
		    <td style="width: 80%" align="left">
                <asp:TextBox ID="tbOffice" CssClass="control-textbox" runat="server" Width="200px"></asp:TextBox>
            </td>
	    </tr>
        <tr>
            <td style="width: 20%" align="left">
                <asp:Label ID="lConferenceHallName" CssClass="control-label" runat="server" meta:resourcekey="lConferenceHallName" />
            </td>
            <td style="width: 80%" align="left">
                <asp:DropDownList ID="ddlConferenceHalls" CssClass="control-dropdownlist" DataTextField="Name" DataValueField="ConferenceHallID" runat="server" Width="207px" AutoPostBack="True" Font-Size="Small">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="control-errorlabel" runat="server" ControlToValidate="ddlConferenceHalls"
                    ErrorMessage="<%$ Resources:rfvConferenceHallError %>"></asp:RequiredFieldValidator>
            </td>
        </tr>
	    <tr>
		    <td style="width: 20%" align="left">
			    <asp:Label ID="lArrName" runat="server" CssClass="control-label" meta:resourcekey="lArrName" />
		    </td>
		    <td style="width: 80%" align="left">
			    <asp:TextBox ID="tbArrName" CssClass="control-textbox" runat="server" Width="80%" TextMode="MultiLine" Font-Size="Small" /><br />
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbArrName"
                    ErrorMessage="<%$ Resources:rfvArrName %>"></asp:RequiredFieldValidator></td>
	    </tr>
	    <tr>
		    <td style="width: 20%" align="left">
			    <asp:Label ID="lDescription" CssClass="control-label" runat="server" meta:resourcekey="lDescription" />
		    </td>
		    <td style="width: 80%" align="left">
			    <asp:TextBox ID="tbDescription" CssClass="control-textbox" runat="server" Width="80%" Height="60px" TextMode="MultiLine" Font-Size="Small"/>
		    </td>
	    </tr>
	    <tr>
            <td align="left">
                <asp:Label ID="lDate" CssClass="control-label" runat="server" meta:resourcekey="lDate" />
            </td>
            <td> 
                <asp:Calendar ID="Calendar" runat="server" DayNameFormat="Short" Font-Name="Verdana;Arial"
                    Font-Size="14px" Height="80px" NextPrevFormat="ShortMonth" OtherMonthDayStyle-ForeColor="gray"
                    SelectMonthText="month" SelectWeekText="week" TodayDayStyle-Font-Bold="True"
                    Width="80%" align="left">
                    <SelectedDayStyle BackColor="#FFCC66" Font-Bold="True" />
                    <TodayDayStyle Font-Bold="True" />
                    <SelectorStyle BackColor="#99CCFF" Font-Size="9px" ForeColor="Navy" />
                    <OtherMonthDayStyle ForeColor="Gray" />
                    <NextPrevStyle Font-Size="10px" ForeColor="White" />
                    <DayHeaderStyle Font-Bold="True" />
                </asp:Calendar>
            </td>    
        </tr>
        <tr>
		    <td style="width: 20%" align="left">
			    <asp:Label ID="lTimeBegin" CssClass="control-label" runat="server" meta:resourcekey="lTimeBegin" />
		    </td>
		    <td style="width: 80%" align="left">
			    <asp:TextBox ID="tbTimeBegin" CssClass="control-textbox" runat="server" Width="200px" Font-Size="Small" />&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="control-errorlabel" runat="server" ControlToValidate="tbTimeBegin"
                    ErrorMessage="<%$ Resources:rfvTimeBegin %>">*</asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvTimeBegin" runat="server" ControlToValidate="tbTimeBegin"
                    ErrorMessage="<%$ Resources:cvTimeError %>" OnServerValidate="cvTimeBegin_ServerValidate" EnableClientScript="False"></asp:CustomValidator>
		    </td>
	    </tr>
	    <tr>
		    <td style="width: 20%" align="left">
			    <asp:Label ID="lTimeEnd" CssClass="control-label" runat="server" meta:resourcekey="lTimeEnd" />
		    </td>
		    <td style="width: 80%" align="left">
			    <asp:TextBox ID="tbTimeEnd" CssClass="control-textbox" runat="server" Width="200px" Font-Size="Small" />&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="control-errorlabel" runat="server" ControlToValidate="tbTimeEnd"
                    ErrorMessage="<%$ Resources:rfvTimeEnd %>">*</asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvTimeEnd" runat="server" ControlToValidate="tbTimeEnd"
                    ErrorMessage="<%$ Resources:cvTimeError %>" OnServerValidate="cvTimeEnd_ServerValidate"></asp:CustomValidator>
		    </td>
	    </tr>
	    <tr>
		    <td style="width: 20%" align="left">
			    <asp:Label ID="lListOfGuests" CssClass="control-label" runat="server" meta:resourcekey="lListOfGuests" />
		    </td>
		    <td style="width: 80%" align="left">
			    <asp:TextBox ID="tbListOfGuests" CssClass="control-textbox" runat="server" Width="80%" Height="60px" TextMode="MultiLine" Font-Size="Small" />
		    </td>
	    </tr>
	    <tr>
		    <td style="width: 20%" align="left">
			    <asp:Label ID="lEquipment" CssClass="control-label" runat="server" meta:resourcekey="lEquipment" />
		    </td>
		    <td style="width: 80%" align="left">
			    <asp:TextBox ID="tbEquipment" CssClass="control-textbox" runat="server" Width="80%" Height="60px" TextMode="MultiLine" Font-Size="Small" />
		    </td>
	    </tr>
	    <tr>
	        <td colspan="2" align="center">
	            <asp:CustomValidator ID="cvCheckAdding" runat="server" ErrorMessage="<%$ Resources:cvCheckAdding %>"
                    OnServerValidate="cvCheckAdding_ServerValidate"></asp:CustomValidator>
                <asp:CustomValidator ID="cvCheckOffice" runat="server" ErrorMessage="<%$ Resources:cvCheckOffice %>"
                    OnServerValidate="cvCheckOffice_ServerValidate"></asp:CustomValidator>
            </td>
	    </tr>
	    <tr>
		    <td class="Default" colspan="2" align="center">
		        <asp:Button ID="btnDelete" CssClass="control-button" runat="server" OnClick="btnDelete_Click" meta:resourcekey="btnDelete" Width="138px" />
		        <asp:Button ID="btnApply" CssClass="control-button" runat="server" OnClick="btnApply_Click" meta:resourcekey="btnApply" Width="100" />
		    </td>
	    </tr>
    </table>
</asp:Content>
